using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class CS_AIPathFinder : MonoBehaviour {

	NavMeshAgent pathFinder;
	public GameObject turretHead;
	float moveSpeed = 5;
	[Tooltip("Make slower by lowering number")]
	public float multiplyer = 1;

	// Use this for initialization
	void Start () {
		pathFinder = this.GetComponent<NavMeshAgent>();
		target = GameObject.FindWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate (){
		TargetPos();
	}

	Transform target;
	void TargetPos (){
		if (pathFinder.speed != (moveSpeed * multiplyer)){
			pathFinder.speed = moveSpeed * multiplyer;
		}
		pathFinder.destination = target.position;
	}
		
}
