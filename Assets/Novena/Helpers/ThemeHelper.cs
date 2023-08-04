using System.Collections.Generic;
using System.Linq;
using Novena.DAL.Model.Guide;

namespace Scripts.Helpers
{
  public static class ThemeHelper
  {
    public static List<Theme> themeList { get; set; }

    /// <summary>
    /// Get list of all themes. Excluding theme with tag name "SYSTEM" by default.
    /// </summary>
    /// <param name="includeSystemTheme">Set true if theme with tag name SYSTEM is needed</param>
    /// <returns>List of themes</returns>
    public static List<Theme> GetThemeList(bool includeSystemTheme = false)
    {
      List<Theme> themes = themeList.Where(t=>t.Tags == null || (t.Tags != null && t.Tags.Any(tag=>tag.Title != "SYSTEM"))).ToList();;
      
      if (includeSystemTheme)
      {
        themes = themeList;
      }
      
      return themes;
    }
    /// <summary>
    /// Helper class. Contains functions that return bool
    /// </summary>
    public static class Check
    {
      public static bool ContainsTag(Theme theme, string name)
      {
        bool output = false;
        if (theme.Tags == null) return output;
        output = theme.Tags.Any(tag => tag.Title == name);
        return output;
      }

      public static bool ContainsMedia(Theme theme, string name)
      {
        bool output = false;
        if (theme.Media == null) return output;
        output = theme.Media.Any(media => media.Name == name);
        return output;
      }

      public static bool ContainsPartOfMediaName(Theme theme, string name)
      {
        bool output = false;
        if (theme.Media == null) return output;
        output = theme.Media.Any(media => media.Name.Contains(name));
        return output;
      }

    }


    public static class Get
    {
			public static Theme GetThemeByLabel(string label)
      {
        Theme theme = themeList.FirstOrDefault(x => x.Label == label);
        return theme;
      }
      
      /// <summary>
      /// Filter themes by Name. Returns first match in list. Null if none found.
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static Theme GetThemeByName(string name)
      {
        Theme theme = null;
        
        if (themeList != null)
        {
          theme = themeList.FirstOrDefault(x => x.Name == name);
        }
        
        return theme;
      }

      public static Theme GetThemeById(int id)
      {
        Theme theme = themeList.FirstOrDefault(x => x.Id == id);
        return theme;
      }

      public static Theme GetThemeByTag(string tagName)
      {
        Theme output = null;

        output = themeList.FirstOrDefault(t => t.Tags.Any(tag => tag.Title == tagName));
        
        return output;
      }

      /// <summary>
      /// Filter themes that contains tag. Filter by tag name.
      /// </summary>
      /// <param name="tagName"></param>
      /// <returns></returns>
      public static List<Theme> GetThemesByTag(string tagName)
      {
        List<Theme> themes = null;

        themes = themeList.Where(t => t.Tags != null && t.Tags.Any(tag => tag.Title == tagName)).ToList();
        
        return themes;
      }

      public static List<Theme> GetThemesByLabel(string label)
      {
        IEnumerable<Theme> themes = themeList.Where(x => x.Label == label);
        return themes.ToList<Theme>();
      }

      public static List<Theme> GetThemesByLabelExcluding(string label)
      {
        IEnumerable<Theme> themes = themeList.Where(x => x.Label != label);
        return themes.ToList<Theme>();
      }

      /// <summary>
      /// Get all subthemes. Returns null if none found.
      /// </summary>
      /// <param name="theme"></param>
      /// <returns></returns>
      public static List<Theme> GetSubThemes(Theme theme)
      {
        List<Theme> subthemes = null;

        if (theme.SubThemes != null)
        {
          //subthemes = theme.SubThemes.ToList();
        }
        
        return subthemes;
      }
    }
  }
}
