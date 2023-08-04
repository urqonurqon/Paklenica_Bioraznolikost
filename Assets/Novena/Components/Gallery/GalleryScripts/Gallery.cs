using Assets.Scripts.Components.PinchScrollRect;
using DG.Tweening;
using Doozy.Engine.Touchy;
using Doozy.Engine.UI;
using Newtonsoft.Json;
using Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Novena.DAL.Model.Guide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gallery {
	public class Gallery : MonoBehaviour {

		#region Public fields for Custom Editor
		// adjustable settings
		public float FadingSpeed = 0.0f;
		public float FullscreenFadingSpeed = 0.0f;
		public float InteractionElementsDisableTime = 0.0f;
		// override media with same media Text
		// other settings
		public bool OverrideImageMediaContent;
		public string _OverrideImageMediaContent;
		// override gallery name loading Media
		public bool OverrideGalleryNameMediaText;
		public string _OverrideGalleryNameMediaText;
		// override gallery captions text
		public bool OverrideGalleryCaptionsMediaText;
		public string _OverrideGalleryCaptionsMediaText;
		// fullscreen options
		public bool UseFullscreen;
		public bool UsePinchableZoom;
		public bool UseDoubleTapZoomOut;
		public bool UseFullscreenImageChange;
		public bool HideButtonsWhenPinchZoom;
		public bool ShowFullscreenPagerText;
		public bool ShowFullscreenCaptionText;
		// Fading Options
		public bool FadeContentText;
		public bool FadePagerText;
		public bool FadeCaptionText;
		public bool FadeImage;
		// SlideShow Options
		public bool UseDefault;
		public bool UseOnePageSlideShow;
		public bool UseTwoPagesSlideShow;
		// gallery Components
		public CanvasGroup FadingContentCG;
		public CanvasGroup FullScreenCG;
		// gallery button section
		public UIButton PreviousGalleryButton;
		public UIButton NextGalleryButton;
		// gallery button fullscreen
		public UIButton FullscreenPreviousGalleryButton;
		public UIButton FullscreenNextGalleryButton;
		// gallery button helpers
		public UIButton ShowFullscreenButton;
		public UIButton HideFullscreenButton;
		// gallery Text section
		public TMP_Text CaptionText;
		public TMP_Text ContentText;
		public TMP_Text PagerText;
		public TMP_Text FullscreenPagerText;
		public TMP_Text FullscreenCaptionText;
		// Gallery presets names
		public string LeftShowPresetName;
		public string LeftHidePresetName;
		public string RightShowPresetName;
		public string RightHidePresetName;
		// Gallery Images
		public RawImage ImageOne;
		public RawImage ImageTwo;
		public RawImage FullScreenImage;
		public RawImage FullScreenPinchIcon;
		// UI views
		public UIView ImageOneUIView;
		public UIView ImageTwoUIView;

		// REFERENCES
		// fullscreen pinchable rect
		public PinchableScrollRect FullscreenPinchableScrollRect;
		// scroolRect content
		public Scrollbar ContentTextScrollBar;
		// RectTransform of GestureListener content
		public RectTransform GestureListenerContent;
		// GestureListener fullscreen
		public GameObject FullscreenGestureListener;
		// buttons Canvas groups
		public CanvasGroup PrevButtonCG;
		public CanvasGroup NextButtonCG;
		// FadingContent
		public GameObject ContentContainer;
		public GameObject PagerContainer;
		public GameObject CaptionContainer;
		public GameObject ImageContainer;
		#endregion

		#region Private gallery fields
		private const string UNCATEGORIZED = "Uncategorized";
		private const string DEFAULT = "Default";
		private const string GALLERY = "Gallery";

		private Theme _theme;
		private List<Photo> _photoList;
		private List<ImageInfo.Root> _imagesInfo;
		private int _currentPageIndex;
		private string _overrideContentText;
		private bool _imageOneIsActive;
		private bool _swipeEnable;
		private bool _lastClicked;
		private bool _fullscreenOpened;
		private CanvasGroup _fullScreenImageCG;

		private CanvasGroup _thisCanvasGroup;
		#endregion

		// Gallery starting point
		// Called out of Gallery
		public void Setup(Theme theme)
		{
			_theme = theme;

			ResetGallery();
			SetupGallery();

			_thisCanvasGroup.DOFade(1.0f, 0.0f);

			// custom testing
			//test();
		}

		private void test()
		{
			//UIView test = UIView.ShowView(GALLERY, LeftShowPresetName,true);
			List<UIView> test = UIView.GetViews(GALLERY,"ImageOne");
			test[0].ShowBehavior.Animation.Move.Duration = 1.0f;
		}

		// reset to start settings
		private void ResetGallery()
		{
			_photoList = new List<Photo>();
			_imagesInfo = new List<ImageInfo.Root>();

			_imageOneIsActive = true;
			_lastClicked = true;
			_swipeEnable = true;
			_fullscreenOpened = false;

			FullScreenCG.DOFade(0.0f, 0.0f);
			FullScreenCG.blocksRaycasts = false;

			FullscreenPinchableScrollRect.activate = true;
			FullscreenPinchableScrollRect._currentZoom = 1.0f;

			_fullScreenImageCG = FullScreenImage.GetComponent<CanvasGroup>();
			_thisCanvasGroup = GetComponent<CanvasGroup>();

			_currentPageIndex = 1;
		}
		// called once to Load/Initialized gallery settings, must call before "SetupGallery"
		private void SetupGalleryEditorSettings()
		{
			if (UseOnePageSlideShow || UseTwoPagesSlideShow)
			{
				ImageOneUIView.InstantHide();
				ImageTwoUIView.InstantHide();
			} 
			else if (UseOnePageSlideShow == false || UseTwoPagesSlideShow == false)
			{
				ImageTwoUIView.InstantHide();
			}
			else
			{
				ImageOneUIView.ShowBehavior.LoadPreset(UNCATEGORIZED, DEFAULT);
				ImageOneUIView.HideBehavior.LoadPreset(UNCATEGORIZED, DEFAULT);

				ImageTwoUIView.ShowBehavior.LoadPreset(UNCATEGORIZED, DEFAULT);
				ImageTwoUIView.HideBehavior.LoadPreset(UNCATEGORIZED, DEFAULT);
			}

			bool fadingNeededChecker = FadeCaptionText || FadeContentText || FadePagerText || FadeImage;
			if (FadeContentText)
			{
				ContentContainer.transform.SetParent(FadingContentCG.transform);
			}
			if (FadePagerText)
			{
				PagerContainer.transform.SetParent(FadingContentCG.transform);
			}
			if (FadeCaptionText)
			{
				CaptionContainer.transform.SetParent(FadingContentCG.transform);
			}
			if (FadeImage)
			{
				ImageContainer.transform.SetParent(FadingContentCG.transform);
			}

			if (fadingNeededChecker == false)
			{
				FadingSpeed = 0.0f;
			}

			if (UseFullscreen == false)
			{
				ShowFullscreenButton.gameObject.SetActive(false);
			}
			else
			{
				if (UsePinchableZoom == false)
				{
					FullscreenPinchableScrollRect.enabled = false;
					FullScreenPinchIcon.gameObject.SetActive(false);
				}
				if (UseFullscreenImageChange == false)
				{
					HideFullscreenButtons();

					GestureListener[] gls = FullscreenGestureListener.GetComponents<GestureListener>();

					foreach (GestureListener gl in gls)
					{
						gl.enabled = false;
					}
				}
				if (ShowFullscreenCaptionText == false)
				{
					Transform go = FullscreenCaptionText.transform.parent;
					go.gameObject.SetActive(false);
				}
				if (ShowFullscreenPagerText == false)
				{
					Transform go = FullscreenPagerText.transform.parent;
					go.gameObject.SetActive(false);
				}
			}
			if (OverrideImageMediaContent)
			{
				_overrideContentText = _theme.GetMediaByName(_OverrideImageMediaContent)?.Text;
				//_overrideContentText = MediaHelper.Get.GetMedia(_theme, _OverrideImageMediaContent, MediaType.Text);
			}
		}
		// Setup buttons, Load images and caption from Theme...
		private void SetupGallery()
		{
			SetupGalleryEditorSettings();

			SetupInteractionElements();
			SetupImagesInfo();
			SetupPhotos();

			LoadNewImage();

			_thisCanvasGroup.DOFade(1.0f, FadingSpeed);
		}

		private void SetupInteractionElements()
		{
			PreviousGalleryButton.OnClick.OnTrigger.Event.RemoveAllListeners();
			FullscreenPreviousGalleryButton.OnClick.OnTrigger.Event.RemoveAllListeners();
			NextGalleryButton.OnClick.OnTrigger.Event.RemoveAllListeners();
			FullscreenNextGalleryButton.OnClick.OnTrigger.Event.RemoveAllListeners();
			ShowFullscreenButton.OnClick.OnTrigger.Event.RemoveAllListeners();
			HideFullscreenButton.OnClick.OnTrigger.Event.RemoveAllListeners();

			PreviousGalleryButton.OnClick.OnTrigger.Event.AddListener(PreviousImage);
			FullscreenPreviousGalleryButton.OnClick.OnTrigger.Event.AddListener(PreviousImage);
			NextGalleryButton.OnClick.OnTrigger.Event.AddListener(NextImage);
			FullscreenNextGalleryButton.OnClick.OnTrigger.Event.AddListener(NextImage);
			ShowFullscreenButton.OnClick.OnTrigger.Event.AddListener(ShowFullScreen);
			HideFullscreenButton.OnClick.OnTrigger.Event.AddListener(HideFullscreen);

			// Fullscreen button double click
			if (UseDoubleTapZoomOut)
			{
				UIButton fullscreenDoubleClick = FullscreenGestureListener.transform.GetComponent<UIButton>();
				fullscreenDoubleClick.OnDoubleClick.OnTrigger.Event.RemoveAllListeners();
				fullscreenDoubleClick.OnDoubleClick.OnTrigger.Event.AddListener(DoubleTapOnFullscreenImage);
			}
			if (HideButtonsWhenPinchZoom)
			{
				// postavljanje metode koja provjerava onvalue change za sakrivanje buttona opcija
				FullscreenPinchableScrollRect.onValueChanged.RemoveAllListeners();
				FullscreenPinchableScrollRect.onValueChanged.AddListener((value) => PinchableScrollRectValueChange(value));
			}
		}

		private void SetupImagesInfo()
		{
			string tmp = "";
			if (OverrideGalleryCaptionsMediaText)
			{
				//tmp = MediaHelper.Get.GetMedia(_theme, _OverrideGalleryCaptionsMediaText, MediaType.Text);
				tmp = _theme.GetMediaByName(_OverrideGalleryCaptionsMediaText)?.Text;
			} else
			{
				//tmp = MediaHelper.Get.GetMedia(_theme, "ImageCaptions", MediaType.Text);
				tmp = _theme.GetMediaByName("ImageCaptions")?.Text;
			}

			List<ImageInfo.Root> imagesInfo = JsonConvert.DeserializeObject<List<ImageInfo.Root>>(tmp);
			_imagesInfo = imagesInfo;
		}

		private void SetupPhotos()
		{
			if (OverrideGalleryNameMediaText)
			{
				//_photoList = MediaHelper.Get.GetMediaPhotos(_theme, _OverrideGalleryNameMediaText);
				_photoList = _theme.GetMediaByName(_OverrideGalleryNameMediaText)?.GetPhotos();
			} else
			{
				//_photoList = MediaHelper.Get.GetMediaPhotos(_theme, GALLERY);
				_photoList = _theme.GetMediaByName(GALLERY)?.GetPhotos();
			}

			_photoList = _photoList.OrderBy(p => p.Rank).ToList();
		}

		private void LoadNewImage(bool pageIndexIncrement = false)
		{
			SetPager();

			// switcher between fullscreen fading speed and classic content fading speed!!
			// used in loadContent and Fading CG-s
			float fadingSpeedSwitcher = 0.0f;
			if (_fullscreenOpened)
			{
				fadingSpeedSwitcher = FullscreenFadingSpeed;
			} 
			else
			{
				fadingSpeedSwitcher = FadingSpeed;
			}

			_fullScreenImageCG.DOFade(0.0f, fadingSpeedSwitcher);

			if (UseOnePageSlideShow == true && (UseOnePageSlideShow && UseTwoPagesSlideShow) == false)
			{
				ImageOneUIView.Hide();

				if (pageIndexIncrement)
				{
					ImageOneUIView.ShowBehavior.LoadPreset(GALLERY, RightShowPresetName);
					ImageOneUIView.HideBehavior.LoadPreset(GALLERY, LeftHidePresetName);
				}
				else
				{
					ImageOneUIView.ShowBehavior.LoadPreset(GALLERY, LeftShowPresetName);
					ImageOneUIView.HideBehavior.LoadPreset(GALLERY, RightHidePresetName);
				}

				FadingContentCG.DOFade(0.0f, fadingSpeedSwitcher).OnComplete(
					() => {
						LoadContent();
						if (UseOnePageSlideShow)
						{
							ImageOneUIView.Show();
						}
						FadingContentCG.DOFade(1.0f, fadingSpeedSwitcher);
					});
			} 
			else if (UseTwoPagesSlideShow)
			{
				// ako je prijasnji klik jednak sadasnjem (next-next) tada stavlja
				// boolean flag na last clicked tako da ne switch-a preset animacije
				if (_lastClicked != pageIndexIncrement)
				{
					_lastClicked = pageIndexIncrement;
				} else
				{
					if (_lastClicked)
					{
						_lastClicked = false;
						_imageOneIsActive = true;
					} else
					{
						_lastClicked = true;
						_imageOneIsActive = false;
					}
				}
				// ulazi ako se ide udesno
				if (pageIndexIncrement)
				{
					// ulazi ako je first page aktivan
					if (_imageOneIsActive)
					{
						ImageOneUIView.ShowBehavior.LoadPreset(GALLERY, RightShowPresetName);
						ImageTwoUIView.HideBehavior.LoadPreset(GALLERY, LeftHidePresetName);
					}
					// ulazi ako je second screen aktivan
					else
					{
						ImageTwoUIView.ShowBehavior.LoadPreset(GALLERY, RightShowPresetName);
						ImageOneUIView.HideBehavior.LoadPreset(GALLERY, LeftHidePresetName);
					}

					FadingContentCG.DOFade(0.0f, fadingSpeedSwitcher).OnComplete(
						() => {
							LoadDualContent(pageIndexIncrement, _imageOneIsActive);
							FadingContentCG.DOFade(1.0f, fadingSpeedSwitcher);

							if (_imageOneIsActive)
							{
								_imageOneIsActive = false;
								ImageOneUIView.Show();
								ImageTwoUIView.Hide();
							} else
							{
								_imageOneIsActive = true;
								ImageOneUIView.Hide();
								ImageTwoUIView.Show();
							}
						});
				}
				// ulazi ako se ide ulijevo
				else
				{
					// ulazi ako je first page aktivan
					if (_imageOneIsActive)
					{
						ImageOneUIView.ShowBehavior.LoadPreset(GALLERY, LeftShowPresetName);
						ImageTwoUIView.HideBehavior.LoadPreset(GALLERY, RightHidePresetName);
					}
					// ulazi ako first page nije aktivan
					else
					{
						ImageTwoUIView.ShowBehavior.LoadPreset(GALLERY, LeftShowPresetName);
						ImageOneUIView.HideBehavior.LoadPreset(GALLERY, RightHidePresetName);

					}

					FadingContentCG.DOFade(0.0f, fadingSpeedSwitcher).OnComplete(
						() => {
							LoadDualContent(pageIndexIncrement, _imageOneIsActive);
							FadingContentCG.DOFade(1.0f, fadingSpeedSwitcher);
							if (_imageOneIsActive)
							{
								_imageOneIsActive = false;
								ImageOneUIView.Show();
								ImageTwoUIView.Hide();
							} else
							{
								_imageOneIsActive = true;
								ImageOneUIView.Hide();
								ImageTwoUIView.Show();
							}
						});
				}
			} 
			else
			{
				FadingContentCG.DOFade(0.0f, fadingSpeedSwitcher).OnComplete(
					() => {
						LoadContent();
						FadingContentCG.DOFade(1.0f, fadingSpeedSwitcher);
					});
			}
			// metoda ucitava content u jednu sliku!!!!
			void LoadContent()
			{
				_fullScreenImageCG.DOFade(1.0f, (fadingSpeedSwitcher));

				ContentTextScrollBar.value = 1.0f;

				int loadOnCurrentIndex = _currentPageIndex - 1;

				// postavlja sliku na fullscreen
				AssetsFileLoader.LoadTexture2D(_photoList[loadOnCurrentIndex].FullPath, FullScreenImage);

				AssetsFileLoader.LoadTexture2D(_photoList[loadOnCurrentIndex].FullPath, ImageOne);
				CaptionText.text = _imagesInfo[loadOnCurrentIndex].ImageCaption;
				FullscreenCaptionText.text = _imagesInfo[loadOnCurrentIndex].ImageCaption;

				if (OverrideImageMediaContent)
				{
					ContentText.text = _overrideContentText;
				} else
				{
					ContentText.text = _imagesInfo[loadOnCurrentIndex].ImageContentText;
				}
			}
			// metoda ucitava dual content u obje slike , u slucaju ako se koristi 2 site slideshow
			void LoadDualContent(bool pageIndexIncrement, bool imageOneIsActive) // ako se ide lijevo tada se treba puniti iz liste -1....???
			{
				_fullScreenImageCG.DOFade(1.0f, (fadingSpeedSwitcher));

				int loadOnCurrentIndex = _currentPageIndex - 1;

				ContentTextScrollBar.value = 1.0f;

				// postavlja sliku na fullscreen
				AssetsFileLoader.LoadTexture2D(_photoList[loadOnCurrentIndex].FullPath, FullScreenImage);

				if (imageOneIsActive)
				{
					AssetsFileLoader.LoadTexture2D(_photoList[loadOnCurrentIndex].FullPath, ImageOne);
				} else
				{
					AssetsFileLoader.LoadTexture2D(_photoList[loadOnCurrentIndex].FullPath, ImageTwo);
				}

				CaptionText.text = _imagesInfo[loadOnCurrentIndex].ImageCaption;
				FullscreenCaptionText.text = _imagesInfo[loadOnCurrentIndex].ImageCaption;

				if (OverrideImageMediaContent)
				{
					ContentText.text = _overrideContentText;
				} else
				{
					ContentText.text = _imagesInfo[loadOnCurrentIndex].ImageContentText;
				}
			}

			Resources.UnloadUnusedAssets();
		}

		private void SetPager()
		{
			PagerText.text = _currentPageIndex + "/" + _imagesInfo.Count;
			FullscreenPagerText.text = _currentPageIndex + "/" + _imagesInfo.Count;
		}

		// called on button click
		public void PreviousImage()
		{
			//_swipeEnable == false ||
			if (_swipeEnable == false || FullscreenPinchableScrollRect._currentZoom > 1.0f)
			{
				return;
			}
			else
			{
				_swipeEnable = false;
				StartCoroutine(EnableSwipeAfterDelay((InteractionElementsDisableTime + 0.2f)));
			}

			DisableButtonsClick();

			SetImageIndexThenLoadImage(false);
		}

		// called on button click
		public void NextImage()
		{
			if (_swipeEnable == false || FullscreenPinchableScrollRect._currentZoom > 1.0f)
			{
				return;
			} else
			{
				_swipeEnable = false;
				StartCoroutine(EnableSwipeAfterDelay((InteractionElementsDisableTime + 0.2f)));
			}

			DisableButtonsClick();

			SetImageIndexThenLoadImage(true);
		}

		private void SetImageIndexThenLoadImage(bool pageIndexIncrement)
		{
			int maxPageIndex = _imagesInfo.Count;

			if (pageIndexIncrement == true)
			{
				_currentPageIndex += 1;
				if (_currentPageIndex > maxPageIndex)
				{
					_currentPageIndex = 1;
				}
			} else
			{
				_currentPageIndex -= 1;
				if (_currentPageIndex < 1)
				{
					_currentPageIndex = maxPageIndex;
				}
			}

			LoadNewImage(pageIndexIncrement);
		}

		private void DisableButtonsClick()
		{
			PreviousGalleryButton.DisableButton(InteractionElementsDisableTime);
			NextGalleryButton.DisableButton(InteractionElementsDisableTime);

			if (UseFullscreen)
			{
				ShowFullscreenButton.DisableButton(InteractionElementsDisableTime);
				HideFullscreenButton.DisableButton(InteractionElementsDisableTime);

				if (UseFullscreenImageChange)
				{
					FullscreenPreviousGalleryButton.DisableButton(InteractionElementsDisableTime);
					FullscreenNextGalleryButton.DisableButton(InteractionElementsDisableTime);
				}
			}
		}

		IEnumerator EnableSwipeAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			_swipeEnable = true;
		}

		public void ShowFullScreen()
		{
			FullScreenCG.DOFade(1.0f, FullscreenFadingSpeed+0.15f);
			FullScreenCG.blocksRaycasts = true;

			_fullscreenOpened = true;

			FullscreenPinchableScrollRect._currentZoom = 1.0f;
		}
		private void HideFullscreen()
		{
			FullScreenCG.DOFade(0.0f, FullscreenFadingSpeed + 0.15f);
			FullScreenCG.blocksRaycasts = false;

			_fullscreenOpened = false;

			FullscreenPinchableScrollRect._currentZoom = 1.0f;
		}

		private void DoubleTapOnFullscreenImage()
		{
			if (FullscreenPinchableScrollRect._currentZoom != 1.0f)
			{
				FullscreenPinchableScrollRect._currentZoom = 1.0f;
			}
		}

		public void PinchableScrollRectValueChange(Vector2 value)
		{
			if (FullscreenPinchableScrollRect._currentZoom <= 1.0f)
			{
				ShowFullscreenButtons();
			} else
			{
				HideFullscreenButtons();
			}
		}

		private void HideFullscreenButtons()
		{
			PrevButtonCG.DOFade(0.0f, 0.25f);
			PrevButtonCG.blocksRaycasts = false;

			NextButtonCG.DOFade(0.0f, 0.25f);
			NextButtonCG.blocksRaycasts = false;
		}

		private void ShowFullscreenButtons()
		{
			PrevButtonCG.DOFade(1.0f, 0.25f);
			PrevButtonCG.blocksRaycasts = true;

			NextButtonCG.DOFade(1.0f, 0.25f);
			NextButtonCG.blocksRaycasts = true;
		}

	}// end of Gallery class
}// end of namespace
