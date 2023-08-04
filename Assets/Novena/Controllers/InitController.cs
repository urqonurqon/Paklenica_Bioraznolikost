using System.Linq;
using Doozy.Engine;
using Novena.Networking.DataLoading;
using Novena.Networking.Download.Helpers;
using Novena.UiUtility.Base;
using Novena.Utility.Interface;
using UnityEngine;

namespace Novena.Controllers
{
  public class InitController : UiController
  {
		public override void OnShowViewStart()
		{

		}
		public override void OnShowViewFinished()
    {
      CheckGuideState();
      base.OnShowViewFinished();
    }
    public void CheckGuideState()
    {
      if (DownloadHelper.CheckIfGuideExist(DataLoading.DownloadCode))
      {
        DataLoading.Instance.LoadGuideData();
        GameEventMessage.SendEvent("GoToHome");
      }
      else
      {
        GameEventMessage.SendEvent("GoToAdmin");
      }

      Initialize();
    }

    private void Initialize()
    {
      var inits = Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<IInitialize>();

      foreach (var init in inits)
      {
        init.Initialize();
      }
    }
  }
}