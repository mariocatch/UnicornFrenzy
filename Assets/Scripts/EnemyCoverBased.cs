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
				if (Target != null) {
					
					transform.LookAt (Target.transform.position);
					
				}
			}
		}
	}

	public override void StartTurn ()
	{
		base.StartTurn ();

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
			Debug.Log (Target.name);
			Debug.Log ("Finding Cover");
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

	public void FindCover(){
		
		List <CoverObject> AvailableCover = new List<CoverObject>();

		Debug.Log (mGameController.CoverObjects.Count);
		for (int i = 0; i < mGameController.CoverObjects.Count; i++){

			Debug.Log(Vector3.Distance(mGameController.CoverObjects[i].transform.position, transform.position));
			if (Vector3.Distance(mGameController.CoverObjects[i].transform.position, transform.position) <= MoveRange){

				Debug.Log ("Distance Check Passed!");
				AvailableCover.Add(mGameController.CoverObjects[i]);
				Debug.Log ("Adding cover object");
			}
			
		}
		Debug.Log ("Passed for loop");
		Debug.Log (AvailableCover.Count);
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
		Debug.Log ("Taking Cover!");
		if (Target.InCover){
			
			if (Target.CurrentCover != CoverTarget){
				
				Target.CurrentCover.gameObject.layer = 0;
				
			}
			
		}

		Debug.Log (MoveableNodes.Count);
		for (int i = 0; i < MoveableNodes.Count; i++){

			Debug.Log ("Checking Linecasts! Node Vector: " + (Vector3)MoveableNodes[i].position + " Target Vector: " + Target.transform.position);
			if (Physics.Linecast ((Vector3)MoveableNodes[i].position + new Vector3(0,1,0), Target.transform.position, CoverLayer)){
				Debug.Log("Point Found! Moving!");
				MoveCharacter((Vector3)MoveableNodes[i].position);
				InCover = true;
				break;
				
			}
			
		}

		if (Target.InCover) {

			Target.CurrentCover.gameObject.layer = 11;

				}

		EndTurn ();
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
