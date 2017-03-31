using System;
using System.Collections;
using UnityEngine;

public class GameActionMove : GameAction {

	public Player p;
	public Vector3 target;

	public GameActionMove (Player p, float dx, float dz){
		this.p = p;
		this.action = delegate(){
			if(p.IsCurrentlyMoving == false){
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
					p.IsCurrentlyMoving = true;
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
				Vector3.Lerp(origin,target,y*3);
			yield return null;
		}
		p.transform.position = target;
		p.IsCurrentlyMoving = false;
	}
}

