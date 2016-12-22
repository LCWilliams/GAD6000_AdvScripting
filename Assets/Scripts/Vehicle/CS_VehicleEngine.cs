/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver | CS_AIDriver | CS_WheeledTankWeapon_00
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Controls the main engine of the vehicle.  
Efficiency is used as a multiplier of 0-1 on various components.
*/


using UnityEngine;
using System.Collections;

public class CS_VehicleEngine : MonoBehaviour {

    [Header("Engine: Enabled")]
    public bool v_EngineEnabled;
    [Header("Engine: Power & Efficiency")][Space(10)]
    [Range(0.1f,1.0f)] public float v_Efficiency; // Determines how well the engine & car components function.
    public float v_EnginePower; // How much power is currently being used. 
    public float v_PowerCap; // The maximum amount of power the engine is capable of holding.
    public float v_RechargeRate; // How much power is recharged per second.
    [Header("Gears & Torque")][Space(10)]
    public float v_MaximumTorque; // Max amount of torque the vehicle is capable of producing.
    [Tooltip("The amount of torque the wheels have when the engine is DISABLED")]public float v_WheelBrakeTorque;
    [Tooltip("The amount of additional torque the engine applies to brakes when enabled")]public float v_PowerBrakeTorque;
    [Range(0,5)] public int v_Gear; // Current gear of the vehicle.
    [Header("Turret & Gun")][Space(10)]
    public Transform v_Turret;
    public float v_TurretTraverseSpeed;
    [Tooltip("Applies a limit to the rotation/traverse of the turret")]public bool v_LimitTraverse;
    [Range(-170,0)] public float v_MaxTurretTraverseLeft;
    [Range(0, 170)] public float v_MaxTurretTraverseRight;
    public Transform v_Gun;
    public float v_GunElevationSpeed;
    [Range(0, 90)] public float v_MaxGunElevation;
    [Range(0, 90)] public float v_MaxGunDepression;
    bool v_GunElevated; // Used to determine if the gun is currently in an elevated rotation.
    [Header("Deficiency Limits")][Space(10)]
    [Tooltip("Higher values equate to the engine cutting out more frequently")]
    [Range(0.0f,0.5f)] public float v_AutoEngineDisableThreshold;
    [Range(0.0f,0.5f)] public float v_rechargeThreshold;
    [Range(0.0f,1.0f)] public float v_GUIUpdateThreshold;
    [Header("Misc")][Space(10)]
    public Transform v_CenterofMass;
    CS_WheelManager v_WheelManager;
    public Quaternion v_GunOriginalPosition;
//    Animation v_VehicleAnimation;

    private void Awake(){
        this.transform.GetComponent<Rigidbody>().centerOfMass = v_CenterofMass.localPosition;
        Debug.Log(this.transform.GetComponent<Rigidbody>().centerOfMass);
    }

    // Use this for initialization
    void Start () {
        v_WheelManager = GetComponent<CS_WheelManager>();
        v_Turret = GameObject.Find("Turret").GetComponent<Transform>();
        v_Gun = GameObject.Find("Gun").GetComponent<Transform>();
        v_GunOriginalPosition = v_Gun.localRotation;

        // Set center of mass:
//        ToggleEngine(); // [DEBUG] -- turns on vehicle.
	} // END - Start
	
	// Update is called once per frame
	void Update () {
        AutoEngineDisable();

        if(v_EngineEnabled){
            PowerRecharge();
        } // END - Engine Enabled only functions
        else if(!v_EngineEnabled){
            EngineIgnition();
        } // END - Engine Disabled only functions
	} // END - Update.

