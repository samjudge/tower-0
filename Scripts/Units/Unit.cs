using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour{

	public GameManager GameManager {get;set;}
	public ActionsManager ActionsManager {get;set;}
	public StatusManager StatusManager {get;set;}
	public SkillManager SkillManager {get;set;}

	//while true, unit cannot recieve additional inputs (UP vs UP,UP,UP... during a single update).
	public bool IsInputLocked = false; 

	//The Target for actions (i.e. -Casting- a Skill, but could be used by any Skill/Spell delegate that needs a target)
	//TODO: interface to a <<Caster>> class
	public Vector3 CastTarget; 

	public Inventory Inventory {get;set;}

	public int level = 1;
	public float Experience = 0;

	public float Hp;
	public float MaxHp;

	public float Mp;
	public float MaxMp;

	public float BaseStrength;
	public float BonusStrength;

	public float BaseLuck;
	public float BonusLuck;

	public float BaseIntelligence;
	public float BonusIntelligence;

	public float BaseDexterity;
	public float BonusDexterity;

	public abstract float ProcessTurn();

	public bool CanLevelUp(){
		float requiredExperience = CalculateRequiredExpForLevelUp();
		if(this.Experience > requiredExperience){
			return true;
		}
		return false;
	}

	public float CalculateRequiredExpForLevelUp(){
		return (this.level * 5)^2;
	}

}

