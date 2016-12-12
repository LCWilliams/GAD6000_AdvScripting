/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: 
OUTBOUND REFERENCES: CS_VehicleEngine | CS_WheelManager
OVERVIEW:  References player input used for: driving, braking and steering.
*/

using UnityEngine;
using System.Collections;

public class CS_PlayerDriver : MonoBehaviour {
    // VARIABLES
    public int v_PlayerIndex;
    //Scripts:
    CS_VehicleEngine Engine; // Engine script attached to this vehicle.
    CS_WheelManager WheelManager; // Wheel management script attached to this vehicle.

    void Start () {
        // Get components:
        Engine = GetComponent<CS_VehicleEngine>();
        WheelManager = GetComponent<CS_WheelManager>();
    } // END - Start
	
    void FixedUpdate(){
        if (Engine.v_EngineEnabled){
            PlayerForward();
            } // END -- Engine Enabled Only functions.
        PlayerSteer();
        PlayerBrake();
    } // END - Fixed Update.


    void PlayerForward() {
        float v_analogueInputValue = Input.GetAxis("P1_Acceleration");
        Engine.Acceleration(v_analogueInputValue);
    } // END - Player forward input.

    void PlayerSteer() {
        float v_analogueInputValue = Input.GetAxis("P1_Steer");
        Engine.Steering(v_analogueInputValue);
    } // END - Player Steer input.

    void PlayerBrake() {
        float v_analogueInputValue = Input.GetAxis("P1_Brake");
    } // END - Player brake input.

} // END - CS_PlayerDriver : Monobehaviour.