using Novena.Settings;
using TMPro;
using UnityEngine;

namespace Novena.Admin
{
  public class SettingsItem : MonoBehaviour
  {
    [SerializeField] private TMP_Text _labelTmp;
    [SerializeField] private TMP_InputField _valueInputField;
    [SerializeField] private TMP_Text _descriptionTmp;

    public SettingItem SettingItem { get; private set; }
    
    public void Setup(SettingItem settingItem)
    {
      SettingItem = settingItem;
      _labelTmp.text = settingItem.Name + ":";
      _descriptionTmp.text = settingItem.Description;
      _valueInputField.text = settingItem.Value;
    }

    public void SaveChanges()
    {
      SettingItem.Value = _valueInputField.text;
    }
    
    
  }
}