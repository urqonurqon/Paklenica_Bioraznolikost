using DG.Tweening;
using Scripts.Helpers;
using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Novena.DAL.Model.Guide;
using Novena.Enumerators;
using Novena.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Novena.Networking;
using Doozy.Engine.UI;
using CGH;
using Novena.DAL;

namespace Scripts.Quiz {
	public class Quiz : MonoBehaviour {
		// CUSTOM ANIMACIJE NA BUTTONIMA SE RADE U QUIZANSWERBUTTON.CS GDJE SE POZIVAJU 2. METODE
		#region Public fields for Custom Editor
		public float AnimationSpeed = 1.0f; // brzina fade in fade out!
		public float AnimationDelay = 1.5f; // panel manager animation!

		public CanvasGroup QuizMainContent = null;
		public QuizAnsweredPanelManager AnswerManager = null;
		public TMP_Text QuestionText = null;
		public TMP_Text QuestionCounterText = null;
		public RawImage QuizImage = null;
		public GameObject AnswerButtonPrefab = null;
		public Transform AnswerButtonContainerTop = null;
		public Transform AnswerButtonContainerBottom = null;
		public RectTransform FinalPanel = null;

		public TMP_Text CorrectAnswerText = null;
		public TMP_Text WrongAnswerText = null;

		// for Custom editor
		public bool UseButtonIndicators;
		public bool UseNumericalIndicators;
		public bool UseAlphabeticalIndicators;
		public bool UseCustomIndicators;
		public List<string> CustomIndicators = null;

		// prikazuje sadrzaj iz CMS-a te prikazuje error koji se nalazi na odredenoj temi
		public bool UseCMSDebugging;
		#endregion

		#region Private quiz fields
		// private for QUIZ internal variables
		private Theme _theme;
		public List<QuizItem> ListOfQuizItems = new List<QuizItem>();
		private List<GameObject> _listOfAnswerButton = new List<GameObject>();

		private int _currentQuestion = 0;
		private int _numberOfMaximumQuestions = 0;
		// correct and wrong answers counters
		private int _correctAnsweredCounter = 0;
		private int _wrongAnsweredCounter = 0;

		string[] _chosenIndicator = null;
		// ako su potrebni custom indikatori moze se prmjenit sadrzaj za jedan od sljedecih array-eva
		string[] _numericalIndicators = new string[] { "1", "2", "3", "4" };
		string[] _alphabeticalIndicators = new string[] { "A", "B", "C", "D" };

		#endregion

		#region Endpoints
		public Action OnQuizFinished;
		public static Action<bool, QuizItem> OnQuestionAnswered;
		public static Action<bool, int> OnQuestionAnsweredUDP;
		public Action<int, int> OnQuizFinishedData;
		public static Action<bool, Theme> OnQuestionAsked;

		public static Action OnNextQuestionStart;

		public static Action<Theme> SendTheme;

		#endregion

		public bool isInGame;
		public Coroutine SetQuestionCoroutine;
		public CanvasGroup SkipButton;
		public CanvasGroup[] RightScreenAnswers;
		public CanvasGroup RightScreenAnswerOverlay;

		public TMP_Text Explanation;

		public RawImage BigPhotoRightScreen;
		public RawImage[] PhotoContainers;

		public Animator RightScreenAnimator;

		public void Setup(Theme theme)
		{
			_theme = theme;

			ResetQuiz();
			//SetupUIElements();
			SetupQuiz();

			//ispisuje sav sadrzaj quiza u Debug.Log te indicira u kojoj temi je krivo unesen sadržaj
			if (UseCMSDebugging)
			{
				CMSDebugging();
			}
		}

		private void SetupUIElements()
		{
			Debug.Log("UI elements setup");
			//string correctText = MediaHelper.Get.GetMedia(_theme, "CorrectText", MediaType.Text);
			string correctText = _theme.GetMediaByName("CorrectText")?.Text;
			//string wrongText = MediaHelper.Get.GetMedia(_theme, "WrongText", MediaType.Text);
			string wrongText = _theme.GetMediaByName("WrongText")?.Text;

			CorrectAnswerText.text = correctText;
			WrongAnswerText.text = wrongText;
		}

