using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePropFactory : MonoBehaviour{

	public GameObject Door;
	
	public GameObject CreateProp(String name, Vector3 position){
		Debug.Log("Prop");
		switch(name){
		case "Door":
			return Instantiate(Door, position, Quaternion.Euler(0,90,0)) as GameObject;
			break;
		default:
			return Instantiate(Door, position, Quaternion.Euler(0,90,0)) as GameObject;
			break;
		}
	}
}

