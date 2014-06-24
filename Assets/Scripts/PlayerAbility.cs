using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerAbility : MonoBehaviour {

	public int Range;
	public int Damage;
	public int APRecov;
	public int ApCost;
	public int StatusChange;
	public int StatusTurns;
	public ParticleSystem Particles;


	public virtual void UseAbility(Player source, Player target, int modify){

	}

	public virtual void UseAbility(Player source, Vector3 targetLoc, int modify, GameController controller){

	}

	public virtual void UseAbility(Player source, int modify, GameController controller){
		
	}

	public virtual void UseAbility(Player source, Enemy target, int modify){

	}
}
