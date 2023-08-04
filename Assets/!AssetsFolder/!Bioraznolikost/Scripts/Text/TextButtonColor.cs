using DG.Tweening;
using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextButtonColor : MonoBehaviour {

  [SerializeField] private Color _color;

  [SerializeField] private List<GameObject> _listOfObjectsToColor = new List<GameObject>();

  private UIButton _uIbutton;


  private void Awake()
  {
    _uIbutton = GetComponent<UIButton>();

    _uIbutton.OnClick.OnTrigger.Event.AddListener(() => ColorObjectsInList());
  }

  public void ColorObjectsInList()
  {
    foreach (var gameObject in _listOfObjectsToColor)
    {

      if (gameObject.GetComponent<TMP_Text>() != null)
      {
        gameObject.GetComponent<TMP_Text>().color = _color;

      }

      if (gameObject.GetComponent<RawImage>() != null)
      {
        gameObject.GetComponent<RawImage>().color = _color;
      }

      if (gameObject.GetComponent<Image>() != null)
      {
        gameObject.GetComponent<Image>().color = _color;

      }

    }
  }

  public void RecolorObjectsInList()
  {
    foreach (var gameObject in _listOfObjectsToColor)
    {

      if (gameObject.GetComponent<TMP_Text>() != null)
      {
        gameObject.GetComponent<TMP_Text>().color = Color.white;
      }

      if (gameObject.GetComponent<RawImage>() != null)
      {
        gameObject.GetComponent<RawImage>().color = Color.white;

      }

      if (gameObject.GetComponent<Image>() != null)
      {
        gameObject.GetComponent<Image>().color = Color.white;

      }

    }
  }
}