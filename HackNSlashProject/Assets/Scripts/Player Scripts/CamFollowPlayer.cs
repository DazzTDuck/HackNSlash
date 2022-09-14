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
    }

    void Update()
    {
        Vector3 move = Vector3.Lerp(transform.position, player.position + camOffset, followspeed * Time.deltaTime);
        transform.position = move;
        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.Euler(player.eulerAngles + new Vector3(pl.udRot, 0, 0)), followspeed * Time.deltaTime);
        transform.rotation = look;
        
        camDir = camPos.position - transform.position;
        if (Physics.Raycast(transform.position, camDir.normalized, out RaycastHit hit, camDst, layerMask))
            camPos.position = hit.point - (camDir.normalized * 0.1f);
        else
            camPos.position = transform.position + camDir.normalized * camDst;
    }
}
