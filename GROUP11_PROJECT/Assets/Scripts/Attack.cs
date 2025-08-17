using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : MonoBehaviour
{
    public GameObject flashScreen;
    public float flashTime;

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
