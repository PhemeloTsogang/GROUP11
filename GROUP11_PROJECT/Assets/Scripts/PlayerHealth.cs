using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public Slider healthSlider;
    public Image fillImage;
    public Color defaultColor = Color.green;
    public Color hurtColor = new Color(1f, 0.5f, 0);
    public Color deathColor = Color.red;

    private void Awake()
    {
        health = 3f;
        fillImage.color = defaultColor; 
    }

    public void ChangeColor()
    {
        if (health == 2)
        {
            fillImage.color = hurtColor;
        }
        else if (health <= 1)
        {
            fillImage.color = deathColor;
        }
    }
}
