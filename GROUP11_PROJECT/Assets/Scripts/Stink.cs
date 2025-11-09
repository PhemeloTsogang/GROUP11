using UnityEngine;
using System.Collections;

public class Stink : MonoBehaviour
{
    public DialogueTrigger trigger;
    public Transform player;
    public float detectDistance;

    private bool hasTriggered;

    private void Awake()
    {
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
                    trigger.TriggerDialogue();
                }
            }
        }
    }
}
