using System;
using UnityEngine;

public class ItemGameObject : MonoBehaviour {

	private Item AsItem;

	public void SetItem (Item i) {
		this.AsItem = i;
	}

	public Item GetItem(){
		return AsItem;
	}


}
