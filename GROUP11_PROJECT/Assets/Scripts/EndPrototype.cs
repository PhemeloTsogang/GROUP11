using UnityEngine;

public class EndPrototype : MonoBehaviour
{
    public GameObject endlevel;

    private void OnTriggerEnter(Collider other)
    {
        endlevel.SetActive(true);
    }
}
