using UnityEngine;

public class CollectPart : MonoBehaviour
{
    public FPController player;
    public GameObject pickUpText;
    public KeyPartUI part;
    public bool inCollectRange = false;

    private void Awake()
    {
        inCollectRange = false;
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
            Destroy(gameObject);
        }
    }
}

