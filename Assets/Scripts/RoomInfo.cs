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

		if (EnemySpawns.Count != 0 && ItemSpawns.Count != 0 && TrapSpawns.Count != 0) {
						SpawnEnemies ();
						SpawnItems ();
						SpawnTrap ();
				}
	}
	
	void SpawnEnemies(){
		
		int enemSpawnLoc = Random.Range(0, EnemySpawns.Count);
		
		int numEnemies = Random.Range(1, 3);
		
		if (numEnemies > 1){
			
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[enemSpawnLoc].position, EnemySpawns[enemSpawnLoc].rotation);
			
			int nextSpawnLoc = Random.Range(0, EnemySpawns.Count);
			
			while (nextSpawnLoc == numEnemies){
				
				nextSpawnLoc = Random.Range(0, EnemySpawns.Count);
				
			}
			
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[nextSpawnLoc].position, EnemySpawns[nextSpawnLoc].rotation);
			
		} else {
			
			Instantiate ( mSpawnablesDatabase.SmallEnemies[Random.Range(0, mSpawnablesDatabase.SmallEnemies.Count)], EnemySpawns[enemSpawnLoc].position, EnemySpawns[enemSpawnLoc].rotation);
			
		}
		
	}
	
	void SpawnTrap(){
		
		if (HasTrap){

			int spawnTrap = Random.Range(0, 2);

			
			if (spawnTrap == 1){
				
				int trapLoc = Random.Range(0, TrapSpawns.Count);
				
				Instantiate (mSpawnablesDatabase.SpawnableTraps[Random.Range(0, mSpawnablesDatabase.SpawnableTraps.Count)], TrapSpawns[trapLoc].position, TrapSpawns[trapLoc].rotation);			
				
			}
			
		}
		
	}

	void SpawnItems(){

			
			int spawnItem = Random.Range(0, 2);
			
			
			if (spawnItem == 1){
				
				int ItemLoc = Random.Range(0, TrapSpawns.Count);
				
			Instantiate (mSpawnablesDatabase.SpawnableItems[Random.Range(0, mSpawnablesDatabase.SpawnableItems.Count)], ItemSpawns[ItemLoc].position, ItemSpawns[ItemLoc].rotation);			
				
			}
			
		}
	

}
