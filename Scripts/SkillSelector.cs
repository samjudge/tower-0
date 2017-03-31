using UnityEngine;
using System.Collections;

public class SkillSelector : MonoBehaviour {

	bool isMoving = true;
	public Vector3 target;

	void Start(){
		this.StartCoroutine(Move());
	}

	void Update () {
	}
	
	public IEnumerator Move(){
		while(isMoving){
			if(target != null){
				while(((this.GetComponent<RectTransform>() as RectTransform).localPosition - target).sqrMagnitude > Vector3.kEpsilon){
					Vector3 toTarget = new Vector3(target.x,target.y,target.z);
					(this.GetComponent<RectTransform>() as RectTransform).localPosition = Vector3.Lerp((this.GetComponent<RectTransform>() as RectTransform).localPosition,toTarget,Time.deltaTime*4);
					yield return null;
				}
			}
			yield return null;
		}
		yield return null;
	}
	
}
