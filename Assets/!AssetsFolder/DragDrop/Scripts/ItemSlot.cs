using DG.Tweening;
using Novena.DAL;
using Novena.DAL.Model.Guide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scripts.Utility;

public class ItemSlot : MonoBehaviour, IDropHandler {

	public static Action<bool, Theme> OnPhotoDropped;

	[HideInInspector] public bool IsDropped;

	public CanvasGroup CanvasGroup;

	[SerializeField] private RawImage _slotOutline;

	private void Awake()
	{
		CanvasGroup = GetComponent<CanvasGroup>();
		CanvasGroup.alpha = 0;
	}



	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
		{
			return;
		}


		if (eventData.pointerDrag.GetComponent<DragDrop>().DraggingItem)
		{
			if (eventData.pointerDrag.GetComponent<DragDrop>().ItemSlot == this)
			{
				IsDropped = true;

				eventData.pointerDrag.transform.SetParent(transform);
				eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

				AssetsFileLoader.LoadTexture2D(eventData.pointerDrag.GetComponent<ItemInfo>().Theme.GetMediaByName("Item").GetPhotos()[0].FullPath,eventData.pointerDrag.GetComponent<RawImage>());
				eventData.pointerDrag.GetComponent<RawImage>().SetNativeSize();

				GetComponent<UnityEngine.UI.Image>().raycastTarget = false;

				OnPhotoDropped?.Invoke(true, eventData.pointerDrag.GetComponent<ItemInfo>().MapTheme);
				_slotOutline.color = Color.clear;
			}
			else
			{
				OnPhotoDropped?.Invoke(false, null);
			}
		}
	}


}