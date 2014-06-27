using UnityEngine;
using System.Collections;

public class EngiHeal : PlayerAbility {

	public override void UseAbility (Player source, Player target, int modify1, int modify2)
	{
		target.Health += Damage + Random.Range ((int)(modify1 / 2), modify1);
		source.ActionPoints -= ApCost;
		Instantiate (Particles, target.transform.position, target.transform.rotation);
	}

}
