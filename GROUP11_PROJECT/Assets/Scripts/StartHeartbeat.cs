using UnityEngine;

public class StartHeartbeat : MonoBehaviour
{
    public AudioSource Heart;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Heart == null || !Heart.isPlaying)
            {
                Heart = AudioManager.instance.Play("HeartRate", this.transform);
            }
        }
    }
}
