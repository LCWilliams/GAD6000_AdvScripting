/*
AUTHOR(S): SCOTT ANDERS 
EDITOR(S): LEE WILLIAMS
SCRIPT HOLDERS: 
INBOUND REFERENCES: 
OUTBOUND REFERENCES: 
OVERVIEW:  Get the tag of the AI vehicles, create an array and load the win scene if no enemies remain
*/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_EnemiesLeft : MonoBehaviour {
    public GameObject[] v_EnemiesLeft;
	// Use this for initialization
	void Start () {
        v_EnemiesLeft = GameObject.FindGameObjectsWithTag("Entity");
    }
	
	// Update is called once per frame
	void Update () {
	    if (v_EnemiesLeft.Length == 0)
        {
            SceneManager.LoadScene("Win");
        }
	}

    void FixedUpdate()
    {
        Debug.Log("Enemies Left=" + v_EnemiesLeft);
    }
}
