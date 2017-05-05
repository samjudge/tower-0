using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemFactory : MonoBehaviour{
	
	public GameObject ChickenItem;
	
	public GameObject CreateItem(String name, Vector3 position){
		GameObject Item = null;
		switch(name){
		case "Chicken":
			Item = Instantiate(ChickenItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			Item i = new Item("Chicken");
			(Item.GetComponent<ItemGameObject>() as ItemGameObject).SetItem(i);
			return Item;
			break;
		default:
			return Instantiate(ChickenItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			break;
		}
	}
}

