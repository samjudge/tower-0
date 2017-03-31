using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DummyEnemy : Enemy {

	public float cHp = 10;

	public override float Hp {
		get{ return cHp;}
		set {cHp = value;}
	}

	public override float MaxHp {
		get{ return 10;}
		set {}
	}
	
	public void Start(){
		base.Start();
		this.CastTarget = new Vector3(0f,0f,0f);
		ActionsManager.AddGameAction("Cast", new GameActionCastSkillByNameToPointTarget("Heal",this as Unit));
	}
	
	float regen_every = 2;
	float regen_current_timer = 0;
	
	public void Update(){
		regen_current_timer += Time.deltaTime;
		if(regen_current_timer > regen_every){
			regen_current_timer = 0;
			this.ActionsManager.GetGameAction("Cast").action();
		}
	}

	override public void OnDeath(){
		this.GameManager.RemoveEnemy(this);
	}
}

