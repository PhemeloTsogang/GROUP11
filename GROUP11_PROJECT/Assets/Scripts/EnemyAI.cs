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
    public Animator animator;
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

    [Header("Dialogue Settings")]
    public DialogueTrigger trigger;

    private Coroutine movementCoroutine; //this helps my movement coroutines work better

    private void Start()
    {
        if (animator != null)
        {
            animator.SetBool("IsCreatureWalkingAnim", true);
        }

        currentState = AIState.Walking;

        // clamp destinationAmount so Random.Range can't pick a bad index
        //it helps the random destinations not mess up the monsters behaviour
        if (destinations == null || destinations.Count == 0)
        {
            destinationAmount = 0;
        }
        else
        {
            destinationAmount = Mathf.Clamp(destinationAmount, 1, destinations.Count);
            int random = Random.Range(0, destinationAmount);
            currDestination = destinations[random];
        }

        canAttack = true;
    }

    private void Update()
    {
        if (player == null || ai == null || health == null || hide == null)
        {
            return; // if everything is disabled
        }
            

        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (health.health <= 0)
        {
            player.gameObject.SetActive(false);
            StopMovementCoroutine();
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
                    StopMovementCoroutine();
                    movementCoroutine = StartCoroutine(Chase());
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
                    animator.SetBool("IsCreatureWalkingAnim", true);
                }

                ai.destination = player.position;
                ai.speed = chaseSpeed;

                float distance = Vector3.Distance(player.position, ai.transform.position);
                if (distance <= caughtDist && canAttack && !hide.isHiding)
                {
                    if (health.health <= 0)
                    {
                        return;
                    }
                    // allow attack coroutine to run without being cancelled by movement coroutine stops
                    StartCoroutine(EnemyAttack());
                }
                break;

            case AIState.Walking:
                if (MonsterMove == null || !MonsterMove.isPlaying)
                {
                    MonsterMove = AudioManager.instance.Play("MonsterMove", this.transform);
                    animator.SetBool("IsCreatureWalkingAnim", true);
                }

                ai.destination = currDestination.position;
                ai.speed = walkSpeed;

                // only consider arrival if path is not pending (prevents false "already arrived" when path still computing)
                // this helps that other problem I had where the monster wouldn't know what to do when states switched quickly
                if (!ai.pathPending && ai.remainingDistance <= ai.stoppingDistance)
                {
                    ai.speed = 0;
                    StopMovementCoroutine();
                    movementCoroutine = StartCoroutine(Idle());
                    currentState = AIState.Idle;
                }
                break;

            case AIState.Idle:
                if (MonsterMove != null)
                {
                    AudioManager.instance.StopSound(MonsterMove);
                }
                    
                if (MonsterRoar == null || !MonsterRoar.isPlaying)
                {
                    MonsterRoar = AudioManager.instance.Play("Roar", this.transform);
                }

                animator.SetBool("IsCreatureWalkingAnim", false);
                MonsterMove = null;
                break;
        }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(idleTime);

        if (destinations != null && destinations.Count > 0)
        {
            int random = Random.Range(0, destinationAmount);
            currDestination = destinations[random];
        }

        currentState = AIState.Walking;
        movementCoroutine = StartCoroutine(DummyCoroutine());
        yield break;
    }

    IEnumerator DummyCoroutine()
    {
        yield return null;
        movementCoroutine = null;
    }

    IEnumerator Chase()
    {
        float chaseTime = Random.Range(minChasetime, maxChasetime);
        yield return new WaitForSeconds(chaseTime);

        if (currentState == AIState.Chasing)
        {
            if (destinations != null && destinations.Count > 0)
            {
                int random = Random.Range(0, destinationAmount);
                currDestination = destinations[random];
            }
            currentState = AIState.Walking;
            movementCoroutine = StartCoroutine(DummyCoroutine());
        }
    }

    private void Dead()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("DeathMenu");
    }

    public void StopChase()
    {
        StopMovementCoroutine();
        if (destinations != null && destinations.Count > 0)
        {
            int random = Random.Range(0, destinationAmount);
            currDestination = destinations[random];
        }
        currentState = AIState.Walking;
    }

    public IEnumerator Stun()
    {
        if (MonsterMove != null)
            AudioManager.instance.StopSound(MonsterMove);

        AudioManager.instance.Play("MonsterHurt", this.transform);
        MonsterMove = null;
        ai.isStopped = true;
        currentState = AIState.Idle; //my monster is stopped
        yield return new WaitForSeconds(3f);
        trigger.TriggerDialogue();
        gameObject.SetActive(false);
        /*ai.isStopped = false;
        int random = Random.Range(0, destinationAmount);
        currDestination = destinations[random];
        currentState = AIState.Walking;*/
    }

    private IEnumerator EnemyAttack()
    {
        if (MonsterAttack == null || !MonsterAttack.isPlaying)
        {
            MonsterAttack = AudioManager.instance.Play("Attack", this.transform);
        }

        canAttack = false;
        ai.isStopped = true;

        if (health != null)
        {
            health.health--;
            health.ChangeColor();
        }

        yield return StartCoroutine(cameraShake.Shake(0.15f, 0.4f));
        yield return new WaitForSeconds(attackCooldown);

        ai.isStopped = false;
        canAttack = true;
    }

    // new helper functionnthat helps stop movement better//
    private void StopMovementCoroutine()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }
}
