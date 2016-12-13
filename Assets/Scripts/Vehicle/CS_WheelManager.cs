/*
AUTHOR(S): LEE WILLIAMS 8======DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | ((AIDriver))
OUTBOUND REFERENCES: CS_VehicleEngine
*/

using UnityEngine;
using System.Collections;

public class CS_WheelManager : MonoBehaviour {
    //Numural.
    public int v_TotalWheels;
    public float v_Steering;
    //Scripts:
    CS_VehicleEngine Engine; // Engine script attached to this vehicle.

    //Wheel Configuration Arrays.
    [Header("Front Wheels")]
    public WheelCollider[] v_WheelsFront;
    public bool v_PowerToFront; // applies driving power to FRONT wheels.
    public bool v_SteeringFront; // Allows front steering.
    public bool v_BrakesFront; // Allows front braking.

    [Header("Middle Wheels")]
    public WheelCollider[] v_WheelsMid;
    public bool v_PowerToMid; // Applies driving power to MIDDLE wheels.
    public bool v_BrakesMid; // Allows mid braking.

    [Header("Rear Wheels")]
    public WheelCollider[] v_WheelsRear;
    public bool v_PowerToRear; // Applies driving power to REAR wheels.
    public bool v_SteeringRear; // Allows rear-steering.
    public bool v_BrakesRear; // Allows rear braking.


    // Use this for initialization
	void Start () {
        Engine = GetComponent<CS_VehicleEngine>();
        v_TotalWheels = (v_WheelsFront.Length + v_WheelsMid.Length + v_WheelsRear.Length);
	} // END - Start
	
	// Update is called once per frame
	void FixedUpdate () {
	    for(int frontWheelIndex = v_WheelsFront.Length; frontWheelIndex < v_WheelsFront.Length; frontWheelIndex++) {
            //v_WheelsFront[frontWheelIndex].transform;
        } // END -- Front wheel position update
	} // END - FixedUpdate.

} // END - Monobehaviour.