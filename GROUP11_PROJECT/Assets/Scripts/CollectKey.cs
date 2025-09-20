using Unity.Hierarchy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CollectKey : MonoBehaviour
{
    public GameObject collectText;
    public bool inRange;
    public FPController controller;

    private void Awake()
    {
        inRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.key = this;
            collectText.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectText.SetActive(false);
        inRange = false;
        if (controller != null && controller.key == this)
        {
            controller.key = null;
        }
    }

    public void Collect()
    {
        if (inRange && controller != null)
        {
            controller.AddKey();
            collectText.SetActive(false);
            Destroy(gameObject);
        }
    }
} 
