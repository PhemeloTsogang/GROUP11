using UnityEngine;

public class RatScurry : MonoBehaviour
{
    public GameObject rats;
    public Animator rat1, rat2, rat3, rat4, rat5, rat6;
    private AudioSource Rats;
    public Transform rat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rats.SetActive(true);
            Rats = AudioManager.instance.Play("Rats", rat);
            rat1.SetTrigger("Walk");
            rat2.SetTrigger("Walk");
            rat3.SetTrigger("Walk");
            rat4.SetTrigger("Walk");
            rat5.SetTrigger("Walk");
            rat6.SetTrigger("Walk");
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
