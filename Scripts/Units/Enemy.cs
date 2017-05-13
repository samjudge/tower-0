using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : Unit {

	public void Start(){
		StartCoroutine(HPWatcher());
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
		this.ActionsManager.AddGameAction("PhysicalHit",new GameActionMeleeHit(this,this));
	}

	public abstract void OnDeath();
	public float ATUsRemaining = 0f;

	protected IEnumerator HPWatcher(){
		while(this.Hp > 0){
			yield return null;
		}
		OnDeath();
		yield return null;
	}

	public bool CheckIsInLOSOf(Unit u){
		if((u.transform.position - this.transform.position).sqrMagnitude < 10f){
			int mask = LayerMask.GetMask("Walls");
			Vector3 boundPoint1 = this.GetComponent<MeshFilter>().mesh.bounds.min;
			boundPoint1 = new Vector3(boundPoint1.x,0.5f,boundPoint1.z);
			Vector3 boundPoint2 = this.GetComponent<MeshFilter>().mesh.bounds.max;
			boundPoint2 = new Vector3(boundPoint2.x,0.5f,boundPoint2.z);
			Vector3 boundPoint3 = new Vector3(boundPoint1.x, 0.5f, boundPoint2.z);
			Vector3 boundPoint4 = new Vector3(boundPoint1.x, 0.5f, boundPoint1.z);
			Vector3 boundPoint5 = new Vector3(boundPoint2.x, 0.5f, boundPoint1.z);
			Vector3 boundPoint6 = new Vector3(boundPoint1.x, 0.5f, boundPoint2.z);
			Vector3 boundPoint7 = new Vector3(boundPoint2.x, 0.5f, boundPoint2.z);
			Vector3 boundPoint8 = new Vector3(boundPoint2.x, 0.5f, boundPoint1.z);
			Vector3[] originPoints =
				{boundPoint1,boundPoint2,boundPoint3,boundPoint4,boundPoint5,boundPoint6,boundPoint7,boundPoint8};
			boundPoint1 = u.GetComponent<MeshFilter>().mesh.bounds.min;
			boundPoint1 = new Vector3(boundPoint1.x,0.5f,boundPoint1.z);
			boundPoint2 = u.GetComponent<MeshFilter>().mesh.bounds.max;
			boundPoint2 = new Vector3(boundPoint2.x,0.5f,boundPoint2.z);
			boundPoint3 = new Vector3(boundPoint1.x, 0.5f, boundPoint2.z);
			boundPoint4 = new Vector3(boundPoint1.x, 0.5f, boundPoint1.z);
			boundPoint5 = new Vector3(boundPoint2.x, 0.5f, boundPoint1.z);
			boundPoint6 = new Vector3(boundPoint1.x, 0.5f, boundPoint2.z);
			boundPoint7 = new Vector3(boundPoint2.x, 0.5f, boundPoint2.z);
			boundPoint8 = new Vector3(boundPoint2.x, 0.5f, boundPoint1.z);
			Vector3[] targetPoints =
				{boundPoint1,boundPoint2,boundPoint3,boundPoint4,boundPoint5,boundPoint6,boundPoint7,boundPoint8};
			foreach(Vector3 origin in originPoints){
				foreach(Vector3 target in targetPoints){
					RaycastHit hit = new RaycastHit();
					bool isHit = Physics.Linecast(
						this.transform.TransformPoint(origin),
						u.transform.TransformPoint(target),
						out hit,
						mask
					);
					if(!isHit){
						return true;
					}
				}
			}
		}
		return false;
	}
}

