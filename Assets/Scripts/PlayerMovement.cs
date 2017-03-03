using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PlayerMovement : MonoBehaviour {

    private CharacterController controller;
    public float speed = 1;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    private Vector3 waypoint;

    private void Start ()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        waypoint = transform.position;
    }
	
	void Update ()
    {
        if(waypoint != null && waypoint != transform.position)
        {
            agent.SetDestination(waypoint);
            //transform.LookAt(waypoint);
        }
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //Debug.DrawLine(hit.point, Input.mousePosition, Color.red);
                waypoint = hit.point;
            }
            else
            {
                //Debug.Log("didnt hit");
            }
        }
        /*
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        //controller.SimpleMove(new Vector3(horizontal, 0, vertical));
        transform.Translate(new Vector3(horizontal, 0, vertical));
        */

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(waypoint, 0.5f);
    }
}
