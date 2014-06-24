using UnityEngine;
using System.Collections;

public class StartingRoom : MonoBehaviour {

	private RoomInfo roominfo;

	void Start(){

		roominfo = gameObject.GetComponent<RoomInfo> ();
		roominfo.SetExits ();

		}
}
