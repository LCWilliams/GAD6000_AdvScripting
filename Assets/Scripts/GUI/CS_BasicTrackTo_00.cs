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
    public bool v_TargetIsPlayer;
    GameObject go_Player;
    [Tooltip("Defaults to item tagged PLAYER if not specified.")]public GameObject go_Target;
    [Tooltip("Will default to current object if not specified.")]public GameObject go_ItemToRotate;
    CS_PlayerDriver v_PlayerDriver;

    // END - Variables.

    private void Start(){
        if(go_ItemToRotate == null) {
            go_ItemToRotate = gameObject;
        }
        if(go_Target == null) {
            go_Target = GameObject.FindGameObjectWithTag("Player");
        } // END - Null target.

        if (v_TargetIsPlayer) { go_Player = GameObject.FindGameObjectWithTag("Player"); v_PlayerDriver = go_Player.GetComponent<CS_PlayerDriver>(); }

    } // END - Start

    // Update is called once per frame
    void Update () {
        if (v_TargetIsPlayer) { go_Target = v_PlayerDriver.go_CurrentCamera.gameObject; }
//        go_ItemToRotate.transform.LookAt(go_Target.transform.position);
        go_ItemToRotate.transform.rotation = Quaternion.LookRotation(transform.position - go_Target.transform.position);
	}
}
