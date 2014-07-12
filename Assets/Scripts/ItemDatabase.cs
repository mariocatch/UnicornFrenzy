using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

	public List<Item> Items = new List<Item> ();

	void Start(){

		var Credit = new Item {

			ItemID = 0,
			ItemName = "Credits"

		};

		Items.AddRange (new [] {
			Credit
		});
	}
}
