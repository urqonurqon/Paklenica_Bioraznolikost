using UnityEngine;

namespace Novena.Utility.Application
{
  public class ApplicationSettings : MonoBehaviour
  {
    [Header("Options")]
    [SerializeField] private bool OnEscapeKeyQuit = true;
    [SerializeField] private bool MultituchEnabled = true;
    
    [Header("Screen")] 
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private bool _isFullscreen;

    void Awake()
    {
      Input.multiTouchEnabled = MultituchEnabled;
      SetScreen();
    }

    private void SetScreen()
    {
      Screen.SetResolution(_width, _height, _isFullscreen);
    }

    void Update()
    {
      //if (Input.GetKey("escape") && OnEscapeKeyQuit)
      //{
      //  UnityEngine.Application.Quit();
      //}
    }
  }
}
