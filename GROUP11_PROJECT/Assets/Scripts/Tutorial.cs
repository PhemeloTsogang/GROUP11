using UnityEngine;

public class Tutorial : MonoBehaviour
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
            collectText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        inRange = false;
    }

    public void Collect()
    {
        if (inRange && player != null)
        {
            player.canCollect = false;
            collectText.SetActive(false);
            Destroy(gameObject);
            Debug.Log("IS this bullshit working?");
        }
    }
}
