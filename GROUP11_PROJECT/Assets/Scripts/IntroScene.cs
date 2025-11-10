using UnityEngine;
using System.Collections;

public class IntroScene : MonoBehaviour
{
    private DialogueManager manage;
    public DialogueTrigger trigger;
    public Transform player;
    public float detectDistance;
    public AudioManager manager;

    private bool hasTriggered;

    private void Awake()
    {
        manage = FindFirstObjectByType<DialogueManager>();
        hasTriggered = false;
    }

    private void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, detectDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {

                if (!hasTriggered)
                {
                    hasTriggered = true;
                    StartCoroutine(wait());
                    
                    if(manager.Heart == null || !manager.Heart.isPlaying)
                    {
                        manager.Heart = AudioManager.instance.Play("HeartRate", player);
                    }
                }
            }
        }
        else
        {
            manage.EndDialogue();
            gameObject.SetActive(false);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(wait());
    }

    private void OnTriggerExit(Collider other)
    {
        manage.EndDialogue();
    }*/

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.75f);
        trigger.TriggerDialogue();
    }
}
