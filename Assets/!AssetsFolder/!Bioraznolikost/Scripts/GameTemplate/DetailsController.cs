using CGH;
using Doozy.Engine.UI;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour {
	public static Action<bool> OnDetailsValueChanged;


	[SerializeField] private TMP_Text _detailsText;
	[SerializeField] private TMP_Text _detailsTextName;
	[SerializeField] private TMP_Text _detailsTextLatinName;
	[SerializeField] private GameTemplateGallery _gameTemplateGallery;

	[SerializeField] private RectTransform _gradientRect;

	[SerializeField] private CanvasGroup _popup;



	[Header("GameTemplate")]
	[SerializeField] private UIButton _closeDetails;


	private Texture2D _tex;

	private void Awake()
	{
		_closeDetails.OnClick.OnTrigger.Event.AddListener(DetailsClosed);
		GameTemplate.OnDetailsShow += SetupDetails;
	}

	private void SetupDetails(Theme theme)
	{
		_popup.Hide();
		StartCoroutine(TakeScreenshotAsTexture());

		_gradientRect.gameObject.SetActive(false);
		_gradientRect.gameObject.SetActive(true);

		_gameTemplateGallery.Theme = theme;
		_gameTemplateGallery.SetupGallery();
		_detailsTextName.text = theme.Name;
		_detailsTextLatinName.text = theme.GetMediaByName("LatinName").Text;
		_detailsText.text = theme.GetMediaByName("Content").Text;
	}

	private void DetailsClosed()
	{
		OnDetailsValueChanged?.Invoke(false);
		Destroy(_tex);
	}

	private IEnumerator TakeScreenshotAsTexture()
	{
		yield return new WaitForEndOfFrame();
		var cam = Camera.main;
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
		cam.targetTexture = rt;
		_tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		cam.Render();
		RenderTexture.active = rt;
		_tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		cam.targetTexture = null;
		RenderTexture.active = null;
		Destroy(rt);
		_tex.Apply();
		_gradientRect.gameObject.GetComponent<RawImage>().texture = _tex;
		_popup.Show();
	}

}
