using System.IO;
using UnityEngine;

namespace Novena.Networking
{
  public static class Api
  {
    public const string ApiGetGuideId =
      "http://n3guide.novena.agency/sys/api/guide/getGuideIdByDownloadCode.aspx?downloadCode=";
    public const string ApiPartialUpdate = 
      "http://n3guide.novena.agency/sys/api/guide/downloadPartial.aspx?downloadCode=";

    public static string GetGuidePath()
    {
      string path = "";
      string persistenDataPath = Application.persistentDataPath;
      
      path = Path.Combine(persistenDataPath, DataLoading.DataLoading.DownloadCode);
      
      return path;
    }
  }
}