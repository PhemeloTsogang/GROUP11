using System.Collections;
using UnityEngine;

public class FallingBoxes : MonoBehaviour
{
    public Animator box1, box2, box3, box4, box5;
    private AudioSource Scuffle, Fall;
    public Transform box;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Fall = AudioManager.instance.Play("Boxes", box);
            Scuffle = AudioManager.instance.Play("Scuffle", box);
            box1.SetTrigger("Fall3");
            box2.SetTrigger("Fall4");
            box3.SetTrigger("Fall2");
            box4.SetTrigger("Fall5");
            box5.SetTrigger("Fall1");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
