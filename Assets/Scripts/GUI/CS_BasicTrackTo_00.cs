/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | CS_AIDriver
OUTBOUND REFERENCES: null.
OVERVIEW:  Uses LookAt to rotate the current items transform to the players direction.
*/

using UnityEngine;
using System.Collections;

public class CS_BasicTrackTo_00 : MonoBehaviour {

    // Variables:
    [Tooltip("Defaults to item tagged PLAYER if not specified.")]public GameObject go_PlayerCamera;
    [Tooltip("Will default to current object if not specified.")]public GameObject go_ItemToRotate;

    // END - Variables.

    private void Start(){
        if(go_ItemToRotate == null) {
            go_ItemToRotate = this.gameObject;
        }
        if(go_PlayerCamera == null) {
            go_PlayerCamera = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update () {
        go_ItemToRotate.transform.LookAt(go_PlayerCamera.transform);
	}
}
