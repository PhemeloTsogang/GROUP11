using UnityEngine;

public class UniLetter : MonoBehaviour
{
    public DialogueTrigger trigger;
    public bool hasCollected;


    private void Awake()
    {
        hasCollected = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hasCollected)
        {
            trigger.TriggerDialogue();
            gameObject.SetActive(false);
        }
    }
}
