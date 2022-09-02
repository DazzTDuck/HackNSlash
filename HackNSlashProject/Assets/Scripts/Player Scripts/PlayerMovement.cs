using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 moveVector;
    public Vector2 lookVector;
    public float moveSpeed;
    public float rotSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext callbackContext)
    {
        lookVector = callbackContext.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(moveVector.x * moveSpeed, 0, moveVector.y * moveSpeed, ForceMode.Acceleration);
        transform.Rotate(0, lookVector.x * rotSpeed, 0);
        rb.drag = 6;
    }
}
