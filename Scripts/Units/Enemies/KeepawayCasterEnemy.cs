using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeepawayCasterEnemy : Enemy {
	
	private AStarPathfind AI;
	private CasterLogic CastLogic;

	public void Start(){
		base.Start();
		this.CastLogic = new CastWhenPlayerVisibleLogic(this);
		this.CastTarget = new Vector3(0f,0f,0f);
		this.AI = new AStarPathfind(this.GameManager.Player.transform.position,new Vector3(1,0,1));
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		ATUsRemaining = 0;
	}
	
	public override float ProcessTurn(){
		if(!this.HasStarted){
			return 1f;
		}
		if(this.CheckIsInLOSOf(GameManager.Player.GetComponent<Unit>() as Unit)){
			if(this.IsInputLocked == false){
				//decide to either run or cast a spell at player position
				bool shouldRun = false;
				float distance = (this.GameManager.Player.transform.position - this.transform.position).sqrMagnitude;
				if(distance < 2){
					shouldRun = true;
				}
				if(!shouldRun){

					String ToCastName = CastLogic.GetSpellName();
					if(ToCastName != null){
						Debug.Log("casting...");
						Debug.Log(ToCastName);
						this.CastTarget = this.GameManager.Player.transform.position;
						this.ActionsManager.AddGameAction(
							"Cast",
							new GameActionCastSkillByNameToPointTarget(ToCastName,this)
						);
						this.ActionsManager.GetGameAction("Cast").action();
					}
				} else {
					this.AI = new AStarPathfind(this.GameManager.Player.transform.position,new Vector3(1,0,1));
					AStarPathfind.Node n = new AStarPathfind.Node();
					n.parent = null;
					n.position = this.transform.position;
					AStarPathfind.Node top = this.AI.FindPath(n);
					int count = 0;
					while(top.parent != null){
						if(top.parent.parent == null){
							break;
						}
						top = top.parent;
						count++;
					}
					//make movement target to be AWAY from player
					float targetX = 0f;
					float targetY = 0f;
					if((top.position.x - this.transform.position.x) < 0){
						targetX = 1;
					} else if ((top.position.x - this.transform.position.x) > 0){
						targetX = -1;
					} else {
						targetX = 0;
					}
					if((top.position.z - this.transform.position.z) < 0){
						targetY = 1;
					} else if((top.position.z - this.transform.position.z) > 0){
						targetY = -1;
					} else {
						targetY = 0;
					}
					ActionsManager.AddGameAction(
						"Move",
						new GameActionAttackMove(
						this,
						targetX,
						targetY,
						"Player",
						"Enemies"
						)
						);
					this.ActionsManager.GetGameAction("Move").action();
				}
			}
		};
		return 1f;
	}
	
	override public void Die(){
		this.DeathAction();
	}
}

