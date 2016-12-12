/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Controls the main engine of the vehicle.  
Efficiency is used as a multiplier of 0-1 on various components.
*/


using UnityEngine;
using System.Collections;

public class CS_VehicleEngine : MonoBehaviour {

    [Header("Engine: Enabled")]
    public bool v_EngineEnabled;
    [Header("Engine: Power & Efficiency")]
    [Range(0.1f,1.0f)] public float v_Efficiency; // Determines how well the engine & car components function.
    public float v_EnginePower; // How much power is currently being used. 
    public float v_PowerCap; // The maximum amount of power the engine is capable of holding.
    public float v_RechargeRate; // How much power is recharged per second.
    [Header("Gears & Torque")]
    public int v_MaximumTorque; // Max amount of torque the vehicle is capable of producing.
    [Range(0,5)] public int v_Gear; // Current gear of the vehicle.
    [Header("Deficiency Limits")]
    [Tooltip("Higher values equate to the engine cutting out more frequently")]
    [Range(0.0f,0.5f)] public float v_AutoEngineDisableThreshold;
    [Range(0.0f,0.5f)] public float v_rechargeThreshold;
    CS_WheelManager v_WheelManager;

    // Use this for initialization
    void Start () {
        v_WheelManager = GetComponent<CS_WheelManager>();
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

    public void Acceleration(float p_accelerationAmmount) {
        float v_torqueToApply = 500 * v_Efficiency;
        if (v_WheelManager.v_PowerToFront) {
            // Apply acceleration to front wheels.
            for (int frontWheelIndex = v_WheelManager.v_WheelsFront.Length; frontWheelIndex < v_WheelManager.v_WheelsFront.Length; frontWheelIndex++) {
                v_WheelManager.v_WheelsFront[frontWheelIndex].motorTorque = v_torqueToApply;
            } // END - For loop
        } // END - Power to front.

        // Only applies if there are wheels within the array:
        if (v_WheelManager.v_PowerToMid && v_WheelManager.v_WheelsMid.Length > 0) { 
            for (int MidWheelIndex = v_WheelManager.v_WheelsMid.Length; MidWheelIndex < v_WheelManager.v_WheelsMid.Length; MidWheelIndex++){
                v_WheelManager.v_WheelsMid[MidWheelIndex].motorTorque = v_torqueToApply;
                } // END - For loop. 
            } // END - Power to mid.

        if (v_WheelManager.v_PowerToRear) {
            for (int RearWheelIndex = v_WheelManager.v_WheelsRear.Length; RearWheelIndex < v_WheelManager.v_WheelsRear.Length; RearWheelIndex++){
                v_WheelManager.v_WheelsRear[RearWheelIndex].motorTorque = v_torqueToApply;
            } // END - For loop. 
        } // END - Power to Rear.

    } // END - Acceleration.

    public void Steering(float p_steerAmmount) {
        // FRONT steering loop.
        if (v_WheelManager.v_SteeringFront) {
            for(int frontSteerIndex = v_WheelManager.v_WheelsFront.Length; frontSteerIndex < v_WheelManager.v_WheelsFront.Length; frontSteerIndex++) {
                v_WheelManager.v_WheelsFront[frontSteerIndex].steerAngle = p_steerAmmount;
            } // END - for steering loop.
        }// END - Steering front.

        // REAR steering loop.
        if (v_WheelManager.v_SteeringRear) {
            for (int rearSteerIndex = v_WheelManager.v_WheelsRear.Length; rearSteerIndex < v_WheelManager.v_WheelsRear.Length; rearSteerIndex++){
                v_WheelManager.v_WheelsRear[rearSteerIndex].steerAngle = p_steerAmmount;
            } // END - for steering loop.
        } // END - Steering rear.

    } // END - Steering.

    public void ApplyBraking(float p_BrakeAmmount) {
        if (v_WheelManager.v_BrakesFront){
            // Apply acceleration to front wheels.
        } // END - Brakes to front.

        if (v_WheelManager.v_BrakesMid){
            // Apply acceleration to mid wheels.
        } // END - Brakes to mid.

        if (v_WheelManager.v_BrakesRear){
            // Apply acceleration to Rear wheels.
        } // END - Brakes to Rear.
    } // END - Braking.



} // END - Mono Behaviour.
