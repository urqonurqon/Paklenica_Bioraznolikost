using Novena.UiUtility.Base;
using Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;
using Novena.DAL;
using Scripts.Utility;
using UnityEngine.UI;
using Novena.Helpers;
using Novena.Networking;
using TMPro;
using DG.Tweening;
using CGH;
using Doozy.Engine.UI;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using UnityEngine.EventSystems;

public class GameTemplate : UiController {

	public static Action<Theme> OnDetailsShow;
	public static Action OnGameRestarted;


	public static Action<bool> OnBackButtonClick;
	[SerializeField] private UIButton _backButton;



	[SerializeField] private InstructionsOverlay _introOverlay;
	[SerializeField] private UIButton _instructionsButton;
	[SerializeField] private UIButton _instructionsCloseButton;
	[SerializeField] private RectTransform _instructionsOverlay;
	[SerializeField] private RawImage _background;
	[SerializeField] public CanvasGroup _cover;

	[SerializeField] private CanvasGroup _languageCanvasGroup;
	[SerializeField] public List<CanvasGroup> _content = new List<CanvasGroup>();




	private List<int> _playedGameIndices = new List<int>();
	private int _gamePlayedInARow = 0;


	[Header("Drag Items references")]
	[SerializeField] private ScrollRect _scrollRect;
	[SerializeField] private ToggleGroup _toggleGroup;

	[SerializeField] private GameObject _photoPrefab;
	[SerializeField] private Transform _galleryParentTransform;

	[SerializeField] private GameObject _slotPrefab;
	[SerializeField] private Transform _slotParentTransform;

	[Header("Correct/Wrong")]
	[SerializeField] private RectTransform _popupRect;
	[SerializeField] private CanvasGroup _popupCanvasGroupAndRaycastBlocker;
	[SerializeField] private Texture _popupCorrectImage;
	[SerializeField] private Texture _popupFalseImage;
	[SerializeField] private RawImage _popupImageContainer;
	[SerializeField] private TMP_Text _popupText;
	[SerializeField] private UIButton _popupButton;
	[Header("Details")]
	[SerializeField] private CanvasGroup _details;
	[SerializeField] private RawImage _softMaskGradientTexture;
	[Header("GameOver")]
	[SerializeField] private UIButton _restartButton;
	[SerializeField] private UIButton _quitButton;
	[SerializeField] private RectTransform _gameFinished;


	private Sequence _popupSequence;
	private bool _isRightConnection;

	private Theme _theme;

	public List<GameObject> ItemSlots = new List<GameObject>();
	private List<GameObject> _items = new List<GameObject>();

	public bool IsInGame;

	public override void Awake()
	{
		base.Awake();
		IsInGame = false;
		_details.Hide();
		ItemSlot.OnPhotoDropped += ShowPhotoConnectedPopup;
		DetailsController.OnDetailsValueChanged += DetailsFade;
		DetailsController.OnDetailsValueChanged += GameFinished;
		_popupButton.OnClick.OnTrigger.Event.AddListener(() => PopupClose(true));
		_restartButton.OnClick.OnTrigger.Event.AddListener(() => RestartGame());
		_instructionsButton.OnClick.OnTrigger.Event.AddListener(OpenInstructions);
		_instructionsCloseButton.OnClick.OnTrigger.Event.AddListener(CloseInstructions);

		RecordPopupSequence();



		_backButton.OnClick.OnTrigger.Event.AddListener(() => {
			if (IsInGame)
			{
				OnBackButtonClick.Invoke(true);
			}
			else
			{
				OnBackButtonClick.Invoke(false);
			}

			_instructionsOverlay.anchoredPosition = new Vector2(1725, 0);


		});
		_quitButton.OnClick.OnTrigger.Event.AddListener(() => {
			OnBackButtonClick.Invoke(false);
			_instructionsOverlay.anchoredPosition = new Vector2(1725, 0);

		});


	}



	private void OpenInstructions()
	{
		_backButton.transform.parent.gameObject.GetComponent<RawImage>().DOFade(0, .5f);
		_backButton.gameObject.SetActive(false);
		_instructionsOverlay.DOAnchorPosX(197, .75f);
	}

	private void CloseInstructions()
	{
		_backButton.transform.parent.gameObject.GetComponent<RawImage>().DOFade(1, .5f);
		_backButton.gameObject.SetActive(true);
		_instructionsOverlay.DOAnchorPosX(1725, .75f);
	}


