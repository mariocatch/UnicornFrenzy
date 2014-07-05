using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerAbility : MonoBehaviour {

	public Texture Icon;
	public int Range;
	public int Damage;
	public int APRecov;
	public int ApCost;
	public int StatusChange;
	public int StatusTurns;
	public ParticleSystem Particles;
	public enum AbilityType
		{
		EnemyTarget,
		AllyTarget,
		WorldTarget,
		Self
	};

	public AbilityType Type;


	public virtual void UseAbility(Player source, Player target, int modify1, int modify2){

	}

	public virtual void UseAbility(Player source, Vector3 targetLoc, int modify, int modify2,  GameController controller){

	}

	public virtual void UseAbility(Player source, int modify, int modify2, GameController controller){
		
	}

	public virtual void UseAbility(Player source, Enemy target, int modify, int modify2){

	}
}
