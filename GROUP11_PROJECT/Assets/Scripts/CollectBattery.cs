using UnityEngine;

public class CollectBattery : MonoBehaviour
{
    public MeshRenderer visible, visible2, visible3, visible4;
    public FPController player;
    public GameObject pickUpText;
    public BatteryUI battery;
    public bool inCollectRange = false;

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
            if (player != null && player.batteryCount < 1)
            {
                player.battery = this;
                pickUpText.SetActive(true);
                targetRenderer.material = glowMaterial;
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
            if (player != null && player.battery == this)
            {
                player.battery = null;
            }
        }
    }

    public void Collect()
    {
        if (inCollectRange && player != null && player.batteryCount < 1)
        {
            player.AddBattery();
            battery.UpdateUI(player.batteryCount);
            pickUpText.SetActive(false);
            targetRenderer.material = originalMaterial;

            if (gameObject.CompareTag("Battery"))
            {
                visible.enabled = false;
                visible2.enabled = false;
                visible3.enabled = false;
                visible4.enabled = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
