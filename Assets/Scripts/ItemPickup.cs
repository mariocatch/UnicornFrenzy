using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

	public int ItemID;
	public int ItemQuantity;
	public string ItemName;
	public GameObject ItemPrefab;
	public ParticleSystem CollectedParticles;
	private GameController mGameController;

	void Start(){

		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		}
	
	public void RemoveObject(){
		
		Instantiate (CollectedParticles, gameObject.transform.position, gameObject.transform.rotation);
		gameObject.layer = 0;
		mGameController.ScanPath ();
		Destroy (gameObject);
		
	}
}
