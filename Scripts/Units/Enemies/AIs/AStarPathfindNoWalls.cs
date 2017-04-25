using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfindNoWalls : AStarPathfind {
	
	public AStarPathfindNoWalls(Vector3 Target,Vector3 StepSize) : base(Target,StepSize){
	}

	//Heuristic
	override protected float CalculateH(Node n){
		if(n.parent != null){
			int mask = LayerMask.GetMask("Walls");
			RaycastHit hit = new RaycastHit();
			bool isHit = Physics.Linecast(n.parent.position, n.position, out hit, mask);
			if(isHit){
				return LowestScore;
				return CalculateDistance(n.position);
			} else {
				return CalculateDistance(n.position);
			}
		} else {
			return CalculateDistance(n.position);
		}

	}
}