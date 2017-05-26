public class ConsumeHPModifier  : ConsumableEffect{

	public float HpMod = 0;

	public override ItemEffect.Modifier GetOnConsumeEffect(){
		ItemEffect.Modifier effect = delegate(Unit u){
			u.Hp += HpMod;
		};

		return effect;
	}
}

