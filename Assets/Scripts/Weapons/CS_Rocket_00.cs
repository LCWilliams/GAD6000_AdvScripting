/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver
OUTBOUND REFERENCES: CS_VehicleEngine
OVERVIEW:  Manages the characteristics of instaniated missiles.
*/

using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]

public class CS_Rocket_00 : MonoBehaviour {

    //VARIABLES
    [Header("Explosion Attributes")]
    public float v_ExplosionDamage;
    public float v_ExplosionRadius;
    [Header("Flight Characteristics:")]
    public float v_FlightSpeed;
    [Range(0, 90)]public float v_MaximumTurnAngle;
    [Tooltip("How long the missile will fly straight before initiating persuit")] public float v_ClearanceTime;

    // Objects:
    public Transform v_Target;
    Rigidbody v_RocketRigidbody;

    // END - Variables.


    void Start () {
        v_RocketRigidbody = GetComponent<Rigidbody>();
	}
	
	void Update () {
	
	}
}
