using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.Components.VideoPlayer
{
  [RequireComponent(typeof(UnityEngine.Video.VideoPlayer))]
  public class VideoPlayerController : MonoBehaviour
  {
    public static Action OnVideoStarted;
    public static Action OnVideoEnded;
    public static Action OnVideoPrepared;
    private static Action _onSetVideo;
    public static bool IsPlaying => _isPlaying;

    [SerializeField] private Slider slider;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private RawImage rawImage;

    private UnityEngine.Video.VideoPlayer _videoPlayer;
    private float _duration;
    private float _percent;
    private static string _videoUrl;
    private static bool _isPlaying;

    private void Awake()
    {
      _videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();

      _videoPlayer.loopPointReached += VideoEnded;
      _videoPlayer.prepareCompleted += PrepareCompleted;
      _videoPlayer.started += OnVideoStart;
      _videoPlayer.seekCompleted += PlaySeekedVideo;

      _videoPlayer.enabled = false;

      _onSetVideo += SetVideo;
    }

    public static void SetUrl(string url)
    {
      _videoUrl = url;
      _onSetVideo.Invoke();
    }

    /// <summary>
    /// Reset render texture of video. So it doesnt have last frame of last video.
    /// </summary>
    private void ClearRenderTexture()
    {
      RenderTexture rt = RenderTexture.active;
      RenderTexture.active = renderTexture;
      GL.Clear(true, true, Color.clear);
      RenderTexture.active = rt;
    }
    
    public void PauseVideo()
    {
      _videoPlayer.Pause();
      _isPlaying = false;
    }

    public void PlayVideo()
    {
      _videoPlayer.Play();
      _isPlaying = true;
    }

    public void SetTime()
    {
      _videoPlayer.time = _duration * slider.value;
    }

    public void ResetVideo()
    {
      _videoPlayer.Stop();
      _videoPlayer.enabled = false;
      ClearRenderTexture();
    }

    private void SetVideo()
    {
      _videoPlayer.enabled = true;
      _videoPlayer.url = _videoUrl;

      StartCoroutine(PrepareVideo());
    }

    IEnumerator PrepareVideo()
    {
      yield return new WaitForEndOfFrame();
      _videoPlayer.Prepare();
    }

    private void PlaySeekedVideo(UnityEngine.Video.VideoPlayer source)
    {
    }

    private void OnVideoStart(UnityEngine.Video.VideoPlayer source)
    {
      OnVideoStarted?.Invoke();
    }

    private void PrepareCompleted(UnityEngine.Video.VideoPlayer source)
    {
      var texture = _videoPlayer.texture;
      SetAspectRatio(texture.width, texture.height);

      _duration = _videoPlayer.frameCount / _videoPlayer.frameRate;
      _videoPlayer.Play();
      _isPlaying = true;
      
      OnVideoPrepared?.Invoke();
    }

    private void SetAspectRatio(int width, int height)
    {
      AspectRatioFitter aspectRatioFitter = rawImage.GetComponent<AspectRatioFitter>();

      if (aspectRatioFitter != null)
      {
        float aspectRatio = (float) width / (float) height;
        aspectRatioFitter.aspectRatio = aspectRatio;
      }
    }

    private void VideoEnded(UnityEngine.Video.VideoPlayer source)
    {
      _isPlaying = false;
      OnVideoEnded?.Invoke();
    }

    void Update()
    {
      if (_isPlaying)
      {
        _percent = (float) (_videoPlayer.time / _duration);
        slider.value = _percent;
      }
    }
  }
}