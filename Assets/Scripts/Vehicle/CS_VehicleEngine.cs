/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES:
OVERVIEW:  Controls the main engine of the vehicle.  Efficiency is used as a multiplier of 0-1 on various components.
*/


using UnityEngine;
using System.Collections;

public class CS_VehicleEngine : MonoBehaviour {

    [Header("Engine: Enabled")]
    public bool v_EngineEnabled;
    [Header("Engine: Power & Efficiency")]
    [Range(0.01f,1.0f)]
    public float v_Efficiency; // Determines how well the engine & car components function.
    public float v_EnginePower; // The maximum amount of power the engine is capable of holding.
//    public float v_UsedPower; // How much power is currently being used.
    public float v_RechargeRate; // How much power is recharged per second.
    public int v_MaximumTorque; // Max amount of torque the vehicle is capable of producing.
    [Range(0,5)]
    public int v_Gear; // Current gear of the vehicle.
    [Header("Deficiency Limits")]
    [Tooltip("Stops the engine being disabled too frequently")]
    [Range(0.0f,0.5f)]
    public float v_AutoEngineDisableThreshold;

    // Use this for initialization
    void Start () {
        DefaultSettings();
        ToggleEngine(); // [DEBUG] -- turns on vehicle.
	} // END - Start
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("e")){
            ToggleEngine();
        }
        AutoEngineDisable();
        PowerRecharge();
	} // END - Update.

    void DefaultSettings(){
        v_EnginePower = v_EnginePower * v_Efficiency;
    } // END - Default settings

    void AutoEngineDisable(){
        // Disables the engine when power exceeds the maximum threshold.
//        if(v_UsedPower > v_EnginePower) {
        if(v_EnginePower <= 0) {
            v_EngineEnabled = false;
        } // Toggle when engine power reaches 0.

        // Disables the engine due to deficiency. 
        bool DeficiencyBlock = (Random.value > (v_Efficiency + v_AutoEngineDisableThreshold));
        if (!DeficiencyBlock) {
            v_EngineEnabled = false;
        } // END -- Deficiency caused disable.
    }


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
        bool DeficiencyBlock = (Random.value > v_Efficiency);
        
        // Only recharge IF vehicle efficiency is ABOVE a random value.
        if (!DeficiencyBlock) {
            v_EnginePower = v_EnginePower + (v_RechargeRate * v_Efficiency);
        } // END - If Deficiency block.
    } // END - Power Recharge.

} // END - Mono Behaviour.
