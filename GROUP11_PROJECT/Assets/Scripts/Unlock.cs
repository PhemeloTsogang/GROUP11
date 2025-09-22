using UnityEngine;

public class Unlock : MonoBehaviour
{
    public GameObject collectText;
    public bool inRange;
    public FPController player;
    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer targetRenderer;


    private void Awake()
    {
        targetRenderer = GetComponent<MeshRenderer>();
        originalMaterial = targetRenderer.material;
        inRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.unlockDoor = this;
            collectText.SetActive(true);
            targetRenderer.material = glowMaterial;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        targetRenderer.material = originalMaterial;
        inRange = false;
        if (player != null && player.unlockDoor == this)
        {
            player.unlockDoor = null;
        }
    }

    public void Open()
    {
        if (inRange && player != null)
        {
            collectText.SetActive(false);
            targetRenderer.material = originalMaterial;
            Destroy(gameObject);
        }
    }
}
