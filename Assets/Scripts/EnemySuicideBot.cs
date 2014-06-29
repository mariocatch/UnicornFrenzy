using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class EnemySuicideBot : Enemy {

	private Path mPath;
	private Seeker mSeeker; 
	private int mCurrentWaypoint;
	private bool mMoving;
	private bool mExplode;
	private Animator mAnimator;
	public int BlastRadius;
	public ParticleSystem HitParticles;


	public override void Start ()
	{
		base.Start ();
		mSeeker = gameObject.GetComponent<Seeker> ();
		mAnimator = gameObject.GetComponent<Animator> ();
		
	}

	public override void StartTurn ()
	{
		base.StartTurn ();

		List<Player> potentialTargets = new List<Player> ();
		for (int i = 0; i < mGameController.Players.Count; i++) {

			if (Vector3.Distance (mGameController.Players[i].transform.position, transform.position) < AggroRange){
				potentialTargets.Add(mGameController.Players[i]);
			}

				}
		if (potentialTargets.Count != 0) {

						MoveCharacter (potentialTargets [Random.Range (0, potentialTargets.Count)].transform.position);
						mAnimator.SetBool("Moving", true);
						TurnTime = Time.time + 4;
						mExplode = true;

				} else {

			EndTurn ();

				}

	}

	void Update(){

		if (mExplode && Time.time > TurnTime) {
			TurnActive = false;
			Death();

				}
		}

	public override void Death ()
	{
		base.Death ();
		for (int i = 0; i < mGameController.Players.Count; i++) {
			
			if (Vector3.Distance(mGameController.Players[i].transform.position, transform.position) < BlastRadius){
				
				mGameController.Players[i].Health -= BasicAttackDamage + Random.Range((int) (DamageBonus /2) , DamageBonus);
				Instantiate (HitParticles, mGameController.Players[i].transform.position, Quaternion.identity);
			}
			
		}
		Destroy (gameObject);
	}

	public override void MoveCharacter (Vector3 target)
	{
		//Seeks out the path to be taken, and calls back with the 'OnPathComplete' method
		mSeeker.StartPath (transform.position, target, OnPathComplete);
	}

	public void OnPathComplete (Path p)
	{
		//Checks if the path had an error, and if it didn't it sets the path variable to the current path and resets the waypoint counter
		if (!p.error) {
			mPath = p;
			mCurrentWaypoint = 0;
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

		if (mCurrentWaypoint == mPath.vectorPath.Count - 1) {

			mAnimator.SetBool ("Moving", false);
			mAnimator.SetBool ("Attacking", true);

				}
		
		//Checks if the player is moving or not, then moves the player if they need to be
		if (!mMoving) {
			transform.LookAt(new Vector3(mPath.vectorPath[mCurrentWaypoint].x, 1, mPath.vectorPath[mCurrentWaypoint].z));
			iTween.MoveTo (gameObject, mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 0, 0), 2f);
			mMoving = true;
		} else if (mMoving) {
			
			//If the next waypoint isn't the last one, allows for corners to be cut for smoother looking motion
			if (Vector3.Distance (transform.position, mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 0, 0)) < 3f && mCurrentWaypoint < mPath.vectorPath.Count - 1) {
				mMoving = false;
				mCurrentWaypoint++;
			} else if (transform.position == mPath.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0) && mCurrentWaypoint == mPath.vectorPath.Count - 1) {
				
				mMoving = false;
				mCurrentWaypoint++;
				if (Target != null) {
					
					transform.LookAt (Target.transform.position);
					
				}
			}
		}
	}

}
