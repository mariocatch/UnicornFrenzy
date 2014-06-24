using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Player : MonoBehaviour {

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
	public bool AttackPhase;
	[HideInInspector]
	public bool mMoving;
	[HideInInspector]
	public float mTurnTime;
	public float TurnLimit = 25;

	//Combat variables
	[HideInInspector]
	public GameObject mPlayerTarget;
	[HideInInspector]
	public bool mSelectLocation;

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
		
	}

	public virtual void Update(){

		if (ActionPoints > MaxActionPoints) {

			ActionPoints = MaxActionPoints;

				}
		if (!AttackPhase) {

			mSelectLocation = false;

				}
		MRangeDisplay.enabled = MovePhase;
		ARangeDisplay.enabled = AttackPhase;
		
		if (Health > MaxHealth) {

			Health = MaxHealth;

				}

		if (Time.time > mTurnTime && TurnActive) {
			
			//Ends the players turn after a certain amount of time
			EndTurn ();
			
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
						}
					}
				}
			}
		}

		if (mSelectLocation && Input.GetMouseButtonDown (0) && AttackPhase) {

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
						
						if (Vector3.Distance (transform.position, targetPoint) <= Ability2.Range) { 
							Ability2.UseAbility(this, targetPoint, 0, mGameController);
							mSelectLocation = false;
							EndTurn ();
						}
					}
				}
			}	
		
		}

		if (TurnActive && Input.GetMouseButtonDown (1) && !mMoving) {
			//Right clicking will end the current phase, or turn
			
			if (MovePhase) {
				
				MovePhase = false;
				AttackPhase = true;
				
			} else {
				
				EndTurn ();
			}
			
			
		}
		
	}
	
	public void StartTurn(){
		
		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
		TurnActive = true;
		MovePhase = true;
		print ("starting turn!");
		mTurnTime = Time.time + TurnLimit;

	}
	
	public void EndTurn(){

		//Ensures all phases are false and sets the players turn to false for the game controller
		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = false;
		TurnActive = false;
		MovePhase = false;
		AttackPhase = false;
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
				AttackPhase = true;
			}
		}
	}
	
	
}
