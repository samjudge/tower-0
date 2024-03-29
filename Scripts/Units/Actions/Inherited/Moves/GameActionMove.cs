using System;
using System.Collections;
using UnityEngine;

public class GameActionMove : GameAction {

	public Unit p;
	public Vector3 target;

	public GameActionMove (Unit p, float dx, float dz){
		this.p = p;
		this.action = delegate(){
			if(p.IsInputLocked == false){
				this.target = new Vector3(
					p.transform.position.x+dx,
					p.transform.position.y,
					p.transform.position.z+dz
				);
				int layerMask = LayerMask.GetMask("Walls");
				RaycastHit hit = new RaycastHit();
				Physics.Linecast(p.transform.position,this.target,out hit,layerMask);
				if(hit.transform != null){
					Debug.DrawLine(p.transform.position, hit.transform.position,Color.red,0f);
				} else {
					Debug.DrawLine(p.transform.position, this.target,Color.green,0f);
					p.IsInputLocked = true;
					p.StartCoroutine(Move());
				}

			}
		};
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
				Vector3.Lerp(origin,target,y*2);
			yield return null;
		}
		p.transform.position = target;
		p.IsInputLocked = false;
	}
}

