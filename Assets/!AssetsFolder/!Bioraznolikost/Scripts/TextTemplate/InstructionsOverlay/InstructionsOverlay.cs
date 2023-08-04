using CGH;
using DG.Tweening;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsOverlay : MonoBehaviour {
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private CanvasGroup _languageCanvasGroup;
	[SerializeField] private CanvasGroup _cover;

	private void Awake()
	{
		var button = GetComponent<UIButton>();

		button.OnClick.OnTrigger.Event.AddListener(() => {
			HideCanvasGroup();
			if (_languageCanvasGroup != null)
			{
				_languageCanvasGroup.Fade(0, 1);
			}
		});
	}

	public void Show()
	{
		_canvasGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(960.03f, 540);
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.Fade(1,1);
	}

	public void HideCanvasGroup()
	{
		_canvasGroup.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-960,540), .75f).From(new Vector2(960.03f,540f));
		_canvasGroup.blocksRaycasts = false;
		_cover.Fade(0, .75f);
		GetComponentInParent<GameTemplate>().IsInGame = true;
	}
}
