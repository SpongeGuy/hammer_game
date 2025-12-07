using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [SerializeField] string hammerTag = "Hammer";
    [SerializeField] string playerTag = "Player";
    public enum AIState { Idle, Chase }
    public AIState currentState;
    public NavMeshAgent agent;
    public Transform player;
    public float detectionRange = 10f;
    public float minTime = 7f;
    public float maxTime = 10f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    private float idleTimer = 0f;
    //Boss phases related to amount of health (phase1 = full(3hp), phase2(2hp), phase3(1hp))
    private bool phase1 = true;
    private bool phase2 = false;
    private bool phase3 = false;
    private bool startAttack = true;
    private bool canAttack = true;
    public bool isAttacking = false;
    private float distanceToPlayer = 0f;
    public bool isDashing = false;
    

    void Start()
    {
        Attack();
        currentState = AIState.Idle;
        agent = GetComponent<NavMeshAgent>();
    }

    void Attack()
    {
        float randomTime = Random.Range(minTime, maxTime);
        Invoke("Attack", randomTime);
        if (canAttack)
        {
            canAttack = false;
            isAttacking = true;
            StartCoroutine(DashAttack());
        }
    }

    IEnumerator DashAttack()
    {
        isDashing = true;

        transform.eulerAngles = new Vector3(270, 0, 0);
        transform.position = new Vector3(transform.position.x, 1.62f, transform.position.z);

        Vector3 dashDirection = (player.position - transform.position).normalized;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null; 
        }

        transform.position = new Vector3(transform.position.x, 0.243f, transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, 0);

        currentState = AIState.Idle;
        isDashing = false;
        isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
        }
        else if (other.gameObject.tag.Equals(hammerTag))
        {
            if (phase1)
            {
                phase1 = false;
                phase2 = true;
                minTime = minTime/1.5f;
                maxTime = maxTime/1.5f;
            }
            else if (phase2)
            {
                phase2 = false;
                phase3 = true;
                minTime = minTime/2f;
                maxTime = maxTime/2f;
            }
            else if (phase3)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (isAttacking)
        {
            
        }
        else
        {
            if (distanceToPlayer <= detectionRange)
            {
                currentState = AIState.Chase;
            }

            switch (currentState)
            {
                case AIState.Idle:
                    Idle();
                    break;
                case AIState.Chase:
                    Chase();
                    break;
            }
        }
    }

    void Idle()
    {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;

        if (idleTimer >= 3f)
        {
            currentState = AIState.Chase;
            canAttack  = true;
            idleTimer = 0f;
        }
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
}
