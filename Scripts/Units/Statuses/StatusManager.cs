using System;
using UnityEngine;
using System.Collections;

public class StatusManager
{
	public ArrayList Statuses{get;set;}
	public bool IsUpdating = true;

	public StatusManager(){
		this.Statuses = new ArrayList();
	}
	
	public void AddStatus(TimedStatus action){
		action.Owner.StartCoroutine(ManageStatusInList(action));
		this.Statuses.Add(action);
		action.Begin();
	}

	public void AddStatus(ATUTimedStatus action){
		action.Owner.StartCoroutine(ManageStatusInList(action));
		this.Statuses.Add(action);
		action.Begin();
	}

	public void AddStatus(Status action){
		this.Statuses.Add(action);
		action.Begin();
	}

	public IEnumerator ManageStatusInList(ATUTimedStatus s){
		while(s.RemainingDuration >= 0){
			yield return null;
			if(s.RemainingDuration <= 0){
				Statuses.Remove(s);
			}
		}
	}

	public IEnumerator ManageStatusInList(TimedStatus s){
		while(s.RemainingDuration >= 0){
			yield return null;
			if(s.RemainingDuration <= 0){
				Statuses.Remove(s);
			}
		}
	}
	
	public ArrayList GetStatuses(){
		return this.Statuses;
	}
}

