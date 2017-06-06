using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit {

	public GameObject TorchLight;
	public GameObject FloorLight;

	public bool IsInventoryOpen = false;
	public bool IsStatsViewOpen = false;
	public int FreeSLIDPoints = 0;

	public void LevelUp(){
		this.FreeSLIDPoints += 1;
	}

	public void Start(){
		this.BaseStrength = 1;
		this.Mp = 100;
		this.MaxMp = 100;
		this.Hp = 100;
		this.MaxHp = 100;
		this.Inventory = new Inventory(this);
		this.IsInputLocked = false;
		this.ActionsManager = new ActionsManager();
		this.StatusManager = new StatusManager();
		this.SkillManager = new SkillManager(this);
		this.TorchLight = Instantiate(TorchLight, this.transform.position, Quaternion.Euler(90,0,0)) as GameObject;
		this.FloorLight = Instantiate(TorchLight, this.transform.position, Quaternion.Euler(90,0,0)) as GameObject;
		ActionsManager.AddGameAction(
			"Up",
			new GameActionAttackMove(this,0,1,"Enemies","Player")
		);
		ActionsManager.AddGameAction(
			"Down",
			new GameActionAttackMove(this,0,-1,"Enemies","Player")
		);
		ActionsManager.AddGameAction(
			"Left",
			new GameActionAttackMove(this,-1,0,"Enemies","Player")
		);
		ActionsManager.AddGameAction(
			"Right",
			new GameActionAttackMove(this,1,0,"Enemies","Player")
		);
		ActionsManager.AddGameAction(
			"Grab",
			new GameActionPickUpGroundItem(this)
		);
		ActionsManager.AddGameAction(
			"UseStairs",
			new GameActionUseStairs(this)
		);
		SkillManager.AddSkill(
			"Immolate"
		);
		SkillManager.AddSkill(
			"Heal"
		);
		SkillManager.AddSkill(
			"SummonSkeleton"
		);
		//Sight Status Passive
		//(Because Skill Factory not created yet - see todo @ GameActionCastSkillByNameToPointTarget.cs)
		new SightPassive(this);
		Renderer renderer = this.GetComponentInParent<Renderer>() as Renderer;
		if (!renderer.material.HasProperty("_Color")){
			renderer.material.SetColor("_Color", Color.white);
		}
		//An inital item in the inventory
		EquipmentStatusModifierEffect daggerStatMod = new EquipmentStatusModifierEffect();
		daggerStatMod.StrMod = 10;
		EquipmentEffect[] effects = {
			daggerStatMod
		};
		Item dagger = new EquipableItem("Dagger",new String[]{"Left","Right"},effects);
		this.Inventory.AddItem(dagger);
		this.StartCoroutine(MainCameraFollowPlayer());
	}

	private float ATUsUsed = 0;

	public void Update(){
		Debug.Log(this.FreeSLIDPoints);
		this.GameManager.GetOnScreenHPBar().UpdateBar(this.Hp,this.MaxHp);
		this.GameManager.GetOnScreenMPBar().UpdateBar(this.Mp,this.MaxMp);
		this.GameManager.GetOnScreenExperienceBar().UpdateBar(this.Experience,this.CalculateRequiredExpForLevelUp());
	}

	public override float ProcessTurn(){
		ATUsUsed = 0;
		if(!IsInputLocked){
			Vector3 MousePosition = Input.mousePosition;
			if(IsInventoryOpen){
				if(Input.GetKey(KeyCode.I)){
					if(GameManager.GetImageInventoryManager().IsInventoryOpen()){
						this.GameManager.GetImageInventoryManager().ToggleInventory();
						IsInventoryOpen = false;
					}
				}
			} else if(IsStatsViewOpen) {
				if(this.IsStatsViewOpen){
					if(Input.GetKey(KeyCode.P)){
						if(GameManager.GetImageStatsManager().IsOpen()){
							this.GameManager.GetImageStatsManager().Toggle();
							this.IsStatsViewOpen = false;
						}
					}
				}
			} else {
				//inputs
				this.CastTarget = Player.TransformMouseClickToPointTarget(MousePosition);
				if(Input.GetMouseButtonDown(0)){
					this.ActionsManager.GetGameAction("Cast").action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.G)){
					GameAction a = ActionsManager.GetGameAction("Grab");
					a.action();
					ATUsUsed += 1;
				}
				if(Input.GetKey(KeyCode.N)){
					GameAction a = ActionsManager.GetGameAction("UseStairs");
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
				if(Input.GetKey(KeyCode.P)){
					if(this.GameManager.GetImageStatsManager().IsClosed()){
						this.GameManager.GetImageStatsManager().Toggle();
						this.IsStatsViewOpen = true;
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

	public static Vector3 TransformMouseClickToPointTarget(Vector3 ClickPosition){
		LayerMask mask = LayerMask.GetMask("Walls","Floors");
		RaycastHit Hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(ClickPosition);
		Physics.Raycast(ray, out Hit, Mathf.Infinity, mask);
		if(Hit.transform != null){
			return Hit.point;
		}
		return ClickPosition;
	}

}

