using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NamedImage {
	public String name;
	public Image i;
}

public class ImageInventoryManager : MonoBehaviour
{
	private Inventory Inventory;

	public GUIImageFactory GuiFactory;
	public Image[] Placeholders;
	public NamedImage[] EquipmentPlaceholders;
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
		if(img.GetComponent<Animator>() != null){
			(img.GetComponent<Animator>() as Animator).enabled = false;
		}
		img.transform.SetParent(placeholder.rectTransform, false);
		img.GetComponent<RectTransform>().position = placeholder.rectTransform.position;
		return img;
	}

	
	private Image InitalizeItemImage(Item i, String NamedSlot){
		Image img = null;
		if(i != null){
			img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
		} else {
			img = GuiFactory.CreateImage("Placeholder",new Vector3(0f,0f,-6f));
		}
		NamedImage target = null;
		foreach(NamedImage namedimage in EquipmentPlaceholders){
			if(NamedSlot == namedimage.name){
				target = namedimage;
			}
		}
		Image placeholder = target.i;
		if(img.GetComponent<Animator>() != null){
			(img.GetComponent<Animator>() as Animator).enabled = false;
		}
		img.transform.SetParent(placeholder.rectTransform, false);
		img.GetComponent<RectTransform>().position = placeholder.transform.position;
		return img;
	}


	
	public class ItemMapKey {
		public Item i;
		public int index;
		public String namedSlot = null;
		public bool isInNamedSlot = false;
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
			foreach(Image i in items.Values){
				if(i != null){
					MonoBehaviour.Destroy(i.gameObject);
				}
			}
			for(int x = 0; x < Inventory.GetMaxSlots() ; x++){
				ItemMapKey imk = new ItemMapKey();
				imk.index = x;
				imk.i = Inventory.GetItemAtIndex(x);
				Image i = InitalizeItemImage(imk.i,imk.index);
				items.Add(imk,i);
			}
			ItemMapKey itemMapKey = new ItemMapKey();
			itemMapKey.isInNamedSlot = true;
			itemMapKey.i = Inventory.GetItemInNamedSlot("Head");
			itemMapKey.namedSlot = "Head";
			Image equipImage = InitalizeItemImage(itemMapKey.i,"Head");
			items.Add(itemMapKey,equipImage);
			itemMapKey = new ItemMapKey();
			itemMapKey.isInNamedSlot = true;
			itemMapKey.i = Inventory.GetItemInNamedSlot("Body");
			itemMapKey.namedSlot = "Body";
			equipImage = InitalizeItemImage(itemMapKey.i,"Body");
			items.Add(itemMapKey,equipImage);
			itemMapKey = new ItemMapKey();
			itemMapKey.isInNamedSlot = true;
			itemMapKey.i = Inventory.GetItemInNamedSlot("Feet");
			itemMapKey.namedSlot = "Feet";
			equipImage = InitalizeItemImage(itemMapKey.i,"Feet");
			items.Add(itemMapKey,equipImage);
			itemMapKey = new ItemMapKey();
			itemMapKey.isInNamedSlot = true;
			itemMapKey.i = Inventory.GetItemInNamedSlot("Left");
			itemMapKey.namedSlot = "Left";
			equipImage = InitalizeItemImage(itemMapKey.i,"Left");
			items.Add(itemMapKey,equipImage);
			itemMapKey = new ItemMapKey();
			itemMapKey.isInNamedSlot = true;
			itemMapKey.i = Inventory.GetItemInNamedSlot("Right");
			itemMapKey.namedSlot = "Right";
			equipImage = InitalizeItemImage(itemMapKey.i,"Right");
			items.Add(itemMapKey,equipImage);
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

	Dictionary<ItemMapKey, Image> items = new Dictionary<ItemMapKey, Image>();

	private IEnumerator DrawInventory(){
		Animator animator = this.GetComponent<Animator>() as Animator;
		while(IsCurrentlyAnimating){
			//Debug.Log ("Still Animating");
			yield return null;
		}
		while(IsOpen){
			//update held item position
			if(this.HeldItemImage != null){
				Vector3 mousePosition = Input.mousePosition;
				this.HeldItemImage.rectTransform.position = new Vector3(mousePosition.x,mousePosition.y,mousePosition.z);
			}
			ArrayList keys = new ArrayList(items.Keys);
			for(int x = 0; x < Inventory.GetMaxSlots(); x++){
				Item item = Inventory.GetItemAtIndex(x);
				foreach(ItemMapKey imk in keys){
					if(!imk.isInNamedSlot){
						if(imk.index == x){
							if(imk.i != item){
								//item in slot has changed
								imk.i = item;
								Image img = items[imk] as Image;
								if(items[imk] != null){
									Destroy(items[imk].gameObject);
								}
								items[imk] = InitalizeItemImage(imk.i,imk.index);
							}
						}
					}
				}
			}
			//equipment slots
			String[] EquipSlotsToCheck = {"Head","Body","Feet","Left","Right"};
			foreach(ItemMapKey imk in keys){
				if(imk.isInNamedSlot){
					foreach(String NamedSlot in EquipSlotsToCheck){
						if(imk.namedSlot == NamedSlot){
							Item i = Inventory.GetItemInNamedSlot(NamedSlot);
							if(i != imk.i){
								imk.i = i;
								Image img = items[imk] as Image;
								if(items[imk] != null){
									Destroy(items[imk].gameObject);
								}
								items[imk] = InitalizeItemImage(imk.i,imk.namedSlot);
								//item has changed, redraw
							}
							break;
						}
					}
				}
			}
			yield return null;
		}
		foreach(ItemMapKey imk in items.Keys){
			if(items[imk] != null){
				Destroy(items[imk].gameObject);
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
					if(this.Inventory.GetItemAtIndex(x) != null){
						return this.Inventory.GetItemAtIndex(x);
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
	private String HeldItemLastNamedSlot = null;
	private bool LastHeldItemFromNamedSlot = false;

	public bool WasLastItemFromNamedSlot(){
		return this.LastHeldItemFromNamedSlot;
	}

	public int GetHeldItemLastOccupiedIndex(){
		return this.HeldItemLastIndex;
	}

	public String GetHeldItemLastOccupiedNamedSlot(){
		return this.HeldItemLastNamedSlot;
	}

	public Item GetHeldItem(){
		return this.HeldItem;
	}

	public void ReleaseHeldItem(){
		this.HeldItem = null;
		Destroy(this.HeldItemImage);
		this.HeldItemImage = null;
		this.HeldItemLastIndex = 0;
		this.HeldItemLastNamedSlot = null;
	}

	public void SetHeldItem(String s){
		Item i = Inventory.GetItemInNamedSlot(s);
		if(i != null){
			Inventory.EmptyItemFromNamedSlot(s);
			this.LastHeldItemFromNamedSlot = true;
			this.HeldItemLastNamedSlot = s;
			SetHeldItem(i);
		}
	}

	public void SetHeldItem(int x){
		Item i = Inventory.GetItemAtIndex(x);
		if(i != null){
			Inventory.EmptyItemAtIndex(x);
			this.LastHeldItemFromNamedSlot = false;
			this.HeldItemLastIndex = x;
			SetHeldItem(i);
		}
	}

	public void SetHeldItem(Item i){
		this.HeldItem = i;
		Image img = GuiFactory.CreateImage(i.Name,new Vector3(0f,0f,-6f));
		if(img.GetComponent<Animator>() != null){
			(img.GetComponent<Animator>() as Animator).enabled = false;
		}
		img.transform.SetParent(this.Inventory.Owner.GameManager.Canvas.GetComponent<RectTransform>(), false);
		img.rectTransform.sizeDelta = new Vector2(img.rectTransform.rect.width*3,img.rectTransform.rect.height*3);
		CanvasGroup group = img.gameObject.AddComponent<CanvasGroup>() as CanvasGroup;
		group.blocksRaycasts = false;
		this.HeldItemImage = img;
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

