/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_GunShell_00
INBOUND REFERENCES: CS_WheelManager | CS_PlayerDriver
OUTBOUND REFERENCES: CS_WheelManager
OVERVIEW:  Creates an explosion that adds force to rigidbodies and issues explosion damage.
*/

using UnityEngine;
using System.Collections;

public class CS_Explosion_00 : MonoBehaviour {
    // VARIABLES:
    [Header("Explosion Settings:")]
    public float v_ExplosionDamage = 10;
    public float v_ExplosionRadius = 5;
    public float v_ExplosionForce = 1;

    [Header("General Settings:")]
    [Tooltip("Will use emitter life to determine when to destroy itself")] public bool v_UseEmitterLife;
    [Tooltip("Timespan in seconds since creation before the object is destroyed")]public int v_Lifetime;
    // END - Variables.

	// Use this for initialization
	void Start () {
        Debug.LogError("hee!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
