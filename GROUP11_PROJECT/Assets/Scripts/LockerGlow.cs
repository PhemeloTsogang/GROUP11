using UnityEngine;

public class LockerGlow : MonoBehaviour
{
    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer targetRenderer;

    private void Awake()
    {
        targetRenderer = GetComponent<MeshRenderer>();
        originalMaterial = targetRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        targetRenderer.material = glowMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        targetRenderer.material = originalMaterial;
    }
}
