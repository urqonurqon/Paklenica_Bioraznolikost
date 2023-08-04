using Doozy.Engine.UI;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTemplateGallery : GalleryController
{

	public Theme Theme { get; set; }

	public override void Awake()
	{
		_previousPhotoButton.OnClick.OnTrigger.Event.AddListener(PreviousPhoto);
		_nextPhotoButton.OnClick.OnTrigger.Event.AddListener(NextPhoto);

		ShowNextPhotoButton();
		ShowPreviousPhotoButton();



	}


	public override void SetupGallery(bool x=false, Theme t=null)
	{
		_index = 0;
		_listOfPhotos = Theme.GetMediaByName("Gallery").GetPhotos();
		LoadPhotoAndPhotoNumber(_index);
		_photoFullscreenButtonOpen.OnClick.OnTrigger.Event.AddListener(() => {
			_photoFullscreen.Open(_listOfPhotos, _index);
		});
	}


}
