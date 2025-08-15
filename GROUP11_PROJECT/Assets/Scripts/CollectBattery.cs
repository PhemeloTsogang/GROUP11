using UnityEngine;

public class CollectBattery : MonoBehaviour
{
    public int batteryCount;
    public FPController battery;

    private void Awake()
    {
        batteryCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Battery"))
        {
            if (batteryCount >= 1)
            {
                return;
            }
            batteryCount++;
            Destroy(other.gameObject);
        }
    }
}
