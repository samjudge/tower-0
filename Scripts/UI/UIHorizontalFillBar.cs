using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHorizontalFillBar : MonoBehaviour {

	public GameManager GameManager;
	Image Bar;

	void Start(){
		this.Bar = this.GetComponent<Image>() as Image;
	}

	public void UpdateBar(float val, float maxVal){
		this.Bar.fillAmount = val/maxVal;
	}

}
