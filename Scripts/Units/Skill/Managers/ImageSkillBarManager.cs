using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageSkillBarManager : MonoBehaviour
{
	private SkillManager SkillManager;

	public GUIImageFactory GuiFactory;
	public Image[] Placeholders;
	public bool IsRunning = false;

	public GameObject SkillSelector;

	private Dictionary<SkillMapKey,Image> SkillMap;

	public Dictionary<SkillMapKey,Image> GetSkillMap(){
		return SkillMap;
	}

	public SkillManager GetSkillManager(){
		return SkillManager;
	}

	public void SetSkillManagerAndInit(SkillManager s){
		this.SkillManager = s;
		this.IsRunning = true;
		this.SkillManager.Owner.StartCoroutine(DrawSkills());
	}
	
	private Image InitalizeSkill(SkillMapKey s){
		Image i = GuiFactory.CreateImage(s.s,new Vector3(0f,0f,-6f));
		Image placeholder = Placeholders[s.index] as Image;
		(i.GetComponent<Animator>() as Animator).enabled = false;
		i.transform.SetParent(placeholder.rectTransform, false);
		i.GetComponent<RectTransform>().position = placeholder.transform.position;
		return i;
	}

	public class SkillMapKey {
		public String s;
		public int index;
	}

	public Image[] GetImages(){
		Image[] I = new Image[SkillMap.Values.Count];
		SkillMap.Values.CopyTo(I,0);
		return I;
	}

	public IEnumerator DrawSkills(){
		SkillMap = new Dictionary<SkillMapKey,Image>();
		int keyCount = 0;
		foreach(String s in SkillManager.Skills){
			SkillMapKey newKey = new SkillMapKey();
			newKey.s = s;
			newKey.index = keyCount;
			Image i = InitalizeSkill(newKey);
			SkillMap.Add(newKey,i);
			keyCount++;
		}
		while(IsRunning){
			SkillMapKey[] kArra = new SkillMapKey[SkillMap.Keys.Count];
			SkillMap.Keys.CopyTo(kArra,0);
			ArrayList keys = new ArrayList(kArra);
			//new Skills
			foreach(String s in SkillManager.Skills){
				bool containsSkill = false;
				foreach(SkillMapKey key in keys){
					if(key.s == s){
						containsSkill = true;
						break;
					}
				}
				if(containsSkill == false){
					SkillMapKey newKey = new SkillMapKey();
					newKey.s = s;
					newKey.index = keyCount+1;
					Image i = InitalizeSkill(newKey);
					SkillMap.Add(newKey,i);
					keyCount++;
				}
			}
			kArra = new SkillMapKey[SkillMap.Keys.Count];
			SkillMap.Keys.CopyTo(kArra,0);
			keys = new ArrayList(kArra);
			for(int x = 0; x < keys.Count ; x++){
				SkillMapKey s = keys[x] as SkillMapKey;
				if(!SkillManager.Skills.Contains(s.s)){
					SkillMap.Remove(s);
					for(int n = 0; n < keys.Count; n++){
						SkillMapKey st = keys[n] as SkillMapKey;
						if(st.index > s.index){
							st.index--;
						}
					}
					keyCount--;
				}
			}
			kArra = new SkillMapKey[SkillMap.Keys.Count];
			SkillMap.Keys.CopyTo(kArra,0);
			keys = new ArrayList(kArra);
			//update postion
			for(int x = 0; x < keys.Count ; x++){/**/}
			yield return null;
		}
	}
}

