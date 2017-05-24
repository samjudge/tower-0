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

	public TextAsset Config;

	public GameObject CreateUnit(string name, Vector3 position){
		Debug.Log (name + "Enemy");
		FieldInfo property = this.GetType().GetField(name + "Enemy");
		Debug.Log (property);
		GameObject ePrefab = property.GetValue(this) as GameObject;
		GameObject nEnemey = Instantiate(ePrefab, position, Quaternion.Euler(15,180,0)) as GameObject;
		XDocument Data = XDocument.Parse(Config.text);
		IEnumerable<XElement> Units = from unit in Data.Descendants("unit") 
			where unit.Descendants("name").ElementAt(0).Value as string == name
			select unit;

		XElement Unit = Units.ElementAt(0);

		String xmlUnitName = Unit.Descendants("name").ElementAt(0).Value as String;
		String xmlUnitSubtype = Unit.Descendants("subtype").ElementAt(0).Value as String;

		Type EnemyScriptType = Type.GetType(xmlUnitSubtype);

		Enemy nEnemeyScript = nEnemey.GetComponent(EnemyScriptType) as Enemy;
		IEnumerable<XElement> xmlDeathAction = Unit.Descendants("deathAction");
		if(xmlDeathAction.Count() != 0){
			String xmlDeathActionName = xmlDeathAction.Descendants("action").ElementAt(0).Value as String;
			switch(xmlDeathActionName){
				case "BasicEnemyDeathAction":
					nEnemeyScript.DeathAction = delegate(){
					int xmlDeathActionExp = Int32.Parse(xmlDeathAction.Descendants("experience").ElementAt(0).Value as String);
						IEnumerable<XElement> xmlDrops = xmlDeathAction.Descendants("drops");
						Player p = nEnemeyScript.GameManager.Player.GetComponent<Player>() as Player;
						p.Experience += xmlDeathActionExp;
						if(p.CanLevelUp()){
							p.Experience -= p.CalculateRequiredExpForLevelUp();
							p.level += 1;
							p.LevelUp();
						}
						foreach(XElement drop in xmlDrops){
							String xmlItemName = drop.Descendants("name").ElementAt(0).Value as String;
							String xmlItemChance = drop.Descendants("chance").ElementAt(0).Value as String;
							GameObject Drop = nEnemeyScript
								.GameManager
								.ItemFactory
								.CreateItem(
									xmlItemName,
									nEnemeyScript.transform.position
								);
							nEnemeyScript.GameManager.level.items.Add(Drop);
						}
						nEnemeyScript.GameManager.level.RemoveEnemy(nEnemeyScript);
					};
					break;
				default:
					break;
			}
		}
		return nEnemey;
	}
}

