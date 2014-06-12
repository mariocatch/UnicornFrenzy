using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class AstarAI : MonoBehaviour {

	public Vector3 targetPosition;

	private Seeker seeker;

	public Path path;

	private float speed = 5;

	private bool moving;

	private float startTime;
	private float journeyLength;
	

	private int currentWaypoint = 0;

	public void Start() {

		seeker = GetComponent<Seeker> ();
		}

	public void Update(){
		if (Input.GetMouseButtonDown(0)&& GUIUtility.hotControl ==0) {
			
			Plane playerPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;
			
			if (playerPlane.Raycast(ray, out hitdist)) {
				Vector3 targetPoint = ray.GetPoint(hitdist);
				print (targetPoint);
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
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			return;
		}
		
		//move character to current waypoint
		if (!moving) {

			StartCoroutine(move(path.vectorPath [currentWaypoint]));

				} else if (moving) {
			if (transform.position == path.vectorPath[currentWaypoint]){
				moving = false;
				currentWaypoint++;

			}
				}

		}

	public IEnumerator move(Vector3 destination) {
		moving = true;
		float t = 0;
		Vector3 startPosition = transform.position;
		
		while (t < 1f) {
			t += Time.deltaTime * (speed) * 5;
			transform.position = Vector3.Lerp(transform.position, destination, t);
			yield return null;
		}
		yield return 0;
	}
}

