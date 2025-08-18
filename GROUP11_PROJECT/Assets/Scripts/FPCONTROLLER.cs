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
    private string lastControlScheme = "Keyboard&Mouse";
    public float verticalLookLimit = 90f;
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
    public StunFlash flash;
    public GameObject stunText,normalPlayer;
    public BatteryUI batteryUI;


    [Header("KeyPart Settings")]
    public CollectPart part;
    public float keyPartCount;

    [Header("KeyPart Settings")]
    public OpenDoor door;


    [Header("General Settings")]
    public CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    public float hidingYRotation = 0f;
    public float hidingLookLimit = 45f;
  

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalSpeed = moveSpeed;
        sprintSpeed = moveSpeed * 2f;
        startPos = _camera.localPosition;
        batteryCount = 0;
        keyPartCount = 0;

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

       
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }
        else if (context.canceled)
        {
            isSprinting = false;
            moveSpeed = originalSpeed;
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

    private void ResetPostion()
    { 
        if (_camera.localPosition == startPos)
        { 
            return; 
        }

        _camera.localPosition = Vector3.Lerp(_camera.localPosition, startPos, 5f * Time.deltaTime);
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

    public void AddBattery()
    {
        batteryCount++;
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

    public void AddPart()
    {
        keyPartCount++;
    }

    public void onStun(InputAction.CallbackContext context)
    {
        if (context.performed && stun.inStunRange && batteryCount == 1)
        {
            StartCoroutine(monster.Stun());
            StartCoroutine(flash.Flash());
            batteryCount--;
            batteryUI.UpdateUI(batteryCount);

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

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }
}
