using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class AstarAI : MonoBehaviour {

	public Vector3 targetPosition;

	private Seeker seeker;
	private CharacterController controller;

	public Path path;

	public float speed = 1;

	public float nextWaypointDistance = 0;

	private int currentWaypoint = 0;

	public void Start() {

		seeker = GetComponent<Seeker> ();
		controller = GetComponent<CharacterController> ();

		}

	public void Update(){
		if (Input.GetMouseButtonDown(0)&& GUIUtility.hotControl ==0) {
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;
			
			if (playerPlane.Raycast(ray, out hitdist)) {
				Vector3 targetPoint = ray.GetPoint(hitdist);
				MoveCharacter (targetPoint);
			}
		}

		}

	public void MoveCharacter(Vector3 target){

		seeker.StartPath (transform.position, target, OnPathComplete);

		}

	public void OnPathComplete (Path p){

		Debug.Log ("Yay, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
		
			path = p;

			currentWaypoint = 0;
		
		}

	}

	public void FixedUpdate() {

				if (path == null) {
						return;
				}

				if (currentWaypoint >= path.vectorPath.Count) {
						Debug.Log ("End Of Path Reached");
						return;
				}
				//Direction to the next waypoint
				Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
				//dir *= speed * Time.fixedDeltaTime;
				controller.SimpleMove (dir);
		print ("wtf?");
		
				//Check if we are close enough to the next waypoint
				//If we are, proceed to follow the next waypoint
				if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
						currentWaypoint++;
						return;
				}
		}

}
