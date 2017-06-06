using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfindLowest : AStarPathfind {

	public AStarPathfindLowest(Vector3 Target,Vector3 StepSize) : base(Target,StepSize){
	}
	
	int MaxSearchDepth = 60;
	
	public Node FindPath(Node Position){
		this.ClosedNodes.Add(Position);
		ArrayList Adjacent = this.GetNeighborOpenNodes(Position);
		Adjacent.Sort();
		Node furthest = Adjacent[0] as Node;
		foreach(Node n in Adjacent){
			Debug.Log (n.value);
			if(n.value > furthest.value && n.value != this.LowestScore){
				Debug.Log ("New Target");
				furthest = n;
			}
		}
		Debug.Log(furthest.pathscore);
		Position = furthest;
		this.OpenNodes.AddRange(Adjacent);
		return Position;
	}

	//Heuristic
	override protected float CalculateH(Node n){
		if(n.parent != null){
			int mask = LayerMask.GetMask("Walls");
			RaycastHit hit = new RaycastHit();
			bool isHit = Physics.Linecast(n.parent.position, n.position, out hit, mask);
			if(isHit){
				Debug.Log ("Wall Here");
				return LowestScore;
			} else {
				return CalculateDistance(n.position);
			}
		} else {
			return CalculateDistance(n.position);
		}
		
	}
}