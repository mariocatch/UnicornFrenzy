using System.Collections;
using Pathfinding;
using UnityEngine;

public class AstarAI : MonoBehaviour
{
		private const float speed = 5;
		private int mCurrentWaypoint;
		private float mJourneyLength;
		private bool mMoving;
		private Seeker mSeeker;
		private float mStartTime;
		private float mTurnTime;
		private float mPathLength;
		private float mMaxPathLength = 25;
		private GameObject mPlayerTarget;
		private AstarPath mAstarPath;
		public MeshRenderer ARangeDisplay;
		public MeshRenderer MRangeDisplay;
		public Path path;
		public int Health;
		public float AttackRange;
		public float TurnLimit;
		public float MaxMoveDistance;
		public bool TurnActive, MovePhase, AttackPhase;
		private bool mAttacking;
		private PlayerAbilityDatabase mAbilityDatabase;
		private GameController mGameController;
		private PlayerAbility mAbility1;

		public void Start ()
		{
				mSeeker = GetComponent<Seeker> ();
				mAstarPath = GameObject.FindGameObjectWithTag ("PathGen").GetComponent<AstarPath> ();
				mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
				mAbilityDatabase = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlayerAbilityDatabase> ();
				mAbility1 = mAbilityDatabase.PlayerAbilities [0];
		}

		public void Update ()
		{
				
				//Sets the display for movement and attack range indicators to only show up during their respective phases
				MRangeDisplay.enabled = MovePhase;
				ARangeDisplay.enabled = AttackPhase;

				if (Health <= 0) {
						//Kill the player (temp)
						print ("I am dead!");
						Destroy (gameObject);

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
                
												if (Vector3.Distance (transform.position, targetPoint) <= MaxMoveDistance) { 
														MoveCharacter (targetPoint);
												}
										}
								}
						}
				}

				if (AttackPhase) {


						//Selects a target to attack. Target must be labeled as an enemy.
						if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && TurnActive && !MovePhase) {
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast (ray, out hit)) {
										if (hit.collider.gameObject.tag == "Enemy") {

												mPlayerTarget = hit.collider.gameObject;
												print ("target aquired!");
										}
								}

						}
						
						//Attacks the currently targeted enemy and ends the turn
						if (mPlayerTarget != null && Input.GetButtonDown ("Attack") && Vector3.Distance (gameObject.transform.position, mPlayerTarget.transform.position) < AttackRange) {

								mPlayerTarget.GetComponent<Enemy> ().TakeDamage (100);
								print ("Attacking!");
								mPlayerTarget = null;
								EndTurn ();
						}

						if (Input.GetButtonDown ("Attack2") && mPlayerTarget != null && Vector3.Distance (gameObject.transform.position, mPlayerTarget.transform.position) < mAbility1.Range) {

				Instantiate (mAbility1.AbilityParticles, mPlayerTarget.transform.position, mPlayerTarget.transform.rotation);
					
				mPlayerTarget.GetComponent<Enemy>().TakeDamage (50);
					
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

		public void StartTurn ()
		{

				//Sets the players turn to true for the game controller and sets the phase and time limit
				mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
				TurnActive = true;
				MovePhase = true;
				print ("starting turn!");
				mTurnTime = Time.time + TurnLimit;
		}

		public void EndTurn ()
		{
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
								path = p;
								MovePhase = false;
								mCurrentWaypoint = 0;
						}
				}
		}

		public void FixedUpdate ()
		{		
				//Do nothing if there is no path currently
				if (path == null) {
						return;
				}
				//Do nothing if already at the destination
				if (mCurrentWaypoint >= path.vectorPath.Count) {
						return;
				}
		

				//Checks if the player is moving or not, then moves the player if they need to be
				if (!mMoving) {
						iTween.MoveTo (gameObject, path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0), 2f);
						mMoving = true;
				} else if (mMoving) {

						//If the next waypoint isn't the last one, allows for corners to be cut for smoother looking motion
						if (Vector3.Distance (transform.position, path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0)) < 3f && mCurrentWaypoint < path.vectorPath.Count - 1) {
								mMoving = false;
								mCurrentWaypoint++;
						} else if (transform.position == path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0) && mCurrentWaypoint == path.vectorPath.Count - 1) {

								mMoving = false;
								mCurrentWaypoint++;
								AttackPhase = true;
						}
				}
		}
}