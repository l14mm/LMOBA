using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MinionMovement : NetworkBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }

    public Transform target;
    public MinionState state;
    public Vector3 previousPosition;

    // Wander stuff
    private float wanderRadius = 50;
    private float wanderTimer = 5;

    private Transform wanderTarget;
    private float timer;

    private float viewRadius = 20;
    public float attackRange;

    public int team = 1;

    public enum MinionState
    {
        wander,
        attack
    }

    // Use this for initialization
    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        
        // Old find target, using field of view list now
        //StartCoroutine(FindTarget());
        state = MinionState.wander;

        timer = wanderTimer;
    }

    private void Update()
    {
        // Avoid spells
        Transform spellToAvoid = null;
        bool moveRight = true;
        float closestDistance = 11;
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
                // Check if it is negative
                Vector3 cross = Vector3.Cross(spellDirection, directionToUs);
                if (cross.y < 0) angle = -angle;

                // If angle is positive, the spell is on the right of us, so we should move to the left
                if (angle < 20 && angle > -20)
                {
                    if (angle > 0) moveRight = false;
                    else moveRight = true;

                    // Calculate distance to spell, we only want to avoid the closest spell at the moment
                    float distance = Vector3.Distance(transform.position, temp.transform.position);
                    if (distance < closestDistance)
                    {
                        spellToAvoid = temp.transform;
                        closestDistance = distance;
                    }
                }
            }
        }

        if (state == MinionState.wander)
        {
            //transform.Translate(Vector3.forward * Time.deltaTime);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                //Debug.Log("wanderpos: " + newPos + ", stop dist: " + agent.stoppingDistance);
                agent.SetDestination(newPos);
                GetComponent<PlayerMovement>().rotationTarget = newPos;
                //Debug.Log("set destination wander");
                timer = 0;
            }

            // Get a target
            if (GetComponent<FieldOfView>().visibleEnemies.Count > 0)
            {
                target = GetComponent<FieldOfView>().visibleEnemies[0];
                state = MinionState.attack;
            }

            //StartCoroutine(FindTarget());

        }
        else if (state == MinionState.attack)
        {
            if (target)
            {
                //Debug.Log("1");
                // Stop just outside of auto attack range
                agent.stoppingDistance = attackRange;
                //agent.SetDestination(target.position);
                //transform.LookAt(target.position);
            }
            // We only want to shoot if we can see the player (the line of sight is clear)
            if (target)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < attackRange)
                {
                    AutoAttack(target);
                }
                else
                {
                    target = null;
                    state = MinionState.wander;

                }
            }
            else
                state = MinionState.wander;
        }
        
    }

    private void AutoAttack(Transform t)
    {

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        UnityEngine.AI.NavMeshHit navHit;

        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(0.1f);

        float closestDistance = 51;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject temp = hitColliders[i].gameObject;
            //if (temp.tag == "Player" && temp.GetComponent<NetworkedPlayerScript>().netId != netId && temp.GetComponent<NetworkedPlayerScript>().isAI)
            if (temp.tag == "Player" && temp.GetComponent<NetworkedPlayerScript>().netId != netId && temp.GetComponent<NetworkedPlayerScript>().team != team)
            {
                float distance = Vector3.Distance(transform.position, temp.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = temp.transform;

                    RaycastHit hit;
                    Vector3 rayDirection = target.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out hit))
                    {
                        if (hit.transform == target)
                        {
                            // We can see the target
                            //LookAndShoot();
                            state = MinionState.attack;
                        }
                        else
                        {
                            // there is something obstructing the view
                        }
                    }
                }
            }
        }


        if (target == null)
            StartCoroutine(FindTarget());
    }
}
