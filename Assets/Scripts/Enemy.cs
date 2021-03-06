﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public int Health;
	public float AttackRange;
	public float MoveRange;
	public float AggroRange;
	[HideInInspector]
	public bool TurnActive;
	public int BasicAttackDamage;
	[HideInInspector]
	public float TurnTime;
	[HideInInspector]
	public Player Target;
	[HideInInspector]
	public int ImmobileTurns;
	[HideInInspector]
	public int HitReducer = 0;
	[HideInInspector]
	public int HitReducedTurns;
	[HideInInspector]
	public int DamageReducer = 0;
	public int DamageBonus;
	[HideInInspector]
	public int DamageReducedTurns;
	public int HitChance;
	[HideInInspector]
	public GameController mGameController;
	[HideInInspector]
	public AstarPath mAstarPath;
	[HideInInspector]
	public List<Item> Drops;
	public ParticleSystem DeathParticles;
	public ParticleSystem BasicAttackParticles;

	public virtual void Start(){

		mGameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		mAstarPath = GameObject.FindGameObjectWithTag ("PathGen").GetComponent<AstarPath> ();
		mGameController.Enemies.Add (this);

		}

	public virtual void TakeDamage(int damage){

		Health -= damage;
		CheckHealth ();

	}

	public virtual void StartTurn(){

		TurnActive = true;

		if (DamageReducedTurns > 0) {

			DamageReducedTurns -= 1;
			print (DamageReducedTurns);

			if (DamageReducedTurns == 0){

				DamageReducer = 0;
				print (DamageReducer);
			}

				}
		if (HitReducedTurns > 0) {
			
			HitReducedTurns -= 1;
			
			if (HitReducedTurns == 0){
				
				HitReducer = 0;
				
			}
			
		}

		}

	public virtual void EndTurn(){

		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = false;
		TurnActive = false;

	}

	public virtual void CheckHealth(){

		if (Health <= 0) {

			Death ();

				}
		}

	public virtual void Death(){

		mAstarPath.astarData.gridGraph.GetNearest (transform.position).node.Walkable = true;
		mGameController.RemoveEnemy (this);
		Instantiate (DeathParticles, transform.position, transform.rotation);

		}

	public virtual void MoveCharacter (Vector3 target)
	{

	}

	public void ReduceDamage(int reduction, int turns){

		DamageReducer = reduction;
		DamageReducedTurns = turns;

		}

	public void ReduceHitChance(int reduction, int turns){

		HitReducer = reduction;
		HitReducedTurns = turns;

		}

}
