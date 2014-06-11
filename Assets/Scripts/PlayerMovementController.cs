using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MovementSpeed;
    private float mDestinationDistance;
    private Transform mPlayerTransform;
    private Vector3 mTargetDestination;


    private void Start()
    {
        mPlayerTransform = transform;
        mTargetDestination = mPlayerTransform.position;
    }

    private void Update()
    {
        mDestinationDistance = Vector3.Distance(mTargetDestination, mPlayerTransform.position);

        if (mDestinationDistance < .5f)
        {
            MovementSpeed = 0;
        }
        else if (mDestinationDistance > .5f)
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
        if (mDestinationDistance > .5f)
        {
            mPlayerTransform.position = Vector3.MoveTowards(mPlayerTransform.position, mTargetDestination, MovementSpeed*Time.deltaTime);
        }
    }
}