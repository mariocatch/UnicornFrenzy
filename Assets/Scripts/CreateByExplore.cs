using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateByExplore : MonoBehaviour {

	public bool SpawnerActive;
	private int mPositionOffset = 3;
	private int mRoomOffset = 40;
	public int mMaxWidth;
	public int mMaxHeight;
	public int mRoomWidth;

	private RoomDatabase roomDatabase;
	private GameController gameController;
	

	void OnTriggerStay(Collider other){

		//Check if a player is within the collider, and if their turn is over.
			//Each floor will have colliders set up across the edges that contain doors.
			//Each floor will need to hold information regarding it's own doors
		if (other.tag == "Player" && SpawnerActive) {

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

		RaycastHit hit;

	
		if (Physics.Raycast (roomPos + new Vector3 (0, 1, mRoomOffset), Vector3.down, out hit)) {
			print ("Checking Northern Room!");
			if (hit.collider.tag == "Ground" ){
				print ("Hit Northern Room!");
				nDoor = hit.collider.gameObject.GetComponent<RoomInfo>().SouthDoor;
				disableNDoor = true;
				if (!nDoor){
					//There is a room, but there is no way in
					nBlocked = true;
				} else {
					hit.collider.gameObject.GetComponent<RoomInfo>().SouthExit.SpawnerActive = false;
				}
			} 
		} else {
			print ("No Northern Room Found!");
		}

		if (Physics.Raycast (roomPos - new Vector3 (0, -1, mRoomOffset), Vector3.down, out hit)) {
			print ("Checking Southern Room!");
			if (hit.collider.tag == "Ground" ){
				print ("Hit Southern Room!");
				sDoor = hit.collider.gameObject.GetComponent<RoomInfo>().NorthDoor;
				disableSDoor = true;
				if (!sDoor){
					//There is a room, but there is no way in
					sBlocked = true;
				} else {
					hit.collider.gameObject.GetComponent<RoomInfo>().NorthExit.SpawnerActive = false;
				}
			}
		} else {
			print ("No Southern Room Found!");
		}

		if (Physics.Raycast (roomPos + new Vector3 (mRoomOffset, 1, 0), Vector3.down, out hit)) {
			print ("Checking Eastern Room!");
			if (hit.collider.tag == "Ground" ){
				print ("Hit Eastern Room!");
				eDoor = hit.collider.gameObject.GetComponent<RoomInfo>().WestDoor;
				disableEDoor = true;
				if (!eDoor){
					//There is a room, but there is no way in
					eBlocked = true;
				}else {
					hit.collider.gameObject.GetComponent<RoomInfo>().WestExit.SpawnerActive = false;
				}
			}
		} else {
			print ("No Eastern Room Found!");
		}
	
		if (Physics.Raycast (roomPos - new Vector3 (mRoomOffset, -1, 0), Vector3.down, out hit)) {
			print ("Checking Western Room!");
			if (hit.collider.tag == "Ground" ){
				print ("Hit Western Room!");
				wDoor = hit.collider.gameObject.GetComponent<RoomInfo>().EastDoor;
				disableWDoor = true;
				if (!wDoor){
					//There is a room, but there is no way in
					wBlocked = true;
				}else {
					hit.collider.gameObject.GetComponent<RoomInfo>().EastExit.SpawnerActive = false;
				}
			}
		} else {
			print ("No Western Room Found!");
		}

		//search through room list and narrowdown to rooms that match the criteria
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
		//spawn room
		GameObject SpawnedRoom = Instantiate (PotentialSpawns [RandomRoom].gameObject, roomPos, transform.rotation) as GameObject;
		SpawnedRoom.GetComponent<RoomInfo>().SetExits ();

		//disable exit spawners as to not create rooms over exiting ones
		if (disableNDoor) {
			SpawnedRoom.GetComponent<RoomInfo>().NorthExit.SpawnerActive = false;
				}
		if (disableSDoor) {
			SpawnedRoom.GetComponent<RoomInfo>().SouthExit.SpawnerActive = false;
				}
		if (disableEDoor) {
			SpawnedRoom.GetComponent<RoomInfo>().EastExit.SpawnerActive = false;
				}
		if (disableWDoor) {
			SpawnedRoom.GetComponent<RoomInfo>().WestExit.SpawnerActive = false;
				}



		//Rescan the grid
		gameController.ScanPath ();

	}

}
