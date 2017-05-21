using System;
using UnityEngine;

public class FloorFactory : MonoBehaviour{
	
	public GameObject StoneFloor;
	
	public GameObject CreateFloor(String floorname, Vector3 position){
		switch(floorname){
			case "Stonefloor":
				return Instantiate(StoneFloor, position, Quaternion.Euler(0,180,0)) as GameObject;
			default:
				return Instantiate(StoneFloor, position, Quaternion.Euler(018,0,0)) as GameObject;
		}
	}
}

