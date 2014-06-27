using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoomInfo : MonoBehaviour {

	public bool NorthDoor, SouthDoor, EastDoor, WestDoor, DeadEnd, EndRoom;

	public CreateByExplore NorthExit, SouthExit, EastExit, WestExit;

	public List<Transform> EnemySpawns;
	public List<Transform> ItemSpawns;
	public List<Transform> TrapSpawns;

	public enum EndRoomType
		{
			Item,
			MiniBoss,
			Boss,
			Mix
		};
	public EndRoomType RoomType;

	private SpawnablesDatabase mSpawnablesDatabase;

	public bool HasTrap;

	void Start(){


		mSpawnablesDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnablesDatabase>();

		//Checks to see if the room contains enemies items and trap locations before spawning them in
		if (EnemySpawns.Count != 0) {
			SpawnEnemies (mSpawnablesDatabase.SmallEnemies);
				}
		if (ItemSpawns.Count != 0) {
			//SpawnItems ();
				}
		if (TrapSpawns.Count != 0) {
			// SpawnTrap ();
				}
		if (EndRoom) {
			// Spawn End Room special based on type

			switch (RoomType){

			case EndRoomType.Boss:
				SpawnEnemies (mSpawnablesDatabase.Bosses);
				break;
			default:
				break;
				}

				}
	}

	public void SetExits(){

		NorthExit.SpawnerActive = NorthDoor;
		SouthExit.SpawnerActive = SouthDoor;
		EastExit.SpawnerActive = EastDoor;
		WestExit.SpawnerActive = WestDoor;

		}
	
	void SpawnEnemies(List<GameObject> enemies){

		
		int numEnemies = Random.Range(0, EnemySpawns.Count);
		List<Transform> PossibleSpawnLocs = new List<Transform>();
		PossibleSpawnLocs = EnemySpawns;

		for (int i = 0; i < numEnemies + 1; i++) {
				
				int enemSpawnLoc = Random.Range(0, PossibleSpawnLocs.Count);
			Instantiate ( enemies[Random.Range(0, enemies.Count)], PossibleSpawnLocs[enemSpawnLoc].position, PossibleSpawnLocs[enemSpawnLoc].rotation);
				PossibleSpawnLocs.RemoveAt(enemSpawnLoc);
				}	
	}
	
	void SpawnTrap(){
		
		if (HasTrap){

			//checks if this room is capable of spawning traps (set in inspector when desired) and spawns it randomly
			int spawnTrap = Random.Range(0, 2);

			
			if (spawnTrap == 1){
				
				int trapLoc = Random.Range(0, TrapSpawns.Count);
				//Spawns a random trap from the trap list in a random trap location within the room
				Instantiate (mSpawnablesDatabase.SpawnableTraps[Random.Range(0, mSpawnablesDatabase.SpawnableTraps.Count)], TrapSpawns[trapLoc].position, TrapSpawns[trapLoc].rotation);			
				
			}
			
		}
		
	}

	void SpawnItems(){

			
			int spawnItem = Random.Range(0, 2);
			
			
			if (spawnItem == 1){
				
				int ItemLoc = Random.Range(0, TrapSpawns.Count);
			//Spawns a random item from the item list in a random item location within the room	
			Instantiate (mSpawnablesDatabase.SpawnableItems[Random.Range(0, mSpawnablesDatabase.SpawnableItems.Count)], ItemSpawns[ItemLoc].position, ItemSpawns[ItemLoc].rotation);			
				
			}
			
		}
	

}
