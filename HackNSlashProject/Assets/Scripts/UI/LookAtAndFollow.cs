using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtAndFollow : MonoBehaviour
{
    public Transform follow;
    public Vector3 offset;
    public Transform lookAt;              
    
    private void Update()
    {
        //lookAt
        transform.LookAt(lookAt);
        
        //follow object
        transform.position = follow.position + offset;
    }
}
