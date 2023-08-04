using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Scripts.Quiz {
	public class QuizAnsweredPanelManager : MonoBehaviour {

		[SerializeField] private Quiz _quiz;

		// moguce staviti custom animation metode koje animiraju panele po zelji
		public void QuestionAnswered(QuizItem quizItem, float offset) // TODO - QUIZ dodati animation float za delay izmedu ekrana werong i sljedeceg pitanja
		{
			switch (quizItem.RightAnswerIndicator)
			{
				case 1:
					_quiz.RightScreenAnswers[0].transform.SetSiblingIndex(3);
					_quiz.RightScreenAnimator.Play("LeftAnswerCorrect");
					_quiz.RightScreenAnswers[0].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);
					_quiz.RightScreenAnswers[1].DOFade(0, 1);
					_quiz.RightScreenAnswers[2].DOFade(0, 1).OnComplete(()=> {
						_quiz.RightScreenAnswers[0].DOFade(0, 0.01f).SetDelay(.5f);
						_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
					});
					break;
				case 2:
					_quiz.RightScreenAnswers[1].transform.SetSiblingIndex(3);
					_quiz.RightScreenAnimator.Play("MiddleAnswerCorrect");
					_quiz.RightScreenAnswers[1].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

					_quiz.RightScreenAnswers[0].DOFade(0, 1); 
					_quiz.RightScreenAnswers[2].DOFade(0, 1).OnComplete(() => {
						_quiz.RightScreenAnswers[1].DOFade(0, 0.01f).SetDelay(.5f);

						_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
					});
					break;
				case 3:
					_quiz.RightScreenAnswers[2].transform.SetSiblingIndex(3);
					_quiz.RightScreenAnimator.Play("RightAnswerCorrect");
					_quiz.RightScreenAnswers[2].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

					_quiz.RightScreenAnswers[0].DOFade(0, 1);
					_quiz.RightScreenAnswers[1].DOFade(0, 1).OnComplete(() => {
						_quiz.RightScreenAnswers[2].DOFade(0, 0.01f).SetDelay(.5f);

						_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
					});
					break;
			}
		}
		//Called near the end of animations
		public void ShowOverlay()
		{
			_quiz.RightScreenAnswerOverlay.DOFade(1, 1).OnComplete(() => {
				_quiz.RightScreenAnimator.Play("Idle");
			});
		}


	}
}