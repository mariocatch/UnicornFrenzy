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
										if (Enemies.Count > 0) {

												Enemies [0].StartTurn ();
												print ("Enemy Turn!");

										}
				
								} else {
										Players [mCurrentPlayer].StartTurn ();
								}
						}
				}
				if (mEnemiesTurn) {



						if (Enemies.Count > 0) {

								if (Enemies [mCurrentEnemy].TurnActive == false) {

										mCurrentEnemy ++;
										if (mCurrentEnemy < Enemies.Count) {

												Enemies [mCurrentEnemy].StartTurn ();

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
								mCurrentEnemy = 0;
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

