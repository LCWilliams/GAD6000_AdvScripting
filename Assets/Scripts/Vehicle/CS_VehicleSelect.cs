using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CS_VehicleSelect : MonoBehaviour {

    public GameObject go_PlayerProfile;
    CS_PlayerProfile v_PlayerProfile;


    // Use this for initialization
    void Start () {
        go_PlayerProfile = GameObject.FindGameObjectWithTag("PlayerProfile");
        v_PlayerProfile = go_PlayerProfile.GetComponent<CS_PlayerProfile>();
	}
	
    public void SelectBasilisk() {
        v_PlayerProfile.v_Vehicle = 0;
        SceneManager.LoadSceneAsync(2);
    }

    public void SelectLeviathan(){
        v_PlayerProfile.v_Vehicle = 1;
        SceneManager.LoadSceneAsync(2);
    }


}
