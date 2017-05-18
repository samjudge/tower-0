using System;
using System.Collections;
using UnityEngine;

public class GameActionPickUpGroundItem : GameAction {
	
	public Unit p;
	
	public GameActionPickUpGroundItem(Unit p){
		this.p = p;
		this.action = delegate(){
			if(p.IsInputLocked == false){
				ArrayList ItemClones = new ArrayList(p.GameManager.items);
				foreach(GameObject t in ItemClones){
					if((t.GetComponent<BoxCollider>() as BoxCollider).bounds.Contains(p.transform.position)){
						Item i = (t.transform.gameObject.GetComponent<ItemGameObject>() as ItemGameObject).GetItem();
						if(i != null){
							this.p.Inventory.AddItem(i);
						}
						p.GameManager.items.Remove(t);
						MonoBehaviour.Destroy(t.transform.gameObject);
					} 
				}
			}
		};
	}
}

