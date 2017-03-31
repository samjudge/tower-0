using System;
using System.Collections;
using UnityEngine;

public class TimedStatus : Status
{
	public float TickTime {get; set;} 
	public float RemainingDuration{get; set;}
	
	public TimedStatus (String Name, float Duration, Unit Owner) : base(Name,Owner)
	{
		this.RemainingDuration = Duration;
		this.TickTime = 1f;
	}
	
	public void Begin(){
		this.Owner.StartCoroutine(StatusLoop());
	}
	
	override protected IEnumerator StatusLoop(){
		if(StartEffect != null){
			StartEffect();
		}
		float timer = 0;
		while(RemainingDuration >= 0){
			timer += Time.deltaTime;
			if(timer > TickTime){
				RemainingDuration -= TickTime;
				timer -= TickTime;
				if(TickEffect != null){
					TickEffect();
				}
			}
			yield return null;
		}
		if(EndEffect != null){
			EndEffect();
		}
		yield return null;
	}
}