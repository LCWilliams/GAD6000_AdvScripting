using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {
    public Text currentStateText;
    public SystemStateMachine systemSM;

	// Use this for initialization
	void Start () {
        systemSM = SystemStateMachine.get_instance();
	}
	
	// Update is called once per frame
	void Update () {
        currentStateText.text = systemSM.get_current_state();
	}

    public void game_over_lost()
    {
        systemSM.handle_transition("GameOverLost");
        SceneLoader.get_instance().load_appropriate_scene();
    }

    public void game_over_won()
    {
        systemSM.handle_transition("GameOverWon");
        SceneLoader.get_instance().load_appropriate_scene();
    }

    public void ok_button()
    {
        systemSM.handle_transition("OkButton");
        SceneLoader.get_instance().load_appropriate_scene();
    }

    public void credits()
    {
        systemSM.handle_transition("CreditsPressed");
        SceneLoader.get_instance().load_appropriate_scene();
    }

    public void replay()
    {
        systemSM.handle_transition("ReplyPressed");
        SceneLoader.get_instance().load_appropriate_scene();
    }
}
