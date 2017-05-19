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

	public void GenerateDungeonLevel(){
		DungeonGenerator dg = new DungeonGenerator(this);
		this.map = dg.m.tiles;
		for(int z = 0; z < mapHeight ; z++){
			for(int x = 0; x < mapWidth; x++){
				DungeonGenerator.Tile tileCode = map[x + z*mapWidth] as DungeonGenerator.Tile;
				switch(tileCode.tag){
				case "Blackwall":
					walls.Add(gm.WallFactory.CreateWall("Blackwall",new Vector3(x,0,z)));
					walls.Add(gm.WallFactory.CreateShadowBlocker(new Vector3(x,0f,z)) as GameObject);
					break;
				case "Stonefloor":
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case "Player":
					gm.Player = GameObject.Instantiate(gm.PlayerPrefab,new Vector3(x,-0.25f,z),Quaternion.Euler(15,180,0)) as GameObject;
					(gm.Player.GetComponent<Player>() as Player).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case "Chicken":
					GameObject chickenEnemy = gm.UnitFactory.CreateUnit("Chicken",new Vector3(x,-0.25f,z)) as GameObject;
					enemies.Add(chickenEnemy);
					Enemy enemy = (chickenEnemy.GetComponent<Enemy>() as Enemy);
					(enemy.GetComponent<Enemy>() as Enemy).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case "Skeleton":
					GameObject skeletonEnemy = gm.UnitFactory.CreateUnit("Skeleton",new Vector3(x,-0.25f,z)) as GameObject;
					enemies.Add(skeletonEnemy);
					Enemy skeleton = (skeletonEnemy.GetComponent<Enemy>() as Enemy);
					(skeletonEnemy.GetComponent<Enemy>() as Enemy).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case "Dummy":
					GameObject dummyEnemy = gm.UnitFactory.CreateUnit("Dummy",new Vector3(x,-0.25f,z)) as GameObject;
					enemies.Add(dummyEnemy);
					(dummyEnemy.GetComponent<Enemy>() as Enemy).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
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
					gameobjects.Add(d);
					GameProp door = (d.GetComponent<GameProp>() as GameProp);
					(door.GetComponent<GameProp>() as GameProp).GameManager = gm;
					floors.Add(gm.FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					walls.Add(sW);
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
