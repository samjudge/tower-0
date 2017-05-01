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
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		ATUsRemaining = 0;
	}

	public override float ProcessTurn(){
		if(this.CheckIsInLOSOf(GameManager.GetPlayer().GetComponent<Unit>() as Unit)){
//			ArrayList statuses = this.StatusManager.GetStatuses();
//			if(statuses.Count == 0){
//				this.ActionsManager.GetGameAction("Cast").action();
//			}
		};
		return 1f;
	}

	override public void OnDeath(){
		this.GameManager.RemoveEnemy(this);
	}
}

