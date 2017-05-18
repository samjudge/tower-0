
using System;

public class EquipableItem : Item{

	public String[] EquipableTo;
	public EquipmentEffect[] EquipmentEffects;

	public EquipableItem (String name, String[] EquipableTo, EquipmentEffect[] EquipmentEffects) : base(name){
		this.EquipableTo = EquipableTo;
		this.EquipmentEffects = EquipmentEffects;
	}


}
