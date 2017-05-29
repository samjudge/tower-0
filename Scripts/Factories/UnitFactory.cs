using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UnitFactory : MonoBehaviour{
	
	public GameObject DummyEnemy;
	public GameObject ChickenEnemy;
	public GameObject SkeletonEnemy;

	System.Random RandomGen = new System.Random();

	public GameObject CreateUnit(string name, Vector3 position){
		FieldInfo property = this.GetType().GetField(name + "Enemy");
		GameObject ePrefab = property.GetValue(this) as GameObject;
		GameObject nEnemey = Instantiate(ePrefab, position, Quaternion.Euler(15,180,0)) as GameObject;
		Enemy nEnemeyScript = nEnemey.GetComponent<Enemy>() as Enemy;
		String DeathActionName = nEnemeyScript.DeathActionName;
		switch(DeathActionName){
			case "BasicEnemyDeathAction":
				nEnemeyScript.DeathAction = delegate(){
					Drop[] Drops = nEnemeyScript.Drops;
					Player p = nEnemeyScript.GameManager.Player.GetComponent<Player>() as Player;
					p.Experience += nEnemeyScript.Experience;
					if(p.CanLevelUp()){
						p.Experience -= p.CalculateRequiredExpForLevelUp();
						p.level += 1;
						p.LevelUp();
					}
					foreach(Drop DropInfo in Drops){
						double roll = RandomGen.NextDouble();
						if(roll < DropInfo.chance){
							GameObject Drop = nEnemeyScript
								.GameManager
								.ItemFactory
								.CreateItem(
									DropInfo.name,
									nEnemeyScript.transform.position
							);
							nEnemeyScript.GameManager.level.items.Add(Drop);
						}
					}
					nEnemeyScript.GameManager.level.RemoveEnemy(nEnemeyScript);
				};
				break;
		default:
			throw new Exception("DeathAction does not exist : " + DeathActionName);
			break;
		}
		return nEnemey;
	}
}

