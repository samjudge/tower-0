using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit {

	private float cHp = 100;

	public override float Hp {
		get{ return cHp;}
		set {}
	}
	
	public override float MaxHp {
		get{ return 100;}
		set { cHp = value;}
	}

	private float cMp = 10;

	public override float Mp {
		get{ return cMp;}
		set { cMp = value; }
	}
	
	public override float MaxMp {
		get{ return 10;}
		set {}
	}


	public GameObject TorchLight;
	public GameObject FloorLight;

	public bool IsInventoryOpen = false;

	public void Start(){
		this.Inventory = new Inventory(this);
		this.IsInputLocked = false;
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
		this.TorchLight = Instantiate(TorchLight, this.transform.position, Quaternion.Euler(90,0,0)) as GameObject;
		this.FloorLight = Instantiate(TorchLight, this.transform.position, Quaternion.Euler(90,0,0)) as GameObject;
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
		ActionsManager.AddGameAction(
			"Grab",
			new GameActionPickUpGroundItem(this)
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
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		//An inital item in the inventory
		EquipmentEffect[] effects = {
			new EquipmentStatusModifierEffect(1,0,0,0)
		};
		Item dagger = new EquipableItem("Dagger",new String[]{"Left","Right"},effects);
		this.Inventory.AddItem(dagger);
		this.StartCoroutine(MainCameraFollowPlayer());
	}

	private float ATUsUsed = 0;

	public void Update(){
		this.GameManager.GetOnScreenHPBar().UpdateBar(this.Hp,this.MaxHp);
		this.GameManager.GetOnScreenMPBar().UpdateBar(this.Mp,this.MaxMp);
	}

	public override float ProcessTurn(){
		ATUsUsed = 0;
		if(!IsInputLocked){
			Vector3 MousePosition = Input.mousePosition;
			if(IsInventoryOpen){
				if(Input.GetKey(KeyCode.I)){
					if(GameManager.GetImageInventoryManager().IsInventoryOpen()){
						this.GameManager.GetImageInventoryManager().ToggleInventory();
						this.IsInventoryOpen = false;
					}
				}
			} else {
				//inputs
				this.CastTarget = MousePosition;
				if(Input.GetMouseButtonDown(0)){
					this.ActionsManager.GetGameAction("Cast").action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.G)){
					GameAction a = ActionsManager.GetGameAction("Grab");
					a.action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.W)){
					GameAction a = ActionsManager.GetGameAction("Up");
					a.action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.S)){
					GameAction a = ActionsManager.GetGameAction("Down");
					a.action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.A)){
					GameAction a = ActionsManager.GetGameAction("Left");
					a.action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.D)){
					GameAction a = ActionsManager.GetGameAction("Right");
					a.action();
					ATUsUsed += 1;
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
				if(Input.GetKey(KeyCode.I)){
					if(this.GameManager.GetImageInventoryManager().IsInventoryClosed()){
						this.GameManager.GetImageInventoryManager().ToggleInventory();
						this.IsInventoryOpen = true;
					}
				}
			}
		}
		return ATUsUsed;
	}

	public IEnumerator MainCameraFollowPlayer(){
		while(true){
			Camera.main.transform.position = new Vector3(
				this.transform.position.x,
				this.transform.position.y+10,
				this.transform.position.z-3
			);
			this.TorchLight.transform.position = new Vector3(
				this.transform.position.x,
				4f,
				this.transform.position.z
			);
			this.FloorLight.transform.position = new Vector3(
				this.transform.position.x,
				0.1f,
				this.transform.position.z
			);
			yield return null;
		}
	}

	public void SetCurrentSkillToIndex(int i){
		ImageSkillBarManager Skillbar = this.GameManager.UISkillBar.GetComponent<ImageSkillBarManager>();
		ImageSkillBarManager.SkillMapKey[] SkillMapKeys =
			new ImageSkillBarManager.SkillMapKey[Skillbar.GetSkillMap().Keys.Count];
		Skillbar.GetSkillMap().Keys.CopyTo(SkillMapKeys,0);
		foreach(ImageSkillBarManager.SkillMapKey k in SkillMapKeys){
			if(k.index == i){
				ActionsManager.AddGameAction(
					"Cast",
					new GameActionCastSkillByNameToPointTarget(k.s,this as Unit)
				);
				Vector3 target = Skillbar.Placeholders[i].rectTransform.localPosition;
				SkillSelector s = (Skillbar.SkillSelector.GetComponent<SkillSelector>() as SkillSelector);
				s.target = new Vector3(
					target.x,
					target.y,
					target.z
				);
			}
		}
	}


}

