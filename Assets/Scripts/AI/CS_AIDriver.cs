/*
AUTHOR(S): SCOTT ANDERS 	DATE: 12/2016 - 01/2017
EDITOR(S): LEE WILLIAMS
SCRIPT HOLDERS: PF_WheeledTank_AI
INBOUND REFERENCES: 
OUTBOUND REFERENCES: CS_VehicleEngine | CS_WheelManager | CS_WheeledTankInteriorPanels
OVERVIEW:  References player input used for: driving, braking and steering.
*/

using UnityEngine;
using System.Collections;

public class CS_AIDriver : MonoBehaviour
{
	
//	public enum BrakeCondition
//	{
//		NoBrake,
//		TargetDirectionDifference,			Temp
//		TargetDistance,
//	}

    // VARIABLES
    public int v_PlayerIndex;
    //Scripts:
    CS_VehicleEngine Engine; // Engine script attached to this vehicle.
    CS_WheelManager WheelManager; // Wheel management script attached to this vehicle.
    CS_WheeledTankInteriorPanels v_InteriorPanels;
    // Components:
    public GameObject v_PlayerCamera;
    int v_CurrentCamera;
    public Transform[] v_CameraPositions;
    [Tooltip("Pixels from center that should count as a deadzone")]
    [Range(0.1f, 0.3f)]
    public float v_TurretInputDeadZone;
    [Range(0.1f, 0.3f)]
    public float v_GunElevationInputDeadzone;
	public GameObject CurrentTarget;
	private GameObject Player;
	private Vector3 currentPosition;
	private float v_Steer = 0.0f;

//	private BrakeCondition v_BrakeCondition = BrakeCondition.TargetDistance;		Temp

    void Start()
    {
        // Get components:
        Engine = GetComponent<CS_VehicleEngine>();
        WheelManager = GetComponent<CS_WheelManager>();
        v_InteriorPanels = GetComponent<CS_WheeledTankInteriorPanels>();

		Player = GameObject.Find ("PF_WheeledTank_00");
		Player.GetComponent (Transform);
		CurrentTarget = Player;


    } // END - Start

    void Update()
    {
//        PlayerChangeMode();
    }

    void FixedUpdate()
    {
        if (Engine.v_EngineEnabled)
        {
            AIForward();
        } // END -- Engine Enabled Only functions.
        //PlayerSteer();
        //PlayerBrake();
        //PlayerTurretRotation();
        //PlayerGunElevation();
    } // END - Fixed Update.


    void AIForward()
    {
		
        //float v_analogueInputValue = Input.GetAxis("P1_Acceleration");

    } // END - Player forward input.

//    void PlayerSteer()
//    {
//        float v_analogueInputValue = Input.GetAxis("P1_Steer");
//        Engine.Steering(v_analogueInputValue);
//    } // END - Player Steer input.

//    void PlayerBrake()
//    {
//        float v_analogueInputValue = Input.GetAxis("P1_Brake");
//        Engine.ApplyBraking(v_analogueInputValue);
//    } // END - Player brake input.

//    void PlayerTurretRotation()
//    {
//        //        Debug.Log(Input.mousePosition.x);
//        int ScreenWidthSegment = Screen.width / 2;
//        float v_MouseXPositionOnScreen = Input.mousePosition.x - Screen.width / 2;
//        float v_TurretRotation = Mathf.Clamp((v_MouseXPositionOnScreen / ScreenWidthSegment), -1, 1);
//
//        if (v_TurretRotation > 0 && v_TurretRotation > v_TurretInputDeadZone)
//        {
//            Engine.TurretRotation(v_TurretRotation);
//        }
//        else if (v_TurretRotation < 0 && v_TurretRotation < v_TurretInputDeadZone)
//        {
//            Engine.TurretRotation(v_TurretRotation);
//        } // END - Player Turret Rotation
//    } // END player turret rotation input.
//


//    void PlayerGunElevation()
//    {
//        // Divide screen height to get center:
//        int ScreenHeightSegment = Screen.height / 2;
//        // Get mouse position FROM center:
//        float v_mouseYPosition = Input.mousePosition.y - Screen.height / 2;
//        // Set variable to single digit value/percentage (to obtain 0-1).
//        float v_GunElevationInput = Mathf.Clamp((v_mouseYPosition / ScreenHeightSegment), -1, 1);
//        // CLAMP values to -1 - 1:
//
//        if (v_GunElevationInput > 0 && v_GunElevationInput > v_GunElevationInputDeadzone)
//        {
//            // Positive-Up.
//            Engine.GunElevation(v_GunElevationInput);
//        } // END if gun elevation Greater than 0 AND greater than deadzone..
//        else if (v_GunElevationInput < 0 && v_GunElevationInput < v_GunElevationInputDeadzone)
//        {
//            // Negative - Down.
//            Engine.GunElevation(v_GunElevationInput);
//        } // END - Else IF: gun elevation LESS than 0 AND greater than deadzone.
//
//    } // END - Player gun elevation.

    void PlayerChangeMode()
    {
        if (Input.GetKeyDown("mouse 1"))
        {
            v_InteriorPanels.SwapMainScreen();
            //    v_CurrentCamera++;
            //    if(v_CurrentCamera == v_CameraPositions.Length) { v_CurrentCamera = 0; }
            //    v_PlayerCamera.transform.localPosition = v_CameraPositions[v_CurrentCamera].transform.localPosition;
        }// END IF input.

    } // END PlayerChangeMode.

} // END - CS_PlayerDrive