	public override void OnShowViewStart()
	{
		base.OnShowViewStart();

		_details.Hide();
		_playedGameIndices.Clear();
		_gamePlayedInARow = 0;
		_instructionsOverlay.anchoredPosition = new Vector2(1725, _instructionsOverlay.anchoredPosition.y);
		_cover.Show();
		IsInGame = false;
		CloseInstructions();
		_gameFinished.anchoredPosition = new Vector2(-1331, 0);
		_introOverlay.Show();
		RaycastBlocker.BlockRaycasts();
		_languageCanvasGroup.Fade(1, 1);
		_popupCanvasGroupAndRaycastBlocker.Hide();
		foreach (var cg in _content)
		{
			cg.DOFade(1, 1).From(0).OnComplete(RaycastBlocker.UnblockRaycasts);
		}
		RestartGame();

	}
	public override void OnHideViewFinished()
	{
		IsInGame = false;
		_instructionsOverlay.anchoredPosition = new Vector2(1725, _instructionsOverlay.anchoredPosition.y);
		_details.Hide();
		_popupCanvasGroupAndRaycastBlocker.Hide();
		_playedGameIndices.Clear();
		_gamePlayedInARow = 0;
		base.OnHideViewFinished();
	}






	private void DetailsFade(bool show)
	{
		_details.Fade(show ? 1 : 0, 1);

		if (show)
		{
			OnDetailsShow?.Invoke(_theme);
		}
		else
		{

			_popupCanvasGroupAndRaycastBlocker.Hide();
		}
	}



	private void RestartGame()
	{
		OnGameRestarted?.Invoke();
		GetComponent<ScoreController>().DidPlayerWin = false;
		DestroyItemsAndItemSlots();
		ChooseGame();
		_gameFinished.DOAnchorPosX(-1331, 1);
	}

	private void DestroyItemsAndItemSlots()
	{
		DestroyItems();
		DestroyItemSlots();
		Resources.UnloadUnusedAssets();
	}

	private void DestroyItemSlots()
	{
		foreach (var slot in ItemSlots)
		{
			Destroy(slot);
		}
		ItemSlots.Clear();
	}

	private void DestroyItems()
	{
		foreach (var item in _items)
		{
			Destroy(item);
		}
		_items.Clear();
	}

	private void RecordPopupSequence()
	{
		_popupSequence = DOTween.Sequence();
		_popupSequence.SetAutoKill(false);
		_popupSequence
			.Append(_popupRect.DOAnchorPos(new Vector2(195, -10), 1f).From(new Vector2(195, -1000)))
			.AppendInterval(2f)
			.Append(_popupRect.DOAnchorPos(new Vector2(195, 1000), 1f))
			.AppendCallback(() => PopupClose(false));
		_popupSequence.Pause();
	}

	public void PopupClose(bool doFadeBeforeRewind)
	{
		if (doFadeBeforeRewind)
		{
			_popupCanvasGroupAndRaycastBlocker.DOFade(0, 1).OnComplete(() => {
				_popupSequence.Rewind();
				_popupCanvasGroupAndRaycastBlocker.blocksRaycasts = false;
				_popupCanvasGroupAndRaycastBlocker.interactable = false;
			});
		}
		else
		{
			_popupCanvasGroupAndRaycastBlocker.Hide();
			_popupSequence.Rewind();
		}

		if (_isRightConnection)
		{
			DetailsFade(true);
		}
	}




	private void GameFinished(bool isDetailsOpening)
	{
		if (!isDetailsOpening)
		{
			if (GetComponent<ScoreController>().DidPlayerWin)
			{
				IsInGame = false;
				BrieflyShowGameScreen();
			}
		}
	}

	private void BrieflyShowGameScreen()
	{
		_popupCanvasGroupAndRaycastBlocker.Show();
		StartCoroutine(ShowEndGameOverlay());
	}


	private System.Collections.IEnumerator ShowEndGameOverlay()
	{
		yield return new WaitForSeconds(2f);
		_gameFinished.DOAnchorPosX(244.82f, 1f).OnComplete(() => _popupCanvasGroupAndRaycastBlocker.Hide());

	}

	private void ShowPhotoConnectedPopup(bool isRightConnection, Theme theme)
	{

		if (isRightConnection)
		{
			_popupImageContainer.texture = _popupCorrectImage;
			_popupText.text = Data.TranslatedContent.GetThemeByName("MISC").GetMediaByName("PopupTextCorrect").Text;
			_theme = theme;
		}
		else
		{
			_popupImageContainer.texture = _popupFalseImage;
			_popupText.text = Data.TranslatedContent.GetThemeByName("MISC").GetMediaByName("PopupTextFalse").Text;
		}
		_isRightConnection = isRightConnection;
		_popupCanvasGroupAndRaycastBlocker.Show();
		_popupSequence.Restart();
	}


