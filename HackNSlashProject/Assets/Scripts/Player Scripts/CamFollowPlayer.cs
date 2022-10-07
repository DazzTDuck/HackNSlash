using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followspeed;
    public Transform camPos;
    Vector3 camDir;
    float camDst;
    PlayerMovement pl;
    public LayerMask layerMask;
    public Vector3 camOffset;

    private void Start()
    {
        pl = player.GetComponent<PlayerMovement>();
        camPos = GetComponentInChildren<Camera>().transform;
        camDst = Vector3.Distance(transform.position, camPos.position);
        camDir = camPos.position - transform.position;
    }

    void Update()
    {
        Vector3 move = Vector3.Slerp(transform.position, player.position + camOffset, followspeed * Time.deltaTime);
        transform.position = move;
        if (pl.lockedOn)
        {
            transform.LookAt(CombatManager.combatManager.engagedEnemies[pl.lockonTargetIndex].transform);
            transform.Rotate(pl.camRot.x, 0, 0);
        }
        else
        {
            Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pl.camRot), followspeed * Time.deltaTime);
            transform.rotation = look;
        }
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(camDir).normalized, out RaycastHit hit, camDst, layerMask))
            camPos.position = hit.point - (camDir.normalized * 0.1f);
        else
            camPos.position = transform.position + transform.TransformDirection(camDir).normalized * camDst;
    }
}
