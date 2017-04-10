using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Unit {

	public void Start(){
		StartCoroutine(HPWatcher());
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
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

	public bool CheckIsInLOSOf(Unit u){
		Vector3 origin = this.transform.position;
		Vector3 target = u.transform.position;
		RaycastHit hit = new RaycastHit();
		int mask = LayerMask.GetMask("Walls");
		bool isHit = Physics.Linecast(origin,target,out hit,mask);
		//Debug.Log(origin);
		//Debug.Log(target);
		//Debug.DrawLine(origin,target);
		if(isHit){
			return false;
		} else {
			return true;
		}
	}
}

