using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public FPController battery;

    void Awake()
    {
        text.text = battery.batteryCount + "/1";
    }

    public void UpdateUI(float batCount)
    {
        text.text = batCount + "/1";
    }
}
