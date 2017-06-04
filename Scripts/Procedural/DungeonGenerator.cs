using System;
using System.Collections;
using UnityEngine;

public class DungeonGenerator {

	private static LevelManager Owner;

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
		public Quaternion rotation;
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
			Dig();
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

		public void Dig(){
			System.Random r = new System.Random();
			foreach(Tile t in m.tiles){
				if(t.x > 0 && t.x < m.width && t.z > 0 && t.z < m.height){
					this.open.Add (new Position(t.x,t.z));
				}
			}
			while(open.Count > 128){
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

				if(!canCarve(x,y)){
					//node does not exist in open, so use another open tile
					while(activePosition.parent != null){
						bool carved = false;
						for(int aX = -1 ; aX <= 1; aX++){
							for(int aY = -1 ; aY <= 1; aY++){
								if(aX == 0 || aY == 0 && !(aX == 0 && aY == 0)){
									if(canCarve(activePosition.x+aX,activePosition.y+aY) && !carved){
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
					carve(x,y);
				}
				//Position op = this.open[indexOfNode(activePosition.x,activePosition.y)] as Position;
				//this.open.Remove(op);
			}
			for(int x = 0; x < 3; x++){
				int sX = r.Next(6,m.width-6);
				int sY = r.Next(6,m.height-6);
				int w = r.Next(3,6);
				int h = r.Next(3,6);
				for(int nx = 0; nx < w ; nx++){
					for(int ny = 0; ny < h ; ny++){
						Tile roomtile = m.GetTile(sX + nx, sY + ny);
						roomtile.tag = "StoneFloor";
					}
				}
			}
			carveExit(this.activePosition.x,this.activePosition.y);
			hasLoaded = true;
			Tile tile = m.GetTile(10,10);
			tile.tag = "Player";
		}

		public void carveExit(float x, float y){
			Tile t = m.GetTile((int)x,(int)y);
			t.tag = "StairsDown";
		}

		private bool canCarve(float x, float y){
			if(indexOfNode(x,y) == -1){
				return false;
			}
			if (y >= 1 && y < m.height-1 && x >= 1 && x < m.width-1){
				int neighborCount = 0;
				int cornerCount = 0;
				int adjacentCount = 0;
				if(indexOfCarved(x-1,y) != -1){
					neighborCount++;
					adjacentCount++;
				}
				if(indexOfCarved(x-1,y-1) != -1){
					neighborCount++;
					cornerCount++;
				}
				if(indexOfCarved(x-1,y+1) != -1){
					neighborCount++;
					cornerCount++;
				}
				if(indexOfCarved(x,y-1) != -1){
					neighborCount++;
					adjacentCount++;
				}
				if(indexOfCarved(x,y) != -1){
					neighborCount++;
					cornerCount++;
				}
				if(indexOfCarved(x,y+1) != -1){
					neighborCount++;
					adjacentCount++;
				}
				if(indexOfCarved(x+1,y) != -1){
					neighborCount++;
					adjacentCount++;
				}
				if(indexOfCarved(x+1,y-1) != -1){
					neighborCount++;
					cornerCount++;
				}
				if(indexOfCarved(x+1,y+1) != -1){
					neighborCount++;
					cornerCount++;
				}
				if(neighborCount <= 3){
					if(adjacentCount == 2 && cornerCount == 1){
						return false;
					}
					if(indexOfCarved(x-1,y-1) != -1){
						if(indexOfCarved(x,y-1) == -1 && indexOfCarved(x-1,y) == -1){
							return false;
						}
					}
					if(indexOfCarved(x+1,y-1) != -1){
						if(indexOfCarved(x,y-1) == -1 && indexOfCarved(x+1,y) == -1){
							return false;
						}
					}
					if(indexOfCarved(x-1,y+1) != -1){
						if(indexOfCarved(x,y+1) == -1 && indexOfCarved(x-1,y) == -1){
							return false;
						}
					}
					if(indexOfCarved(x+1,y+1) != -1){
						if(indexOfCarved(x,y+1) == -1 && indexOfCarved(x+1,y) == -1){
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		private void carve(float x, float y){
			Position openPosition = this.open[indexOfNode(x,y)] as Position;

			if(x == 10 && y == 10){
				Tile t = m.GetTile((int)x,(int)y);
				t.tag = "Player";
				openPosition.parent = this.activePosition;
				this.activePosition = openPosition;
			} else {
				carveTile(openPosition);
				openPosition.parent = this.activePosition;
				this.activePosition = openPosition;
			}
		}

		int step = 0;
		System.Random r = new System.Random();

		private void carveTile(Position openPosition){
			Tile t = m.GetTile((int)openPosition.x,(int)openPosition.y);
			Tile par = m.GetTile((int)this.activePosition.x,(int)this.activePosition.y) as Tile;
			if(par.tag == "Door"){
				this.history.Add(openPosition);
				this.open.Remove(openPosition);
				return;
			}
			int n = this.r.Next(0,100); //;
			if(n < 10){
				this.history.Add(openPosition);
				this.open.Remove(openPosition);
				int up = indexOfNode(this.activePosition.x,this.activePosition.y+1);
				int down = indexOfNode(this.activePosition.x,this.activePosition.y-1);
				int left = indexOfNode(this.activePosition.x-1,this.activePosition.y);
				int right = indexOfNode(this.activePosition.x+1,this.activePosition.y);
				if(up != -1 && down != -1){
					t.tag = par.tag;
					par.tag = "Door";
					par.rotation = Quaternion.Euler(new Vector3(0,0,0));
				} else if(left != -1 && right != -1){
					t.tag = par.tag;
					par.tag = "Door";
					par.rotation = Quaternion.Euler(new Vector3(0,90,0));
				} else {
					t.tag = "StoneFloor";
				}
				return;

			} else if (n < 20){
				float enemyTypeRoll = this.r.Next(0,12);
				if(enemyTypeRoll <= 2){
					if(Owner.level >= 5){
						t.tag = "GoblinAlphaShaman";
					} else if (Owner.level >= 3){
						t.tag = "GoblinSpear";
					} else {
						t.tag = "Snail";
					}
				} else if (enemyTypeRoll <= 3){
					if(Owner.level >= 4){
						t.tag = "Ghost";
					} else if (Owner.level >= 2){
						t.tag = "Skeleton";
					} else {
						t.tag = "Rat";
					}
				} else if(enemyTypeRoll <= 10){
					if(Owner.level >= 5){
						t.tag = "GoblinAlpha";
					} else if (Owner.level >= 2){
						t.tag = "GoblinSpear";
					} else {
						t.tag = "Goblin";
					}
				} else if(enemyTypeRoll <= 12){
					if(Owner.level >= 3){
						t.tag = "Rat";
					} else if (Owner.level >= 2){
						t.tag = "Chicken";
					} else {
						t.tag = "Crab";
					}
				}
			} else if (n < 24){
				t.tag = "Dummy";
			} else if(n < 2){
				float potionTypeRoll = this.r.Next(0,10);
				if(potionTypeRoll < 5){
					t.tag = "HealthPotion";
				} else {
					t.tag = "ManaPotion";
				}
			} else{
				t.tag = "StoneFloor";
			}
			this.history.Add(openPosition);
			this.open.Remove(openPosition);
			return;
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
					t.tag = "BlackWall";
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
				case "StoneFloor" :
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

	public DungeonGenerator (LevelManager Owner) {
		DungeonGenerator.Owner = Owner;
		this.m = new Map();
		Digger d = new Digger(this.m);
	}
}
