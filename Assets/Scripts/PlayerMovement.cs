using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PlayerMovement : NetworkBehaviour {

    private CharacterController controller;
    public float speed = 1;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    [HideInInspector]
    public Vector3 waypoint;

    private Vector3 lastPosition;
    public bool isMoving = false;

    private void Start ()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        waypoint = transform.position;
    }
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        if(transform.position == lastPosition)
        {
            if (isMoving && GetComponent<NetworkedPlayerScript>().anim)
            {
                isMoving = false;
                GetComponent<NetworkedPlayerScript>().anim.SetTrigger("JogToIdle");
            }
        }
        else
        {
            if (!isMoving && GetComponent<NetworkedPlayerScript>().anim)
            {
                isMoving = true;
                GetComponent<NetworkedPlayerScript>().anim.SetTrigger("IdleToJog");
            }
        }
        if (GetComponent<ShootingScript>().isCastingSpell)
        {
            //agent.Stop();
            //agent.speed = 0;
        }

        if(GetComponent<ShootingScript>().target)
        {
            if(Vector3.Distance(transform.position, GetComponent<ShootingScript>().target.position) >= GetComponent<ShootingScript>().autoRange)
            {
                //Debug.Log("too far away to auto, moving closer");
                agent.SetDestination(GetComponent<ShootingScript>().target.position);
            }
        }
        else if (GetComponent<ShootingScript>().isCasting || GetComponent<ShootingScript>().isCastingSpell)
        {
            agent.SetDestination(transform.position);
            agent.updateRotation = false;
            
            // Lerp rotation to face target
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GetComponent<ShootingScript>().autoTarget - transform.position), Time.deltaTime * 10);
        }
        else if(waypoint != null && waypoint != transform.position)
        {
            if(!agent.updateRotation)
                agent.updateRotation = true;

            agent.SetDestination(waypoint);
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if(hit.transform.tag == "Floor")
                {   
                    waypoint = hit.point;
                    // If we click somewhere else on the map, we want to remove our target
                    if(GetComponent<ShootingScript>().target)
                    {
                        GetComponent<ShootingScript>().target = null;
                    }
                }
            }
        }

        lastPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(waypoint, 0.5f);
        if(GetComponent<NetworkedPlayerScript>().myCamera)
            Gizmos.DrawLine(GetComponent<NetworkedPlayerScript>().myCamera.transform.position, waypoint);
    }
}
