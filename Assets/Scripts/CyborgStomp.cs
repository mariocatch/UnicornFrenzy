using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CyborgStomp : PlayerAbility {

	public int BlastRange;
	
	private List<Enemy> mEnemyTargets = new List<Enemy>();
	
	
	public override void UseAbility (Player source, int modify1, int modify2, GameController controller)
	{
		
		Instantiate (Particles, source.transform.position, Quaternion.identity);
		
		for (int i = 0; i < controller.Enemies.Count; i++) {
			
			mEnemyTargets.Add (controller.Enemies[i]);
			
		}
		
		
		for (int j=0; j < mEnemyTargets.Count; j++) {
			
			if (Vector3.Distance (mEnemyTargets[j].transform.position, source.transform.position) <= BlastRange){
				
				mEnemyTargets[j].TakeDamage(Damage);
			}
			
		}
		
		mEnemyTargets.Clear ();
		source.ActionPoints -= ApCost;
		
	}
}
