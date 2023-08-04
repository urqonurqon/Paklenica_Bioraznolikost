##IF YOU ARE WORKING ON A NEW PROJECT THATS DERIVED FROM BASE IGNORE THIS README
##IF YOU ARE WORKING ON A NEW PROJECT THATS DERIVED FROM BASE IGNORE THIS README
##IF YOU ARE WORKING ON A NEW PROJECT THATS DERIVED FROM BASE IGNORE THIS README
##IF YOU ARE WORKING ON A NEW PROJECT THATS DERIVED FROM BASE IGNORE THIS README

####EXAMPLES BELOW-------
#Add  GenerateLanguageButton call in UiController virtual OnShowViewStart method and remove/don't include base.OnShowViewStart() call
      in any UIViews you don't want 
#Add  SetThemeOnLanguageChange() and it's calls in TranslatedContent method in Data.cs
#Add  LanguageSwitchCode field in Theme.cs
#Add  GetThemeByLanguageSwitchCode method in TranslatedContent.cs
####EXAMPLES BELOW-------


####EXAMPLES IN README.PNG
#Add  On Nody Add event called "ChangeLanguage" on every View you want to change your language on.
#Add  On Nody Add a view that has 1 second delay Time Delay output called LanguageView
#Add  On Nody Add Switch Back that has a double connection between TARGET and Language View and connect ChangeLanguage with a source
      and a source back to corresponding Template (repeat for every view u want to have change language on)
####EXAMPLES IN README.PNG


In CMS there is a new field for every theme LanguageSwitchCode that should match same theme (and subthemes) in different language.
Example: 


Language: Hrvatski
Theme: Brod
LanguageSwitchCode: 10

Language: English
Theme: Ship
LanguageSwtichCode: 10


___________________________________________________UIVIEW___________________________________________________

using Doozy.Engine.UI;
using UnityEngine;

namespace Novena.UiUtility.Base
{
  /// <summary>
  /// Base class for doozy UiView event implementation.
  /// This class is standard for every controller that handles Ui.
  /// <example>
  /// Every script inheriting this must be on object with UiView component.
  /// </example>
  /// </summary>
  [RequireComponent(typeof(UIView))]
  public class UiController : MonoBehaviour
  {
    /// <summary>
    /// UiView component.
    /// </summary>
    [HideInInspector]
    public UIView UiView;

    
    public virtual void Awake()
    {
      UiView = gameObject.GetComponent<UIView>();
      
      UiView.ShowBehavior.OnStart.Event.RemoveListener(OnShowViewStart);
      UiView.ShowBehavior.OnStart.Event.AddListener(OnShowViewStart);
      
      UiView.ShowBehavior.OnFinished.Event.RemoveListener(OnShowViewFinished);
      UiView.ShowBehavior.OnFinished.Event.AddListener(OnShowViewFinished);
      
      UiView.HideBehavior.OnStart.Event.RemoveListener(OnHideViewStart);
      UiView.HideBehavior.OnStart.Event.AddListener(OnHideViewStart);
      
      UiView.HideBehavior.OnFinished.Event.RemoveListener(OnHideViewFinished);
      UiView.HideBehavior.OnFinished.Event.AddListener(OnHideViewFinished);
      
      UiView.OnVisibilityChanged.RemoveListener(OnVisibilityChanged);
      UiView.OnVisibilityChanged.AddListener(OnVisibilityChanged);
      
      UiView.OnInverseVisibilityChanged.RemoveListener(OnInverseVisibilityChanged);
      UiView.OnInverseVisibilityChanged.AddListener(OnInverseVisibilityChanged);
    }
    /// <summary>
    /// ProgressEvent executed when the view is animating (showing or hiding) and the progress has been updated.
    /// </summary>
    /// <param name="arg0">Passes the InverseVisibility (float between 1 – NotVisible and 0 – Visible). InverseVisibility = 1 – Visibility</param>
    public virtual void OnInverseVisibilityChanged(float arg0)
    {
      
    }

    /// <summary>
    /// ProgressEvent executed when the view is animating (showing or hiding) and the progress has been updated.
    /// </summary>
    /// <param name="arg0">Passes the Visibility (float between 0 – NotVisible and 1 – Visible)</param>
    public virtual void OnVisibilityChanged(float arg0)
    {
      
    }

    /// <summary>
    /// Doozy event when View Show animation start.
    /// </summary>
    public virtual void OnShowViewStart()
    {
      GetComponentInChildren<Paklenica.Language>().GenerateLanguageButtons();
    }
    
    /// <summary>
    /// Doozy event when View Show animation finish.
    /// </summary>
    public virtual void OnShowViewFinished()
    {
      
    }
    
    /// <summary>
    /// Doozy event when View Hide animation start.
    /// </summary>
    public virtual void OnHideViewStart()
    {
      
    }

    /// <summary>
    /// Doozy event when View Hide animation finish.
    /// </summary>
    public virtual void OnHideViewFinished()
    {
      Resources.UnloadUnusedAssets();
    }
  }
}




