using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Novena.Components.AudioPlayer
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioPlayer : MonoBehaviour
  {
    public Action OnAudioEnd;
    
    [Header("Components")] 
    [SerializeField] private AudioClip audioClip;
    [Header("Button")] 
    [SerializeField] private Sprite playIcon;
    [SerializeField] private Sprite pauseIcon;
    [SerializeField] private Image iconPlaceholder;
    [Header("Text")] 
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text totalTimeText;
    
    [Header("Controls")]
    [SerializeField] private Slider slider;

    #region private fields

    private AudioSource _audioSource;
    private bool _isPlaying;
    private float _totalAudioTime;

    #endregion

    private void Awake()
    {
      if (_audioSource == null)
      {
        _audioSource = GetComponent<AudioSource>();
      }
    }

    public void LoadAudioClip(AudioClip clip)
    {
      audioClip = clip;
      _audioSource.clip = audioClip;
      Init();
    }

    private void Init()
    {
      _totalAudioTime = audioClip.length;
      slider.maxValue = _totalAudioTime;
      SetTimeText(00f);
    }

    public void OnButtonPlayClick(bool isOn)
    {
      if (isOn)
      {
        Play();
      }
      else
      {
        Pause();
      }
    }

    private void ToggleButtonState(bool state)
    {
      iconPlaceholder.sprite = state ? pauseIcon : playIcon;
    }
    
    public void Play()
    {
      _audioSource.Play();
      ToggleButtonState(true);
      _isPlaying = true;
    }

    public void Pause()
    {
      _audioSource.Pause();
      ToggleButtonState(false);
      _isPlaying = false;
    }

    private void SetTimeText(float currentTime)
    {
      currentTimeText.text = TimeFormatter(currentTime);
      totalTimeText.text = TimeFormatter(_totalAudioTime);;
    }
    
    private static string TimeFormatter( float seconds)
    {
      float secondsRemainder = Mathf.Floor( (seconds % 60) * 100) / 100.0f;
      int minutes = ((int)(seconds / 60)) % 60;
      
      return $"{minutes:00}:{secondsRemainder:00}";
    }

    private bool IsEnded()
    {
      bool output = Math.Abs(_totalAudioTime - _audioSource.time) < 0.0001;

      return output;
    }

    private void UpdateSlider()
    {
      slider.value = _audioSource.time;
    }

    public void SetTime()
    {
      _audioSource.time = slider.value;
      SetTimeText(_audioSource.time);
    }

    private void OnEnd()
    {
      _isPlaying = false;
      _audioSource.time = 0;
      SetTimeText(_audioSource.time);
      UpdateSlider();
      ToggleButtonState(false);
      OnAudioEnd?.Invoke();
    }

    private void Update()
    {
      if (_isPlaying)
      {
        UpdateSlider();
        SetTimeText(_audioSource.time);

        if (!IsEnded()) return;
        OnEnd();
      }
    }
  }
}