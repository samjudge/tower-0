using System;
using UnityEngine;

public class ImmolateSkill : PointTargetSkill {

	public String Name = "Immolate";

	public ImmolateSkill (Unit Caster, Vector3 PointTarget) : base(Caster, PointTarget){
		this.action = delegate(){
			this.PointTarget = GetPointTarget(ref PointTarget);
			Vector3 target = new Vector3(
				this.PointTarget.x,
				this.PointTarget.y,
				this.PointTarget.z
			);
			//cast to see where it intersects
			LayerMask mask = LayerMask.GetMask("Walls");
			RaycastHit WallHit = new RaycastHit();
			Physics.Linecast(Caster.transform.position, target, out WallHit, mask);
			mask = LayerMask.GetMask("Enemies");
			RaycastHit EnemyHit = new RaycastHit();
			Physics.Linecast(Caster.transform.position, target, out EnemyHit, mask);
			Debug.DrawLine(target, Caster.transform.position, Color.magenta, 5f);
			if(EnemyHit.transform != null){
				Debug.Log ("hit!");
				float enemyDistance = (Caster.transform.position - EnemyHit.transform.position).sqrMagnitude;
				if(WallHit.transform != null){
					float wallDistance = (Caster.transform.position - WallHit.transform.position).sqrMagnitude;
					if(wallDistance < enemyDistance){
						return;
					}
				}
				GameObject g = EnemyHit.collider.gameObject;
				Unit u = g.GetComponent<Unit>();
				TimedStatus status = new TimedStatus("Immolate",4,u);
				status.TickTime = 0.1f;
				status.StartEffect = delegate(){
					Renderer render = u.GetComponent<Renderer>();
					render.material.color = new Color(render.material.color.r,render.material.color.g-0.3f,render.material.color.b-0.3f);
				};
				status.TickEffect = delegate(){
					u.Hp -= 0.1f;
				};
				status.EndEffect = delegate(){
					Renderer render = u.GetComponent<Renderer>();
					render.material.color = new Color(render.material.color.r,render.material.color.g+0.3f,render.material.color.b+0.3f);
				};

				u.StatusManager.AddStatus(status);
			}

		};
	}
}

