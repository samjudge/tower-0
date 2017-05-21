using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitFactory : MonoBehaviour{
	
	public GameObject DummyEnemy;
	public GameObject ChickenEnemy;
	public GameObject SkeletonEnemy;

	public GameObject CreateUnit(String name, Vector3 position){
		switch(name){
			case "Dummy":
				return Instantiate(DummyEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
				break;
			case "Chicken":
				GameObject nChickenEnemey = Instantiate(ChickenEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
				ChaserEnemy nChickenEnemeyScript = nChickenEnemey.GetComponent<ChaserEnemy>() as ChaserEnemy;
				nChickenEnemeyScript.DeathAction = delegate(){
					GameObject ChickenDrop = nChickenEnemeyScript
						.GameManager
						.ItemFactory
						.CreateItem(
							"Chicken",
							nChickenEnemeyScript.transform.position
					);
					nChickenEnemeyScript.GameManager.level.items.Add(ChickenDrop);
					nChickenEnemeyScript.GameManager.level.RemoveEnemy(nChickenEnemeyScript);
				};
				return nChickenEnemey;
				break;
			case "Skeleton":
				GameObject nSkeletonEnemey = Instantiate(SkeletonEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
				ChaserEnemy nSkeletonEnemeyScript = nSkeletonEnemey.GetComponent<ChaserEnemy>() as ChaserEnemy;
				nSkeletonEnemeyScript.DeathAction = delegate(){
					System.Random r = new System.Random();
					float rNum = r.Next(0,100);
					if(rNum < 12.5f){
						GameObject SwordDrop = nSkeletonEnemeyScript
							.GameManager
								.ItemFactory
								.CreateItem(
									"Sword",
									nSkeletonEnemeyScript.transform.position
									);
						nSkeletonEnemeyScript.GameManager.level.items.Add(SwordDrop);
					}
					nSkeletonEnemeyScript.GameManager.level.RemoveEnemy(nSkeletonEnemeyScript);
				};
				return nSkeletonEnemey;
				break;
			default:
				return Instantiate(DummyEnemy, position, Quaternion.Euler(15,180,0)) as GameObject;
				break;
		}
	}
}

