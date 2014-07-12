using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

	public int ItemID;
	public int ItemQuantity;
	public string ItemName;
	public GameObject ItemPrefab;
	public ParticleSystem CollectedParticles;
	
	public void RemoveObject(){
		
		Instantiate (CollectedParticles, gameObject.transform.position, gameObject.transform.rotation);
		Destroy (gameObject);
		
	}
}
