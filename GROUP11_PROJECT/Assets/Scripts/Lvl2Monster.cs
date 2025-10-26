using UnityEngine;

public class Lvl2Monster : MonoBehaviour
{
    public GameObject lvl2Monst;

    private void OnTriggerEnter(Collider other)
    {
        lvl2Monst.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
