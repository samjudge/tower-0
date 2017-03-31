using System;
using System.Collections;
using UnityEngine;

public class PersistantStatus : Status
{
	public bool IsActive;
	
	public PersistantStatus (String Name, Unit Owner) : base(Name,Owner)
	{
		this.Name = Name;
		this.Owner = Owner;
		IsActive = true;
	}
	
	public void Begin(){
		this.Owner.StartCoroutine(StatusLoop());
	}
	
	override protected IEnumerator StatusLoop(){
		if(StartEffect != null){
			StartEffect();
		}
		while(IsActive){
			if(TickEffect != null){
				TickEffect();
			}
			yield return null;
		}
		if(EndEffect != null){
			EndEffect();
		}
		yield return null;
	}
	
}

