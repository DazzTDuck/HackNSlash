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
            StartCoroutine(RegenningHolyPower());
    }

    IEnumerator RegenningHolyPower()
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
        }
        isRegening = false;
    }
}
