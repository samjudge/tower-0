using System;

public class EquipmentStatusModifierEffect : EquipmentEffect{

	public float StrMod;
	public float LukMod;
	public float IntMod;
	public float DexMod;

	public EquipmentStatusModifierEffect(){
		this.StrMod = StrMod;
		this.LukMod = LukMod;
		this.IntMod = IntMod;
		this.DexMod = DexMod;
	}

	public override ItemEffect.Modifier GetOnEquipEffect(){
		ItemEffect.Modifier effect = delegate(Unit u){
			u.BonusStrength += StrMod;
			u.BonusLuck += LukMod;
			u.BonusIntelligence += IntMod;
			u.BonusDexterity += DexMod;
		};
		return effect;
	}

	public override ItemEffect.Modifier GetOnUnequipEffect(){
		ItemEffect.Modifier effect = delegate(Unit u){
			u.BonusStrength -= StrMod;
			u.BonusLuck -= LukMod;
			u.BonusIntelligence -= IntMod;
			u.BonusDexterity -= DexMod;
		};
		return effect;
	}

}


