using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Utility;
using Scripts.Gallery;
using DG.Tweening;
using Assets.Scripts.Components.PinchScrollRect;
using CGH;
using RenderHeads.Media.AVProVideo;

public class PinController : MonoBehaviour {
	public List<GameObject> Pins = new List<GameObject>();
	public List<CanvasGroup> Territories = new List<CanvasGroup>();

	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _latinName;
	[SerializeField] private TMP_Text _content;

	[SerializeField] private MapTemplateGallery _mapTemplateGallery;

	[SerializeField] private CanvasGroup _backButtonCG;

	[SerializeField] private RectTransform _pinContent;
	[SerializeField] private UIButton[] _closePinContentButton;


	[SerializeField] private RectTransform _mapRectTransform;

	[SerializeField] private PinchableScrollRect _pinchableScrollRect;


	[SerializeField] private TextGradient _textGradient;

	[SerializeField] private GameObject _photo;
	[SerializeField] private GameObject _audioIcon;
	[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

	[SerializeField] private MapTemplate _mapTemplate;

	[SerializeField] private MediaPlayer _mediaPlayer;

	private SubTheme[] _subThemes;
	private bool isPinContentMoved;
	[HideInInspector] public int LastPinIndex = -1;


	private void Awake()
	{
		foreach (var button in _closePinContentButton)
		{
			button.OnClick.OnTrigger.Event.AddListener(() => ResetMap());
		}
	}
	public void ResetMap()
	{
		_mediaPlayer.CloseMedia();
		var newPinContentPosition = _pinContent.anchoredPosition.x + 850;
		_pinContent.DOAnchorPosX(Mathf.Clamp(newPinContentPosition, 451, 1301), .5f);
		StartCoroutine(FixGradient());

		isPinContentMoved = false;
		_backButtonCG.Fade(1, .5f);


		ScalePin();
		ShowTerritory();
		LastPinIndex = -1;
		_mapRectTransform.DOPivot(new Vector2(.5f, .5f), .5f);
		ZoomMap(1f);
		MoveMap(new Vector2(168, 94.5f));
	}

	public void ConnectPinsWithThemes(Theme theme)
	{

		_subThemes = theme.SubThemes;

		for (int i = 0; i < _subThemes.Length; i++)
		{
			var index = i;
			if (_subThemes[i].Label == Pins[i].name)
			{
				var pinButton = Pins[i].GetComponent<UIButton>();

				pinButton.OnClick.OnTrigger.Event.AddListener(() => PinSelected(index));
			}
			Pins[i].transform.localScale = Vector3.one;
		}
	}

	private void PinSelected(int index)
	{
		Resources.UnloadUnusedAssets();
		if (index != LastPinIndex)
		{
			PlayAudio(index);
			ChangeContent(index);
			SlideInContent();


			ZoomAndMoveMap();

			ShowTerritory(index);
			ScalePin(index);
			LastPinIndex = index;
			_textGradient.FixSoftMaskAndResetScrollView();
		}
	}

	private void PlayAudio(int index)
	{
		if (_subThemes[index].GetMediaByName("Audio") != null)
		{
			MediaPath path = new MediaPath(_subThemes[index].GetMediaByName("Audio").FullPath, MediaPathType.AbsolutePathOrURL);
			_mediaPlayer.OpenMedia(path);
			_audioIcon.SetActive(true);
		}
		else
		{
			_mediaPlayer.CloseMedia();
			_audioIcon.SetActive(false);
		}
	}

	private void ScalePin(int index = -1)
	{

		if (LastPinIndex >= 0 && LastPinIndex != index)
			Pins[LastPinIndex].transform.DOScale(new Vector3(1 / Mathf.Sqrt(_mapRectTransform.localScale.x), 1 / Mathf.Sqrt(_mapRectTransform.localScale.y), 1 / Mathf.Sqrt(_mapRectTransform.localScale.z)), .5f);

		if (index >= 0)
			Pins[index].transform.DOScale(1.5f * .86f, .5f);

	}

	private void ChangeContent(int index)
	{
		var content = _subThemes[index].GetMediaByName("Content").Text;
		var name = _subThemes[index].Name;
		var latinName = _subThemes[index].GetMediaByName("LatinName").Text;
		if (_subThemes[index].GetMediaByName("Gallery").GetPhotos() != null)
		{
			_photo.SetActive(true);
			_verticalLayoutGroup.spacing = -54.58f;

			_mapTemplateGallery.Gallery = _subThemes[index].GetMediaByName("Gallery").GetPhotos();
			_mapTemplateGallery.SetupGallery();


		}
		else
		{
			_photo.SetActive(false);
			_verticalLayoutGroup.spacing = -337.7f;
		}


		_content.text = content;
		_name.text = name;
		_latinName.text = latinName;
	}

	private void SlideInContent()
	{
		var newPosition = _pinContent.anchoredPosition.x - 850;
		if (!isPinContentMoved)
		{
			_pinContent.DOAnchorPosX(Mathf.Clamp(newPosition, 451, 1301), .5f);
			StartCoroutine(FixGradient());
		}
		isPinContentMoved = true;
		_backButtonCG.Fade(0, .5f);
	}

	private IEnumerator FixGradient()
	{
		var countDown = .5f;

		while (countDown >= 0)
		{
			_textGradient.FixSoftMask();
			countDown -= Time.deltaTime;
			yield return null;
		}

	}

	private void ShowTerritory(int index = -1)
	{
		if (index >= 0)
			Territories[index].DOFade(1f, .5f);
		if (LastPinIndex >= 0 && LastPinIndex != index)
			Territories[LastPinIndex].DOFade(0f, .5f);
	}

	private void ZoomAndMoveMap()
	{
		_mapRectTransform.DOPivot(new Vector2(.5f, .5f), .5f);

		ZoomMap(1.35f);
		MoveMap(new Vector2(-324.92f, 136.81f));
	}

	private void MoveMap(Vector2 position)
	{
		_mapRectTransform.DOAnchorPos(position, 0.5f);
	}

	private void ZoomMap(float zoom)
	{
		DOVirtual.Float(_pinchableScrollRect._currentZoom, zoom, 0.5f, (currentZoom) => {
			_pinchableScrollRect._currentZoom = currentZoom;
		});
	}
}
