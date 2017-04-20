using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfind {
	
	public Vector3 Target;
	public ArrayList ClosedNodes;
	public ArrayList OpenNodes;
	public float LowestScore = 99999;
	public Vector3 StepSize;

	public AStarPathfind(Vector3 Target,Vector3 StepSize){
		this.Target = Target;
		this.StepSize = StepSize;
		this.ClosedNodes = new ArrayList();
		this.OpenNodes = new ArrayList();
	}

	private ArrayList GetNeighborOpenNodes(Node node){
		ArrayList nodeList = new ArrayList();
		for(int x = -1;x < 2;x++){
			for(int y = -1;y < 2;y++){
				//no diagonals
				if(x == -1 && y == -1){ 
					continue;
				}
				if(x == 1 && y == -1){
					continue;
				}
				if(x == -1 && y ==1){
					continue;
				}
				if(x == 1 && y == 1){
					continue;
				}
				Vector3 nNode = new Vector3((node.position.x+(StepSize.x*(x))), node.position.y, (node.position.z+(StepSize.z*(y))));
				Node n = new Node();
				n.position = nNode;
				n.value = CalculateDistance(nNode);
				n.pathscore = (node.pathscore) + n.value;
				if(IsClosed(n)){
					continue; //ignore
				}
				if(IsOpen(n)){
					Node oNode = GetOpen(n); //recalculate
					if(n.pathscore < oNode.pathscore){
						oNode.parent = node;
						oNode.value = n.value;
					}
					continue;
				}
				nodeList.Add(n);
			}
		}
		return nodeList;
	}

	private Node GetOpen(Node n){
		foreach(Node CNode in OpenNodes){
			if(CNode.position == n.position){
				return CNode;
			}
		}
		throw new Exception("No Such Node Exists (A*)");
	}

	private bool IsOpen(Node n){
		foreach(Node CNode in OpenNodes){
			if(CNode.position == n.position){
				return true;
			}
		}
		return false;
	}

	private bool IsClosed(Node Node){
		foreach(Node CNode in ClosedNodes){
			if(CNode.position == Node.position){
				return true;
			}
		}
		return false;
	}

	public class Node : IComparable {
		public Node(){}
		public float pathscore;
		public float value;
		public Vector3 position;
		public Node parent;

		public virtual int CompareTo(object obj) {
			Node n = obj as Node;
			return (pathscore.CompareTo(n.pathscore));
		}
	}
	
	public Vector3 GetTopNode(){
		return ((Node) this.OpenNodes[0]).position;
	}

	int MaxSearchDepth = 10;

	public Node FindPath(Node Position){
		ArrayList Adjacent = this.GetNeighborOpenNodes(Position);
		Debug.Log("pathfind");
		bool pathFound = false;
		int searchDepth = 0;
		while(!pathFound && searchDepth < MaxSearchDepth){
			searchDepth++;
			this.ClosedNodes.Add(Position);
			Adjacent = this.GetNeighborOpenNodes(Position);
			Adjacent.Sort();
			Position = Adjacent[0] as Node;
			this.OpenNodes.AddRange(Adjacent);
			if(Position.value <= 1f){
				pathFound = true;
				break;
			}
		}
		return Position;
	}

	//Heuristic
	private float CalculateDistance(Vector3 Position){
		return (this.Target - Position).sqrMagnitude;
	}
}
