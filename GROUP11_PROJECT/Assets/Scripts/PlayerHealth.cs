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
    public Animator animator;


    private void Awake()
    {
        health = 5f;
        fillImage.color = defaultColor;
        animator.SetInteger("Health", 3);
    }

    public void ChangeColor()
    {
        if (health <=4 && health > 1)
        {
            fillImage.color = hurtColor;
            animator.SetInteger("Health",2 );
        }
        else if (health <= 1)
        {
            fillImage.color = deathColor;
            animator.SetInteger("Health", 1);
        }
    }
}
