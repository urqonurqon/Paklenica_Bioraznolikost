using System.Collections.Generic;
using _AssetsFolder.BaseExample.Scripts;
using DG.Tweening;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.UiUtility.Base;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.UI;
using Paklenica;
using Novena.Admin;
using Scripts.Utility;
using System;
using Doozy.Engine.Nody;

public class HomeTemplate : UiController {


	[Header("MainMenu components")]
	[SerializeField] private Transform _mainMenuContainer;
	[SerializeField] private GameObject _mainMenuBtnPrefab;
	[SerializeField] private RawImage _backgroundImage;

	//[SerializeField] private AdminGestureLogin _adminGestureLogin;
	#region Private fields


	private bool _isFirstTime;
	private List<GameObject> _menuButtonList;

	#endregion

	public override void Awake()
	{
		base.Awake();
		_menuButtonList = new List<GameObject>();
		_isFirstTime = true;

		Idle.OnEnterExitIdle += ResetFirstTimeBool;
	}

	private void ResetFirstTimeBool(bool isIdle)
	{
		if (isIdle)
			_isFirstTime = true;
	}

	public override void OnShowViewStart()
	{
		base.OnShowViewStart();
		//_adminGestureLogin.EnableGestureDraw = false;
		//_adminGestureLogin.ClearLines();
		if (FindObjectOfType<GraphController>().Graph.PreviousActiveNode.name == "SwitchBackNode")
			_isFirstTime = true;

		GenerateMainMenu();

	}

	public override void OnHideViewFinished()
	{
		base.OnHideViewFinished();
	}



	private void GenerateMainMenu()
	{
		UnityHelper.DestroyObjects(_menuButtonList);

		var themeList = Data.TranslatedContent.GetThemesExcludeByTag("SYSTEM");



		for (int i = 0; i < themeList.Count; i++)
		{
			Theme theme = themeList[i];

			GameObject go = Instantiate(_mainMenuBtnPrefab, _mainMenuContainer);
			MenuButton mb = go.GetComponent<MenuButton>();
			TextButtonColor tbc = go.GetComponent<TextButtonColor>();

			if (theme.isVisited)
			{
				tbc.ColorObjectsInList();
			}
			else
			{
				tbc.RecolorObjectsInList();
			}

			mb.SetButton(theme);
			go.SetActive(true);

			if (_isFirstTime)
				tbc.RecolorObjectsInList();

			_menuButtonList.Add(go);
		}

		if (_isFirstTime || Data.Theme == null)
		{
			AssetsFileLoader.LoadTexture2D(themeList[0].GetMediaByName("Gallery").GetPhotos()[1].FullPath, _backgroundImage);
			_isFirstTime = false;
		}
		else
		{
			AssetsFileLoader.LoadTexture2D(Data.Theme.GetMediaByName("Gallery").GetPhotos()[1].FullPath, _backgroundImage);
		}
	}
}