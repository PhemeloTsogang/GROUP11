using UnityEngine;

public class HandleMonsters : MonoBehaviour
{
    public GameObject memories, memoryCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            memories.SetActive(true);
            memoryCount.SetActive(true);    
            
        }
    }
}
