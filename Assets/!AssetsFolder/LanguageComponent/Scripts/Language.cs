using _AssetsFolder.BaseExample.Scripts;
using Novena.DAL;
using Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paklenica {
	public class Language : MonoBehaviour {

		[Header("Language components")]
		[SerializeField] private Transform languageBtnContainer;
		[SerializeField] private GameObject languagePrefab;

		private List<GameObject> _languageItems = new List<GameObject>();

		public void GenerateLanguageButtons()
		{
			UnityHelper.DestroyObjects(_languageItems);

			for (int i = 0; i < Data.Guide.TranslatedContents.Length; i++)
			{
				GameObject btn = Instantiate(languagePrefab, languageBtnContainer);
				LanguageButton languageButton = btn.GetComponent<LanguageButton>();
				languageButton.Setup(Data.Guide.TranslatedContents[i]);
				_languageItems.Add(btn);
			}

		}
	}
}