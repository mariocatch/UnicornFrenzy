using UnityEngine;
using System.Collections;

public class SniperChargedShot : PlayerAbility {

	private bool mCharged;
	public ParticleSystem ChargeParticles;

	public override void UseAbility (Player source, Enemy target, int modify1, int modify2)
	{
		if (mCharged) {
						target.TakeDamage (Damage + Random.Range (0, modify1));
						target.ReduceDamage (StatusChange, StatusTurns);
						Instantiate (Particles, target.transform.position, target.transform.rotation);
						mCharged = false;
						source.ActionPoints -= ApCost;
						ApCost = 2;
				} else {
			mCharged = true;
			ApCost = 0;
			Instantiate (ChargeParticles, source.transform.position, source.transform.rotation);
				}
	}
}
