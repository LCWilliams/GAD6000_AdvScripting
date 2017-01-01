/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Manages the characteristics of instaniated rockets.
NOTE!  ALWAYS ENSURE THE TRACKER IS THE SECOND CHILD OF THE CONTAINING GAME OBJECT!
*/

using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CS_Rocket_01 : MonoBehaviour
{

    //VARIABLES
    [Header("KINETIC DAMAGE SETTINGS:")]
    public bool v_ApplyKinetic;
    public float v_KineticDamage;
    public bool v_KineticShockwave;
    public float v_KineticShockwaveRadius;

    [Space(10)]

    [Header("EXPLOSION (& DAMAGE) SETTINGS:")]
    public bool v_ApplyExplosion;
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    public float v_PlasmaRadius;
    [Tooltip("The rocket will explode when its flight time is reached.")]
    public bool v_ExplodeOnFlightEnd;
    public GameObject go_Explosion;
    bool v_Exploded; // Flag used to prevent explode on flight end from spamming explosion instances; despite being pretty.

    [Space(10)]

    [Header("PLASMA DAMAGE SETTINGS:")]
    public bool v_ApplyPlasma;
    public float v_PlasmaDamage;
    public float v_PlasmaSubDamage;
    public float v_PlasmaEffectDuration;
    public GameObject go_PlasmaExplosion;

    [Space(10)]

    [Header("FLIGHT CHARACTERISTICS:")]
    [Tooltip("How long the rocket will be 'alive' for, in seconds.")]public float v_RocketLife;
    Stopwatch v_FlightTime; // How long the rocket has been in flight for.
    [Tooltip("Higher speeds will result in less accurate rockets")]public float v_FlightSpeed = 0.1f;
    [Tooltip("How long the rocket will fly straight before initiating persuit: IN MILLISECONDS")]public int v_ClearanceTime;
    [Tooltip("How long the rockets will take to line up: Lower values result in sharper turns, and vice versa.")][Range(0.1f, 10)]public float v_AimTime = 0.1f;
    float v_CurrentAimTime; // How long the missile has currently spent aiming.
    [Range(0.01f, 0.5f)] public float v_AimTimeMultiplier = 0.25f;
    Quaternion v_InitialRotation;

    [Space(10)]

    [Header("TRACKING OBJECTS:")]    // Objects:
    public Transform v_Target;
    [Tooltip("If not set, will get the SECOND child object.")]public GameObject v_Tracker;

    // END - Variables.

    private void Awake(){
        v_FlightTime = new Stopwatch();
        v_FlightTime.Start();
        GetComponent<Collider>().enabled = false;
    } // END - Awake.

    void Start(){
        //        v_Tracker = GameObject.Find("TargetTracker");
        if (v_Tracker == null) { v_Tracker = this.transform.GetChild(1).gameObject; } // Get second child if tracker is NULL.
        v_InitialRotation = transform.rotation;
    } // END - Start


    private void Update(){

        v_Tracker.transform.LookAt(v_Target);

        //        this.transform.Translate(Vector3.forward * v_FlightSpeed);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * v_FlightSpeed, ForceMode.Force);
        UpdateTrackingTimes();


    } // END - Update.

    void LateUpdate() {
        // CLEARENCE TIME: Prevents rocket from tracking immediately.
        if (v_FlightTime.ElapsedMilliseconds >= v_ClearanceTime){
            SetTrackingRotations();
            if ((GetComponent<Collider>().enabled == false)) { GetComponent<Collider>().enabled = true; }
        } // END - If elapsed > Clearance time.
        // Rocket lifetime:
        if (v_FlightTime.Elapsed.TotalSeconds >= v_RocketLife){
            v_FlightTime.Stop();
            if (v_ExplodeOnFlightEnd && v_Exploded == false) {
                v_Exploded = true;
                if (v_ApplyExplosion == true) { CreateStandardExplosion(); }
                if (v_ApplyPlasma) { CreatePlasmaExplosion(); }
                Destroy(gameObject, 1);
            }
            else {
                Destroy(gameObject, 1); }
        } // END - Rocket lifetime.
    } // END - Fixed update.


    // ------------------------------------------------------------------------------------------------------

        void UpdateTrackingTimes() {
        v_CurrentAimTime = Mathf.Clamp01(v_CurrentAimTime + ((v_AimTimeMultiplier * Time.deltaTime) / v_AimTime));
        
        if (v_CurrentAimTime >= v_AimTime){
            v_CurrentAimTime = 0;
            v_InitialRotation = transform.rotation;
        } // END - End of aim time.
    } // END - Update tracking times.

    void SetTrackingRotations() {
        Quaternion v_TargetToAim = Quaternion.Lerp((v_InitialRotation), v_Tracker.transform.rotation, v_CurrentAimTime);
        this.transform.rotation = v_TargetToAim;
        //transform.localRotation = v_TargetToAim;
    } // END - Tracking.


    // ------------------------------------------------------------------------------------------------------

    private void OnCollisionEnter(Collision p_Collision){
        CS_DamageModule v_HitObjectDamageModule = p_Collision.gameObject.GetComponent<CS_DamageModule>();
        // Apply kinetic damage immediately:
        if (v_KineticShockwave) { 
        if(v_HitObjectDamageModule != null && v_ApplyKinetic) {
            Collider[] v_ObjectsHit = Physics.OverlapSphere(transform.localPosition, v_KineticShockwaveRadius);
                for(int objectHitIndex = 0; objectHitIndex > v_ObjectsHit.Length; objectHitIndex++) {
                    CS_DamageModule v_ShockWaveHitDamageModule = v_ObjectsHit[objectHitIndex].gameObject.GetComponent<CS_DamageModule>();
                    v_ShockWaveHitDamageModule.ApplyKineticDamage(v_KineticDamage);
                }// END - Kinetic shockwave for loop.
            }
            else { 
            v_HitObjectDamageModule.ApplyKineticDamage(v_KineticDamage); }
        }

        // Apply Explosion (Instantiate).
        if (v_ApplyExplosion == true) { CreateStandardExplosion(); }

        // Apply Plasma Damage (Instantiate).
        if (v_ApplyPlasma) { CreatePlasmaExplosion(); }

        Destroy(gameObject);
    } // END - On collision Enter.

    void CreateStandardExplosion() {
            GameObject v_ExplosionInstance = (GameObject) Instantiate(go_Explosion, transform.position, transform.rotation, null);
                // Obtain Explosion Script:
                CS_Explosion_00 ExplosionModule = v_ExplosionInstance.GetComponent<CS_Explosion_00>();
                // Apply settings onto explosion:
                ExplosionModule.v_ExplosionForce = v_ExplosionDamage * v_ExplosionRadius;
                ExplosionModule.v_ExplosionDamage = v_ExplosionDamage;
                ExplosionModule.v_ExplosionRadius = v_ExplosionRadius;
    } // END - Create Standard Explosion.


    void CreatePlasmaExplosion() {
        //v_HitObjectDamageModule.ApplyPlasmaDamage(v_PlasmaDamage, v_PlasmaDamageOverTime, v_PlasmaEffectDuration

        // Instantiate an explosion:
        GameObject v_PlasmaExplosionInstance = (GameObject)Instantiate(go_PlasmaExplosion, transform.position, transform.rotation, null);
        // Obtain Explosion Script:
        CS_PlasmaExplosion_00 PlasmaExplosionModule = v_PlasmaExplosionInstance.GetComponent<CS_PlasmaExplosion_00>();
        // Apply settings onto explosion:
        PlasmaExplosionModule.v_PlasmaExplosionRadius = v_PlasmaRadius;
        PlasmaExplosionModule.v_PlasmaExplosionDamage = v_PlasmaDamage;
        PlasmaExplosionModule.v_PlasmaExplosionSubDamage = v_PlasmaSubDamage;
        PlasmaExplosionModule.v_PlasmaExplosionEffectDuration = v_PlasmaEffectDuration;
    } // END - Create Plasma Explosion.

} // END - Monobehaviour.