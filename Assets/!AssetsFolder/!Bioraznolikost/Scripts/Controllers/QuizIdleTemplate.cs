using Novena.UiUtility.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizIdleTemplate : UiController {

	[SerializeField] private QuizIdleGallery _quizIdleGallery;
	

	public override void OnShowViewStart()
	{
		_quizIdleGallery.SetupGallery();
	}



}
