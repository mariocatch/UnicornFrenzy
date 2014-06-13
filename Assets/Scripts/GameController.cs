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

		Players [mCurrentPlayer].StartTurn ();

		}

	void Update(){

		if (mNodesChanged) {

			if (Time.time > mTimeToScan){

				ScanPath ();
				mNodesChanged = false;

			}

				}


		if (Players [mCurrentPlayer].TurnActive == false) {

			mCurrentPlayer++;

			if (mCurrentPlayer > Players.Count - 1) {
				
				mCurrentPlayer = 0;
				
			}
			print (mCurrentPlayer);
			Players[mCurrentPlayer].StartTurn ();
			print (Players.Count);
				}

		if (Input.GetButtonUp ("SpawnPlane")){

			GameObject newPlane = Instantiate(NewPlane, SpawnLocation.position, SpawnLocation.rotation) as GameObject;
			ChangeNodeSize (10);
		}

	}

	public void ScanPath(){

		aStarPath.Scan ();

	}

	public void ChangeNodeSize(float size){

		aStarPath.astarData.gridGraph.nodeSize = size;
		mTimeToScan = Time.time + mScanDelay;
		mNodesChanged = true;


	}

}
