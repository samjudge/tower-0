using System;

public abstract class EquipmentEffect : ItemEffect{

	public String[] EquipableTo;

	public abstract ItemEffect.Modifier GetOnEquipEffect();
	public abstract ItemEffect.Modifier GetOnUnequipEffect();
}

