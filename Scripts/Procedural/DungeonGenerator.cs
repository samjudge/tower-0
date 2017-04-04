using System;
using System.Collections;
using UnityEngine;

public class DungeonGenerator {

	public static bool hasLoaded = false;

	public class Tile {
		public Tile(float x, float y, float z){
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public float x = 0;
		public float y = 0;
		public float z = 0;
		public String tag = "Default";
	}

	public class Digger {

		public float planar_accelerationX = 0;
		public float planar_accelerationY = 1;
		
		public float bias = 0.5f; //where 1 is always follow planar acceleration, and 0 is always mutate acceleration value
		
		ArrayList history;
		ArrayList open;
		Map m;

		public Digger(Map m){
			this.history = new ArrayList();
			this.open = new ArrayList();
			this.m = m;
			this.activePosition = new Position(10,10);
			GameManager.instance.StartCoroutine(Dig());
		}

		private int indexOfNode(float x,float y){
			for(int t = 0 ; t < this.open.Count; t++){
				Position p = this.open[t] as Position;
				if(p.x == x && p.y == y){
					return t;
				}
			}
			return -1;
		}

		private int indexOfCarved(float x, float y){
			for(int t = 0 ; t < this.history.Count; t++){
				Position p = this.history[t] as Position;
				if(p.x == x && p.y == y){
					return t;
				}
			}
			return -1;
		}


		public class Position {
			public float x = 0;
			public float y = 0;

			public Position(){}

			public Position(float x, float y){
				this.x = x;
				this.y = y;
			}

			public Position Clone(){
				Position p = new Position();
				p.x = x;
				p.y = y;
				p.parent = parent;
				return p;
			}

			public Position parent = null;
		}

		public Position activePosition;

		public IEnumerator Dig(){
			foreach(Tile t in m.tiles){
				if(t.x > 0 && t.x < m.width && t.z > 0 && t.z < m.height){
					this.open.Add (new Position(t.x,t.z));
				}
			}
			while(open.Count > 32){
				Debug.Log(open.Count);
				Debug.Log(m.LogTiles());
				System.Random r = new System.Random();
				float roll = (float)(r.Next(0,100))/100f;
				if(roll >= 0.5){
					if(planar_accelerationX != 0){
						float ro = r.Next(0,1);
						this.planar_accelerationX = 0;
						if(ro == 0){
							this.planar_accelerationY = 1;
						} else {
							this.planar_accelerationY = -1;
						}
					} else if(planar_accelerationY != 0){
						float ro = r.Next(0,1);
						this.planar_accelerationY = 0;
						if(ro == 0){
							this.planar_accelerationX = 1;
						} else {
							this.planar_accelerationX = -1;
						}
					}
				}
				float x = this.activePosition.x+planar_accelerationX;
				float y = this.activePosition.y+planar_accelerationY;
				Debug.Log("Resolving");

				if(!canCarve(x,y)){
					//node does not exist in open, so use another open tile
					Debug.Log("Other");
					while(activePosition.parent != null){
						Debug.Log("finding open...");
						bool carved = false;
						for(int aX = -1 ; aX <= 1; aX++){
							for(int aY = -1 ; aY <= 1; aY++){
								if(aX == 0 || aY == 0 && !(aX == 0 && aY == 0)){
									if(canCarve(activePosition.x+aX,activePosition.y+aY)){
										Debug.Log("found");
										carve(activePosition.x+aX,activePosition.y+aY);
										carved = true;
									} else {
										if(indexOfNode(activePosition.x+aX,activePosition.y+aY) != -1){
											this.open.RemoveAt(indexOfNode(activePosition.x+aX,activePosition.y+aY));
										}
									}
								}
								if (carved) break;
							}
							if (carved) break;
						}
						if (carved) break;
						this.activePosition = this.activePosition.parent;
					}
				} else if(indexOfNode(x,y) != -1){
					//preferred
					Debug.Log("Carving Preferred");
					carve(x,y);
				} else {
					Position openPosition = this.open[indexOfNode(activePosition.x,activePosition.y)] as Position;
					this.open.Remove(openPosition);
				}
				//Position op = this.open[indexOfNode(activePosition.x,activePosition.y)] as Position;
				//this.open.Remove(op);
				yield return null;
			}
			hasLoaded = true;
			Tile tile = m.GetTile(10,10);
			tile.tag = "Player";
		}

		private bool canCarve(float x, float y){
			indexOfCarved(x,y);
			if(indexOfNode(x,y) == -1){
				return false;
			}
			if (y >= 1 && y < m.height-1 && x >= 1 && x < m.width-1){
				int neighborCount = 0;
				if(indexOfCarved(x-1,y) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x-1,y-1) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x-1,y+1) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x,y-1) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x,y+1) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x+1,y) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x+1,y-1) != -1){
					neighborCount++;
				}
				if(indexOfCarved(x+1,y+1) != -1){
					neighborCount++;
				}
				Debug.Log(neighborCount);
				if(neighborCount <= 3){
					return true;
				}
			}
			return false;
		}

		private void carve(float x, float y){
			Position openPosition = this.open[indexOfNode(x,y)] as Position;
			Tile t = m.GetTile((int)x,(int)y);
			if(x == 10 && y == 10){
				t.tag = "Player";
			} else {
				carveTile(t);
			}
			this.history.Add(openPosition);
			this.open.Remove(openPosition);
			openPosition.parent = this.activePosition;
			this.activePosition = openPosition;
		}
		
		private void carveTile(Tile t){
			t.tag = "Stonefloor";
		}
	}

	public class Map {
		public int width = 25;
		public int height = 25;

		public ArrayList tiles;

		public Map(){
			this.tiles = new ArrayList();
			for(int x = 0; x < height ; x++){
				for(int y = 0; y < width ; y++){
					Tile t = new Tile(x,0,y);
					t.tag = "Blackwall";
					this.tiles.Add(t);
				}
			}
		}

		public Tile GetTile(int x, int y){
			y = y * width;
			return this.tiles[(x + y)] as Tile;
		}

		public void SetTile(int x, int y, String tag){
			y = y * width;
			Tile t = new Tile(x,0,y);
			t.tag = tag;
			this.tiles.Insert((x + y),t);
		}

		public String LogTiles(){
			String s = "";
			for(int i = 0 ; i < tiles.Count; i++){
				Tile t = tiles[i] as Tile;
				if(i % width == 0){
					s += "\n";
				}
				switch(t.tag){
				case "Blackwall":
					s += "/";
					break;
				case "Stonefloor" :
					s += "\\";
					break;
				default :
					s += "#";
					break;
				}
			}
			return s;
		}

	}

	public Map m;

	public DungeonGenerator () {
		this.m = new Map();
		Digger d = new Digger(this.m);
	}
}
