﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public FloorFactory FloorFactory;
	public WallFactory WallFactory;
	public GUIImageFactory GuiFactory;
	public UnitFactory UnitFactory;
	public GamePropFactory GamePropFactory;
	public ItemFactory ItemFactory;

	public Canvas Canvas; //for in-game ui (static)
	public CanvasGroup CanvasStatuses;

	public Canvas Overlay; //for hp bars
	
	public ImageStatusManager ImageStatusManager;
	
	private ImageSkillBarManager ImageSkillBarManager;
	public GameObject UISkillBar;
	
	private ImageInventoryManager ImageInventoryManager;
	public GameObject UIInventory;

	private UIHorizontalFillBar HPBar;
	public GameObject UIHealthBar;

	private UIHorizontalFillBar MPBar;
	public GameObject UIManaBar;

	public GameObject PlayerPrefab;
	public GameObject Player;



	public UIHorizontalFillBar GetOnScreenHPBar(){
		return this.HPBar;
	}

	public UIHorizontalFillBar GetOnScreenMPBar(){
		return this.MPBar;
	}

	public ImageSkillBarManager GetImageSkillBarManager(){
		return this.ImageSkillBarManager;
	}

	public ImageInventoryManager GetImageInventoryManager(){
		return this.ImageInventoryManager;
	}

	public LevelManager level;

	private void LoadNewLevel(string levelName){
		level = new LevelManager(levelName,this);
		level.GenerateDungeonLevel();
	}

	void Start () {
		this.HPBar = UIHealthBar.GetComponent<UIHorizontalFillBar>();
		this.MPBar = UIManaBar.GetComponent<UIHorizontalFillBar>();
		this.ImageSkillBarManager = this.UISkillBar.GetComponent<ImageSkillBarManager>();
		this.ImageInventoryManager = this.UIInventory.GetComponent<ImageInventoryManager>();
		this.LoadNewLevel("Dungeon");
	}

	public TurnManager TurnManager;

	void Update () {
		if(ImageStatusManager == null && Player != null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageStatusManager = new ImageStatusManager(
				p.StatusManager,
				GuiFactory
			);
		}
		if(!ImageInventoryManager.IsRunning && Player != null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageInventoryManager.SetInventoryAndInit(p.Inventory);
		}
		if(!ImageSkillBarManager.IsRunning && Player != null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageSkillBarManager.SetSkillManagerAndInit(p.SkillManager);
			(ImageSkillBarManager.SkillSelector.GetComponent<RectTransform>() as RectTransform).localPosition =
				this.ImageSkillBarManager.Placeholders[0].rectTransform.localPosition;
			p.SetCurrentSkillToIndex(0);

		}
		if(TurnManager == null){
			if(Player != null){
				Player p = Player.GetComponent<Player>();
				if(p != null){
					TurnManager = new TurnManager(this);
				}
			}
		}
		if(TurnManager != null){
			if(TurnManager.Phase == TurnManager.TurnPhase.End){
				this.StartCoroutine(TurnManager.ProcessAllTurns());
			}
		}
	}
}
