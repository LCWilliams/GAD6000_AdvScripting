/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: 
OUTBOUND REFERENCES: CS_VehicleEngine | CS_WheelManager
*/

using UnityEngine;
using System.Collections;

public class CS_PlayerDriver : MonoBehaviour {
    // VARIABLES
    //Scripts:
    CS_VehicleEngine Engine; // Engine script attached to this vehicle.

    void Start () {
        // Get components:
        Engine = GetComponent<CS_VehicleEngine>();
    } // END - Start
	
	void Update () {
	    
	}
}
