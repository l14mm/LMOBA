using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        private Transform target;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            //agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;

            StartCoroutine(FindTarget());

        }

        private IEnumerator FindTarget()
        {
            yield return new WaitForSeconds(0.1f);

            target = GameObject.Find("LOCAL Player").transform;

            if (target != null)
                StartCoroutine(FindTarget());
        }

        private void Update()
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
                transform.LookAt(target.position);

            }


            //if (agent.remainingDistance > agent.stoppingDistance)
                //character.Move(agent.desiredVelocity, false, false);
            //else
                //character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}