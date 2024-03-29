using System;
using System.Collections;
using UnityEngine;

public class GameActionAttackMove : GameAction {

	public Unit p;
	public Vector3 target;

	public GameActionAttackMove (Unit p, float dx, float dz, string AttackMask, string BlockerMask){
		this.p = p;
		this.action = delegate(){
			if(p.IsInputLocked == false){
				this.target = new Vector3(
					p.transform.position.x+dx,
					p.transform.position.y,
					p.transform.position.z+dz
				);
				int wallLayerMask = LayerMask.GetMask("Walls");
				RaycastHit hit = new RaycastHit();
				Physics.Linecast(p.transform.position,this.target,out hit,wallLayerMask);
				if(hit.transform != null){
					Debug.DrawLine(p.transform.position, hit.transform.position,Color.red,1f);

				} else {
					int enemyLayerMask = LayerMask.GetMask(AttackMask);
					Physics.Linecast(p.transform.position,this.target,out hit,enemyLayerMask);
					if(hit.transform != null){
						GameAction a = null;
						Unit e = hit.transform.gameObject.GetComponent<Unit>() as Unit;
						if(e == null){ // not unit, must be prop (and thus a door)
							GameProp prop = hit.transform.gameObject.GetComponent<GameProp>() as GameProp;
							a = prop.ActionsManager.GetGameAction("PhysicalHit") as GameAction;
							GameActionMeleeOpenDoor aOpen = prop.ActionsManager.GetGameAction("PhysicalHit") as GameActionMeleeOpenDoor;
							if(aOpen.isOpen){
								p.IsInputLocked = true;
								p.StartCoroutine(Move());
							} else {
								p.IsInputLocked = true;
								p.StartCoroutine(Attack());
							}
						} else {
							e.ActionsManager.AddGameAction("PhysicalHit", new GameActionMeleeHit(e,p));
							a = e.ActionsManager.GetGameAction("PhysicalHit") as GameAction;
							p.IsInputLocked = true;
							p.StartCoroutine(Attack());
						}
						if(a != null){
							a.action();
							Debug.DrawLine(p.transform.position, this.target,Color.red,1f);

						}
					} else {
						int friendlyLayerMask = LayerMask.GetMask(BlockerMask);
						Physics.Linecast(p.transform.position,this.target,out hit,friendlyLayerMask);
						if(hit.transform == null){  // no friendly has been hit
							Debug.DrawLine(p.transform.position, this.target,Color.green,1f);
							p.IsInputLocked = true;
							p.StartCoroutine(Move());
						} else {
							//a friendly is blocking the way
						}
					}
				}
			}
		};
	}

	private IEnumerator Attack(){
		Vector3 origin = new Vector3(p.transform.position.x,p.transform.position.y,p.transform.position.z);
		float distance = (origin - target).sqrMagnitude;
		float distanceLimit = distance/2;
		float y = 0;
		while(distance > distanceLimit){
			distance =
				(p.transform.position - target).sqrMagnitude;
			y += Time.deltaTime;
			p.transform.position =
				Vector3.Lerp(origin,target,y*3);
			yield return null;
		}
		this.target = origin;
		origin = new Vector3(p.transform.position.x,p.transform.position.y,p.transform.position.z);
		distance = (p.transform.position - target).sqrMagnitude;
		y = 0;
		while(distance > Vector3.kEpsilon){
			distance =
				(p.transform.position - target).sqrMagnitude;
			y += Time.deltaTime;
			p.transform.position =
				Vector3.Lerp(origin,target,y*6);
			yield return null;
		}
		p.transform.position = target;
		p.IsInputLocked = false;
	}

	private IEnumerator Move(){
		Vector3 origin = p.transform.position;
		float distance = (origin - target).sqrMagnitude;
		float y = 0;
		while(distance > Vector3.kEpsilon){
			distance =
				(p.transform.position - target).sqrMagnitude;
			y += Time.deltaTime;
			p.transform.position =
				Vector3.Lerp(origin,target,y*4);
			yield return null;
		}
		p.transform.position = target;
		p.IsInputLocked = false;
	}
}

