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
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CS_MainMenu_00 : MonoBehaviour {
    // VARIABLES:
    public GameObject GUI_MainMenuPanel;
    public GameObject GUI_OptionsPanel;
    public GameObject go_PlayerProfilePREFAB;
    public GameObject go_PlayerProfile;
    CS_PlayerProfile v_PlayerProfile;

    public AudioMixer AM_Music;
    public AudioMixer AM_Effects;

    private void Start() {
        go_PlayerProfile = GameObject.FindGameObjectWithTag("PlayerProfile");
        if(go_PlayerProfile == null){ go_PlayerProfile =  Instantiate(go_PlayerProfilePREFAB); go_PlayerProfile.tag = "PlayerProfile"; }
    }

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

    public void ShowOptions() {
        GUI_OptionsPanel.SetActive(true);
        GUI_MainMenuPanel.SetActive(false);
    }

    public void HideOptions() {
        GUI_OptionsPanel.SetActive(false);
        GUI_MainMenuPanel.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void QuitToDesktop() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    } // END - Quit to desktop.


} // END - Monobehaviour
