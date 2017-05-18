using System;
using System.Collections;
using UnityEngine;

public class GameActionCastSkillByNameToPointTarget: GameAction {

	public GameActionCastSkillByNameToPointTarget(String SkillName, Unit Caster){
		this.action = delegate(){
			//TODO Replace with A SkillFactory, that will retrieve an appropriate intance of a skill based
			//including non-PointTarget skills
			//Just replace it with anything that works really
			//Currently, you cannot cast passive skills, using this action :(
			PointTargetSkill skill = Activator.CreateInstance(Type.GetType(SkillName+"Skill"),Caster, Caster.CastTarget) as PointTargetSkill;
			if(Caster.Mp < skill.MPCost){
				return; //not enough mana
			} else {
				Caster.Mp = Caster.Mp - skill.MPCost;
			}
			skill.action();
		};
	}
}

