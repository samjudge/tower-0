public class ConsumeHPModifier  : ConsumableEffect{

	float HpMod = 0;
	
	public ConsumeHPModifier(float HpMod){
		this.HpMod = HpMod;
	}
	
	public override ConsumableEffect.ConsumeAction GetOnConsumeEffect(){
		ConsumableEffect.ConsumeAction effect = delegate(Unit u){
			u.Hp += HpMod;
		};

		return effect;
	}
}

