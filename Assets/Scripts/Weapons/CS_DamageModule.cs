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
    public bool v_ExplodeOnDestroyed;
    public GameObject go_Explosion;
    public bool v_DetatchFromParent;
    [Tooltip("Time in seconds after object is classed as destroyed for the GameObject to be destroyed.")]public float v_ObjectDestructionDelay;
    float v_DamageSustained; // The amount of damage currently sustained by the module.
    bool v_Destroyed; // Flag to ensure GameObject.Destroy is only ran once, not recursively.
    [Space(15)]
    [Header("MASS SETTINGS:")]
    [Tooltip("If true, the module will use the mass as 'health'.  \nREQUIRES RIGIDBODY!")]public bool v_UseMass;
    [Tooltip("If true, mass will decrease from damage.")] public bool v_AffectsMass;
    [Tooltip("The minimum amount of mass until the holder is declared destroyed. (Multiplier: from initial mass). \n\nLower number equates to higher damage taking")][Range(0.1f, 0.9f)] public float v_MinMassMultiplier = 0.1f;
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
    [Tooltip("Module will take increased damage from KINETIC/IMPACT weapons")] public bool v_KineticWeakness;
    [Range(1.2f, 3)] public float v_KineticMultiplier = 1.2f;
    [Tooltip("Module will take increased damage from EXPLOSION weapons")] public bool v_ExplosionWeakness;
    [Range(1.2f, 3)] public float v_ExplosionMultiplier = 1.2f;
    [Tooltip("Module will take increased damage from PLASMA weapons")]public bool v_PlasmaWeakness;
    [Range(1.2f, 3)] public float v_PlasmaMultiplier = 1.2f;
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
    } // END - Awake.
	
	// Update is called once per frame
	void Update () {
        ApplyPlasmaDamageOverTime();
	} // END - Update.

    private void LateUpdate(){
        HealthCheck();
    } // END - Late Update.

    public void ApplyKineticDamage(float p_DamageToApply) {
        if (v_KineticWeakness) { // IF Kinetic weakness is true, multiply by specified value.
            v_DamageSustained += p_DamageToApply * v_KineticMultiplier;
        }// END - Weakness modified damage.
        else { v_DamageSustained += p_DamageToApply; } // STANDARD APPLY        
    } // END - Apply KINETIC Damage.

    public void ApplyPlasmaDamage(float p_DamageToApply, float p_SubDamage, float p_EffectDuration) {
        v_PlasmaDamageOverTime = true; // ENABLE Damage Over Time.
        
        // Allow plasma effects to be stacked:
        v_PlasmaSubDamage += p_SubDamage;
        v_DamageOverTimeLeft += p_EffectDuration; 

        if (v_PlasmaWeakness){ // IF PLASMA weakness is true, multiply by specified value.
            v_DamageSustained += p_DamageToApply * v_PlasmaMultiplier;
        }// END - Weakness modified damage.
        else { v_DamageSustained += p_DamageToApply; } // STANDARD APPLY

    } // END - Apply plasma damage.


    void ApplyPlasmaDamageOverTime(){
        if (v_PlasmaDamageOverTime) {
            if(v_DamageOverTimeLeft >= 0.1f) {
                v_DamageOverTimeLeft -= 1 * Time.deltaTime; // Decrease time left.
                v_DamageSustained += v_PlasmaSubDamage; // Apply subdamage from plasma.
            } // END - Decrease time left.
            else { // Set Plasma Damage to FALSE & Reset values.
                v_PlasmaDamageOverTime = false;
                v_PlasmaSubDamage = 0;
            } // END - Reset Plasma DoT.
        } // END - If PlasmaDamageOverTime is TRUE.
    } // END - Apply Plasma DoT.

    public void ApplyExplosionDamage(float p_DamageToApply) {
        if (v_ExplosionWeakness) { // IF EXPLOSION weakness is true, multiply by specified value.
            v_DamageSustained += p_DamageToApply * v_ExplosionMultiplier;
        } // END - Weakness modified damage.
        else { v_DamageSustained += p_DamageToApply; } // STANDARD APPLY        
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
            ObjectDestroyed();
        } // END - NOT using Mass.
    } // END - Health CHeck.

    void ObjectDestroyed() {
        if(v_Destroyed == false) {
            v_Destroyed = true; // This flag ensures that destruction call only occurs ONCE.
            Destroy(gameObject, v_ObjectDestructionDelay);

            if (v_DetatchFromParent) { transform.SetParent(null, true); }
            if (v_ExplodeOnDestroyed) { Instantiate(go_Explosion, transform.position, transform.rotation); }

        } // END - If destroyed is FALSE.
    } // END - Object Destroyed.

} // END - MonoBehaviour.
