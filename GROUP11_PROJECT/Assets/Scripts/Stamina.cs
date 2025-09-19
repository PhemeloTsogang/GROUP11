using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    //Title: Creating a STAMINA SYSTEM in Unity | Unity UI
    //Author: SpeedTutor (Youtube)
    //18 September 2025
    //Availability: https://www.youtube.com/watch?v=Fs2YCoamO_U

    [Header("Mechanic Settings")]
    public float playerStamina = 100f;
    public float maxStamina = 100f;
    public bool isSprinting = false;
    public bool hasRegenerated = true;
    public float stamDrain = 0.5f;
    public float stamRegen = 0.5f;

    [Header("UI Settings")]
    public Image staminaProgressUI;
    public CanvasGroup staminaCanvasGroup;

    [Header("General Settings")]
    private FPController player;


    void Start()
    {
        player = GetComponent<FPController>();
    }

    void Update()
    {
        if (!isSprinting)
        {
            if (playerStamina <= maxStamina - 0.01)
            {
                playerStamina += stamRegen * Time.deltaTime;
                updateStamina(1);


                if (playerStamina >= maxStamina)
                {
                    hasRegenerated = true;
                    player.sprintSpeed = player.originalSprintSpeed;
                    staminaCanvasGroup.alpha = 0;
                }
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            isSprinting = true;
            playerStamina -= stamDrain * Time.deltaTime;
            updateStamina(1);

            if (playerStamina <= 0)
            {
                hasRegenerated = false;
                player.sprintSpeed = 5f;
                staminaCanvasGroup.alpha = 0;

                player.Tired();
            }
        }
    }
    private void updateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        
        if (value == 0)
        {
            staminaCanvasGroup.alpha = 0;
        }
        else if (value == 1)
        {
            staminaCanvasGroup.alpha = 1;
        }
    }

}
