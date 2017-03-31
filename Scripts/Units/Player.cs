using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit {

	public override float Hp {
		get{ return 100;}
		set {}
	}
	
	public override float MaxHp {
		get{ return 100;}
		set {}
	}

	public void Start(){
		IsCurrentlyMoving = false;
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
		ActionsManager.AddGameAction(
			"Up",
		    new GameActionAttackMove(this,0,1)
		);
		ActionsManager.AddGameAction(
			"Down",
			new GameActionAttackMove(this,0,-1)
		);
		ActionsManager.AddGameAction(
			"Left",
			new GameActionAttackMove(this,-1,0)
		);
		ActionsManager.AddGameAction(
			"Right",
			new GameActionAttackMove(this,1,0)
		);
		SkillManager.AddSkill(
			"Immolate"
		);
		SkillManager.AddSkill(
			"Heal"
		);
		//Sight Status Passive
		//(Because Skill Factory not created yet - see todo @ GameActionCastSkillByNameToPointTarget.cs)
		new SightPassive(this);

	}

	public void Update(){
		CastTarget = Input.mousePosition;
		//inputs
		if(Input.GetKey(KeyCode.W)){
			GameAction a = ActionsManager.GetGameAction("Up");
			a.action();
		}
		if(Input.GetKey(KeyCode.S)){
			GameAction a = ActionsManager.GetGameAction("Down");
			a.action();
		}
		if(Input.GetKey(KeyCode.A)){
			GameAction a = ActionsManager.GetGameAction("Left");
			a.action();
		}
		if(Input.GetKey(KeyCode.D)){
			GameAction a = ActionsManager.GetGameAction("Right");
			a.action();
		}
		if(Input.GetMouseButtonDown(0)){
			//Select Skill
			ImageSkillBarManager skillbar = this.GameManager.ImageSkillBarManager;
			Vector2 pos = new Vector2();
			bool HasHitSkill = false;
			foreach(Image SkillImage in skillbar.GetImages()){
				if(RectTransformUtility.RectangleContainsScreenPoint(SkillImage.rectTransform,CastTarget,Camera.main)){
					HasHitSkill = true;
					foreach(ImageSkillBarManager.SkillMapKey k in skillbar.SkillMap.Keys){
						if(skillbar.SkillMap[k] == SkillImage){
							ActionsManager.AddGameAction(
								"Cast",
								new GameActionCastSkillByNameToPointTarget(k.s,this as Unit)
							);
							SetCurrentSkillToIndex(k.index);
						}

					}
					break;
				}
			}
			//Cast Spell
			if(HasHitSkill == false){
				GameAction a = ActionsManager.GetGameAction("Cast");
				if(a != null){
					a.action();
				}
			}
		}
		if(Input.GetKey(KeyCode.Alpha1)){
			SetCurrentSkillToIndex(0);
		}
		if(Input.GetKey(KeyCode.Alpha2)){
			SetCurrentSkillToIndex(1);
		}
		if(Input.GetKey(KeyCode.Alpha3)){
			SetCurrentSkillToIndex(2);
		}
		if(Input.GetKey(KeyCode.Alpha4)){
			SetCurrentSkillToIndex(3);
		}
		if(Input.GetKey(KeyCode.Alpha5)){
			SetCurrentSkillToIndex(4);
		}

		//camera
		Camera.main.transform.position = new Vector3(
			this.transform.position.x,
			this.transform.position.y+10,
			this.transform.position.z-3
		);
	}

	public void SetCurrentSkillToIndex(int i){
		ImageSkillBarManager skillbar = this.GameManager.ImageSkillBarManager;
		ImageSkillBarManager.SkillMapKey[] SkillMapKeys =
			new ImageSkillBarManager.SkillMapKey[skillbar.SkillMap.Keys.Count];
		skillbar.SkillMap.Keys.CopyTo(SkillMapKeys,0);
		foreach(ImageSkillBarManager.SkillMapKey k in SkillMapKeys){
			if(k.index == i){
				ActionsManager.AddGameAction(
					"Cast",
					new GameActionCastSkillByNameToPointTarget(k.s,this as Unit)
				);
				Vector3 t = GameManager.SkillBarPlaceholders[i].rectTransform.localPosition;
				SkillSelector s = (GameManager.SkillSelector.GetComponent<SkillSelector>() as SkillSelector);
				s.target = new Vector3(
					t.x,
					t.y,
					t.z
				);
			}
		}
	}


}

