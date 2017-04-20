using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChickenEnemy : Enemy {

	public float cHp = 6;
	private AStarPathfind AI;

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
		this.AI = new AStarPathfind(this.GameManager.Player.transform.position,new Vector3(1,1,0));
		ActionsManager.AddGameAction("Cast", new GameActionCastSkillByNameToPointTarget("Heal",this as Unit));
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
	}

	public void Update(){
		if(this.CheckIsInLOSOf(GameManager.Player.GetComponent<Unit>() as Unit)){
			ArrayList statuses = this.StatusManager.GetStatuses();
			if(statuses.Count == 0){
				this.ActionsManager.GetGameAction("Cast").action();
			}
			if(this.IsCurrentlyMoving == false){
				this.AI = new AStarPathfind(this.GameManager.Player.transform.position,new Vector3(1,0,1));
				AStarPathfind.Node n = new AStarPathfind.Node();
				n.parent = null;
				n.position = this.transform.position;
				AStarPathfind.Node top = this.AI.FindPath(n);
				while(top.parent != null){
					if(top.parent.parent != null){ //so that it doen't get the position it's standing on
						top = top.parent;
					}
				}
				Debug.Log (top.position.x - this.transform.position.x);
				Debug.Log (top.position.z - this.transform.position.z);
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
					new GameActionMove(
					this,
					targetX,
					targetY
					)
					);
				this.ActionsManager.GetGameAction("Move").action();
			}
		};
	}

	override public void OnDeath(){
		this.GameManager.RemoveEnemy(this);
	}
}

