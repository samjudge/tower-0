using System;
using UnityEngine;

public class PointTargetSkill : Skill {

	public PointTargetSkill(Unit Caster, Vector3 PointTarget) : base(Caster){
		this.PointTarget = PointTarget;
		this.Caster = Caster;
	}

	public Vector3 PointTarget;
}

