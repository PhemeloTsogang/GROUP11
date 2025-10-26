using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject openText;
    public FPController player;
    public bool canOpen;

    private void Awake()
    {
        canOpen = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && player.keyPartCount == 5)
        {
            if (player != null)
            {
                player.door = this;
                openText.SetActive(true);
                canOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openText.SetActive(false);
            canOpen = false;
        }
    }

    public void Open()
    {
      canOpen = false;

      if (player.door != null)
        {
            player.door = null;
        }
      
      gameObject.SetActive(false);
      openText.SetActive(false);
    }
}
