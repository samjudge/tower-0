using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ImageSkillBarPlaceholder : MonoBehaviour, IPointerClickHandler {

	public int SlotIndex;

	public void OnPointerClick(PointerEventData e){
		GameObject UISkillBar = this.gameObject.transform.parent.gameObject;
		ImageSkillBarManager SkillBar = UISkillBar.GetComponent<ImageSkillBarManager>();
		SkillManager skills = SkillBar.GetSkillManager();
		GameManager gm = skills.Owner.GameManager;
		gm.Player.GetComponent<Player>().SetCurrentSkillToIndex(SlotIndex);
	}
}