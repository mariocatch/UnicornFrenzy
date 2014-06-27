using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTestBoss : Enemy
{
	
		private int Turns;
		public ParticleSystem HeavyAttackParticles;
		public ParticleSystem AreaAttackParticles;
		public float BlastRadius;
		public int AreaAttackDamage;
	
		public override void Start ()
		{
				base.Start ();
		}
	
		public override void StartTurn ()
		{
				TurnTime = Time.time + 2;
				base.StartTurn ();
				Turns++;

				Player CurTarget = mGameController.Players [Random.Range (0, mGameController.Players.Count)];
				
				transform.parent.transform.LookAt (CurTarget.transform.position);
					
				if (Turns % 2 == 0) {

						AreaAttack (CurTarget.transform.position);
						print ("Area attack cast!");

				} else if (Vector3.Distance (CurTarget.transform.position, transform.position) <= AttackRange) {

						BasicAttack (CurTarget);

				} else {

						HeavyAttack (CurTarget);

				}


		}
	
		public override void Death ()
		{
				base.Death ();
				mGameController.mGameWon = true;
				Destroy (gameObject);
		}
	
		public void Update ()
		{

				if (Time.time > TurnTime) {

						EndTurn ();

				}
		}
	
		public void BasicAttack (Player target)
		{

		target.Health -= BasicAttackDamage;
		Instantiate (BasicAttackParticles, target.transform.position, target.transform.rotation);

		}

		public void HeavyAttack (Player target)
		{

		target.Health -= BasicAttackDamage * 2;
		Instantiate (HeavyAttackParticles, target.transform.position, target.transform.rotation);

		}

		public void AreaAttack (Vector3 targetArea)
		{

		Instantiate (AreaAttackParticles, targetArea, Quaternion.identity);
		for (int i=0; i < mGameController.Players.Count; i++) {

			if (Vector3.Distance (mGameController.Players[i].transform.position, targetArea) <= BlastRadius){

				mGameController.Players[i].Health -= AreaAttackDamage;

			}
				}

		}

		public override void EndTurn ()
		{
				base.EndTurn ();

		}


}
