﻿/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Manages the elements of the player vehicle (PF_PlayerVehicle/WheeledTank) interior panels:
Used for visual player feedback and the swapping of render textures.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CS_WheeledTankInteriorPanels : MonoBehaviour {


    // VARIABLES:
    public CS_VehicleEngine v_Engine;

    [Header("Main Screens:")]
    GameObject v_Viewfinder;
    GameObject v_GunView;

    [Header("Panel: Curved Center")]
    public Image v_TankBase;
    public RectTransform v_TankTurret;

    [Header("VisualEffects")]
    ParticleSystem v_ConsoleParticleSystem;
    ParticleSystem.EmissionModule v_ConsoleSparkEmitter;
    [Range(100, 5000)]public int v_SparkEmitterMaxRate;
    [Range(0.5f, 0.95f)][Tooltip("Sparks will only begin to start when efficiency goes BELOW this number")]public float v_SparksAtEfficiency;
    [Range(1, 100)][Tooltip("Rate deduction per frame of spark emitter.")]public int v_SparkEmitterRateDecrease;
    //    public UnityEngine.UI.Image v_TankBase;

    // END - VARIABLES

    void Start () {
        v_GunView = GameObject.Find("MainScreen_GunView");
        v_Viewfinder = GameObject.Find("MainScreen_Viewport");
        v_Engine = GetComponentInParent<CS_VehicleEngine>();
        v_ConsoleParticleSystem = GameObject.Find("PanelDeficiencyParticle").GetComponent < ParticleSystem>();
        v_ConsoleSparkEmitter = v_ConsoleParticleSystem.emission;
        v_ConsoleSparkEmitter.rate = 0;
        if(!v_Viewfinder.activeSelf) { v_Viewfinder.SetActive(true); }
        if(v_GunView.activeSelf) { v_GunView.SetActive(false); }
	} // END - Start
	
	// Update is called once per frame
	void Update () {
        SparkEmitter_Decrease();
        bool DeficiencyBlock0 = Random.Range(-1f, v_Engine.v_GUIUpdateThreshold) > v_Engine.v_Efficiency;
        bool DeficiencyBlock1 = Random.Range(-1f, v_Engine.v_GUIUpdateThreshold) > v_Engine.v_Efficiency;

        if(DeficiencyBlock0 && DeficiencyBlock1){ // Randomisation of update chances.                
            Debug.Log("Panels not updated");
            SparkEmitter_Increase(true, ((v_Engine.v_Efficiency * v_SparkEmitterMaxRate) * (1 - v_Engine.v_Efficiency)));
        } // END Deficiency Block check.
        else {
            UpdatePanels();
        } // END - Else Apply Defficiency block effect.
        
    } // END - Update.

    private void FixedUpdate(){
    //    if (v_Engine.v_Efficiency <= v_SparksAtEfficiency){
     //       SparkEmitter_Increase(true, ((v_Engine.v_Efficiency * v_SparkEmitterMaxRate) * (1 - v_Engine.v_Efficiency)));
      //  } // END - Start sparks when efficiency reaches below this point.
    }

    void UpdatePanels() {
//        Vector3 v_turretRotationAsEuler = v_Engine.v_Turret.eulerAngles;
//        Debug.Log(v_turretRotationAsEuler.y
//        Quaternion v_TurretRotAsQuaternion = Quaternion.Euler(0, 0, v_turretRotationAsEuler.y * -1);
//        Mathf.LerpAngle(v_turretRotationAsEuler.x,v_turretRotationAsEuler.y,v_turretRotationAsEuler.z);
        v_TankTurret.localRotation = Quaternion.Euler(v_Engine.v_Turret.transform.localEulerAngles);


    } // END - Update Panels.

    void SparkEmitter_Decrease() {
        // Gradually decrease the amount of sparks.
        v_ConsoleSparkEmitter.rate = v_ConsoleSparkEmitter.rate.constant - v_SparkEmitterRateDecrease;
    } // END - Spark Emitter Decrease.

    void SparkEmitter_Increase(bool p_AddElseSet, float p_increaseAmmount){
        if (p_AddElseSet) {
            // Increases/Adds to the emitter rate by the ammount specified.
            v_ConsoleSparkEmitter.rate = v_SparkEmitterMaxRate + p_increaseAmmount;
        } // END - Increase/Add.
        else { // Set the emitter rate to the ammount specified.
            v_ConsoleSparkEmitter.rate = p_increaseAmmount;
        } // END - Set.
        if(v_ConsoleSparkEmitter.rate.constant >= v_SparkEmitterMaxRate){
            v_ConsoleSparkEmitter.rate = v_SparkEmitterMaxRate;
        } // END - Clamp values after add.
    } // END - SparkEmitter Increase.


    public void SwapMainScreen() {
        Debug.Log("hit");
        if (v_GunView.activeSelf) {
            v_GunView.SetActive(false);
            v_Viewfinder.SetActive(true);
        } // END - if Gunview is active screen.
        else {
            v_GunView.SetActive(true);
            v_Viewfinder.SetActive(false);
        } // END - If else.
    } // END - Swap main screen.
} // END - MonoBehavior.