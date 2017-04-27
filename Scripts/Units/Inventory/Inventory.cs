
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory { //An "ItemManager" type class

	public Dictionary<String,Item> Equipped;
	public ArrayList Slots;
	public Unit Owner;

	private int MaxInventorySlots = 9;

	public Inventory(Unit Owner) {
		this.Owner = Owner;
		this.Equipped = new Dictionary<String, Item>();
		this.Slots = new ArrayList();
	}

	public bool canAddItemToInventory(){
		if(Slots.Count <= MaxInventorySlots){
			return true;
		} else {
			return false;
		}
	}

	public void addItemToInventory(Item i){
		if(canAddItemToInventory()){
			this.Slots.Add(i);
		}
	}

	public void equipItemTo(Item i, String slot){
		this.Equipped.Add(slot,i);
	}
}