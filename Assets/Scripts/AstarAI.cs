using System.Collections;
using Pathfinding;
using UnityEngine;

public class AstarAI : MonoBehaviour
{
    private const float speed = 5;
    private int mCurrentWaypoint;
    private float mJourneyLength;
    private bool mMoving;
    private Seeker mSeeker;
	private float mStartTime;
	private float mTurnTime;
	private GameObject mPlayerTarget;
    public Path path;
	public int Health;
	public float AttackRange;
	public float TurnLimit;
    public Vector3 targetPosition;
	public bool turnActive, movePhase, attackPhase;

    public void Start()
    {
        mSeeker = GetComponent<Seeker>();
    }

    public void Update()
    {
		if (Health <= 0) {

			print ("I am dead!");
			Destroy (gameObject);

				}

		/*if (Time.time > mTurnTime) {

			EndTurn ();

				}*/

        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0 && turnActive && movePhase)
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                print(targetPoint);
                MoveCharacter(targetPoint);
				movePhase = false;
				attackPhase = true;
            }
        }

		if (attackPhase) {

						if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && turnActive && !movePhase) {
								Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
								RaycastHit hit;
								if (Physics.Raycast(ray, out hit)) {
										if (hit.collider.gameObject.tag == "Enemy"){

											mPlayerTarget = hit.collider.gameObject;
											print ("target aquired!");
										}
								}

						}

						if (mPlayerTarget != null && Input.GetButtonDown("Attack") && Vector3.Distance (gameObject.transform.position, mPlayerTarget.transform.position) < AttackRange){

								mPlayerTarget.GetComponent<AstarAI>().Health -= 10;
								print ("Attacking!");
								attackPhase = false;
								mPlayerTarget = null;
						}
				}

		if (turnActive && Input.GetMouseButtonDown (1)) {

			EndTurn ();

				}
    }

	public void StartTurn(){

		mTurnTime = Time.time + TurnLimit;
		turnActive = true;
		movePhase = true;
		print ("starting turn!");

		}

	public void EndTurn(){

		turnActive = false;
		movePhase = false;
		attackPhase = false;
		print ("Took too long! Ending turn!");
		}

    public void MoveCharacter(Vector3 target)
    {
        mSeeker.StartPath(transform.position, target, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;

            mCurrentWaypoint = 0;
        }
    }

    public void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (mCurrentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
		
        if (!mMoving)
        {
            iTween.MoveTo(gameObject, path.vectorPath[mCurrentWaypoint] + new Vector3(0, 1, 0), 2f);
            mMoving = true;
        }
        else if (mMoving)
        {
            if (Vector3.Distance (transform.position, path.vectorPath[mCurrentWaypoint] + new Vector3(0, 1, 0)) < 3f && mCurrentWaypoint < path.vectorPath.Count - 1)
            {
                mMoving = false;
                mCurrentWaypoint++;
            } else if (transform.position == path.vectorPath[mCurrentWaypoint] + new Vector3(0, 1, 0) && mCurrentWaypoint == path.vectorPath.Count -1){

				mMoving= false;
				mCurrentWaypoint++;
			}
        }
    }
}