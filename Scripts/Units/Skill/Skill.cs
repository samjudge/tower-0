using System;
using UnityEngine;

public abstract class Skill{
	
	public Skill(Unit Caster){
		this.Caster = Caster;
	}

	public Unit Caster;
	public delegate void Action();
	public Action action;
	public String Name;
}

