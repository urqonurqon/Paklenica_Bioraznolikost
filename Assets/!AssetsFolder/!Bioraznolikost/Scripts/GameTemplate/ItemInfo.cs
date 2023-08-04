using CGH;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfo : MonoBehaviour, IPointerClickHandler {

	private DragDrop _dragDrop;

	public UIToggle UIToggle;

	[SerializeField] private CanvasGroup _names;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _latinName;

	[HideInInspector] public Theme MapTheme;
	[HideInInspector] public Theme Theme;

	private void Awake()
	{
		_dragDrop = GetComponent<DragDrop>();
		UIToggle.OnValueChanged.AddListener((isOn) => ValueChanged(isOn));
	}

	public void SetName()
	{
		if (MapTheme.GetMediaByName("LatinName") != null)
		{
			_name.text = MapTheme.Name;
			_latinName.text = MapTheme.GetMediaByName("LatinName").Text;
		}
		else
		{
			_name.text = Theme.Name;
			_latinName.text = Theme.GetMediaByName("LatinName").Text;

		}
	}

	private void ValueChanged(bool isOn)
	{
		if (isOn)
		{
			NameVisibility(true);
		}
		else
		{
			NameVisibility(false);
		}
	}

	private void NameVisibility(bool isVisible)
	{
		_names.Fade(isVisible ? 1 : 0, 1);
	}

	public void OnPointerClick(PointerEventData eventData)
	{

		if (_dragDrop.ShowInfo)
		{
			if (UIToggle.IsOn)
			{
				UIToggle.IsOn = false;
			}
			else
			{
				UIToggle.IsOn = true;
			}


			_dragDrop.ShowInfo = false;
		}

	}
}
