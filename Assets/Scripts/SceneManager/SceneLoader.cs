using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    static SceneLoader instance = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SceneLoader get_instance()
    {
        return instance;
    }
    public SystemStateMachine systemSM;
	void Start () {
        systemSM = SystemStateMachine.get_instance();
	}

   public void load_appropriate_scene()
    {
        string currentState = systemSM.get_current_state();

        if (currentState == "SPLASHSCREEN")

        {
            SceneManager.LoadScene("SplashScreen");
        }

        else if (currentState == "MENUSCREEN")

        {
            SceneManager.LoadScene("MenuScreen");
        }

        else if (currentState == "CREDITSSCREEN")

        {
            SceneManager.LoadScene("CreditsScreen");
        }

        else if (currentState == "GAMESCREEN")

        {
            SceneManager.LoadScene("GameScreen");
        }

        else if (currentState == "YOUWONSCREEN")

        {
            SceneManager.LoadScene("YouWonScreen");
        }

        else if (currentState == "YOULOSTSCREEN")

        {
            SceneManager.LoadScene("YouLostScreen");
        }
    }
}
