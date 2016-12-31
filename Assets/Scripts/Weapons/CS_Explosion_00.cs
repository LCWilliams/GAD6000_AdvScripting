/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_GunShell_00
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Creates an explosion that adds force to rigidbodies and issues explosion damage.
           
            NOTE!
            The object holding this script should be an effect; this effect will then be instantiated when required.
*/

using UnityEngine;
using System.Collections;

public class CS_Explosion_00 : MonoBehaviour {
    // VARIABLES:
    [Header("Explosion Settings:")]
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    public float v_ExplosionForce;

    [Space(10)]
    [Header("General Settings:")]
    [Tooltip("Will destroy itself when there are no live particles.")] public bool v_DestroyOnNoParticles;
    [Tooltip("Will delay the destruction IF DestroyOnNoParticles is true:  Ideal if the effect contains sub-emitters.")]public int v_ParticleBasedDestructionDelay;
    [Tooltip("Timespan in seconds since creation before the object is destroyed")]public int v_Lifetime;
    ParticleSystem v_ParticleEffect;
    // END - Variables.

    // Use this for initialization
    void Start () {
        v_ParticleEffect = GetComponent<ParticleSystem>();
        if (!v_DestroyOnNoParticles) { Destroy(gameObject, v_Lifetime); } // If not using emitter life, use lifetime value.

        Explosion();
    } // END - Start

    void Update(){
        // Destroy the effect once particle count is 0 & UseEmitterLife is true.
        if ((v_ParticleEffect.particleCount == 0) && v_DestroyOnNoParticles) { Destroy(gameObject, v_ParticleBasedDestructionDelay); }
    } // END - Update.

    void Explosion(){
        // Generate a sphere and collect all objects within it:
        Collider[] go_ObjectsHit = Physics.OverlapSphere(this.transform.position, v_ExplosionRadius);

        // Run for loop to recursively effect objects:
        foreach( Collider objectIndex in go_ObjectsHit) {
            CS_DamageModule v_ObjectDamageModule = objectIndex.GetComponent<CS_DamageModule>();
            
            // Apply explosion force if rigidbody is not null:
            Rigidbody v_ObjectRigidbody = objectIndex.GetComponent<Rigidbody>();
            if (v_ObjectRigidbody != null) { v_ObjectRigidbody.AddExplosionForce(v_ExplosionForce, transform.position, v_ExplosionRadius, 0, ForceMode.Impulse); }

            // Apply Explosion Damage:
            if(v_ObjectDamageModule != null && v_ExplosionDamage > 0) { v_ObjectDamageModule.ApplyExplosionDamage(v_ExplosionDamage); }
        } // END - ForEach loop.
    } // END - Explosion.

} // END - Monobehaviour.