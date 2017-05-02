
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory { //An "ItemManager" type class

	public Dictionary<String,Item> Equipped;
	private ArrayList Slots;
	public Unit Owner;

	private int MaxInventorySlots = 16;

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

	public bool CanAddItemToInventory(){
		if(Slots.Count <= MaxInventorySlots){
			return true;
		} else {
			return false;
		}
	}

	public void SetItemInInvenotry(Item i, int index){
		this.Slots[index] = i;
	}

	public Item GetItemFromInventory(int ItemIndex){
		if(ItemIndex > this.MaxInventorySlots){
			throw new Exception("Slot Out Of Range Of Max");
		}
		Item i = this.Slots[ItemIndex] as Item;
		return i;
	}

	public void AddItemToInventory(Item i){
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