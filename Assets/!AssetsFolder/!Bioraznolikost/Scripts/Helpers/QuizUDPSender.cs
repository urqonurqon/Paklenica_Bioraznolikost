using Novena.DAL;
using Novena.DAL.Model.Guide;
using Novena.Settings;
using Scripts.Quiz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class QuizUDPSender : MonoBehaviour {
	private string _leftQuizIP;
	private string _rightQuizIP;

	private void Awake()
	{
		_leftQuizIP = Settings.GetValue<string>("QuizLeft");
		_rightQuizIP = Settings.GetValue<string>("QuizRight");

		IdleTemplate.OnAllIdle += OnAllIdle;
		IdleTemplate.OnSingleIdle += OnSingleIdle;
		QuizHomeTemplate.OnEnterQuizPressed += OnEnterQuizPressed;
		Idle.OnEnterExitIdle += OnEnterExitIdle;
		QuizTemplate.OnQuizEnded += OnQuizEnded;
		Quiz.OnNextQuestionStart += OnNextQuestionStart;
		Quiz.OnQuestionAnsweredUDP += OnQuestionAnswered;
		QuizTemplate.OnQuitClicked += OnQuitClicked;
		QuizTemplate.OnRestartClicked += OnRestartClicked;
		Quiz.SendTheme += SendTheme;
		Data.OnTranslatedContentUpdated += SendLanguage;
	}

	private void SendLanguage()
	{
		FMNetworkManager.instance.SendToTarget("SendLanguage" + Data.TranslatedContent.LanguageId.ToString(), _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("SendLanguage" + Data.TranslatedContent.LanguageId.ToString(), _rightQuizIP);
	}

	private void SendTheme(Theme theme)
	{
		FMNetworkManager.instance.SendToTarget("SendTheme" + theme.Id.ToString(), _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("SendTheme" + theme.Id.ToString(), _rightQuizIP);
	}

	private void OnRestartClicked()
	{
		FMNetworkManager.instance.SendToTarget("RestartClicked", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("RestartClicked", _rightQuizIP);
	}

	private void OnQuitClicked()
	{
		FMNetworkManager.instance.SendToTarget("QuitClicked", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("QuitClicked", _rightQuizIP);
	}

	private void OnQuestionAnswered(bool isAnswerCorrect, int rightAnswerIndicator)
	{
		string correctAnswerNumber = rightAnswerIndicator.ToString();

		if (isAnswerCorrect)
		{
			FMNetworkManager.instance.SendToTarget("QuestionAnsweredCorrect " + correctAnswerNumber, _leftQuizIP);
			FMNetworkManager.instance.SendToTarget("QuestionAnsweredCorrect " + correctAnswerNumber, _rightQuizIP);
		}
		else
		{
			FMNetworkManager.instance.SendToTarget("QuestionAnsweredWrong " + correctAnswerNumber, _leftQuizIP);
			FMNetworkManager.instance.SendToTarget("QuestionAnsweredWrong " + correctAnswerNumber, _rightQuizIP);
		}
	}

	private void OnNextQuestionStart()
	{
		FMNetworkManager.instance.SendToTarget("NextQuestionStarting", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("NextQuestionStarting", _rightQuizIP);
	}

	private void OnQuizEnded()
	{
		FMNetworkManager.instance.SendToTarget("QuizEnded", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("QuizEnded", _rightQuizIP);
	}

	private void OnEnterExitIdle(bool isIdle)
	{
		if (isIdle)
		{
			FMNetworkManager.instance.SendToTarget("EnterIdle", _leftQuizIP);
			FMNetworkManager.instance.SendToTarget("EnterIdle", _rightQuizIP);
		}
		else
		{
			FMNetworkManager.instance.SendToTarget("ExitIdle", _leftQuizIP);
			FMNetworkManager.instance.SendToTarget("ExitIdle", _rightQuizIP);
		}
	}

	private void OnEnterQuizPressed()
	{
		FMNetworkManager.instance.SendToTarget("EnterQuiz", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("EnterQuiz", _rightQuizIP);
	}

	private void OnSingleIdle()
	{
		FMNetworkManager.instance.SendToTarget("SingleIdle", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("SingleIdle", _rightQuizIP);
	}

	private void OnAllIdle()
	{
		FMNetworkManager.instance.SendToTarget("AllIdle", _leftQuizIP);
		FMNetworkManager.instance.SendToTarget("AllIdle", _rightQuizIP);
	}
}
