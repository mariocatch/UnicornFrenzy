using UnityEngine;
using System.Collections;

public class EnemyTurret : Enemy
{

		public ParticleSystem BloodParticles;
		private bool mAttacking;
		public GameObject Turret;
		public LayerMask layerMask;

		public override void Start ()
		{
				base.Start ();
		}

		public override void StartTurn ()
		{
				base.StartTurn ();
		
				if (Target != null) {
			
						if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
								TurnTime = Time.time + 2;
								mAttacking = true;
						} else {
							
								for (int i = 0; i < mGameController.Players.Count; i++) {
					
					
					
										if (Vector3.Distance (transform.position, mGameController.Players [i].transform.position) <= AggroRange) {
						
												Target = mGameController.Players [i];
												if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
														TurnTime = Time.time + 3;
														mAttacking = true;
														break;
												}
										}
					
								}
						}
			
				} else {
			
						for (int i = 0; i < mGameController.Players.Count; i++) {
				
				
				
								if (Vector3.Distance (transform.position, mGameController.Players [i].transform.position) <= AggroRange) {
					
										Target = mGameController.Players [i];
										if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
												TurnTime = Time.time + 3;
												mAttacking = true;
												break;
										}
								}
				
						}
			
			
				}
				if(!mAttacking) {
						EndTurn ();
				}
		}

		public override void Death ()
		{
				base.Death ();
				Destroy (gameObject);
		}

		public void Update ()
		{

				if (mAttacking) {

						  Quaternion rotate = Quaternion.LookRotation(Target.transform.position - Turret.transform.position);
						 Turret.transform.rotation = Quaternion.Slerp (Turret.transform.rotation, rotate, Time.deltaTime * 5f);
						Target.gameObject.layer = 10;
						RaycastHit hit;
						Debug.DrawRay (Turret.transform.position, Turret.transform.forward, Color.blue);
						if (Physics.Raycast (Turret.transform.position, Turret.transform.forward, out hit, Mathf.Infinity, layerMask.value) ) {

							if (hit.collider.gameObject.GetComponent<Player>() == Target){
								print ("Hit the target!");
								BasicAttack (Target);
								Target.gameObject.layer = 0;
								mAttacking = false;
								}
						}
				}

			if (!mAttacking) {

			EndTurn ();

				}

		}

		public void BasicAttack (Player target)
		{

				if (Random.Range (0, 5) < (HitChance - HitReducer)) {
						int DamageDealt = (BasicAttackDamage + Random.Range (0, DamageBonus)) - DamageReducer;
						target.Health -= DamageDealt;
						Instantiate (BloodParticles, target.transform.position, target.transform.rotation);
						print ("Attacked for " + DamageDealt);
						EndTurn ();
				}
		}
		public override void EndTurn ()
	{
		if (Time.time > TurnTime) {
						mAttacking = false;
						base.EndTurn ();
				}
	}
}
