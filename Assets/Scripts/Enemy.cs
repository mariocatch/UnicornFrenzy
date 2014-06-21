using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public int Health;
	public float AttackRange;
	public float MoveRange;
	public float AggroRange;
	public bool TurnActive;
	public int BasicAttackDamage;
	public AstarAI Target;

	public List<Item> Drops;

	public virtual void TakeDamage(int damage){



	}

	public virtual void StartTurn(){


		}

	public virtual void MoveCharacter (Vector3 target)
	{

	}

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
