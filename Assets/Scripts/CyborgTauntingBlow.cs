using UnityEngine;
using System.Collections;

public class CyborgTauntingBlow : PlayerAbility {

	public override void UseAbility (Player source, Enemy target, int modify1, int modify2)
	{
		target.TakeDamage(Damage + Random.Range(0, modify1));
		target.Target = source;
		Instantiate (Particles, target.transform.position, target.transform.rotation);
		source.ActionPoints += APRecov;
	}

}
