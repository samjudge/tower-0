using System;
using System.Collections;
using UnityEngine;

public class GameActionMeleeOpenDoor : GameAction {
	
	//a test update
	
	public GameObject door;

	public bool isOpen;

	public GameActionMeleeOpenDoor (GameObject attacker){
		this.action = delegate(){
			if(!isOpen){
				Animator a = attacker.GetComponent<Animator>() as Animator;
				a.SetBool("OpenAnimationPlay",true);
				isOpen = true;
			}
		};
	}
}

