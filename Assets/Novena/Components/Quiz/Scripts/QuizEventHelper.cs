using Scripts.Quiz;
using UnityEngine;
using DG.Tweening;
using Doozy.Engine.UI;

public class QuizEventHelper : MonoBehaviour
{
  public Quiz quiz;

	[SerializeField] private UIButton _restartGame;

	private void Awake()
	{
	//	Quiz.OnCorrectAnswered += QuizCorrectAnswered;
	//	Quiz.OnWrongAnswered += QuizWrongAnswered;
		//quiz.OnQuizFinished += QuizFinished;
		quiz.OnQuizFinishedData += QuizFinishedInfo;

		//_restartGame.OnClick.OnTrigger.Event.AddListener(() => { 
			
		//});
	}


	private void QuizCorrectAnswered()
	{
		Debug.Log("Endpoint -> correct answered!!");
	}

	private void QuizWrongAnswered()
	{
		Debug.Log("Endpoint -> wrong answered!!");
	}

	private void QuizFinishedInfo(int correctAnswers, int WrongAnswers)
	{
		Debug.Log("Endpoint -> correct = " + correctAnswers + " , wrong = " + WrongAnswers);
	}

	private void QuizFinished()
	{
		//quiz.QuestionCounterText.alpha = 0;
		//quiz.QuestionText.alpha = 0;
		//quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(0, .75f);
	}
}