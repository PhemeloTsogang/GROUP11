using UnityEngine;

public class Speech : MonoBehaviour
{
    public DialogueTrigger trigger;
    private bool hasSaid;

    private void Awake()
    {
        hasSaid = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !hasSaid)
        {
            hasSaid |= true;
            trigger.TriggerDialogue();
        }
    }
}
