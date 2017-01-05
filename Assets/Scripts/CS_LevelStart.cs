using UnityEngine;
using System.Collections;

public class CS_LevelStart : MonoBehaviour {

    public GameObject go_PlayerProfile;
    CS_PlayerProfile v_PlayerProfile;
    public GameObject go_Basilisk;
    public GameObject go_Leviathan;

    // Use this for initialization
    void Start(){
        go_PlayerProfile = GameObject.FindGameObjectWithTag("PlayerProfile");
        v_PlayerProfile = go_PlayerProfile.GetComponent<CS_PlayerProfile>();

        if(v_PlayerProfile.v_Vehicle == 0) { Instantiate(go_Basilisk); }
        else { Instantiate(go_Leviathan); }
    }

}
