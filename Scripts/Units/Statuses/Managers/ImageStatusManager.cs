using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageStatusManager
{
	public StatusManager StatusManager {get;set;}
	public GUIImageFactory GuiFactory {get;set;}
	public bool IsRunning = true;

	public ImageStatusManager(StatusManager StatusManager,GUIImageFactory GuiFactory){
		this.StatusManager = StatusManager;
		this.GuiFactory = GuiFactory;
		GuiFactory.StartCoroutine(DrawStatuses());
	}

	private Image InitalizeImage(Status s){
		Image i = GuiFactory.CreateImage(s.Name,new Vector3(0f,0f,-6f));
		i.rectTransform.sizeDelta = new Vector3(45,49,3);
		i.transform.SetParent(s.Owner.GameManager.CanvasStatuses.transform, false);
		i.GetComponent<RectTransform>().localPosition = new Vector3((StatusManager.Statuses.Count*64),0,0);
		Animator a = i.GetComponent<Animator>() as Animator;
		a.Play("StatusAnimationStart");
		return i;
	}

	public class StatusMapKey {
		public Status s;
		public int index;
	}

	public IEnumerator DrawStatuses(){
		Dictionary<StatusMapKey,Image> StatusMap = new Dictionary<StatusMapKey,Image>();
		int keyCount = 0;
		foreach(Status s in StatusManager.Statuses){
			Image i = InitalizeImage(s);
			StatusMapKey newKey = new StatusMapKey();
			newKey.s = s;
			newKey.index = keyCount;
			StatusMap.Add(newKey,i);
			keyCount++;
		}
		float drawFrom = StatusMap.Keys.Count;
		while(IsRunning){
			StatusMapKey[] kArra = new StatusMapKey[StatusMap.Keys.Count];
			StatusMap.Keys.CopyTo(kArra,0);
			ArrayList keys = new ArrayList(kArra);
			//new statuses
			foreach(Status s in StatusManager.Statuses){
				bool containsStatus = false;
				foreach(StatusMapKey key in keys){
					if(key.s == s){
						containsStatus = true;
						break;
					}
				}
				if(containsStatus == false){
					StatusMapKey newKey = new StatusMapKey();
					newKey.s = s;
					newKey.index = keyCount;
					Image i = InitalizeImage(s);
					StatusMap.Add(newKey,i);
					keyCount++;
				}
			}
			kArra = new StatusMapKey[StatusMap.Keys.Count];
			StatusMap.Keys.CopyTo(kArra,0);
			keys = new ArrayList(kArra);
			//removed statuses
			for(int x = 0; x < keys.Count ; x++){
				StatusMapKey s = keys[x] as StatusMapKey;
				if(!StatusManager.Statuses.Contains(s.s)){
					Image i = StatusMap[s];
					Animator a = i.GetComponent<Animator>() as Animator;
					a.SetBool("EndAnimationPlay",true);
					a.Play("StatusAnimationEnd");
					s.s.Owner.StartCoroutine(DestroyOnEndAnimation(i));
					StatusMap.Remove(s);
					for(int n = 0; n < keys.Count; n++){
						StatusMapKey st = keys[n] as StatusMapKey;
						if(st.index > s.index){
							st.index--;
						}
					}
					keyCount--;
				}
			}
			kArra = new StatusMapKey[StatusMap.Keys.Count];
			StatusMap.Keys.CopyTo(kArra,0);
			keys = new ArrayList(kArra);
			//update postion
			for(int x = 0; x < keys.Count ; x++){
				StatusMapKey s = keys[x] as StatusMapKey;
				Image i = StatusMap[s];
				Animator a = i.GetComponent<Animator>() as Animator;
				Vector3 targetPosition = new Vector3((s.index*64),0,1);
				s.s.Owner.StartCoroutine(UpdatePosition(i,targetPosition));
			}
			yield return null;
		}
	}

	private IEnumerator DestroyOnEndAnimation(Image i){
		Animator a = i.GetComponent<Animator>() as Animator;
		while(a.GetCurrentAnimatorClipInfo(0).Length < 1){
			yield return null;
		}
		AnimatorStateInfo state = a.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo clip = a.GetCurrentAnimatorClipInfo(0)[0];
		float current_time = 0;
		float length = clip.clip.length;
		while(current_time < length || a.IsInTransition(0)){
			current_time += Time.deltaTime;
			yield return null;
		}
		MonoBehaviour.Destroy(i.gameObject);
	}

	private IEnumerator UpdatePosition(Image i, Vector3 targetPosition){
		float timer = 0;
		while((i.transform.localPosition - targetPosition).sqrMagnitude > Vector3.kEpsilon){
			timer += Time.deltaTime/50;
			i.transform.localPosition = Vector3.Lerp(i.transform.localPosition,targetPosition,timer);
			yield return null;
			if(!(i != null)){
				break;
			}
		}
		yield return null;
	}
}