___________________________________________________DATA___________________________________________________



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
            ---------------------ADD HERE--------------------
            SetThemeOnLanguageChange();
            ---------------------ADD HERE--------------------
          }
				}
				else
				{
          s_translatedContent = value;
          OnTranslatedContentUpdated?.Invoke();
          ---------------------ADD HERE--------------------
          SetThemeOnLanguageChange();
          ---------------------ADD HERE--------------------
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
    /// When user click theme in list of themes store that in here.
    /// </example>
    /// </summary>
    public static Theme Theme { get; set; }

    ---------------------ADD HERE-----------------------------------------ADD HERE--------------------
    /// <summary>
    /// 
    /// </summary>
    private static void SetThemeOnLanguageChange()
		{
      if (Theme == null) return;

      Theme = TranslatedContent.GetThemeByLanguageSwitchCode(Theme.LanguageSwitchCode);
		}
    ---------------------ADD HERE-----------------------------------------ADD HERE--------------------

    #region Private fields

    private static TranslatedContent s_translatedContent;

    #endregion
  }
}





___________________________________________________THEME___________________________________________________







#nullable enable
using System.Linq;
using Novena.Enumerators;

namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class Theme
  {
    public int Id;
    public string Name;
    public int Rank;
    public float Longitude;
    public float Latitude;
    public float PositionX;
    public float PositionY;
    public string ImagePath;
    public string ImageTimestamp;
    public string Label;
    public int BeaconId;
    public int MapId;
    public Tag[] Tags;
    public SubTheme[] SubThemes;
    public Media[] Media;
    ---------------------ADD HERE--------------------
    public int LanguageSwitchCode;
    ---------------------ADD HERE--------------------

    /// <summary>
    /// This is thumbnail of theme
    /// </summary>
    public Image Image => GetImage();

    /// <summary>
    /// Creates Image object
    /// </summary>
    /// <returns>Image</returns>
    private Image GetImage()
    {
      return new Image { Path = ImagePath, TimeStamp = ImageTimestamp };
    }

    #region Helper methods

    public SubTheme? GetSubThemeByName(string name)
    {
      if (SubThemes.Any() == false) return null;

      return SubThemes.FirstOrDefault(sb => sb.Name == name);
    }

    /// <summary>
    /// Returns first media of requested type.
    /// </summary>
    /// <returns>Media or null if nothing found</returns>
    public Media? GetMediaByType(MediaType type)
    {
      Media? media = Media?.FirstOrDefault(m => m.MediaTypeId == (int)type);

      return media;
    }

    /// <summary>
    /// Gets media by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>Media or null if nothing found</returns>
    public Media? GetMediaByName(string name)
    {
      Media? media = Media?.FirstOrDefault(m => m.Name == name);
      
      return media;
    }

    /// <summary>
    /// Get theme tag by category name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns>Tag or NULL if nothing found!</returns>
    public Tag? GetThemeTagByCategoryName(string categoryName)
    {
      TagCategorie? tagCategorie = Data.TranslatedContent.GetTagCategoryByName(categoryName);

      if (tagCategorie == null) return null;

      Tag? tag = Tags.FirstOrDefault(tag => tag.TagCategoryId == tagCategorie.Id);

      return tag;
    }
    
    #endregion
  }
  
  public class Image
  {
    public string? Path;
    public string? TimeStamp;
  }

  [System.Serializable]
  public class SubTheme : Theme
  {
    
  }
}



___________________________________________________TRANSLATEDCONTENT___________________________________________________



#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class TranslatedContent
  {
    public int Id;
    public int LanguageId;
    public int Rank;
    public string ContentTitle;
    public string LanguageName;
    public string LanguageEnglishName;
    public string LanguageThumbnailPath;
    public string LanguageThumbnailTimestamp;
    public Theme[] Themes;
    public TagCategorie[] TagCategories;

    #region Helper methods
    ---------------------ADD HERE-----------------------------------------ADD HERE--------------------
    public Theme? GetThemeByLanguageSwitchCode(int code)
		{
      Theme? output = null;

      output = Themes.FirstOrDefault(t => t.LanguageSwitchCode == code);

      return output;
		}
    ---------------------ADD HERE-----------------------------------------ADD HERE--------------------
    /// <summary>
    /// Get theme by name.
    /// </summary>
    /// <param name="name">Theme name</param>
    /// <returns>Theme if found or null</returns>
    public Theme? GetThemeByName(string name)
    {
      Theme? output = null;

      output = Themes?.FirstOrDefault(theme => theme.Name == name);

      return output;
    }

    /// <summary>
    /// Get theme by label.
    /// </summary>
    /// <param name="label">Label name</param>
    /// <returns>Theme if found or null</returns>
    public Theme? GetThemeByLabel(string label)
    {
      Theme? output = null;
      
      output = Themes?.FirstOrDefault(theme => theme.Label == label);
      
      return output;
    }

    /// <summary>
    /// Get list of theme that have no tag or excluded tag name.
    /// </summary>
    /// <param name="excludeTagName"></param>
    /// <returns></returns>
    public List<Theme> GetThemesExcludeByTag(string excludeTagName)
    {
      List<Theme> output = new List<Theme>();

      output = Themes?.Where(t => t.Tags == null || t.Tags.Any(tag => tag.Title != excludeTagName)).ToList();

      return output;
    }

    /// <summary>
    /// Get tag category by name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns>TagCategorie or NULL if nothing found!</returns>
    public TagCategorie? GetTagCategoryByName(string categoryName)
    {
      return TagCategories?.FirstOrDefault(cat => cat.Title == categoryName);
    }

    #endregion
  }
}