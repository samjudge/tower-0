//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

public abstract class EquipmentEffect {

	public delegate void EquipmentModifier(Unit u);

	public abstract EquipmentEffect.EquipmentModifier GetOnEquipEffect();
	public abstract EquipmentEffect.EquipmentModifier GetOnUnequipEffect();
}
