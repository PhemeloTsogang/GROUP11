using UnityEngine;
using UnityEngine.InputSystem;

public class Hide : MonoBehaviour
{
    private CharacterController controller;
    public GameObject hideText, stopHidingText, stopHidingVentText, blackScreen;
    public GameObject normalPlayer, hidingPlayer;
    bool interact;
    public bool isHiding;
    private bool isUsed;

    //monster settings
    public EnemyAI monsterScript;
    public Transform monster;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   
        interact = false;
        isHiding = false;
        isUsed = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed)
        {
            return;
        }

        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            interact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            interact = false;
        }
    }

    public void onHide(InputAction.CallbackContext context)
    { 
        if (isUsed)
        {
            return;
        }

        if (interact == true)
        {
            if (context.performed)
            {
                if (monsterScript.currentState == EnemyAI.AIState.Chasing)
                {
                    monsterScript.StopChase();
                }

                hideText.SetActive(false);
                hidingPlayer.SetActive(true);

                if(this.gameObject.CompareTag("Vent"))
                {
                    stopHidingVentText.SetActive(true);
                    blackScreen.SetActive(true);
                }

                stopHidingText.SetActive(true);
                isHiding = true;
                normalPlayer.SetActive(false);
                interact = false;

                if (this.gameObject.CompareTag("Locker")) //makes only the lockers a one time use, not the vents.
                {
                    isUsed = true;
                }
                else
                {
                    isUsed = false;
                }
            }
        }
    }

    public void onStopHide(InputAction.CallbackContext context)
    {
        if (isHiding == true)
        { 
            if(context.performed)
            {
                stopHidingText.SetActive(false);

                if (this.gameObject.CompareTag("Vent"))
                {
                    stopHidingVentText.SetActive(false);
                    blackScreen.SetActive(false);
                }

                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);
                isHiding = false;
            }
        }
    }
}
