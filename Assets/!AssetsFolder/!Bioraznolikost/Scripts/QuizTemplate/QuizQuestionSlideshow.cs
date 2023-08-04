using DG.Tweening;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Scripts.Quiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizQuestionSlideshow : GalleryController {


	[SerializeField] private float _timeBetweenPhotos;
	private bool _loopPhotos;

	private Tween _tween;

	public override void Awake()
	{
		base.Awake();
		Quiz.OnQuestionAsked += SetupGallery;
		Quiz.OnQuestionAnswered += StopLoopingPhotos;
		GetComponentInParent<UIView>().HideBehavior.OnFinished.Event.AddListener(() => StopLoopingPhotos(true, null));
	}

	public override void SetupGallery(bool x, Theme question)
	{
		_index = 0;
		_listOfPhotos = question.GetMediaByName("LeftScreen").GetPhotos();
		LoadPhotoAndPhotoNumber(_index);
		_loopPhotos = true;
		StartLoopingPhotos(_index);
	}

	public void StopLoopingPhotos(bool isCorrectAnswer, QuizItem item)
	{
		_loopPhotos = false;
		_tween.Kill();

		for (int i = 0; i < _listOfPhotos.Count; i++)
		{
			if (_listOfPhotos[i].Name.Contains("*"))
			{
				_index = i;
				if (isCorrectAnswer)
				{
					ChangePhoto();
				}
				else
				{
					StartCoroutine(ChangePhotoAfterDelay());
				}
			}
		}
	}

	private IEnumerator ChangePhotoAfterDelay()
	{
		yield return new WaitForSeconds(2);
		ChangePhoto();

	}


	private void StartLoopingPhotos(int index)
	{
		
		_tween = DOVirtual.DelayedCall(_timeBetweenPhotos, () => {
			NextPhoto();
			Resources.UnloadUnusedAssets();
			if (_loopPhotos) StartLoopingPhotos(_index);
		});
	}



}