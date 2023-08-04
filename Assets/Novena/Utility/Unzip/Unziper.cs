using System;
using System.IO;
using Novena.Controllers;
using Novena.Networking;
using Novena.Networking.Download;
using Scripts.Controllers;
using Scripts.Utility;
using UnityEngine;

namespace Novena.Utility.Unzip
{
  public  class Unziper : MonoBehaviour
  {
    public static Action OnUnzipComplete;
    
    private static int[] _progress = new int[1];
    private static int _totalFiles;
    private static bool _unzipStarted;

    private void Awake()
    {
      Downloader.OnDownloadComplete += OnDownloadComplete;
    }

    private void OnDownloadComplete()
    {
      Unzip();
    }

    public static void Unzip()
    {
      LoadingController.Instance.SetStatus("UNZIP");
      _unzipStarted = true;
      string filePath = Api.GetGuidePath() + ".zip";
      string folder = Api.GetGuidePath() + "/";
      DoDecompression(filePath, folder);
    }
    
    private static void DoDecompression(string file, string folder)
    {
      _totalFiles = lzip.getTotalFiles(file);
      var _lzip = lzip.decompress_File(file, folder, _progress, null, null);
    }
    
    private void Update()
    {
      if (_unzipStarted)
      {
        float percentFloat = _progress[0] / _totalFiles;
        int percent = (int) (percentFloat * 100);
        LoadingController.Instance.UpdateLoadingState(percent);
        
        if (_progress[0] == _totalFiles)
        {
          _unzipStarted = false;
          OnUnzipComplete?.Invoke();
          DeleteZipFile();
        }
      }
    }

    private void DeleteZipFile()
    {
      string filePath = Api.GetGuidePath() + ".zip";

      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }
      else
      {
        Debug.LogWarning("DeleteZipFile: File doesn't exist = " + filePath);
      }
    }
  }
}
