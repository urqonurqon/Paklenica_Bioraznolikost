using Novena.DAL;
using Novena.UiUtility.Base;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace _AssetsFolder.BaseExample.Controllers
{
  public class VideoTemplate : UiController
  {
    [Header("Components")] 
    [SerializeField] private MediaPlayer _mediaPlayer;

    public override void OnShowViewStart()
    {
      SetVideo();
    }

    public override void OnHideViewStart()
    {
      _mediaPlayer.CloseMedia();
    }

    private void SetVideo()
    {
      var videoMedia = Data.Theme.GetMediaByName("Video");
      var videoPath = videoMedia?.FullPath;

      if (string.IsNullOrEmpty(videoPath)) return;
      
      MediaPath mediaPath = new MediaPath(videoPath, MediaPathType.AbsolutePathOrURL);
      _mediaPlayer.OpenMedia(mediaPath);
    }
  }
}