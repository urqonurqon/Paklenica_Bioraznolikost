using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

	public bool Draggable { get; set; }
	[HideInInspector] public bool DraggingItem;
	[HideInInspector] public ScrollRect ScrollRect;
	[HideInInspector] public Transform DragParent;


	private RectTransform _rectTransform;
	private CanvasGroup _canvasGroup;
	private Transform _startingParent;
	//private RectTransform _itemSlotRectTransform = null;


	[HideInInspector] public ItemSlot ItemSlot;

	[HideInInspector] public bool ShowInfo;

	private GameTemplate _gameTemplate;



	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_canvasGroup = GetComponent<CanvasGroup>();
		_startingParent = transform.parent;
		Draggable = true;
		_gameTemplate = FindObjectOfType<GameTemplate>();
	}





	public void OnBeginDrag(PointerEventData eventData)
	{
		ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
		if (DraggingItem)
		{
			//if (_itemSlotRectTransform == null && ItemSlot != null)
			//	_itemSlotRectTransform = ItemSlot.GetComponent<RectTransform>();
			GetComponent<ItemInfo>().UIToggle.IsOn = false;
			transform.SetParent(DragParent);
			_canvasGroup.blocksRaycasts = false;
		}

	}

	public void OnDrag(PointerEventData eventData)
	{
		if (DraggingItem)
		{
			_rectTransform.anchoredPosition += eventData.delta;
			for (int i = 0; i < _gameTemplate.ItemSlots.Count; i++)
			{
				var itemSlot = _gameTemplate.ItemSlots[i].GetComponent<ItemSlot>();
				if (!itemSlot.IsDropped)
				{
					if (Vector3.Distance(eventData.pointerDrag.transform.localPosition, itemSlot.transform.localPosition - new Vector3(Screen.width / 2, Screen.height / 2, 0) + new Vector3(itemSlot.GetComponent<RectTransform>().sizeDelta.x / 2, itemSlot.GetComponent<RectTransform>().sizeDelta.y / 2, 0)) < 350)
					{

						itemSlot.CanvasGroup.DOFade(1, .4f);

					}
					else
					{

						itemSlot.CanvasGroup.DOFade(0, .4f);

					}
				}
			}
		}
		else
		{
			ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (DraggingItem)
		{
			for (int i = 0; i < _gameTemplate.ItemSlots.Count; i++)
			{
				if (!_gameTemplate.ItemSlots[i].GetComponent<ItemSlot>().IsDropped)
					_gameTemplate.ItemSlots[i].GetComponent<CanvasGroup>().DOFade(0, .4f);
			}
			if (ItemSlot == null)
			{
				transform.SetParent(_startingParent);

				SnapBackToContainer(eventData);


			}
			else
			{
				if (!ItemSlot.IsDropped)
				{
					transform.SetParent(_startingParent);

					SnapBackToContainer(eventData);


				}
				else
				{
					_startingParent.gameObject.SetActive(false);
					eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;

				}
			}

			DraggingItem = false;
			_rectTransform.localScale = new Vector3(1f, 1f, 1f);

		}
		else
		{

			ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
		}
	}
	private void SnapBackToContainer(PointerEventData eventData)
	{
		eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		_canvasGroup.blocksRaycasts = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopAllCoroutines();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		StopAllCoroutines();
		_rectTransform.localScale = new Vector3(1f, 1f, 1f);
		_rectTransform.SetParent(_startingParent);
		_rectTransform.anchoredPosition = Vector2.zero;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!Draggable)
		{
			return;
		}
		ShowInfo = true;
		StartCoroutine(StartTimer());

	}

	private IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(.2f);
		DraggingItem = true;
		ShowInfo = false;
		_rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

		transform.SetParent(DragParent);
		_rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);

	}

	private void Update()
	{
		if (HasMouseMoved())
		{
			StopAllCoroutines();
		}


	}

	private bool HasMouseMoved()
	{
		return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
	}

}
