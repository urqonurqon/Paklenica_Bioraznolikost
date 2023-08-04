using Doozy.Engine;
using Novena.UiUtility.Base;
using TMPro;
using UnityEngine;

namespace Novena.Admin {
  public class AdminLoginController : UiController {
    [Header("Pin login")]
    [SerializeField] private string pinPassword;
    [SerializeField] private TMP_InputField pinTmpInput;

    //[Header("Gesture login")]
    //[SerializeField] private AdminGestureLogin adminGestureLogin;

    public override void Awake()
    {
      //adminGestureLogin.EnableGestureDraw = false;
      //adminGestureLogin.ClearLines();
      base.Awake();
    }
		public override void OnShowViewStart()
		{
			
		}
		public override void OnShowViewFinished()
    {
      //adminGestureLogin.EnableGestureDraw = true;
    }

    public override void OnHideViewFinished()
    {
      //adminGestureLogin.EnableGestureDraw = false;
      //adminGestureLogin.ClearLines();
    }

    public void OnPinDrawerOpen()
    {
      pinTmpInput.text = "";
      //adminGestureLogin.EnableGestureDraw = false;
      //adminGestureLogin.ClearLines();
    }

    public void OnPinDrawerClose()
    {
      pinTmpInput.text = "";
      //adminGestureLogin.EnableGestureDraw = true;
    }

    public void OnPinDrawerToggle(bool isEnabled)
    {
      pinTmpInput.text = "";
      //adminGestureLogin.EnableGestureDraw = !isEnabled;
      //adminGestureLogin.ClearLines();
    }

    public void OnPinInputValueChange()
    {
      if (pinTmpInput.text == pinPassword)
      {
        GameEventMessage.SendEvent("OnLoginSuccess");
        pinTmpInput.text = "";
      }
    }
  }
}