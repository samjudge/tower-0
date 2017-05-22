using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour {

	private Player Owner;

	public Text StrengthStat;
	public Text IntelligenceStat;
	public Text DexterityStat;
	public Text LuckStat;
	public Text FreePoints;

	public bool IsRunning = false;

	public void SetOwner(Player Owner){
		this.Owner = Owner;
		this.IsRunning = true;
	}

	public Text GetStatLabel(string Name){
		switch(Name){
			case "Strength":
				return StrengthStat;
			case "Intelligence":
				return IntelligenceStat;
			case "Dexterity":
				return DexterityStat;
			case "Luck":
				return LuckStat;
			default: 
				return null;
		}
	}

	private void UpdateStatLabels(){
		if(this.Owner != null){
			this.StrengthStat.text = this.Owner.BaseStrength.ToString();
			this.IntelligenceStat.text = this.Owner.BaseIntelligence.ToString();
			this.DexterityStat.text = this.Owner.BaseDexterity.ToString();
			this.LuckStat.text = this.Owner.BaseLuck.ToString();
			this.FreePoints.text = this.Owner.FreeSLIDPoints.ToString();
		}
	}

	private IEnumerator Draw(){
		while(IsCurrentlyAnimating){
			yield return null;
		}
		while(IsCurrentlyOpen){
			Debug.Log("isopen");
			UpdateStatLabels();
			yield return null;
		}
	}

	public void Open(){
		if(CanOpen()){
			Animator animator = this.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Opening");
			IsCurrentlyAnimating = true;
			this.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(true));
			this.Owner.StartCoroutine(Draw());
		}
	}

	public void Close(){
		if(CanClose()){
			Animator animator = this.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Closing");
			IsCurrentlyAnimating = true;
			this.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(false));
		}
	}

	public void Toggle(){
		if(IsCurrentlyAnimating == false){
			if(this.IsCurrentlyOpen){
				if(CanClose()){
					Close();
				}
			} else {
				if(CanOpen()){
					Open();
				}
			}
		}
	}

	public bool IsOpen(){
		if(this.IsCurrentlyAnimating == false &&
		   this.IsCurrentlyOpen == true){
			return true;
		}
		return false;
	}

	public bool IsClosed(){
		if(this.IsCurrentlyAnimating == false &&
		   this.IsCurrentlyOpen == false){
			return true;
		}
		return false;
	}

	private bool CanClose(){
		if(IsCurrentlyOpen == true &&
		   IsCurrentlyAnimating == false){
			return true;
		} else {
			return false;
		}
	}

	private bool CanOpen(){
		if(IsCurrentlyOpen == false && IsCurrentlyAnimating == false){
			return true;
		} else {
			return false;
		}
	}

	private bool IsCurrentlyClosed = false;
	private bool IsCurrentlyOpen = false;
	private bool IsCurrentlyAnimating = false;

	private IEnumerator SetIsOpenStatusOnAnimationEnd(bool VisiblityState){
		Animator a = this.GetComponent<Animator>() as Animator;
		while(a.GetCurrentAnimatorClipInfo(0).Length < 1){
			yield return null;
		}
		AnimatorClipInfo clip = a.GetCurrentAnimatorClipInfo(0)[0];
		float current_time = 0;
		float length = clip.clip.length;
		while(current_time < length || a.IsInTransition(0)){
			current_time += Time.deltaTime;
			yield return null;
		}
		this.IsCurrentlyOpen = VisiblityState;
		IsCurrentlyAnimating = false;
	}

}

