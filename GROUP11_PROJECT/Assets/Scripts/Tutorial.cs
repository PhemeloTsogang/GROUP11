using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public MeshRenderer walkie, walkie2;
    public GameObject collectText;
    public bool inRange;
    public FPController player;
    public DialogueTrigger trigger;
    private DialogueManager manage;
    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer targetRenderer;



    private void Awake()
    {
        manage = FindFirstObjectByType<DialogueManager>();
        targetRenderer = GetComponent<MeshRenderer>();
        inRange = false;

        originalMaterial = targetRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && walkie.enabled)
        {
            collectText.SetActive(true);
            inRange = true;
            targetRenderer.material = glowMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        inRange = false;
        targetRenderer.material = originalMaterial;

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
            targetRenderer.material = originalMaterial;
            walkie.enabled = false;
            walkie2.enabled = false;
        }
    }
}
