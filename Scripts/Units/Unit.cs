using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour{

	public GameManager GameManager {get;set;}
	public ActionsManager ActionsManager {get;set;}
	public StatusManager StatusManager {get;set;}
	public SkillManager SkillManager {get;set;}
	public bool IsInputLocked;
	public Vector3 CastTarget;
	abstract public float Hp {get;set;}
	abstract public float MaxHp {get;set;}
	public float AttackDamage {get;set;}
	public Inventory Inventory {get;set;}

	public float BaseStrength {get; set;}
	public float BonusStrength {get; set;}

	public float BaseLuck {get; set;}
	public float BonusLuck {get; set;}

	public float BaseIntelligence {get; set;}
	public float BonusIntelligence {get; set;}

	public float BaseDexterity {get; set;}
	public float BonusDexterity {get; set;}

	public abstract float ProcessTurn();

}

