using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static EnemyAI;



public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 0.5f;
    public float controllerSensitivity = 50f;
    public float lockerBaseYRotation = 0f;
    private string lastControlScheme = "Keyboard&Mouse";
    public float verticalLookLimit = 90f;
    public float horizontalPeekLimit = 60f;
    public float peekLimit = 180f;

    [Header("Sprint Settings")]
    public float originalSpeed;
    public float sprintSpeed;
    private bool isSprinting = false;

    [Header("Head Bob Settings")]
    public float amplitutde = 0.015f;
    private float frequency = 10.0f;

    public Transform _camera;
    public Transform cameraHolder;

    private Vector3 startPos;

    [Header("Stun Settings")]
    public WalkieTalkie stun;
    public CollectBattery battery;
    public float batteryCount = 0;
    public EnemyAI monster;
    public TutorialMonster tut;
    public StunFlash flash;
    public GameObject stunText,normalPlayer;
    public BatteryUI batteryUI;


    [Header("KeyPart Settings")]
    public CollectPart part;
    public float keyPartCount;

    [Header("Door Settings")]
    public OpenDoor door;

    [Header("Stamina Settings")]
    public Stamina stamina;
    public float originalSprintSpeed;

    [Header("Key Settings")]
    public CollectKey key;
    public float keyCount;

    [Header("Cabinet Settings")]
    public Openable cabinet;

    [Header("WalkieTalkie Settings")]
    public Tutorial walkieTalkie;
    public GameObject walkie;
    public bool canCollect;

    [Header("Tutorial Settings")]
    public float memoryCount;
    public OpenTutDoor tutDoor;
    public bool canOpen;

    [Header("UnlockDoor Settings")]
    public Unlock unlockDoor;

    [Header("General Settings")]
    public CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f; 
    public float hidingYRotation = 0f;
    public float hidingLookLimit = 45f; 

    [Header("Hide Settings")]
    public Hide locker;

    [Header("Audio Settings")]
    private AudioSource Walk, Run, Breathe;

    [Header("Monster Handling Settings")]
    public GameObject tutMonst, Monst;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalSpeed = moveSpeed;
        sprintSpeed = moveSpeed * 2.5f;
        originalSprintSpeed = sprintSpeed;
        startPos = _camera.localPosition;
        batteryCount = 0;
        keyPartCount = 0;
        keyCount = 0;
        memoryCount = 0;    
        walkie.SetActive(false);
        canCollect = true;
        canOpen = false;
    }

    private void Update()
    {
        if (controller.enabled)
        {
            HandleMovement();
        }

        HandleLook();


        if (isSprinting && controller.isGrounded && moveInput != Vector2.zero)
        {
            stamina.Sprinting();
            headShake();
        }
        else
        {
            //ResetPostion();
        }


        if (!normalPlayer.activeInHierarchy)
        {
            monster.ai.isStopped = false;
        }

        if (memoryCount == 1 && tutDoor.inTutRange)
        {
            canOpen = true;
        }

       
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (Walk == null || !Walk.isPlaying)
        {
            Walk = AudioManager.instance.Play("Walk", this.transform);
        }

        if (moveSpeed != sprintSpeed)
        {
            stamina.isSprinting = false;
        }

        moveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            AudioManager.instance.StopSound(Walk);
            Walk = null;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.control != null)
        {
            var device = context.control.device;
            if (device is Mouse)
            {
                lastControlScheme = "Keyboard&Mouse";
            }    

            else if (device is Gamepad)
            {
                lastControlScheme = "Gamepad";
            }
        }
        lookInput = context.ReadValue<Vector2>();
    }

    public void onSprint(InputAction.CallbackContext context)
    {

        if (context.performed && controller.isGrounded)
        {
            if (Run == null || !Run.isPlaying)
            {
                Run = AudioManager.instance.Play("Run", this.transform);
            }

            if (Breathe == null || !Breathe.isPlaying)
            {
                Breathe = AudioManager.instance.Play("Breathe", this.transform);
            }

            isSprinting = true;
            moveSpeed = sprintSpeed;
        }
        else if (context.canceled)
        {
            isSprinting = false;
            moveSpeed = originalSpeed;

            stamina.isSprinting = false;
            AudioManager.instance.StopSound(Run);
            Run = null;

            AudioManager.instance.StopSound(Breathe);
            Breathe = null;
        }
    }

    public void Tired()
    {
        isSprinting = false;
        moveSpeed = originalSpeed;

        if (Run != null)
        {
            AudioManager.instance.StopSound(Run);
            Run = null;
        }

        if (Breathe != null)
        {
            AudioManager.instance.StopSound(Breathe);
            Breathe = null;
        }
    }

    //Title: Unity Quick Guide - FPS Head Bob + Stabilization
    //Author: Hero 3D (Youtube)
    //15 August 2025
    //Availability: https://www.youtube.com/watch?v=5MbR2qJK8Tc
    private void headShake()
    {
        float bobX = Mathf.Cos(Time.time * frequency / 2f) * amplitutde * 2f;
        float bobY = Mathf.Sin(Time.time * frequency) * amplitutde;

        Vector3 bobOffset = new Vector3(bobX, bobY, 0);
        _camera.localPosition = startPos + bobOffset;
    }

    public void onCollectBat(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (battery != null && battery.inCollectRange && batteryCount < 1)
            {
                battery.Collect();
                battery = null;
            }
        }
    }

    public void onOpenCabinet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cabinet != null && cabinet.inRange)
            {
                cabinet.Open();
                cabinet = null;
            }
        }
    }

    public void onUnlockDoor(InputAction.CallbackContext context)
    {
        if (context.performed && keyCount > 0)
        {
            if (unlockDoor != null && unlockDoor.inRange)
            {
                keyCount--;
                unlockDoor.Open();
                unlockDoor = null;
            }
        }
    }

    public void AddBattery()
    {
        batteryCount++;
    }

    public void AddKey()
    {
        keyCount++;
    }

    public void onCollectKeyPart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (part != null && part.inCollectRange)
            {
                part.Collect();
                part = null;
            }
        }
    }

    public void onCollectWalkie(InputAction.CallbackContext context)
    {
        if (context.performed && canCollect)
        {
            if (walkieTalkie.inRange && walkieTalkie != null)
            {
                memoryCount++;
                walkieTalkie.Collect();
                walkie.SetActive(true);
                walkieTalkie = null;
            }
        }
    }

    public void onCollectKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (key != null && key.inRange)
            {
                key.Collect();
                key = null;
            }
        }
    }

    public void AddPart()
    {
        keyPartCount++;
    }

    public void onStun(InputAction.CallbackContext context)
    {
        if (context.performed && stun.inStunRange && batteryCount == 1)
        {
            if (Monst.activeInHierarchy)
            {
                AudioManager.instance.Play("Stun", this.transform);
                StartCoroutine(monster.Stun());
                StartCoroutine(flash.Flash());
                batteryCount--;
                batteryUI.UpdateUI(batteryCount);
            }
            else if (tutMonst.activeInHierarchy)
            {
                AudioManager.instance.Play("Stun", this.transform);
                StartCoroutine(tut.Stun());
                StartCoroutine(flash.Flash());
                batteryCount--;
                batteryUI.UpdateUI(batteryCount);
            }

        }

        stunText.SetActive(false);
    }

    public void onOpen(InputAction.CallbackContext context)
    {
        if (context.performed && door.canOpen)
        {
            door.Open();
        }
    }

    public void onOpenTut(InputAction.CallbackContext context)
    {
        if (context.performed && canOpen)
        {
            tutDoor.Open();
            memoryCount--;
            canOpen = false;
        }
    }

    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //Title: How to convert mouse sensitivity from FPS games into Unity!
    //Author: NeatoDev (Youtube)
    //18 August 2025
    //Availability: https://www.youtube.com/watch?v=81GXWMRubsA
    public void HandleLook()
    {
        float sensitivity = lastControlScheme == "Gamepad" ? controllerSensitivity : mouseSensitivity;

        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        if (locker.isHiding & locker.isLocker)
        {
            horizontalRotation += mouseX;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -horizontalPeekLimit, horizontalPeekLimit);
            locker.cameraHolder.rotation = Quaternion.Euler(0f, lockerBaseYRotation + horizontalRotation, 0f);
        }
        else if (locker.isHiding & locker.isVent)
        {

        }
        else 
        {
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

    }
}
