using System;
using UnityEngine;

/**
 * Wrapper for Items on the ground (In game world) 
 */ 
public class ItemGameObject : MonoBehaviour {

	private Item AsItem;

	public void SetItem (Item i) {
		this.AsItem = i;
	}

	public Item GetItem(){
		return AsItem;
	}


}
