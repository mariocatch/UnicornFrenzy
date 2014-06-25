using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Player : MonoBehaviour {

	//GUI Variables
	public string PlayerName;

	//GameController
	[HideInInspector]
	public GameController mGameController;

	//Player stats
	public int Strength;
	public int Accuracy;
	public int Endurance;
	public int Armor;
	public int Technology;
	public int Speed;
	[HideInInspector]
	public int PrimaryStat;
	[HideInInspector]
	public int SecondaryStat;
	public int Range;

	//Resource variables
	public int Health;
	[HideInInspector]
	public int ActionPoints;

	//Resource maximums
	public int MaxHealth;
	public int MaxActionPoints = 6;

	//Navigation variables
	[HideInInspector]
	public Seeker mSeeker;
	[HideInInspector]
	public int mCurrentWaypoint;
	[HideInInspector]
	public float mPathLength;
	public float mMaxPathLength = 25;
	[HideInInspector]
	public AstarPath mAstarPath;
	[HideInInspector]
	public Path Path;

	//Turn variables
	[HideInInspector]
	public bool TurnActive;
	[HideInInspector]
	public bool MovePhase;
	[HideInInspector]
	public bool FinishedMoving;
	[HideInInspector]
	public bool AttackAble;
	[HideInInspector]
	public bool mMoving;
	[HideInInspector]
	public bool MoveAble;
	[HideInInspector]
	public float mTurnTime;
	public float TurnLimit = 25;

	//Combat variables
	[HideInInspector]
	public Enemy mEnemyTarget;
	[HideInInspector]
	public Player mFriendlyTarget;
	[HideInInspector]
	public bool mSelectLocation;
	[HideInInspector]
	public PlayerAbility mWorldAbility;

	//Range displays
	public MeshRenderer ARangeDisplay;
	public MeshRenderer MRangeDisplay;

	//Abilities
	public List<PlayerAbility> Abilities;
	public PlayerAbility Ability1, Ability2, Ability3, Ability4;

	public virtual void Start(){

		mSeeker = gameObject.GetComponent<Seeker> ();
		mAstarPath = GameObject.FindGameObjectWithTag ("PathGen").GetComponent<AstarPath> ();
		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		ARangeDisplay.transform.localScale = new Vector3 (Range * 2, 0, Range * 2);
		
	}

	public virtual void Update(){

		if (ActionPoints > MaxActionPoints) {

			ActionPoints = MaxActionPoints;

				}
		if (TurnActive) {

						if (!mSelectLocation && !MoveAble && !AttackAble && !mMoving && FinishedMoving) {

								EndTurn ();

						}

						MRangeDisplay.enabled = MovePhase;

						if (mSelectLocation || !AttackAble) {
								ARangeDisplay.enabled = false;
						} else {
								ARangeDisplay.enabled = !MovePhase;
						}
		
						if (Health > MaxHealth) {

								Health = MaxHealth;

						}

						if (Time.time > mTurnTime && TurnActive) {
			
								//Ends the players turn after a certain amount of time
								EndTurn ();
			
						}

						if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && TurnActive && !MovePhase) {
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit)) {
										if (hit.collider.gameObject.tag == "Enemy") {
					
												mEnemyTarget = hit.collider.gameObject.GetComponent<Enemy> ();
												print ("enemy target aquired!");
										} else if (hit.collider.gameObject.tag == "Player") {

												mFriendlyTarget = hit.collider.gameObject.GetComponent<Player> ();
												print ("friendly target aquired!");
										}
								}
			
						}

						if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && TurnActive && MovePhase) {
								var playerPlane = new Plane (Vector3.up, transform.position);
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								float hitdist = 0.0f;
			
								//Casts a ray at the screen position to select a new point to move to
								//will only navigate to said point if the ground is hit
								if (Physics.Raycast (ray, out hit)) {
				
										if (hit.collider.tag == "Ground") {
					
												if (playerPlane.Raycast (ray, out hitdist)) {
														Vector3 targetPoint = ray.GetPoint (hitdist);
						
														if (Vector3.Distance (transform.position, targetPoint) <= Speed) { 
																MoveCharacter (targetPoint);
																MoveAble = false;
														}
												}
										}
								}
			
						}

						if (mSelectLocation && Input.GetMouseButtonDown (0)) {

								var playerPlane = new Plane (Vector3.up, transform.position);
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								float hitdist = 0.0f;
			
								//Casts a ray at the screen position to select a new point to move to
								//will only navigate to said point if the ground is hit
								if (Physics.Raycast (ray, out hit)) {
				
										if (hit.collider.tag == "Ground") {
					
												if (playerPlane.Raycast (ray, out hitdist)) {
														Vector3 targetPoint = ray.GetPoint (hitdist);
						
														if (Vector3.Distance (transform.position, targetPoint) <= Range) { 
																mWorldAbility.UseAbility (this, targetPoint, PrimaryStat, SecondaryStat, mGameController);
																mWorldAbility = null;
																mSelectLocation = false;
														}
												}
										}
								}	
		
						}
				} else {
			MRangeDisplay.enabled = false;
			ARangeDisplay.enabled = false;

				}
	}

	void OnGUI(){

		//Activates player specific Gui elements during the respective players turn
		if (TurnActive) {

			GUI.Label (new Rect (225, 15, 80, 25), PlayerName);

			if (GUI.Button(new Rect(90, 45, 80, 20), "Move") && MoveAble && !mSelectLocation){

				MovePhase = true;

			}

			if (GUI.Button(new Rect(180, 45, 80, 20), "Ability1") && AttackAble && !MovePhase && FinishedMoving){
				
				AbilityHandler (Ability1);
	
			}

			if (GUI.Button(new Rect(270, 45, 80, 20), "Ability2") && AttackAble && !MovePhase && FinishedMoving){
				
				AbilityHandler (Ability2);
				
			}

			if (GUI.Button(new Rect(360, 45, 80, 20), "End Turn") && FinishedMoving){
				
				EndTurn ();
				
			}



				}

		}

	public void AbilityHandler(PlayerAbility ability){

		switch (ability.Type) {

		case PlayerAbility.AbilityType.EnemyTarget:
			if (mEnemyTarget != null){
				if (Vector3.Distance(mEnemyTarget.gameObject.transform.position, gameObject.transform.position) <= Range){
				ability.UseAbility (this, mEnemyTarget, PrimaryStat, SecondaryStat);
				AttackAble = false;
				}
			}
			break;

		case PlayerAbility.AbilityType.AllyTarget:
			if (mFriendlyTarget != null){
				if (Vector3.Distance(mFriendlyTarget.gameObject.transform.position, gameObject.transform.position) <= Range){
				ability.UseAbility (this, mFriendlyTarget, PrimaryStat, SecondaryStat);
				AttackAble = false;
				}
			}
			break;

		case PlayerAbility.AbilityType.Self:

				ability.UseAbility (this, PrimaryStat, SecondaryStat, mGameController);
				AttackAble = false;                   

			break;

		case PlayerAbility.AbilityType.WorldTarget:

				mWorldAbility = ability;
				mSelectLocation = true;
				AttackAble = false;                   

			break;

				}
		}
	
	public void StartTurn(){
		
		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
		TurnActive = true;
		MoveAble = true;
		AttackAble = true;
		FinishedMoving = true;
		print ("starting turn!");
		mTurnTime = Time.time + TurnLimit;

	}
	
	public void EndTurn(){

		//Ensures all phases are false and sets the players turn to false for the game controller
		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = false;
		TurnActive = false;
		MovePhase = false;
		FinishedMoving = false;
		MoveAble = false;
		AttackAble = false; 
		print ("Ending turn!");
	}
	
	public void MoveCharacter (Vector3 target)
	{
		//Seeks out the path to be taken, and calls back with the 'OnPathComplete' method
		mSeeker.StartPath (transform.position, target, OnPathComplete);
	}
	
	public void OnPathComplete (Path p)
	{
		//Checks if the path had an error, and if it didn't it sets the path variable to the current path and resets the waypoint counter
		if (!p.error) {
			
			mPathLength = p.GetTotalLength ();
			if (mPathLength <= mMaxPathLength) {
				FinishedMoving = false;
				Path = p;
				MovePhase = false;
				mCurrentWaypoint = 0;
			}
		}
	}

	public virtual void FixedUpdate ()
	{		
		//Do nothing if there is no path currently
		if (Path == null) {
			return;
		}
		//Do nothing if already at the destination
		if (mCurrentWaypoint >= Path.vectorPath.Count) {
			return;
		}
		
		
		//Checks if the player is moving or not, then moves the player if they need to be
		if (!mMoving) {
			iTween.MoveTo (gameObject, Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0), 2f);
			mMoving = true;
		} else if (mMoving) {
			
			//If the next waypoint isn't the last one, allows for corners to be cut for smoother looking motion
			if (Vector3.Distance (transform.position, Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0)) < 3f && mCurrentWaypoint < Path.vectorPath.Count - 1) {
				mMoving = false;
				mCurrentWaypoint++;
			} else if (transform.position == Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0) && mCurrentWaypoint == Path.vectorPath.Count - 1) {
				
				mMoving = false;
				mCurrentWaypoint++;
				FinishedMoving = true;
			}
		}
	}
	
	
}
