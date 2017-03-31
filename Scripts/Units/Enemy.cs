using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Unit {

	public void Start(){
		StartCoroutine(HPWatcher());
		this.ActionsManager.AddGameAction("PhysicalHit",new GameActionMeleeHit(this,this));
	}

	public abstract void OnDeath();

	protected IEnumerator HPWatcher(){
		while(this.Hp > 0){
			yield return null;
		}
		OnDeath();
		yield return null;
	}
}

