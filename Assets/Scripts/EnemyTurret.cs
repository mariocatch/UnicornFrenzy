using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTurret : Enemy
{

		public ParticleSystem BloodParticles;
		private bool mAttacking;
		public GameObject Turret;
		public LayerMask layerMask;
		public LayerMask CheckWalls;
		public LayerMask CheckCover;
		private List<Player> Targets = new List<Player>();

		public override void Start ()
		{
				base.Start ();
		}

		public override void StartTurn ()
		{
				base.StartTurn ();
		
				if (Target != null) {
			
				if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange && !Physics.Linecast (transform.position, Target.transform.position, CheckWalls)) {
										TurnTime = Time.time + 2;
										mAttacking = true;
						} else {
								
								SelectNewTarget ();
								if (Targets.Count != 0) {
										Target = Targets [Random.Range (0, Targets.Count)];
										if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
												TurnTime = Time.time + 3;
												mAttacking = true;
										}
								}
										
					
								
						}
			
				} else {
			
						SelectNewTarget ();
						if (Targets.Count != 0) {
								Target = Targets [Random.Range (0, Targets.Count)];
								if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
										TurnTime = Time.time + 3;
										mAttacking = true;
								}
						}
			
			
				}
				if (!mAttacking) {
						EndTurn ();
				}
		}

		public override void Death ()
		{
				base.Death ();
				Destroy (gameObject);
		}

		public void SelectNewTarget ()
		{
				for (int i = 0; i < mGameController.Players.Count; i++) {
			
						if (Vector3.Distance (transform.position, mGameController.Players [i].transform.position) <= AggroRange) {
								if (!Physics.Linecast (transform.position, mGameController.Players [i].transform.position, CheckWalls)) {
										Targets.Add (mGameController.Players [i]);
									} 
								}
				}

		}

		public void Update ()
		{

				if (mAttacking) {

						Quaternion rotate = Quaternion.LookRotation (Target.transform.position - Turret.transform.position);
						Turret.transform.rotation = Quaternion.Slerp (Turret.transform.rotation, rotate, Time.deltaTime * 5f);
						Target.gameObject.layer = 10;
						RaycastHit hit;
						if (Physics.Raycast (Turret.transform.position, Turret.transform.forward, out hit, Mathf.Infinity, layerMask.value)) {

								if (hit.collider.gameObject.GetComponent<Player> () == Target) {
									if (Target.InCover){
								if (Physics.Linecast (transform.position, Target.transform.position, CheckCover)) {
							print ("Using Cover Attack!");
										CoverAttack(Target);
										} else {
											BasicAttack (Target);
										}
									}else {
										BasicAttack (Target);
										}
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

		public void CoverAttack (Player target)
		{
		
			if (Random.Range (4, 8) < (HitChance - HitReducer)) {
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
						Targets.Clear ();
						mAttacking = false;
						base.EndTurn ();
				}
		}
}
