using System;
using UnityEngine;

public class HealSkill : PointTargetSkill {

	public String Name = "Heal";

 	public HealSkill (Unit Caster, Vector3 PointTarget) : base(Caster, PointTarget)
	{
		this.action = delegate(){
			TimedStatus status = new TimedStatus("Heal",2f,Caster);
			status.StartEffect = delegate(){
				Renderer render = Caster.gameObject.GetComponent<Renderer>() as Renderer;
				render.material.color = new Color(render.material.color.r-0.2f,render.material.color.g-0.2f,render.material.color.b);
			};
			status.EndEffect = delegate(){
				Renderer render = Caster.gameObject.GetComponent<Renderer>() as Renderer;
				render.material.color = new Color(render.material.color.r+0.2f,render.material.color.g+0.2f,render.material.color.b);
			};
			Caster.StatusManager.AddStatus(status);
		};
	}
}

