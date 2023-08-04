using RenderHeads.Media.AVProVideo;
using Scripts.Quiz;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Idle : MonoBehaviour {

	public static Action<bool> OnEnterExitIdle;

	[SerializeField] private float timeToGoIdle = 120.0f;
	[SerializeField] private MediaPlayer _mediaPlayer;
	private float counter = 0.0f;
	public bool IsOnIdleScreen = false;

	private Quiz _quiz;
	private QuizTemplate _quizTemplate;
	private void Start()
	{
		_quiz = FindObjectOfType<Quiz>();
		_quizTemplate = FindObjectOfType<QuizTemplate>();
	}

	void Update()
	{
		counter += Time.deltaTime;

		if (counter >= timeToGoIdle && !IsOnIdleScreen)
		{
			counter = 0.0f;
			Doozy.Engine.GameEventMessage.SendEvent("GoToIdle");
			IsOnIdleScreen = true;
			OnEnterExitIdle?.Invoke(true);
			RestartWholeGame();
		}
		bool isMediaPlaying = false;
		if (_mediaPlayer != null)
			isMediaPlaying = _mediaPlayer.Control.IsPlaying();

		if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 || isMediaPlaying)
		{
			if (_quiz != null)
			{
				if (!_quiz.isInGame && IsOnIdleScreen)
				{
					_quizTemplate.QuitGame(null);
				}
			}
			counter = 0.0f;
			IsOnIdleScreen = false;
			OnEnterExitIdle?.Invoke(false);

		}
	}

	public void RestartWholeGame()
	{
		Resources.UnloadUnusedAssets();
	}


}
