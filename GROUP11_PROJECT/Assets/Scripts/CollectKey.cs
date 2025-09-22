using Unity.Hierarchy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CollectKey : MonoBehaviour
{
    public GameObject collectText;
    public bool inRange;
    public FPController controller;
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
            originalMaterial = targetRenderer.material;
            controller.key = this;
            collectText.SetActive(true);
            targetRenderer.material = glowMaterial;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        inRange = false;
        targetRenderer.material = originalMaterial;
        if (controller != null && controller.key == this)
        {
            controller.key = null;
        }
    }

    public void Collect()
    {
        if (inRange && controller != null)
        {
            controller.AddKey();
            collectText.SetActive(false);
            targetRenderer.material = originalMaterial;
            Destroy(gameObject);
        }
    }
} 
