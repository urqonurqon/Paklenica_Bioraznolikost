using Novena.DAL;
using Novena.UiUtility.Base;
using UnityEngine;
using Assets.Scripts.Components.PinchScrollRect;
using DG.Tweening;
using System;
using Doozy.Engine.UI;
using Novena.DAL.Model.Guide;
using CGH;
using Scripts.Helpers;
using System.Collections.Generic;

public class MapTemplate : UiController {

	public static Action OnBackButtonClick;
	[SerializeField] public UIButton BackButton;

	[SerializeField] private PinchableScrollRect _pinchableScrollRect;
	[SerializeField] private PinController _pinController;

	[SerializeField] private RectTransform _legend;

	[SerializeField] private GameObject[] maps;



	public override void Awake()
	{
		base.Awake();
		_legend.anchoredPosition = new Vector2(_legend.anchoredPosition.x, _legend.anchoredPosition.y + 373);
		Data.OnTranslatedContentUpdated += FixLegendPosition;
		BackButton.OnClick.OnTrigger.Event.AddListener(() => {
			RaycastBlocker.BlockRaycasts();
			_pinController.ResetMap();
			_legend.DOAnchorPosY(-786.2f, 1f).OnComplete(OnBackButtonClick.Invoke);
		});
	}

	private void FixLegendPosition()
	{
		_legend.DOAnchorPosY(-786.2f, 1f);
	}

	public override void OnShowViewStart()
	{
		base.OnShowViewStart();
		_legend.DOAnchorPos(new Vector2(_legend.anchoredPosition.x,-786.2f + 373), 1f).From(new Vector2(_legend.anchoredPosition.x, -786.2f));
		_pinchableScrollRect.activate = true;


		var currentMapTag = Data.Theme.GetThemeTagByCategoryName("MAPS").Title;
		var themesWithMapTag = Data.TranslatedContent.GetThemesByTag(currentMapTag);
		Theme mapTheme = null;
		foreach (var themeWithMapTag in themesWithMapTag)
		{
			if (themeWithMapTag != Data.Theme)
				mapTheme = themeWithMapTag;
		}

		_pinController.Pins.Clear();
		_pinController.Territories.Clear();
		foreach (var map in maps)
		{
			map.SetActive(false);
			if (map.name == currentMapTag)
			{
				map.SetActive(true);
				foreach (Transform child in map.transform)
				{
					if (child.name.Contains("Pin"))
					{
						_pinController.Pins.Add(child.gameObject);
					}
					else
					{
						_pinController.Territories.Add(child.gameObject.GetComponent<CanvasGroup>());
					}
				}
			}
		}
		
		_pinController.ConnectPinsWithThemes(mapTheme);

	}
}
