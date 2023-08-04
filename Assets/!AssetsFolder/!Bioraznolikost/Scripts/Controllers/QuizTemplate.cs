using DG.Tweening;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.UiUtility.Base;
using Scripts.Quiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine;
using System;
using CGH;
using Novena.DAL.Model.Guide;
using UnityEngine.UI;

public class QuizTemplate : UiController {

	public static Action OnQuizEnded;
	public static Action OnRestartClicked;
	public static Action OnQuitClicked;


	[SerializeField] Quiz _quiz;
	[SerializeField] private UIButton _restartGame;
	[SerializeField] private UIButton _skipQuestion;
	[SerializeField] private UIButton[] _back;

	[SerializeField] private RectTransform _finalWhitePanel;
	[SerializeField] private RectTransform _finalYellowPanelRightScreen;
	[SerializeField] private RectTransform _finalWhitePanelLeftScreen;
	[SerializeField] private RectTransform _sideBackgrounds;

	[SerializeField] private QuizIdleGallery _finalGalleryLeft;
	[SerializeField] private QuizIdleGallery _finalGalleryRight;

	[SerializeField] private CanvasGroup _endGameButtons;

	[SerializeField] private UnityEngine.UI.Image _background;

	private Sequence _resetSequence;

	public override void Awake()
	{
		base.Awake();
		RecordResetSequence();
		_quiz.OnQuizFinished += QuizFinished;
		Quiz.OnQuestionAnswered += QuestionAnswered;
		Quiz.OnQuestionAsked += QuestionAsked;

		_restartGame.OnClick.OnTrigger.Event.AddListener(() => {
			Debug.Log("stisno");
			OnRestartClicked?.Invoke();
			_restartGame.Interactable = false;
			_finalGalleryLeft.GetComponent<CanvasGroup>().alpha = 0;
			_finalGalleryRight.GetComponent<CanvasGroup>().alpha = 0;
			DOTween.Clear();

			_quiz.Setup(Data.TranslatedContent.GetThemeByTag("QuizTemplate"));
			_quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(-2162, .75f);
			_finalWhitePanel.DOAnchorPosX(340.5f, .75f);
			_quiz.QuestionCounterText.DOFade(1, .75f);
			_quiz.QuestionText.DOFade(1, .75f);
			_skipQuestion.GetComponent<CanvasGroup>().Hide();
			_back[0].transform.parent.GetComponent<RawImage>().DOFade(1, 1).OnComplete(() => {
				_back[0].Interactable = true;
				_restartGame.Interactable = true;
			});


		});
		_skipQuestion.GetComponent<CanvasGroup>().Hide();

		foreach (var button in _back)
		{
			button.OnClick.OnTrigger.Event.AddListener(() => QuitGame(button));
		}

		_skipQuestion.OnClick.OnTrigger.Event.AddListener(() => {
			Debug.Log("stisno");

			_skipQuestion.GetComponent<CanvasGroup>().Fade(0, 1);
			_quiz.StopAllCoroutines();
			_quiz.SetAnotherQuestionWithoutDelay();
		});
	}

	public void QuitGame(UIButton button)
	{
		Debug.Log("stisno");
		OnQuitClicked?.Invoke();
		if (button != null)
			button.Interactable = false;
		_finalGalleryLeft.GetComponent<CanvasGroup>().alpha = 0;
		_finalGalleryRight.GetComponent<CanvasGroup>().alpha = 0;
		DOTween.Clear();

		_resetSequence.Restart();
		_quiz.StopAllCoroutines();
		_quiz.RightScreenAnswerOverlay.DOFade(0, .2f);
		_skipQuestion.GetComponent<CanvasGroup>().Hide();
		_quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(-2162, .75f);
		_quiz.QuestionCounterText.DOFade(1, .75f);
		_quiz.QuestionText.DOFade(1, .75f);
		_finalWhitePanel.DOAnchorPosX(340.5f, .75f);

		_sideBackgrounds.DOAnchorPosX(-1385, 1).OnComplete(() => GameEventMessage.SendEvent("BackButton"));
	}

