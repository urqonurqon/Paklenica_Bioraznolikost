using Doozy.Engine.UI;
using Novena.DAL.Model.Guide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Utility;
using DG.Tweening;
using Doozy.Engine.Touchy;
using System;
using TMPro;

public class PhotoFullscreen : GalleryController {

	[SerializeField] private GalleryController _galleryController;


	[Space(30)]
	[Header("Class fields")]


	[Space(10)]
	[SerializeField] private CanvasGroup _fullscreenPhotoCanvasGroup;
	[SerializeField] private UIButton _closeFullscreenPhotoButton;


	public override void Awake()
	{
		_previousPhotoButton.OnClick.OnTrigger.Event.AddListener(PreviousPhoto);
		_nextPhotoButton.OnClick.OnTrigger.Event.AddListener(NextPhoto);

		ShowNextPhotoButton();
		ShowPreviousPhotoButton();

		_closeFullscreenPhotoButton.OnClick.OnTrigger.Event.AddListener(() => {
			Close();

		});
	}

	public void Open(List<Photo> listOfPhotos, int indexOfPhoto)
	{
		_listOfPhotos = listOfPhotos;

		_index = indexOfPhoto;
		if (_listOfPhotos.Count == 1)
		{
			HideNextPhotoButton();
			HidePreviousPhotoButton();
		}
		LoadPhotoAndPhotoNumber(_index);
		if (GetComponent<FullscreenPhotoLabel>() != null)
			GetComponent<FullscreenPhotoLabel>().SetLabel();
		StartCoroutine(ShowFullscreenDelayed());
	}

	private IEnumerator ShowFullscreenDelayed()
	{
		yield return new WaitForSeconds(.25f);
		ShowFade(_fullscreenPhotoCanvasGroup);
	}

	public void Close()
	{
		ShowNextPhotoButton();
		ShowPreviousPhotoButton();
		HideFade(_fullscreenPhotoCanvasGroup);

		_galleryController.FullscreenClosed(_index);
	}


}