		private void ResetQuiz()
		{
			Resources.UnloadUnusedAssets();

			//QuizImage.DOFade(0.0f, 0.0f);
			ShowHideAnswersRightScreen(true);
			RightScreenAnswerOverlay.DOFade(0, 1);
			QuizMainContent.DOFade(1.0f, 0.0f);
			QuizMainContent.blocksRaycasts = true;

			_numberOfMaximumQuestions = 5;
			_currentQuestion = -1;

			_wrongAnsweredCounter = 0;
			_correctAnsweredCounter = 0;

			//_numberOfMaximumQuestions = _theme.SubThemes.Length;
			QuestionCounterText.text = "0/" + _numberOfMaximumQuestions;

			isInGame = true;
		}

		private void SetupQuiz()
		{
			if (UseNumericalIndicators)
			{
				_chosenIndicator = _numericalIndicators;
			}
			else if (UseAlphabeticalIndicators)
			{
				_chosenIndicator = _alphabeticalIndicators;
			}
			else
			{
				_chosenIndicator = CustomIndicators.ToArray();
			}

			FillListWithQuizItems();
			SetAnotherQuestionContent();
		}

		// popunjava listu iz CMS-a
		public void FillListWithQuizItems()
		{
			ListOfQuizItems.Clear();

			var subThemesList = _theme.SubThemes;
			for (int x = 0; x <= subThemesList.Length - 1; x++)
			{
				Theme subTheme = subThemesList[x];

				List<string> answerlist = MediaHelper.Get.GetAllMediaContainingPartOfName(subTheme, "Answer", MediaType.Text);
				List<string> photoPathList = new List<string>();

				if (answerlist != null)
				{
					string question = "";
					var photoList = new List<Photo>();
					string imagePath = "";
					string explanation = "";

					if (ThemeHelper.Check.ContainsMedia(subTheme, "Question"))
					{
						question = MediaHelper.Get.GetMedia(subTheme, "Question", MediaType.Text);
					}
					if (ThemeHelper.Check.ContainsMedia(subTheme, "Thumbnails"))
					{
						photoList = MediaHelper.Get.GetMediaPhotos(subTheme, "Thumbnails");
					}
					if (photoList.Count > 0)
					{
						foreach (var photo in photoList)
						{
							photoPathList.Add(photo.FullPath);
						}
					}



					if (ThemeHelper.Check.ContainsMedia(subTheme, "Explanation"))
					{
						explanation = MediaHelper.Get.GetMedia(subTheme, "Explanation", MediaType.Text);
					}

					QuizItem QU = null;
					switch (answerlist.Count)
					{
						case 2:
							QU = new QuizItem(question, FindRightAnswerIndicator(subTheme), imagePath, answerlist[0], answerlist[1]);
							break;

						case 3:
							QU = new QuizItem(question, FindRightAnswerIndicator(subTheme), explanation, answerlist[0], photoPathList[0], answerlist[1], photoPathList[1], answerlist[2], photoPathList[2]);
							break;

						case 4:
							QU = new QuizItem(question, FindRightAnswerIndicator(subTheme), imagePath, answerlist[0], answerlist[1], answerlist[2], answerlist[3]);
							break;

						default:
							break;
					}
					ListOfQuizItems.Add(QU);
				}
			}
			ShuffleList();
		}

