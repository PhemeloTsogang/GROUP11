using UnityEngine;

public class CollectPart : MonoBehaviour
{
    public FPController player;
    public GameObject pickUpText;
    public KeyPartUI part;
    public bool inCollectRange = false;
    public DialogueTrigger trigger;
    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer targetRenderer;

    private void Awake()
    {
        inCollectRange = false;
        targetRenderer = GetComponent<MeshRenderer>();
        originalMaterial = targetRenderer.material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.part = this;
                pickUpText.SetActive(true);
                inCollectRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickUpText.SetActive(false);
            targetRenderer.material = originalMaterial;
            inCollectRange = false;

            if (player != null && player.part == this)
            {
                player.part = null;
            }

        }
    }

    public void Collect()
    {
        if (inCollectRange && player != null)
        {
            player.AddPart();
            part.UpdateUI(player.keyPartCount);
            pickUpText.SetActive(false);
            targetRenderer.material = originalMaterial;
            if (gameObject.CompareTag("Trophy") || gameObject.CompareTag("Bracelet"))
            {
                trigger.TriggerDialogue();
            }
            
            Destroy(gameObject);
        }
    }
}

