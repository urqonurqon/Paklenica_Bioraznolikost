using System.Collections.Generic;
using Novena.Helpers;
using Novena.Settings;
using Novena.UiUtility.Base;
using Scripts.Helpers;
using UnityEngine;

namespace Novena.Admin
{
  public class SettingsController : UiController
  {
    [SerializeField] private SettingsItem _settingsItem;
    [SerializeField] private Transform _itemsContainer;

    private SettingsManager _settingsManager;
    private List<SettingsItem> _settingsItems = new List<SettingsItem>();
    
    public override void Awake()
    {
      _settingsManager = FindObjectOfType<SettingsManager>();
      base.Awake();
    }

    public override void OnShowViewStart()
    {
      Setup();
    }

    private void Setup()
    {
      GenerateSettingsItems();
    }

    private void GenerateSettingsItems()
    {
      UnityHelper.DestroyObjectsScripts(_settingsItems);
      var settingItems = _settingsManager.SettingItems;

      foreach (var settingItem in settingItems)
      {
        var si = Instantiate(_settingsItem, _itemsContainer);
        si.Setup(settingItem);
        _settingsItems.Add(si);
      }
    }

    public void OnButtonSave_Click()
    {
      foreach (var settingsItem in _settingsItems)
      {
        settingsItem.SaveChanges();
      }
      
      _settingsManager.SaveSettings();
      Debug.Log("SETTINGS SAVED");
    }
  }
}