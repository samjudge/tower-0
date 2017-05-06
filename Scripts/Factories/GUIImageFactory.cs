using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIImageFactory : MonoBehaviour{

	//statuses+skill icons
	public Image Immolate;
	public Image Heal;
	public Image Sight;
	public Image Placeholder;

	//extra
	public Image Green;
	public Image Red;

	//items
	public Image Chicken;
	public Image Dagger;

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
		case "Chicken":
			return Instantiate(Chicken, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Dagger":
			return Instantiate(Dagger, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Red":
			return Instantiate(Red, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		case "Placeholder":
			return Instantiate(Placeholder, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		default:
			return Instantiate(Placeholder, position, Quaternion.Euler(0,0,0)) as Image;
			break;
		}
	}
}

