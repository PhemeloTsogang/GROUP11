using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
    public GameObject flashScreen;
    public float flashTime;

    //Title: CAMERA SHAKE in Unity
    //Author: Brackeys (Youtube)
    //16 August 2025
    //Availability: https://www.youtube.com/watch?v=9A9yj8KnM8c
    public IEnumerator Shake(float duration, float mag)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * mag;
            float y = Random.Range(-1f, 1f) * mag;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPos;

        StartCoroutine(Flash());
    }
    public IEnumerator Flash()
    {
        flashScreen.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        flashScreen.SetActive(false);
    }
}
