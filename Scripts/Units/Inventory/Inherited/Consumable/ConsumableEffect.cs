using System;

public abstract class ConsumableEffect : ItemEffect{
	public abstract ItemEffect.Modifier GetOnConsumeEffect();
}

