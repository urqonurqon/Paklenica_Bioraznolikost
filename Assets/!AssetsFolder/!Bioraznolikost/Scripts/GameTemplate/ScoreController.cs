using Doozy.Engine.UI;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	[SerializeField] private ScrollRect _scrollRect;

	private int _score;
	[HideInInspector] public int MaxScore = 3;

	[HideInInspector] public bool DidPlayerWin;

	private void Awake()
	{
		GameTemplate.OnGameRestarted += RestartScore;
		ItemSlot.OnPhotoDropped += AddScore;
		ItemSlot.OnPhotoDropped += FixScrollRect;
	}

	private void FixScrollRect(bool arg1, Theme arg2)
	{
		if (_score > 0)
		{
			_scrollRect.verticalNormalizedPosition = .5f;
		}
	}

	private void AddScore(bool isRightConnection, Theme theme = null)
	{
		if (isRightConnection)
		{
			_score++;
			CheckIfPlayerWon();
		}
	}

	private void RestartScore()
	{
		_score = 0;
	}

	private void CheckIfPlayerWon()
	{
		if (_score >= MaxScore)
		{
			DidPlayerWin = true;
			_score = 0;
		}
	}
}
