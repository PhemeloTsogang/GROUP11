using UnityEngine;

public class Speech : MonoBehaviour
{
    public DialogueTrigger trigger;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trigger.TriggerDialogue();
        }
    }
}
