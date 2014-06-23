using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerAbility : MonoBehaviour {

	public int Range;
	public ParticleSystem Particles;


	public virtual void UseAbility(Player target, int modify){

	}

	public virtual void UseAbility(Vector3 targetLoc, int modify, GameController controller){

	}

	public virtual void UseAbility(Enemy target, int modify){

	}
}
