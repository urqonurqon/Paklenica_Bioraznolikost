using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizHomeGallery : QuizIdleGallery
{
	public override void StartLoopingPhotos(int index)
	{
		DOVirtual.DelayedCall(_timeBetweenPhotos, () => {
			NextPhoto();
			if (QuizHomeTemplate.IsOnHomeScreen) StartLoopingPhotos(_index);
		});
	}
	
}
