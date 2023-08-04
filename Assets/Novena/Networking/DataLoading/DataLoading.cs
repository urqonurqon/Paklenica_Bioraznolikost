using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.Networking.Download;
using Novena.Networking.Download.Helpers;
using Novena.Utility.Map;
using Novena.Utility.Unzip;
using UnityEngine;

namespace Novena.Networking.DataLoading
{
#if UNITY_EDITOR
  using UnityEditor;

  [CustomEditor(typeof(DataLoading))]
  public class DownloadButton : Editor {
    public override void OnInspectorGUI()
    {

      if (GUILayout.Button("Download Content"))
      {
        Downloader.DownloadGuide();
      }

      base.OnInspectorGUI();
    }
  }
#endif



  public class DataLoading : MonoBehaviour
  {
    public static DataLoading Instance;

    public static Action OnGuideLoaded;
    
    public static string DownloadCode { get; private set; }
    
    [Header("Guide data")] 
    [SerializeField] private string downloadCode = "r5NMtmxN"; //default test base guide

    private void Awake()
    {
      Instance = this;
      DownloadCode = downloadCode;
      Unziper.OnUnzipComplete += LoadGuideData;
    }
    
    public void CheckGuideState()
    {
      if (DownloadHelper.CheckIfGuideExist(downloadCode))
      {
        LoadGuideData();
      }
      else
      {
        Downloader.DownloadGuide();
      }
    }
    
    public void LoadGuideData()
    {
     LoadDataFiles();
    }

    private void LoadDataFiles()
    {
      string path = Api.GetGuidePath() + "/data.json";
      TextReader textReader = File.OpenText(path);
      string jsonData = textReader.ReadToEnd();
      textReader.Dispose();
      
      jsonData = jsonData.Replace("~/files", "/files");
      
      var guide = JsonConvert.DeserializeObject<Guide>(jsonData);

      //Load Maps static class
      if (guide.Maps != null)
      {
        Maps.MapList = guide.Maps.ToList();
      }

      Data.Guide = guide;

      if (guide.TranslatedContents != null)
      {
        Data.TranslatedContent = guide.TranslatedContents[0];
      }
    }
  }
}