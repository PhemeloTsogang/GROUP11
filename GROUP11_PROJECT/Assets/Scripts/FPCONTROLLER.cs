using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



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
    //initializers for head bob movement while running
    private bool isSprinting = false;

    public float amplitutde = 0.015f;
    private float frequency = 10.0f;

    public Transform _camera;
    public Transform cameraHolder;

    private Vector3 startPos;
    //


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalSpeed = moveSpeed;
        sprintSpeed = moveSpeed * 3f;
        startPos = _camera.localPosition;

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
            ResetPostion();
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

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
