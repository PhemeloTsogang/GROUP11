using System.Collections;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public GameObject closeOff, player, levelText;
    private AudioSource Drip;

    private void Update()
    {
        if (levelText.activeInHierarchy)
        {
            StartCoroutine(wait());
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            levelText.SetActive(true);
            closeOff.SetActive(true);
            if (Drip == null || !Drip.isPlaying)
            {
                Drip = AudioManager.instance.Play("Water", player.transform);
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(4f);
        levelText.SetActive(false);
    }
}
