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
    public GameObject go_Turret;
    public Transform[] go_RocketPods;

    [Header("Shell Prefabs")] [Space(10)]
    public GameObject go_Shell;
    public GameObject go_Rocket;

    [Header("Audio")][Space(10)]
    public AudioSource as_MainGunAudio;
    public AudioClip ac_StandardShot;

    //MISC:
//    public AnimationClip v_StandardShot;
//    public Animator v_TankAnimation;
    public Animation v_TankAnimation;

    // END - Variables.

    void Start() {
        as_MainGunAudio = go_MainGun.GetComponent<AudioSource>();
//        v_TankAnimation = GetComponent<Animator>();
        v_TankAnimation = GetComponent<Animation>();
    } // END - Start.

    public void FireMain_Basic() {
        
        as_MainGunAudio.clip = ac_StandardShot;
        as_MainGunAudio.Play();
        v_TankAnimation.Play("VehicleBone|Gun_StandardShot");
        //v_TankAnimation.SetBool("p_StandardShot", true);
        Instantiate(go_Shell, go_MainGun.transform.position, go_MainGun.transform.rotation);
    } // END - fire main.

    public void FireMain_Charged(float p_Charge) {

    } // END - Fire main (charged).

    public void FireRockets(Transform p_Target) {

    } // END - Fire Rockets.

} // END - Monobehaviour.