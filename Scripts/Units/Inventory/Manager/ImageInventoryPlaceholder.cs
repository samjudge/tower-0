using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageInventoryPlaceholder : MonoBehaviour, IPointerClickHandler{
	
	public int SlotIndex;

	//callbacks

	public void OnPointerClick(PointerEventData e){
//		GameObject UIInventory = this.gameObject.transform.parent.gameObject;
//		ImageInventoryManager im = UIInventory.GetComponent<ImageInventoryManager>();
//		Inventory i = im.GetInventory();
//		GameManager gm = i.Owner.GameManager;
		Debug.Log("Item clicked, btw");
	}

}