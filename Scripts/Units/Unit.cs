using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour{

	public GameManager GameManager {get;set;}
	public ActionsManager ActionsManager {get;set;}
	public StatusManager StatusManager {get;set;}
	public SkillManager SkillManager {get;set;}
	public bool IsCurrentlyMoving;
	public Vector3 CastTarget;
	abstract public float Hp {get;set;}
	abstract public float MaxHp {get;set;}
	public float AttackDamage {get;set;}
	
}

