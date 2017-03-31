using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public FloorFactory FloorFactory;
	public WallFactory WallFactory;
	public GUIImageFactory GuiFactory;
	public EnemyFactory EnemyFactory;
	public Canvas Canvas;
	public CanvasGroup CanvasStatuses;
	public Canvas Overlay;
	
	public ImageStatusManager ImageStatusManager;
	public ImageSkillBarManager ImageSkillBarManager;
	public Image[] SkillBarPlaceholders;
	public Image SkillBarImage;
	public GameObject PlayerPrefab;
	public GameObject SkillSelector;
	GameObject Player;

	//0 = wall
	//1 = floor
	//2 = player + floor
	int[] map = {
		0,0,0,0,0,0,0,0,0,0,
		0,1,1,1,1,1,0,1,0,0,
		0,1,2,1,1,3,0,1,0,0,
		0,1,1,3,1,1,1,1,3,0,
		0,1,1,1,1,1,0,1,0,0,
		0,1,1,1,1,1,0,1,0,0,
		0,0,1,0,1,0,1,1,0,0,
		0,1,1,3,3,1,1,1,0,0,
		0,0,0,1,0,0,0,0,1,0,
		0,0,0,0,0,0,0,0,0,1,
	};

	int mapWidth = 10;
	int mapHeight = 10;

	ArrayList walls {get;set;}
	ArrayList floors {get;set;}
	ArrayList enemies {get;set;}

	void Start () {
		Debug.Log("Hi!");
		Canvas.overrideSorting = true;

		walls = new ArrayList();
		floors = new ArrayList();
		enemies = new ArrayList();
		for(int z = 0; z < mapHeight ; z++){
			for(int x = 0; x < mapWidth; x++){
				int tileCode = map[x + z*mapWidth];
				GameObject o;
				switch(tileCode){
				case 0:
					walls.Add(WallFactory.CreateWall("Blackwall",new Vector3(x,0,z)));
					break;
				case 1:
					floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case 2:
					Player = Instantiate(PlayerPrefab,new Vector3(x,0,z),Quaternion.Euler(15,180,0)) as GameObject;
					(Player.GetComponent<Player>() as Player).GameManager = this;
					(Player.GetComponent<Player>() as Player).StatusManager = new StatusManager();
					(Player.GetComponent<Player>() as Player).ActionsManager = new ActionsManager();
					(Player.GetComponent<Player>() as Player).SkillManager = new SkillManager(Player.GetComponent<Player>() as Unit);
					floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
					break;
				case 3:
					GameObject e = EnemyFactory.CreateEnemy("Dummy",new Vector3(x,0,z)) as GameObject;
					enemies.Add(e);
					Enemy enemy = (e.GetComponent<Enemy>() as Enemy);
					(enemy.GetComponent<Enemy>() as Enemy).GameManager = this;
					floors.Add(FloorFactory.CreateFloor("Stonefloor",new Vector3(x,-0.5f,z)));
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
		Destroy(e.gameObject);
	}

	void Update () {
		if(ImageStatusManager == null){
			Player p = Player.GetComponent<Player>() as Player;
			ImageStatusManager = new ImageStatusManager(
				p.StatusManager,
				GuiFactory
			);
		}
		if(ImageSkillBarManager == null){
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
