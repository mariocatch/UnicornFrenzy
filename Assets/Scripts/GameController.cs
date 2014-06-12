using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public List <AstarAI> Players;
	private int mCurrentPlayer;


	void Start(){

		Players [mCurrentPlayer].StartTurn ();

		}

	void Update(){


		if (Players [mCurrentPlayer].turnActive == false) {

			mCurrentPlayer++;

			if (mCurrentPlayer > Players.Count - 1) {
				
				mCurrentPlayer = 0;
				
			}
			print (mCurrentPlayer);
			Players[mCurrentPlayer].StartTurn ();
			print (Players.Count);
				}


	}

}
