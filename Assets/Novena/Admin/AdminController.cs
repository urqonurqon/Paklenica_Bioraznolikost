using Novena.Networking.Download;
using Novena.UiUtility.Base;
using Novena.Utility.Application;
using UnityEngine;
using UnityEngine.UI;

namespace Novena.Admin {
  public class AdminController : UiController {
    [Space(10)]
    public Toggle debugLogViewerToggle;

    [Space(10)]
    public GameObject reportCanvas;

    public Toggle statsManToggle;
    public GameObject statsManCanvas;

    private void Start()
    {
      EnableDebugLogViewer();
      EnableStatsMan();
    }
    public override void OnShowViewStart()
    {
      
    }
    public void EnableDebugLogViewer()
    {
      reportCanvas.SetActive(debugLogViewerToggle.isOn);
    }

    public void EnableStatsMan()
    {
      statsManCanvas.SetActive(statsManToggle.isOn);
    }

    public void ShutDown()
    {
      Debug.Log("ShutDown");
      CloseApp();
    }

    public void Restart()
    {
      Debug.Log("Restart");
      ApplicationManager.RestartApplication();
    }

    private void CloseApp()
    {
      Application.Quit();
    }

    public void UpdateData()
    {
      Downloader.DownloadGuide();
    }
  }
}