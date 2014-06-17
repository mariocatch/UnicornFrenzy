using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateByExplore : MonoBehaviour {

	public bool SpawnerActive = true;
	public int mPositionOffset;
	public int mRoomOffset;
	public int mMaxWidth;
	public int mMaxHeight;
	public int mRoomWidth;

	private RoomDatabase roomDatabase;
	private GameController gameController;
	

	void OnTriggerStay(Collider other){

		//Check if a player is within the collider, and if their turn is over.
			//Each floor will have colliders set up across the edges that contain doors.
			//Each floor will need to hold information regarding it's own doors
		if (other.tag == "Enemy" && !other.GetComponent<AstarAI>().TurnActive && SpawnerActive) {

			if (transform.localPosition.x > mPositionOffset) {
				//Spawn Room at x- location
				SpawnNextRoom (gameObject.transform.parent.transform.position + new Vector3 (mRoomOffset, 0, 0));
				SpawnerActive = false;
			} else if (transform.localPosition.x < -mPositionOffset) {
				//Spawn Room at x- location
				SpawnNextRoom (gameObject.transform.parent.transform.position - new Vector3 (mRoomOffset, 0, 0));
				SpawnerActive = false;
			} else if (transform.localPosition.z > mPositionOffset) {
				//Spawn Room at z+ location
				SpawnNextRoom (gameObject.transform.parent.transform.position + new Vector3 (0, 0, mRoomOffset));
				SpawnerActive = false;
			} else if (transform.localPosition.z < -mPositionOffset) {
				//Spawn Room at z- location
				SpawnNextRoom (gameObject.transform.parent.transform.position - new Vector3 (0, 0, mRoomOffset));
				SpawnerActive = false;
			} else {

				print ("Something went wrong! Local position check failed!");

			}


			//Spawn next area
			//SpawnNextRoom ();

				}
	}

	void SpawnNextRoom(Vector3 roomPos){


		//Room has to be chosen based upon surrounding rooms information
		//Create variables to hold door information
		bool nDoor = false, sDoor = false, eDoor = false, wDoor = false;
		bool disableNDoor = false, disableSDoor = false, disableEDoor = false ,disableWDoor = false;
		bool nBlocked = false, sBlocked = false, eBlocked = false, wBlocked = false;
		roomDatabase = GameObject.FindGameObjectWithTag ("RoomDatabase").GetComponent<RoomDatabase> ();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		//Cast Rays at the centerpoint of the surrounding rooms
		//set local bools to these values

		RaycastHit nHit, sHit, eHit, wHit;

	
		if (Physics.Raycast (roomPos + new Vector3 (0, 1, mRoomOffset), Vector3.down, out nHit)) {
			print ("Checking Northern Room!");
			if (nHit.collider.tag == "Ground" ){
				print ("Hit Northern Room!");
				nDoor = nHit.collider.gameObject.GetComponent<RoomInfo>().SouthDoor;
				disableNDoor = true;
				if (!nDoor){
					//There is a room, but there is no way in
					nBlocked = true;
				}
			} 
		} else {
			print ("No Northern Room Found!");
		}

		if (Physics.Raycast (roomPos - new Vector3 (0, -1, mRoomOffset), Vector3.down, out nHit)) {
			print ("Checking Southern Room!");
			if (nHit.collider.tag == "Ground" ){
				print ("Hit Southern Room!");
				sDoor = nHit.collider.gameObject.GetComponent<RoomInfo>().NorthDoor;
				disableSDoor = true;
				if (!sDoor){
					//There is a room, but there is no way in
					sBlocked = true;
				}
			}
		} else {
			print ("No Southern Room Found!");
		}

		if (Physics.Raycast (roomPos + new Vector3 (mRoomOffset, 1, 0), Vector3.down, out nHit)) {
			print ("Checking Eastern Room!");
			if (nHit.collider.tag == "Ground" ){
				print ("Hit Eastern Room!");
				eDoor = nHit.collider.gameObject.GetComponent<RoomInfo>().WestDoor;
				disableEDoor = true;
				if (!eDoor){
					//There is a room, but there is no way in
					eBlocked = true;
				}
			}
		} else {
			print ("No Eastern Room Found!");
		}
	
		if (Physics.Raycast (roomPos - new Vector3 (mRoomOffset, -1, 0), Vector3.down, out nHit)) {
			print ("Checking Western Room!");
			if (nHit.collider.tag == "Ground" ){
				print ("Hit Western Room!");
				wDoor = nHit.collider.gameObject.GetComponent<RoomInfo>().EastDoor;
				disableWDoor = true;
				if (!wDoor){
					//There is a room, but there is no way in
					wBlocked = true;
				}
			}
		} else {
			print ("No Western Room Found!");
		}

		List<RoomInfo> PotentialSpawns = roomDatabase.Rooms;

		if (nDoor) {
						PotentialSpawns = PotentialSpawns.Where (x => x.NorthDoor == true).ToList();
				}
		if (sDoor) {
						PotentialSpawns = PotentialSpawns.Where (x => x.SouthDoor == true).ToList();
				}
		if (wDoor) {
						PotentialSpawns = PotentialSpawns.Where (x => x.WestDoor == true).ToList();
		}
		if (eDoor) {
						PotentialSpawns = PotentialSpawns.Where (x => x.EastDoor == true).ToList();
		}
		if (nBlocked) {
						PotentialSpawns = PotentialSpawns.Where (x => x.NorthDoor == false).ToList();
				}
		if (sBlocked) {
						PotentialSpawns = PotentialSpawns.Where (x => x.SouthDoor == false).ToList();
		}
		if (wBlocked) {
						PotentialSpawns = PotentialSpawns.Where (x => x.WestDoor == false).ToList();
		}
		if (eBlocked) {
						PotentialSpawns = PotentialSpawns.Where (x => x.EastDoor == false).ToList();
		}

		int RandomRoom = Random.Range (0, PotentialSpawns.Count);
		PotentialSpawns [RandomRoom].NorthExit.SpawnerActive = !disableNDoor;
		PotentialSpawns [RandomRoom].SouthExit.SpawnerActive = !disableSDoor;
		PotentialSpawns [RandomRoom].EastExit.SpawnerActive = !disableEDoor;
		PotentialSpawns [RandomRoom].WestExit.SpawnerActive = !disableWDoor;

		Instantiate (PotentialSpawns [RandomRoom].gameObject, roomPos, transform.rotation);
		gameController.ScanPath ();
		//search through room list for a room that matches these criteria
		//spawn room
		//room contents based upon spawning objects at random transforms
		//spawned enemies are added to the enemy turn list
		//spawned loot containers are filled with rewards
		//spawned doodads are marked as obstacles
		//Rescan the grid

	}

}
