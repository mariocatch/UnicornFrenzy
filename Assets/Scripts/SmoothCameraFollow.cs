using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public float DampAmount = 6.0f;
    public float MinimumDistance = 10.0f;
    public Transform PlayerTransform;
    public bool ShouldSmooth = true;
    private float mAlpha = 1.0f;
    private Color mColor;
    private Transform mThisTransform;
    

    private void Awake()
    {
        mThisTransform = transform;
    }

    private void LateUpdate()
    {
        if (PlayerTransform)
        {
            if (ShouldSmooth)
            {
                Quaternion rotation = Quaternion.LookRotation(PlayerTransform.position - mThisTransform.position);
                mThisTransform.rotation = Quaternion.Slerp(mThisTransform.rotation, rotation, Time.deltaTime*DampAmount);
                mThisTransform.position = Vector3.Slerp(transform.position, PlayerTransform.position + new Vector3(0f, (15 - PlayerTransform.position.y), -15.0f), Time.deltaTime*2.0f);
            }
            else
            {
                mThisTransform.rotation = Quaternion.FromToRotation(-Vector3.forward, (new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, PlayerTransform.position.z) - mThisTransform.position).normalized);
                mThisTransform.position = Vector3.Slerp(transform.position, PlayerTransform.position + new Vector3(0f, (15 - PlayerTransform.position.y), -15.0f), Time.deltaTime * 2.0f);

                float distance = Vector3.Distance(PlayerTransform.position, mThisTransform.position);

                if (distance < MinimumDistance)
                {
                    mAlpha = Mathf.Lerp(mAlpha, 0.0f, Time.deltaTime*2.0f);
                }
                else
                {
                    mAlpha = Mathf.Lerp(mAlpha, 1.0f, Time.deltaTime*2.0f);
                }
            }
        }
    }
}