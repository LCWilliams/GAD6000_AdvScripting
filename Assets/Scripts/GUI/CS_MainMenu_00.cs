/*
AUTHOR(S): LEE WILLIAMS     DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerProfile
INBOUND REFERENCES: CS_Bridge_WheeledTankWeapons_To_PlayerProfile
OUTBOUND REFERENCES: null
OVERVIEW:  Provides functionality for the main menu.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CS_MainMenu_00 : MonoBehaviour {
    // VARIABLES:
    GameObject GUI_MainMenuPanel;
    GameObject GUI_OptionsPanel;
    GameObject GUI_CreditsPanel;
    GameObject go_PlayerProfile;
    CS_PlayerProfile v_PlayerProfile;

    public AudioMixer AM_Music;
    public AudioMixer AM_Effects;

    public void ModifyVolume_Music(float p_Volume) {
        AM_Music.SetFloat("volume_Music", p_Volume);
    }

    public void ModifyVolume_Effects(float p_Volume) {
        AM_Effects.SetFloat("volume_Effects", p_Volume);
    }

    public void ResetPlayer() {
        v_PlayerProfile = go_PlayerProfile.GetComponent<CS_PlayerProfile>();
        v_PlayerProfile.v_CurrencyOwned = 0;
        v_PlayerProfile.v_UnlockedTrident = false;
        v_PlayerProfile.v_UnlockedTitan = false;
        v_PlayerProfile.v_UnlockedHellseeker = false;
        v_PlayerProfile.v_LevelsCompleted = 0;
} // END- Player reset.

    public void Debug_UnlockAll() {
        v_PlayerProfile.v_UnlockedTrident = true;
        v_PlayerProfile.v_UnlockedTitan = true;
        v_PlayerProfile.v_UnlockedHellseeker = true;
        v_PlayerProfile.v_LevelsCompleted = 3;
    }

    public void ToggleHeadbob(bool p_HeadbobEnabled) {
        v_PlayerProfile.v_Headbob = p_HeadbobEnabled;
    }


    public void QuitToDesktop() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    } // END - Quit to desktop.


} // END - Monobehaviour
