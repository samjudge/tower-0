using System;
using UnityEngine;

public class SummonSkeletonSkill : PointTargetSkill {
	
	public new String Name = "SummonSkeleton";
	
	public SummonSkeletonSkill (Unit Caster, Vector3 PointTarget) : base(Caster, PointTarget){
		this.MPCost = 1;
		this.action = delegate(){
			Vector3 target = new Vector3(
				this.PointTarget.x,
				this.PointTarget.y,
				this.PointTarget.z
			);
			Debug.DrawLine(target,Caster.transform.position, Color.white, 5f);
			//cast to see where it intersects
			LayerMask mask = LayerMask.GetMask("Walls");
			RaycastHit WallHit = new RaycastHit();
			//from camera, as it is smite-target based (from camera)
			Physics.Linecast(Caster.transform.position, target, out WallHit, mask);
			mask = LayerMask.GetMask("Enemies");
			RaycastHit EnemyHit = new RaycastHit();
			Physics.Linecast(Caster.transform.position, target, out EnemyHit, mask);
			if(WallHit.transform == null && EnemyHit.transform == null){
				Caster.GameManager.level.enemies.Add(
					Caster.GameManager.UnitFactory.CreateUnit(
						"Skeleton",
						new Vector3(
							target.x,
							-0.25f,
							target.z
						)
					)
				);
			}
			
		};
	}
}

