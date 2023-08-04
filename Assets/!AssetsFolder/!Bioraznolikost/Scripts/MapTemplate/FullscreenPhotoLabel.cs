using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FullscreenPhotoLabel : MonoBehaviour {
	[SerializeField] private TMP_Text _label;
	[SerializeField] private TMP_Text _speciesName;

	public void SetLabel()
	{
		_label.text = _speciesName.text.Split('\n')[0];
	}
}
