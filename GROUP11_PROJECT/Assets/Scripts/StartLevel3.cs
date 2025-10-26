using System.Collections;
using UnityEngine;

public class StartLevel2 : MonoBehaviour
{
    public GameObject closeOff, player, levelText, monst1, monst2;
    private AudioSource Drip;
    public AudioManager manager;
    public StartHeartbeat heartbeat;
    public FPController memory;


    private void Update()
    {
        if (levelText.activeInHierarchy)
        {
            StartCoroutine(wait());
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (monst1.activeInHierarchy)
            {
                monst1.SetActive(false);
            }

            if (!monst2.activeInHierarchy)
            {
                monst2.SetActive(true);
            }

            AudioManager.instance.StopSound(manager.Heart);
            if (heartbeat.Heart != null)
            {
                AudioManager.instance.StopSound(heartbeat.Heart);
            }

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
