using UnityEngine;

public class CollectBattery : MonoBehaviour
{
    public MeshRenderer visible, visible2, visible3, visible4;
    public FPController player;
    public GameObject pickUpText;
    public BatteryUI battery;
    public bool inCollectRange = false;
    private DialogueManager manage;

    public DialogueTrigger trigger;
    private void Awake()
    {
        manage = FindFirstObjectByType<DialogueManager>();
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

            if (manage != null)
            {
                manage.EndDialogue();
            }

            if(gameObject.name == "TUT_BATTERY")
            {
                Destroy(gameObject);
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

            if (gameObject.name == "TUT_BATTERY")
            {
                trigger.TriggerDialogue();
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
