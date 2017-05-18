using System;
using System.Collections;
using UnityEngine;

public class GameActionMeleeHit : GameAction {

	//a test update

	public Unit target;

	public GameActionMeleeHit (Unit target, Unit attacker){
		this.target = target;
		this.action = delegate(){
			TakeDamage(attacker.BaseStrength + attacker.BonusStrength + 1);
		};
	}
	
	private void TakeDamage(float d){
		this.target.Hp -= d;
	}
}

