public class ConsumeMPModifier  : ConsumableEffect{

	public float MpMod = 0;

	public override ItemEffect.Modifier GetOnConsumeEffect(){
		ItemEffect.Modifier effect = delegate(Unit u){
			u.Mp += MpMod;
		};

		return effect;
	}
}

