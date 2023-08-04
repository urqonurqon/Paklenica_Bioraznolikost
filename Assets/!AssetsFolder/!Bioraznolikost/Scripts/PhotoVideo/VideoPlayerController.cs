using DG.Tweening;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.UiUtility.Base;
using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;
using UnityEngine.UI;
using CGH;

public class VideoPlayerController : MonoBehaviour {


	[SerializeField] private RectTransform _transform;
	[SerializeField] private UIButton _openFullscreenButton;
	[SerializeField] private UIButton _closeFullscreenButton;

	[SerializeField] private CanvasGroup _fullscreenOverlay;
	[SerializeField] private RectTransform _dotsContainer;

	[SerializeField] private RectTransform _nextArrow;
	[SerializeField] private RectTransform _prevArrow;

	private Vector2 _originalTransformPosition;

	private void Awake()
	{
		_openFullscreenButton.OnClick.OnTrigger.Event.AddListener(() => OpenCloseFullscreen(true));
		_closeFullscreenButton.OnClick.OnTrigger.Event.AddListener(() => OpenCloseFullscreen(false));
		_originalTransformPosition = _transform.anchoredPosition;
	}

	private void OpenCloseFullscreen(bool open)
	{
		if (open)
		{
			_transform.DOAnchorPos(Vector2.zero, .5f);
			_transform.DOScale(1.77f, .5f);
			_dotsContainer.DOScale(0.72f, .5f);
			_nextArrow.DOScale(0.72f, .5f);
			_prevArrow.DOScale(0.72f, .5f);
			_fullscreenOverlay.Fade(1, .5f);
			_dotsContainer.DOAnchorPosX(-862.2393f / 2f + _dotsContainer.rect.width / 2, .5f);
			_nextArrow.DOAnchorPosX(61.875f, .5f);
			_prevArrow.DOAnchorPosX(-61.875f, .5f);
		}
		else
		{
			_transform.DOAnchorPos(_originalTransformPosition, .5f);
			_transform.DOScale(1f, .5f);
			_fullscreenOverlay.Fade(0, .5f);
			_dotsContainer.DOAnchorPosX(0, .5f);
			_nextArrow.DOAnchorPosX(-35f, .5f);
			_prevArrow.DOAnchorPosX(35f, .5f);
		}
	}
}