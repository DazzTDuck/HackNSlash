using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followspeed;
    PlayerMovement pl;

    private void Start()
    {
        pl = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector3 move = Vector3.Lerp(transform.position, player.position, followspeed * Time.deltaTime);
        transform.position = move;
        //Vector3 look = Vector3.Lerp(transform.rotation.eulerAngles, player.rotation.eulerAngles, followspeed * Time.deltaTime);
        Quaternion look = Quaternion.Slerp(transform.rotation, Quaternion.Euler(player.eulerAngles + new Vector3(pl.udRot, 0, 0)), followspeed * Time.deltaTime);
        transform.rotation = look;

    }
}
