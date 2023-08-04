using Doozy.Engine;
using Doozy.Engine.Progress;
using Novena.Networking.Download;
using Novena.UiUtility.Base;
using Novena.Utility.Unzip;
using TMPro;
using UnityEngine;

namespace Novena.Controllers {
  public class LoadingController : UiController {
    public static LoadingController Instance;

    [Header("Components")]
    [SerializeField] private Progressor progressor;
    [SerializeField] private TMP_Text currentMbText;
    [SerializeField] private TMP_Text totalMbText;
    [SerializeField] private TMP_Text statusText;

    public override void Awake()
    {
      base.Awake();
      Instance = this;
      Unziper.OnUnzipComplete += OnDownloadComplete;
    }
    public override void OnShowViewStart()
    {

    }
    public override void OnShowViewFinished()
    {
      base.OnShowViewFinished();
      Downloader.DownloadGuide();
    }

    private void OnDownloadComplete()
    {
      GameEventMessage.SendEvent("GoToInit");
    }

    public void UpdateLoadingState(int percentage, long currentDownloaded, long totalToDownload)
    {
      progressor.SetValue(percentage);
      currentMbText.text = (currentDownloaded / 1048576.00).ToString("0.00");
      totalMbText.text = (totalToDownload / 1048576.00).ToString("0.00");
    }

    public void UpdateLoadingState(int percentage)
    {
      progressor.SetValue(percentage);
      currentMbText.text = "";
      totalMbText.text = "";
    }

    public void SetStatus(string status)
    {
      statusText.text = status;
    }
  }
}