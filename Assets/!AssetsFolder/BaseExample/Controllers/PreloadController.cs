using Scripts.Helpers;
using Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controllers
{
  public class PreloadController : MonoBehaviour
  {
    public static PreloadController Instance { get; private set; }
    
    [Header("Components")]
    [SerializeField] private Image preloadBar;
    [SerializeField] private TMP_Text percent;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text currentMbText;
    [SerializeField] private TMP_Text totalMbText;

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
    }

    public void OnScreenEnable()
    {
      ShowPreload();
    }
    
    private void ShowPreload()
    {
      preloadBar.fillAmount = 0f;
      percent.text = "0";
    }

    public void PreloadAmount(float progress)
    {
      preloadBar.fillAmount = progress / 100;
      percent.text = Mathf.Round(progress).ToString() + " %";
    }

    public void SetStatus(string msg)
    {
      statusText.text = msg;
    }

    public void UpdateLoadingState(int percentage, long currentDownloaded, long totalToDownload)
    {
      PreloadAmount(percentage);
      currentMbText.text = (currentDownloaded / 1048576.00).ToString("0.00");
      totalMbText.text = (totalToDownload / 1048576.00).ToString("0.00");
    }
    
    public void UpdateLoadingState(int percentage)
    {
      PreloadAmount(percentage);
      currentMbText.text = "";
      totalMbText.text = "";
    }
  }
}
