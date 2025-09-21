using UnityEngine;

public class CollectBattery : MonoBehaviour
{
    public FPController player;
    public GameObject pickUpText;
    public BatteryUI battery;
    public bool inCollectRange = false;

    public DialogueTrigger trigger;
    private void Awake()
    {
        inCollectRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null && player.batteryCount < 1)
            {
                player.battery = this;
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
            Destroy(gameObject);
            //trigger.TriggerDialogue();
        }
    }
}
