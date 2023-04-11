using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BatAi callingBat;

    private Transform Bat;

    List<GameObject> BatsInRadius = new List<GameObject>();

    public float CallingRadius;
    public Action<WhistleEventArgs> OnWhistle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CallingBat();
    }

    void CallingBat()
    {
        if (Input.GetButtonDown("space"))
        {
            BatsInRadius = GetBatsInRadius(BatsInRadius);
            Transform positionWhenCalled = gameObject.transform;

            OnWhistle?.Invoke(new WhistleEventArgs(BatsInRadius[UnityEngine.Random.Range(0, BatsInRadius.Count)], positionWhenCalled));

        }
    }

    private List<GameObject> GetBatsInRadius(List<GameObject> objectInRadius)
    {
        Collider[] Temporary = Physics.OverlapSphere(transform.position, CallingRadius);

        foreach (var item in Temporary)
        {
            if (item.gameObject.tag == "Bat")
            {
                objectInRadius.Add(item.gameObject);
            }
        }

        return objectInRadius;
    }
}