	private void QuestionAsked(bool x, Theme obj)
	{
		_skipQuestion.GetComponent<CanvasGroup>().Fade(0, 1);
		_back[0].transform.parent.GetComponent<RawImage>().DOFade(1, 1).OnComplete(() => _back[0].Interactable = true);

	}

	private void QuestionAnswered(bool isCorrectAnswer, QuizItem item)
	{
		_back[0].transform.parent.GetComponent<RawImage>().DOFade(0, 1);
		_back[0].Interactable = false;
		if (isCorrectAnswer)
		{
			_skipQuestion.CanvasGroup.DOFade(1, 1).SetDelay(1).OnComplete(() => {
				_skipQuestion.CanvasGroup.blocksRaycasts = true;
				_skipQuestion.CanvasGroup.interactable = true;

			});
		}
	}

	private void RecordResetSequence()
	{
		_resetSequence = DOTween.Sequence();
		_resetSequence.SetAutoKill(false);
		_resetSequence
			.Append(_quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(-1920, .75f))
			.Append(_quiz.QuestionCounterText.DOFade(1, .75f))
			.Append(_quiz.QuestionText.DOFade(1, .75f))
			.PrependInterval(1);
		_resetSequence.Pause();
	}

	public override void OnShowViewStart()
	{
		_quiz.Setup(Data.TranslatedContent.GetThemeByTag("QuizTemplate"));
		_back[0].transform.parent.GetComponent<RawImage>().DOFade(1, 1).OnComplete(() => _back[0].Interactable = true);
		foreach (var button in _back)
		{
			button.Interactable = true;
		}
		_sideBackgrounds.DOAnchorPos(new Vector2(-720.5401f, 0), 1).From(new Vector2(-1385, 0));
		_quiz.QuestionCounterText.DOFade(1, 0);
		_quiz.QuestionText.DOFade(1, 0f);
		_quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(-1920, 0);
		_background.DOColor(new Color(0.7411765f, 0.7411765f, 0.7411765f, 0.5f), 1).From(new Color(0.1529412f, 0.1529412f, 0.1529412f, 0.8f));
	}



	private void QuizFinished()
	{
		RaycastBlocker.BlockRaycasts();
		_quiz.isInGame = false;
		_quiz.QuestionCounterText.alpha = 0;
		_quiz.QuestionText.alpha = 0;
		_back[0].transform.parent.GetComponent<RawImage>().DOFade(0, 1);
		_back[0].Interactable = false;
		_endGameButtons.blocksRaycasts = false;


		_quiz.QuizMainContent.DOFade(0.0f, 1).SetDelay(0.1f).OnComplete(() => {
			_finalWhitePanel.DOAnchorPosX(138.42f, .75f);
			_quiz.FinalPanel.GetComponent<RectTransform>().DOAnchorPosX(0, .75f).OnComplete(() => {


				OnQuizEnded?.Invoke();

				_finalWhitePanelLeftScreen.DOSizeDelta(new Vector2(2800, 1080), 1);
				_finalYellowPanelRightScreen.DOAnchorPosX(132.07f, 1).OnComplete(() => {
					_finalGalleryRight.SetupGallery(true);
					_finalGalleryLeft.SetupGallery(true);
					_finalGalleryLeft.GetComponent<CanvasGroup>().DOFade(1, 3);
					_finalGalleryRight.GetComponent<CanvasGroup>().DOFade(1, 3).OnComplete(() => {
						_finalWhitePanelLeftScreen.sizeDelta = new Vector2(681, 1080);
						_finalYellowPanelRightScreen.anchoredPosition = new Vector2(-2052.1f, 0);
						_endGameButtons.blocksRaycasts = true;
						RaycastBlocker.UnblockRaycasts();
					});

				});

			});
		});
	}


}
