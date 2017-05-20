public class ConsumeMPModifier  : ConsumableEffect{

	float MpMod = 0;
	
	public ConsumeMPModifier(float MpMod){
		this.MpMod = MpMod;
	}
	
	public override ConsumableEffect.ConsumeAction GetOnConsumeEffect(){
		ConsumableEffect.ConsumeAction effect = delegate(Unit u){
			u.Mp += MpMod;
		};

		return effect;
	}
}

