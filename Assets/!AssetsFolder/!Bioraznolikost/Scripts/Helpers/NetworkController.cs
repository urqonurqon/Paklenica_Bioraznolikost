using Novena.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Flags]
public enum Nuc {
	Leftmost = 1,
	Left = 2,
	Middle = 4,
	Right = 8,
	Rightmost = 16
}

public class NetworkController : MonoBehaviour {


	private Nuc _nuces;
	[SerializeField] private int _nucNumber;
	[SerializeField] private IdleTemplate _idleTemplate;
	private void Awake()
	{
		Idle.OnEnterExitIdle += SendToServer;
		_nuces = 0;


	}
	private void Start()
	{
		FMNetworkManager.instance.ClientSettings.ClientListenPort = Settings.GetValue<int>("ClientListenPort");
		FMNetworkManager.instance.ServerSettings.ServerListenPort = Settings.GetValue<int>("ServerListenPort");
		if (FMNetworkManager.instance.NetworkType == FMNetworkType.Client)
		{
			FMNetworkManager.instance.ClientSettings.ServerIP = Settings.GetValue<string>("ServerIP");
		}

	}

	public void CatchString(string caughtString)
	{
		bool containsInt = caughtString.Any(char.IsDigit);
		if (containsInt)
		{
			if (FMNetworkManager.instance.NetworkType == FMNetworkType.Server)
			{
				Nuc screen = 0;
				bool idle = false;

				switch (caughtString)
				{
					case "1":
						screen = Nuc.Leftmost;
						idle = true;
						break;
					case "2":
						screen = Nuc.Left;
						idle = true;

						break;
					case "4":
						screen = Nuc.Middle;
						idle = true;

						break;
					case "8":
						screen = Nuc.Right;
						idle = true;
						break;
					case "16":
						screen = Nuc.Rightmost;
						idle = true;
						break;
					case "-1":
						screen = Nuc.Leftmost;
						idle = false;
						break;
					case "-2":
						screen = Nuc.Left;
						idle = false;
						break;
					case "-4":
						screen = Nuc.Middle;
						idle = false;
						break;
					case "-8":
						screen = Nuc.Right;
						idle = false;
						break;
					case "-16":
						screen = Nuc.Rightmost;
						idle = false;
						break;
					default:
						return;



				}

				if (idle)
				{
					_nuces |= screen;
				}
				else
				{
					_nuces &= ~screen;

				}

				CheckForGroupIdles(_nuces);
			}
		}
		else
		{
			_idleTemplate.ChangePhoto(caughtString);
		}

	}



	private void CheckForGroupIdles(Nuc screens)
	{
		if (screens != (Nuc)31)
		{

			if (screens.HasFlag(Nuc.Leftmost) && screens.HasFlag(Nuc.Left))
			{
				FMNetworkManager.instance.SendToTarget("GroupOn", Settings.GetValue<string>("LeftmostIP"));
				FMNetworkManager.instance.SendToTarget("GroupOn", Settings.GetValue<string>("LeftIP"));
			}
			else
			{
				FMNetworkManager.instance.SendToTarget("SingleOn", Settings.GetValue<string>("LeftmostIP"));
				FMNetworkManager.instance.SendToTarget("SingleOn", Settings.GetValue<string>("LeftIP"));
			}



			if (screens.HasFlag(Nuc.Rightmost) && screens.HasFlag(Nuc.Right))
			{
				FMNetworkManager.instance.SendToTarget("GroupOn", Settings.GetValue<string>("RightmostIP"));
				FMNetworkManager.instance.SendToTarget("GroupOn", Settings.GetValue<string>("RightIP"));
			}
			else
			{
				FMNetworkManager.instance.SendToTarget("SingleOn", Settings.GetValue<string>("RightmostIP"));
				FMNetworkManager.instance.SendToTarget("SingleOn", Settings.GetValue<string>("RightIP"));

			}

			if (screens.HasFlag(Nuc.Middle))
			{
				FMNetworkManager.instance.SendToServer("SingleOn");
			}
		}
		else
		{
			FMNetworkManager.instance.SendToAll("AllOn");
		}


	}

	private void SendToServer(bool enteredIdle)
	{
		var stringToSend = "";
		if (enteredIdle)
			stringToSend = $"{_nucNumber}";
		else
			stringToSend = $"-{_nucNumber}";

		FMNetworkManager.instance.SendToServer(stringToSend);
	}


}
