using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIImageFactory : MonoBehaviour{
	
	public Image Immolate;
	public Image Heal;
	public Image Sight;
	public Image Green;
	public Image Red;

	public Image CreateImage(String name, Vector3 position){
		switch(name){
		case "Immolate":
			return Instantiate(Immolate, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Heal":
			return Instantiate(Heal, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Sight":
			return Instantiate(Sight, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Green":
			return Instantiate(Green, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Red":
			return Instantiate(Red, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		default:
			return Instantiate(Immolate, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		}
	}
}

