using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Drop {
	public String name;
	public float chance;
}


public abstract class Enemy : Unit {

	public void Start(){
		StartCoroutine(HPWatcher());
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
		if(this.DeathAction == null){
			this.DeathAction = delegate(){
				this.GameManager.level.RemoveEnemy(this);
			};
		}
		this.HasStarted = true;
	}

	public bool HasStarted = false;

	public delegate void OnDeath();
	public abstract void Die();
	public OnDeath DeathAction;
	public String DeathActionName;
	public Drop[] Drops;

	public float ATUsRemaining = 0f;

	protected IEnumerator HPWatcher(){
		while(this.Hp > 0){
			yield return null;
		}
		foreach(GameObject eObj in GameManager.level.GetEnemies()){
			//wait for all enemy inputs to become unlocked before proceding.. If they're locked it's because they
			//are in the middle of an animation and have not completed it yet
			Enemy e = eObj.GetComponent<Enemy>();
			while(e.IsInputLocked){
				yield return null; //exit early
			}
		}
		while(this.IsInputLocked){
			yield return null;
		}
		Die();
		yield return null;
	}

	public bool CheckIsInLOSOf(Unit u){
		if((u.transform.position - this.transform.position).sqrMagnitude < 10f){
			int mask = LayerMask.GetMask("Walls");
			//shitty names, im trying to go from point to point on the plane
			Vector3[] originPoints =
				{
				new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z),
				new Vector3(this.transform.position.x+0.25f,this.transform.position.y,this.transform.position.z),
				new Vector3(this.transform.position.x-0.25f,this.transform.position.y,this.transform.position.z),
			}; // from the sides of the center also (for corners)
			Vector3 boundPoint11 = u.GetComponent<MeshFilter>().mesh.bounds.min;
			boundPoint11 = new Vector3(boundPoint11.x,0.5f,boundPoint11.z);
			Vector3 boundPoint21 = u.GetComponent<MeshFilter>().mesh.bounds.max;
			boundPoint21 = new Vector3(boundPoint21.x,0.5f,boundPoint21.z);
			Vector3 boundPoint31 = new Vector3(boundPoint11.x, 0.5f, boundPoint21.z);
			Vector3 boundPoint41 = new Vector3(boundPoint11.x, 0.5f, boundPoint11.z);
			Vector3 boundPoint51 = new Vector3(boundPoint21.x, 0.5f, boundPoint11.z);
			Vector3 boundPoint61 = new Vector3(boundPoint11.x, 0.5f, boundPoint21.z);
			Vector3 boundPoint71 = new Vector3(boundPoint21.x, 0.5f, boundPoint21.z);
			Vector3 boundPoint81 = new Vector3(boundPoint21.x, 0.5f, boundPoint11.z);
			Vector3 boundPoint91 = Camera.main.WorldToViewportPoint(new Vector3(u.transform.position.x, u.transform.position.y, u.transform.position.z));
			Vector3[] targetPoints =
				{boundPoint11,boundPoint21,boundPoint31,boundPoint41,boundPoint51,boundPoint61,boundPoint71,boundPoint81,boundPoint91};
			foreach(Vector3 origin in originPoints){
				foreach(Vector3 target in targetPoints){
					RaycastHit hit = new RaycastHit();
					bool isHit = Physics.Linecast(
						origin,
						u.transform.TransformPoint(target),
						out hit,
						mask
					);
//					Debug.DrawLine(origin, u.transform.TransformPoint(target),Color.green, 5f);
					if(!isHit){
//						Debug.DrawLine(origin, u.transform.TransformPoint(target),Color.red, 5f);
						return true;
					}
				}
			}
		}
		return false;
	}
}

