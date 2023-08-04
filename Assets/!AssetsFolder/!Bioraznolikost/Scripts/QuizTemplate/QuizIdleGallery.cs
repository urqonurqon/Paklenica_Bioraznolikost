using DG.Tweening;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum IdleType {
	GalleryLeft,
	GalleryRight
}


public class QuizIdleGallery : GalleryController {
	[SerializeField] private Idle _idle;

	[SerializeField] protected float _timeBetweenPhotos;

	//[SerializeField] private IdleType _idleType = new IdleType();

	[SerializeField] private int _seed;

	private Tween _tween;

	public override void SetupGallery(bool isQuizEnd = false, Theme theme = null)
	{
		var subThemes = Data.TranslatedContent.GetThemeByTag("QuizTemplate").SubThemes;
		_index = 0;
		foreach (var subTheme in subThemes)
		{
			List<Photo> subThemePhotos = new List<Photo>();
			if (subTheme.GetMediaByName("LeftScreen") != null)
			{
				if (subTheme.GetMediaByName("LeftScreen").GetPhotos() != null)
				{
					subThemePhotos = subTheme.GetMediaByName("LeftScreen").GetPhotos();
					_listOfPhotos.AddRange(subThemePhotos);
				}
			}
		}
		ShuffleList();
		LoadPhotoAndPhotoNumber(_index);
		_tween.Kill();
		if (!isQuizEnd)
			StartLoopingPhotos(_index);
		else
			StartLoopingPhotosQuizEnd(_index);
	}
	private void ShuffleList()
	{
		System.Random random = new System.Random(_seed);

		int n = _listOfPhotos.Count;
		Photo tmp;

		for (int i = 0; i < n; i++)
		{
			int r = i + (int)(random.NextDouble() * (n - i));

			tmp = _listOfPhotos[r];
			_listOfPhotos[r] = _listOfPhotos[i];
			_listOfPhotos[i] = tmp;

		}
	}

	public virtual void StartLoopingPhotos(int index)
	{

		_tween = DOVirtual.DelayedCall(_timeBetweenPhotos, () => {
			NextPhoto();
			if (_idle.IsOnIdleScreen) StartLoopingPhotos(_index);
		});
	}


	public void StartLoopingPhotosQuizEnd(int index)
	{

		DOVirtual.DelayedCall(_timeBetweenPhotos, () => {
			NextPhoto();
			StartLoopingPhotosQuizEnd(_index);
		});
	}


}
