using DG.Tweening;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleGallery : GalleryController {
	[SerializeField] private Idle _idle;

	[SerializeField] private float _timeBetweenPhotos;
	private Tween _tween;

	public override void SetupGallery(bool isQuizEnd = false, Theme theme = null)
	{
		_index = 0;
		_listOfPhotos.Clear();

		if (Data.TranslatedContent.GetThemeByName("Map1") != null)
		{
			_listOfPhotos.AddRange(CollectImages("Map1"));
		}

		if (Data.TranslatedContent.GetThemeByName("Map2") != null)
		{
			_listOfPhotos.AddRange(CollectImages("Map2"));
		}
		LoadPhotoAndPhotoNumber(_index);
		_tween.Kill();
		StartLoopingPhotos(_index);
	}

	private List<Photo> CollectImages(string map)
	{
		List<Photo> photoList = new List<Photo>();
		var subThemes = Data.TranslatedContent.GetThemeByName(map).SubThemes;
		foreach (var subtheme in subThemes)
		{
			List<Photo> subThemePhotos = new List<Photo>();
			if (subtheme.GetMediaByName("Gallery") != null)
			{
				if (subtheme.GetMediaByName("Gallery").GetPhotos() != null)
				{
					subThemePhotos = subtheme.GetMediaByName("Gallery").GetPhotos();
					photoList.AddRange(subThemePhotos);
				}
			}
		}
		return photoList;
	}

	private void StartLoopingPhotos(int index)
	{

		_tween = DOVirtual.DelayedCall(_timeBetweenPhotos, () => {
			NextPhoto();
			Resources.UnloadUnusedAssets();
			if (_idle.IsOnIdleScreen) StartLoopingPhotos(_index);
		});
	}



}
