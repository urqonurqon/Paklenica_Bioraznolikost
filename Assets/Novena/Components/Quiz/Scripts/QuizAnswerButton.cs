using DG.Tweening;
using Scripts.Utility;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Quiz {
	public class QuizAnswerButton : MonoBehaviour {

		[Header("Components")]
		[SerializeField] private TMP_Text _answerText;
		[SerializeField] private TMP_Text _indicatorLetterText;
		[Space(10)]
		[Header("Helpers")]
		[SerializeField] private TMP_Text _textHelper;
		[SerializeField] private Image _fillImageHelper;
		[SerializeField] private RawImage _buttonBackground;
		[Header("Settings")]
		[SerializeField] private float _animationSpeed = 0.25f;

		private bool _rightAnswer = false;
		public bool RightAnswer { get => _rightAnswer; set => _rightAnswer = value; }

		public void SetupButton(string indicatorForText, string answerText, Action onClick, string imagePath)
		{
			_answerText.text = answerText;

			if (indicatorForText == "null")
			{
				_indicatorLetterText.text = "";
			}
			else
			{
				_indicatorLetterText.text = indicatorForText;
			}

			AssetsFileLoader.LoadTexture2D(imagePath, _buttonBackground);

			Button btn = gameObject.GetComponent<Button>();
			btn.onClick.AddListener(() => { onClick(); IndicateAnswer(); });
		}

		// TODO - QUIZ change naming
		// CALLED RIGHT ANSWERED BUTTON IF ITS WRONG ANSWERED
		// OnWrongButtonClickTriggerRightButton
		

		public void IndicateAnswer()
		{
			// kada je dobro odgovoren tada ne prikaze Texthelper!!!!!
			_textHelper.enabled = false;

			if (!_rightAnswer)
			{
				Color color = new Color(0.1529412f, 0.1529412f, 0.1529412f, .9f);
				_fillImageHelper.DOColor(color, 1);
				_indicatorLetterText.DOColor(new Color(0.9058824f, 0.8392158f, 0, 1), 1);
			}
			else
			{
				_fillImageHelper.DOFade(0,1);
				_indicatorLetterText.DOColor(new Color(1, 1, 1, 1), 1);
			}
		}

	}
}