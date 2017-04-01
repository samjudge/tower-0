using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager {

	Dictionary<String, GameAction> actions;

	public ActionsManager(){
		this.actions = new Dictionary<String, GameAction>();
	}

	public void AddGameAction(String name,GameAction action){
		if(this.actions.ContainsKey(name)){
			this.actions.Remove(name);
		}
		this.actions.Add(name, action);
	}

	public void RemoveGameAction(String name){
		this.actions.Remove(name);
	}

	public GameAction GetGameAction(String name){
		if(actions.ContainsKey(name)){
			return this.actions[name];
		} else {
			return null;
		}
	}
}