    void AutoEngineDisable(){
        // Disables the engine when power exceeds the maximum threshold.
//        if(v_UsedPower > v_EnginePower) {
        if(v_EnginePower <= 0) {
            v_EngineEnabled = false;
        } // Toggle when engine power reaches 0.


        // Disables the engine due to deficiency.
        bool DeficiencyBlock = (Random.Range(-1f,v_AutoEngineDisableThreshold) > v_Efficiency);
        bool DeficiencyBlock2 = (Random.Range(-1f, v_AutoEngineDisableThreshold) > v_Efficiency);
        if (DeficiencyBlock && DeficiencyBlock2) {
            v_EngineEnabled = false;
        } // END -- Deficiency caused disable.
    } // END - AutoEngineDisable.


    void ToggleEngine(){
        if (v_EngineEnabled){ 
            // Enables the engine if it was previously disabled.
            v_EngineEnabled = false;
        }else{
            // Disables the engine if it was previously enabled.
            v_EngineEnabled = true;
        } // END - if else.
    } // END - Toggle Engine.


    void PowerRecharge() {
        bool DeficiencyBlock = (Random.value > (v_Efficiency + v_rechargeThreshold));
        
        // Only recharge IF vehicle efficiency is ABOVE a random value.
        if (!DeficiencyBlock && v_EnginePower < v_PowerCap) {
            v_EnginePower = v_EnginePower + (v_RechargeRate * v_Efficiency);
        } // END - If Deficiency block.
    } // END - Power Recharge.

    public void EngineIgnition() {
        // Hold E.
    } // END - EngineIgnition

    // ---------------------------------------------------------------------------------------------------------

    public void Acceleration(float p_accelerationAmmount) {
        float v_torqueToApply = (v_MaximumTorque * v_Efficiency) * p_accelerationAmmount;
        if (v_WheelManager.v_PowerToFront) {
            // Apply acceleration to front wheels.
            for (int frontWheelIndex = 0; frontWheelIndex < v_WheelManager.v_WheelsFront.Length; frontWheelIndex++) {
                v_WheelManager.v_WheelsFront[frontWheelIndex].motorTorque = v_torqueToApply;
            } // END - For loop
        } // END - Power to front.

        // Only applies if there are wheels within the array:
        if (v_WheelManager.v_PowerToMid && v_WheelManager.v_WheelsMid.Length > 0) { 
            for (int MidWheelIndex = 0; MidWheelIndex < v_WheelManager.v_WheelsMid.Length; MidWheelIndex++){
                v_WheelManager.v_WheelsMid[MidWheelIndex].motorTorque = v_torqueToApply;
                } // END - For loop. 
            } // END - Power to mid.

        if (v_WheelManager.v_PowerToRear) {
            for (int RearWheelIndex = 0; RearWheelIndex < v_WheelManager.v_WheelsRear.Length; RearWheelIndex++){
                v_WheelManager.v_WheelsRear[RearWheelIndex].motorTorque = v_torqueToApply;
            } // END - For loop. 
        } // END - Power to Rear.

    } // END - Acceleration.

    public void Steering(float p_steerInput) {
        float v_steerAmmount = v_WheelManager.v_Steering * p_steerInput;
        // FRONT steering loop.
        if (v_WheelManager.v_SteeringFront) {
            for(int frontSteerIndex = 0; frontSteerIndex < v_WheelManager.v_WheelsFront.Length; frontSteerIndex++) {
                v_WheelManager.v_WheelsFront[frontSteerIndex].steerAngle = v_steerAmmount;
            } // END - for steering loop.
        }// END - Steering front.

        // REAR steering loop.
        if (v_WheelManager.v_SteeringRear) {
            for (int rearSteerIndex = 0; rearSteerIndex < v_WheelManager.v_WheelsRear.Length; rearSteerIndex++){
                v_WheelManager.v_WheelsRear[rearSteerIndex].steerAngle = v_steerAmmount *-1;
            } // END - for steering loop.
        } // END - Steering rear.

    } // END - Steering.

