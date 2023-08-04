using Novena.UiUtility.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Utility;
using Novena.DAL;
using Doozy.Engine.UI;
using TMPro;
using DG.Tweening;
using Paklenica;
using Doozy.Engine.Touchy;
using Novena.DAL.Model.Guide;
using _AssetsFolder.BaseExample.Scripts;
using Doozy.Engine;
using CGH;
using RenderHeads.Media.AVProVideo;

public class TextTemplate : UiController {

	public static Action OnHideViewStartAction;


	[SerializeField] private CanvasGroup _raycastBlocker;

	[SerializeField] private CanvasGroup _content;
	[SerializeField] private RawImage _gradient;

	[SerializeField] private RawImage _background;
	[SerializeField] private RawImage _backgroundImage;
	[SerializeField] private Color _black;
	[SerializeField] private Color _yellow;
	[SerializeField] private UIButton _backButton;

	[SerializeField] private SideMenu _sideMenu;
	[SerializeField] private GameTemplate _gameTemplate;

	[SerializeField] private TMP_Text _contentText;
	[SerializeField] private TMP_Text _titleText;
	[SerializeField] private TMP_Text _photoTitleText;
	[SerializeField] private TextGradient _textGradient;

	[SerializeField] private RectTransform _fakeInstructions;

	[SerializeField] private MediaPlayer _mediaPlayer;
	[SerializeField] private TextTemplateGallery _textTemplateGallery;
	private Sequence _hideGameContent;


	public override void Awake()
	{


		MenuButton.OnClick += ShowFromHome;
		base.Awake();
		SideMenuButton.OnClick += HideToMapOrGame;
		MapTemplate.OnBackButtonClick += ShowFromMap;
		Idle.OnEnterExitIdle += ResetOnIdle;
		GameTemplate.OnBackButtonClick += ShowFromGame;
		_backButton.OnClick.OnTrigger.Event.AddListener(HideToHome);
		Data.OnTranslatedContentUpdated += FixGradient;

		RecordHideGameContentSequence();
	}

	private void FixGradient()
	{
		StartCoroutine(FixGradientCoroutine());
	}

	private IEnumerator FixGradientCoroutine()
	{
		yield return new WaitForSeconds(2);
		_gradient.DOFade(1, .2f);
		_gradient.enabled = false;
		_gradient.enabled = true;
	}

	private void ResetOnIdle(bool isIdle)
	{
		if (isIdle)
		{

			_fakeInstructions.anchoredPosition = new Vector2(-2151.2f, _fakeInstructions.anchoredPosition.y);



		}
	}

	private void RecordHideGameContentSequence()
	{
		_hideGameContent = DOTween.Sequence();
		_hideGameContent.SetAutoKill(false);
		foreach (var content in transform.parent.GetComponentInChildren<GameTemplate>()._content)
		{
			_hideGameContent.Insert(0, content.DOFade(0, 1));
		}
		_hideGameContent.Insert(0, transform.parent.GetComponentInChildren<GameTemplate>()._cover.DOFade(0, 1));
		_hideGameContent.Pause();
	}

	private void ShowFromGame(bool isFromInGame)
	{
		RaycastBlocker.BlockRaycasts();
		_hideGameContent.Play().OnComplete(() => {


			GameEventMessage.SendEvent("BackButton");


			if (isFromInGame)
			{

				_fakeInstructions.anchoredPosition = new Vector2(-2151.2f, _fakeInstructions.anchoredPosition.y);
				ShowHideContent(true, () => {
					_gradient.DOFade(1, .2f);
					_gradient.enabled = false;
					_gradient.enabled = true;
					RaycastBlocker.UnblockRaycasts();
				});
			}
			else
			{

				SlideSidePanel(670.8f, -2151.2f, () => {
					ShowHideContent(true, () => {
						_gradient.DOFade(1, .2f);
						_gradient.enabled = false;
						_gradient.enabled = true;
						RaycastBlocker.UnblockRaycasts();
					});
				});
			}

		});
	}

	private void ShowFromMap()
	{
		GameEventMessage.SendEvent("BackButton");
		SlideSideMenu(-1300f, -620f);
		ShowHideContent(true, () => {
			_gradient.DOFade(1, .2f);
			_gradient.enabled = false;
			_gradient.enabled = true;
			RaycastBlocker.UnblockRaycasts();

		});
	}



