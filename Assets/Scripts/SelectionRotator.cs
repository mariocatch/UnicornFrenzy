using UnityEngine;
using System.Collections;

public class SelectionRotator : MonoBehaviour {


	// Update is called once per frame
	void Update () {
	
		transform.Rotate (0, 30 * Time.deltaTime, 0);

	}
}
