/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_GunShell_00
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Damages objects within the explosion radius with Plasma Damage.
           
            NOTE!
            The object holding this script should be an effect; this effect will then be instantiated when required.
*/

using UnityEngine;
using System.Collections;

public class CS_PlasmaExplosion_00 : MonoBehaviour {
    // VARIABLES:
    [Header("Explosion Settings:")]
    public float v_PlasmaExplosionDamage;
    public float v_PlasmaExplosionRadius;
    public float v_PlasmaExplosionSubDamage;
    public float v_PlasmaExplosionEffectDuration;

    [Header("General Settings:")]
    [Tooltip("Will destroy itself when there are no live particles.")] public bool v_DestroyOnNoParticles;
    [Tooltip("Will delay the destruction IF DestroyOnNoParticles is true:  Ideal if the effect contains sub-emitters.")]public int v_ParticleBasedDestructionDelay;
    [Tooltip("Timespan in seconds since creation before the object is destroyed")]public int v_Lifetime;
    ParticleSystem v_ParticleEffect;
    // END - Variables.


    // Use this for initialization
    void Start(){
        v_ParticleEffect = GetComponent<ParticleSystem>();
        if (!v_DestroyOnNoParticles) { Destroy(gameObject, v_Lifetime); } // If not using emitter life, use lifetime value.


        // Generate a sphere and collect all objects within it:
        Collider[] go_ObjectsHit = Physics.OverlapSphere(this.transform.position, v_PlasmaExplosionRadius);
        // Run for loop to recursively effect objects:
        foreach (Collider objectIndex in go_ObjectsHit){
        // Apply Plasma Damage:
            CS_DamageModule v_ObjectDamageModule = objectIndex.GetComponent<CS_DamageModule>();
            if (v_ObjectDamageModule != null) { v_ObjectDamageModule.ApplyPlasmaDamage(v_PlasmaExplosionDamage, v_PlasmaExplosionSubDamage, v_PlasmaExplosionEffectDuration); }
        } // END - ForEach loop.


    } // END - Start


    private void Update(){
        PlasmaDamageOverTime();
        // Destroy the effect once particle count is 0 & UseEmitterLife is true.
        if ((v_ParticleEffect.particleCount == 0) && v_DestroyOnNoParticles) { Destroy(gameObject, v_ParticleBasedDestructionDelay); }
    } // END - Fixed Update.

    // DOES NOT INCLUDE INITIAL DAMAGE WHEN APPLYING PLASMA DAMAGE!
    void PlasmaDamageOverTime(){
        Collider[] go_ObjectsHit = Physics.OverlapSphere(this.transform.position, v_PlasmaExplosionRadius);
        foreach (Collider objectIndex in go_ObjectsHit){
            CS_DamageModule v_ObjectDamageModule = objectIndex.GetComponent<CS_DamageModule>();
            if (v_ObjectDamageModule != null) { v_ObjectDamageModule.ApplyPlasmaDamage(0, v_PlasmaExplosionSubDamage * Time.deltaTime, (v_PlasmaExplosionEffectDuration * 0.1f) * Time.deltaTime); }
        } // END - ForEach loop.
    } // END - Explosion.

} // END - Monobehaviour.