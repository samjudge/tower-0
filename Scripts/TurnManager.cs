using System;
using System.Collections;
using UnityEngine;

public class TurnManager {

	private ArrayList Enemies;
	private Player p;

	public enum TurnPhase {
		Start,
		Player,
		Enemies,
		End
	} 

	public TurnPhase Phase = TurnPhase.Start;

	public float TotalATUsConsumed = 0f;

	public TurnManager (GameManager gm) {
		this.Enemies = gm.level.enemies;
		this.p = gm.Player.GetComponent<Player>();
		this.Phase = TurnPhase.End;
	}

	public IEnumerator ProcessAllTurns(){
		this.Phase = TurnPhase.Start;
		foreach(GameObject eObj in Enemies){
			//wait for all enemy inputs to become unlocked before proceding.. If they're locked it's because they
			//are in the middle of an animation and have not completed it yet
			Enemy e = eObj.GetComponent<Enemy>();
			while(e.IsInputLocked){
				this.Phase = TurnPhase.End;
				return false; //exit early
			}
		}
		this.Phase = TurnPhase.Player;
		float ATUs = this.p.ProcessTurn();
		while(p.IsInputLocked){
			yield return null;
		}
		this.Phase = TurnPhase.Enemies;
		foreach(GameObject eObj in Enemies){
			Enemy e = eObj.GetComponent<Enemy>();
			float ATUsConsumed = -e.ATUsRemaining;
			if(ATUsConsumed > 0){
				// their last action spilled into this turn, so their current turn is skipped
				e.ATUsRemaining += ATUs;
				continue;
			}
			while(ATUsConsumed < ATUs){
				ATUsConsumed += e.ProcessTurn();
			}
			//calculate remainder
			float remainingATUs = ATUs - ATUsConsumed;
			e.ATUsRemaining = remainingATUs;
		}
		this.TotalATUsConsumed += ATUs;
		this.Phase = TurnPhase.End;

	}
}