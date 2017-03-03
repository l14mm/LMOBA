using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootingScript))]
public class AI_Controller : MonoBehaviour {

    private Transform target;
    private ShootingScript shootingScript;
    public State state;

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter character { get; private set; } // the character we are controlling

    public enum State
    {
        wander,
        attack
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
        if(state == State.wander)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
        }
        else if (state == State.attack && target != null)
        {
            agent.SetDestination(target.position);
            transform.LookAt(target.position);
        }

    }

    private IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(0.1f);

        target = GameObject.Find("LOCAL Player").transform;

        if (target != null)
            StartCoroutine(FindTarget());
    }

    private void LookAndShoot()
    {
        if (target)
        {
            //shootingScript.Fireball();
        }
    }
}
