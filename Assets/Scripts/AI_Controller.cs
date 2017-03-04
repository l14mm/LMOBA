using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootingScript))]
public class AI_Controller : NetworkBehaviour {

    private Transform target;
    private ShootingScript shootingScript;
    public State state;

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter character { get; private set; }

    public enum State
    {
        wander,
        attack,
        avoid
    }

    // Use this for initialization
    private void Start ()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        shootingScript = GetComponent<ShootingScript>();
        StartCoroutine(FindTarget());
        state = State.attack;
        
        InvokeRepeating("LookAndShoot", 1, 1);
    }

    private void Update()
    {
        // Avoid spells
        Transform spellToAvoid = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject temp = hitColliders[i].gameObject;
            if (temp.tag == "DangerSpell" && temp.GetComponent<FireballScript>().creator.netId.Value != GetComponent<NetworkedPlayerScript>().netId.Value)
            {
                // Check if spell will hit us
                Vector3 spellDirection = temp.transform.forward.normalized;
                Vector3 directionToUs = (transform.position - temp.transform.position).normalized;
                // Check angle between two vectors/direcitons
                float angle = Vector3.Angle(spellDirection, directionToUs);
                if(angle <  20)
                {
                    //Debug.Log("Incoming spell");
                    spellToAvoid = temp.transform;
                }

            }
        }

        if (spellToAvoid)
        {
            state = State.avoid;
        }

        if (state == State.wander)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
        }
        else if (state == State.attack && target != null)
        {
            agent.SetDestination(target.position);
            transform.LookAt(target.position);
        }
        else if(state == State.avoid)
        {
            if (spellToAvoid)
            {
                Vector3 spellDirection = spellToAvoid.forward;
                Vector3 avoidDirection = spellToAvoid.right.normalized * 25;
                agent.SetDestination(transform.position + avoidDirection);
                transform.LookAt(target.position);
            }
            else
                state = State.attack;
        }
    }

    private IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(0.1f);

        //target = GameObject.Find("LOCAL Player").transform;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject temp = hitColliders[i].gameObject;
            if (temp.tag == "Player" && temp.GetComponent<NetworkedPlayerScript>().netId.Value != netId.Value && temp.GetComponent<NetworkedPlayerScript>().isAI)
            {
                //Debug.Log("my id: " + netId.Value);
                //Debug.Log("their id: " + temp.GetComponent<NetworkedPlayerScript>().netId.Value);
                target = temp.transform;
            }
        }


        if (target == null)
            StartCoroutine(FindTarget());
    }

    private void LookAndShoot()
    {
        if (target)
        {
            shootingScript.CreateFire();
            shootingScript.CreateFireBall(target.transform);
        }
    }
}
