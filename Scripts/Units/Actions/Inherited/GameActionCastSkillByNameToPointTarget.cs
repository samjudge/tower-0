using System;
using System.Collections;
using UnityEngine;

public class GameActionCastSkillByNameToPointTarget: GameAction {

	public GameActionCastSkillByNameToPointTarget(String SkillName, Unit Caster){
		this.action = delegate(){
			//TODO Replace with A SkillManager, that will retrieve an appropriate intance of a skill based
			//including non-PointTarget skills
			PointTargetSkill skill = Activator.CreateInstance(Type.GetType(SkillName+"Skill"),Caster, Caster.CastTarget) as PointTargetSkill;
			skill.action();
		};
	}
}

