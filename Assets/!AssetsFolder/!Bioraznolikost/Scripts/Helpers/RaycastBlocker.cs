using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBlocker : MonoBehaviour {

	private static CanvasGroup _blocker;

	private void Awake()
	{
		_blocker = GetComponent<CanvasGroup>();
	}

	public static void BlockRaycasts()
	{
		_blocker.blocksRaycasts = true;
	}
	public static void UnblockRaycasts()
	{
		_blocker.blocksRaycasts = false;
	}

}
