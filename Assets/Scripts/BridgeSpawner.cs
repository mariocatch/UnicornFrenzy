using UnityEngine;
using System.Collections;

public class BridgeSpawner : InteractiveObject {

	public Transform SpawnLoc;
	public GameObject Bridge;
	private bool mSpawned;
	private GameController mGameController;

	void Start(){

		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		}

	public override void UseObject ()
	{

		if (!mSpawned) {

			Instantiate (Bridge, SpawnLoc.position, SpawnLoc.rotation);
			mSpawned = true;
			mGameController.ScanPath ();

				}

	}


}
