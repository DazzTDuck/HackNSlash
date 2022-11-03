using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyAfterDelay", destroyDelay);
    }

    void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }
}
