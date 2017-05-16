using System;
using UnityEngine;

public class HealSkill : PointTargetSkill {

	public new String Name = "Heal";

 	public HealSkill (Unit Caster, Vector3 PointTarget) : base(Caster, PointTarget)
	{
		this.MPCost = 1;
		this.action = delegate(){
			ATUTimedStatus status = new ATUTimedStatus("Heal",5f,Caster.GameManager.TurnManager,Caster);
			status.StartEffect = delegate(){
				Renderer render = Caster.gameObject.GetComponent<Renderer>() as Renderer;
				render.material.color = new Color(render.material.color.r-0.2f,render.material.color.g-0.2f,render.material.color.b);
			};
			status.TickEffect = delegate(){
				if((Caster.Hp + 2) < Caster.MaxHp){
					Caster.Hp += 2;
				} else {
					Caster.Hp = Caster.MaxHp;
				}
			};
			status.EndEffect = delegate(){
				Renderer render = Caster.gameObject.GetComponent<Renderer>() as Renderer;
				render.material.color = new Color(render.material.color.r+0.2f,render.material.color.g+0.2f,render.material.color.b);
			};
			Caster.StatusManager.AddStatus(status);
		};
	}
}

