﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class TestEnemy : Enemy
{
	

		private Path mPath;
		private int mCurrentWaypoint;
		private int mNumTurns;
		private bool mMoving;
		private bool mMovePhase;
		private bool mAttacked;
		private bool mEndOfTurn;
		private Seeker mSeeker;
		private List<Vector3> mPossibleMoves;
		public ParticleSystem FlameParticles;
		public ParticleSystem BloodParticles;
		

		public override void Start ()
		{
		base.Start ();
				mSeeker = gameObject.GetComponent<Seeker> ();
		
		}

		public override void StartTurn ()
		{
				base.StartTurn ();

				mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
				mNumTurns ++;
				mMovePhase = true;

				if (Target != null) {

						if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
						TurnTime = Time.time + 3;
						mAttacked = true;
						mEndOfTurn = true;
								if (mNumTurns % 2 == 0) {
									
										FlameThrower (Target);

								} else {

									if (Random.Range (0, 5) < (HitChance - HitReducer)){
											BasicAttack (Target);
										} else {
						print ("Missed!");
					}
								}
								EndTurn ();

						} else if (Vector3.Distance (transform.position, Target.transform.position) <= MoveRange) {

								transform.LookAt (Target.transform.position);
								MoveCharacter (Target.transform.position); // - transform.forward * AttackRange

						} else {
								transform.LookAt (Target.transform.position);
								MoveCharacter (transform.position + transform.forward * MoveRange);
						}


				} else {

						for (int i = 0; i < mGameController.Players.Count; i++) {
				
				
				
								if (Vector3.Distance (transform.position, mGameController.Players [i].transform.position) <= AggroRange) {
					
										Target = mGameController.Players [i];
					if (Vector3.Distance (transform.position, Target.transform.position) <= MoveRange) {
						
						transform.LookAt (Target.transform.position);
						MoveCharacter (Target.transform.position); // - transform.forward * AttackRange
						
					} else {
						transform.LookAt (Target.transform.position);
						MoveCharacter (transform.position + transform.forward * MoveRange);
					}
					
								}
				
						}


				}

				if (Target == null) {

						EndTurn ();

				}

		}

		public override void EndTurn ()
		{
			if (Time.time > TurnTime) {
						base.EndTurn ();
						mAttacked = false;
						mMovePhase = false;
						mEndOfTurn = false;
				}
		}

		public override void MoveCharacter (Vector3 target)
		{
				//Seeks out the path to be taken, and calls back with the 'OnPathComplete' method
				mSeeker.StartPath (transform.position, target, OnPathComplete);
		}
	
		public void BasicAttack (Player target)
		{
		
		int DamageDealt = (BasicAttackDamage + Random.Range (0, DamageBonus)) - DamageReducer;
		target.Health -= DamageDealt;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) {

			if (hit.collider.tag == "Player"){

				Instantiate (BloodParticles, hit.point, transform.rotation);

			}

				}
		print ("Attacked for " + DamageDealt);
		
		
		}

		public void FlameThrower (Player target)
		{

				transform.LookAt (target.transform.position);
				int DamageDealt = (BasicAttackDamage + (2 * Random.Range (0, DamageBonus))) - DamageReducer;
				target.Health -= DamageDealt;
				FlameParticles.Play ();
				print ("FLAMETHROWER! for " + DamageDealt);


		}

		public override void Death ()
	{
		base.Death ();
		Destroy (gameObject);
	}
	
		public void OnPathComplete (Path p)
		{
				//Checks if the path had an error, and if it didn't it sets the path variable to the current path and resets the waypoint counter
				if (!p.error) {
						mPath = p;
						mCurrentWaypoint = 0;
				}
		}

		void Update ()
		{

		if (mEndOfTurn) {

			EndTurn ();

				}

		if (TurnActive && !mMovePhase && !mAttacked) {

						if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {
				mAttacked = true;
				if (mNumTurns % 2 == 0) {
					
					FlameThrower (Target);
					
				} else {
					
					BasicAttack (Target);
				}
				EndTurn ();
						} else {

								EndTurn ();

						}

				} 

				if (Health <= 0) {

						mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
						mGameController.Enemies.Remove (this);
						Destroy (gameObject);

				}

		}
	
		public void FixedUpdate ()
		{		
				//Do nothing if there is no path currently
				if (mPath == null) {
						return;
				}
				//Do nothing if already at the destination
				if (mCurrentWaypoint >= mPath.vectorPath.Count) {
						return;
				}
		
				//Checks if the player is moving or not, then moves the player if they need to be
				if (!mMoving) {
						iTween.MoveTo (gameObject, mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0), 2f);
						mMoving = true;
				} else if (mMoving) {
			
						//If the next waypoint isn't the last one, allows for corners to be cut for smoother looking motion
						if (Vector3.Distance (transform.position, mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0)) < 3f && mCurrentWaypoint < mPath.vectorPath.Count - 1) {
								mMoving = false;
								mCurrentWaypoint++;
						} else if (transform.position == mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0) && mCurrentWaypoint == mPath.vectorPath.Count - 1) {

								mMoving = false;
								mCurrentWaypoint++;
								mMovePhase = false;
								if (Target != null) {

										transform.LookAt (Target.transform.position);
					
								}
						}
				}
		}
	
}
