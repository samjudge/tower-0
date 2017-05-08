using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageInventorySlotPlaceholder : ImageInventoryPlaceholder,IPointerClickHandler{
	
	public int SlotIndex;
	
	//callbacks
	
	public void OnPointerClick(PointerEventData e){
		ImageInventoryManager im = this.transform.parent.GetComponent<ImageInventoryManager>();
		Item HeldItem = im.GetHeldItem();
		if(HeldItem != null){
			Item OccupyingItem = im.GetInventory().GetItemAtIndex(SlotIndex);
			if(OccupyingItem != null){ //swap
				if(im.WasLastItemFromNamedSlot()){
					String HeldItemLastName = im.GetHeldItemLastOccupiedNamedSlot();
					im.GetInventory().EquipItemToNamedSlot(im.GetInventory().GetItemAtIndex(SlotIndex),HeldItemLastName);
				} else {
					int HeldItemLastIndex = im.GetHeldItemLastOccupiedIndex();
					im.GetInventory().SetItemAtIndex(im.GetInventory().GetItemAtIndex(SlotIndex),HeldItemLastIndex);
				}
			}
			im.GetInventory().SetItemAtIndex(HeldItem,SlotIndex); //place into new slot
			im.ReleaseHeldItem();
		} else {
			im.SetHeldItem(this.SlotIndex);
		}
	}
	
}