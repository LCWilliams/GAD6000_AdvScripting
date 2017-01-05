/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle_*
INBOUND REFERENCES: null
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Creates a bridge between damage module and a scene event.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CS_Bridge_DamageModule_To_SceneEvent : MonoBehaviour {

    // VARIABLES:
    [Header("GAME OBJECTS:")]
    //public GameObject go_PhysicalEngine;
    [Tooltip("Which objects health to use. Will use self if not specified.")]public GameObject go_ObjectHealthToUse;
    CS_DamageModule v_DamageModule;
    [Header("SETTINGS:")]
//    [Tooltip("Damage Module: Health converted to lerp value/percentage. (0 - 1)")]
//    [Range(0.1f, 1)] public float v_EngineHealthRatio = 1;
    [Tooltip("Event will be triggered when damage sustained goes above this value. 1 equates to complete damage/destruction.")]
    [Range(0.1f, 1)] public float v_EventRatio = 0.1f;

    [Space(15)]
    public bool v_LoadLevel;
    [Tooltip("Will use string if not left empty!")]public string v_LevelToLoad_String;
    public int v_LevelToLoad;
    //

    // Use this for initialization
    void Start() {
        if(go_ObjectHealthToUse == null) { go_ObjectHealthToUse = gameObject; }

        v_DamageModule = go_ObjectHealthToUse.GetComponent<CS_DamageModule>();
    } // END - Start

    // Update is called once per frame
    void Update(){
        float v_DamageToHealthRatio = v_DamageModule.v_DamageSustained / v_DamageModule.v_ModuleHealth;

        if(v_DamageToHealthRatio >= v_EventRatio){
            if (v_LoadLevel) {
                if(v_LevelToLoad_String != null) { SceneManager.LoadSceneAsync(v_LevelToLoad_String, LoadSceneMode.Single); } // END - Use string.
                else { SceneManager.LoadSceneAsync(v_LevelToLoad, LoadSceneMode.Single); } // END - Use int.
            } // END - Load Level
        } // END - EVENT.

    } // END - Update.
}
