using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private float DestinationDistance;
    public float MovementSpeed;
    private Transform mPlayerTransform;
    private Vector3 mTargetDestination;


    private void Start()
    {
        mPlayerTransform = transform;
        mTargetDestination = mPlayerTransform.position;
    }

    private void Update()
    {
        DestinationDistance = Vector3.Distance(mTargetDestination, mPlayerTransform.position);

        if (DestinationDistance < .5f)
        {
            MovementSpeed = 0;
        }
        else if (DestinationDistance > .5f)
        {
            MovementSpeed = 3;
        }


        // Moves the Player if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            var playerPlane = new Plane(Vector3.up, mPlayerTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                mTargetDestination = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                mPlayerTransform.rotation = targetRotation;
            }
        }

        //    // Moves the player if the mouse button is hold down
        //else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        //{
        //    var playerPlane = new Plane(Vector3.up, mPlayerTransform.position);
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    float hitdist = 0.0f;

        //    if (playerPlane.Raycast(ray, out hitdist))
        //    {
        //        Vector3 targetPoint = ray.GetPoint(hitdist);
        //        mTargetDestination = ray.GetPoint(hitdist);
        //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        //        mPlayerTransform.rotation = targetRotation;
        //    }
        //    //	mPlayerTransform.position = Vector3.MoveTowards(mPlayerTransform.position, mTargetDestination, moveSpeed * Time.deltaTime);
        //}

        // To prevent code from running if not needed
        if (DestinationDistance > .5f)
        {
            mPlayerTransform.position = Vector3.MoveTowards(mPlayerTransform.position, mTargetDestination, MovementSpeed*Time.deltaTime);
        }
    }
}