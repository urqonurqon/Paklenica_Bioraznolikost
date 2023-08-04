using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace CGH {
	public static class CanvasGroupHelper {

		public static void Show(this CanvasGroup target)
		{
			target.alpha = 1;
			target.blocksRaycasts = true;
			target.interactable = true;
		}

		public static void Hide(this CanvasGroup target)
		{
			target.alpha = 0;
			target.blocksRaycasts = false;
			target.interactable = false;
		}


		public static void Fade(this CanvasGroup target, float endValue, float duration)
		{
			target.DOFade(endValue, duration).OnComplete(() => {

				if (endValue > 0)
				{
					target.blocksRaycasts = true;
					target.interactable = true;
				}
			});
			if (endValue == 0)
			{
				target.blocksRaycasts = false;
				target.interactable = false;
			}



		}

	}
}
