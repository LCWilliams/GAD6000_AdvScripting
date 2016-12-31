/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | ((AIDriver))
OUTBOUND REFERENCES: null.
*/

using UnityEngine;
using System.Collections;

public class CS_WheelManager : MonoBehaviour {
    //Numural.
    public int v_TotalWheels;
    public float v_Steering;

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


	void Start () {
        v_TotalWheels = (v_WheelsFront.Length + v_WheelsMid.Length + v_WheelsRear.Length);
	} // END - Start


    // Recursively update graphical wheel to match wheel collider:
    void FixedUpdate() {
        if(v_WheelsFront.Length != 0) { 
        for (int WheelIndex = 0; WheelIndex < v_WheelsFront.Length; WheelIndex++) {
            Transform VisualWheel = v_WheelsFront[WheelIndex].GetComponent<Collider>().transform.GetChild(0);
            Vector3 v_position;
            Quaternion v_rotation;
            v_WheelsFront[WheelIndex].GetWorldPose(out v_position, out v_rotation);

            VisualWheel.position = v_position;
            VisualWheel.rotation = v_rotation;
        } // END -- Front wheel position update
        }

        if (v_WheelsMid.Length != 0) { 
        for (int WheelIndex = 0; WheelIndex < v_WheelsMid.Length; WheelIndex++) {
            Transform VisualWheel = v_WheelsMid[WheelIndex].GetComponent<Collider>().transform.GetChild(0);
            Vector3 v_position;
            Quaternion v_rotation;
            v_WheelsMid[WheelIndex].GetWorldPose(out v_position, out v_rotation);

            VisualWheel.position = v_position;
            VisualWheel.rotation = v_rotation;
        } // END -- Mid wheel position update
    }

        if(v_WheelsRear.Length != 0) { 
        for (int WheelIndex = 0; WheelIndex < v_WheelsRear.Length; WheelIndex++){
            Transform VisualWheel = v_WheelsRear[WheelIndex].GetComponent<Collider>().transform.GetChild(0);
            Vector3 v_position;
            Quaternion v_rotation;
            v_WheelsRear[WheelIndex].GetWorldPose(out v_position, out v_rotation);

            VisualWheel.position = v_position;
            VisualWheel.rotation = v_rotation;
            }
        } // END -- Rear wheel position update

    } // END - FixedUpdate.

} // END - Monobehaviour.