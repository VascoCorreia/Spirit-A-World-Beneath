using System;
using System.Collections;
using UnityEngine;

//class used for cooldowns
static class Cooldowns
{
    public static IEnumerator Cooldown(float cooldownTime, Action<bool> callback)
    {
        callback(false);
        yield return new WaitForSeconds(cooldownTime);
        callback(true);
    }
}

