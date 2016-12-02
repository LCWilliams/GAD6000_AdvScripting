using UnityEngine;
using System.Collections;

public class CS_VehicleEngine : MonoBehaviour {

    [Header("Engine")]
    public bool v_EngineEnabled;
    public float v_Efficiency; // Determines how well the engine & car components function.
    public float v_EnginePower; // The maximum amount of power the engine is capable of holding.
    public float v_UsedPower; // How much power is currently remaining.

	// Use this for initialization
	void Start () {
        DefaultSettings();
        ToggleEngine();
	} // END - Start
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("e"))
        {
            ToggleEngine();
        }
        AutoEngineDisable();

	} // END - Update.

    void DefaultSettings(){
        v_EngineEnabled = false;
        v_Efficiency = 1.0f;
        v_EnginePower = 100 * v_Efficiency;
        v_UsedPower = 0;
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
