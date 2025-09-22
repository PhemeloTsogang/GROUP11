using UnityEngine;

public class Openable : MonoBehaviour
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
        inRange = false;
        originalMaterial = targetRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetRenderer.material = glowMaterial;
            player.cabinet = this;
            collectText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetRenderer.material = originalMaterial;
        collectText.SetActive(false);
        inRange = false;
        if (player != null && player.cabinet == this)
        {
            player.cabinet = null;
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
