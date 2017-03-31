using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SightPassive : PassiveSkill {
	
	public String Name = "Sight";
	public Dictionary<GameObject,Image> EnemyHealthBars;
	public Dictionary<GameObject,Image> EnemyHealthBarsFill;
	public ArrayList EnemiesPrevious;

	private void IntalizeHPBar(GameObject e){
		Image red = Caster.GameManager.GuiFactory.CreateImage("Red",new Vector3(e.transform.position.x,e.transform.position.y+1f,e.transform.position.z+0.5f));
		red.GetComponent<RectTransform>().SetParent(Caster.GameManager.Overlay.transform);
		red.GetComponent<RectTransform>().rotation = Quaternion.Euler(75,0,0);
		EnemyHealthBars.Add(e,red);
		Image green = Caster.GameManager.GuiFactory.CreateImage("Green",new Vector3(e.transform.position.x,e.transform.position.y+1f,e.transform.position.z+0.5f));
		green.GetComponent<RectTransform>().rotation = Quaternion.Euler(75,0,0);
		green.GetComponent<RectTransform>().SetParent(Caster.GameManager.Overlay.transform);
		EnemyHealthBarsFill.Add(e,green);
		EnemiesPrevious.Add(e);
	}
	
	public SightPassive (Player Caster) : base(Caster) {
		EnemyHealthBars = new Dictionary<GameObject,Image>();
		EnemyHealthBarsFill = new Dictionary<GameObject,Image>();
		EnemiesPrevious = new ArrayList();
		this.action = delegate(){
			PersistantStatus Sight = new PersistantStatus("Sight", Caster);
			Sight.StartEffect = delegate(){
				ArrayList Enemies = Caster.GameManager.GetEnemies();
				foreach(GameObject e in Enemies){
					IntalizeHPBar(e);
				}
			};
			Sight.TickEffect = delegate(){
				ArrayList Enemies = Caster.GameManager.GetEnemies();
				foreach(GameObject e in Enemies){
					if(EnemiesPrevious.Contains(e)){
						Enemy enemy = e.GetComponent<Enemy>() as Enemy;
						float hpBarPercent = enemy.Hp/enemy.MaxHp;
						Image red = EnemyHealthBars[e];
						Image green = EnemyHealthBarsFill[e];
						red.GetComponent<RectTransform>().position = new Vector3(e.transform.position.x,e.transform.position.y+1f,e.transform.position.z+0.5f);
						red.GetComponent<RectTransform>().sizeDelta = new Vector3(1f,0.1f,0f);
						green.GetComponent<RectTransform>().position = new Vector3(e.transform.position.x,e.transform.position.y+1f,e.transform.position.z+0.5f);
						green.GetComponent<RectTransform>().sizeDelta = new Vector3(1f*hpBarPercent,0.1f,0f);
					} else {
						IntalizeHPBar(e);
					}
				}
				GameObject[] EnemiesPreviousClone = new GameObject[EnemiesPrevious.Count];
				EnemiesPrevious.CopyTo(EnemiesPreviousClone);
				foreach(GameObject ex in EnemiesPreviousClone){
					if(!Enemies.Contains(ex)){
						Image i = null;
						EnemyHealthBars.TryGetValue(ex, out i);
						MonoBehaviour.Destroy(i);
						EnemyHealthBars.Remove(ex);
						EnemyHealthBarsFill.TryGetValue(ex, out i);
						MonoBehaviour.Destroy(i);
						EnemyHealthBarsFill.Remove(ex);
						EnemiesPrevious.Remove(ex);
					}
				}
			};
			Caster.StatusManager.AddStatus(Sight);
		};
		this.action();
	}

}

