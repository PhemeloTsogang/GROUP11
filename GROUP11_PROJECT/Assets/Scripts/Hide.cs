using UnityEngine;
using UnityEngine.InputSystem;

public class Hide : MonoBehaviour
{
    private CharacterController controller;
    public GameObject hideText, stopHidingText;
    public GameObject normalPlayer, hidingPlayer;
    bool interact;
    public bool isHiding;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();   
        interact = false;
        isHiding = false;

    }

    private void OnTriggerEnter(Collider other)
    {
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
        if (interact == true)
        {
            if (context.performed)
            {
                hideText.SetActive(false);
                hidingPlayer.SetActive(true);
                stopHidingText.SetActive(true);
                isHiding = true;
                normalPlayer.SetActive(false);
                interact = false;
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
                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);
                isHiding = false;
            }
        }
    }
}
