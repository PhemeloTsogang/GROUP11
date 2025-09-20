using UnityEngine;

public class Openable : MonoBehaviour
{
    public GameObject collectText;
    public bool inRange;
    public FPController player;

    private void Awake()
    {
        inRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.cabinet = this;
            collectText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
            Destroy(gameObject);
        }
    }
}
