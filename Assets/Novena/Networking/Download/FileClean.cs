using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Novena.Networking.Download.PartialUpdate;
using Novena.Utility.Unzip;
using UnityEngine;

namespace Novena.Networking.Download
{
  public class FileClean : MonoBehaviour
  {
    private string _oldStateJson = "";
    private string _newStateJson = "";

    private void Awake()
    {
      Unziper.OnUnzipComplete += OnUnzipComplete;
    }

    private void OnUnzipComplete()
    {
      DeleteUnusedFiles();
    }

    /// <summary>
    /// This is invoked on OnUnzipComplete event
    /// </summary>
    private void DeleteUnusedFiles()
    {
      //Get old state that was saved before download started
      _oldStateJson = PlayerPrefs.GetString($"FileState");
      //Get new state after download completed
      _newStateJson = Novena.Networking.Download.PartialUpdate.PartialUpdate.GetJson();

      if (string.IsNullOrWhiteSpace(_oldStateJson))
      {
        Debug.Log("Old file state doesn't exist! Cannot CompareAndDelete");
      }
      else
      {
        CompareAndDelete();
      }
    }

    /// <summary>
    /// Takes list of old and new file paths. Compare and delete files that are not necessary
    /// </summary>
    private void CompareAndDelete()
    {
      var newStatePud = JsonConvert.DeserializeObject<PartialUpdateData>(_newStateJson);
      var oldStatePud = JsonConvert.DeserializeObject<PartialUpdateData>(_oldStateJson);
      
      if (oldStatePud.partialUpdateData == null) return;
      
      foreach (var oldPud in oldStatePud.partialUpdateData)
      {
        if (newStatePud.partialUpdateData.Any(pud=>pud.Path == oldPud.Path) == false)
        {
          string filePath = Application.persistentDataPath + "/" + DataLoading.DataLoading.DownloadCode + oldPud.Path;
          filePath = filePath.Replace("~", "").Replace("%20", " ");

          Debug.Log("Old file to delete: " + Path.GetFileName(filePath));

          if (File.Exists(filePath))
          {
            File.Delete(filePath);
          }
          else
          {
            Debug.Log("File doesn't exist or wrong file path: " + filePath);
          }
        }
      }
    }
  }
}