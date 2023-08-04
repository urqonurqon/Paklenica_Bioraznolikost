using Assets.Scripts.Components.PinchScrollRect;
using Doozy.Engine.UI;
using Scripts.Gallery;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for "Gallery" Custom editor
/// Should be placed in folder named "Editor"
/// </summary>
[CustomEditor(typeof(Gallery))]
public class GalleryCustomEditor : Editor {

	private bool fadingPosition;
	private bool otherOptions;
	private bool imageTransitionOptions;
	private bool fullscreenOptions;

	private int	 tab;

	override public void OnInspectorGUI()
	{
		var gallery = target as Gallery;

		tab = GUILayout.Toolbar(tab, new string[] { "Settings", "Components" });
		switch (tab)
		{
			case 0:
				// change Image Settings
				#region SlideShow Settings - Section
				EditorGUILayout.Space(15);
				imageTransitionOptions = EditorGUILayout.BeginFoldoutHeaderGroup(imageTransitionOptions, "Image Transition");
				if (imageTransitionOptions)
					if (Selection.activeTransform)
					{
						gallery.UseDefault = GUILayout.Toggle(gallery.UseDefault, "Use default image transition");
						EditorGUILayout.Space(3);
						bool defaultSettings = true;
						gallery.UseOnePageSlideShow = GUILayout.Toggle(gallery.UseOnePageSlideShow, "Use one page slideShow");
						if (gallery.UseOnePageSlideShow)
						{
							defaultSettings = false;
							if (gallery.UseTwoPagesSlideShow == true)
							{
								gallery.UseTwoPagesSlideShow = false;
								gallery.UseDefault = false;
							}
						}
						gallery.UseTwoPagesSlideShow = GUILayout.Toggle(gallery.UseTwoPagesSlideShow, "Use two pages slideShow");
						if (gallery.UseTwoPagesSlideShow)
						{
							defaultSettings = false;
							if (gallery.UseOnePageSlideShow == true)
							{
								gallery.UseOnePageSlideShow = false;
								gallery.UseDefault = false;
							}
						}

						if (defaultSettings)
						{
							gallery.UseDefault = true;
						} 
						else
						{
							gallery.UseDefault = false;
						}

						// Preset Names - section
						if (gallery.UseOnePageSlideShow || gallery.UseTwoPagesSlideShow)
						{
							EditorGUILayout.Space(2);
							GuiLine(1);
							EditorGUILayout.Space(2);
							gallery.LeftShowPresetName = EditorGUILayout.TextField("LeftShow Preset Name", gallery.LeftShowPresetName);
							gallery.LeftHidePresetName = EditorGUILayout.TextField("LeftHide Preset Name", gallery.LeftHidePresetName);
							EditorGUILayout.Space(2);
							gallery.RightShowPresetName = EditorGUILayout.TextField("RightShow Preset Name", gallery.RightShowPresetName);
							gallery.RightHidePresetName = EditorGUILayout.TextField("RightHide Preset Name", gallery.RightHidePresetName);
						}

					}
				if (!Selection.activeTransform)
				{
					imageTransitionOptions = false;
				}
				EditorGUILayout.Space(10);
				EditorGUILayout.EndFoldoutHeaderGroup();
				#endregion
				// Fading Settings
				#region Fading Settings - Section
				fadingPosition = EditorGUILayout.BeginFoldoutHeaderGroup(fadingPosition, "Fade Settings");
				if (fadingPosition)
					if (Selection.activeTransform)
					{
						gallery.FadingSpeed = EditorGUILayout.FloatField(new GUIContent("Fading speed", "Time representing content speed FadeOut-FadeIn on content change"), gallery.FadingSpeed);
						if (gallery.FadingSpeed < 0.0f)
						{
							gallery.FadingSpeed = 0.0f;
						}
						EditorGUILayout.Space(3);
						gallery.FadeContentText = EditorGUILayout.Toggle("Fade Content", gallery.FadeContentText);
						gallery.FadePagerText = EditorGUILayout.Toggle("Fade Pager", gallery.FadePagerText);
						gallery.FadeCaptionText = EditorGUILayout.Toggle("Fade Caption", gallery.FadeCaptionText);
						gallery.FadeImage = EditorGUILayout.Toggle("Fade Image", gallery.FadeImage);
					}
				if (!Selection.activeTransform)
				{
					fadingPosition = false;
				}
				EditorGUILayout.Space(10);
				EditorGUILayout.EndFoldoutHeaderGroup();
				#endregion
				// advanced settings (options)
				#region Advanced Settings - Section
				otherOptions = EditorGUILayout.BeginFoldoutHeaderGroup(otherOptions, "Advanced Settings");
				if (otherOptions)
					if (Selection.activeTransform)
					{
						float valueForLabel = gallery.FadingSpeed;
						EditorGUILayout.LabelField("Value should be same or higher then " + valueForLabel);
						gallery.InteractionElementsDisableTime = EditorGUILayout.FloatField(new GUIContent("Interaction Elements Disable Time", "Representing gallery buttons disable time - time after you can click gallery button again"), gallery.InteractionElementsDisableTime);

						if (gallery.InteractionElementsDisableTime < valueForLabel)
						{
							gallery.InteractionElementsDisableTime = valueForLabel;
						}

						EditorGUILayout.Space(5);
						EditorGUILayout.LabelField("This value override all JSON data");
						gallery.OverrideImageMediaContent = GUILayout.Toggle(gallery.OverrideImageMediaContent, "Override image Content Text");
						if (gallery.OverrideImageMediaContent)
						{
							gallery._OverrideImageMediaContent = EditorGUILayout.TextField("Media text", gallery._OverrideImageMediaContent);
						}
						EditorGUILayout.Space(3);
						gallery.OverrideGalleryNameMediaText = GUILayout.Toggle(gallery.OverrideGalleryNameMediaText, "Override Gallery Name Media Text");
						if (gallery.OverrideGalleryNameMediaText)
						{
							gallery._OverrideGalleryNameMediaText = EditorGUILayout.TextField("Media text", gallery._OverrideGalleryNameMediaText);
						}
						EditorGUILayout.Space(3);
						gallery.OverrideGalleryCaptionsMediaText = GUILayout.Toggle(gallery.OverrideGalleryCaptionsMediaText, "Override Gallery Captions Media Text");
						if (gallery.OverrideGalleryCaptionsMediaText)
						{
							gallery._OverrideGalleryCaptionsMediaText = EditorGUILayout.TextField("Media text", gallery._OverrideGalleryCaptionsMediaText);
						}
					}
				if (!Selection.activeTransform)
				{
					otherOptions = false;
				}
				EditorGUILayout.EndFoldoutHeaderGroup();
				EditorGUILayout.Space(5);
				#endregion
				GuiLine(2);
				#region FullScreen Settings - Section
				gallery.UseFullscreen = GUILayout.Toggle(gallery.UseFullscreen, "Enable/Disable Fullscreen");
				if (gallery.UseFullscreen)
				{
					fullscreenOptions = EditorGUILayout.BeginFoldoutHeaderGroup(fullscreenOptions, "FullscreenSettings");
					if (fullscreenOptions)
						if (Selection.activeTransform)
						{
							gallery.FullscreenFadingSpeed = EditorGUILayout.FloatField("Fullscreen Fading speed",gallery.FullscreenFadingSpeed);
							gallery.UsePinchableZoom = GUILayout.Toggle(gallery.UsePinchableZoom, "Use PinchableZoom");
							if (gallery.UsePinchableZoom)
							{
								EditorGUILayout.BeginHorizontal();
								GUILayout.Space(15);
								EditorGUILayout.BeginVertical();

								gallery.UseDoubleTapZoomOut = GUILayout.Toggle(gallery.UseDoubleTapZoomOut, "Use Fullscreen Double Tap Zoom out");
								gallery.HideButtonsWhenPinchZoom = GUILayout.Toggle(gallery.HideButtonsWhenPinchZoom, "Hide buttons when Pinched");

								EditorGUILayout.EndVertical();
								EditorGUILayout.EndHorizontal();							

								EditorGUILayout.Space(5);
							} else
							{
								gallery.UseDoubleTapZoomOut = false;
								gallery.HideButtonsWhenPinchZoom = false;
							}

							gallery.UseFullscreenImageChange = GUILayout.Toggle(gallery.UseFullscreenImageChange, "Use Fullscreen Image Change");
							EditorGUILayout.Space(3);
							gallery.ShowFullscreenPagerText = GUILayout.Toggle(gallery.ShowFullscreenPagerText, "Show/Hide Pager");
							gallery.ShowFullscreenCaptionText = GUILayout.Toggle(gallery.ShowFullscreenCaptionText, "Show/Hide Caption");

						}
					if (!Selection.activeTransform)
					{
						fullscreenOptions = false;
					}
					EditorGUILayout.Space(15);
				}
				#endregion
				break;
			case 1:
				#region Gallery Components - Section
				EditorGUILayout.Space(15);
				EditorGUILayout.LabelField("Canvas groups :", EditorStyles.boldLabel);
				gallery.FadingContentCG = (CanvasGroup)EditorGUILayout.ObjectField("Fading Content CG", gallery.FadingContentCG, typeof(CanvasGroup), true);
				gallery.FullScreenCG = (CanvasGroup)EditorGUILayout.ObjectField("Fading Fullscreen CG", gallery.FullScreenCG, typeof(CanvasGroup), true);
				// gallery buttons
				EditorGUILayout.Space(5);
				EditorGUILayout.LabelField("UI Buttons :", EditorStyles.boldLabel);
				gallery.PreviousGalleryButton = (UIButton)EditorGUILayout.ObjectField("Previous Gallery Button", gallery.PreviousGalleryButton, typeof(UIButton), true);
				gallery.NextGalleryButton = (UIButton)EditorGUILayout.ObjectField("Next Gallery Button", gallery.NextGalleryButton, typeof(UIButton), true);
				gallery.FullscreenPreviousGalleryButton = (UIButton)EditorGUILayout.ObjectField("Fullscreen Previous Gallery Button", gallery.FullscreenPreviousGalleryButton, typeof(UIButton), true);
				gallery.FullscreenNextGalleryButton = (UIButton)EditorGUILayout.ObjectField("Fullscreen Next Gallery Button", gallery.FullscreenNextGalleryButton, typeof(UIButton), true);
				gallery.ShowFullscreenButton = (UIButton)EditorGUILayout.ObjectField("Show fullscreen Button", gallery.ShowFullscreenButton, typeof(UIButton), true);
				gallery.HideFullscreenButton = (UIButton)EditorGUILayout.ObjectField("Hide fullscreen Button", gallery.HideFullscreenButton, typeof(UIButton), true);
				// TMP Texts
				EditorGUILayout.Space(5);
				EditorGUILayout.LabelField("TMP Texts :", EditorStyles.boldLabel);
				gallery.CaptionText = (TMP_Text)EditorGUILayout.ObjectField("Caption gallery Text", gallery.CaptionText, typeof(TMP_Text), true);
				gallery.ContentText = (TMP_Text)EditorGUILayout.ObjectField("Content gallery Text", gallery.ContentText, typeof(TMP_Text), true);
				gallery.PagerText = (TMP_Text)EditorGUILayout.ObjectField("Pager gallery Text", gallery.PagerText, typeof(TMP_Text), true);
				// Fullscreen part
				gallery.FullscreenPagerText = (TMP_Text)EditorGUILayout.ObjectField("Fullscreen Pager gallery Text", gallery.FullscreenPagerText, typeof(TMP_Text), true);
				gallery.FullscreenCaptionText = (TMP_Text)EditorGUILayout.ObjectField("Fullscreen Caption gallery Text", gallery.FullscreenCaptionText, typeof(TMP_Text), true);
				// Raw Images
				EditorGUILayout.Space(5);
				EditorGUILayout.LabelField("Raw Images :", EditorStyles.boldLabel);
				gallery.ImageOne = (RawImage)EditorGUILayout.ObjectField("Image One RI", gallery.ImageOne, typeof(RawImage), true);
				gallery.ImageTwo = (RawImage)EditorGUILayout.ObjectField("Image Two RI", gallery.ImageTwo, typeof(RawImage), true);
				gallery.FullScreenImage = (RawImage)EditorGUILayout.ObjectField("Fullscreen RI", gallery.FullScreenImage, typeof(RawImage), true);
				gallery.FullScreenPinchIcon = (RawImage)EditorGUILayout.ObjectField("Fullscreen Pinch icon", gallery.FullScreenPinchIcon, typeof(RawImage), true);
				// UIViews
				EditorGUILayout.Space(5);
				EditorGUILayout.LabelField("UI Views :", EditorStyles.boldLabel);
				gallery.ImageOneUIView = (UIView)EditorGUILayout.ObjectField("Image One UIView", gallery.ImageOneUIView, typeof(UIView), true);
				gallery.ImageTwoUIView = (UIView)EditorGUILayout.ObjectField("Image Tvo UIView", gallery.ImageTwoUIView, typeof(UIView), true);
				// Scroll rects
				EditorGUILayout.Space(3);
				GuiLine(2);
				EditorGUILayout.LabelField("REFERENCES: ", EditorStyles.boldLabel);
				gallery.FullscreenPinchableScrollRect = (PinchableScrollRect)EditorGUILayout.ObjectField("Pinchable Scroll Rect", gallery.FullscreenPinchableScrollRect, typeof(PinchableScrollRect), true);
				gallery.ContentTextScrollBar = (Scrollbar)EditorGUILayout.ObjectField("Content text Scroll Rect", gallery.ContentTextScrollBar, typeof(Scrollbar), true);
				gallery.FullscreenGestureListener = (GameObject)EditorGUILayout.ObjectField("Fullscreen gesture Listener", gallery.FullscreenGestureListener, typeof(GameObject), true);
				// buttons Canvas Group reference to avoid "GO.Find()"
				gallery.PrevButtonCG = (CanvasGroup)EditorGUILayout.ObjectField("Fullscreen PreviousButton CG", gallery.PrevButtonCG, typeof(CanvasGroup), true);
				gallery.NextButtonCG = (CanvasGroup)EditorGUILayout.ObjectField("Fullscreen NextButton CG", gallery.NextButtonCG, typeof(CanvasGroup), true);
				// containers reference to avoid "GO.Find()"
				gallery.ContentContainer = (GameObject)EditorGUILayout.ObjectField("ContentContainer", gallery.ContentContainer, typeof(GameObject), true);
				gallery.PagerContainer = (GameObject)EditorGUILayout.ObjectField("PagerContainer", gallery.PagerContainer, typeof(GameObject), true);
				gallery.CaptionContainer = (GameObject)EditorGUILayout.ObjectField("CaptionContainer", gallery.CaptionContainer, typeof(GameObject), true);
				gallery.ImageContainer = (GameObject)EditorGUILayout.ObjectField("ImageContainer", gallery.ImageContainer, typeof(GameObject), true);
				EditorGUILayout.Space(15);
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

