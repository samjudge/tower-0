using UnityEngine;
using System.Collections;

public class IncreaseStatButton : MonoBehaviour {

	public GameManager GameManager;
	public string StatName;
	
	public void OnPress(){
		Player p = GameManager.Player.GetComponent<Player>() as Player;
		if(p.FreeSLIDPoints > 0){
			p.FreeSLIDPoints -= 1;
			switch(StatName){
				case "Strength":
					p.BaseStrength += 1;
					break;
				case "Intelligence":
					p.BaseIntelligence += 1;
					break;
				case "Dexterity":
					p.BaseDexterity += 1;
					break;
				case "Luck":
					p.BaseLuck += 1;
					break;
			}
		}
	}

}
