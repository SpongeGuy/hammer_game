using UnityEngine;

using UnityEngine.AI;

using System.Collections;

 

public class EnemyAI : MonoBehaviour

{

    public enum AIState { Idle, Patrol, Chase }

    public AIState currentState;

 

    public NavMeshAgent agent;

    public Transform[] patrolPoints;

    public Transform player;

    public float detectionRange = 10f;

 

    private int patrolIndex = 0;

    private float idleTimer = 0f;

 

    void Start()

    {

        currentState = AIState.Idle;

        agent = GetComponent<NavMeshAgent>();

    }

 

    void Update()

    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

 

        if (distanceToPlayer <= detectionRange)

        {

            currentState = AIState.Chase;

        }

        else if (currentState == AIState.Chase && distanceToPlayer > detectionRange)

        {

            currentState = AIState.Patrol;

        }

 

        switch (currentState)

        {

            case AIState.Idle:

                Idle();

                break;

            case AIState.Patrol:

                Patrol();

                break;

            case AIState.Chase:

                Chase();

                break;

        }

    }

 

    void Idle()

    {

        agent.isStopped = true;

        idleTimer += Time.deltaTime;

 

        if (idleTimer >= 3f)

        {

            currentState = AIState.Patrol;

            idleTimer = 0f;

        }

    }

 

    void Patrol()

    {

        agent.isStopped = false;

 

        if (!agent.pathPending && agent.remainingDistance < 0.5f)

        {

            currentState = AIState.Idle;

            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

            agent.SetDestination(patrolPoints[patrolIndex].position);

        }

 

        if (!agent.hasPath)

        {

            agent.SetDestination(patrolPoints[patrolIndex].position);

        }

    }

 

    void Chase()

    {

        agent.isStopped = false;

        agent.SetDestination(player.position);

    }

}