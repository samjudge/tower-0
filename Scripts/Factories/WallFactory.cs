using System;
using UnityEngine;

public class WallFactory : MonoBehaviour{
		
	public GameObject BlackWall;

	public GameObject CreateWall(String wallname, Vector3 position){
		switch(wallname){
			case "Blackwall":
				return Instantiate(BlackWall, position, Quaternion.Euler(0,180,0)) as GameObject;
				break;
			default:
				return Instantiate(BlackWall, position, Quaternion.Euler(0,180,0)) as GameObject;
				break;
		}
	}
}

