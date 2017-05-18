using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageInventorySlotPlaceholder : ImageInventoryPlaceholder,IPointerClickHandler{
	
	public int SlotIndex;
	
	//callbacks
	
	public void OnPointerClick(PointerEventData e){
		if(e.button == PointerEventData.InputButton.Left){
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
		} else if (e.button == PointerEventData.InputButton.Right){
			ImageInventoryManager im = this.transform.parent.GetComponent<ImageInventoryManager>();
			Item ItemAtSlot = im.GetInventory().GetItemAtIndex(SlotIndex);
			if(ItemAtSlot != null){
				if(ItemAtSlot.GetType() == typeof(ConsumableItem)){
					ConsumableItem Consumable = (ConsumableItem) ItemAtSlot;
					ConsumableEffect[] effects = Consumable.OnConsumeEffects;
					foreach(ConsumableEffect effect in effects){
						ConsumableEffect.ConsumeAction cEffect = effect.GetOnConsumeEffect();
						cEffect(im.GetInventory().Owner);
					}
					Consumable.SetCharges(Consumable.GetCharges() - 1);
					if(Consumable.GetCharges() <= 0){
						im.GetInventory().EmptyItemAtIndex(SlotIndex);
					}
				}
			}
		}
	}
	
}