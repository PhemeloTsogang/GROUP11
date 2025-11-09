using System.Collections;
using UnityEngine;

public class Lvl2Monster : MonoBehaviour
{
    public GameObject lvl2Monst;
    public DialogueTrigger trigger;

    private void OnTriggerEnter(Collider other)
    {
        lvl2Monst.SetActive(true);
        trigger.TriggerDialogue();

    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
