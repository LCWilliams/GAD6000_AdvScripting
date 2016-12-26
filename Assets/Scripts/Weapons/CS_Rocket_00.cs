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
[RequireComponent(typeof(Rigidbody))]

public class CS_Rocket_00 : MonoBehaviour {

    //VARIABLES
    [Header("Explosion Attributes")]
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    [Tooltip("The rocket will explode when its flight time is reached.")]
    public bool v_ExplodeOnFlightEnd;

    [Header("Flight Characteristics:")][Space(10)]
    [Tooltip("How long the rocket will be 'alive' for")]
    public float v_MaxFlightTime;
    Stopwatch v_FlightTime; // How long the rocket has been in flight for.
    public float v_FlightSpeed;
    [Range(0, 90)]
    public float v_MaximumTurnAngle;
    public float v_CorrectionTime; // Number of seconds for the rocket to correct its course.
    public float v_CurrentCorrection; // How many seconds have currently elapsed. (used as lerp).
    [Tooltip("How long the rocket will fly straight before initiating persuit: IN MILLISECONDS")] public int v_ClearanceTime;

    // Tracking:
    float v_Xrotation;
    float v_Yrotation;
    float v_Zrotation;

    // Objects:
    public Transform v_Target;
    public Vector3 v_InitialPosition;
    public Vector3 v_TargetNewPosition;
    Rigidbody v_RocketRigidbody;
    GameObject v_Tracker;
    // END - Variables.

    private void Awake() {
        v_RocketRigidbody = GetComponent<Rigidbody>();
        v_FlightTime = new Stopwatch();
        v_FlightTime.Start();
    } // END - Awake.

    void Start() {
        v_Tracker = GameObject.Find("TargetTracker");
    } // END - Start


    private void Update() {
        // Rocket lifetime:
        if (v_FlightTime.Elapsed.TotalSeconds >= v_MaxFlightTime) {
            if (v_ExplodeOnFlightEnd) { /* SPLOSION! */ } // If explode on flight end is enabled, run explosion function.
            else { Destroy(this.gameObject); } // If not: destroy gameobject.
        } // END - Rocket lifetime.

        // CLEARENCE TIME: Prevents rocket from tracking immediately.
        if (v_FlightTime.ElapsedMilliseconds >= v_ClearanceTime) {
            // Set position one, to be used in lerp inside tracking.
            v_InitialPosition = this.transform.rotation.eulerAngles;
            Tracking();
        } // END - If elapsed > Clearance time.

    } // END - Update.


    private void FixedUpdate() {
        v_RocketRigidbody.AddRelativeForce(Vector3.forward * v_FlightSpeed, ForceMode.Force);
        v_Tracker.transform.LookAt(v_Target);
//        ApplyTrackingRotation();
    } // END - Fixed update.

    // ------------------------------------------------------------------------------------------------------

    void Tracking() {
        
        v_CurrentCorrection += 1 * Time.deltaTime / v_CorrectionTime;

        v_TargetNewPosition = v_Target.position;
        //        v_ClearanceTime
        Vector3 v_lookAtTarget = Vector3.Lerp(v_InitialPosition, v_TargetNewPosition, v_CurrentCorrection);
//        Quaternion v_TargetRotAsQuat = 
//        this.transform.rotation = v_lookAtTarget;

    } // END - Tracking.

    void ApplyTrackingRotation(){

        Quaternion v_RotAsQuaternion = Quaternion.Euler(v_Xrotation, v_Yrotation, 0);
        Vector3 v_RotAsVector3 = v_RotAsQuaternion.eulerAngles;

        //this.transform.Rotate(v_RotAsVector3);
        //v_RocketRigidbody.MoveRotation(v_RotAsQuaternion);
        v_RocketRigidbody.AddTorque(v_RotAsVector3);


    } // END -- Apply tracking Rot.

} // END - Monobehaviour.