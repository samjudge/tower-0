using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChickenEnemy : Enemy {

	public float cHp = 6;
	private AStarPathfindNoWalls AI;

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
		this.AI = new AStarPathfindNoWalls(this.GameManager.GetPlayer().transform.position,new Vector3(1,0,1));
		ActionsManager.AddGameAction("Cast", new GameActionCastSkillByNameToPointTarget("Heal",this as Unit));
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		ATUsRemaining = 0;
	}

	public override float ProcessTurn(){
		if(this.CheckIsInLOSOf(GameManager.GetPlayer().GetComponent<Unit>() as Unit)){
			ArrayList statuses = this.StatusManager.GetStatuses();
			if(statuses.Count == 0){
				this.ActionsManager.GetGameAction("Cast").action();
			}
			if(this.IsInputLocked == false){
				this.AI = new AStarPathfindNoWalls(this.GameManager.GetPlayer().transform.position,new Vector3(1,0,1));
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
					new GameActionMove(
					this,
					targetX,
					targetY
					)
					);
				this.ActionsManager.GetGameAction("Move").action();
			}
		};
		return 1f;
	}

	override public void OnDeath(){
		GameObject ChickenDrop = this.GameManager.ItemFactory.CreateItem("Chicken",this.transform.position);
		this.GameManager.items.Add(ChickenDrop);
		this.GameManager.RemoveEnemy(this);
	}
}

