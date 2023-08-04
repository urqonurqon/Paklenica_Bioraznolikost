using DG.Tweening;
using Doozy.Engine;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.Settings;
using Scripts.Quiz;
using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class QuizUDPListener : MonoBehaviour {


	enum Screen {
		Left,
		Right
	}

	[SerializeField] private Screen _screen = new Screen();

	[SerializeField] private IdleTemplate _idleTemplate;
	[SerializeField] private QuizIdleGallery _finalGallery;
	[SerializeField] private RectTransform _finalWhitePanelLeftScreen;
	[SerializeField] private QuizIdleGallery _finalGalleryLeft;
	[SerializeField] private RectTransform _finalYellowPanelRightScreen;
	[SerializeField] private QuizIdleGallery _finalGalleryRight;
	[SerializeField] private Quiz _quiz;
	[SerializeField] private QuizQuestionSlideshow _quizQuestionSlideshow;


	private Theme _theme;
	private QuizItem _quizItem;
	private Idle _idle;


	private void Awake()
	{
		_idle = FindObjectOfType<Idle>();
	}

	private void Start()
	{
		if (FMNetworkManager.instance.NetworkType == FMNetworkType.Client)
		{
			FMNetworkManager.instance.ClientSettings.ServerIP = Settings.GetValue<string>("ServerIP");
		}
	}


	public void CatchString(string caughtString)
	{
		if (caughtString.Contains("SendLanguage"))
		{
			int id;
			caughtString = caughtString.Replace("SendLanguage", "");
			int.TryParse(caughtString, out id);
			foreach (var tc in Data.Guide.TranslatedContents)
			{
				if (tc.LanguageId == id)
					Data.TranslatedContent = tc;
			}
		}


		if (caughtString.Contains("SendTheme"))
		{
			int id;
			caughtString = caughtString.Replace("SendTheme", "");
			int.TryParse(caughtString, out id);
			foreach (var theme in Data.TranslatedContent.GetThemeByTag("QuizTemplate").SubThemes)
			{
				if (theme.Id == id)
					_theme = theme;
			}
			SetAnotherQuestionContent();
		}
		


		switch (caughtString)
		{
			case "RestartClicked":
				RestartClicked();
				break;
			case "QuitClicked":
				QuitClicked();
				break;

			case "QuestionAnsweredCorrect 1":
				QuestionAnswered(true, 1);
				break;
			case "QuestionAnsweredCorrect 2":
				QuestionAnswered(true, 2);
				break;
			case "QuestionAnsweredCorrect 3":
				QuestionAnswered(true, 3);
				break;

			case "QuestionAnsweredWrong 1":
				QuestionAnswered(false, 1);
				break;
			case "QuestionAnsweredWrong 2":
				QuestionAnswered(false, 2);
				break;
			case "QuestionAnsweredWrong 3":
				QuestionAnswered(false, 3);
				break;


			case "NextQuestionStarting":
				StartCoroutine(NextQuestionStarting());
				break;


			case "EnterIdle":
				EnterIdle();
				break;
			case "ExitIdle":
				ExitIdle();
				break;

			case "EnterQuiz":
				EnterQuiz();
				break;
			case "QuizEnded":
				QuizEnded();
				break;

			case "SingleIdle":
				SingleIdle();
				break;
			case "AllIdle":
				AllIdle();
				break;

			default:
				return;


		}
	}

	private void AllIdle()
	{
		_idleTemplate.RawImageSingle.DOFade(0, 0.5f);
		//_idleTemplate.RawImageGroup.DOFade(0, 0.5f);
		_idleTemplate.RawImageAll.DOFade(1, 0.5f);
	}

	private void SingleIdle()
	{
		_idleTemplate.RawImageSingle.DOFade(1, 0.5f);
		//_idleTemplate.RawImageGroup.DOFade(0, 0.5f);
		_idleTemplate.RawImageAll.DOFade(0, 0.5f);
	}

	private void EnterQuiz()
	{
		GameEventMessage.SendEvent("GoToQuiz");
		ShowHideAnswersRightScreen(true);
		_quiz.RightScreenAnswerOverlay.DOFade(0, 1);
		//_quiz.FillListWithQuizItems();
	}

	private void ExitIdle()
	{
		GameEventMessage.SendEvent("ExitIdle");

		_idle.IsOnIdleScreen = false;
	}

	private void EnterIdle()
	{
		
		GameEventMessage.SendEvent("GoToIdle");
		Resources.UnloadUnusedAssets();
		_idle.IsOnIdleScreen = true;
	}
	private void QuitClicked()
	{
		_finalGallery.GetComponent<CanvasGroup>().alpha = 0;
		DOTween.Clear();
		GameEventMessage.SendEvent("BackToHome");
		_quiz.RightScreenAnswerOverlay.DOFade(0, .2f);
	}

	private void RestartClicked()
	{
		_finalGallery.GetComponent<CanvasGroup>().alpha = 0;
		DOTween.Clear();
		EnterQuiz();
	}

	private void QuizEnded()
	{
		if (_screen == Screen.Left)
		{
			_finalWhitePanelLeftScreen.DOSizeDelta(new Vector2(2800, 1080), 1).OnComplete(() => {
				_finalGalleryLeft.SetupGallery(true);
				_finalGalleryLeft.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => {
					_finalWhitePanelLeftScreen.sizeDelta = new Vector2(681, 1080);
				});
			});
		}
		else
		{
			_finalYellowPanelRightScreen.DOAnchorPosX(132.07f, 1).OnComplete(() => {
				_finalGalleryRight.SetupGallery(true);
				_finalGalleryRight.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => {
					_finalYellowPanelRightScreen.anchoredPosition = new Vector2(-2052.1f, 0);
				});
			});
		}
	}

	private IEnumerator NextQuestionStarting()
	{
		ShowHideAnswersRightScreen(false);
		_quiz.RightScreenAnswerOverlay.DOFade(0, 1);
		yield return new WaitForSeconds(2);


	}

	private void SetAnotherQuestionContent()
	{
		ShowHideAnswersRightScreen(true);
		_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 0);
		List<string> photoPathList = new List<string>();
		List<Photo> photoList = new List<Photo>();
		photoList = _theme.GetMediaByName("Gallery").GetPhotos();

		foreach (var photo in photoList)
		{
			photoPathList.Add(photo.FullPath);
		}


		_quizItem = new QuizItem(
			_theme.GetMediaByName("Question").Text,
			_quiz.FindRightAnswerIndicator(_theme),
			_theme.GetMediaByName("Explanation").Text,
			"",
			photoPathList[0],
			"",
			photoPathList[1],
			"",
			photoPathList[2]
			);

		_quiz.Explanation.text = _quizItem.Explanation;

		if (_screen == Screen.Right)
			SetupRightScreenPhotos(photoList, _quizItem.RightAnswerIndicator, 3);
		else
		{
			_quizQuestionSlideshow.SetupGallery(false, _theme);

		}

	}


	//private void SetupRightScreenPhotos(List<string> imagePathList, int rightAnswer, int numberOfAnswers)
	//{
	//	for (int i = 0; i < numberOfAnswers; i++)
	//	{
	//		if (i + 1 == rightAnswer)
	//		{
	//			AssetsFileLoader.LoadTexture2D(imagePathList[i], _quiz.BigPhotoRightScreen);
	//		}
	//		AssetsFileLoader.LoadTexture2D(imagePathList[i], _quiz.PhotoContainers[i]);

	//	}
	//}

	private float SetupRightScreenPhotos(List<Photo> imageList, int rightAnswer, int numberOfAnswers)
	{
		float photoOffsetX = 0;

		for (int i = 0; i < numberOfAnswers; i++)
		{

			_quiz.PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}

		for (int i = 0; i < numberOfAnswers; i++)
		{
			AssetsFileLoader.LoadTexture2D(imageList[i].FullPath, _quiz.PhotoContainers[i]);
			_quiz.PhotoContainers[i].SetNativeSize();

			if (i + 1 == rightAnswer)
			{
				AssetsFileLoader.LoadTexture2D(imageList[i].FullPath, _quiz.BigPhotoRightScreen);
				_quiz.PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(_quiz.PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.x + _theme.PositionX, _quiz.PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.y);
				photoOffsetX = _quiz.PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.x;
			}

		}


		return photoOffsetX;
	}

	public void ShowHideAnswersRightScreen(bool show)
	{
		foreach (var answerPhoto in _quiz.RightScreenAnswers)
		{
			answerPhoto.DOFade(show ? 1 : 0, 1);
		}
	}

	private void QuestionAnswered(bool isCorrectAnswer, int correctAnswer)
	{
		switch (correctAnswer)
		{
			case 1:
				_quiz.RightScreenAnswers[0].transform.SetSiblingIndex(3);

				_quiz.RightScreenAnimator.Play("LeftAnswerCorrect");
				_quiz.RightScreenAnswers[1].DOFade(0, 1);

				_quiz.RightScreenAnswers[0].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);


				_quiz.RightScreenAnswers[2].DOFade(0, 1).OnComplete(() => {
					_quiz.RightScreenAnswers[0].DOFade(0, 0.01f).SetDelay(.5f);
					_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
				});
				break;
			case 2:
				_quiz.RightScreenAnswers[1].transform.SetSiblingIndex(3);

				_quiz.RightScreenAnimator.Play("MiddleAnswerCorrect");
				_quiz.RightScreenAnswers[0].DOFade(0, 1);

				_quiz.RightScreenAnswers[1].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

				_quiz.RightScreenAnswers[2].DOFade(0, 1).OnComplete(() => {
					_quiz.RightScreenAnswers[1].DOFade(0, 0.01f).SetDelay(.5f);

					_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
				});
				break;
			case 3:
				_quiz.RightScreenAnswers[2].transform.SetSiblingIndex(3);

				_quiz.RightScreenAnimator.Play("RightAnswerCorrect");
				_quiz.RightScreenAnswers[0].DOFade(0, 1);
				_quiz.RightScreenAnswers[2].GetComponentInChildren<SoftMaskScript>().GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

				_quiz.RightScreenAnswers[1].DOFade(0, 1).OnComplete(() => {
					_quiz.RightScreenAnswers[2].DOFade(0, 0.01f).SetDelay(.5f);

					_quiz.BigPhotoRightScreen.color = new Color(1, 1, 1, 1);
				});
				break;
		}
		if (_screen == Screen.Left)
			_quizQuestionSlideshow.StopLoopingPhotos(isCorrectAnswer, _quizItem);
	}

}
