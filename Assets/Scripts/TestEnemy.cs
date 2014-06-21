using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class TestEnemy : Enemy {
	

	private Path mPath;
	private int mCurrentWaypoint;
	private bool mMoving;
	private bool mMovePhase;
	private Seeker mSeeker;
	private AstarPath mAstarPath;
	private List<Vector3> mPossibleMoves;

	private GameController mGameController;

	void Start(){
		
		mSeeker = gameObject.GetComponent<Seeker> ();
		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		mAstarPath = GameObject.FindGameObjectWithTag ("PathGen").GetComponent<AstarPath> ();
		mGameController.Enemies.Add (this);
		
	}

	public override void StartTurn(){

		mAstarPath.astarData.gridGraph.GetNearest(transform.position).node.Walkable = true;
		TurnActive = true;
		mMovePhase = true;

		if (Target != null) {

						if (Vector3.Distance (transform.position, Target.transform.position) <= AttackRange) {

								BasicAttack (Target);
								EndTurn ();

						} else if (Vector3.Distance (transform.position, Target.transform.position) <= MoveRange) {

								transform.LookAt (Target.transform.position);
								MoveCharacter (Target.transform.position - Vector3.forward * AttackRange);

						} else {
								MoveCharacter (transform.position + Vector3.forward * MoveRange);
								EndTurn ();
						}


				} else {

			for (int i = 0; i < mGameController.Players.Count; i++) {
				
				
				
				if (Vector3.Distance (transform.position, mGameController.Players [i].transform.position) <= AggroRange) {
					
					Target = mGameController.Players[i];
					
				}
				
			}


				}

		if (Target == null) {

			EndTurn ();

				}

		}

	void EndTurn(){

		TurnActive = false;
		mMovePhase = false;
	}

	public override void MoveCharacter (Vector3 target)
	{
		//Seeks out the path to be taken, and calls back with the 'OnPathComplete' method
		mSeeker.StartPath (transform.position, target, OnPathComplete);
	}

	
	public void BasicAttack(AstarAI target){
		
		
		target.Health -= BasicAttackDamage;
		print ("Attacked for " + BasicAttackDamage);
		
		
	}
	
	public void OnPathComplete (Path p)
	{
		//Checks if the path had an error, and if it didn't it sets the path variable to the current path and resets the waypoint counter
		if (!p.error) {
					mPath = p;
					mCurrentWaypoint = 0;
			}
	}

	void Update(){

		if (TurnActive && !mMovePhase) {

			BasicAttack (Target);
			EndTurn ();

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

				mAstarPath.astarData.gridGraph.GetNearest(transform.position).node.Walkable = false;
				mMoving = false;
				mCurrentWaypoint++;
				mMovePhase = false;
			}
		}
	}
	
}
