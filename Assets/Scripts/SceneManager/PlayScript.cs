using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        SceneManager.LoadScene("L_Menu_Main");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoldCar()
    {
        SceneManager.LoadScene("L_TestEnvironment");
    }

    public void GreyCar()
    {

        SceneManager.LoadScene("L_Menu_Main");
    }

    public void CarSelect()
    {

        SceneManager.LoadScene("CarSelect");
    }
}
