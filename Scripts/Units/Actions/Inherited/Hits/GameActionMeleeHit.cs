using System;
using System.Collections;
using UnityEngine;

public class GameActionMeleeHit : GameAction {

	//a test update

	public Unit attacker;
	public Unit target;

	public GameActionMeleeHit (Unit target){
		this.target = target;
		this.action = delegate(){
			TakeDamage(3);
		};
	}
	
	private void TakeDamage(float d){
		this.target.Hp -= d;
	}
}

