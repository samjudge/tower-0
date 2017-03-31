using System;
using UnityEngine;

public class PointTargetSkill : Skill{

	public PointTargetSkill(Unit Caster, Vector3 PointTarget) : base(Caster){
		this.PointTarget = PointTarget;
		this.Caster = Caster;
	}

	protected Vector3 GetPointTarget(ref Vector3 PointTarget){
		LayerMask mask = LayerMask.GetMask("Walls","Floors");
		RaycastHit Hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(PointTarget);
		Physics.Raycast(ray, out Hit);
		if(Hit.transform != null){
			PointTarget = Hit.point;
		} else {
			PointTarget = Caster.transform.position;
		}
		return PointTarget;
	}

	public Vector3 PointTarget;
	public Unit Caster;
}

