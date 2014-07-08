using UnityEngine;
using System.Collections;

public class CoverObject : MonoBehaviour {

	private GameController mGameController;

	void Start(){

		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		mGameController.CoverObjects.Add (this);

	}
	
	void OnTriggerEnter(Collider other){

		if (other.tag == "Player") {

			other.transform.gameObject.GetComponent<Player>().InCover = true;
			other.transform.gameObject.GetComponent<Player>().CurrentCover = this;
				}

		}

	void OnTriggerExit(Collider other){

		if (other.tag == "Player") {
			
			other.transform.gameObject.GetComponent<Player>().InCover = false;
			other.transform.gameObject.GetComponent<Player>().CurrentCover = null;
		}

	}
	

}
