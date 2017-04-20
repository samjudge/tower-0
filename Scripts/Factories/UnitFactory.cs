using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitFactory : MonoBehaviour{
	
	public GameObject DummyEnemy;
	public GameObject ChickenEnemy;

	public GameObject CreateUnit(String name, Vector3 position){
		switch(name){
		case "Dummy":
			return Instantiate(DummyEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
			break;
		case "Chicken":
			return Instantiate(ChickenEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
			break;
		default:
			return Instantiate(DummyEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
			break;
		}
	}
}

