using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public float idleTime, walkSpeed, chaseSpeed, detectDistance, caughtDist;
    public float chasetime, minChasetime, maxChasetime; 
    public bool isWalking, isChasing, isIdle;
    public Transform player;
    private Transform currDestination;
    private Vector3 dest;
    private int random, random2;
    public int destinationAmount;
    public Vector3 rayCastOffset;
    public float aiDistance;

    private void Awake()
    {
        isWalking = true;
        isIdle = false;
        random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
    }

    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);
        
        if (Physics.Raycast(transform.position + rayCastOffset,direction, out hit, detectDistance))
        {
            if(hit.collider.CompareTag("Player"))
            {
                isWalking = false;
                StopCoroutine(Idle());
                StartCoroutine(Chase());
                isChasing = true;
            }
        }

        if (isChasing)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;

            if (aiDistance <= caughtDist)
            {
                player.gameObject.SetActive(false);
                StartCoroutine(Dead());
                isChasing = false;
            }
        }

        if(isWalking)
        {
            dest = currDestination.position;
            ai.destination = dest;
            ai.speed = walkSpeed;

            if (ai.remainingDistance <= ai.stoppingDistance & !isIdle)
            {
                random2 = Random.Range(0, 2);   
                if (random2 == 0 )
                {
                    random = Random.Range(0, destinationAmount);
                    currDestination = destinations[random];
                }
                else
                {
                    ai.speed = 0;
                    isWalking = false;
                    isIdle = true;
                    StartCoroutine(Idle());
                }
            }
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);
        isWalking = true;
        isIdle = false;
        random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
    }

    IEnumerator Chase()
    {
        chasetime = Random.Range(minChasetime, maxChasetime);
        yield return new WaitForSeconds(chasetime);
        isWalking = true;
        isChasing = false;
        random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }

    public IEnumerator stopChase()
    {
        yield return new WaitForSeconds(3);
        isWalking = true;
        isChasing = false;
        StopCoroutine(Chase());
        random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
    }
}
