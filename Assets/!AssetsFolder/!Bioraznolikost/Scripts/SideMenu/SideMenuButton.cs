using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.Enumerators;
using Novena.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SideMenuButton : MonoBehaviour
{
  public static Action<string> OnClick;

  [Header("Components")]
  [SerializeField] private TMP_Text _themeTitle;
  [SerializeField] private TMP_Text _themeSubtitle;

  public void SetButton(Theme subTheme)
  {
    _themeTitle.text = subTheme.Name;
    _themeSubtitle.text = subTheme.GetMediaByName("SubTitle").Text;

    UIButton btn = gameObject.GetComponent<UIButton>();
    btn.OnClick.OnTrigger.Event.AddListener(() =>
    {
      Tag? subThemeTag = subTheme.GetThemeTagByCategoryName("TEMPLATE");
      OnClick?.Invoke(subThemeTag.Title);

      Data.SubTheme = subTheme;
    });
  }
}
