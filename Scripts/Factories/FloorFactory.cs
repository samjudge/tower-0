using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class FloorFactory : MonoBehaviour{
	
	public GameObject StoneFloor;
	
	public GameObject CreateFloor(String floorname, Vector3 position){
		FieldInfo Property = this.GetType().GetField(floorname);
		GameObject FloorPrefab = Property.GetValue(this) as GameObject;
		return Instantiate(FloorPrefab, position, Quaternion.Euler(0,180,0)) as GameObject;
	}
}

