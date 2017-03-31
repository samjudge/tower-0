using System;
using System.Collections;
using UnityEngine;

public abstract class Status
{
	public String Name {get; set;}
	public Unit Owner {get; set;}

	public delegate void Effect();

	public Effect StartEffect;
	public Effect EndEffect;
	public Effect TickEffect;

	public Status (String Name, Unit Owner)
	{
		this.Name = Name;
		this.Owner = Owner;
	}

	public void Begin(){
		this.Owner.StartCoroutine(StatusLoop());
	}

	protected abstract IEnumerator StatusLoop();

}

