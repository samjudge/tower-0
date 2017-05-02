using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageInventoryPlaceholder : MonoBehaviour, IPointerClickHandler{
	
	public int SlotIndex;

	//callbacks

	public void OnPointerClick(PointerEventData e){
		ImageInventoryManager im = this.transform.parent.GetComponent<ImageInventoryManager>();
		Item HeldItem = im.GetHeldItem();
		if(HeldItem != null){
			Item OccupyingItem = im.GetInventory().GetItemFromInventory(SlotIndex);
			if(OccupyingItem != null){ //swap
				int HeldItemLastIndex = im.GetHeldItemLastOccupiedIndex();
				im.GetInventory().SetItemInInvenotry(im.GetInventory().GetItemFromInventory(SlotIndex),HeldItemLastIndex);
			}
			im.GetInventory().SetItemInInvenotry(HeldItem,SlotIndex); //place into new slot
			im.ReleaseHeldItem();
		} else {
			im.SetHeldItem(this.SlotIndex);
		}
		Debug.Log("Item clicked, btw");
	}

}