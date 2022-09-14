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
    public float playerRotSpeed;
    bool useController;
    public float minCamAngle = 0;
    public float maxCamAngle = 30;
    public Animator animator;
    public float animationSmoothing = 1;
    public Transform cam;
    public Vector3 camRot;
    Vector2 moveAnim;
    Vector3 moveDir;
    public Transform lookDir;

    public float maxSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveDir = transform.forward;
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
        rb.AddRelativeForce(0, 0, moveVector.magnitude * moveSpeed, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        float rotSpeed = useController ? rotSpeedController : rotSpeedMouse;
        //transform.Rotate(0, lookVector.x * rotSpeed, 0);
        camRot.x -= lookVector.y * rotSpeed;
        camRot.x = Mathf.Clamp(camRot.x, minCamAngle, maxCamAngle);
        camRot += new Vector3(0, lookVector.x * rotSpeed, 0);

        moveAnim = Vector2.Lerp(moveAnim, moveVector, Time.fixedDeltaTime * animationSmoothing);
        animator.SetFloat("Blend", moveAnim.magnitude);

        Vector3 camAngle = cam.forward;
        lookDir.LookAt(lookDir.position + new Vector3(camAngle.x, 0, camAngle.z));

        if (moveVector.magnitude > 0)
        {
            //camAngle = camAngle.normalized;
            camAngle.y = 0;
            moveDir = Vector3.Lerp(moveDir, lookDir.TransformDirection(new Vector3(moveVector.x, 0, moveVector.y)), Time.deltaTime * playerRotSpeed);
        }

        transform.LookAt(transform.position + moveDir, Vector3.up);
    }
}
