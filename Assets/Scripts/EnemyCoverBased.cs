using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class EnemyCoverBased : Enemy {

	private List<GraphNode> MoveableNodes;
	private CoverObject CoverTarget;
	private bool mMovePhase;
	private bool mMoving;
	private bool mAttacked;
	private bool mEndingTurn;
	private bool InCover;
	private int mCurrentWaypoint;
	private Seeker mSeeker;
	public LayerMask CoverLayer; 

	private Path mPath;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		mSeeker = gameObject.GetComponent<Seeker> ();
	}

	public void Update(){

		if (TurnActive && !mMovePhase && !mAttacked) {

			if (Vector3.Distance (Target.transform.position, transform.position) < AttackRange){

				BasicShot ();
				mAttacked = true;
				mEndingTurn = true;

			} else {

				EndTurn ();

			}


				}

		if (TurnActive && mEndingTurn) {

			if (Time.time > TurnTime){
			EndTurn ();
			}
				}

		}

	public void BasicShot(){

		int DamageDealt = (BasicAttackDamage + Random.Range (0, DamageBonus)) - DamageReducer;
		Target.Health -= DamageDealt;
		Instantiate (BasicAttackParticles, Target.transform.position, Target.transform.rotation);
		print ("Attacked for " + DamageDealt);
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
				mAttacked = false;
				if (Target != null) {
					
					transform.LookAt (Target.transform.position);
					
				}
			}
		}
	}

	public override void StartTurn ()
	{
		base.StartTurn ();
		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
		mMovePhase = true;

		TurnTime = Time.time + 5;

		if (Target != null) {

			if (Target.InCover){

				FindTarget ();

				}

				} else {

			Debug.Log ("Finding Target");
					FindTarget ();
				
				}

		if (Target == null) {
						EndTurn ();
				} else {
			FindCover ();
				}
	}

	public override void MoveCharacter (Vector3 target)
	{
		base.MoveCharacter (target);
		//Seeks out the path to be taken, and calls back with the 'OnPathComplete' method
		mSeeker.StartPath (transform.position, target, OnPathComplete);
	}

	public void FindTarget(){
		
		for (int i=0; i < mGameController.Players.Count; i++){
			if (Vector3.Distance(mGameController.Players[i].transform.position, transform.position) <= AggroRange){
				
				if (!mGameController.Players[i].InCover){
					
					Target = mGameController.Players[i];
					break;
					
				} else {
					
					Target = mGameController.Players[i];
					
				}
				
			}
			
		}

	}

	public override void TakeDamage (int damage)
	{
		if (InCover) {

						for (int i = 0; i < mGameController.Players.Count; i++) {

								if (mGameController.Players [i].TurnActive) {

										if (mGameController.Players [i].InCover) {

												if (mGameController.Players [i].CurrentCover == CoverTarget) {

														if (Physics.Linecast (mGameController.Players [i].transform.position, transform.position, CoverLayer)) {

																TakeCoverDamage(damage);

														} else {

																base.TakeDamage (damage);

														}

												} else {

							mGameController.Players [i].CurrentCover.gameObject.transform.parent.gameObject.layer = 0;

														if (Physics.Linecast (mGameController.Players [i].transform.position, transform.position, CoverLayer)) {

															TakeCoverDamage(damage);

														} else {

																base.TakeDamage (damage);

														}

							mGameController.Players [i].CurrentCover.transform.parent.gameObject.layer = 11;

												}

										} else {

												if (Physics.Linecast (mGameController.Players [i].transform.position, transform.position, CoverLayer)) {

														TakeCoverDamage(damage);

												} else {

															base.TakeDamage (damage);

												}

										}

								}
						}

				} else {

			base.TakeDamage (damage);

				}
	}

	public void TakeCoverDamage(int damage){

		if (Random.Range (0, 4) > 2) {

						Health -= damage;
						CheckHealth ();
						Debug.Log ("Took cover damage! Ow!");
				} else {

			Debug.Log ("Ha! You missed me!");

				}
		}

	public void FindCover(){
		
		List <CoverObject> AvailableCover = new List<CoverObject>();

		for (int i = 0; i < mGameController.CoverObjects.Count; i++){

			Debug.Log(Vector3.Distance(mGameController.CoverObjects[i].transform.position, transform.position));
			if (Vector3.Distance(mGameController.CoverObjects[i].transform.position, transform.position) <= MoveRange){

				AvailableCover.Add(mGameController.CoverObjects[i]);
			}
			
		}
		if (AvailableCover.Count > 0){
			
			CoverTarget = AvailableCover[0];
			
			for (int i = 0; i < AvailableCover.Count; i ++){
				
				if (Vector3.Distance(CoverTarget.transform.position, transform.position) > Vector3.Distance(AvailableCover[i].transform.position, transform.position)){
					CoverTarget = AvailableCover[i];
					
				}
				
			}
			
		}
		
		if (CoverTarget != null){
			StartCoroutine(Constant());
		}
		
	}

	public void TakeCover(){

		if (Target.InCover){
			
			if (Target.CurrentCover != CoverTarget){
				
				Target.CurrentCover.gameObject.transform.parent.gameObject.layer = 0;
				
			}
			
		}

		Debug.Log (MoveableNodes.Count);
		for (int i = 0; i < MoveableNodes.Count; i++){

			Debug.Log ("Checking Linecasts! Node Vector: " + (Vector3)MoveableNodes[i].position + " Target Vector: " + Target.transform.position);
			if (Physics.Linecast ((Vector3)MoveableNodes[i].position + new Vector3(0,1,0), Target.transform.position, CoverLayer)){
				Debug.Log("Point Found! Moving!");
				MoveCharacter((Vector3)MoveableNodes[i].position);
				mMovePhase = true;
				InCover = true;
				if (Target.InCover) {
					
					Target.CurrentCover.gameObject.transform.parent.gameObject.layer = 11;
					
				}
				return;	
			}
			
		}
		mMovePhase = false;
	}

	public override void Death ()
	{
		base.Death ();
		Destroy (gameObject);
	}

	public override void EndTurn ()
	{
						base.EndTurn ();
						mMovePhase = false;
						mEndingTurn = false;
	}

	public void OnConstantPathComplete (Path p)
	{
		
				ConstantPath constPath = p as ConstantPath;
				List<GraphNode> nodes = constPath.allNodes;
				MoveableNodes = nodes;
				TakeCover ();
	}

	public IEnumerator Constant ()
	{
		ConstantPath constPath = ConstantPath.Construct (CoverTarget.transform.position, 5 * 3000, OnConstantPathComplete);
		AstarPath.StartPath (constPath);
		yield return constPath.WaitForPath ();
		Debug.Log (constPath.pathID + " " + constPath.allNodes.Count);
	}
}
