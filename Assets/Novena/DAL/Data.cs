using System;
using Novena.DAL.Model.Guide;

namespace Novena.DAL
{
  /// <summary>
  /// Utility class for storing objects in use (Guide, Theme...)
  /// </summary>
  public static class Data
  {
    #region Events

    /// <summary>
    /// Invoked when translated content is changed!
    /// </summary>
    public static Action OnTranslatedContentUpdated;

    #endregion

    /// <summary>
    /// Current guide type. Long (true) or short.
    /// </summary>
    public static bool GuideType { get; set; }

    public static Guide Guide { get; set; }


    /// <summary>
    /// Current translated content (language).
    /// <para>
    /// Use this to store and get current selected language.
    /// </para>
    /// <example>
    /// When user click language button store that in here.
    /// </example>
    /// </summary>
    public static TranslatedContent TranslatedContent
    {
      get { return s_translatedContent; }
      set
      {
        if (s_translatedContent != null)
				{
					if (value.Id != s_translatedContent.Id)
					{
            s_translatedContent = value;
            OnTranslatedContentUpdated?.Invoke();
            SetThemeOnLanguageChange();
          }
				}
				else
				{
          s_translatedContent = value;
          OnTranslatedContentUpdated?.Invoke();
          SetThemeOnLanguageChange();
        }

        s_translatedContent = value;
      }
    }

    /// <summary>
    /// Current theme.
    /// <para>
    /// Use this to store and get current theme.
    /// </para>
    /// <example>
    /// When user clicks a theme in list of themes store that in here.
    /// </example>
    /// </summary>
    public static Theme Theme { get; set; }

    /// <summary>
    /// Current subtheme.
    /// <para>
    /// Use this to store and get current subtheme.
    /// </para>
    /// <example>
    /// When user clicks a subtheme in list of subthemes store that in here.
    /// </example>
    /// </summary>
    public static Theme SubTheme { get; set; }


    /// <summary>
    /// 
    /// </summary>
    private static void SetThemeOnLanguageChange()
		{
      if (Theme == null) return;

      Theme = TranslatedContent.GetThemeByLanguageSwitchCode(Theme.LanguageSwitchCode);
		}


    #region Private fields

    private static TranslatedContent s_translatedContent;

    #endregion
  }
}