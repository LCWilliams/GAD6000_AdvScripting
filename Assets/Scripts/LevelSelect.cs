using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    

        // Use this for initialization
        void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Level_1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Level_2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level_3()
    {
        SceneManager.LoadScene("Level 3");
    }
}
