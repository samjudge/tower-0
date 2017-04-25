
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory {

	Dictionary<String,Item> Equipped;
	Dictionary<int,Item> Slots;

	private int MaxInventorySlots = 9;

	public Inventory() {
		this.Equipped = new Dictionary<String, Item>();
		this.Slots = new Dictionary<int,Item>();
	}

	public bool canAddItemToInventory(Item i){

	}
}