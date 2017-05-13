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
		this.Enemies = gm.enemies;
		this.p = gm.GetPlayer().GetComponent<Player>();
	}

	public void ProcessAllTurns(){
		this.Phase = TurnPhase.Start;
		this.Phase = TurnPhase.Player;
		foreach(GameObject eObj in Enemies){
			//wait for all enemy inputs to become unlocked before proceding.. If they're locked it's because they
			//are in the middle of an animation and have not completed it yet
			Enemy e = eObj.GetComponent<Enemy>();
			if(e.IsInputLocked){
				return; //exit early
			}
		}
		float ATUs = this.p.ProcessTurn();
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