/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerProfile
INBOUND REFERENCES: CS_Bridge_WheeledTankWeapons_To_PlayerProfile
OUTBOUND REFERENCES: null
OVERVIEW:  Holds the progress of the player.

ATTENTION: THERE SHOULD ONLY BE ONE INSTANCE OF THIS SCRIPT/OBJECT RUNNING!
*/

using UnityEngine;
using System.Collections;

public class CS_PlayerProfile : MonoBehaviour {
    // VARIABLES:
    public int v_CurrencyOwned;
    public bool v_Headbob;
    public int v_LevelsCompleted;

    [Header("Unlocks:")]
    public bool v_UnlockedTrident;
    public bool v_UnlockedTitan;
    public bool v_UnlockedHellseeker;

    [Header("Upgrades:")]
    public bool v_Upgrade1;

	// Use this for initialization
	void Start () {
        // Ensures the object will not be destroyed.
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
