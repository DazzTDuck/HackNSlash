using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    void Start()
    {
        instance = this;
    }

    public event Action<int, int> onTakeDamage;
    public void OnTakeDamage(int id, int damageTaken)
    {
        onTakeDamage?.Invoke(id, damageTaken);
    }

    public event Action<int> onDeath;
    public void OnDeath(int id)
    {
        onDeath?.Invoke(id);
    }
}
