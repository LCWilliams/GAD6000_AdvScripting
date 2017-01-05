using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
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
