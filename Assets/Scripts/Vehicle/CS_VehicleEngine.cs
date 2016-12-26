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

    [Header("Gears & Speed")][Space(10)]
    public float v_MaximumSpeed; // The max speed the vehicle is able to reach.
    float v_GearLimitedSpeed; // The max speed the vehicle is able to reach based upon its current gear.
    float v_TorqueStep; // A single step in torque between gears.
    float v_SpeedStep; // A single step in speed between gears.
    public float v_CurrentSpeed;
    public float v_MaximumTorque; // Max amount of torque the vehicle is capable of producing.
    float v_torqueToApply = 0;
    public float v_TorqueLerpTime; // Used to prevent the player from going full speed quickly using only gear 5.
    float v_GearLimitedTorque; // Max torque the vehicle is able to produce based upon its current gear.
    [Tooltip("The amount of torque the wheels have when the engine is DISABLED")]public float v_WheelBrakeTorque;
    [Tooltip("The amount of additional torque the engine applies to brakes when enabled")]public float v_PowerBrakeTorque;
    [Range(3,10)] public int v_MaxGears;
    [Range(0,10)] public int v_Gear; // Current gear of the vehicle.
    public bool v_Reversing;
    public bool v_Braking;

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

    [Header("Engine Audio")][Space(10)]
    public AudioClip ac_EngineIdle;
    public AudioClip ac_EngineStart;
    AudioSource as_EngineAudioSource;

    [Header("Misc")][Space(10)]
    public Transform v_CenterofMass;
    CS_WheelManager v_WheelManager;
    public Quaternion v_GunOriginalPosition;
    public Rigidbody v_EngineRigidbody;
