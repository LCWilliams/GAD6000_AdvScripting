/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Manages the characteristics of instaniated rockets.
*/

using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CS_Rocket_01 : MonoBehaviour
{

    //VARIABLES
    [Header("Explosion Attributes")]
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    [Tooltip("The rocket will explode when its flight time is reached.")]
    public bool v_ExplodeOnFlightEnd;

    [Header("Flight Characteristics:")][Space(10)]
    [Tooltip("How long the rocket will be 'alive' for, in seconds.")]public float v_RocketLife;
    Stopwatch v_FlightTime; // How long the rocket has been in flight for.
    [Tooltip("Higher speeds will result in less accurate rockets")][Range(0.1f, 1)]public float v_FlightSpeed;
    [Tooltip("How long the rocket will fly straight before initiating persuit: IN MILLISECONDS")]public int v_ClearanceTime;
    [Tooltip("How long the rockets will take to line up: Lower values result in sharper turns, and vice versa.")][Range(0.1f, 1)]public float v_AimTime;
    float v_CurrentAimTime; // How long the missile has currently spent aiming.
    Quaternion v_InitialRotation;

    // Objects:
    public Transform v_Target;
    GameObject v_Tracker;

    // END - Variables.

    private void Awake(){
        v_FlightTime = new Stopwatch();
        v_FlightTime.Start();
    } // END - Awake.

    void Start(){
        v_Tracker = GameObject.Find("TargetTracker");
        v_InitialRotation = transform.rotation;
    } // END - Start


    private void Update(){

        v_Tracker.transform.LookAt(v_Target);

        this.transform.Translate(Vector3.forward * v_FlightSpeed);


    } // END - Update.

    void FixedUpdate() {
        // CLEARENCE TIME: Prevents rocket from tracking immediately.
        if (v_FlightTime.ElapsedMilliseconds >= v_ClearanceTime){
            Tracking();
        } // END - If elapsed > Clearance time.
        // Rocket lifetime:
        if (v_FlightTime.Elapsed.TotalSeconds >= v_RocketLife){
            v_FlightTime.Stop();
            if (v_ExplodeOnFlightEnd) { /* SPLOSION! */ }
            else {
                Destroy(gameObject, 1); }
        } // END - Rocket lifetime.
    } // END - Fixed update.


    // ------------------------------------------------------------------------------------------------------

    void Tracking() {
        v_CurrentAimTime += Mathf.Clamp(((0.1f * Time.deltaTime / v_AimTime)), 0, 1);
        if(v_CurrentAimTime >= v_AimTime) {
            v_CurrentAimTime = 0;
            v_InitialRotation = transform.rotation;
        } // END - End of aim time.
        Quaternion v_TargetToAim = Quaternion.Lerp(v_InitialRotation, v_Tracker.transform.rotation, v_CurrentAimTime);
        this.transform.rotation = v_TargetToAim;

    } // END - Tracking.

} // END - Monobehaviour.