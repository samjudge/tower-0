using System;

public abstract class EquipmentEffect {

	public delegate void EquipmentModifier(Unit u);

	public abstract EquipmentEffect.EquipmentModifier GetOnEquipEffect();
	public abstract EquipmentEffect.EquipmentModifier GetOnUnequipEffect();
}

