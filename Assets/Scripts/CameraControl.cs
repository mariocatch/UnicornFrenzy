using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public int Boundary = 50;
	public int Speed = 50;

	private int mTheScreenWidth;
	private int mTheScreenHeight;
	private Vector3 mPosHolder;

	void Start(){

		mTheScreenWidth = Screen.width;
		mTheScreenHeight = Screen.height;

		}

	void Update(){

		if (Input.mousePosition.x > mTheScreenWidth - Boundary) {
			mPosHolder = transform.position;
			mPosHolder.x += Speed * Time.deltaTime;
			transform.position = mPosHolder;
				}

		if (Input.mousePosition.x < 0 + Boundary) {
			mPosHolder = transform.position;
			mPosHolder.x -= Speed * Time.deltaTime;
			transform.position = mPosHolder;
				}

		if (Input.mousePosition.y > mTheScreenHeight - Boundary) {
			mPosHolder = transform.position;
			mPosHolder.z += Speed * Time.deltaTime;
			transform.position = mPosHolder;
				}

		if (Input.mousePosition.y < 0 + Boundary) {
			mPosHolder = transform.position;
			mPosHolder.z -= Speed * Time.deltaTime;
			transform.position = mPosHolder;
				}

	}
}
