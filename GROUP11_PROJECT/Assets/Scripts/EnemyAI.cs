using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Idle, Walking, Chasing }
    public AIState currentState;

    public NavMeshAgent ai;
    public List<Transform> destinations;
    public float idleTime, walkSpeed, chaseSpeed, detectDistance, caughtDist;
    public float minChasetime, maxChasetime;
    public Transform player;
    public int destinationAmount;
    public Vector3 rayCastOffset;
    public LayerMask raycastLayerMask;

    private Transform currDestination;
    private Vector3 dest;

    private void Awake()
    {
        currentState = AIState.Walking;
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
    }

    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        // Player detection only if not already chasing
        if (currentState != AIState.Chasing)
        {
            if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, detectDistance, raycastLayerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    StopAllCoroutines();
                    StartCoroutine(Chase());
                    currentState = AIState.Chasing;
                }
            }
        }

        switch (currentState)
        {
            case AIState.Chasing:
                ai.destination = player.position;
                ai.speed = chaseSpeed;

                float distance = Vector3.Distance(player.position, ai.transform.position);
                if (distance <= caughtDist)
                {
                    player.gameObject.SetActive(false);
                    StopAllCoroutines();
                    StartCoroutine(Dead());
                    currentState = AIState.Idle;
                }
                break;

            case AIState.Walking:
                ai.destination = currDestination.position;
                ai.speed = walkSpeed;

                if (ai.remainingDistance <= ai.stoppingDistance)
                {
                    ai.speed = 0;
                    StopAllCoroutines();
                    StartCoroutine(Idle());
                    currentState = AIState.Idle;
                }
                break;

            case AIState.Idle:
                // Do nothing while idle — the coroutine will transition to Walking
                break;
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);

        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        currentState = AIState.Walking;
    }

    IEnumerator Chase()
    {
        float chaseTime = Random.Range(minChasetime, maxChasetime);
        yield return new WaitForSeconds(chaseTime);

        // Only go back to patrol if player wasn't caught
        if (currentState == AIState.Chasing)
        {
            int random = Random.Range(0, destinationAmount);
            currDestination = destinations[random];
            currentState = AIState.Walking;
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }

    public void StopChase()
    {
        StopAllCoroutines();
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        currentState = AIState.Walking;
    }

    public IEnumerator Stun()
    {
        ai.isStopped = true;
        currentState = AIState.Idle; //my monster is stopped

        yield return new WaitForSeconds(3f);

        ai.isStopped = false;
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        currentState = AIState.Walking;
    }
}
