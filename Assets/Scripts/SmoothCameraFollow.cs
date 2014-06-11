using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public float BackwardsDistanceFromPlayer = -15.0f;
    public float DampAmount = 6.0f;
    public float ElevatedDistanceFromPlayer = 15.0f;
    public Transform PlayerTransform;
    private Transform mThisTransform;


    private void Awake()
    {
        mThisTransform = transform;
    }

    private void LateUpdate()
    {
        if (PlayerTransform != null)
        {
            Quaternion rotation = Quaternion.LookRotation(PlayerTransform.position - mThisTransform.position);
            mThisTransform.rotation = Quaternion.Slerp(mThisTransform.rotation, rotation, Time.deltaTime*DampAmount);
            mThisTransform.position = Vector3.Slerp(transform.position, PlayerTransform.position + new Vector3(0f, (15 - PlayerTransform.position.y), -15.0f), Time.deltaTime*2.0f);
        }
    }
}