/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | CS_AIDriver
OUTBOUND REFERENCES: null.
OVERVIEW:  The hub that spawns the wheeled tank weapon shells independant of input method.
*/

using UnityEngine;
using System.Collections;

public class CS_WheeledTankWeapons_00 : MonoBehaviour {

    //VARIABLES:
    [Header("SpawnPoints:")]
    public GameObject go_MainGun;
    public Transform[] go_RocketPods;

    [Space(10)]
    [Header("Shell Prefabs:")] 
    public GameObject go_Discus;
    public GameObject go_Trident;

    [Space(10)]
    [Header("MISSILE - JAVELIN:")]
    public GameObject go_Missile_Javelin;
    public int Javelin_SpawnCountPerPod;
    public float Javelin_TimeBetweenLaunches;
    public bool Javelin_Sequential;

    [Space(10)]
    [Header("MISSILE - TITAN:")]
    public GameObject go_Missile_Titan;
    public int Titan_SpawnCountPerPod;
    public float Titan_TimeBetweenLaunches;
    public bool Titan_Sequential;

    [Space(10)]
    [Header("MISSILE - HELLSEEKER:")]
    public GameObject go_Missile_Hellseeker;
    public int Hellseeker_SpawnCountPerPod;
    public float Hellseeker_TimeBetweenLaunches;
    public bool Hellseeker_Sequential;

    [Space(10)]
    [Header("Other:")]
    public int v_CurrentMissile;
    public float v_RocketCooldown;
    public Transform go_Target;
    public float v_ScannerRange;

    [Header("Audio:")]
    public AudioSource as_MainGunAudio;
    public AudioClip ac_StandardShot;

    //MISC:
    public GameObject go_CurrentMain;
    public GameObject go_CurrentMissile;
    public int v_SpawnCountPerPod;
    public float v_TimeBetweenLaunches;
    public bool v_Sequential;
    public Animator v_TankAnimation;
    public bool v_CurrentlyTargeting;
    public Camera v_CurrentModeCamera;
    // END - Variables.

    void Start() {
        as_MainGunAudio = go_MainGun.GetComponent<AudioSource>();
        v_TankAnimation = GetComponent<Animator>();
        v_CurrentMissile = 1;
        go_CurrentMissile = go_Missile_Javelin;
        v_SpawnCountPerPod = Javelin_SpawnCountPerPod;
        v_TimeBetweenLaunches = Javelin_TimeBetweenLaunches;
        v_Sequential = Javelin_Sequential;
        go_CurrentMain = go_Discus;
    } // END - Start.

    public void FireMain_Basic() {

        AnimatorStateInfo v_CurrentStateInfo = v_TankAnimation.GetCurrentAnimatorStateInfo(0);
        //        if(v_CanShoot == true)
        //        if (v_TankAnimation.GetBool("P_Shoot") == false) {
        if (v_CurrentStateInfo.IsName("DefaultLayer.VehicleBone|TankIdle")){
            v_TankAnimation.SetTrigger("P_Shoot");
            as_MainGunAudio.clip = ac_StandardShot;
            as_MainGunAudio.Play();
            Instantiate(go_CurrentMain, go_MainGun.transform.position, go_MainGun.transform.rotation);
        } // END - If current state(animation) is IDLE, allow shot.

    } // END - fire main.

    public void ChangeMain() {
        if(go_CurrentMain == go_Discus) { go_CurrentMain = go_Trident; }
        else { go_CurrentMain = go_Discus; }
    } // END - Change Main;

    public void ChangeRocket(int p_Increment) {
        v_CurrentMissile += p_Increment;
        // Loop Figure:
        if (v_CurrentMissile == 3) { v_CurrentMissile = 0; }
        else if(v_CurrentMissile == -1) { v_CurrentMissile = 2; }

        switch (v_CurrentMissile) {

            // JAVELIN:
            case 0:
                go_CurrentMissile = go_Missile_Javelin;
                v_SpawnCountPerPod = Javelin_SpawnCountPerPod;
                v_TimeBetweenLaunches = Javelin_TimeBetweenLaunches;
                v_Sequential = Javelin_Sequential;
                break;

            // TITAN:
            case 1:
                go_CurrentMissile = go_Missile_Titan;
                v_SpawnCountPerPod = Titan_SpawnCountPerPod;
                v_TimeBetweenLaunches = Titan_TimeBetweenLaunches;
                v_Sequential = Titan_Sequential;
                break;

            // HELLSEEKER:
            case 2:
                go_CurrentMissile = go_Missile_Hellseeker;
                v_SpawnCountPerPod = Hellseeker_SpawnCountPerPod;
                v_TimeBetweenLaunches = Hellseeker_TimeBetweenLaunches;
                v_Sequential = Hellseeker_Sequential;
                break;
        } // END - Switch: Current Missile.
    } // END - Change Rocket.


    public void InitialRocket() {
        if (!v_CurrentlyTargeting) {
            v_CurrentlyTargeting = true;
            Collider[] go_ObjectsWithinRange = Physics.OverlapSphere(gameObject.transform.position, v_ScannerRange, 1 << 8);
            Debug.Log(go_ObjectsWithinRange[0]);

            go_Target = go_ObjectsWithinRange[0].transform;

            StartCoroutine(FireRockets());

        } // END - If not currently targeting:
    } // END - Initial Rocket.


    public IEnumerator FireRockets() {
        if(v_RocketCooldown > 0) {
            Debug.Log("Fire rockets!");
            for (int rocketpodIndex = 0; rocketpodIndex < go_RocketPods.Length; rocketpodIndex++) {
                for (int rocketLaunchIndex = 0; rocketLaunchIndex < v_SpawnCountPerPod;) {
                    GameObject v_RocketClone = (GameObject)Instantiate(go_CurrentMissile, go_RocketPods[rocketpodIndex].transform.position, go_RocketPods[rocketpodIndex].transform.rotation);
                    v_RocketClone.GetComponent<CS_Rocket_01>().v_Target = go_Target;
                    //if (v_Sequential) { rocketLaunchIndex++; yield return new WaitForSeconds(v_TimeBetweenLaunches); } else { break; }
                } // END - RocketLaunch Index.
                yield return new WaitForSeconds(v_TimeBetweenLaunches);
                
            } // END - For loop: RocketPods.

        } // END - Rocket cooldown.
    } // END - Fire Rockets.

} // END - Monobehaviour.