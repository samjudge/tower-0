
using UnityEngine;
using System;

public class ConsumableItem : Item {

	public ConsumableEffect[] OnConsumeEffects;
	private int charges = 1;

	public ConsumableItem (String name, ConsumableEffect[] OnConsumeEffects) : base(name){
		Debug.Log(OnConsumeEffects.Length);
		this.OnConsumeEffects = OnConsumeEffects;
	}

	public int GetCharges(){
		return this.charges;
	}

	public void SetCharges(int charges){
		this.charges = charges;
	}

}

