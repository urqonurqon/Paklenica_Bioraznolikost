using Doozy.Engine.UI;
using Scripts.Quiz;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for "Quiz" Custom editor
/// Should be placed in folder named "Editor"
/// </summary>
[CustomEditor(typeof(Quiz))]
public class QuizCustomEditor : Editor {

	private int tab;

	override public void OnInspectorGUI()
	{
		this.serializedObject.Update();

		var quiz = target as Quiz;

		tab = GUILayout.Toolbar(tab, new string[] { "Settings", "Components" });
		switch (tab)
		{
			//GuiLine(2);		
			case 0:
				#region Animation settings - Section
				EditorGUILayout.Space(15);
				EditorGUILayout.LabelField("Quiz settings: ", EditorStyles.boldLabel);
				quiz.AnimationSpeed = EditorGUILayout.FloatField("Fading speed", quiz.AnimationSpeed);
				quiz.AnimationDelay = EditorGUILayout.FloatField("Fading delay speed", quiz.AnimationDelay);

				if (quiz.AnimationDelay <= quiz.AnimationSpeed)
				{
					quiz.AnimationDelay = quiz.AnimationSpeed + 0.25f;
				}

				GuiLine(1);

				quiz.UseButtonIndicators = GUILayout.Toggle(quiz.UseButtonIndicators, "Use ButtonIndicator");
				if (quiz.UseButtonIndicators)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(15);
					EditorGUILayout.BeginVertical();

					GUILayout.Space(5);
					quiz.UseNumericalIndicators = GUILayout.Toggle(quiz.UseNumericalIndicators, "Use Numerical Indicators");
					if (quiz.UseNumericalIndicators)
					{
						quiz.UseAlphabeticalIndicators = false;
					}

					quiz.UseAlphabeticalIndicators = GUILayout.Toggle(quiz.UseAlphabeticalIndicators, "Use Alphabetical Indicators");
					if (quiz.UseAlphabeticalIndicators)
					{
						quiz.UseNumericalIndicators = false;
					}

					EditorGUILayout.EndVertical();
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space(5);
				} else
				{
					quiz.UseAlphabeticalIndicators = false;
					quiz.UseNumericalIndicators = false;
				}

				GuiLine(1);

				quiz.UseCMSDebugging = GUILayout.Toggle(quiz.UseCMSDebugging, "Use CMS Debugging");


				#endregion

				break;
			case 1:
				#region Quiz components - Section
				EditorGUILayout.Space(15);
				//EditorGUILayout.LabelField("Canvas groups :", EditorStyles.boldLabel);
				quiz.QuizMainContent = (CanvasGroup)EditorGUILayout.ObjectField("Quiz entire Content", quiz.QuizMainContent, typeof(CanvasGroup), true);

				quiz.AnswerManager = (QuizAnsweredPanelManager)EditorGUILayout.ObjectField("Answered panel", quiz.AnswerManager, typeof(QuizAnsweredPanelManager), true);

				quiz.QuestionText = (TMP_Text)EditorGUILayout.ObjectField("Question text", quiz.QuestionText, typeof(TMP_Text), true);
				quiz.QuestionCounterText = (TMP_Text)EditorGUILayout.ObjectField("Question Counter Text", quiz.QuestionCounterText, typeof(TMP_Text), true);

				quiz.QuizImage = (RawImage)EditorGUILayout.ObjectField("Image", quiz.QuizImage, typeof(RawImage), true);

				quiz.AnswerButtonPrefab = (GameObject)EditorGUILayout.ObjectField("Answer button prefab", quiz.AnswerButtonPrefab, typeof(GameObject), true);

				quiz.AnswerButtonContainerTop = (Transform)EditorGUILayout.ObjectField("Answer button container TOP", quiz.AnswerButtonContainerTop, typeof(Transform), true);
				quiz.AnswerButtonContainerBottom = (Transform)EditorGUILayout.ObjectField("Answer button container BOT", quiz.AnswerButtonContainerBottom, typeof(Transform), true);

				quiz.FinalPanel = (RectTransform)EditorGUILayout.ObjectField("Final panel", quiz.FinalPanel, typeof(RectTransform), true);
				quiz.SkipButton = (CanvasGroup)EditorGUILayout.ObjectField("Skip Button", quiz.SkipButton, typeof(CanvasGroup), true);
				quiz.RightScreenAnswerOverlay = (CanvasGroup)EditorGUILayout.ObjectField("Right Overlay", quiz.RightScreenAnswerOverlay, typeof(CanvasGroup), true);

				quiz.Explanation = (TMP_Text)EditorGUILayout.ObjectField("Correct Content", quiz.Explanation, typeof(TMP_Text), true);
				
				var serializedObject = new SerializedObject(target);
				var property = serializedObject.FindProperty("RightScreenAnswers");
				serializedObject.Update();
				EditorGUILayout.PropertyField(property, true);
				serializedObject.ApplyModifiedProperties();
				
				var serializedObject1 = new SerializedObject(target);
				var property1 = serializedObject1.FindProperty("PhotoContainers");
				serializedObject1.Update();
				EditorGUILayout.PropertyField(property1, true);
				serializedObject1.ApplyModifiedProperties();


				quiz.BigPhotoRightScreen = (RawImage)EditorGUILayout.ObjectField("Right screen big photo", quiz.BigPhotoRightScreen, typeof(RawImage), true);
				quiz.RightScreenAnimator = (Animator)EditorGUILayout.ObjectField("Right screen animator", quiz.RightScreenAnimator, typeof(Animator), true);

				EditorGUILayout.Space(5);
				GuiLine(1);
				EditorGUILayout.Space(5);
				quiz.CorrectAnswerText = (TMP_Text)EditorGUILayout.ObjectField("Correct Text", quiz.CorrectAnswerText, typeof(TMP_Text), true);
				quiz.WrongAnswerText = (TMP_Text)EditorGUILayout.ObjectField("Wrong Text", quiz.WrongAnswerText, typeof(TMP_Text), true);

				#endregion

				break;

		}

		// method to draw horizontal line in editor depends on size parameter
		void GuiLine(int i_height = 1)
		{
			Rect rect = EditorGUILayout.GetControlRect(false, i_height);
			rect.height = i_height;
			EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
		}
	}
}// End of Custom Gallery Editor Class

