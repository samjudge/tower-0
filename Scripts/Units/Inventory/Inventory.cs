
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory { //An "ItemManager" type class

	public Dictionary<String,Item> Equipped;
	private ArrayList Slots;
	public Unit Owner;

	private int MaxInventorySlots = 16;

	public int GetEquipedSlotsCount(){
		return Equipped.Count;
	}

	public int GetMaxSlots(){
		return MaxInventorySlots;
	}

	public Inventory(Unit Owner) {
		this.Owner = Owner;
		this.Equipped = new Dictionary<String, Item>();
		this.Slots = new ArrayList();
		for(int x = 0 ; x < MaxInventorySlots ; x++){
			this.Slots.Add(null); //initalize open slots
		}
	}

	public bool CanAddItem(){
		if(Slots.Count <= MaxInventorySlots){
			return true;
		} else {
			return false;
		}
	}

	public bool CanEquipToNamedSlot(Item i, String NamedSlot){
		if(i is EquipableItem){
			EquipableItem ei = (EquipableItem) i;
			foreach(String CanEquipToSlot in ei.EquipableTo){
				if(CanEquipToSlot == NamedSlot){
					return true;
				}
			}
		}
		return false;
	}

	public void EmptyItemFromNamedSlot(String EquipmentSlot){
		if(this.Equipped.ContainsKey(EquipmentSlot)){
			this.Equipped.Remove(EquipmentSlot);
		}
		this.Equipped.Add(EquipmentSlot,null);
	}

	public void EquipItemToNamedSlot(Item i, String EquipmentSlot){
		if(CanEquipToNamedSlot(i,EquipmentSlot)){
			if(this.Equipped.ContainsKey(EquipmentSlot)){
				this.Equipped.Remove(EquipmentSlot);
			}
			this.Equipped.Add(EquipmentSlot,i);
		}
	}

	public void RemoveEquippedItemFromNamedSlot(String EquipmentSlot){
		if(this.Equipped.ContainsKey(EquipmentSlot)){
			this.Equipped.Remove(EquipmentSlot);
		}
	}

	public Item GetItemInNamedSlot(String EquipmentSlot){
		if(this.Equipped.ContainsKey(EquipmentSlot)){
			return this.Equipped[EquipmentSlot];
		} else {
			return null;
		}
	}

	public void EmptyItemAtIndex(int index){
		SetItemAtIndex(null, index);
	}

	public void SetItemAtIndex(Item i, int index){
		this.Slots[index] = i;
	}

	public Item GetItemAtIndex(int ItemIndex){
		if(ItemIndex > this.MaxInventorySlots){
			throw new Exception("Slot Out Of Range Of Max");
		}
		Item i = this.Slots[ItemIndex] as Item;
		return i;
	}

	public void AddItem(Item i){
		for(int x = 0; x < this.MaxInventorySlots; x++){
			if(this.Slots[x] == null){
				this.Slots[x] = i;
				break;
			}
		}
	}

	public void EquipItemTo(Item i, String slot){
		this.Equipped.Add(slot,i);
	}
}