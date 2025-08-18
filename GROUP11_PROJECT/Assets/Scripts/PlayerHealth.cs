using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public float health;

    public Slider HealthBar;

    private void Awake()
    {
        health = 3f;
    }
}
