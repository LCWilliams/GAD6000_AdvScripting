﻿/*
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
    [Tooltip("Will create explosion at end of shell life")]public bool v_ExplodeOnDestroy;
    [Header("Bounce:")]
    [Tooltip("Will explode on contact if false")]public bool v_AllowBounce;
    public int v_BouncesAllowed;
    [Tooltip("Multiplier to reduce values below by per bounce")][Range(0f,1f)]public float v_EfficiencyDeduction;
    [Header("Explosion:")]
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    public AudioClip v_ExplosionAudio;
    public GameObject v_ExplosionEffects;

	// Use this for initialization
	void Start () {
        if (v_ExplosionEffects == null) { v_ExplosionEffects = GameObject.Find("ExplosionEffects"); }
        //        Destroy(this.gameObject, v_ShellLifetime);
        this.GetComponent<Rigidbody>().AddForce((Vector3.forward * v_ShellPropulsionForce), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision p_HitObject){
        if (v_AllowBounce) { // Bounce.
            if (v_BouncesAllowed <= 0) { // Check remaining bounce count: no more bounces.
                ShellExplode(v_ExplosionDamage, v_ExplosionRadius);
            } else {
                v_BouncesAllowed = v_BouncesAllowed - 1;
                v_ExplosionDamage = v_ExplosionDamage - (v_ExplosionDamage * v_EfficiencyDeduction);
                v_ExplosionRadius = v_ExplosionRadius - (v_ExplosionRadius * v_EfficiencyDeduction);
            } // END - Bounce
        } else { // END - If Allow Bounce / Start: Shell Explode on collision.
            ShellExplode(v_ExplosionDamage, v_ExplosionRadius);
        }
    } // END - OnCollisionEnter.


    void ShellExplode(float p_damage, float p_radius){
        this.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(this.GetComponent<MeshRenderer>());
        v_ExplosionEffects.SetActive(true);

        Vector3 v_ExplosionOrigin = transform.position;
        Collider[] v_ObjectsHit = Physics.OverlapSphere(v_ExplosionOrigin, v_ExplosionRadius);
        foreach (Collider v_ObjectHit in v_ObjectsHit) {
            Rigidbody v_ObjectHitRigidbody = v_ObjectHit.GetComponent<Rigidbody>();
            if (v_ObjectHitRigidbody != null) {
                v_ObjectHitRigidbody.AddExplosionForce(v_ExplosionRadius * v_ExplosionDamage, v_ExplosionOrigin, v_ExplosionRadius, 0, ForceMode.Impulse);
            } // END - If object has rigidbody.
        } // END - for each, explosion loop.

        
        // Destroy(this.gameObject);
    }    




} // END - Monobehaviour.