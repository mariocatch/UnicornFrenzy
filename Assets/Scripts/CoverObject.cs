using UnityEngine;
using System.Collections;

public class CoverObject : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {

			other.transform.gameObject.GetComponent<Player>().InCover = true;

				}

		}

	void OnTriggerExit(Collider other){

		if (other.tag == "Player") {
			
			other.transform.gameObject.GetComponent<Player>().InCover = false;
			
		}

	}
	

}