		private void ShuffleList()
		{
			System.Random random = new System.Random();
			int n = ListOfQuizItems.Count;
			QuizItem tmp;
			SubTheme theme;

			for (int i = 0; i < n; i++)
			{
				int r = i + (int)(random.NextDouble() * (n - i));

				tmp = ListOfQuizItems[r];
				ListOfQuizItems[r] = ListOfQuizItems[i];
				ListOfQuizItems[i] = tmp;

				theme = _theme.SubThemes[r];
				_theme.SubThemes[r] = _theme.SubThemes[i];
				_theme.SubThemes[i] = theme;

			}
		}
		// postavlja sljedeci content!!!
		public void SetAnotherQuestionContent()
		{
			UnityHelper.DestroyObjects(_listOfAnswerButton);
			_listOfAnswerButton.Clear();
			//if ((_currentQuestion + 1) >= _numberOfMaximumQuestions)
			//{
			//	QuizFinished();
			//	return;
			//}
			ShowHideAnswersRightScreen(true);
			BigPhotoRightScreen.color = new Color(1, 1, 1, 0);
			_currentQuestion++;

			SendTheme?.Invoke(_theme.SubThemes[_currentQuestion]);

			QuestionCounterText.text = (_currentQuestion + 1) + "/" + _numberOfMaximumQuestions;

			QuestionText.text = ListOfQuizItems[_currentQuestion].Question;




			Explanation.text = ListOfQuizItems[_currentQuestion].Explanation;





			// spawn 4 buttons
			if (!string.IsNullOrEmpty(ListOfQuizItems[_currentQuestion].Answer4))
			{
				SpawnButtons(ListOfQuizItems[_currentQuestion], 4);
			}
			// spawn3 buttons
			else if (!string.IsNullOrEmpty(ListOfQuizItems[_currentQuestion].Answer3))
			{
				SpawnButtons(ListOfQuizItems[_currentQuestion], 3);
			}
			//spawn2 buttons
			else
			{
				SpawnButtons(ListOfQuizItems[_currentQuestion], 2);
			}



		}

		// call when quiz is finished!
		private void QuizFinished()
		{

			OnQuizFinished?.Invoke();
			//OnQuizFinishedData?.Invoke(_correctAnsweredCounter, _wrongAnsweredCounter);

			//FinalPanel.DOFade(1.0f, AnimationSpeed);
			//FinalPanel.blocksRaycasts = true;

			//Debug.Log("Right answers = " + _correctAnsweredCounter);
			//Debug.Log("Wrong answers = " + _wrongAnsweredCounter);
		}

		private void SpawnButtons(QuizItem quizItem, int numberOfButtonsToSpawn)
		{
			List<string> listOfAnswersTMP = new List<string>();
			List<string> listOfPhotosTMP = new List<string>();
			listOfAnswersTMP.Clear();
			listOfPhotosTMP.Clear();

			listOfAnswersTMP.Add(quizItem.Answer1);
			listOfAnswersTMP.Add(quizItem.Answer2);
			listOfAnswersTMP.Add(quizItem.Answer3);
			listOfAnswersTMP.Add(quizItem.Answer4);

			listOfPhotosTMP.Add(quizItem.ImagePath1);
			listOfPhotosTMP.Add(quizItem.ImagePath2);
			listOfPhotosTMP.Add(quizItem.ImagePath3);
			listOfPhotosTMP.Add(quizItem.ImagePath4);

			var photoOffsetX = SetupRightScreenPhotos(_theme.SubThemes[_currentQuestion].GetMediaByName("Gallery").GetPhotos(), quizItem.RightAnswerIndicator, numberOfButtonsToSpawn);

			int containerSeparator = 0;
			for (int x = 0; x < numberOfButtonsToSpawn; x++)
			{
				// provjera kolko je buttona potrebno za spawn, ako je 4 tada se napravi separate u top i bottom container
				containerSeparator++;
				Transform chosenContainer = null;
				if (numberOfButtonsToSpawn >= 4 && containerSeparator >= 3)
				{
					chosenContainer = AnswerButtonContainerBottom;
				}
				else
				{
					chosenContainer = AnswerButtonContainerTop;
				}

				GameObject answerButton = Instantiate(AnswerButtonPrefab, chosenContainer);

				QuizAnswerButton mb = answerButton.GetComponent<QuizAnswerButton>();

				if (quizItem.RightAnswerIndicator == (x + 1))
				{
					if (!UseButtonIndicators)
					{
						mb.SetupButton("null", listOfAnswersTMP[x], () => { QuestionAnswered(true, quizItem, photoOffsetX); Debug.Log("stisno"); }, listOfPhotosTMP[x]);
					}
					else
					{
						mb.SetupButton(_chosenIndicator[x], listOfAnswersTMP[x], () => { QuestionAnswered(true, quizItem, photoOffsetX); Debug.Log("stisno"); }, listOfPhotosTMP[x]);
					}
					mb.RightAnswer = true;
				}
				else
				{
					if (!UseButtonIndicators)
					{
						mb.SetupButton("null", listOfAnswersTMP[x], () => { QuestionAnswered(false, quizItem, photoOffsetX); Debug.Log("stisno"); }, listOfPhotosTMP[x]);
					}
					else
					{
						mb.SetupButton(_chosenIndicator[x], listOfAnswersTMP[x], () => { QuestionAnswered(false, quizItem, photoOffsetX); Debug.Log("stisno"); }, listOfPhotosTMP[x]);
					}

					mb.RightAnswer = false;
				}
				_listOfAnswerButton.Add(answerButton);
			}

			OnQuestionAsked?.Invoke(false, _theme.SubThemes[_currentQuestion]);

		}

