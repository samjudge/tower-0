using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;
public class DungeonGenerator {

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
		public Digger(float x, float y, float z, Map m){
			this.x = x;
			this.y = y;
			this.z = z;
			this.history = new ArrayList();
			this.open = new ArrayList();
			Position p = new Position(x,z);
			this.open.Add(p);
			carveAndOpen((int)this.x,(int)this.z,m);
		}


		private void openNodesAround(Position parent, float x, float y){
			if(canOpenNode(x-1,y)) {
				Position p = new Position();
				p.x = x-1;
				p.y = y;
				p.parent = parent;
				this.open.Add(p);
			}
			if(canOpenNode(x-1,y)) {
				Position p = new Position();
				p.x = x+1;
				p.y = y;
				p.parent = parent;
				this.open.Add(p);
			}
			if(canOpenNode(x-1,y)) {
				Position p = new Position();
				p.x = x;
				p.y = y-1;
				p.parent = parent;
				this.open.Add(p);
			}
			if(canOpenNode(x-1,y)) {
				Position p = new Position();
				p.x = x;
				p.y = y+1;
				p.parent = parent;
				this.open.Add(p);
			}
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

		private bool canOpenNode(float x, float z){
			if(indexOfCarved(x-1,y) == -1
			   && indexOfCarved(x+1,y) == -1
			   && indexOfCarved(x,y-1) == -1
			   && indexOfCarved(x,y+1) == -1){
				return true;
			}
			return false;
		}

		public class Position {
			public float x = 0;
			public float y = 0;

			public Position(){}

			public Position(float x, float y){
				this.x = x;
				this.y = y;
			}

			public Position parent = null;
		}

		public float x = 0;
		public float y = 0;
		public float z = 0;

		public float planar_accelerationX = 1;
		public float planar_accelerationZ = 0;

		public float bias = 0.85f; //where 1 is always follow planar acceleration, and 0 is always mutate acceleration value

		ArrayList history;
		ArrayList open;

		public void Dig(Map m){
			while(this.open.Count > 0){
				System.Random r = new System.Random();
			  float roll = ((float)r.Next(0,100))/100f;
				if(roll > bias){
					if(planar_accelerationX != 0){
						planar_accelerationX = 0;
						int changeTo = (r.Next(0,2) - 1);
						while(changeTo == 0){
							changeTo = (r.Next(0,2) - 1);
						}
						planar_accelerationZ = changeTo;
					} else if(planar_accelerationZ != 0){
						planar_accelerationZ = 0;
						int changeTo = (r.Next(0,2) - 1);
						while(changeTo == 0){
							changeTo = (r.Next(0,2) - 1);
						}
						planar_accelerationX = changeTo;
					}
				}
				float tX = this.x + planar_accelerationX;
				float tY = this.z + planar_accelerationZ;
				if(this.canOpenNode(tX,tY)){
					carveAndOpen((int)tX,(int)tY,m);
					this.x += planar_accelerationX;
					this.z += planar_accelerationZ;
				} else {
					for(int i = this.open.Count -1 ; i > -1 ; i--){
						Position p = this.open[i] as Position;
						if(this.canOpenNode(p.x,p.y)){
							carveAndOpen((int)p.x,(int)p.y,m);
							this.x = p.x;
							this.z = p.y;
							this.planar_accelerationX = -(this.x - p.parent.x);
							this.planar_accelerationZ = -(this.z - p.parent.y);
							break;
						} else {
							this.open.RemoveAt(i);
						}
					}
				}
			}
		}

		private void carveAndOpen(int x, int y, Map m){
			Position historic = new Position(x,y);
			if(x > -1 && y > -1 && x < m.width+1 && y < m.height+1){
				int i = this.indexOfNode(x,y);
				if(i != -1){
					Tile t = m.GetTile(x,y);
					carve(t);
					this.openNodesAround(historic,historic.x,historic.y);
					this.open.RemoveAt(this.indexOfNode(x,y));
				}
			}
			this.history.Add(historic);
		}
		
		private void carve(Tile t){
			t.tag = "Stonefloor";
		}
	}

	public class Map {
		public int width = 25;
		public int height = 25;

		private ArrayList tiles;

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
					s += "#";
					break;
				case "Stonefloor" :
					s += i%10;
					break;
				default :
					s += "1";
					break;
				}
			}
			return s;
		}

	}

	public Map m;

	public DungeonGenerator () {
		this.m = new Map();
		Digger d = new Digger(10,0,10, this.m);
		d.Dig(this.m);
	}
}
