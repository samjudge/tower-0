using System;
using UnityEngine;
/**
 * Item Effects are more akin to interfaces to
 * the Unity inspector, sorry for shitty
 * naming+directories 
 */
public abstract class ItemEffect : MonoBehaviour{
	public String ItemTypeName;
	public delegate void Modifier(Unit u);
}

