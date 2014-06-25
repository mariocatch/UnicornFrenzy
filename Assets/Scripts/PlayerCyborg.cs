using UnityEngine;
using System.Collections;

public class PlayerCyborg : Player {
	

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		Ability1 = Abilities [0];
		Ability2 = Abilities [1];

		PrimaryStat = Strength;
		SecondaryStat = Technology;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update ();
	}
	
	public override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}
}
