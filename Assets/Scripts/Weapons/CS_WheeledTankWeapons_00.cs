/*
AUTHOR(S): LEE WILLIAMS		DATE: 10/2016 - 01/2017
EDITOR(S): SCOTT ANDERS
SCRIPT HOLDERS: PF_PlayerVehicle
INBOUND REFERENCES: CS_PlayerDriver | CS_AIDriver | CS_BasicTrackTo
OUTBOUND REFERENCES: null.
OVERVIEW:  The hub that spawns the wheeled tank weapon shells independant of input method.  
ATTENTION: This script does (and should) *NOT* contact other scripts.
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
    [Header("MISSILE - Other Settings:")]
    public int v_CurrentMissile;
    public float v_RocketCooldown;
    public float v_CurrentCooldown;
    public Transform go_Target;
    public float v_ScannerRange;
    public bool v_TargetLocked = false;
    bool v_RocketCanFire;
    [Tooltip("How many seconds per meter: used when calculating targeting time. /nLower values equate to less time per second per meter, thus faster locking time.")]public float v_SecondsPerMeter;
    public float v_TargetingTime;
    public float v_Distance;

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
    public bool v_ReleasingSalvo; // Bool to prevent mid-switching.
    public Camera v_CurrentModeCamera;
    // END - Variables.

    void Start() {
        as_MainGunAudio = go_MainGun.GetComponent<AudioSource>();
        v_TankAnimation = GetComponent<Animator>();
        v_CurrentMissile = 0;
        go_CurrentMissile = go_Missile_Javelin;
        v_SpawnCountPerPod = Javelin_SpawnCountPerPod;
        v_TimeBetweenLaunches = Javelin_TimeBetweenLaunches;
        v_Sequential = Javelin_Sequential;
        go_CurrentMain = go_Discus;
    } // END - Start.


    private void Update(){
        if(v_CurrentCooldown > 0) {
            v_CurrentCooldown -= 1 * Time.deltaTime;
        } // END - If current cooldown is greater than 0.

        if(v_TargetingTime > 0) {
            v_TargetingTime -= 1 * Time.deltaTime;
        } else if(v_TargetingTime <= 0 && v_RocketCanFire) {
           v_RocketCanFire = false;
            v_TargetLocked = true;
        } // END - elseIf Targetimg time is 0.

    } // END - Update.


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

    public void ChangeRocket() {
        v_CurrentMissile++;
        Debug.Log("Change missile!" + v_CurrentMissile);
        // Loop Figure:
        if (v_CurrentMissile >= 3) { v_CurrentMissile = 0; }

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

    public void LockTarget() {
        // IF TargetLocked is false, obtain target:
        if (v_TargetLocked == false) {
            v_RocketCanFire = true;
            ObtainClosestTarget();
            // With target obtained, start locking time (allows the player to change the target).
            // Locking time is calculated using distance & Seconds per meter.
            v_Distance = Vector3.Distance(gameObject.transform.position, go_Target.position);
//            v_InitialTargetTime = v_Distance * v_SecondsPerMeter;
            v_TargetingTime = v_Distance * v_SecondsPerMeter;
            //v_RocketCanFire = true;
        } // END - If target locked is false

    } // END - Rockets.

    // Obtains the VISIBLE gameobject with layer "EnemyEntity" closest to camera center.
    public void ObtainClosestTarget() {
            Collider[] go_ObjectsWithinRange = Physics.OverlapSphere(gameObject.transform.position, v_ScannerRange, 1 << 8); // Return all colliders on the ENEMY layer.
            Transform go_ClosestTarget = null;
        if (go_ObjectsWithinRange != null){ // Prevent code running if no objects within range.
            // Start for loop: Check against all objects within range.
            for (int currentObjectIndex = 0; currentObjectIndex < go_ObjectsWithinRange.Length; currentObjectIndex++){
                // Get the current object and set its camera space position to a variable:
                Vector3 ObjectInCameraSpaceCentered = v_CurrentModeCamera.WorldToViewportPoint(go_ObjectsWithinRange[currentObjectIndex].transform.position) - new Vector3(0.5f, 0.5f, 0);
                ObjectInCameraSpaceCentered.z = 0;
                print(ObjectInCameraSpaceCentered);
                float ObjectMagnitude = ObjectInCameraSpaceCentered.magnitude;

                // If current object in array is within the center bounds:  apply as current target and exit loop.
                //                if ((ObjectInCameraSpace.x > 0.45f && ObjectInCameraSpace.x < 0.55f) && (ObjectInCameraSpace.y > 0.45f && ObjectInCameraSpace.y < 0.55f)) {
                if (ObjectMagnitude < 0.05 && ObjectInCameraSpaceCentered.z > 0){
                    go_Target = go_ObjectsWithinRange[currentObjectIndex].transform;
                    goto Exitloop;
                } // END - If within center.
                  // If the closest target variable is null (first loop), set to current object.
                if (go_ClosestTarget == null) { go_ClosestTarget = go_ObjectsWithinRange[currentObjectIndex].transform; }

                // Run Compare function: Returns true if compared (current object) is closer to center:
                if (CompareFromCenter(go_ClosestTarget, ObjectInCameraSpaceCentered)) { go_ClosestTarget = go_ObjectsWithinRange[currentObjectIndex].transform; }
            } // END - For loop.

            // FINAL CHECK: If target magnitude is WITHIN 1 (screen space):
            go_Target = go_ClosestTarget;
            if(go_Target != null) { 
            Vector3 v_TargetOnScreen = v_CurrentModeCamera.WorldToViewportPoint(go_Target.position) - new Vector3(0.5f, 0.5f, 0);
            v_TargetOnScreen.z = 0;
            float v_TargetMagnitude = v_TargetOnScreen.magnitude;
            if (v_TargetMagnitude > 0.9f || (v_CurrentModeCamera.WorldToScreenPoint(go_Target.position).z) < 0) { print("Returning..."); return; }
            }
            Exitloop:; // EXIT from nested loop:

        } // END - Overlaying IF loop.
    } // END - Initial Rocket.

    // RETURNS TRUE IF "COMPARE TO" IS CLOSER TO CENTER THAN CURRENT CLOSEST!
    bool CompareFromCenter(Transform p_CurrentClosest, Vector3 p_CompareTo) {

        Vector3 CurrentClosestCentered = v_CurrentModeCamera.WorldToViewportPoint(p_CurrentClosest.position) - new Vector3(0.5f, 0.5f);
        CurrentClosestCentered.z = 0; // Set Z component (fustrum) to be 0.
        float CurrentClosestMagnitude = CurrentClosestCentered.magnitude;

        // CompareTo is the result of WorldToViewportPoint.
        Vector3 CompareToCentered = p_CompareTo - new Vector3(0.5f, 0.5f);
        CompareToCentered.z = 0; // Set Z component (fustrum) to be 0.
        float CompareTo_Magnitude = CompareToCentered.magnitude;

        if (CompareTo_Magnitude < CurrentClosestMagnitude) { return true; }
        else { return false; }
    } // END - Compare.

    // Instantiates the rockets.
    public IEnumerator FireRockets() {
        //        yield return new WaitForSeconds(v_LockTime);
        v_TargetLocked = false;
        v_ReleasingSalvo = true;
        v_CurrentCooldown = v_RocketCooldown;

        // Sequential firing (pod 1 then 2, etc).
        if (v_Sequential){
            for (int rocketLaunchIndex = 0; rocketLaunchIndex < v_SpawnCountPerPod; rocketLaunchIndex++){
                for (int rocketpodIndex = 0; rocketpodIndex < go_RocketPods.Length; rocketpodIndex++){
                    GameObject v_RocketClone = (GameObject)Instantiate(go_CurrentMissile, go_RocketPods[rocketpodIndex].transform.position, go_RocketPods[rocketpodIndex].transform.rotation);
                    v_RocketClone.GetComponent<CS_Rocket_01>().v_Target = go_Target;
                    yield return new WaitForSeconds(v_TimeBetweenLaunches);
                } // END - For loop: RocketPods.
            } // END - RocketLaunch Index.
        } // END - SEQUENTIAL LAUNCH

        // Nonsequential firing (all pods fire at once).
        else {
            for (int rocketLaunchIndex = 0; rocketLaunchIndex < v_SpawnCountPerPod; rocketLaunchIndex++){
                foreach (Transform RocketPodIndex in go_RocketPods){
                    GameObject v_RocketClone = (GameObject)Instantiate(go_CurrentMissile, RocketPodIndex.transform.position, RocketPodIndex.transform.rotation);
                    v_RocketClone.GetComponent<CS_Rocket_01>().v_Target = go_Target;
                } // END - For each rocket pod.
                    yield return new WaitForSeconds(v_TimeBetweenLaunches);
            } // END - Rocket Launch count.
        } // END - NONSEQUENTIAL
        v_ReleasingSalvo = false;
        go_Target = null;
    } // END - Fire Rockets.

} // END - Monobehaviour.