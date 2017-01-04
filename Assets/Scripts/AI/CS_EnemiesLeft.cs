using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
}
