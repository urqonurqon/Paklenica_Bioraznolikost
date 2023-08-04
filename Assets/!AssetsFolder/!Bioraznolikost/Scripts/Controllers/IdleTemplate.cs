using DG.Tweening;
using Doozy.Engine;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.UiUtility.Base;
using Scripts.Utility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IdleTemplate : UiController {

	public static Action OnSingleIdle;
	public static Action OnGroupIdle;
	public static Action OnAllIdle;

	public RawImage RawImageSingle;
	public RawImage RawImageGroup;
	public RawImage RawImageAll;
	public UIButton[] _backButtons;

	private Idle _idle;
	private IdleGallery _idleGallery;

	[SerializeField] private UIView _homeTemplate;

	public override void Awake()
	{
		_idle = FindObjectOfType<Idle>();
		Idle.OnEnterExitIdle += ResetMenuButtonColors;
		base.Awake();
		if (_backButtons.Length > 0)
		{
			try
			{
				foreach (var button in _backButtons)
				{
					button.OnClick.OnTrigger.Event.AddListener(() => {
						_homeTemplate.ShowBehavior.InstantAnimation = false;
						GameEventMessage.SendEvent("BackButton");
						StartCoroutine(SetInstantAnimationAfterDelay());
					});
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}
	}

	private void ResetMenuButtonColors(bool isIdle)
	{
		if (isIdle)
		{
			foreach (var theme in Data.TranslatedContent.GetThemesExcludeByTag("SYSTEM"))
			{
				theme.isVisited = false;
			}
		}
	}

	private IEnumerator SetInstantAnimationAfterDelay()
	{
		yield return new WaitForSeconds(0.4f);
		_homeTemplate.ShowBehavior.InstantAnimation = true;
	}


	public override void OnShowViewStart()
	{
		if (SceneManager.GetActiveScene().name.Contains("Kviz")) return;
		base.OnShowViewStart();
		_idleGallery = GetComponent<IdleGallery>();
		_idleGallery.SetupGallery();

		_idle.IsOnIdleScreen = true;
	}



	public void ChangePhoto(string caughtString)
	{
		switch (caughtString)
		{
			case "AllOn":
				RawImageSingle.DOFade(0, 0.5f);
				RawImageGroup.DOFade(0, 0.5f);
				RawImageAll.DOFade(1, 0.5f);
				OnAllIdle?.Invoke();
				break;
			case "GroupOn":
				RawImageSingle.DOFade(0, 0.5f);
				RawImageGroup.DOFade(1, 0.5f);
				RawImageAll.DOFade(0, 0.5f);
				OnGroupIdle?.Invoke();
				break;
			case "SingleOn":
				RawImageSingle.DOFade(1, 0.5f);
				RawImageGroup.DOFade(0, 0.5f);
				RawImageAll.DOFade(0, 0.5f);
				OnSingleIdle?.Invoke();
				break;
		}
	}

}