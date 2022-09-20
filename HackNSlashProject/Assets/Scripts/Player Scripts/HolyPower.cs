using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyPower : MonoBehaviour
{
    public int maxHolyPower = 100;
    public int currentHolyPower;
    public float powerRegenSpeed;
    public float powerRegenDelay;
    float regenDelay;
    bool isRegening;

    private void Start()
    {
        currentHolyPower = maxHolyPower;
    }

    public void UseHolyPower(int powerUsed)
    {
        currentHolyPower -= powerUsed;
        regenDelay = powerRegenDelay;
        if (!isRegening)
            StartCoroutine(RegenHolyPower());

        EventsManager.instance.InvokeOnBarEvent(currentHolyPower, 0, maxHolyPower, gameObject);
    }

    private IEnumerator RegenHolyPower()
    {
        isRegening = true;
        while (currentHolyPower < maxHolyPower)
        {
            while (regenDelay > 0)
            {
                yield return new WaitForFixedUpdate();
                regenDelay -= Time.fixedDeltaTime;
            }
            yield return new WaitForSeconds(1f / powerRegenSpeed);
            currentHolyPower++;

            EventsManager.instance.InvokeOnBarEvent(currentHolyPower, 0, maxHolyPower, gameObject);
        }
        isRegening = false;
    }
}
