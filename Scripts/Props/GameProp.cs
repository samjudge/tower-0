using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameProp : MonoBehaviour {
	public GameManager GameManager {get;set;}
	public ActionsManager ActionsManager;

	public void Start(){
		this.ActionsManager = new ActionsManager();
		this.ActionsManager.AddGameAction("PhysicalHit",new GameActionMeleeOpenDoor(this.gameObject));
	}
}