//    Animation v_VehicleAnimation;

    private void Awake(){
        v_EngineRigidbody = GetComponent<Rigidbody>();
        v_WheelManager = GetComponent<CS_WheelManager>();
        v_EngineRigidbody.centerOfMass = v_CenterofMass.localPosition; // Debug.Log(this.transform.GetComponent<Rigidbody>().centerOfMass);
        v_TorqueStep = v_MaximumTorque / v_MaxGears;
        v_SpeedStep = v_MaximumSpeed / v_MaxGears;
        Debug.Log(v_TorqueStep + " " + v_SpeedStep);
    }

    // Use this for initialization
    void Start () {
        v_Gear = 0; // Set the gear as 0 by defualt.
        as_EngineAudioSource = GameObject.Find("Engine").GetComponent<AudioSource>();
        v_Turret = GameObject.Find("Turret").GetComponent<Transform>();
        v_Gun = GameObject.Find("Gun").GetComponent<Transform>();
        v_GunOriginalPosition = v_Gun.localRotation;
	} // END - Start
	
	void Update () {
        v_CurrentSpeed = v_EngineRigidbody.velocity.magnitude * 2.2369362912f;

        AutoEngineDisable();

        if (v_EngineEnabled){
            PowerRecharge();
        } // END - Engine Enabled only functions
        else if(!v_EngineEnabled){
            EngineIgnition();
        } // END - Engine Disabled only functions
	} // END - Update.

    private void FixedUpdate(){
        
    }

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

        if (p_accelerationAmmount != 0 && v_Gear != 0) { // Increase engine audio pitch based on input.
            as_EngineAudioSource.pitch = (Mathf.Lerp(as_EngineAudioSource.pitch, (0.5f + (1 * p_accelerationAmmount)), 0.1f));
        } else{
            as_EngineAudioSource.pitch = (Mathf.Lerp(as_EngineAudioSource.pitch, 1, 0.1f));
        } // END - Engine audio pitch.

        // LIMIT SPEED:  Used over clamps to allow the vehicle to travel faster in the event of going downhill.
        if (v_CurrentSpeed > v_GearLimitedSpeed){
            v_torqueToApply = 0;
        } else // END - Prevent torque application if speed is greater than MAX

        if(v_CurrentSpeed <= 1) { // REQUIRED TO ENABLE VEHICLE TO MOVE FROM STANDSTILL.
            v_TorqueLerpTime =  p_accelerationAmmount;
            v_torqueToApply = v_MaximumTorque * v_TorqueLerpTime;
        } else {
            v_TorqueLerpTime = Mathf.Clamp01((v_CurrentSpeed / v_GearLimitedSpeed) * (p_accelerationAmmount * GearEfficiency()));
            v_torqueToApply = Mathf.Lerp(0, v_GearLimitedTorque, v_TorqueLerpTime * 2);
        }

        // SET reverse flag if input dictates reverse action.
        if(p_accelerationAmmount < 0) {
            v_Reversing = true;
        } else { v_Reversing = false; }

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

    // Returns the current efficiency of the gear selection as a range of 0-1, to be used as a multiplier.
    float GearEfficiency() {
        float v_GearEfficiencyLow = v_CurrentSpeed / ((v_GearLimitedSpeed) - (v_SpeedStep * 1f));
        float v_GearEfficiencyHigh = v_CurrentSpeed / (v_GearLimitedSpeed + (v_SpeedStep));
        // Create the end value- compare both, devide by desired medium, then clamp:
        float v_GearEfficiency = Mathf.Clamp((v_GearEfficiencyLow + v_GearEfficiencyHigh), 0, 1);

        Debug.Log(v_GearEfficiency +" L: " +v_GearEfficiencyLow +" H: " +v_GearEfficiencyHigh);

        return v_GearEfficiency;
    } // END - gear efficiency.


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
        float v_BrakeTorqueToApply;

        // SET brake flag.
        if (p_BrakeAmmount > 0) {
            v_Braking = true;
        } else { v_Braking = false; }

        if (v_EngineEnabled) {
            // When Engine is enabled: ADD wheel brake torque to engine-assisted brakes:
            v_BrakeTorqueToApply =  (v_WheelBrakeTorque + (v_PowerBrakeTorque * v_Efficiency)) * p_BrakeAmmount;
        } else { // When engine is disabled: use wheel brakes only.
            v_BrakeTorqueToApply = v_WheelBrakeTorque * p_BrakeAmmount;
        } // END - TorqueToApply If statements.

        if (v_WheelManager.v_BrakesFront){
            for (int frontBrakeIndex = 0; frontBrakeIndex < v_WheelManager.v_WheelsFront.Length; frontBrakeIndex++) {
                v_WheelManager.v_WheelsFront[frontBrakeIndex].brakeTorque = v_BrakeTorqueToApply;
            } // END - Front wheel loop.
        } // END - Brakes to front.

        if (v_WheelManager.v_BrakesMid){
            for( int midBrakeIndex = 0; midBrakeIndex < v_WheelManager.v_WheelsMid.Length; midBrakeIndex++) {
                v_WheelManager.v_WheelsMid[midBrakeIndex].brakeTorque = v_BrakeTorqueToApply;
            } // END - Mid wheel loop.
        } // END - Brakes to mid.

        if (v_WheelManager.v_BrakesRear){
            for (int rearBrakeIndex = 0; rearBrakeIndex < v_WheelManager.v_WheelsRear.Length; rearBrakeIndex++) {
                v_WheelManager.v_WheelsRear[rearBrakeIndex].brakeTorque = v_BrakeTorqueToApply;
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

    public void ChangeGear(int p_GearChange) {
        v_Gear = Mathf.Clamp((v_Gear + p_GearChange), 0, v_MaxGears);
        v_GearLimitedSpeed = (v_MaximumSpeed / v_MaxGears) * v_Gear;
        v_GearLimitedTorque = v_TorqueStep * v_Gear;

        Debug.Log("GEAR: " + v_Gear +"| LimitedSpeed: " +v_GearLimitedSpeed +"| Limited Torque: " +v_GearLimitedTorque);
        
    } // END - gear change.

} // END - Mono Behaviour.
