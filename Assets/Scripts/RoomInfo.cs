using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoomInfo : MonoBehaviour {

	public bool NorthDoor, SouthDoor, EastDoor, WestDoor, DeadEnd;

	public CreateByExplore NorthExit, SouthExit, EastExit, WestExit;

	public List<Transform> EnemySpawns;
	public List<Transform> ItemSpawns;
	public List<Transform> TrapSpawns;

	private SpawnablesDatabase mSpawnablesDatabase;

	public bool HasTrap;

	void Start(){


		mSpawnablesDatabase = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnablesDatabase>();

		//Checks to see if the room contains enemies items and trap locations before spawning them in
		if (EnemySpawns.Count != 0) {
			SpawnEnemies ();
				}
		if (ItemSpawns.Count != 0) {
			SpawnItems ();
				}
		if (TrapSpawns.Count != 0) {
			SpawnTrap ();
				}
	}

	public void SetExits(){

		NorthExit.SpawnerActive = NorthDoor;
		SouthExit.SpawnerActive = SouthDoor;
		EastExit.SpawnerActive = EastDoor;
		WestExit.SpawnerActive = WestDoor;

		}
	
	void SpawnEnemies(){
		
		int enemSpawnLoc = Random.Range(0, EnemySpawns.Count);
		
		int numEnemies = Random.Range(1, 3);

		//If the number of enemies is greater than 1 (only 2 possible at the time of this comment) it will ensure that they don't spawn atop one another
		if (numEnemies > 1){

			//Spawns a random enemy from the enemy list in a random enemy location within the room
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[enemSpawnLoc].position, EnemySpawns[enemSpawnLoc].rotation);
			
			int nextSpawnLoc = Random.Range(0, EnemySpawns.Count);
			
			while (nextSpawnLoc == numEnemies){
				
				nextSpawnLoc = Random.Range(0, EnemySpawns.Count);
				
			}
			//Spawns a random enemy from the enemy list in a random enemy location within the room, that isn't the same as the previous one
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[nextSpawnLoc].position, EnemySpawns[nextSpawnLoc].rotation);
			
		} else {

			//Spawns a random enemy from the enemy list in a random enemy location within the room
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[enemSpawnLoc].position, EnemySpawns[enemSpawnLoc].rotation);
			
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
