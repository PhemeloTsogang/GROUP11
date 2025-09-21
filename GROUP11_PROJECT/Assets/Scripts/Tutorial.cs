using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public MeshRenderer walkie, walkie2;
    public GameObject collectText;
    public bool inRange;
    public FPController player;
    public DialogueTrigger trigger;
    private DialogueManager manage;

    private void Awake()
    {
        manage = FindFirstObjectByType<DialogueManager>();
        inRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && walkie.enabled)
        {
            collectText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        inRange = false;

        if (manage != null)
        {
            manage.EndDialogue();
        }
    }

    public void Collect()
    {
        if (inRange && player != null)
        {
            player.canCollect = false;
            collectText.SetActive(false);
            trigger.TriggerDialogue();
            walkie.enabled = false;
            walkie2.enabled = false;
        }
    }
}