	private void ChooseGame()
	{
		var theme = SetTheme();
		SetBackground(theme);
		LoadPhotos(theme);
		StartCoroutine(SetScrollRectDefault());

	}

	private IEnumerator SetScrollRectDefault()
	{
		yield return null;
		_scrollRect.verticalNormalizedPosition = 1;
	}

	private void SetBackground(Theme theme)
	{
		AssetsFileLoader.LoadTexture2D(Api.GetGuidePath() + theme.Image.Path, _background);

	}

	private void LoadPhotos(Theme theme)
	{

		var subThemes = theme.SubThemes;

		for (int i = 0; i < subThemes.Length; i++)
		{
			Theme speciesContent;

			var photoContainer = Instantiate(_photoPrefab, _galleryParentTransform);

			if (subThemes[i].Label != "Fake")
			{
				speciesContent = ConnectMapContentWithGameContent(subThemes[i]);
				photoContainer.GetComponentInChildren<ItemInfo>().MapTheme = speciesContent;
				var slotContainer = Instantiate(_slotPrefab, _slotParentTransform);
				slotContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(subThemes[i].PositionX, subThemes[i].PositionY);
				StartCoroutine(SetAnchors(slotContainer));
				AssetsFileLoader.LoadTexture2D(subThemes[i].GetMediaByName("ItemSlot").GetPhotos()[0].FullPath, slotContainer.GetComponentInChildren<RawImage>());
				slotContainer.GetComponentInChildren<RawImage>().SetNativeSize();
				photoContainer.GetComponentInChildren<DragDrop>().ItemSlot = slotContainer.GetComponent<ItemSlot>();
				ItemSlots.Add(slotContainer);
			}

			photoContainer.GetComponentInChildren<ItemInfo>().Theme = subThemes[i];

			photoContainer.GetComponentInChildren<ItemInfo>().SetName();
			AssetsFileLoader.LoadTexture2D(Api.GetGuidePath() + subThemes[i].Image.Path, photoContainer.GetComponentInChildren<RawImage>());

			photoContainer.GetComponent<Toggle>().group = _toggleGroup;
			photoContainer.GetComponentInChildren<DragDrop>().ScrollRect = _scrollRect;
			photoContainer.GetComponentInChildren<DragDrop>().DragParent = GetComponent<RectTransform>();
			_items.Add(photoContainer);

		}

		ShuffleList();
		GetComponent<ScoreController>().MaxScore = ItemSlots.Count;
		StartCoroutine(FixScrollRect());
	}

	private IEnumerator SetAnchors(GameObject slotContainer)
	{
		yield return new WaitForEndOfFrame();
		var tempPos = slotContainer.transform.position;
		slotContainer.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, .5f);
		slotContainer.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);
		slotContainer.transform.position = tempPos;
	}

	private IEnumerator FixScrollRect()
	{
		yield return new WaitForEndOfFrame();
		_scrollRect.normalizedPosition = new Vector2(0, 0);

	}

	private void ShuffleList()
	{
		System.Random random = new System.Random();
		int n = _items.Count;

		for (int i = 0; i < n; i++)
		{
			int r = i + (int)(random.NextDouble() * (n - i));

			_items[i].transform.SetSiblingIndex(r);
		}
	}


	private Theme ConnectMapContentWithGameContent(Theme subTheme)
	{
		var mapTag = Data.Theme.GetThemeTagByCategoryName("MAPS").Title;

		var mapThemes = Data.TranslatedContent.GetThemesByTag(mapTag);
		Theme mapTheme = null;
		foreach (var themeWithMapTag in mapThemes)
		{
			if (themeWithMapTag != Data.Theme)
				mapTheme = themeWithMapTag;
		}
		var pins = mapTheme.SubThemes;
		foreach (var pin in pins)
		{
			if (pin.Label == subTheme.Label)
				return pin;
		}
		return null;
	}

	private Theme SetTheme()
	{
		var currentGameTag = Data.Theme.GetThemeTagByCategoryName("GAMES");
		var gameList = Data.TranslatedContent.GetThemesByTag(currentGameTag.Title);
		for (int i = 0; i < gameList.Count; i++)
		{
			if (gameList[i] == Data.Theme)
				gameList.Remove(gameList[i]);

		}


		int gameNumber;
		do
		{
			if (_gamePlayedInARow >= gameList.Count)
			{
				_playedGameIndices.Clear();
				_gamePlayedInARow = 0;
			}
			gameNumber = UnityEngine.Random.Range(0, gameList.Count);

		} while (_playedGameIndices.Contains(gameNumber));

		_gamePlayedInARow++;
		_playedGameIndices.Add(gameNumber);


		return gameList[gameNumber];

	}
}

