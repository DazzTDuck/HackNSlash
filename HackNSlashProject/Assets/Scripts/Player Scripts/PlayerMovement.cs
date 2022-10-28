using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float moveAccel;
    public float moveDecel;
    public float stopVel;
    Rigidbody rb;
    Vector2 moveVector;
    Vector2 lookVector;
    Vector3 moveDir;

    [HideInInspector] public Vector3 gForceVector = Vector3.zero;
    [HideInInspector] public bool isStationary;
    Vector3 velLastFrame;
    float runSpeedPercent;
    float inputmagnitude;
    Vector3 curWorldInput;

    [Header("Rotating")]
    public float mouseSensitivity;
    public float controllerSinsitivity;
    public float playerRotSpeed;

    [Header("Camera")]
    public float minCamAngle = 0;
    public float maxCamAngle = 30;
    public float camFollowSpeed;
    public Transform lookDir;
    public Transform cam;
    public Vector3 camRot;

    [Header("Animations")]
    public Animator animator;
    public float animationSmoothing = 1;
    Vector3 moveAnim;

    [Header("Combat")]
    public bool lockedOn;
    public int lockonTargetIndex;
    public bool canAct = true;
    PlayerSwordAttack swordAttack;
    PlayerHolyWater holyWater;
    PlayerDivineScripture divineScripture;
    [HideInInspector] public HolyPower holyPower;

    [Header("Interactions")]
    public Interactable interactable;
    bool finishCleanse;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = transform.forward;
        swordAttack = GetComponentInChildren<PlayerSwordAttack>();
        holyWater = GetComponentInChildren<PlayerHolyWater>();
        holyPower = GetComponentInChildren<HolyPower>();
        divineScripture = GetComponent<PlayerDivineScripture>();
        canAct = true;
        EventsManager.instance.CleanseUpdateEvent += Cleanse;
    }
    private void OnDestroy()
    {
        EventsManager.instance.CleanseUpdateEvent -= Cleanse;
    }

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLookMouse(InputAction.CallbackContext callbackContext)
    {
        lookVector = callbackContext.ReadValue<Vector2>();
    }
    public void OnLockOn(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !PauseGameHandler.isPaused)
        {
            if (lockedOn)
                lockedOn = false;
            else if (CombatManager.combatManager.engagedEnemies.Count > 0)
            {
                lockedOn = true;
                lockonTargetIndex = 0;
            }
        }
    }
    public void OnLockonSwitch(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && lockedOn && CombatManager.combatManager.engagedEnemies.Count > 0 && !PauseGameHandler.isPaused)
        {
            lockonTargetIndex++;
            if (lockonTargetIndex >= CombatManager.combatManager.engagedEnemies.Count)
                lockonTargetIndex = 0;
        }
    }

    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !PauseGameHandler.isPaused)
            swordAttack.Attack(this);
    }
    public void OnHolyWater(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !PauseGameHandler.isPaused)
            holyWater.UseHolyWater(this);
    }
    public void OnDivineScripture(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !PauseGameHandler.isPaused)
            divineScripture.ReadScripture(this);
    }

    public void OnInteract(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && canAct && interactable && !PauseGameHandler.isPaused)
            interactable.Interact(true);
        else if (callbackContext.canceled && interactable && !PauseGameHandler.isPaused)
            interactable.Interact(false);
    }

    void Cleanse(object sender, CleanseUpdateArgs cleanseUpdateArgs)
    {
        if (cleanseUpdateArgs.isCleansing == true)
        {
            animator.SetBool("Cleansing", true);
            canAct = false;
        }
        else if (cleanseUpdateArgs.isCleanseCompleted == true)
        {
            finishCleanse = true;
            StartCoroutine(FinishCleansing());
        }
        else if (cleanseUpdateArgs.isCleansing == false && !finishCleanse)
        {
            animator.SetBool("Cleansing", false);
            canAct = true;
        }
    }
    
    IEnumerator FinishCleansing()
    {
        yield return new WaitForSeconds(0.5f);
        canAct = true;
        finishCleanse = false;
    }

    private void FixedUpdate()
    {
        Movement();
        GForceCalculation();

        if (CombatManager.combatManager.engagedEnemies.Count == 0 && lockedOn)
            lockedOn = false;

        float rotSpeed = InputChecker.usesController ? controllerSinsitivity : mouseSensitivity;
        camRot.x -= lookVector.y * rotSpeed;
        camRot.x = Mathf.Clamp(camRot.x, minCamAngle, maxCamAngle);
        camRot.z = 0;
        camRot += new Vector3(0, lookVector.x * rotSpeed, 0);
        if (camRot.y >= 180)
            camRot.y -= 360;
        else if (camRot.y < -180)
            camRot.y += 360;

        //moveAnim = Vector3.Lerp(moveAnim, rb.velocity, Time.fixedDeltaTime * animationSmoothing);
        animator.SetFloat("Blend", rb.velocity.magnitude / moveSpeed);

        Vector3 camAngle = cam.forward;
        lookDir.LookAt(lookDir.position + new Vector3(camAngle.x, 0, camAngle.z));

        if (moveVector.magnitude > 0 && canAct)
        {
            //camAngle = camAngle.normalized;
            camAngle.y = 0;
            moveDir = Vector3.Lerp(moveDir, lookDir.TransformDirection(new Vector3(moveVector.x, 0, moveVector.y)), Time.fixedDeltaTime * playerRotSpeed);

            if (lookVector.magnitude == 0 && Vector3.Dot(transform.forward, lookDir.forward) > -0.7f && !lockedOn)
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

    void Movement()
    {
        curWorldInput = transform.TransformDirection(0, 0, moveVector.magnitude);
        inputmagnitude = curWorldInput.magnitude;
        curWorldInput = new Vector3(curWorldInput.x, 0, curWorldInput.z).normalized;

        float targetSpeed = moveSpeed * inputmagnitude;
        Vector3 playerVel = rb.velocity;
        Vector3 velHorizontal = new Vector3(playerVel.x, 0, playerVel.z);

        isStationary = velHorizontal.sqrMagnitude < stopVel && inputmagnitude == 0;
        if (isStationary || !canAct)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), 5 * Time.fixedDeltaTime);
            return;
        }

        float playerVelMag = playerVel.x * playerVel.x + playerVel.z * playerVel.z;

        if (inputmagnitude > 0)
            runSpeedPercent = playerVelMag / (targetSpeed * targetSpeed);
        else
            runSpeedPercent = 2;

        float overSpeedMult = 0;
        if (runSpeedPercent > 1)
        {
            overSpeedMult = Mathf.Clamp01(runSpeedPercent - 1);
            runSpeedPercent = Mathf.Clamp01(runSpeedPercent);
        }

        float velAccelerationMult = 1 - runSpeedPercent;
        velAccelerationMult *= moveAccel;
        Vector3 finalForce = curWorldInput * velAccelerationMult + -velHorizontal * overSpeedMult * moveDecel;
        Vector3 inputCrossVelocity = Vector3.Cross(curWorldInput, velHorizontal.normalized);

        inputCrossVelocity = Quaternion.AngleAxis(90, curWorldInput) * inputCrossVelocity;
        finalForce += inputCrossVelocity * moveAccel * 0.5f;
        rb.AddForce(finalForce, ForceMode.Acceleration);
    }
    void GForceCalculation()
    {
        var playerAccel = (rb.velocity - velLastFrame) / Time.fixedDeltaTime;
        gForceVector = transform.InverseTransformVector(playerAccel) / -Physics.gravity.y;
        velLastFrame = rb.velocity;
    }
}
