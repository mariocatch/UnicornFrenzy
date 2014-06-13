using UnityEngine;
using System.Collections;
using Pathfinding;

public class GenerateWorld : MonoBehaviour {

	public GameObject Room;
	public GameObject RoomDX;
	public GameObject RoomDZ;
	public GameObject TempFloor;
	private GameObject StartRoom;
	public Transform StartPoint;
	public GameController Controller;
	private Seeker mSeeker;
	public Path path;
	private bool mDistanceCheck;

	void Start(){

		mSeeker = GetComponent<Seeker> ();
		StartRoom = Instantiate(Room, StartPoint.position + new Vector3(Random.Range (-50f, 50f), 0, Random.Range (-50f, 50f)), StartPoint.rotation) as GameObject;
		Vector3 nextRoom = new Vector3(StartRoom.transform.position.x + Random.Range (-150f, 150f), 0, StartRoom.transform.position.z + Random.Range (-150f, 150f));
		while (!mDistanceCheck){

			if (Vector3.Distance (nextRoom, StartRoom.transform.position) > 100 && nextRoom.x < 100 && nextRoom.x > -100 && nextRoom.z < 100 && nextRoom.z > -100){

				mDistanceCheck = true;

			} else {

				nextRoom = new Vector3(StartRoom.transform.position.x + Random.Range (-150f, 150f), 0, StartRoom.transform.position.z + Random.Range (-150f, 150f));

			}

		}
		GeneratePath (StartRoom.transform.position, nextRoom);
	}

	public void GenerateRooms(){

		StartRoom.transform.position = path.vectorPath [0];
		for (int i = 1; i < path.vectorPath.Count; i++) {

			if (i+1 < path.vectorPath.Count){
			if (path.vectorPath[i].x != path.vectorPath[i-1].x && path.vectorPath[i+1].x != path.vectorPath[i].x ){
			Instantiate (RoomDX, path.vectorPath[i], StartRoom.transform.rotation);
			} else if (path.vectorPath[i].z != path.vectorPath[i-1].z && path.vectorPath[i+1].z != path.vectorPath[i].z){
			Instantiate (RoomDZ, path.vectorPath[i], StartRoom.transform.rotation);
				} else {
				Instantiate (Room, path.vectorPath[i], StartRoom.transform.rotation);
			}
			} else {
				Instantiate (Room, path.vectorPath[i], StartRoom.transform.rotation);
			}

				}
			Destroy (TempFloor);
			Controller.ChangeNodeSize (2);
		}
	
	public void GeneratePath (Vector3 start, Vector3 target)
	{
		mSeeker.StartPath (start, target, OnPathComplete);
	}
	
	public void OnPathComplete (Path p)
	{
		//Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			GenerateRooms();
		}
	}

	//Create 'End' Tile
	//Create a random point X dinstance away, within the grid
	//Instantiate rooms at each vector along the path
	//Create another random point, x dinstance away from the current one, and the end point, within the grid
	//Instantiate rooms at each vector along that path
	//Instantiate the 'start' room at the end of the path
	//change the node size from 10 to 4 (aStarPath.astarData.gridGraph.nodeSize = size;)
	//rescan the map
	//spawn the players into the start room
	//begin the game
}
