using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hide : MonoBehaviour
{
    public GameObject hideText, stopHidingText, player, hidingSpot, walkieTalkie, lockerScreen, Sam;
    bool interact;
    public bool isHiding;
    //private bool isUsed;

    public FPController fpController;
    public Transform cameraHolder;
    public MeshRenderer playerMesh;
    public Collider playerCollider;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    public Transform hideSpot;
    public bool isLocker;

    //monster settings
    public EnemyAI monsterScript;
    public Transform monster;

    public Material glowMaterial;
    private Material originalMaterial;
    private MeshRenderer targetRenderer;

    private void Awake()
    {
        interact = false;
        isHiding = false;
        //isUsed = false;
        isLocker = false;

        targetRenderer = GetComponent<MeshRenderer>();
        originalMaterial = targetRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (isUsed)
        {
            return;
        }*/

        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            targetRenderer.material = glowMaterial;
            interact = true;

            fpController.locker = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            targetRenderer.material = originalMaterial;
            interact = false;

        }
    }

    public void onHide(InputAction.CallbackContext context)
    {
        /*if (isUsed)
        {
            return;
        }*/

        if (interact == true)
        {
            if (context.performed)
            {  
                walkieTalkie.SetActive(false);
                Sam.SetActive(false);

                if (monsterScript.currentState == EnemyAI.AIState.Chasing)
                {
                    monsterScript.StopChase();
                }

                hideText.SetActive(false);
                targetRenderer.material = originalMaterial;

                if (this.gameObject.CompareTag("Locker"))
                {
                    lockerScreen.SetActive(true);
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

                    Vector3 lockerEuler = hideSpot.rotation.eulerAngles;
                    fpController.lockerBaseYRotation = lockerEuler.y;
                }

                interact = false;

                if (this.gameObject.CompareTag("Locker"))
                {
                    isLocker = true;
                    //isUsed = true;
                }
                /*else
                {
                    isUsed = false;
                }*/
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
                walkieTalkie.SetActive(true);
                Sam.SetActive(true);

                if (this.gameObject.CompareTag("Locker"))
                {
                    lockerScreen.SetActive(false);
                    targetRenderer.material = originalMaterial;
                }

                isHiding = false;
                isLocker = false;

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
