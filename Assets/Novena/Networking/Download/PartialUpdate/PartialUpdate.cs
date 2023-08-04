using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Novena.DAL.Model.Guide;

namespace Novena.Networking.Download.PartialUpdate
{
  public static class PartialUpdate
  {
    /// <summary>
    /// Get json of timestamps. If current json doesnt exist return empty json. That is necessary for server post request! 
    /// </summary>
    /// <returns></returns>
    public static string GetJson()
    {
      string currentJsonPath = Api.GetGuidePath() + "/data.json";
      Guide guide = new Guide();
      if (File.Exists(currentJsonPath))
      {
        TextReader textReader = File.OpenText(currentJsonPath);
        string json = textReader.ReadToEnd();
        textReader.Dispose();

        guide = JsonConvert.DeserializeObject<Guide>(json);
      }
      PartialUpdateData pud = new PartialUpdateData();
      return CreateJson(guide, ref pud);
    }
    
    /// <summary>
    /// Create json string with file paths and timestamps.
    /// </summary>
    public static string CreateJson(Guide _guideModel, ref PartialUpdateData puda)
    {
      string output = "";
      var guide = _guideModel;
      List<PartialUpdateModel> _partialUpdateModels = new List<PartialUpdateModel>();

      if (guide.Maps != null)
      {
        foreach (var map in guide.Maps)
        {
          if (string.IsNullOrWhiteSpace(map.ImagePath) == false)
          {
            PartialUpdateModel puMap = new PartialUpdateModel();
            puMap.Path = map.ImagePath;
            puMap.Timestamp = map.ImageTimestamp;
            _partialUpdateModels.Add(puMap);
          }
        }
      }

      //Guide start
      if (guide.TranslatedContents != null)
      {
        foreach (var tc in guide.TranslatedContents)
        {
          PartialUpdateModel puTc = new PartialUpdateModel();
          
          if (string.IsNullOrWhiteSpace(tc.LanguageThumbnailPath) == false)
          {
            puTc.Path = tc.LanguageThumbnailPath;
            puTc.Timestamp = tc.LanguageThumbnailTimestamp;
            _partialUpdateModels.Add(puTc);
          }
          
          //THEME - START
          
          if (tc.Themes == null) break;
          
          foreach (var theme in tc.Themes)
          {
            PartialUpdateModel puTheme = new PartialUpdateModel();

            if (string.IsNullOrWhiteSpace(theme.ImagePath) == false)
            {
              puTheme.Path = theme.ImagePath;
              puTheme.Timestamp = theme.ImageTimestamp;
              _partialUpdateModels.Add(puTheme);
            }
            
            //SUBTHEME - START
            if (theme.SubThemes != null)
            {
              foreach (var subTheme in theme.SubThemes)
              {
                PartialUpdateModel puSubTheme = new PartialUpdateModel();

                if (string.IsNullOrWhiteSpace(subTheme.ImagePath) == false)
                {
                  puSubTheme.Path = subTheme.ImagePath;
                  puSubTheme.Timestamp = subTheme.ImageTimestamp;
                  _partialUpdateModels.Add(puSubTheme);
                }
                
                if (subTheme.Media != null)
                {
                  //SUBTHEME MEDIA
                  foreach (var subThemeMedia in subTheme.Media)
                  {
                    PartialUpdateModel puSubThemeMedia = new PartialUpdateModel();

                    if (string.IsNullOrWhiteSpace(subThemeMedia.ContentPath) == false)
                    {
                      puSubThemeMedia.Path = subThemeMedia.ContentPath;
                      puSubThemeMedia.Timestamp = subThemeMedia.ContentTimestamp;
                      _partialUpdateModels.Add(puSubThemeMedia);
                    }
                    
                    //PHOTOS
                    if (subThemeMedia.Photos != null)
                    {
                      foreach (var photo in subThemeMedia.Photos)
                      {
                        PartialUpdateModel puThemeMediaPhoto = new PartialUpdateModel();

                        if (string.IsNullOrWhiteSpace(photo.Path) == false)
                        {
                          puThemeMediaPhoto.Path = photo.Path;
                          puThemeMediaPhoto.Timestamp = photo.Timestamp;
                          _partialUpdateModels.Add(puThemeMediaPhoto);
                        }
                      }
                    }
                  }
                  //SUBTHEME MEDIA
                }
              }
            }//SUBTHEME - END
            
            //THEME MEDIA - START
            if (theme.Media != null)
            {
              foreach (var media in theme.Media)
              {
                PartialUpdateModel puThemeMedia = new PartialUpdateModel();
                if (string.IsNullOrWhiteSpace(media.ContentPath) == false)
                {
                  puThemeMedia.Path = media.ContentPath;
                  puThemeMedia.Timestamp = media.ContentTimestamp;
                  _partialUpdateModels.Add(puThemeMedia);
                }
                
                //PHOTOS
                if (media.Photos != null)
                {
                  foreach (var photo in media.Photos)
                  {
                    PartialUpdateModel puThemeMediaPhoto = new PartialUpdateModel();
                    if (string.IsNullOrWhiteSpace(photo.Path) == false)
                    {
                      puThemeMediaPhoto.Path = photo.Path;
                      puThemeMediaPhoto.Timestamp = photo.Timestamp;
                      _partialUpdateModels.Add(puThemeMediaPhoto);
                    }
                  }
                } //PHOTOS
              }
            }//THEME MEDIA - END
          }//THEME - END
        }
        
        PartialUpdateData pud = new PartialUpdateData();
        pud.partialUpdateData = _partialUpdateModels.ToArray();
        puda = pud;
        output = JsonConvert.SerializeObject(pud);
      }//Guide end
      else
      {
        PartialUpdateData partialUpdateData = new PartialUpdateData();
        output = JsonConvert.SerializeObject(partialUpdateData);
        puda = partialUpdateData;
      }

      return output;
    }
  }
}