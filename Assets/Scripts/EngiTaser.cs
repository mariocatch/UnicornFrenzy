using UnityEngine;
using System.Collections;

public class EngiTaser : PlayerAbility {


	public override void UseAbility (Player source, Enemy target, int modify)
	{

		target.TakeDamage(Damage + Random.Range(0, modify));
		target.ReduceDamage (StatusChange, StatusTurns);
		Instantiate (Particles, target.transform.position, target.transform.rotation);
		source.ActionPoints += APRecov;
	
	}
	
}
