using System;

public abstract class ConsumableEffect {
	
	public delegate void ConsumeAction(Unit u);
	
	public abstract ConsumableEffect.ConsumeAction GetOnConsumeEffect();
}

