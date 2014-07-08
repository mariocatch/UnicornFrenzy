using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System.Linq;

public class GameController : MonoBehaviour
{

		public GUISkin skin;
		public List <Player> Players;
		public List <Enemy> Enemies;
		public List <CoverObject> CoverObjects;
		private List<Player> ActivePlayers;
		public GameObject NewPlane;
		public Transform SpawnLocation;
		public AstarPath aStarPath;
		private float mTextFadeTime;
		private float mTimeToScan;
		private float mScanDelay = .5f;
		private int mCurrentPlayer;
		private int mCurrentEnemy;
		public int RoomsNeeded;
		[HideInInspector]
		public int RoomsSpawned;
		[HideInInspector]
		public bool EndRoomSpawned;
		private bool mNodesChanged;
		private bool mPlayersTurn = true;
		private bool mEnemiesTurn;
		private bool mGameOver;
		[HideInInspector]
		public bool mPlayerSelected;
		public bool mGameWon;

		void Start ()
		{

				//Starts the game with the first players turn
				ActivePlayers = new List<Player> ();
				AddPlayers();
				mTextFadeTime = Time.time + 4;

				mPlayersTurn = true;

		}

		void Update ()
		{
		  
				if (Players.Count == 0) {
					
						if (!mGameOver) {
								mGameOver = true;
						} else {
								return;
						}
				}
				//Checks if the nodes have been changed, waits for a brief moment, then scans the grid.
				if (mNodesChanged) {

						if (Time.time > mTimeToScan) {

								ScanPath ();
								mNodesChanged = false;

						}

				}

				if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && mPlayersTurn && !mPlayerSelected) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit)) {
						if (hit.collider.gameObject.tag == "Player") {	
							Player SelectedPlayer = hit.collider.gameObject.GetComponent<Player>();
					if (PlayerInActiveList(SelectedPlayer)){
							mPlayerSelected = true;
							SelectedPlayer.StartTurn ();
					}
						} 
					}
				}
				/*if (mPlayersTurn) {
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
				}*/
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
										AddPlayers();
										mPlayersTurn = true;
										mTextFadeTime = Time.time + 3;
										//Players [mCurrentPlayer].StartTurn ();
								} 
						} else {
								mCurrentEnemy = 0;
								mEnemiesTurn = false;
								AddPlayers();
								mPlayersTurn = true;
								mTextFadeTime = Time.time + 3;
								//Players [mCurrentPlayer].StartTurn ();
								
						}
				}

		}

		void OnGUI(){

		if (GUI.Button (new Rect (Screen.width - 150, 80, 140, 20), "End All", skin.button) && mPlayersTurn) {

			print ("ending all!");
			EndAllTurn();

				}
		for (int i=0; i < Players.Count; i++) {
			GUI.Box(new Rect(Screen.width - 150, 100 + (i * 145), 140, 140), new GUIContent(Players[i].name + "\n" + "HP: " + Players[i].Health + "\n" + "AP: " + Players[i].ActionPoints), skin.box);
				}  
		if (mPlayersTurn && mTextFadeTime > Time.time) {

			GUI.Label (new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 50, 300, 100), "Your Turn", skin.GetStyle("PopUpText"));

				}

		if (mGameOver) {

						if (GUI.Button (new Rect ((Screen.width / 2) - 50, (Screen.height / 2) - 50, 200, 300), "Restart?")) {

								Application.LoadLevel (0);

						}
				}
		
		if (mGameWon) {

				if (GUI.Button (new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 50, 200, 300), "You win! Restart?")){
					
					Application.LoadLevel (0);
					
				}

			}


		}

		public void AddPlayers(){

		for (int i = 0; i < Players.Count; i++) {
			
			ActivePlayers.Add (Players[i]);
			
		}

		}

		public void EndPlayerTurn(Player currentPlayer){

		mPlayerSelected = false;
		ActivePlayers.Remove (currentPlayer);
		if (ActivePlayers.Count == 0) {

			mPlayersTurn = false;
			mEnemiesTurn = true;
			if (Enemies.Count > 0) {
				
				Enemies [0].StartTurn ();
				print ("Enemy Turn!");
				
			}

				}
		}

		public void EndAllTurn(){
		if (!mPlayerSelected) {

			ActivePlayers.Clear ();
			mPlayersTurn = false;
			mEnemiesTurn = true;
			if (Enemies.Count > 0) {
				
				Enemies [0].StartTurn ();
				print ("Enemy Turn!");
				
			}

				}


		}

		public bool PlayerInActiveList(Player selectedPlayer){

		if (selectedPlayer == ActivePlayers.FirstOrDefault (x => x == selectedPlayer)) {

						return true;

				} else {

						return false;
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

		public void RemoveEnemy (Enemy enemy){

		if (mEnemiesTurn && mCurrentEnemy != 0) {
						Enemies.Remove (enemy);
						mCurrentEnemy -= 1;
				} else {
						Enemies.Remove (enemy);
				}

		}

}

