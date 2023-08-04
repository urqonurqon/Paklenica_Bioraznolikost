using DG.Tweening;
using Doozy.Engine.Touchy;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour {

	[SerializeField] protected CanvasGroup _previousButtonCanvasGroup;
	[SerializeField] protected CanvasGroup _nextButtonCanvasGroup;

	[SerializeField] protected UIButton _previousPhotoButton;
	[SerializeField] protected UIButton _nextPhotoButton;

	[SerializeField] protected TMP_Text _photoNumber;

	[SerializeField] protected RawImage _rawImage;

	[SerializeField] protected UIButton _photoFullscreenButtonOpen;

	[SerializeField] protected PhotoFullscreen _photoFullscreen;


	protected List<Photo> _listOfPhotos = new List<Photo>();
	protected int _index;

	[Header("Properties")]
	[SerializeField] protected bool hasNumber;

	public virtual void Awake()
	{

	}

	public virtual void SetupGallery(bool x = false, Theme theme = null)
	{
		_index = 0;
		_listOfPhotos = Data.Theme.GetMediaByName("Gallery").GetPhotos();
		if (_listOfPhotos.Count == 1)
		{
			HideNextPhotoButton();
			HidePreviousPhotoButton();
		}
		else
		{
			ShowNextPhotoButton();
			ShowPreviousPhotoButton();
		}
		LoadPhotoAndPhotoNumber(_index);
	}

	public void FullscreenClosed(int index)
	{
		_index = index;
		LoadPhotoAndPhotoNumber(_index);
	}

	protected void LoadPhotoAndPhotoNumber(int index)
	{
		if (hasNumber)
		{
			SetPhotoNumber();
		}
		LoadPhoto(index);

	}

	private void SetPhotoNumber()
	{
		_photoNumber.text = (_index + 1) + "/" + _listOfPhotos.Count;
	}

	private async void LoadPhoto(int index)
	{
		Resources.UnloadUnusedAssets();
		await AssetsFileLoader.LoadTextureAsync(_listOfPhotos[index].FullPath, _rawImage);
	}

	protected void BlockPhotoChangeDuringTransition(bool isBlocked)
	{
		if (_previousButtonCanvasGroup != null)
		{

			_previousButtonCanvasGroup.blocksRaycasts = !isBlocked;
			if (_previousPhotoButton.transform.parent.GetComponent<GestureListener>() != null)
				_previousPhotoButton.transform.parent.GetComponent<GestureListener>().enabled = !isBlocked;
			if (_previousPhotoButton.GetComponent<GestureListener>() != null)
				_previousPhotoButton.GetComponent<GestureListener>().enabled = !isBlocked;
		}
		if (_nextButtonCanvasGroup != null)
		{

			_nextButtonCanvasGroup.blocksRaycasts = !isBlocked;

			if (_nextPhotoButton.transform.parent.GetComponent<GestureListener>() != null)
				_nextPhotoButton.transform.parent.GetComponent<GestureListener>().enabled = !isBlocked;
			if (_nextPhotoButton.GetComponent<GestureListener>() != null)
				_nextPhotoButton.GetComponent<GestureListener>().enabled = !isBlocked;
		}


	}


	#region Next/Prev Photo
	public virtual void PreviousPhoto()
	{
		BlockPhotoChangeDuringTransition(true);
		bool prevPhotoExists = _index - 1 >= 0;
		if (_nextButtonCanvasGroup != null)
			ShowNextPhotoButton();

		_index--;


		if (prevPhotoExists)
		{
			ChangePhoto();
		}
		else
		{
			ChangePhoto();
			_index = _listOfPhotos.Count - 1;
		}
	}
	public virtual void NextPhoto()
	{
		BlockPhotoChangeDuringTransition(true);
		bool nextPhotoExist = _index + 1 <= _listOfPhotos.Count - 1;
		if (_previousButtonCanvasGroup != null)
			ShowPreviousPhotoButton();

		_index++;


		if (nextPhotoExist)
		{
			ChangePhoto();
		}
		else
		{
			ChangePhoto();
			_index = 0;
		}

	}


	public virtual void ChangePhoto()
	{
		_rawImage.DOColor(Color.black, .15f).OnComplete(() => {
			LoadPhotoAndPhotoNumber(_index);

			_rawImage.DOColor(Color.white, .15f).OnComplete(() => {

				BlockPhotoChangeDuringTransition(false);
			});

		});
	}
	#endregion

	#region Previous&Next CG Show/Hide
	public void HidePreviousPhotoButton()
	{
		HideFade(_previousButtonCanvasGroup);
		_previousButtonCanvasGroup.GetComponent<GestureListener>().enabled = false;
	}
	public void ShowPreviousPhotoButton()
	{
		ShowFade(_previousButtonCanvasGroup);
		_previousButtonCanvasGroup.GetComponent<GestureListener>().enabled = true;
	}
	public void HideNextPhotoButton()
	{
		HideFade(_nextButtonCanvasGroup);
		_nextButtonCanvasGroup.GetComponent<GestureListener>().enabled = false;
	}
	public void ShowNextPhotoButton()
	{
		ShowFade(_nextButtonCanvasGroup);
		_nextButtonCanvasGroup.GetComponent<GestureListener>().enabled = true;
	}

	#endregion

	#region ShowHideCanvasGroup
	protected void ShowFade(CanvasGroup canvasGroup)
	{
		canvasGroup.DOFade(1, .3f);
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}
	protected void HideFade(CanvasGroup canvasGroup)
	{
		canvasGroup.DOFade(0, .3f);
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}
	#endregion
}
