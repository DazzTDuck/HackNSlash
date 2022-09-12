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
    bool useController;
    public float minCamAngle = 0;
    public float maxCamAngle = 30;
    public Animator animator;
    public Transform model;
    public float animationSmoothing = 1;
    Vector3 toLook;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb.drag = 6;
        toLook = model.forward;
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
        float rotSpeed = useController ? rotSpeedController : rotSpeedMouse;
        transform.Rotate(0, lookVector.x * rotSpeed, 0);
        udRot -= lookVector.y * rotSpeed;
        udRot = Mathf.Clamp(udRot, minCamAngle, maxCamAngle);

        Vector3 forLook = new Vector3(moveVector.x, 0, moveVector.y);
        //model.TransformDirection(forLook);
        //toLook = Vector3.Lerp(toLook, new Vector3(forLook.x + toLook.x, forLook.y + toLook.y, forLook.z + toLook.z), animationSmoothing * Time.fixedDeltaTime);
        toLook = Vector3.Lerp(toLook, forLook, animationSmoothing * Time.fixedDeltaTime);
        animator.SetFloat("Blend", toLook.magnitude);
        model.LookAt(model.position + transform.TransformDirection(toLook), Vector3.up);
    }
}