	private void ShowFromHome(string themeTag)
	{
		_gradient.enabled = false;
		RaycastBlocker.BlockRaycasts();
		GameEventMessage.SendEvent(themeTag);
		ShowHideContent(true);
		SlideSideMenu(-1300, -620f);
		_background.DOColor(_black, 1).From(_yellow).OnComplete(() => {
			_gradient.DOFade(1, .2f);
			_gradient.enabled = false;
			_gradient.enabled = true;
			RaycastBlocker.UnblockRaycasts();
		});


	}
	private void HideToHome()
	{
		ShowHideContent(false);
		SlideSideMenu(-620f, -1300f);
		_gradient.DOFade(0, .05f).OnComplete(() => {
			_background.DOColor(_yellow, 1).From(_black).OnComplete(() => {

				GameEventMessage.SendEvent("BackButton");
				RaycastBlocker.UnblockRaycasts();

			});
		});
	}

	private void HideToMapOrGame(string themeTag)
	{
		RaycastBlocker.BlockRaycasts();
		if (themeTag == "MapTemplate")
		{
			SlideSideMenu(-620f, -1300f);
			_gradient.DOFade(0, .05f);
		}
		else
		{
			SlideSidePanel(-2151.2f, 0f);
			_gradient.DOFade(0, .05f);
		}
		ShowHideContent(false, () => {
			GameEventMessage.SendEvent(themeTag);
			RaycastBlocker.UnblockRaycasts();

		});
	}

	private void ShowHideContent(bool show, TweenCallback onComplete = null)
	{
		_content.blocksRaycasts = show ? true : false;
		_content.DOFade(show ? 1 : 0, 1).OnComplete(onComplete);
	}
	private void SlideSideMenu(float from, float to, TweenCallback onComplete = null)
	{
		_sideMenu.GetComponent<RectTransform>().DOAnchorPos(new Vector2(to, 0), 1).From(new Vector2(from, 0)).OnComplete(onComplete);

	}
	private void SlideSidePanel(float from, float to, TweenCallback onComplete = null)
	{
		_fakeInstructions.DOAnchorPos(new Vector2(to, 0), 1).From(new Vector2(from, 0)).OnComplete(onComplete);

	}



	public override void OnShowViewStart()
	{
		base.OnShowViewStart();
		SetupSideMenuButtons();
		SetupTitles();
		SetupText();
		_textGradient.FixSoftMaskAndResetScrollView();
		_hideGameContent.Rewind();
		SetMediaPlayer();
		_mediaPlayer.Pause();

		AssetsFileLoader.LoadTexture2D(Data.Theme.GetMediaByName("Gallery").GetPhotos()[1].FullPath, _backgroundImage);
		_textTemplateGallery.SetupGallery();
	}
	public override void OnHideViewStart()
	{
		base.OnHideViewStart();
		OnHideViewStartAction?.Invoke();
	}

	public override void OnHideViewFinished()
	{
		_mediaPlayer.Stop();
		_mediaPlayer.CloseMedia();
		base.OnHideViewFinished();
	}
	public override void OnShowViewFinished()
	{
		_mediaPlayer.Play();
	}
	private void SetupTitles()
	{

		_titleText.text = Data.Theme.GetMediaByName("Title").Text;
		_photoTitleText.text = Data.Theme.GetMediaByName("PhotoTitle").Text;

	}

	private void SetupText()
	{
		_contentText.text = Data.Theme.GetMediaByName("ContentText").Text;
	}




	private void SetupSideMenuButtons()
	{
		_sideMenu.Setup();
	}
	private void SetMediaPlayer()
	{
		var videoPath = GetVideoPath();
		if (string.IsNullOrEmpty(videoPath)) return;

		MediaPath mediaPath = new MediaPath(videoPath, MediaPathType.AbsolutePathOrURL);

		_mediaPlayer.OpenMedia(mediaPath);

		_mediaPlayer.Pause();
		_mediaPlayer.Play();


	}


	private string GetVideoPath()
	{
		string output = string.Empty;
		var media = Data.Theme.GetMediaByName("Video");

		if (media != null)
		{
			output = media.FullPath;
		}

		return output;
	}
}
