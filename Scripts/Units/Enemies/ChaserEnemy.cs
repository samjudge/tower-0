using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChaserEnemy : Enemy {

	private AStarPathfindNoWalls AI;

	public void Start(){
		base.Start();
		this.BaseStrength = 1;
		this.Mp = 10;
		this.MaxMp = 10;
		this.Hp = 10;
		this.MaxHp = 10;
		this.CastTarget = new Vector3(0f,0f,0f);
		this.AI = new AStarPathfindNoWalls(this.GameManager.Player.transform.position,new Vector3(1,0,1));
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		ATUsRemaining = 0;
	}

	public override float ProcessTurn(){
		if(this.CheckIsInLOSOf(GameManager.Player.GetComponent<Unit>() as Unit)){
			if(this.IsInputLocked == false){
				this.AI = new AStarPathfindNoWalls(this.GameManager.Player.transform.position,new Vector3(1,0,1));
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
				float targetX = 0f;
				float targetY = 0f;
				if((top.position.x - this.transform.position.x) < 0){
					targetX = -1;
				} else if ((top.position.x - this.transform.position.x) > 0){
					targetX = 1;
				} else {
					targetX = 0;
				}
				if((top.position.z - this.transform.position.z) < 0){
					targetY = -1;
				} else if((top.position.z - this.transform.position.z) > 0){
					targetY = 1;
				} else {
					targetY = 0;
				}
				ActionsManager.AddGameAction(
					"Move",
					new GameActionAttackMove(
					this,
					targetX,
					targetY,
					"Player"
					)
				);
				this.ActionsManager.GetGameAction("Move").action();
			}
		};
		return 1f;
	}

	override public void Die(){
		this.DeathAction();
	}
}

