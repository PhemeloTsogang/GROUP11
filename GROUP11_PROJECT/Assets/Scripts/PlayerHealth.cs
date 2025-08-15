using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public float maxhealth = 100;

    public Slider healthBar;
    
    void Start()
    {
        health = maxhealth;


    }

    
    void Update()
    {
        healthBar.value = health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        print("You Died");
    }
}
