using System;
using System.Collections;
using UnityEngine;

public class GameActionMeleeHit : GameAction {
	
	public Unit attacker;
	public Unit target;

	public GameActionMeleeHit (Unit attacker, Unit target){
		this.attacker = attacker;
		this.target = target;
		this.action = delegate(){
			TakeDamage(3);
		};
	}
	
	private void TakeDamage(float d){
		this.target.Hp -= d;
	}
}

