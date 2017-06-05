using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GUIImageFactory : MonoBehaviour{

	//statuses+skill icons
	public Image Immolate;
	public Image Heal;
	public Image Sight;
	public Image Placeholder;
	public Image SummonSkeleton;

	//extra
	public Image Green;
	public Image Red;

	//items
	public Image Chicken;
	public Image Dagger;
	public Image HealthPotion;
	public Image ManaPotion;
	public Image NiceCheese;
	public Image Sword;
	public Image WeirdCheese;

	public Image CreateImage(String name, Vector3 position){
		FieldInfo Property = this.GetType().GetField(name);
		Image GUIPrefab = Property.GetValue(this) as Image;
		Image nImage = Instantiate(GUIPrefab, position, Quaternion.Euler(0,180,0)) as Image;
		nImage.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
		return nImage;
	}
}

