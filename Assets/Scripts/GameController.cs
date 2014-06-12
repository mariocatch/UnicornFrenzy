﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class GameController : MonoBehaviour {

	public List <AstarAI> Players;
	public GameObject NewPlane;
	public Transform SpawnLocation;
	public AstarPath aStarPath;
	private int mCurrentPlayer;


	void Start(){

		Players [mCurrentPlayer].StartTurn ();

		}

	void Update(){


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

	void ScanPath(){

		aStarPath.Scan ();

	}

	void ChangeNodeSize(float size){

		aStarPath.astarData.gridGraph.nodeSize = size;
		ScanPath ();

	}

}
