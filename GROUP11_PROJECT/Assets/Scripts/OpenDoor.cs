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
      Destroy(gameObject);
      openText.SetActive(false);
    }
}
