using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyPartUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public FPController battery;

    void Awake()
    {
        text.text = battery.batteryCount + "/5";
    }

    public void UpdateUI(float partCount)
    {
        text.text = partCount + "/5";
    }
}
