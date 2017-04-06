using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShootingScript))]
public class AI_Controller : NetworkBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter character { get; private set; }

    public Transform target;
    private ShootingScript shootingScript;
    public State state;
    public Vector3 previousPosition;

    // Wander stuff
    private float wanderRadius = 50;
    private float wanderTimer = 5;

    private Transform wanderTarget;
    private float timer;

    private float viewRadius = 20;

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
                if (angle <  20 && angle > -20)
                {
                    if (angle > 0) moveRight = false;
                    else moveRight = true;

                    // Calculate distance to spell, we only want to avoid the closest spell at the moment
                    float distance = Vector3.Distance(transform.position, temp.transform.position);
                    if(distance < closestDistance)
                    {
                        spellToAvoid = temp.transform;
                        closestDistance = distance;
                    }
                }
            }
        }

        if (spellToAvoid && state != State.avoid)
        {
            //Debug.Log("current position: " + transform.position);
            previousPosition = transform.position;
            state = State.avoid;
        }

        if (state == State.wander)
        {
            //transform.Translate(Vector3.forward * Time.deltaTime);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                Debug.Log("wanderpos: " + newPos);
                agent.SetDestination(newPos);
                timer = 0;
            }

            StartCoroutine(FindTarget());

        }
        else if (state == State.attack)
        {
            if(target)
            {
                // Stop just outside of auto attack range
                agent.stoppingDistance = shootingScript.autoRange;
                //agent.SetDestination(target.position);
                //transform.LookAt(target.position);
            }
            if (Vector3.Distance(transform.position, previousPosition) > 1)
            {
                agent.stoppingDistance = 1;
                agent.SetDestination(previousPosition);
                transform.LookAt(previousPosition);
            }
            if(target)
            {
                if (Vector3.Distance(transform.position, target.transform.position) > 20)
                {
                    target = null;
                }
            }
            // We only want to shoot if we can see the player (the line of sight is clear)
            if (target)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < shootingScript.autoRange)
                {
                    shootingScript.AutoAttack(target);
                }
                else
                {
                    RaycastHit hit;
                    Vector3 rayDirection = target.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out hit))
                    {
                        if (hit.transform == target)
                        {
                            // enemy can see the player!
                            StartCoroutine(LookShootFireball());
                        }
                        else
                        {
                            // there is something obstructing the view
                            state = State.wander;
                        }
                    }
                }
            }
        }
        else if(state == State.avoid)
        {
            if (spellToAvoid)
            {
                Vector3 spellDirection = spellToAvoid.forward;
                Vector3 avoidDirection = spellToAvoid.right.normalized * 25;

                if (Vector3.Distance(spellToAvoid.position, transform.position) < 2)
                {
                    shootingScript.Blink(avoidDirection);
                }
                else
                {
                    
                if (moveRight) avoidDirection *= -1;
                agent.SetDestination(transform.position + avoidDirection);
                //transform.LookAt(target.position);
                transform.LookAt(transform.position + avoidDirection);
                }
            }
            else
                state = State.attack;
        }
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
            if (temp.tag == "Player" && temp.GetComponent<NetworkedPlayerScript>().netId != netId && temp.GetComponent<NetworkedPlayerScript>().team != GetComponent<NetworkedPlayerScript>().team)
            {
                float distance = Vector3.Distance(transform.position, temp.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    target = temp.transform ;

                    RaycastHit hit;
                    Vector3 rayDirection = target.position - transform.position;
                    if (Physics.Raycast(transform.position, rayDirection, out hit))
                    {
                        if (hit.transform == target)
                        {
                            // We can see the target
                            //LookAndShoot();
                            state = State.attack;
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

    private IEnumerator LookShootFireball()
    {
        if (target && !shootingScript.isCastingSpell && shootingScript.fireballTimeValue <= 0)
        {
            // Calculate where target will be
            Vector3 estimatedPosition = target.position + target.forward * Vector3.Distance(transform.position, target.position + target.forward) * 0.15f;
            //Debug.Log("is casting: " + shootingScript.isCasting);
            transform.LookAt(estimatedPosition);
            //Debug.Log("create fire");
            shootingScript.CreateFire();
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("create fireball");
            shootingScript.CreateFireBall(estimatedPosition);
        }
    }

    void OnDrawGizmos()
    {
        if(target)
        {
            Vector3 estimatedPosition = target.position + target.forward * Vector3.Distance(transform.position, target.position + target.forward) * 0.15f;
            Gizmos.DrawLine(target.position, estimatedPosition);
        }
    }
}
