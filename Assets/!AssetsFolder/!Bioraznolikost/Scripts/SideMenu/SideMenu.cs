using Novena.DAL;
using Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour {

	[SerializeField] private GameObject _sideMenuButtonPrefab;
	[SerializeField] private Transform _buttonContainerTransform;


	private List<GameObject> _sideMenuButtonList;

	private void Awake()
	{
		_sideMenuButtonList = new List<GameObject>();
	}

	public void Setup()
	{

		UnityHelper.DestroyObjects(_sideMenuButtonList);

		var subThemeList = Data.Theme.SubThemes;


		for (int i = 0; i < subThemeList.Length; i++)
		{

			var sideMenuButton = Instantiate(_sideMenuButtonPrefab, _buttonContainerTransform);
			sideMenuButton.GetComponent<SideMenuButton>().SetButton(subThemeList[i]);

			_sideMenuButtonList.Add(sideMenuButton);

		}
	}
}
