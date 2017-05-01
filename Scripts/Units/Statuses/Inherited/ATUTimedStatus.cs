using System;
using System.Collections;
using UnityEngine;

public class ATUTimedStatus : Status
{
	public float TickTime {get; set;} 
	public TurnManager TurnManager{get; set;}
	public float LastTickTotalATUs {get; set;}
	public float EndTotalATUs {get; set;}
	public float RemainingDuration {get; set;}

	public ATUTimedStatus (String Name, float Duration, TurnManager TurnManager, Unit Owner) : base(Name,Owner)
	{
		this.RemainingDuration = Duration;
		this.LastTickTotalATUs = TurnManager.TotalATUsConsumed;
		this.EndTotalATUs = TurnManager.TotalATUsConsumed + Duration;
		this.TurnManager = TurnManager;
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
			timer = TurnManager.TotalATUsConsumed - this.LastTickTotalATUs;
//			Debug.Log("pre :" + timer);
			if(timer >= TickTime){
				RemainingDuration -= TickTime;
				timer -= TickTime;
				if(TickEffect != null){
					TickEffect();
				}
//				Debug.Log("timer @ " + timer);
				this.LastTickTotalATUs = this.LastTickTotalATUs + TickTime;
//				Debug.Log("last tick recalc :" + this.LastTickTotalATUs);
			}
//			Debug.Log("post :" + timer);
			yield return null;
		}
		if(EndEffect != null){
			EndEffect();
		}
		yield return null;
	}
}