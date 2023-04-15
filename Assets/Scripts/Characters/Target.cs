using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Target : MonoBehaviour
{
    public float health = 50;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Select();
        }
    } 
    void Select()
    {
        Destroy(gameObject);
    }
}
