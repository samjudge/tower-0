using System;
using System.Reflection;
using UnityEngine;

public class WallFactory : MonoBehaviour{
		
	public GameObject BlackWall;
	public GameObject SBlocker10;

	public GameObject CreateWall(String wallname, Vector3 position){
		FieldInfo Property = this.GetType().GetField(wallname);
		Debug.Log(Property);
		GameObject WallPrefab = Property.GetValue(this) as GameObject;
		return Instantiate(WallPrefab, position, Quaternion.Euler(0,180,0)) as GameObject;
	}

	public GameObject CreateShadowBlocker(Vector3 position){
		return Instantiate(SBlocker10, position, Quaternion.Euler(0,180,0)) as GameObject;
	}
}

