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
    public Path path;
    public Vector3 targetPosition;

    public void Start()
    {
        mSeeker = GetComponent<Seeker>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                print(targetPoint);
                MoveCharacter(targetPoint);
            }
        }
    }

    public void MoveCharacter(Vector3 target)
    {
        mSeeker.StartPath(transform.position, target, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
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
            //We have no path to move after yet
            return;
        }

        if (mCurrentWaypoint >= path.vectorPath.Count)
        {
            Debug.Log("End Of Path Reached");
            return;
        }

        //move character to current waypoint
        if (!mMoving)
        {
            iTween.MoveTo(gameObject, path.vectorPath[mCurrentWaypoint] + new Vector3(0, 1, 0), .5f);
            mMoving = true;
        }
        else if (mMoving)
        {
            if (transform.position == path.vectorPath[mCurrentWaypoint] + new Vector3(0, 1, 0))
            {
                mMoving = false;
                mCurrentWaypoint++;
            }
        }
    }

    public IEnumerator move(Vector3 destination)
    {
        mMoving = true;
        float t = 0;
        Vector3 startPosition = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime*(speed)*5;
            transform.position = Vector3.Lerp(transform.position, destination, t);
            yield return null;
        }
        yield return 0;
    }
}