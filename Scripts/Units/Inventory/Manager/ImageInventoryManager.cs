using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageInventoryManager : MonoBehaviour
{
	private Inventory Inventory;

	public GUIImageFactory GuiFactory;
	public Image[] Placeholders;
	public bool IsOpen = false;
	public bool IsRunning = false;


	public Inventory GetInventory(){
		return this.Inventory;
	}

	public void SetInventoryAndInit(Inventory i){
		this.Inventory = i;
		this.IsRunning = true;
	}
	
	private Image InitalizeItemImage(Item i, int index){
		Image img = null;
		if(i != null){
			img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
		} else {
			img = GuiFactory.CreateImage("Placeholder",new Vector3(0f,0f,-6f));
		}
		Image placeholder = Placeholders[index] as Image;
		(img.GetComponent<Animator>() as Animator).enabled = false;
		img.transform.SetParent(placeholder.rectTransform, false);
		img.GetComponent<RectTransform>().position = placeholder.transform.position;
		return img;
	}
	
	public class ItemMapKey {
		public Item i;
		public int index;
	}

	private bool CanOpenInventory(){
		if(IsOpen == false && IsCurrentlyAnimating == false){
			return true;
		} else {
			return false;
		}
	}

	private bool CanCloseInventory(){
		if(IsOpen == true && IsCurrentlyAnimating == false){
			return true;
		} else {
			return false;
		}
	}

	public bool IsInventoryOpen(){
		if(this.IsCurrentlyAnimating == false &&
		   this.IsOpen == true){
			return true;
		}
		return false;
	}

	public bool IsInventoryClosed(){
		if(this.IsCurrentlyAnimating == false &&
		   this.IsOpen == false){
			return true;
		}
		return false;
	}
	
	public void ToggleInventory(){
		if(IsCurrentlyAnimating == false){
			if(this.IsOpen){
				if(CanCloseInventory()){
					CloseInventory();
				}
			} else {
				if(CanOpenInventory()){
					OpenInventory();
				}
			}
		}
	}

	public void OpenInventory(){
		if(CanOpenInventory()){
			Animator animator = this.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Opening");
			IsCurrentlyAnimating = true;
			Inventory.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(true));
			Inventory.Owner.StartCoroutine(DrawInventory());
		}
	}

	public void CloseInventory(){
		if(CanCloseInventory()){
			Animator animator = this.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Closing");
			IsCurrentlyAnimating = true;
			Inventory.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(false));
		}
	}

	private IEnumerator DrawInventory(){
		Animator animator = this.GetComponent<Animator>() as Animator;
		while(IsCurrentlyAnimating){
			//Debug.Log ("Still Animating");
			yield return null;
		}
		Dictionary<ItemMapKey, Image> items = new Dictionary<ItemMapKey, Image>();
		for(int x = 0; x < Inventory.GetMaxSlots() ; x++){
			ItemMapKey imk = new ItemMapKey();
			imk.index = x;
			imk.i = Inventory.GetItemFromInventory(x);
			Image i = InitalizeItemImage(imk.i,imk.index);
			items.Add(imk,i);
		}
		while(IsOpen){
			//update held item position
			if(this.HeldItemImage != null){
				Vector3 mousePosition = Input.mousePosition;
				//this.HeldItemImage.rectTransform.position = new Vector3(mousePosition.x,mousePosition.y,mousePosition.z);
			}
			for(int x = 0; x < Inventory.GetMaxSlots(); x++){
				Item item = Inventory.GetItemFromInventory(x);
				ArrayList keys = new ArrayList(items.Keys);
				foreach(ItemMapKey imk in keys){
					if(imk.index == x){
						if(imk.i == item){
							//no problem
						} else {
							//item in slot has changed
							imk.i = item;
							Destroy(items[imk]);
							if(imk.i != null){
								items[imk] = InitalizeItemImage(imk.i,imk.index);
							}
						}
					}
				}
			}
			yield return null;
		}
		foreach(ItemMapKey imk in items.Keys){
			if(items[imk] != null){
				Destroy(items[imk]);
			}
		}
		yield return null;
		
	}
	
	public Item GetItemRefAtVectorPos(Vector3 CastTo){
		//Debug.Log (CastTo);
		bool HasHitSkill = false;
		for(int x = 0; x < this.Placeholders.Length ; x++){
			Image Placeholder = this.Placeholders[x] as Image;
			if(RectTransformUtility.RectangleContainsScreenPoint(Placeholder.rectTransform,CastTo,Camera.main)){
				HasHitSkill = true;
				if(x <= this.Inventory.GetMaxSlots()){
					if(this.Inventory.GetItemFromInventory(x) != null){
						return this.Inventory.GetItemFromInventory(x);
					}
				}
				break;
			}
		}
		return null;
	}

	private Item HeldItem = null;
	private Image HeldItemImage = null;
	private int HeldItemLastIndex = 0;

	public int GetHeldItemLastOccupiedIndex(){
		return this.HeldItemLastIndex;
	}

	public Item GetHeldItem(){
		return this.HeldItem;
	}

	public void ReleaseHeldItem(){
		this.HeldItem = null;
		Destroy(this.HeldItemImage);
		this.HeldItemImage = null;
		this.HeldItemLastIndex = 0;
	}

	public void SetHeldItem(int x){
		Item i = Inventory.GetItemFromInventory(x);
		if(i != null){
			Inventory.SetItemInInvenotry(null,x);
			this.HeldItem = i;
			this.HeldItemLastIndex = x;
			Image img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
			(img.GetComponent<Animator>() as Animator).enabled = false;
			img.transform.SetParent(this.Inventory.Owner.GameManager.Canvas.GetComponent<RectTransform>(), false);
			img.rectTransform.sizeDelta = new Vector2(96,96);
			this.HeldItemImage = img;
		}
	}

	private bool IsCurrentlyAnimating = false;

	private IEnumerator SetIsOpenStatusOnAnimationEnd(bool VisiblityState){
		Animator a = this.GetComponent<Animator>() as Animator;
		while(a.GetCurrentAnimatorClipInfo(0).Length < 1){
			yield return null;
		}
		AnimatorStateInfo state = a.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo clip = a.GetCurrentAnimatorClipInfo(0)[0];
		float current_time = 0;
		float length = clip.clip.length;
		while(current_time < length || a.IsInTransition(0)){
			current_time += Time.deltaTime;
			yield return null;
		}
		this.IsOpen = VisiblityState;
		IsCurrentlyAnimating = false;
	}
	
}

