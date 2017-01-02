/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver
OUTBOUND REFERENCES: CS_VehicleEngine | CS_WheeledTankWeapons_00
OVERVIEW:  Moves the Rocket Target UI (held on a GameObject) to the current active target.
*/


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CS_RocketTargetUI_00 : MonoBehaviour {

    public GameObject go_Target;
    public Image GUI_Image;
    public Canvas GUI_Canvas;

    // Use this for initialization
    void Awake(){
        GUI_Image = GetComponent<Image>();
        GUI_Canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(go_Target != null) {
            transform.position = go_Target.transform.position;
            GUI_Image.color = new Color(1, 0, 0, 0.8f);
        }
        else {
            GUI_Image.color = new Color(0,0,0,0);
        }
	}
}
