using System.Collections;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    public Animator monster;
    private AudioSource Scuffle;
    public Transform monst;
    public DialogueTrigger trigger;

    private void Awake()
    {
        monster.SetBool("IsCreatureWalkingAnim", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scuffle = AudioManager.instance.Play("Roar", monst);
            monster.SetTrigger("Move");
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(wait());
        }
    }
    
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        trigger.TriggerDialogue();
    }

}
