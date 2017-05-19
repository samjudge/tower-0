using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemFactory : MonoBehaviour{
	
	public GameObject ChickenItem;
	public GameObject DaggerItem;
	public GameObject SwordItem;
	
	public GameObject CreateItem(String name, Vector3 position){
		GameObject Item = null;
		Item i = null;
		switch(name){
		case "Chicken":
			Item = Instantiate(ChickenItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			ConsumableEffect[] chickenEffects = {
				new ConsumeHPModifier(25)
			};
			i = new ConsumableItem("Chicken",chickenEffects);
			(Item.GetComponent<ItemGameObject>() as ItemGameObject).SetItem(i);
			return Item;
			break;
		case "Dagger":
			Item = Instantiate(DaggerItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			EquipmentEffect[] daggerEffects = {
				new EquipmentStatusModifierEffect(1,0,0,0)
			};
			i = new EquipableItem("Dagger",new String[]{"Left","Right"},daggerEffects);
			(Item.GetComponent<ItemGameObject>() as ItemGameObject).SetItem(i);
			return Item;
			break;
		case "Sword":
			Item = Instantiate(SwordItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			EquipmentEffect[] swordEffects = {
				new EquipmentStatusModifierEffect(4,0,0,0)
			};
			i = new EquipableItem("Sword",new String[]{"Left","Right"},swordEffects);
			(Item.GetComponent<ItemGameObject>() as ItemGameObject).SetItem(i);
			return Item;
			break;
		default:
			return Instantiate(ChickenItem, position, Quaternion.Euler(15,180,0)) as GameObject;
			break;
		}
	}
}