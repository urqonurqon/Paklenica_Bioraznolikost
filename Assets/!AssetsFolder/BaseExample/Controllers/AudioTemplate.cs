using Novena.DAL;
using Novena.UiUtility.Base;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace _AssetsFolder.BaseExample.Controllers
{
  public class AudioTemplate : UiController
  {
    [Header("Components")] [SerializeField]
    private MediaPlayer _mediaPlayer;

    public override void OnShowViewStart()
    {
      SetAudio();
    }

    public override void OnHideViewStart()
    {
      _mediaPlayer.CloseMedia();
    }

    private void SetAudio()
    {
      var audioMedia = Data.Theme.GetMediaByName("Audio");
      var audioPath = audioMedia?.FullPath;

      if (string.IsNullOrEmpty(audioPath)) return;

      MediaPath mediaPath = new MediaPath(audioPath, MediaPathType.AbsolutePathOrURL);
      _mediaPlayer.OpenMedia(mediaPath);
    }
  }
}