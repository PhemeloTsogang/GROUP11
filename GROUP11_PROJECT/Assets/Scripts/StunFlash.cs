using System.Collections;
using UnityEngine;

public class StunFlash : MonoBehaviour
{
    public GameObject flashScreen;
    public float flashTime;

    public IEnumerator Flash()
    {
        flashScreen.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        flashScreen.SetActive(false);
    }
}
