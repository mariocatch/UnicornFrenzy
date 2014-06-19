using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public int Health;
	public float AttackRange;
	public float MoveRange;
	public float AggroRange;
	public int BasicAttackDamage;
	public AstarAI Target;

	public List<Item> Drops;

	public virtual void MoveCharacter (Vector3 target)
	{

	}

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BasicAttack(AstarAI target){
		
		
		target.Health -= BasicAttackDamage;
		print ("Attacked for " + BasicAttackDamage);
		
		
	}
}
