using UnityEngine;

public class OpenTutDoor : MonoBehaviour
{
    public GameObject openText;
    public FPController player;
    public bool inTutRange;

    private void Awake()
    {
        inTutRange = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                inTutRange = true;
                openText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTutRange = false;
            openText.SetActive(false);
        }
    }

    public void Open()
    {
        Destroy(gameObject);
        openText.SetActive(false);
    }
}
