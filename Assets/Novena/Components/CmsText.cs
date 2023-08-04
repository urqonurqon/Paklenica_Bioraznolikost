using Novena.DAL;
using Novena.Enumerators;
using TMPro;
using UnityEngine;

namespace Novena.Components
{
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CmsText : MonoBehaviour
  {
    [SerializeField] private string _themeName;
    [Tooltip("If Subtheme name is empty it search media only in Theme")]
    [SerializeField] private string  _subThemeName;
    [Tooltip("If media name is empty it takes first media of type Text!")]
    [SerializeField] private string _mediaName;

    private TMP_Text _textMeshPro;

    private void Awake()
    {
      _textMeshPro = GetComponent<TMP_Text>();
      Data.OnTranslatedContentUpdated -= OnTranslatedContentUpdated;
      Data.OnTranslatedContentUpdated += OnTranslatedContentUpdated;

      //For CmsText objects that are enabled after OnTranslatedContentUpdate triggered!
      if (Data.TranslatedContent != null)
      //if (Data.TranslatedContent.Id > 0)
      {
        SetText();
      }
    }

    /// <summary>
    /// When language is changed lets update cmsText
    /// </summary>
    private void OnTranslatedContentUpdated()
    {
      SetText();
    }
    
    private void SetText()
    {
      _textMeshPro.text = "";
      
      var theme = Data.TranslatedContent.GetThemeByName(_themeName);
      
      if(theme == null) return;

      if (string.IsNullOrEmpty(_subThemeName) == false)
      {
        theme = theme.GetSubThemeByName(_subThemeName);
        
        if(theme == null) return;
      }
      
      var media = _mediaName == "" ? theme.GetMediaByType(MediaType.Text) : theme.GetMediaByName(_mediaName);

      if (media != null)
      {
        _textMeshPro.text = media.Text;
      }
    }
  }
}