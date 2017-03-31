using System;
using UnityEngine;
using System.Collections;

public class SkillManager
{

	public Unit Owner;

	public ArrayList Skills{get;set;}
	
	public SkillManager(Unit Owner){
		this.Skills = new ArrayList();
		this.Owner = Owner;
	}
	
	public void AddSkill(String skillName){
		this.Skills.Add(skillName);
	}
	
	public ArrayList GetSkills(){
		return this.Skills;
	}
}

