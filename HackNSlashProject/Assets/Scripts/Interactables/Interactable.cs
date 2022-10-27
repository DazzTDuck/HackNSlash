using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool canInteract;
    public float interactRadius;
    PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    public virtual void Interact(bool pressDown)
    {

    }

    protected virtual void FixedUpdate()
    {
        if (canInteract)
        {
            CheckInteract();
        }
        else if (player.interactable = this)
            player.interactable = null;

    }

    void CheckInteract()
    {
        Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (Vector3.Distance(player.transform.position, transform.position) <= interactRadius)
        {
            if (player.interactable == null || (player.interactable != this && 
                    Vector3.Distance(player.transform.position, player.interactable.transform.position)
                    > Vector3.Distance(transform.position, player.transform.position)))
                player.interactable = this;
        }
        else if (player.interactable == this)
            player.interactable = null;
    }
}
