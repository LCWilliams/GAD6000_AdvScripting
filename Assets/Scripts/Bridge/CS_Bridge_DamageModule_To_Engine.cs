/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle_*
INBOUND REFERENCES: null
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Bridges events from: CS_DamageModule, to: CS_VehicleEnigne.
        Purpose of bridge: To interconnect the damage module of the engine INSIDE the vehicle, and convert into a clamped 01 value to be used on EFFICIENCY.
        This allows the vehicle to have an engine with its own independant damage module, that effects the vehicleEngine efficiency value appropriately.

        ATTENTION!
        MUST BE PLACED ON THE GAME OBJECT REPRESENTING THE PHYSICAL ENGINE!
*/

using UnityEngine;
using System.Collections;

public class CS_Bridge_DamageModule_To_Engine : MonoBehaviour {

    // VARIABLES:
    [Header("GAME OBJECTS:")]
    [Tooltip("The object holding 'CS_VehicleEngine'")] public GameObject go_ObjectHoldingEngine;
    CS_VehicleEngine v_VehicleEngine;
    //public GameObject go_PhysicalEngine;
    CS_DamageModule v_PhysicalEngineDamageModule;
    [Header("SETTINGS:")]
    [Tooltip("The ratio of engine health (from damage module) to VehicleEngine Efficiency. \n1 Equates to full correlation, 0.5 is half correlation (will result in maximum efficiency of 0.5).")][Range(0.1f, 1)]public float v_EngineHealthToEfficiencyRatio = 1;
    //

	// Use this for initialization
	void Start () {
        // DEBUG ERROR:  Inform if the script is placed incorrectly.
        if (go_ObjectHoldingEngine == gameObject) { Debug.LogError("BRIDGE PLACED ON OBJECT HOLDING MAIN SCRIPT!  Move bridge to the gameobject representing the physical engine!");}

        else {
            v_PhysicalEngineDamageModule = gameObject.GetComponent<CS_DamageModule>();
            v_VehicleEngine = go_ObjectHoldingEngine.GetComponent<CS_VehicleEngine>();
        } // END - If correct script placement is true.
	} // END - Start
	
	// Update is called once per frame
	void Update () {
            float v_DamageToHealthRatio = v_PhysicalEngineDamageModule.v_DamageSustained / v_PhysicalEngineDamageModule.v_ModuleHealth;
            v_VehicleEngine.v_Efficiency = 1 - (v_DamageToHealthRatio * v_EngineHealthToEfficiencyRatio);
            // Ratio:  DamageToHealth = 0.5 | DamageToEfficiency = 1 | Efficiency = 0.5
    } // END - Update.
} // END - Monobehaviour.
