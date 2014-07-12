using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerInventory : MonoBehaviour {

	public int TotalCredits;
	public int CurrentCredits;
	public ItemDatabase IDatabase;

	public List<Item> PlayerItems = new List<Item> ();
	

	public void AddItem(int ItemID, int ItemQuantity){

		if (ItemID != 0) {

			Item itemToAdd = IDatabase.Items.FirstOrDefault (x => x.ItemID == ItemID);

			Item itemInInventory = PlayerItems.FirstOrDefault (x => x.ItemID == ItemID);
			if (itemInInventory != null){

				itemInInventory.ItemQuantity += ItemQuantity;

			} else {

				PlayerItems.Add (itemToAdd);

			}

				}

		CurrentCredits += ItemQuantity;
		TotalCredits += ItemQuantity;
		Debug.Log (CurrentCredits);

		}
	 
}
