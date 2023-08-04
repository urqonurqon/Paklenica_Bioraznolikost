using System;
using System.IO;
using System.Text;
using CI.HttpClient;
using Novena.Controllers;
using UnityEngine;

namespace Novena.Networking.Download
{
  public static class Downloader
  {
    public static Action OnDownloadComplete;
    
    public static void DownloadGuide()
    {
      LoadingController.Instance.SetStatus("Downloading");
      Download();
    }
    
    /// <summary>
    /// Start file download
    /// </summary>
    private static void Download()
    {
      //Get current data.json
      string timeStampJson = Novena.Networking.Download.PartialUpdate.PartialUpdate.GetJson();
      
      //Save current data.json to FileState key
      PlayerPrefs.SetString($"FileState", timeStampJson);
      
      byte[] data = Encoding.UTF8.GetBytes(timeStampJson);
      
      HttpClient httpClient = new HttpClient();
      
      var multiform = new MultipartFormDataContent();
      var fileContent = new ByteArrayContent(data, "application/json");
      multiform.Add(fileContent, "json", "file.json");

      string downloadPath = Api.GetGuidePath() + ".zip";
      
      var fs = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
      string url = Api.ApiPartialUpdate + DataLoading.DataLoading.DownloadCode;

      httpClient.Post(new Uri(url), multiform, HttpCompletionOption.StreamResponseContent,
        r =>
        {
          if (r.Exception != null)
          {
            Debug.LogError(r.Exception.Message);
            httpClient.Abort();
            //Navigator.To("Admin");
          }

          if (r.IsSuccessStatusCode == false)
          {
            Debug.LogError(r.ReasonPhrase + " : " + r.ReadAsString());
            httpClient.Abort();
            //Navigator.To("Admin");
          }
          else
          {
            var stream = r.ReadAsStream();
            stream.CopyTo(fs);
        
            LoadingController.Instance.UpdateLoadingState(r.PercentageComplete, r.TotalContentRead, r.ContentLength);
        
            if (r.ContentLength == r.TotalContentRead)
            {
              fs.Dispose();
              OnDownloadComplete?.Invoke();
            }
          }
        });
    }
  }
}