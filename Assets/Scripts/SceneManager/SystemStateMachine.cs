using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemStateMachine : MonoBehaviour
{
    static SystemStateMachine instance = null;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static SystemStateMachine get_instance()
    {
        return instance;
    }

    private List<string> stateList;
    private List<string> transitionList;
    private Dictionary<string, string> stateTransitionTable;
    private string currentState = "";
    void Start()
    {
        set_initial_state("SplashScreen");
        set_state_transition("SplashScreen", "OkButton", "MenuScreen");
        set_state_transition("MenuScreen", "Play", "GameScreen");
        set_state_transition("MenuScreen", "CreditsPressed", "Credits");
        set_state_transition("Credits", "OkButton", "MenuScreen");
        set_state_transition("GameScreen", "GameOverLost", "YouLostScreen");
        set_state_transition("GameScreen", "GameOverWon", "YouWonScreen");
        set_state_transition("YouLostScreen", "ReplayPressed", "MenuScreen");
        set_state_transition("YouWonScreen", "ReplayPressed", "MenuScreen");
        SceneLoader.get_instance().load_appropriate_scene();
    }


    void Update()
    {

    }

    public void set_initial_state(string stateName)
    {
        currentState = stateName;
    }

    public string get_current_state()
    {
        return currentState;
    }

    public void set_state_transition(string ifCurrentNameIs, string receiveTransition, string newStateIs)
    {
        string key;

        if (!stateList.Contains(ifCurrentNameIs)) stateList.Add(ifCurrentNameIs);
        if (!stateList.Contains(newStateIs)) stateList.Add(newStateIs);
        if (!transitionList.Contains(receiveTransition)) transitionList.Add(receiveTransition);
        key = ifCurrentNameIs + "==>" + receiveTransition;
        if (!stateTransitionTable.ContainsKey(key))
        {
            stateTransitionTable[key] = newStateIs;
        }
    }

    public string handle_transition(string transition)
    {
        string key;
        key = currentState + "==>" + transition;
        if (stateTransitionTable.ContainsKey(key))
        {
            currentState = stateTransitionTable[key];
        }

        return currentState;
    }
}