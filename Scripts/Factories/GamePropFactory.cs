using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class GamePropFactory : MonoBehaviour{

	public GameObject Door;
	public GameObject StairsDown;
	
	public GameObject CreateProp(String name, Vector3 position){
		FieldInfo Property = this.GetType().GetField(name);
		GameObject PropPrefab = Property.GetValue(this) as GameObject;
		return Instantiate(PropPrefab, position, Quaternion.Euler(0,180,0)) as GameObject;
	}
}

