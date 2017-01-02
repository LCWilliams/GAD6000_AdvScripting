/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_GunShell_00
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Manages the components of weapon shells: intended to be used on objects created via instantiation.
*/

using UnityEngine;
using System.Collections;

public class CS_GunShell_Basic : MonoBehaviour {
    [Header("Shell Characteristics:")]
    public int v_ShellLifetime;
    public float v_ShellPropulsionForce;
    WindZone go_TravelWind; // Will find gameobject named "TravelWind" if not set manually.
    [Header("Bounce:")]
    [Tooltip("Will explode on contact if false")]public bool v_AllowBounce;
    public int v_BouncesAllowed;
    [Tooltip("Multiplier to reduce values below by per bounce")][Range(0f,1f)]public float v_EfficiencyDeduction;

    //VARIABLES
    [Header("KINETIC DAMAGE SETTINGS:")]
    public bool v_ApplyKinetic;
    public float v_KineticDamage;

    [Space(10)]

    [Header("EXPLOSION (& DAMAGE) SETTINGS:")]
    public bool v_ApplyExplosion;
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    public float v_PlasmaRadius;
    [Tooltip("The rocket will explode when its flight time is reached.")]
    public bool v_ExplodeOnFlightEnd;
    public GameObject go_Explosion;

    [Space(10)]

    [Header("PLASMA DAMAGE SETTINGS:")]
    public bool v_ApplyPlasma;
    public float v_PlasmaDamage;
    public float v_PlasmaSubDamage;
    public float v_PlasmaEffectDuration;
    public GameObject go_PlasmaExplosion;

//    public GameObject v_ExplosionEffects;
    ParticleSystem.Particle[] PS_ExplosionParticles;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, v_ShellLifetime);

        if (go_TravelWind == null) { go_TravelWind = transform.GetComponentInChildren<WindZone>(); }

//        if (v_ExplosionEffects == null) { v_ExplosionEffects = GameObject.Find("ExplosionEffects"); }
        //        Destroy(this.gameObject, v_ShellLifetime);
        this.GetComponent<Rigidbody>().AddRelativeForce((Vector3.forward * v_ShellPropulsionForce), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision p_HitObject){
        // Apply Kinetic damage to hit objects:
        if (v_ApplyKinetic) {
            CS_DamageModule v_DamageModule = p_HitObject.collider.GetComponent<CS_DamageModule>();
            if (v_DamageModule != null) { v_DamageModule.ApplyKineticDamage(v_KineticDamage); }
        } // END - Apply Kinetic.

        if (v_AllowBounce) { // Bounce.
            if (v_BouncesAllowed <= 0) { // Check remaining bounce count: no more bounces.
                ShellExplode();
            } else {
                v_BouncesAllowed = v_BouncesAllowed - 1;
                v_ExplosionDamage = v_ExplosionDamage - (v_ExplosionDamage * v_EfficiencyDeduction);
                v_ExplosionRadius = v_ExplosionRadius - (v_ExplosionRadius * v_EfficiencyDeduction);
            } // END - Bounce
        } else { // END - If Allow Bounce / Start: Shell Explode on collision.
            ShellExplode();
        }
    } // END - OnCollisionEnter.


    void ShellExplode(){
        Destroy(gameObject, 2f);
//        v_Exploded = true;
        go_TravelWind.windMain = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(GetComponent<MeshRenderer>());

        if (v_ApplyExplosion) { CreateStandardExplosion(); }
        if (v_ApplyPlasma) { CreatePlasmaExplosion(); }

        

        //Vector3 v_ExplosionOrigin = transform.position;
        //Collider[] v_ObjectsHit = Physics.OverlapSphere(v_ExplosionOrigin, v_ExplosionRadius);
        //foreach (Collider v_ObjectHit in v_ObjectsHit) {
        //    Rigidbody v_ObjectHitRigidbody = v_ObjectHit.GetComponent<Rigidbody>();
        //    if (v_ObjectHitRigidbody != null) {
        //        v_ObjectHitRigidbody.AddExplosionForce(v_ExplosionDamage, v_ExplosionOrigin, v_ExplosionRadius, 0, ForceMode.Impulse);
            //} // END - If object has rigidbody.
        //} // END - for each, explosion loop.
    }  // END - Shell explode.  


    void CreateStandardExplosion(){
        GameObject v_ExplosionInstance = (GameObject)Instantiate(go_Explosion, transform.position, transform.rotation, null);
        // Obtain Explosion Script:
        CS_Explosion_00 ExplosionModule = v_ExplosionInstance.GetComponent<CS_Explosion_00>();
        // Apply settings onto explosion:
        ExplosionModule.v_ExplosionForce = v_ExplosionDamage * v_ExplosionRadius;
        ExplosionModule.v_ExplosionDamage = v_ExplosionDamage;
        ExplosionModule.v_ExplosionRadius = v_ExplosionRadius;
    } // END - Create Standard Explosion.


    void CreatePlasmaExplosion(){
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