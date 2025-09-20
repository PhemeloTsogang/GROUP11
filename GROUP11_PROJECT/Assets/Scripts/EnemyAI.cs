using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    //Title: How to Program in Unity: State Machines Explained
    //Author: iHeartGameDev (Youtube)
    //16 August 2025
    //Availability: https://www.youtube.com/watch?v=Vt8aZDPzRjI
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


    //Attack settings
    public Attack cameraShake;
    public float attackCooldown = 2f;
    private bool canAttack;
    public PlayerHealth health;
    public Hide hide;

    [Header("Audio Settings")]
    private AudioSource MonsterMove;
    private AudioSource MonsterRoar;
    private AudioSource MonsterAttack;

    private void Start()
    {
        currentState = AIState.Walking;
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        canAttack = true;
    }

    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (health.health <= 0)
        {
            player.gameObject.SetActive(false);
            StopAllCoroutines();
            Dead();
            currentState = AIState.Idle;
            return;
        }

        if (currentState != AIState.Chasing)
        {
            if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, detectDistance, raycastLayerMask))
            {
                if (hit.collider.CompareTag("Player") && !hide.isHiding)
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
                if (MonsterMove == null || !MonsterMove.isPlaying)
                {
                    MonsterMove = AudioManager.instance.Play("MonsterMove", this.transform);
                }

                ai.destination = player.position;
                ai.speed = chaseSpeed;

                float distance = Vector3.Distance(player.position, ai.transform.position);
                if (distance <= caughtDist && canAttack && !hide.isHiding )
                {
                    if (health.health <= 0)
                    {
                        return;
                    }
                    StartCoroutine(EnemyAttack());
                }
                break;

            case AIState.Walking:
                if (MonsterMove == null || !MonsterMove.isPlaying)
                {
                    MonsterMove = AudioManager.instance.Play("MonsterMove", this.transform);
                }

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
                AudioManager.instance.StopSound(MonsterMove);

                if (MonsterRoar == null || !MonsterRoar.isPlaying)
                {
                    MonsterRoar = AudioManager.instance.Play("Roar", this.transform);
                }

                MonsterMove = null;
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

    private void Dead()
    {
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
        //Stun
        AudioManager.instance.StopSound(MonsterMove);
        AudioManager.instance.Play("MonsterHurt", this.transform);
        MonsterMove = null;

        ai.isStopped = true;
        currentState = AIState.Idle; //my monster is stopped

        yield return new WaitForSeconds(3f);

        ai.isStopped = false;
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        currentState = AIState.Walking;

    }

    private IEnumerator EnemyAttack()
    {
        if (MonsterAttack == null || !MonsterAttack.isPlaying)
        {
            MonsterAttack = AudioManager.instance.Play("Attack", this.transform);
        }

        canAttack = false;
        ai.isStopped = true;

        health.health--;
        health.ChangeColor();
        yield return StartCoroutine(cameraShake.Shake(0.15f, 0.4f));


        yield return new WaitForSeconds(attackCooldown);

        ai.isStopped = false;

        canAttack = true;

    }
}
