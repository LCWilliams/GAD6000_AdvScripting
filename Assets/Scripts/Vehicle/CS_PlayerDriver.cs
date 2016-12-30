/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: 
OUTBOUND REFERENCES: CS_VehicleEngine | CS_WheelManager | CS_WheeledTankInteriorPanels
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
    CS_WheeledTankInteriorPanels v_InteriorPanels; // Player UI elements.
    CS_WheeledTankWeapons_00 v_TankWeapons; // player weapon scripts.
    // Components:
    [Header("CAMERAS: ")][Space(10)]
    [Tooltip("The players camera/head.")][Header("CAM0: Player Internal:")]public GameObject v_PlayerCamera;
    [Tooltip("The VIEWPORT camera- located at the front of the vehicle.")][Header("CAM1: Viewport:")] public GameObject v_ViewportCamera;
    [Tooltip("The GUN camera: ideally somewhere that makes sense and will aim with the gun.")][Header("CAM2: Viewport:")] public GameObject v_GunCamera;
    int v_CurrentCamera; // Which camera is currently active.
    [Tooltip("Pixels from center that should count as a deadzone")]
    [Range(0.01f,0.3f)] public float v_TurretInputDeadZone;
    [Range(0.01f, 0.3f)] public float v_GunElevationInputDeadzone;

    void Start () {
        // Get components:
        Engine = GetComponent<CS_VehicleEngine>();
        WheelManager = GetComponent<CS_WheelManager>();
        v_InteriorPanels = GetComponent<CS_WheeledTankInteriorPanels>();
        v_TankWeapons = GetComponent<CS_WheeledTankWeapons_00>();
    } // END - Start
	
    void Update() {
            PlayerShoot();
        if (Engine.v_EngineEnabled){
            PlayerChangeMode();
            PlayerForward();

            PlayerGearChange();
            PlayerSteer();
            PlayerBrake();
            PlayerTurretRotation();
            PlayerGunElevation();
        } // END -- Engine Enabled Only functions.
    }

    void FixedUpdate(){
        if (Engine.v_EngineEnabled){

            } // END -- Engine Enabled Only functions.

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
        Engine.ApplyBraking(v_analogueInputValue);
    } // END - Player brake input.


    void PlayerGearChange() {
        if (Input.GetButtonDown("P1_Gears")) {
            int v_DigitalInputValue = (int) Input.GetAxis("P1_Gears");
            Engine.ChangeGear(v_DigitalInputValue);
        }
    }


    void PlayerTurretRotation() {
//        Debug.Log(Input.mousePosition.x);
        int ScreenWidthSegment = Screen.width / 2;
        float v_MouseXPositionOnScreen = Input.mousePosition.x - Screen.width / 2;
        float v_TurretRotation = Mathf.Clamp((v_MouseXPositionOnScreen / ScreenWidthSegment), -1, 1);

        if(v_TurretRotation > 0 && v_TurretRotation > v_TurretInputDeadZone) {
                Engine.TurretRotation(v_TurretRotation);
        } else if (v_TurretRotation < 0 && v_TurretRotation < v_TurretInputDeadZone){
                Engine.TurretRotation(v_TurretRotation);
        } // END - Player Turret Rotation
    } // END player turret rotation input.



    void PlayerGunElevation() {
        // Divide screen height to get center:
        int ScreenHeightSegment = Screen.height / 2;
        // Get mouse position FROM center:
        float v_mouseYPosition = Input.mousePosition.y - Screen.height / 2;
        // Set variable to single digit value/percentage (to obtain 0-1).
        float v_GunElevationInput = Mathf.Clamp((v_mouseYPosition / ScreenHeightSegment),-1, 1);
        // CLAMP values to -1 - 1:

        if(v_GunElevationInput > 0 && v_GunElevationInput > v_GunElevationInputDeadzone) {
            // Positive-Up.
                Engine.GunElevation(v_GunElevationInput);
        } // END if gun elevation Greater than 0 AND greater than deadzone..
        else if (v_GunElevationInput < 0 && v_GunElevationInput < v_GunElevationInputDeadzone){
            // Negative - Down.
                Engine.GunElevation(v_GunElevationInput);
        } // END - Else IF: gun elevation LESS than 0 AND greater than deadzone.

    } // END - Player gun elevation.

    void PlayerChangeMode() {
        if(Input.GetButtonDown("P1_SwapCamera")) {
        v_InteriorPanels.SwapMainScreen();
        //    v_CurrentCamera++;
        //    if(v_CurrentCamera == v_CameraPositions.Length) { v_CurrentCamera = 0; }
        //    v_PlayerCamera.transform.localPosition = v_CameraPositions[v_CurrentCamera].transform.localPosition;
        }// END IF input.

    } // END PlayerChangeMode.

    void PlayerShoot(){
        if(Input.GetButtonDown("P1_Shoot")) {
            Debug.Log("Fired");
            v_TankWeapons.FireMain_Basic();
        } // END - Input/axis.
    } // END - Player shoot.

} // END - CS_PlayerDriver : Monobehaviour.