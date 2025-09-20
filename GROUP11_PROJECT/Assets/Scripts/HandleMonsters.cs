using UnityEngine;

public class HandleMonsters : MonoBehaviour
{
    public GameObject monster, tutMonster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutMonster.SetActive(false);
            monster.SetActive(true);
        }
    }
}
