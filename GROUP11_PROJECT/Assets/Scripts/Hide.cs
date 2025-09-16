using UnityEngine;
using UnityEngine.InputSystem;

public class Hide : MonoBehaviour
{
    public GameObject hideText, stopHidingText, stopHidingVentText, blackScreen, player;
    bool interact;
    public bool isHiding;
    private bool isUsed;

    public FPController fpController;
    public Transform cameraHolder;
    public MeshRenderer playerMesh;
    public Collider playerCollider;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    public Transform hideSpot;
    public bool isVent;
    public bool isLocker;

    //monster settings
    public EnemyAI monsterScript;
    public Transform monster;

    private void Awake()
    {
        interact = false;
        isHiding = false;
        isUsed = false;
        isLocker = false;
        isVent = false;

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

                if (this.gameObject.CompareTag("Vent"))
                {
                    stopHidingVentText.SetActive(true);
                    blackScreen.SetActive(true);
                }

                stopHidingText.SetActive(true);
                isHiding = true;

                originalCameraPosition = cameraHolder.position;
                originalCameraRotation = cameraHolder.rotation;

                if (playerMesh != null)
                {
                    playerMesh.enabled = false;
                }

                if (playerCollider != null)
                {
                    playerCollider.enabled = false;
                }

                fpController.controller.enabled = false;

                if (hideSpot != null)
                {
                    cameraHolder.position = hideSpot.position;
                    cameraHolder.rotation = hideSpot.rotation;
                }

                interact = false;

                if (this.gameObject.CompareTag("Locker"))
                {
                    isLocker = true;
                    isUsed = true;
                }
                else
                {
                    isVent = true;
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

                isHiding = false;
                isLocker = false;
                isVent = false;

                if (playerMesh != null)
                {
                    playerMesh.enabled = true;
                }

                if (playerCollider != null)
                {
                    playerCollider.enabled = true;
                }

                fpController.controller.enabled = true;

                cameraHolder.position = originalCameraPosition;
                cameraHolder.transform.rotation = originalCameraRotation;
            }
        }
    }
}
