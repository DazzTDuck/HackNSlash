using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 moveVector;
    Vector2 lookVector;
    public float moveSpeed;
    public float rotSpeedMouse;
    public float rotSpeedController;
    public float udRot;
    public float minCamAngle = 0;
    public float maxCamAngle = 30;
    bool useController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb.drag = 6;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLookMouse(InputAction.CallbackContext callbackContext)
    {
        lookVector = callbackContext.ReadValue<Vector2>();
        useController = false;
    }
    public void OnLookController(InputAction.CallbackContext callbackContext)
    {
        lookVector = callbackContext.ReadValue<Vector2>();
        useController = true;
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(moveVector.x * moveSpeed, 0, moveVector.y * moveSpeed, ForceMode.Acceleration);
        if (!useController)
        {
            transform.Rotate(0, lookVector.x * rotSpeedMouse, 0);
            udRot -= lookVector.y * rotSpeedMouse;
        }
        else
        {
            transform.Rotate(0, lookVector.x * rotSpeedController, 0);
            udRot -= lookVector.y * rotSpeedController;
        }
        udRot = Mathf.Clamp(udRot, minCamAngle, maxCamAngle);
    }
}
