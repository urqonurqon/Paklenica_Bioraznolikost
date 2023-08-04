using System.Collections.Generic;
using System.Linq;
using Novena.DAL.Model.Guide;
using Novena.Enumerators;
using Novena.Networking;
using UnityEngine;

namespace Novena.Helpers {
	public static class MediaHelper{

    public static class Get
    {
      /// <summary>
      /// Get media text or file path. Video and Audio full file path.
      /// </summary>
      /// <param name="theme">Theme object</param>
      /// <param name="name">Media name</param>
      /// <param name="type">Media type</param>
      /// <returns>Empty string if nothing found</returns>
			public static string GetMedia(Theme theme, string name, MediaType type)
      {
        string text = "";

        if (theme.Media == null)
        {
          Debug.LogWarning($"GetMedia: Theme ({theme.Name}) doesnt contain any Media!" );
          return text;
        }
        
        if (theme.Media.Any(x => x.Name == name))
        {
          switch (type)
          {
            case MediaType.Text:
              text = theme.Media.FirstOrDefault(x => x.Name == name).Text;
              break;
            case MediaType.Video:
              text = theme.Media.FirstOrDefault(x => x.Name == name).ContentPath;
              text = text != "" ? Api.GetGuidePath() + text : "";
              break;
            case MediaType.Audio:
              text = theme.Media.FirstOrDefault(x => x.Name == name).ContentPath;
              text = text != "" ? Api.GetGuidePath() + text : "";
              break;
          }
        }
        else
        {
          Debug.LogWarning($"GetMedia: Media ({name}) in theme ({theme.Name}) doesnt exist!" );
        }
        return text;
      }

      public static string GetARMediaName(Theme theme)
      {
        string text = "";

        if (theme.Media == null)
          return text;

        text = theme.Media.FirstOrDefault(x => x.MediaTypeId == (int)MediaType.AR).Name;

        return text;
      }

      public static List<string> GetAllMediaContainingPartOfName(Theme theme, string partOfName, MediaType type)
      {
        List<string> mediaList = new List<string>();

        if (theme.Media == null)
        {
          return mediaList;
        }

        string text = "";
        string guidePath = Api.GetGuidePath();

        if (theme.Media.Any(x => x.Name.Contains(partOfName)))
        {
          for (int i = 0; i < theme.Media.Length; i++)
          {
            if (theme.Media[i].MediaTypeId == (int)type && theme.Media[i].Name.Contains(partOfName))
            {
              switch (type)
              {
                case MediaType.Text:
                  mediaList.Add(theme.Media[i].Text);
                  break;
                case MediaType.Video:
                  text = theme.Media[i].ContentPath;
                  text = text != "" ? guidePath + text : "";
                  mediaList.Add(text);
                  break;
                case MediaType.Audio:
                  text = theme.Media[i].ContentPath;
                  text = text != "" ? guidePath + text : "";
                  mediaList.Add(text);
                  break;
              }
            }
          }
        }

        return mediaList;
      }

      /// <summary>
      /// Get list of photos with full path to file.
      /// </summary>
      /// <param name="theme">Theme object</param>s
      /// <param name="name">Media name</param>
      /// <returns>Empty list if nothing found</returns>
      public static List<Photo> GetMediaPhotos(Theme theme, string name)
      {
        List<Photo> photos = new List<Photo>();
        List<Photo> output = new List<Photo>();
        
        if (theme.Media == null)
        {
          Debug.LogWarning($"GetMediaPhotos: Theme ({theme.Name}) doesnt contain any Media!" );
          return output;
        }

        if (theme.Media.Any(x => x.Name == name && x.MediaTypeId == (int) MediaType.Gallery))
        {
          if (theme.Media.FirstOrDefault(x => x.Name == name && x.MediaTypeId == (int) MediaType.Gallery).Photos != null)
          {
            photos = theme.Media.FirstOrDefault(x => x.Name == name && x.MediaTypeId == (int) MediaType.Gallery).Photos.ToList();
            string guidePath = Api.GetGuidePath();
            
            foreach (var photo in photos)
            {
              output.Add(photo);
            }
          }
          else
          {
            Debug.LogWarning($"GetMediaPhotos: Media ({name}) in theme ({theme.Name}) doesnt contain photos!" );
          }
        }
        else
        {
          Debug.LogWarning($"GetMediaPhotos: Media ({name}) in theme ({theme.Name}) doesnt exist!" );
        }
        return output;
      }
    }

    public static class Check
    {


		}

  }
}
