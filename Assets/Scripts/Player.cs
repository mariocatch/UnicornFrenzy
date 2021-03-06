﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class Player : MonoBehaviour
{

		//GUI Variables
		public string PlayerName;
		public Texture MoveIcon;
		public Texture EndTurnIcon;

		//GameController and Inventory
		[HideInInspector]
		public GameController
				mGameController;
	[HideInInspector]
	public PlayerInventory Inventory;

		//Player stats
		public int Strength;
		public int Accuracy;
		public int Endurance;
		public int Armor;
		public int Technology;
		public int Speed;
		[HideInInspector]
		public int
				PrimaryStat;
		[HideInInspector]
		public int
				SecondaryStat;
		public int Range;
		[HideInInspector]
		public int
				MoveSpeed;

		//Resource variables
		public int Health;
		[HideInInspector]
		public int
				ActionPoints;

		//Resource maximums
		[HideInInspector]
		public int
				mMaxMove;
		public int MaxHealth;
		[HideInInspector]
		public int
				MaxActionPoints;

		//Navigation variables
		[HideInInspector]
		public GraphNode TargetNode;
		[HideInInspector]
		public List<GameObject>
				RenderedGrid = new List<GameObject> ();
		[HideInInspector]
		public List<GraphNode>
				MoveableNodes;
		[HideInInspector]
		public Material
				SquareMat;
		[HideInInspector]
		public Vector3
				PathOffset;
		[HideInInspector]
		public List<GraphNode>
				Nodes = new List<GraphNode> ();
		[HideInInspector]
		public Seeker
				mSeeker;
		[HideInInspector]
		public int
				mCurrentWaypoint;
		[HideInInspector]
		public float
				mPathLength;
		[HideInInspector]
		public float
				mMaxPathLength;
		[HideInInspector]
		public AstarPath
				mAstarPath;
		[HideInInspector]
		public Path
				Path;

		//Turn variables
		[HideInInspector]
		public bool EndingTurn;	
		[HideInInspector]
		public bool
				TurnActive;
		[HideInInspector]
		public bool TurnPaused;
		[HideInInspector]
		public bool
				MovePhase;
		[HideInInspector]
		public bool
				FinishedMoving;
		[HideInInspector]
		public bool
				AttackAble;
		[HideInInspector]
		public bool
				mMoving;
		[HideInInspector]
		public bool
				MoveAble;
		[HideInInspector]
		public float
				mTurnTime;
		public float TurnLimit = 25;

		//Combat variables
		[HideInInspector]
		public CoverObject
				CurrentCover;
		[HideInInspector]
		public bool
				InCover;
		[HideInInspector]
		public Enemy
				mEnemyTarget;
		[HideInInspector]
		public Player
				mFriendlyTarget;
		[HideInInspector]
		public bool
				mSelectLocation;
		[HideInInspector]
		public bool
				mFTargetSelect;
		[HideInInspector]
		public bool
				mETargetSelect;
		[HideInInspector]
		public PlayerAbility
				mWorldAbility;
		[HideInInspector]
		public PlayerAbility
				mTargetAbility;

		//Displays
		public GameObject SelectionIndicator;
		public MeshRenderer ARangeDisplay;

		//Abilities
		public List<PlayerAbility> Abilities;
		public PlayerAbility Ability1, Ability2, Ability3, Ability4;

		public virtual void Start ()
		{
				SelectionIndicator.SetActive (false);
				FinishedMoving = true;
				mMaxMove = 20;
				MaxActionPoints = 6;
				mMaxPathLength = 35;
				mSeeker = gameObject.GetComponent<Seeker> ();
				mAstarPath = GameObject.FindGameObjectWithTag ("PathGen").GetComponent<AstarPath> ();
				mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
				Inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<PlayerInventory> ();
				PathOffset = new Vector3 (0, .1f, 0);
				SquareMat = Resources.Load ("pathSquare", typeof(Material)) as Material;
				MoveIcon = Resources.Load ("Move", typeof(Texture)) as Texture;
				EndTurnIcon = Resources.Load ("EndTurn", typeof(Texture)) as Texture;
			
		}

		public virtual void Update ()
		{

				
				
				if (Health <= 0) {
						Death ();
				}

				if (ActionPoints > MaxActionPoints) {

						ActionPoints = MaxActionPoints;

				}

				if (EndingTurn) {

				if (FinishedMoving){
				EndTurn ();
				}

				}

				if (TurnActive || TurnPaused) {
						
						if (!mSelectLocation && !mFTargetSelect && !mETargetSelect && !MoveAble && !AttackAble && !mMoving && FinishedMoving) {
			
								EndTurn ();
			
						}
				}
				
				if (TurnActive) {

		
						if (Health > MaxHealth) {

								Health = MaxHealth;

						}

						if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && TurnActive) {

								var playerPlane = new Plane (Vector3.up, transform.position);
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								float hitdist = 0.0f;

								if (mETargetSelect) {

										if (Physics.Raycast (ray, out hit)) {
												if (hit.collider.gameObject.tag == "Enemy" && Vector3.Distance (hit.transform.position, transform.position) <= mTargetAbility.Range) {
							
														mEnemyTarget = hit.collider.gameObject.GetComponent<Enemy> ();
														print ("enemy target aquired!");
														mTargetAbility.UseAbility (this, mEnemyTarget, PrimaryStat, SecondaryStat);
														ARangeDisplay.enabled = false;
														mETargetSelect = false;
														mTargetAbility = null;
												}
										} 

								} else if (mFTargetSelect) {

										if (Physics.Raycast (ray, out hit)) {
												if (hit.collider.gameObject.tag == "Player" && Vector3.Distance (hit.transform.position, transform.position) <= mTargetAbility.Range) {
							
														mFriendlyTarget = hit.collider.gameObject.GetComponent<Player> ();
														print ("friendly target aquired!");
														mTargetAbility.UseAbility (this, mFriendlyTarget, PrimaryStat, SecondaryStat);
														ARangeDisplay.enabled = false;
														mFTargetSelect = false;
														mTargetAbility = null;
												} 
										}

								} else if (MovePhase) {

										if (Physics.Raycast (ray, out hit)) {
						
												if (hit.collider.tag == "Ground") {
							
														if (playerPlane.Raycast (ray, out hitdist)) {
																Vector3 targetPoint = ray.GetPoint (hitdist);
																TargetNode = mAstarPath.astarData.gridGraph.GetNearest (targetPoint).node;
																if (TargetNode != null) { 
																		GraphNode match = MoveableNodes.FirstOrDefault (x => x.position == TargetNode.position);
																		if (match != null) {
																				MoveCharacter (targetPoint);
																				ClearRender ();
																				MoveAble = false;
																				SelectionIndicator.SetActive (true);
																		}
																}
														}
												}
										}

								} else if (mSelectLocation) {

										if (Physics.Raycast (ray, out hit)) {
						
												if (hit.collider.tag == "Ground") {
							
														if (playerPlane.Raycast (ray, out hitdist)) {
																Vector3 targetPoint = ray.GetPoint (hitdist);
								
																if (Vector3.Distance (transform.position, targetPoint) <= mWorldAbility.Range) { 
																		mWorldAbility.UseAbility (this, targetPoint, PrimaryStat, SecondaryStat, mGameController);
																		ARangeDisplay.enabled = false;
																		mWorldAbility = null;
																		mSelectLocation = false;
																}
														}
												}
										}

								} else {

										if (Physics.Raycast (ray, out hit)) {
												if (hit.collider.gameObject.tag == "Item" && Vector3.Distance (hit.transform.position, transform.position) <= 8) {
												
													ItemPickup hitItem = hit.collider.gameObject.GetComponent<ItemPickup>();
													Inventory.AddItem(hitItem.ItemID, hitItem.ItemQuantity);
													hitItem.RemoveObject ();

												} else if (hit.collider.gameObject.tag == "Interactable" && Vector3.Distance (hit.transform.position, transform.position) <= 8){

													Debug.Log("Using Object!");
													hit.collider.gameObject.GetComponent<InteractiveObject>().UseObject ();

												} else if (hit.collider.gameObject.tag == "Player"){

												Player SelectedPlayer = hit.collider.gameObject.GetComponent<Player>();

												if (mGameController.PlayerInActiveList(SelectedPlayer)){

															PauseTurn ();
								if (SelectedPlayer.TurnPaused){
									SelectedPlayer.ResumeTurn ();
								} else {
									SelectedPlayer.StartTurn ();
								}

													}

												}
										}

								}

						}

						

						if (Input.GetMouseButtonDown (1) && TurnActive) {

								if (mSelectLocation || mETargetSelect || mFTargetSelect) {

										mSelectLocation = false;
										mETargetSelect = false;
										mFTargetSelect = false;
										mWorldAbility = null;
										mTargetAbility = null;
										ARangeDisplay.enabled = false;
										AttackAble = true;
								}

								if (MovePhase && MoveAble){

									MovePhase = false;
									SelectionIndicator.SetActive (true);
									ClearRender ();
					
								}
				
						}
				} else {
						ARangeDisplay.enabled = false;
				}
		}

		public void PauseTurn(){

		TurnPaused = true;
		SelectionIndicator.SetActive (false);
		TurnActive = false;

		}


		public void ResumeTurn(){

		TurnActive = true;
		SelectionIndicator.SetActive (true);
		TurnPaused = false;

		}

		public void Death ()
		{

				mGameController.Players.Remove (this);
				mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
				Destroy (gameObject);
		
		}

		void OnGUI ()
		{

				//Activates player specific Gui elements during the respective players turn
				if (TurnActive) {

						if (GUI.Button (new Rect ((Screen.width / 2) - 200, Screen.height - 80, 64, 64), MoveIcon) && MoveAble && !mSelectLocation && !mFTargetSelect && !mETargetSelect && !MovePhase) {

								MovePhase = true;
								SelectionIndicator.SetActive (false);
								StartCoroutine (Constant ());
				
						}

						if (GUI.Button (new Rect ((Screen.width / 2) - 130, Screen.height - 80, 64, 64), Ability1.Icon) && AttackAble && !MovePhase && FinishedMoving) {
				
								if (ActionPoints >= Ability1.ApCost) {	
										AbilityHandler (Ability1);
								}
	
						}

						if (GUI.Button (new Rect ((Screen.width / 2) - 60, Screen.height - 80, 64, 64), Ability2.Icon) && AttackAble && !MovePhase && FinishedMoving) {
								if (ActionPoints >= Ability2.ApCost) {
										AbilityHandler (Ability2);
								}
				
						}

						if (GUI.Button (new Rect ((Screen.width / 2) + 10, Screen.height - 80, 64, 64), EndTurnIcon) && FinishedMoving) {
				
								EndTurn ();
				
						}



				}

		}

		public void AbilityHandler (PlayerAbility ability)
		{

				switch (ability.Type) {

				case PlayerAbility.AbilityType.EnemyTarget:
						mTargetAbility = ability;
						ARangeDisplay.transform.localScale = new Vector3 (ability.Range * 2, 0, ability.Range * 2);
						ARangeDisplay.enabled = true;
						mETargetSelect = true;
						AttackAble = false;
						break;

				case PlayerAbility.AbilityType.AllyTarget:
						mTargetAbility = ability;
						ARangeDisplay.transform.localScale = new Vector3 (ability.Range * 2, 0, ability.Range * 2);
						ARangeDisplay.enabled = true;
						mFTargetSelect = true;
						AttackAble = false;
						break;

				case PlayerAbility.AbilityType.Self:

						ability.UseAbility (this, PrimaryStat, SecondaryStat, mGameController);
						AttackAble = false;                   

						break;

				case PlayerAbility.AbilityType.WorldTarget:

						mWorldAbility = ability;
						ARangeDisplay.transform.localScale = new Vector3 (ability.Range * 2, 0, ability.Range * 2);
						ARangeDisplay.enabled = true;
						mSelectLocation = true;
						AttackAble = false;                   

						break;

				}
		}
	
		public void StartTurn ()
		{
				SelectionIndicator.SetActive (true);
				MoveSpeed = mMaxMove;
				
				for (int i = 0; i < mGameController.Enemies.Count; i++) {
					
						if (Vector3.Distance (mGameController.Enemies [i].transform.position, transform.position) < 100) {
				
								MoveSpeed = Speed;
								break;
						}

				}
				
				TurnActive = true;
				MoveAble = true;
				AttackAble = true;
				FinishedMoving = true;
				print ("starting turn!");
				mTurnTime = Time.time + TurnLimit;

		}

		public void OnConstantPathComplete (Path p)
		{

				ConstantPath constPath = p as ConstantPath;
				List<GraphNode> nodes = constPath.allNodes;
				MoveableNodes = nodes;
		
				Mesh mesh = new Mesh ();
		
				List<Vector3> verts = new List<Vector3> ();
		
				bool drawRaysInstead = false;
		
				List<Vector3> pts = Pathfinding.PathUtilities.GetPointsOnNodes (nodes, 20, 0);
				Vector3 avg = Vector3.zero;
				for (int i=0; i<pts.Count; i++) {
						Debug.DrawRay (pts [i], Vector3.up * 5, Color.red, 3);
						avg += pts [i];
				}
		
				if (pts.Count > 0)
						avg /= pts.Count;
		
				for (int i=0; i<pts.Count; i++) {
						pts [i] -= avg;
				}
		
				Pathfinding.PathUtilities.GetPointsAroundPoint (transform.position, AstarPath.active.astarData.graphs [0] as IRaycastableGraph, pts, 0, 1);
		
				for (int i=0; i<pts.Count; i++) {
						Debug.DrawRay (pts [i], Vector3.up * 5, Color.blue, 3);
				}
		
				//This will loop through the nodes from furthest away to nearest, not really necessary... but why not :D
				//Note that the reverse does not, as common sense would suggest, loop through from the closest to the furthest away
				//since is might contain duplicates and only the node duplicate placed at the highest index is guarenteed to be ordered correctly.
				for (int i=nodes.Count-1; i>=0; i--) {
			
						Vector3 pos = (Vector3)nodes [i].position + PathOffset;
						if (verts.Count == 65000 && !drawRaysInstead) {
								Debug.LogError ("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
								drawRaysInstead = true;
						}
			
						if (drawRaysInstead) {
								Debug.DrawRay (pos, Vector3.up, Color.blue);
								continue;
						}
			
						//Add vertices in a square
			
						GridGraph gg = AstarData.GetGraph (nodes [i]) as GridGraph;
						float scale = 1F;
			
						if (gg != null)
								scale = gg.nodeSize;
			
						verts.Add (pos + new Vector3 (-0.5F, 0, -0.5F) * scale);
						verts.Add (pos + new Vector3 (0.5F, 0, -0.5F) * scale);
						verts.Add (pos + new Vector3 (-0.5F, 0, 0.5F) * scale);
						verts.Add (pos + new Vector3 (0.5F, 0, 0.5F) * scale);
				}
		
				//Build triangles for the squares
				Vector3[] vs = verts.ToArray ();
				int[] tris = new int[(3 * vs.Length) / 2];
				for (int i=0, j=0; i<vs.Length; j+=6, i+=4) {
						tris [j + 0] = i;
						tris [j + 1] = i + 1;
						tris [j + 2] = i + 2;
			
						tris [j + 3] = i + 1;
						tris [j + 4] = i + 3;
						tris [j + 5] = i + 2;
				}
		
				Vector2[] uv = new Vector2[vs.Length];
				//Set up some basic UV
				for (int i=0; i<uv.Length; i+=4) {
						uv [i] = new Vector2 (0, 0);
						uv [i + 1] = new Vector2 (1, 0);
						uv [i + 2] = new Vector2 (0, 1);
						uv [i + 3] = new Vector2 (1, 1);
				}
		
				mesh.vertices = vs;
				mesh.triangles = tris;
				mesh.uv = uv;
				mesh.RecalculateNormals ();
		
				GameObject go = new GameObject ("Mesh", typeof(MeshRenderer), typeof(MeshFilter));
				MeshFilter fi = go.GetComponent<MeshFilter> ();
				fi.mesh = mesh;
				MeshRenderer re = go.GetComponent<MeshRenderer> ();
				re.material = SquareMat;
		
				RenderedGrid.Add (go);

		}

		public void ClearRender ()
		{

				for (int i=0; i<RenderedGrid.Count; i++) {

						Destroy (RenderedGrid [i]);

				}
				RenderedGrid.Clear ();
		}
	
		public void EndTurn ()
		{

				//Ensures all phases are false and sets the players turn to false for the game controller
				SelectionIndicator.SetActive (false);
				ClearRender ();
				//mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = false;
				TurnActive = false;
				TurnPaused = false;
				MovePhase = false;
				EndingTurn = false;
				MoveAble = false;
				AttackAble = false;
				mFTargetSelect = false;
				mETargetSelect = false;
				mGameController.EndPlayerTurn (this);
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
								TargetNode.Walkable = false;
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
						iTween.MoveTo (gameObject, Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0), 1.5f);
						mMoving = true;
				} else if (mMoving) {
			
						//If the next waypoint isn't the last one, allows for corners to be cut for smoother looking motion
						if (Vector3.Distance (transform.position, Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0)) < 2f && mCurrentWaypoint < Path.vectorPath.Count - 1) {
								mMoving = false;
								mCurrentWaypoint++;
						} else if (transform.position == Path.vectorPath [mCurrentWaypoint] + new Vector3 (0, 1, 0) && mCurrentWaypoint == Path.vectorPath.Count - 1) {
				
								mMoving = false;
								mCurrentWaypoint++;
								FinishedMoving = true;
						}
				}
		}

		public IEnumerator Constant ()
		{
				mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
				ConstantPath constPath = ConstantPath.Construct ((transform.position - new Vector3(0,1, 0)), (MoveSpeed / 3) * 3000, OnConstantPathComplete);
				AstarPath.StartPath (constPath);
				yield return constPath.WaitForPath ();
				Debug.Log (constPath.pathID + " " + constPath.allNodes.Count);
		}
	
	
}
