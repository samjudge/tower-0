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
		Image img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
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
		for(int x = 0; x < Inventory.Slots.Count ; x++){
			ItemMapKey imk = new ItemMapKey();
			imk.index = x;
			imk.i = Inventory.Slots[x] as Item;
			Image i = InitalizeItemImage(imk.i,imk.index);
			items.Add(imk,i);
		}
		while(IsOpen){
			//update held item position
//			Debug.Log (Input.mousePosition);
//			Debug.Log (Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if(this.HeldItemImage != null){
//				Vector3 mousePosition = Input.mousePosition;
//				mousePosition.z = 100;
//				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
//				this.HeldItemImage.rectTransform.localPosition = new Vector3(mousePosition.x,mousePosition.z,mousePosition.y);
			}
			for(int x = 0; x < Inventory.Slots.Count; x++){
				Item item = Inventory.Slots[x] as Item;
				foreach(ItemMapKey imk in items.Keys){
					if(imk.index == x){
						if(imk.i == item){
							//no problem
						} else {
							//item in slot has changed
							imk.i = item;
							MonoBehaviour.Destroy(items[imk]);
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
			MonoBehaviour.Destroy(items[imk].gameObject);
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
				if(x <= this.Inventory.Slots.Count){
					if(this.Inventory.Slots[x] != null){
						return this.Inventory.Slots[x] as Item;
					}
				}
				break;
			}
		}
		return null;
	}

	private Item HeldItem = null;
	private Image HeldItemImage = null;

	public void SetHeldItem(Item i){
		for(int x = Inventory.Slots.Count-1 ; x >= 0 ; x--){
			Item thatItem = Inventory.Slots[x] as Item;
			if(thatItem == i){
				Inventory.Slots[x] = null;
				this.HeldItem = i;
				Image img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
				(img.GetComponent<Animator>() as Animator).enabled = false;
				img.transform.SetParent(this.Inventory.Owner.GameManager.Canvas.GetComponent<RectTransform>(), false);
				this.HeldItemImage = img;
			}
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

