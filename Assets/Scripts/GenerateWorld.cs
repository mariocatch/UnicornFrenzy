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

		//Creates a random 'start' tile from which to branch out
		StartRoom = Instantiate(Room, StartPoint.position + new Vector3(Random.Range (-50f, 50f), 0, Random.Range (-50f, 50f)), StartPoint.rotation) as GameObject;

		//Creates an 'end point' for the branch path
		Vector3 nextRoom = new Vector3(StartRoom.transform.position.x + Random.Range (-150f, 150f), 0, StartRoom.transform.position.z + Random.Range (-150f, 150f));
		//Makes sure the end room doesn't spawn too close
		while (!mDistanceCheck){

			if (Vector3.Distance (nextRoom, StartRoom.transform.position) > 100 && nextRoom.x < 100 && nextRoom.x > -100 && nextRoom.z < 100 && nextRoom.z > -100){

				mDistanceCheck = true;

			} else {

				nextRoom = new Vector3(StartRoom.transform.position.x + Random.Range (-150f, 150f), 0, StartRoom.transform.position.z + Random.Range (-150f, 150f));

			}

		}
		//Creates the path
		GeneratePath (StartRoom.transform.position, nextRoom);
	}

	public void GenerateRooms(){

		//Ensures that the 'startroom' is snapped to the grid
		StartRoom.transform.position = path.vectorPath [0];

		//Generates rooms point by point along the path, currently using certain rooms depending on position (will need to be largely altered before final use)
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

			//Removes the tempfloor (required for creating the path) and changes the nodesize to 2 (for player pathing) then rescans the grid
			Destroy (TempFloor);
			Controller.ChangeNodeSize (3);
		}
	
	public void GeneratePath (Vector3 start, Vector3 target)
	{
		mSeeker.StartPath (start, target, OnPathComplete);
	}
	
	public void OnPathComplete (Path p)
	{
		if (!p.error) {
			path = p;

			//Creates rooms along the returned path
			GenerateRooms();
		}
	}
}
