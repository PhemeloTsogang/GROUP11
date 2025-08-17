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
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

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
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private float hidingYRotation = 0f;
    public float hidingLookLimit = 45f;
    public Hide player;

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
        HandleMovement();
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

    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        if (player.isHiding)
        {
            transform.Rotate(Vector3.up * mouseX);
            hidingYRotation += mouseX;
            hidingYRotation = Mathf.Clamp(hidingYRotation, -hidingLookLimit, hidingLookLimit);

            Quaternion lockerRotation = Quaternion.Euler(0f, hidingYRotation, 0f);
            transform.localRotation = lockerRotation;
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
