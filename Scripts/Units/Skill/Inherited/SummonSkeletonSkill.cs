using System;
using UnityEngine;

public class SummonSkeletonSkill : PointTargetSkill {
	
	public new String Name = "SummonSkeleton";
	
	public SummonSkeletonSkill (Unit Caster, Vector3 PointTarget) : base(Caster, PointTarget){
		this.MPCost = 1;
		this.action = delegate(){
			this.PointTarget = GetPointTarget(ref PointTarget);
			Vector3 target = new Vector3(
				this.PointTarget.x,
				this.PointTarget.y,
				this.PointTarget.z
				);
			Debug.DrawLine(this.PointTarget,Camera.main.transform.position, Color.white, 5f);
			//cast to see where it intersects
			LayerMask mask = LayerMask.GetMask("Walls");
			RaycastHit WallHit = new RaycastHit();
			//from camera, as it is smite-target based (from camera)
			Physics.Linecast(Camera.main.transform.position, target, out WallHit, mask);
			mask = LayerMask.GetMask("Enemies");
			RaycastHit EnemyHit = new RaycastHit();
			Physics.Linecast(Camera.main.transform.position, target, out EnemyHit, mask);
			mask = LayerMask.GetMask("Floors");
			RaycastHit FloorHit = new RaycastHit();
			Physics.Linecast(Camera.main.transform.position, target, out FloorHit, mask);
			Debug.Log(WallHit.transform);
			Debug.Log(EnemyHit.transform);
			Debug.Log(FloorHit.transform);
			if(WallHit.transform == null && EnemyHit.transform == null){
				Caster.GameManager.level.enemies.Add(
					Caster.GameManager.UnitFactory.CreateUnit(
						"Skeleton",
						new Vector3(
							FloorHit.transform.position.x,
							-0.25f,
							FloorHit.transform.position.z
						)
					)
				);
			}
			
		};
	}
}

