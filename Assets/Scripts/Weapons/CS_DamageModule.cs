/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: [Multiple]
INBOUND REFERENCES: CS_GunShell* | CS_Rocket*
OUTBOUND REFERENCES: Null
OVERVIEW: The script is a module that manages damage applied to the attached object.
*/

using UnityEngine;
using System.Collections;

public class CS_DamageModule : MonoBehaviour {

    // VARIABLES:
    [Space(15)]
    [Header("GENERAL SETTINGS:")]
    [Tooltip("Will disable the object rather than destroying it.")]public bool v_DisableOverDestroy;
    public bool v_ExplodeOnDestroyed;
    public GameObject go_Explosion;
    public bool v_DetatchFromParent;
    [Tooltip("Time in seconds after object is classed as destroyed for the GameObject to be destroyed.")]public float v_ObjectDestructionDelay;
    float v_DamageSustained; // The amount of damage currently sustained by the module.
    bool v_Destroyed; // Flag to ensure GameObject.Destroy is only ran once, not recursively.
    [Space(15)]
    [Header("MASS SETTINGS:")]
    [Tooltip("If true, the module will use the mass as 'health'.  \nREQUIRES RIGIDBODY!")]public bool v_UseMass;
    [Tooltip("MULTIPLIED AGAINST INITIAL MASS! \nDamage taken reduces mass. \nWhen the mass of the object has lost the amount (result of this value), it is destroyed. \n\nLOWER VALUES RESULTS IN LESS DAMAGE SUSTAINABILITY.")][Range(0.1f, 0.9f)] public float v_MinMassMultiplier = 0.1f;
    float v_MinimumMass; // The calculated minimum mass value.
    Rigidbody v_Rigidbody;
    [Space(15)]
    [Header("HEALTH SETTINGS:")]
    [Tooltip("Used if 'UseMass' is FALSE")]public int v_ModuleHealth;
    bool v_PlasmaDamageOverTime; // If true, will apply plasma damage over time.
    float v_DamageOverTimeLeft; // Remaining time of DoT.
    float v_PlasmaSubDamage; // How much damage to apply.
    [Space(15)]
    [Header("DAMAGE MODIFIER SETTINGS:")]
    [Tooltip("Values below 1 will reduce damage.  Values above 1 increase damage \n0 Makes the object immune from this damage type!")][Range(0, 3)] public float v_KineticMultiplier = 1;
    [Tooltip("Values below 1 will reduce damage.  Values above 1 increase damage \n0 Makes the object immune from this damage type!")][Range(0, 3)] public float v_ExplosionMultiplier = 1;
    [Tooltip("Values below 1 will reduce damage.  Values above 1 increase damage \n0 Makes the object immune from this damage type!")][Range(0, 3)] public float v_PlasmaMultiplier = 1;
    // END - Variables.

    // Use this for initialization
    private void Awake(){
        // CHECK:  If rigidbody is NULL: create rigidbody.
        if(v_UseMass){
            v_Rigidbody = GetComponent<Rigidbody>();
            if (v_Rigidbody == null){ // If no rigidbody is attached, create one.
                v_Rigidbody = this.gameObject.AddComponent<Rigidbody>();
                v_Rigidbody.mass = 100;
            } // END - If rigidbody = NULL.
            v_MinimumMass = v_Rigidbody.mass * v_MinMassMultiplier;
        } // END - If Use Mass

        // Debug Error if all multipliers are 0.
        if(v_KineticMultiplier == 0 && v_ExplosionMultiplier == 0 && v_PlasmaMultiplier == 0) { Debug.LogError("WARNING! " + this.gameObject +" Is Immune to ALL types of damage!  Kinetic set to 0.1."); v_KineticMultiplier = 0.1f; }
    } // END - Awake.
	
	// Update is called once per frame
	void Update () {
        ApplyPlasmaDamageOverTime();
	} // END - Update.

    private void LateUpdate(){
        HealthCheck();
    } // END - Late Update.

    public void ApplyKineticDamage(float p_DamageToApply) {
        v_DamageSustained += p_DamageToApply * v_KineticMultiplier;
    } // END - Apply KINETIC Damage.

    public void ApplyPlasmaDamage(float p_DamageToApply, float p_SubDamage, float p_EffectDuration) {
        v_PlasmaDamageOverTime = true; // ENABLE Damage Over Time.
        
        // Allow plasma effects to be stacked:
        v_PlasmaSubDamage += p_SubDamage;
        v_DamageOverTimeLeft += p_EffectDuration;
        v_DamageSustained += p_DamageToApply * v_PlasmaMultiplier;

//        print("PLASMA HIT! " + v_DamageSustained);
    } // END - Apply plasma damage.


    void ApplyPlasmaDamageOverTime(){
        if (v_PlasmaDamageOverTime) {
            // If there is time left on DoT:
            if (v_DamageOverTimeLeft >= 0.01f) { 
                v_DamageOverTimeLeft -= 1 * Time.deltaTime; // Decrease time left.
                v_DamageSustained += v_PlasmaSubDamage * v_PlasmaMultiplier; // Apply subdamage from plasma.
            } // END - Decrease time left.
            else { // Set Plasma Damage to FALSE & Reset values.
                v_PlasmaDamageOverTime = false;
                v_PlasmaSubDamage = 0;
            } // END - Reset Plasma DoT.
        } // END - If PlasmaDamageOverTime is TRUE.

    } // END - Apply Plasma DoT.

    public void ApplyExplosionDamage(float p_DamageToApply) {
        v_DamageSustained += p_DamageToApply * v_ExplosionMultiplier;
    } // END - Apply plasma damage.


    void HealthCheck(){
        // IF USING MASS FOR HEALTH:
        if (v_UseMass) {
            if((v_DamageSustained >= v_MinimumMass)) {
                ObjectDestroyed();
            } // END - Damage Sustained greater than mass.
        } // END - Using Mass.
        
        // IF USING STATIC INTEGER FOR HEALTH:
        else if(v_DamageSustained >= v_ModuleHealth) {
            print("Using Health!");
            ObjectDestroyed();
        } // END - NOT using Mass.
    } // END - Health CHeck.

    void ObjectDestroyed() {
        if(v_Destroyed == false) {
            v_Destroyed = true; // This flag ensures that destruction call only occurs ONCE.
            if (v_DetatchFromParent) { gameObject.transform.SetParent(null, true); }
            if (v_ExplodeOnDestroyed) { Instantiate(go_Explosion, transform.position, transform.rotation); }

            // If DisableOverDestroy is true, disable the object, else destroy it.
            if (v_DisableOverDestroy) { gameObject.SetActive(false); }else { 
            Destroy(gameObject, v_ObjectDestructionDelay); }
        } // END - If destroyed is FALSE.
    } // END - Object Destroyed.

} // END - MonoBehaviour.
