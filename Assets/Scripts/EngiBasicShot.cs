using UnityEngine;
using System.Collections;

public class EngiBasicShot : PlayerAbility {
	
	public int Damage;

	public override void UseAbility (Enemy target, int modify)
	{

		target.TakeDamage(Damage + Random.Range(0, modify));
		Instantiate (Particles, target.transform.position, target.transform.rotation);
	
	}
	
}
