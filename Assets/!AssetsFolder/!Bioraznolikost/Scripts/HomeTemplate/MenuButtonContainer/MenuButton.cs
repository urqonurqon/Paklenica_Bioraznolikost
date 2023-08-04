#nullable enable
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System;
using TMPro;
using UnityEngine;

namespace _AssetsFolder.BaseExample.Scripts {
	public class MenuButton : MonoBehaviour {
		public static Action<string>? OnClick;

		[Header("Components")]
		[SerializeField] private TMP_Text _label;
		[SerializeField] private TMP_Text _title;

		public void SetButton(Theme theme)
		{
			var textButtonColor = GetComponent<TextButtonColor>();

			_label.text = theme.Label;
			_title.text = theme.Name;

			UIButton btn = gameObject.GetComponent<UIButton>();
			btn.OnClick.OnTrigger.Event.AddListener(() => {
				Tag? themeTag = theme.GetThemeTagByCategoryName("TEMPLATE");
				Data.Theme = theme;
				foreach (var theme in Data.TranslatedContent.GetThemesExcludeByTag("SYSTEM"))
				{
					theme.isVisited = false;
					textButtonColor.RecolorObjectsInList();
				}
				Data.Theme.isVisited = true;
				textButtonColor.ColorObjectsInList();
				OnClick?.Invoke(themeTag.Title);
			});
		}
	}
}