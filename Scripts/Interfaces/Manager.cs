/* General Manager Pattern
 * 
 * 
 * 
 * 
 * 
 * 
 * 

using System;
using UnityEngine;
using System.Collections;

public class XManager
{
	public ArrayList X{get;set;}
	public bool IsUpdating = true;
	
	public XManager(){
		this.Skills = new ArrayList();
	}
	
	public void AddX(Status action){
		action.Owner.StartCoroutine(ManageSkillsInList(action));
		this.Skills.Add(action);
		action.Begin();
	}
	public IEnumerator ManageXInList(Status s){
		while(IsUpdating){
			yield return null;
		}
	}
	
	public ArrayList GetXs(){
		return this.Skills;
	}
}

*
*
*
*/