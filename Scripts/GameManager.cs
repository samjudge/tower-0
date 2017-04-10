using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public FloorFactory FloorFactory;
	public WallFactory WallFactory;
	public GUIImageFactory GuiFactory;
	public UnitFactory UnitFactory;
	public GamePropFactory GamePropFactory;
	public Canvas Canvas;
	public CanvasGroup CanvasStatuses;
	public Canvas Overlay;
	
	public ImageStatusManager ImageStatusManager;
	public ImageSkillBarManager ImageSkillBarManager;
	public Image[] SkillBarPlaceholders;
	public Image SkillBarImage;
	public GameObject PlayerPrefab;
	public GameObject SkillSelector;
	public GameObject Player;

	//0 = wall
	//1 = floor
	//2 = player + floor
	ArrayList map;

	int mapWidth = 25;
	int mapHeight = 25;

	ArrayList walls {get;set;}
	ArrayList floors {get;set;}
	ArrayList enemies {get;set;}
	ArrayList gameobjects {get;set;}

	private IEnumerator MapLoadCallback(){
		DungeonGenerator dg = new DungeonGenerator();
		while(DungeonGenerator.hasLoaded == false){
			yield return null;
		}
		this.map = dg.m.tiles;
		for(int z = 0; z < mapHeight ; z++){
			for(int x = 0; x < mapWidth; x++){
				DungeonGenerator.Tile tileCode = map[x + z*mapWidth] as DungeonGenerator.Tile;
				GameObject o;
				switch(tileCode.tag){
					case "Blackwall":
						walls.Add(WallFactory.CreateWall("Blackwall",new Vector3(x,0,z)));
						walls.Add(WallFactory.CreateShadowBlocker(new Vector3(x,0f,z)) as GameObject);
						break;
					case "Stonefloor":
						floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
						break;
					case "Player":
						Debug.Log("Player!!");
						Player = Instantiate(PlayerPrefab,new Vector3(x,-0.25f,z),Quaternion.Euler(15,180,0)) as GameObject;
						(Player.GetComponent<Player>() as Player).GameManager = this;
						floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
						break;
					case "Dummy":
						GameObject e = UnitFactory.CreateUnit("Dummy",new Vector3(x,-0.25f,z)) as GameObject;
						enemies.Add(e);
						Enemy enemy = (e.GetComponent<Enemy>() as Enemy);
						(enemy.GetComponent<Enemy>() as Enemy).GameManager = this;
						floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
						break;
					case "Door":
						GameObject d = null;
						GameObject sW = null;
						if(tileCode.rotation.eulerAngles == new Vector3(0,90,0)){
							d = GamePropFactory.CreateProp("Door",new Vector3(x-0.3f,0.25f,z+0.25f)) as GameObject;
							sW = WallFactory.CreateShadowBlocker(new Vector3(x,5f,z)) as GameObject;
							sW.transform.localScale = new Vector3(0.1f,10f,1f);
							sW.transform.rotation = Quaternion.Euler(0,90,0);
						} else if(tileCode.rotation.eulerAngles == new Vector3(0,0,0)){
							d = GamePropFactory.CreateProp("Door",new Vector3(x-0.3f,0.25f,z-0.25f)) as GameObject;
							sW = WallFactory.CreateShadowBlocker(new Vector3(x,5f,z)) as GameObject;
							sW.transform.localScale = new Vector3(0.1f,10f,1f);
							sW.transform.rotation = Quaternion.Euler(0,0,0);
						}
						d.transform.rotation = tileCode.rotation;
						gameobjects.Add(d);
						GameProp door = (d.GetComponent<GameProp>() as GameProp);
						(door.GetComponent<GameProp>() as GameProp).GameManager = this;
						floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
						walls.Add(sW);
						break;
				}
			}
		}
	}

	void Start () {
		Debug.Log("Hi!");
		instance = this;
		this.StartCoroutine(MapLoadCallback());
		walls = new ArrayList();
		floors = new ArrayList();
		enemies = new ArrayList();
		gameobjects = new ArrayList();
		Canvas.overrideSorting = true;
	}

	public ArrayList GetEnemies(){
		return this.enemies;
	}

	public void RemoveEnemy(Enemy e){
		this.enemies.Remove(e.gameObject);
		Destroy(e.gameObject);
	}

	void Update () {
		if(ImageStatusManager == null && Player != null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageStatusManager = new ImageStatusManager(
				p.StatusManager,
				GuiFactory
			);
		}
		if(ImageSkillBarManager == null && Player != null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageSkillBarManager = new ImageSkillBarManager(
				p.SkillManager,
				GuiFactory,
				SkillBarPlaceholders
			);
			(this.SkillSelector.GetComponent<RectTransform>() as RectTransform).localPosition = this.SkillBarPlaceholders[0].rectTransform.localPosition;
			p.SetCurrentSkillToIndex(0);

		}
	}
}
