using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageInventoryEquippedPlaceholder : ImageInventoryPlaceholder,IPointerClickHandler{
	
	public String SlotName;
	
	//callbacks
	
	public void OnPointerClick(PointerEventData e){
		ImageInventoryManager im = this.transform.parent.GetComponent<ImageInventoryManager>();
		Item HeldItem = im.GetHeldItem();
		if(HeldItem != null){
			Item OccupyingItem = im.GetInventory().GetItemInNamedSlot(SlotName);
			if(im.GetInventory().CanEquipToNamedSlot(HeldItem,SlotName)){
				if(OccupyingItem != null){ //swap
					if(im.WasLastItemFromNamedSlot()){
						String HeldItemLastName = im.GetHeldItemLastOccupiedNamedSlot();
						im.GetInventory().EquipItemToNamedSlot(im.GetInventory().GetItemInNamedSlot(SlotName),HeldItemLastName);
					} else {
						int HeldItemLastIndex = im.GetHeldItemLastOccupiedIndex();
						im.GetInventory().SetItemAtIndex(im.GetInventory().GetItemInNamedSlot(SlotName),HeldItemLastIndex);
					}
				}
				im.GetInventory().EquipItemToNamedSlot(HeldItem,SlotName); //place into new slot
				im.ReleaseHeldItem();
			}
		} else {
			im.SetHeldItem(this.SlotName);
		}
	}
	
}