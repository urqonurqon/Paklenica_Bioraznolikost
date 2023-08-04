using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Components.VideoPlayer
{
  public class Controls : MonoBehaviour
  {
    [SerializeField] private VideoPlayerController videoPlayerController = null;
    [SerializeField] private CanvasGroup controlsCanvasGroup = null;
    [SerializeField] private Sprite playSprite = null;
    [SerializeField] private Sprite pauseSprite = null;
    [SerializeField] private Image buttonIconSprite = null;
    [SerializeField] private float timeToDisplay = 5f;

    private Coroutine _timeToDisplayCoroutine;


    private void Start()
    {
      VideoPlayerController.OnVideoStarted += VideoStarted;
      VideoPlayerController.OnVideoEnded += VideoEnded;
      VideoPlayerController.OnVideoPrepared += VideoPrepared;
      ShowControls(false);
    }
    private void VideoPrepared()
    {
      ShowControls(true);
    }

    private void VideoStarted()
    {
      SetButtonState(false);
    }
    
    private void VideoEnded()
    {
      SetButtonState(true);
      ShowControls(true, true);
    }

    public void OnRawImageClick()
    {
      ShowControls(true);
    }

    /// <summary>
    /// To show controls for NN seconds. If withoutTimer is true controls will be display indefinitely.
    /// </summary>
    /// <param name="show"></param>
    /// <param name="withoutTimer"></param>
    public void ShowControls(bool show, bool withoutTimer = false)
    {
      if( _timeToDisplayCoroutine != null ) StopCoroutine( _timeToDisplayCoroutine );
      
      controlsCanvasGroup.DOFade(show ? 1 : 0, 0.5f);
      controlsCanvasGroup.interactable = show;
      controlsCanvasGroup.blocksRaycasts = show;

      if (show && withoutTimer == false)
        _timeToDisplayCoroutine = StartCoroutine(TimeToDisplay());
    }

    /// <summary>
    /// How long to display controls.
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeToDisplay()
    {
      yield return new WaitForSeconds(timeToDisplay);
      ShowControls(false);
    }

    private void SetButtonState(bool state)
    {
      buttonIconSprite.sprite = state ? playSprite : pauseSprite;
    }

    public void PlayPauseToggle()
    {
      if (VideoPlayerController.IsPlaying)
      {
        videoPlayerController.PauseVideo();
        SetButtonState(true);
        ShowControls(true, true);
      }
      else
      {
        videoPlayerController.PlayVideo();
        SetButtonState(false);
        ShowControls(true);
      }
    }
  }
}