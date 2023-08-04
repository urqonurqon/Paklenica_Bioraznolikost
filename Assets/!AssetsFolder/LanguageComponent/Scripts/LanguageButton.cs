using Doozy.Engine;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Paklenica
{
  public class LanguageButton : MonoBehaviour
  {
    [Header("Components")]


    [SerializeField] private TMP_Text label;
    [SerializeField] private Color textColorActive;
    [SerializeField] private Color textColorInactive;

    private UIToggle _toggle;
    private TranslatedContent _translatedContent;

    private void Awake()
    {
			_toggle = gameObject.GetComponent<UIToggle>();
			_toggle.OnClick.OnToggleOff.Event.AddListener(() => OnButtonClick(false));
			_toggle.OnClick.OnToggleOn.Event.AddListener(() => OnButtonClick(true));
		}

    public void Setup(TranslatedContent translatedContent)
    {
      _translatedContent = translatedContent;
      _toggle.Toggle.group = gameObject.transform.parent.GetComponent<ToggleGroup>();
      label.text = translatedContent.ContentTitle;

      SetState(Data.TranslatedContent.Id == _translatedContent.Id);
    }

    private void SetState(bool state)
    {
      label.color = textColorInactive;
      _toggle.IsOn = state;

			if (state)
			{
        label.color = textColorActive;
			}
		}

    public void OnButtonClick(bool state)
    {
      if (state)
      {
        if (Data.TranslatedContent.Id != _translatedContent.Id)
        {
          Data.TranslatedContent = _translatedContent;
          GameEventMessage.SendEvent("ChangeLanguage");
        }
      }

      SetState(state);
    }
  }
}