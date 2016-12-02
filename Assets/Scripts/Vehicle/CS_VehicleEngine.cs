/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES:
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
    public float v_UsedPower; // How much power is currently being used.
    public int v_MaximumTorque; // Max amount of torque the vehicle is capable of producing.
    [Range(0,5)]
    public int v_Gear; // Current gear of the vehicle.

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

	} // END - Update.

    void DefaultSettings(){
        v_EnginePower = v_EnginePower * v_Efficiency;
    } // END - Default settings

    void AutoEngineDisable(){
        // Automatically disables the engine when power exceeds the maximum threshold.
        if(v_UsedPower > v_EnginePower) {
            v_EngineEnabled = false;
        }
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

} // END - Mono Behaviour.
