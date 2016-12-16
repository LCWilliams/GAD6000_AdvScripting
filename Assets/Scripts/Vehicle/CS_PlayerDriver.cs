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
    // Components:
    public GameObject v_PlayerCamera;

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
        PlayerTurretRotation();
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

    void PlayerTurretRotation() {
//        Debug.Log(Input.mousePosition.x);
        int ScreenWidthSegment = Screen.width / 3;
        Vector3 v_MousePositionOnScreen = Input.mousePosition;
        if(v_MousePositionOnScreen.x < (ScreenWidthSegment * 1)) {
            Engine.TurretRotation(+1);
            //Debug.Log("Test: Rot: Left");
        } else if(v_MousePositionOnScreen.x > (ScreenWidthSegment * 2)){
            Engine.TurretRotation(-1);
        }else {
            return;
        } // END else if loops.

    } // END player mouse input.

} // END - CS_PlayerDriver : Monobehaviour.