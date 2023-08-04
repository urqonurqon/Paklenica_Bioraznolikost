using Novena.DAL;
using Novena.DAL.Model.Guide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTemplateGallery : GalleryController {
	
	[HideInInspector]	public List<Photo> Gallery = new List<Photo>();
	

	public override void Awake()
	{
		_previousPhotoButton.OnClick.OnTrigger.Event.AddListener(PreviousPhoto);
		_nextPhotoButton.OnClick.OnTrigger.Event.AddListener(NextPhoto);

		ShowNextPhotoButton();
		ShowPreviousPhotoButton();



	}

	public override void SetupGallery(bool x=false,Theme t=null)
	{
		_index = 0;
		_listOfPhotos = Gallery;
		_photoFullscreenButtonOpen.OnClick.OnTrigger.Event.AddListener(() => {
			_photoFullscreen.Open(_listOfPhotos, _index);
		});
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
}
