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
	public Player Target;
	public int ImmobileTurns;
	public int HitReducer = 0;
	public int HitReducedTurns;
	public int DamageReducer = 0;
	public int DamageBonus;
	public int DamageReducedTurns;
	public int HitChance;
	public GameController mGameController;
	public AstarPath mAstarPath;

	public List<Item> Drops;

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
		mGameController.Enemies.Remove (this);

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
