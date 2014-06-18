using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GameController : MonoBehaviour {

	public List <AstarAI> Players;
	public GameObject NewPlane;
	public Transform SpawnLocation;
	public AstarPath aStarPath;
	private float mTimeToScan;
	private float mScanDelay = .5f;
	private int mCurrentPlayer;
	private bool mNodesChanged;


	void Start(){

		//Starts the game with the first players turn
		Players [mCurrentPlayer].StartTurn ();

		}

	void Update(){

		//Checks if the nodes have been changed, waits for a brief moment, then scans the grid.
		if (mNodesChanged) {

			if (Time.time > mTimeToScan){

				ScanPath ();
				mNodesChanged = false;

			}

				}

		//Cycles through player turns
		if (Players [mCurrentPlayer].TurnActive == false) {

			mCurrentPlayer++;

			if (mCurrentPlayer > Players.Count - 1) {
				
				mCurrentPlayer = 0;
				
			}
			print (mCurrentPlayer);
			Players[mCurrentPlayer].StartTurn ();
			print (Players.Count);
				}

	}

	//Rescans the pathing grid
	public void ScanPath(){

		aStarPath.Scan ();

	}

	//Changes the pathing grids node size
	public void ChangeNodeSize(float size){

		aStarPath.astarData.gridGraph.nodeSize = size;
		mTimeToScan = Time.time + mScanDelay;
		mNodesChanged = true;


	}

}
