using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ItemFactory : MonoBehaviour{
	
	public GameObject ChickenItem;
	public GameObject DaggerItem;
	public GameObject SwordItem;
	public GameObject ManaPotionItem;
	public GameObject HealthPotionItem;

	public GameObject CreateItem(String name, Vector3 position){
		FieldInfo Property = this.GetType().GetField(name + "Item");
		GameObject ItemPrefab = Property.GetValue(this) as GameObject;
		GameObject nItem = Instantiate(ItemPrefab, position, Quaternion.Euler(15,180,0)) as GameObject;
		ItemEffect[] ItemEffects = nItem.GetComponents<ItemEffect>() as ItemEffect[];
		Debug.Log(ItemEffects.Length);
		String TypeName = ItemEffects[0].ItemTypeName;
		Item i;
		int Tracker = 0;
		switch(TypeName){
			case "Consumable":
				ConsumableEffect[] ConsumableEffects = new ConsumableEffect[ItemEffects.Length];
				foreach(ItemEffect ConsumableEffect in ItemEffects){
					ConsumableEffects[Tracker] = ConsumableEffect as ConsumableEffect;
	              	Tracker++;
				}
				i = new ConsumableItem(name,ConsumableEffects);
				break;
			case "Equipable":
				EquipmentEffect[] EquipmentEffects = new EquipmentEffect[ItemEffects.Length];
				foreach(ItemEffect EquipmentEffect in ItemEffects){
					EquipmentEffects[Tracker] = EquipmentEffect as  EquipmentEffect;
					Tracker++;
				}
				Debug.Log(EquipmentEffects[0].EquipableTo[0]);
				i = new EquipableItem(name,EquipmentEffects[0].EquipableTo,EquipmentEffects);
				break;
			default:
				throw new Exception("Item Type does not exist : " + TypeName);
		}
		(nItem.GetComponent<ItemGameObject>() as ItemGameObject).SetItem(i);
		return nItem;
	}
}