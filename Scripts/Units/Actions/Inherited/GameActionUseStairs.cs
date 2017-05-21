using System;
using System.Collections;
using UnityEngine;

public class GameActionUseStairs : GameAction {
	
	public Unit stairUser;
	
	public GameActionUseStairs(Unit stairUser){
		this.stairUser = stairUser;
		this.action = delegate(){
			if(stairUser.IsInputLocked == false){
				ArrayList PropClones = new ArrayList(stairUser.GameManager.level.gameobjects);
				foreach(GameObject t in PropClones){
					Debug.Log(t);
					if((t.GetComponent<BoxCollider>() as BoxCollider).bounds.Contains(stairUser.transform.position)){
						if((t.GetComponent<GameProp>() as GameProp).Name == "StairsDown"){
							Debug.Log(".");
							stairUser.GameManager.level.ResetLevel();
							stairUser.GameManager.level.GenerateDungeonLevel();
							return;
						}
					} 
				}
			}
		};
	}
}

