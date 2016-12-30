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

    [Header("Shell Prefabs:")] [Space(10)]
    public GameObject go_Shell;
    public GameObject go_Rocket;

    [Header("Cooldowns:")][Space(10)]
    public float v_RocketCooldown;

    [Header("Audio:")][Space(10)]
    public AudioSource as_MainGunAudio;
    public AudioClip ac_StandardShot;

    //MISC:
    public Animator v_TankAnimation;
    // END - Variables.

    void Start() {
        as_MainGunAudio = go_MainGun.GetComponent<AudioSource>();
        v_TankAnimation = GetComponent<Animator>();
    } // END - Start.

    public void FireMain_Basic() {

        AnimatorStateInfo v_CurrentStateInfo = v_TankAnimation.GetCurrentAnimatorStateInfo(0);
        //        if(v_CanShoot == true)
        //        if (v_TankAnimation.GetBool("P_Shoot") == false) {
        if (v_CurrentStateInfo.IsName("DefaultLayer.VehicleBone|TankIdle")){
            v_TankAnimation.SetTrigger("P_Shoot");
            as_MainGunAudio.clip = ac_StandardShot;
            as_MainGunAudio.Play();
            Instantiate(go_Shell, go_MainGun.transform.position, go_MainGun.transform.rotation);
        } // END - If current state(animation) is IDLE, allow shot.

    } // END - fire main.

    public void FireMain_Charged(float p_Charge) {

    } // END - Fire main (charged).

    public void FireRockets(Transform p_Target) {
        if(v_RocketCooldown > 0) {

        } // END - Rocket cooldown.

    } // END - Fire Rockets.

} // END - Monobehaviour.