    public void ApplyBraking(float p_BrakeAmmount) {
        float v_TorqueToApply;
        if (v_EngineEnabled) {
            // When Engine is enabled: ADD wheel brake torque to engine-assisted brakes:
             v_TorqueToApply =  (v_WheelBrakeTorque + (v_PowerBrakeTorque * v_Efficiency)) * p_BrakeAmmount;
        } else { // When engine is disabled: use wheel brakes only.
             v_TorqueToApply = v_WheelBrakeTorque * p_BrakeAmmount;
        } // END - TorqueToApply If statements.

        if (v_WheelManager.v_BrakesFront){
            for (int frontBrakeIndex = 0; frontBrakeIndex < v_WheelManager.v_WheelsFront.Length; frontBrakeIndex++) {
                v_WheelManager.v_WheelsFront[frontBrakeIndex].brakeTorque = v_TorqueToApply;
            } // END - Front wheel loop.
        } // END - Brakes to front.

        if (v_WheelManager.v_BrakesMid){
            for( int midBrakeIndex = 0; midBrakeIndex < v_WheelManager.v_WheelsMid.Length; midBrakeIndex++) {
                v_WheelManager.v_WheelsMid[midBrakeIndex].brakeTorque = v_TorqueToApply;
            } // END - Mid wheel loop.
        } // END - Brakes to mid.

        if (v_WheelManager.v_BrakesRear){
            for (int rearBrakeIndex = 0; rearBrakeIndex < v_WheelManager.v_WheelsRear.Length; rearBrakeIndex++) {
                v_WheelManager.v_WheelsRear[rearBrakeIndex].brakeTorque = v_TorqueToApply;
            } // END - Rear wheel loop.
        } // END - Brakes to Rear.
    } // END - Braking.

    // ---------------------------------------------------------------------------------------------------------

    public void TurretRotation(float p_Direction) {
        v_Turret.Rotate(0, 0, (-p_Direction * v_TurretTraverseSpeed) * v_Efficiency);
    } // END - Turret Rotation.

    public void GunElevation(float p_inputDirection) {
        float RotationAmmount = (p_inputDirection * v_GunElevationSpeed) * v_Efficiency;
        float v_DifferenceAngle = Quaternion.Angle(v_Gun.localRotation, v_GunOriginalPosition);

         if(v_DifferenceAngle < v_MaxGunElevation && p_inputDirection > 0) { // Gun Elevation
            v_GunElevated = true;
            v_Gun.Rotate(RotationAmmount, 0, 0);
        } else if (p_inputDirection < 0) { // END - Gun Elevation // Start: Gun Depression.
            if (v_GunElevated) {
                v_Gun.Rotate(RotationAmmount, 0, 0);
                // If difference angle is within the range of MaxDepression, disable gun elevation status.
                if(v_DifferenceAngle < v_MaxGunDepression) { v_GunElevated = false; }
            } // END - If Gun Elevated.
            if (v_DifferenceAngle < v_MaxGunDepression && v_GunElevated == false) {
                v_Gun.Rotate(RotationAmmount, 0, 0);
            } // END - If difference angle allows rotation AND gun is elevated.


            // PREVIOUS ATTEMPS:
//        float v_CurrentGunRot =+ v_Gun.localEulerAngles.x;
//        Debug.Log(v_CurrentGunRot);
        //        float v_RotationAmmountClamped = Mathf.Clamp(RotationAmmount, v_MaxGunDepression, v_MaxGunElevation) *-1; // * -1 for correcting inversion.
        //        v_Gun.Rotate(RotationAmmount, 0, 0);
        //        float v_newRotation = v_CurrentGunRot + RotationAmmount;
        //        v_Gun.localEulerAngles = new Vector3(v_RotationAmmountClamped, 0f, 0f);
        //        v_Gun.localRotation = Quaternion.Euler(v_RotationAmmountClamped, 0, 0); // WORKING-Ish.
//        v_DifferenceAngle = (v_DifferenceAngle > 180) ? v_DifferenceAngle - 360 : v_DifferenceAngle;
        } // END - gun depression.

    } // END - Gun elevation & Depression.

} // END - Mono Behaviour.
