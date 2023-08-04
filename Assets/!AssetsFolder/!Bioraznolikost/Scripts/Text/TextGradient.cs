using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGradient : MonoBehaviour {

	[SerializeField] private ScrollRect _textScrollView;
	[SerializeField] private RectTransform _textScrollViewContent;
	[SerializeField] private CanvasGroup _softMaskCanvasGroup;
	[SerializeField] private CanvasGroup _arrowBelowTextCanvasGroup;

	private void Awake()
	{

		_textScrollView.onValueChanged.AddListener(FadeOutTextGradiantAndArrow);
	}

	public void FixSoftMaskAndResetScrollView()
	{
		FixSoftMask();
		ResetScrollView();
	}

	public void FixSoftMask()
	{

		_softMaskCanvasGroup.gameObject.SetActive(false);
		_softMaskCanvasGroup.gameObject.SetActive(true);
	}

	public void ResetScrollView()
	{

		_textScrollView.verticalNormalizedPosition = 1;
	}

	private void FadeOutTextGradiantAndArrow(Vector2 scrollRectValue)
	{
		var contentY = _textScrollViewContent.anchoredPosition.y;
		var contentMaxY = _textScrollViewContent.rect.height - _textScrollView.GetComponent<RectTransform>().rect.height;
		var gradientHeight = _softMaskCanvasGroup.GetComponent<RectTransform>().rect.height;
		var contentYSkewed = contentY - (contentMaxY - gradientHeight);
		var positionPercentage = contentYSkewed / gradientHeight;


		if (contentMaxY - gradientHeight < contentY)
		{
			_softMaskCanvasGroup.alpha = 1 - positionPercentage;
			_arrowBelowTextCanvasGroup.alpha = _softMaskCanvasGroup.alpha;
		}
		else
		{
			_softMaskCanvasGroup.alpha = 1;
			_arrowBelowTextCanvasGroup.alpha = _softMaskCanvasGroup.alpha;
		}
	}
}
