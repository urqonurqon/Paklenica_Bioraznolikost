using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Novena.Networking;

namespace Novena.DAL.Model.Guide
{
  [Serializable]
  public class Media
  {
    public int Id;
    public string Name;
    public int Rank;
    public int MediaTypeId;
    public Photo[] Photos;
    public string Text;
    public string ContentPath;
    public string ContentTimestamp;
    public int ContentSize;

    private string _fullPath;

    /// <summary>
    /// Get full path to file either on internet or local file.
    /// </summary>
    public string FullPath 
    { get=> GetFullPath();
      protected set => _fullPath = value;
    }
    
    /// <summary>
    /// Get full path to file.
    /// </summary>
    /// <returns>File url. Removes ~ from ContentPath. Empty string if ContentPath is null!</returns>
    private string GetFullPath()
    {
      string output = "";

      if (string.IsNullOrWhiteSpace(ContentPath) == false)
      {
        output = Api.GetGuidePath() + ContentPath.Replace("~", "");
      }
      
      return output;
    }
    

    #region Helper methods

    /// <summary>
    /// Get list of photos ordered by rank!
    /// </summary>
    /// <returns>Ordered list of photos or NULL if nothing found.</returns>
    [CanBeNull]
    public List<Photo> GetPhotos()
    {
      if (Photos == null) return null;
      
      var orderedPhotos = Photos.OrderBy(photo => photo.Rank).ToList();
      
      return orderedPhotos;
    }

    #endregion
  }

  [Serializable]
  public class Photo
  {
    public int Id;
    public int Rank;
    public string Name;
    public string Path;
    public string Timestamp;
    public int Size;
    public string Description;
    
    private string _fullPath;
    
    /// <summary>
    /// Get full local path to image.
    /// </summary>
    public string FullPath
    {
      get => GetFullPath();
      protected set => _fullPath = value;
    }

    private string GetFullPath()
    {
      return Api.GetGuidePath() + Path.Replace("~", "");
    }
  }
}