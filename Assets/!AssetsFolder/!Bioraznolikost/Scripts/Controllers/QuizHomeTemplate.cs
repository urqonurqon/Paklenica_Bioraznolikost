using Doozy.Engine.UI;
using Novena.UiUtility.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizHomeTemplate : UiController {
	public static Action OnEnterQuizPressed;

	public UIButton EnterQuizButton;

	public static bool IsOnHomeScreen;
	[SerializeField] private QuizHomeGallery _quizHomeGallery;

	public override void Awake()
	{
		base.Awake();
		try
		{
			EnterQuizButton.OnClick.OnTrigger.Event.AddListener(() => OnEnterQuizPressed?.Invoke());
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}

	public override void OnShowViewStart()
	{
		base.OnShowViewStart();
		IsOnHomeScreen = true;
		try
		{

			_quizHomeGallery.SetupGallery();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}
	public override void OnHideViewFinished()
	{
		base.OnHideViewFinished();
		IsOnHomeScreen = false;
	}
}
