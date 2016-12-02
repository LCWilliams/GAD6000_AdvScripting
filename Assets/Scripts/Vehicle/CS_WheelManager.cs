/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | ((AIDriver))
OUTBOUND REFERENCES: CS_VehicleEngine
*/

using UnityEngine;
using System.Collections;

public class CS_WheelManager : MonoBehaviour {
    // VARIABLES
    
    //Scripts:
    CS_VehicleEngine Engine; // Engine script attached to this vehicle.

    //Wheel Configuration Arrays.
    [Header("Front Wheels")]
    public GameObject[] v_WheelsFront;
    public bool v_PowerToFront; // applies driving power to FRONT wheels.
    public bool v_SteeringFront;
    public bool v_BrakesFront;

    [Header("Middle Wheels")]
    public bool v_PowerToMid; // Applies driving power to MIDDLE wheels.
    public bool v_BrakesMid;
    public GameObject[] v_WheelsMid;

    [Header("Rear Wheels")]
    public bool v_PowerToRear; // Applies driving power to REAR wheels.
    public bool v_SteeringRear;
    public bool v_BrakesRear;
    public GameObject[] v_WheelsRear;


    // Use this for initialization
	void Start () {
        Engine = GetComponent<CS_VehicleEngine>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void Gears() {
        /*
        Gear 1 : Torque : 
        */
    }
}