		private float SetupRightScreenPhotos(List<Photo> imageList, int rightAnswer, int numberOfAnswers)
		{
			float photoOffsetX = 0;

			for (int i = 0; i < numberOfAnswers; i++)
			{

				PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}

			for (int i = 0; i < numberOfAnswers; i++)
			{
				AssetsFileLoader.LoadTexture2D(imageList[i].FullPath, PhotoContainers[i]);
				PhotoContainers[i].SetNativeSize();

				if (i + 1 == rightAnswer)
				{
					AssetsFileLoader.LoadTexture2D(imageList[i].FullPath, BigPhotoRightScreen);
					PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.x + _theme.SubThemes[_currentQuestion].PositionX, PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.y);
					photoOffsetX = PhotoContainers[i].GetComponent<RectTransform>().anchoredPosition.x;
				}

			}


			return photoOffsetX;
		}


		// Answer button CLICK trigera sljedecu metodu
		private void QuestionAnswered(bool isAnswerRight, QuizItem quizItem, float offset)
		{
			QuizMainContent.blocksRaycasts = false;

			// provjera koji button je tocan i na njemu se pozove trigger za indikaciju dobrog odgovora
			foreach (GameObject go in _listOfAnswerButton)
			{
				QuizAnswerButton mb = go.GetComponent<QuizAnswerButton>();
				if (mb.RightAnswer == true && isAnswerRight == false)
				{
					StartCoroutine(AnsweredWrong(mb, quizItem, offset));
				}
			}

			if (isAnswerRight == true)
			{
				_correctAnsweredCounter++;

				OnQuestionAnsweredUDP?.Invoke(true, quizItem.RightAnswerIndicator);
				AnswerManager.QuestionAnswered(quizItem, offset);

			}
			else
			{
				_wrongAnsweredCounter++;
			}
			OnQuestionAnswered?.Invoke(isAnswerRight, quizItem);



			SetQuestionCoroutine = StartCoroutine(SetAnotherQuestionWithDelay());


		}

		private IEnumerator AnsweredWrong(QuizAnswerButton mb, QuizItem quizItem, float offset)
		{
			yield return new WaitForSeconds(2);
			OnQuestionAnsweredUDP?.Invoke(false, quizItem.RightAnswerIndicator);
			AnswerManager.QuestionAnswered(quizItem, offset);

			mb.IndicateAnswer();
			SkipButton.DOFade(1, 1).SetDelay(1).OnComplete(() => {
				SkipButton.blocksRaycasts = true;
				SkipButton.interactable = true;
			});

		}

		public IEnumerator SetAnotherQuestionWithDelay()
		{
			yield return new WaitForSeconds(30f);
			// TODO - QUIZ potencijalno slozit da se setdelay postavi kao editor parametar!!
			// TODO - QUIZ potencijalno slozit da se blockraycast produzi!!
			SkipButton.Fade(0, 1);
			NextQuestion();
		}

		public void SetAnotherQuestionWithoutDelay()
		{
			StopNextQuestionCoroutine();
			NextQuestion();
		}

