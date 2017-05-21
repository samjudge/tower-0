using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePropFactory : MonoBehaviour{

	public GameObject Door;
	public GameObject StairsDown;
	
	public GameObject CreateProp(String name, Vector3 position){
		switch(name){
		case "Door":
			return Instantiate(Door, position, Quaternion.Euler(0,90,0)) as GameObject;
		case "StairsDown":
			return Instantiate(StairsDown, position, Quaternion.Euler(0,180,0)) as GameObject;
		default:
			return Instantiate(Door, position, Quaternion.Euler(0,90,0)) as GameObject;
		}
	}
}

