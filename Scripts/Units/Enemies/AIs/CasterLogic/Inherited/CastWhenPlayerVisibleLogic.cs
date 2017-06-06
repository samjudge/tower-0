using System;

public class CastWhenPlayerVisibleLogic : CasterLogic{

	Enemy Caster;

	public CastWhenPlayerVisibleLogic(Enemy e){
		this.Caster = e;
	}

	public String GetSpellName(){
		Player p = Caster.GameManager.Player.GetComponent<Player>();
		bool isInLos = this.Caster.CheckIsInLOSOf(p);
		if(isInLos){
			//get spell with highest priority
			Spell HighestPriority = new Spell();
			HighestPriority.name = "";
			HighestPriority.priority = 0f;
			foreach(Spell s in Caster.Spells){
				if(s.priority > HighestPriority.priority){
					HighestPriority = s;
				} else if (s.priority == HighestPriority.priority){
					//flip a coin to see if it should cast the new spell
					System.Random r = new System.Random();
					float roll = r.Next(1000);
					if(roll > 500){
						HighestPriority = s;
					}
				}
			}
			return HighestPriority.name;
		}
		return null;
	}

	public float GetLogicPriority(){
		return 1f;
	}
}

