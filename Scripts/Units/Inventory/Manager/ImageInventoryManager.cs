using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageInventoryManager
{
	public Inventory Inventory {get;set;}
	public GUIImageFactory GuiFactory {get;set;}

	public bool IsOpen = false;
	public ArrayList Placeholders;
	public Image InventoryOverlay;

	public ImageInventoryManager(Inventory Inventory, GUIImageFactory GuiFactory, Image InventoryOverlay, Image[] InventoryImagePlaceholders){
		this.Inventory = Inventory;
		this.GuiFactory = GuiFactory;
		this.Placeholders = new ArrayList(InventoryImagePlaceholders);
		this.InventoryOverlay = InventoryOverlay;
//		Color transparent = InventoryOverlay.color;
//		transparent.a = 0;
//		InventoryOverlay.color = transparent;
//		foreach(Image c in this.Placeholders){
//			c.color = transparent;
//		}
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
			Animator animator = InventoryOverlay.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Opening");
			IsCurrentlyAnimating = true;
			Inventory.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(InventoryOverlay,true));
			Inventory.Owner.StartCoroutine(DrawInventory());
		}
	}

	public void CloseInventory(){
		if(CanCloseInventory()){
			Animator animator = InventoryOverlay.GetComponent<Animator>() as Animator;
			animator.SetTrigger("Closing");
			IsCurrentlyAnimating = true;
			Inventory.Owner.StartCoroutine(SetIsOpenStatusOnAnimationEnd(InventoryOverlay,false));
		}
	}

	private IEnumerator DrawInventory(){
		//
//		Color opaque = InventoryOverlay.color;
//		opaque.a = 1f;
//		InventoryOverlay.color = opaque;
//		foreach(Image c in this.Placeholders){
//			c.color = opaque;
//		}
		Animator animator = InventoryOverlay.GetComponent<Animator>() as Animator;
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
		Debug.Log (CastTo);
		bool HasHitSkill = false;
		for(int x = 0; x < this.Placeholders.Count ; x++){
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
	
	private bool IsCurrentlyAnimating = false;

	private IEnumerator SetIsOpenStatusOnAnimationEnd(Image i, bool VisiblityState){
		Animator a = i.GetComponent<Animator>() as Animator;
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

