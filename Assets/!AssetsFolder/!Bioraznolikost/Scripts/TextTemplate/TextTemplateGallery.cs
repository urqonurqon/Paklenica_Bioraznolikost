using Novena.DAL;
using Novena.DAL.Model.Guide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGH;
using DG.Tweening;
using System;

public class TextTemplateGallery : GalleryController {

	[SerializeField] private GameObject _videoPlayer;
	[SerializeField] private RectTransform _dotsContainer;
	[SerializeField] private GameObject _dotPrefab;
	[SerializeField] private GameObject _dotCirclePrefab;

	private bool _shouldTurnOffRaycastingP;
	private bool _shouldTurnOffRaycastingN;

	private List<GameObject> _dots = new List<GameObject>();
	private GameObject _dotCircle;


	public override void Awake()
	{
		_previousPhotoButton.OnClick.OnTrigger.Event.AddListener(PreviousPhoto);
		_nextPhotoButton.OnClick.OnTrigger.Event.AddListener(NextPhoto);

		ShowNextPhotoButton();
		ShowPreviousPhotoButton();

		TextTemplate.OnHideViewStartAction += DestroyDots;
	}

	private void DestroyDots()
	{
		for (int i = 0; i < _dots.Count; i++)
		{
			Destroy(_dots[i]);
		}
		Destroy(_dotCircle);
		_dots.Clear();
	}

	public override void SetupGallery(bool x = false, Theme theme = null)
	{
		base.SetupGallery(x, theme);
		_index = -1;
		if (_index == -1)
		{
			_videoPlayer.SetActive(true);
		}
		HidePreviousPhotoButton();
		ShowNextPhotoButton();


		SetupDots();
	}

	private void SetupDots()
	{
		for (int i = 0; i < _listOfPhotos.Count + 1; i++)
		{
			var dot = Instantiate(_dotPrefab, _dotsContainer);
			_dots.Add(dot);
		}
		_dotCircle = Instantiate(_dotCirclePrefab, _dots[0].GetComponent<RectTransform>());
		_dots[0].GetComponent<RectTransform>().sizeDelta *= .7f;
	}


	private void MoveCircle(int index)
	{
		foreach (var dot in _dots)
		{
			dot.GetComponent<RectTransform>().DOSizeDelta(new Vector2(7f, 7f), .7f);
		}
		_dots[index].GetComponent<RectTransform>().DOSizeDelta(new Vector2(4.9f, 4.9f), .7f);
		_dotCircle.GetComponent<RectTransform>().SetParent(_dots[index].transform);
		_dotCircle.GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
	}

	public override void NextPhoto()
	{

		_videoPlayer.SetActive(false);

		BlockPhotoChangeDuringTransition(true);
		if (_previousButtonCanvasGroup != null)
			ShowPreviousPhotoButton();


		_index++;
		bool nextPhotoExist = _index + 1 < _listOfPhotos.Count;
		MoveCircle(_index + 1);

		if (!nextPhotoExist)
		{
			HideNextPhotoButton();
			_shouldTurnOffRaycastingN = true;
			ChangePhoto();
		}
		else
		{

			_shouldTurnOffRaycastingN = false;
			_shouldTurnOffRaycastingP = false;
			ChangePhoto();
		}
	}

	private IEnumerator TurnOnGestureListener()
	{
		yield return new WaitForSeconds(0.31f);
		HideNextPhotoButton();
	}

	public override void PreviousPhoto()
	{
		BlockPhotoChangeDuringTransition(true);
		if (_nextButtonCanvasGroup != null)
			ShowNextPhotoButton();

		_index--;
		MoveCircle(_index + 1);

		if (_index < 0)
		{
			_videoPlayer.SetActive(true);
			HidePreviousPhotoButton();
			_shouldTurnOffRaycastingP = true;
		}
		else
		{
			_shouldTurnOffRaycastingP = false;
			_shouldTurnOffRaycastingN = false;
			ChangePhoto();
		}


	}

	public override void ChangePhoto()
	{
		_rawImage.DOColor(Color.black, .15f).OnComplete(() => {
			LoadPhotoAndPhotoNumber(_index);

			_rawImage.DOColor(Color.white, .15f).OnComplete(() => {

				BlockPhotoChangeDuringTransition(false);
				if (_shouldTurnOffRaycastingP)
				{
					HidePreviousPhotoButton();
				}
				if (_shouldTurnOffRaycastingN)
					HideNextPhotoButton();
			});

		});
	}
}