		private void NextQuestion()
		{
			if ((_currentQuestion + 1) >= _numberOfMaximumQuestions)
			{
				QuizFinished();
				return;
			}

			OnNextQuestionStart?.Invoke();
			ShowHideAnswersRightScreen(false);
			RightScreenAnswerOverlay.DOFade(0, 1);
			QuizMainContent.DOFade(0.0f, AnimationSpeed).SetDelay(0.1f).OnComplete(() => {
				QuizMainContent.DOFade(1.0f, AnimationSpeed).SetDelay(0.1f).OnComplete(() =>
					QuizMainContent.blocksRaycasts = true); // setdelay nakon oncomplete-a!!
				SetAnotherQuestionContent();


			});
		}

		public void ShowHideAnswersRightScreen(bool show)
		{
			foreach (var answerPhoto in RightScreenAnswers)
			{
				answerPhoto.DOFade(show ? 1 : 0, 1);
			}
		}


		public void StopNextQuestionCoroutine()
		{
			if (SetQuestionCoroutine != null)
				StopCoroutine(SetQuestionCoroutine);
		}

		public int FindRightAnswerIndicator(Theme subTheme)
		{
			List<string> answerlist = MediaHelper.Get.GetAllMediaContainingPartOfName(subTheme, "Answer", MediaType.Text);

			for (int x = 0; x < answerlist.Count; x++)
			{
				string checkForRightAnswer = "";
				// TODO - QUIZ napraviti da ne baca warning u slucaju da nema medie u themi
				if (ThemeHelper.Check.ContainsPartOfMediaName(subTheme, "Answer" + (x + 1) + "*"))
				{
					checkForRightAnswer = MediaHelper.Get.GetMedia(subTheme, "Answer" + (x + 1) + "*", MediaType.Text);
				}

				//Debug.Log(checkForRightAnswer);
				if (!string.IsNullOrEmpty(checkForRightAnswer))
				{
					return (x + 1);
				}
			}
			return -1;
		}


		// testna metoda koja ispisuje sadrzaj pitanja i odgovora te sluzio kao debbugiranje ako je sadrzaj u CMS krivo postavljen
		private void CMSDebugging()
		{
			Debug.Log("_________________________QUIZ CONTENT_________________________");
			int themeCounter = 0;
			foreach (QuizItem quizItem in ListOfQuizItems)
			{
				string message = "";
				themeCounter++;

				if (quizItem?.Answer1 == null || quizItem?.Answer2 == null || quizItem?.RightAnswerIndicator == -1 || (quizItem?.Question == null /*&& quizItem?.ImagePath == null*/))
				{
					Debug.Log("<color=red>Error: </color>There is problem with setup of " + themeCounter + ". subtheme!");
				}
				else
				{
					message += "[<color=blue>Firt answer =</color> " + quizItem.Answer1 + "] [<color=blue>Second Answer =</color> " + quizItem.Answer2 + "] ";

					if (!string.IsNullOrEmpty(quizItem.Answer3))
					{
						message += " [<color=blue>Third answer =</color> " + quizItem.Answer3 + "] ";
					}
					else
					{
						message += " [<color=blue>Third Answer = There is no 3.answer</color>] ";
					}

					if (!string.IsNullOrEmpty(quizItem.Answer4))
					{
						message += " [<color=blue>Forth answer =</color> " + quizItem.Answer4 + "] ";
					}
					else
					{
						message += " [<color=blue>Forth Answer = There is no 4.answer</color>] ";
					}

					if (!string.IsNullOrEmpty(quizItem.Question))
					{
						message += " [<color=blue>Question is =</color> " + quizItem.Question + "] ";
					}

					//if (!string.IsNullOrEmpty(quizItem.ImagePath))
					//{
					//	message += " [<color=blue>Image Path =</color> " + quizItem.ImagePath + "] ";
					//}
					else
					{
						message += " [<color=blue>There is no image</color>] ";
					}

					Debug.Log(message);
				}

			}
		}

	}// end of Main class
}