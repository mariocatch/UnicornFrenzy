using UnityEngine;
using System.Collections;
using Pathfinding;

public class RoomPathController : MonoBehaviour {

	private Seeker mSeeker;
	public Path path;

	// Use this for initialization
	void Start () {
	
		mSeeker = GetComponent<Seeker> ();

	}

	public void GeneratePath (Vector3 target)
	{
		print (target);
		mSeeker = GetComponent<Seeker> ();
		mSeeker.StartPath (transform.position, target, OnPathComplete);
	}
	
	public void OnPathComplete (Path p)
	{
		//Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
		}
	}

	public Path GetCurrentPath(Vector3 target){

		GeneratePath (target);

		if (path != null) {
						return path;
				} else {
			print ("returning null");
			return null;
				}
		}
}
