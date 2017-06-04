using System;
using System.Collections;
using UnityEngine;

public class LevelManager {

	public GameManager gm;

	private ArrayList map;

	private string levelName;

	private int mapWidth = 25;
	private int mapHeight = 25;
	
	public ArrayList walls {get;set;}
	public ArrayList floors {get;set;}
	public ArrayList enemies {get;set;}
	public ArrayList items {get;set;}
	public ArrayList gameobjects {get;set;}

	public bool markedForReset = false;

	public LevelManager (string levelName, GameManager gm) {
		walls = new ArrayList();
		floors = new ArrayList();
		enemies = new ArrayList();
		gameobjects = new ArrayList();
		items = new ArrayList();
		this.levelName = levelName;
		this.gm = gm;
	}

	public void SetLevelName(string levelName){
		this.levelName = levelName;
	}

	public string GetLevelName(){
		return this.levelName;
	}

	public void ResetLevel(){
		//delete old refs
		foreach(GameObject w in walls){
			GameObject.Destroy(w);
		}
		walls = new ArrayList();
		foreach(GameObject f in floors){
			GameObject.Destroy(f);
		}
		floors = new ArrayList();
		foreach(GameObject e in enemies){
			GameObject.Destroy(e);
		}
		enemies = new ArrayList();
		foreach(GameObject p in gameobjects){
			GameObject.Destroy(p);
		}
		gameobjects = new ArrayList();
		foreach(GameObject i in items){
			GameObject.Destroy(i);
		}
		items = new ArrayList();
		this.markedForReset = true;
	}

	public int level = 0;

	public void GenerateDungeonLevel(){
		//generate new dungeon
		level++;
		this.markedForReset = false;
		DungeonGenerator dg = new DungeonGenerator(this);
		this.map = dg.m.tiles;
		for(int z = 0; z < mapHeight ; z++){
			for(int x = 0; x < mapWidth; x++){
				DungeonGenerator.Tile tileCode = map[x + z*mapWidth] as DungeonGenerator.Tile;
				switch(tileCode.tag){
				case "BlackWall":
					walls.Add(gm.WallFactory.CreateWall("BlackWall",new Vector3(x,0,z)));
					walls.Add(gm.WallFactory.CreateShadowBlocker(new Vector3(x,0f,z)) as GameObject);
					break;
				case "StoneFloor":
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					break;
				case "StairsDown":
					GameObject stairDown = gm.GamePropFactory.CreateProp("StairsDown",new Vector3(x,-0.5f,z));
					GameProp stairDownProp = stairDown.GetComponent<GameProp>() as GameProp;
					this.gameobjects.Add(stairDown);
					break;
				case "Player":
					if(gm.Player == null){
						gm.Player = GameObject.Instantiate(gm.PlayerPrefab,new Vector3(x,-0.25f,z),Quaternion.Euler(15,180,0)) as GameObject;
						(gm.Player.GetComponent<Player>() as Player).GameManager = gm;
					} else {
						gm.Player.transform.position = new Vector3(x,-0.25f,z);
					}
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					break;
				case "HealthPotion" :
					GameObject healthPotion = gm.ItemFactory.CreateItem("HealthPotion",new Vector3(x,-0.25f,z)) as GameObject;
					items.Add(healthPotion);
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					break;
				case "ManaPotion" :
					GameObject manaPotion = gm.ItemFactory.CreateItem("ManaPotion",new Vector3(x,-0.25f,z)) as GameObject;
					items.Add(manaPotion);
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					break;
				case "Door":
					GameObject d = null;
					GameObject sW = null;
					if(tileCode.rotation.eulerAngles == new Vector3(0,90,0)){
						d = gm.GamePropFactory.CreateProp("Door",new Vector3(x-0.3f,0.25f,z+0.25f)) as GameObject;
						sW = gm.WallFactory.CreateShadowBlocker(new Vector3(x,5f,z)) as GameObject;
						sW.transform.localScale = new Vector3(0.1f,10f,1f);
						sW.transform.rotation = Quaternion.Euler(0,90,0);
					} else if(tileCode.rotation.eulerAngles == new Vector3(0,0,0)){
						d = gm.GamePropFactory.CreateProp("Door",new Vector3(x-0.3f,0.25f,z-0.25f)) as GameObject;
						sW = gm.WallFactory.CreateShadowBlocker(new Vector3(x,5f,z)) as GameObject;
						sW.transform.localScale = new Vector3(0.1f,10f,1f);
						sW.transform.rotation = Quaternion.Euler(0,0,0);
					}
					d.transform.rotation = tileCode.rotation;
					GameProp door = (d.GetComponent<GameProp>() as GameProp);
					door.GameManager = gm;
					door.ActionsManager
						.AddGameAction(
							"PhysicalHit",
							new GameActionMeleeOpenDoor(d)
					);
					gameobjects.Add(d);
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					walls.Add(sW);
					break;
				default:
					GameObject MakeEnemy = gm.UnitFactory.CreateUnit(tileCode.tag,new Vector3(x,-0.25f,z)) as GameObject;
					enemies.Add(MakeEnemy);
					Enemy ghost = (MakeEnemy.GetComponent<Enemy>() as Enemy);
					(ghost.GetComponent<Enemy>() as Enemy).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("StoneFloor",new Vector3(x,-0.5f,z)));
					break;
				}
			}
		}
	}

	public ArrayList GetEnemies(){
		return this.enemies;
	}
	
	public void RemoveEnemy(Enemy e){
		this.enemies.Remove(e.gameObject);
		GameObject.Destroy(e.gameObject);
	}

}
