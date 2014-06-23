using UnityEngine;
using System.Collections;

public class PlayerEngineer : Player {

	public int AttackRange = 5;

	// Use this for initialization
public override void Start ()
	{
		base.Start ();
		Ability1 = Abilities [0];
		Ability2 = Abilities [1];

	}
	
	// Update is called once per frame
public override void Update ()
	{
		base.Update ();
		//Attack Phase should be defined for each hero, as this is largely where they differ (apart from stats)

		if (AttackPhase) {
			
			
			//Selects a target to attack. Target must be labeled as an enemy.
			if (Input.GetMouseButtonDown (0) && GUIUtility.hotControl == 0 && TurnActive && !MovePhase) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject.tag == "Enemy") {
						
						mPlayerTarget = hit.collider.gameObject;
						print ("target aquired!");
					}
				}
				
			}
			
			//Attacks the currently targeted enemy and ends the turn
			if (mPlayerTarget != null && Input.GetButtonDown ("Attack") && Vector3.Distance (gameObject.transform.position, mPlayerTarget.transform.position) <= Ability1.Range) {
				
				Ability1.UseAbility(this, mPlayerTarget.GetComponent<Enemy>(), Technology);
				EndTurn ();
			}

			if (Input.GetButtonDown ("Attack2")){

					mSelectLocation = !mSelectLocation;

			}
		}
	}

	public override void FixedUpdate ()
	{
		base.FixedUpdate ();
	}
}
