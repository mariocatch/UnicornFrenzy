using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GameController : MonoBehaviour
{

		public List <AstarAI> Players;
		public List <Enemy> Enemies;
		public GameObject NewPlane;
		public Transform SpawnLocation;
		public AstarPath aStarPath;
		private float mTimeToScan;
		private float mScanDelay = .5f;
		private int mCurrentPlayer;
		private int mCurrentEnemy;
		private bool mNodesChanged;
		private bool mPlayersTurn = true;
		private bool mEnemiesTurn;

		void Start ()
		{

				//Starts the game with the first players turn
				Players [mCurrentPlayer].StartTurn ();

		}

		void Update ()
		{

				//Checks if the nodes have been changed, waits for a brief moment, then scans the grid.
				if (mNodesChanged) {

						if (Time.time > mTimeToScan) {

								ScanPath ();
								mNodesChanged = false;

						}

				}
				if (mPlayersTurn) {
						//Cycles through player turns
						if (Players [mCurrentPlayer].TurnActive == false) {

								mCurrentPlayer++;

								if (mCurrentPlayer > Players.Count - 1) {
				
										mCurrentPlayer = 0;
										mPlayersTurn = false;
										mEnemiesTurn = true;
				
								} else {
										Players [mCurrentPlayer].StartTurn ();
								}
						}
				}
				if (mEnemiesTurn) {

						if (Enemies.Count > 0) {
						
								print ("Enemy turn!");
								Enemy currentEnemy = Enemies [mCurrentEnemy];
								//Check if the enemy has a current target
								//Check if that target is within attack range
								//if so, attack then end turn, increment enemy turn
								//Check if that target, if not within attack range, is within movement range
								//move to within attack range
								//attack then end turn, increment enemy turn
								//if not within movement range, check if any other players are within attack range, then movement range
								//if another player is within attack range, attack that player
								//if not, and another player is within movement range, move to and attack that player
								//if no players are within range for anything, move towards initial target
								//end turn, increment enemy turn
			
								//if all enemies have gone, switch to player turn.
								if (currentEnemy.Target != null) {
										print ("I have a target!");
										//Check if that target is within attack range
										if (Vector3.Distance (currentEnemy.transform.position, currentEnemy.Target.gameObject.transform.position) <= currentEnemy.AttackRange) {

												print ("Attacking target!");
												currentEnemy.BasicAttack (currentEnemy.Target);
												mCurrentEnemy++;
												

										} else if (Vector3.Distance (currentEnemy.transform.position, currentEnemy.Target.gameObject.transform.position) <= currentEnemy.MoveRange) {

												print ("Moving to target!");
												currentEnemy.transform.LookAt (currentEnemy.Target.gameObject.transform);
												currentEnemy.MoveCharacter (currentEnemy.Target.gameObject.transform.position - Vector3.forward * currentEnemy.AttackRange);
												currentEnemy.BasicAttack (currentEnemy.Target);
												mCurrentEnemy++;
												
										} else {

						currentEnemy.transform.LookAt (currentEnemy.Target.gameObject.transform);
						currentEnemy.MoveCharacter (currentEnemy.transform.position + Vector3.forward * currentEnemy.MoveRange);
						mCurrentEnemy++;

												}
			
								} else {

										for (int i = 0; i < Players.Count; i++) {



						if (Vector3.Distance (currentEnemy.transform.position, Players [i].transform.position) <= currentEnemy.AggroRange) {

							currentEnemy.Target = Players[i];

												}

										}

					if (currentEnemy.Target == null){

						mCurrentEnemy++;

					}

					
								}

								if (mCurrentEnemy > Enemies.Count - 1) {
										print ("Ending enemy phase!");
										mCurrentEnemy = 0;
										mEnemiesTurn = false;
										mPlayersTurn = true;
										Players [mCurrentPlayer].StartTurn ();
								}
						} else {
					
				mEnemiesTurn = false;
				mPlayersTurn = true;
				Players [mCurrentPlayer].StartTurn ();
								}
				}

		}

		//Rescans the pathing grid
		public void ScanPath ()
		{

				aStarPath.Scan ();

		}

		//Changes the pathing grids node size
		public void ChangeNodeSize (float size)
		{

				aStarPath.astarData.gridGraph.nodeSize = size;
				mTimeToScan = Time.time + mScanDelay;
				mNodesChanged = true;


		}

}

