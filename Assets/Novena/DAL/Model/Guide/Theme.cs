#nullable enable
using System.Collections.Generic;
using System.Linq;
using Novena.Enumerators;
using Novena.Networking;

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
    public int LanguageSwitchCode;
    //custom property
    public bool isVisited;

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

    public List<SubTheme> GetSubThemesByTag(string tagName)
    {
      List<SubTheme> subThemes = null;

      if (SubThemes.Any() == false) return null;

      subThemes = SubThemes.Where(t => t.Tags != null && t.Tags.Any(tag => tag.Title == tagName)).ToList();

      return subThemes;
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