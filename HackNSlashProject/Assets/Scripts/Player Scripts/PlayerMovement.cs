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
    Vector3 moveAnim;
    Vector3 moveDir;
    public Transform lookDir;
    public float camFollowSpeed;

    public float maxSpeed;

    public bool lockedOn;
    public Transform lockonTarget;

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
    public void OnLockOn(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (lockedOn)
                lockedOn = false;
            else
                lockedOn = true;
        }
    }
    public void OnLockonSwitch(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && lockedOn && CombatManager.combatManager.engagedEnemies.Count > 0)
        {
            for (int i = 0; i < CombatManager.combatManager.engagedEnemies.Count; i++)
            {
                if (lockonTarget != CombatManager.combatManager.engagedEnemies[i].transform)
                {
                    lockonTarget = CombatManager.combatManager.engagedEnemies[i].transform;
                    break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(0, 0, moveVector.magnitude * moveSpeed, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        

        float rotSpeed = useController ? rotSpeedController : rotSpeedMouse;
        camRot.x -= lookVector.y * rotSpeed;
        camRot.x = Mathf.Clamp(camRot.x, minCamAngle, maxCamAngle);
        camRot.z = 0;
        camRot += new Vector3(0, lookVector.x * rotSpeed, 0);
        if (camRot.y >= 180)
            camRot.y -= 360;
        else if (camRot.y < -180)
            camRot.y += 360;

        moveAnim = Vector3.Lerp(moveAnim, rb.velocity, Time.fixedDeltaTime * animationSmoothing);
        animator.SetFloat("Blend", moveAnim.magnitude / maxSpeed);

        Vector3 camAngle = cam.forward;
        lookDir.LookAt(lookDir.position + new Vector3(camAngle.x, 0, camAngle.z));

        if (moveVector.magnitude > 0)
        {
            //camAngle = camAngle.normalized;
            camAngle.y = 0;
            moveDir = Vector3.Lerp(moveDir, lookDir.TransformDirection(new Vector3(moveVector.x, 0, moveVector.y)), Time.fixedDeltaTime * playerRotSpeed);

            if (lookVector.magnitude == 0 && Vector3.Dot(transform.forward, lookDir.forward) > -0.7f && !(lockedOn && lockonTarget))
            {
                if (transform.eulerAngles.y - camRot.y < 180 && transform.eulerAngles.y - camRot.y > -180)
                    camRot = Vector3.Lerp(camRot, transform.eulerAngles + new Vector3(camRot.x, 0, 0), Time.fixedDeltaTime * camFollowSpeed);
                else if (transform.eulerAngles.y - camRot.y > 180)
                    camRot = Vector3.Lerp(camRot, transform.eulerAngles - new Vector3(-camRot.x, 360, 0), Time.fixedDeltaTime * camFollowSpeed);
                else if (transform.eulerAngles.y - camRot.y < -180)
                    camRot = Vector3.Lerp(camRot, transform.eulerAngles + new Vector3(camRot.x, 360, 0), Time.fixedDeltaTime * camFollowSpeed);

            }
        }

        transform.LookAt(transform.position + moveDir, Vector3.up);

    }
}
