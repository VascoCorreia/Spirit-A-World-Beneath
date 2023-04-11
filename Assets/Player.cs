using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BatAi callingBat;

    private Transform Bat;

    List<GameObject> BatsInRadius = new List<GameObject>();

    public float CallingRadius;
    public Action<calledBatsEventArgs> OnWhistle;

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

            OnWhistle?.Invoke(new calledBatsEventArgs(BatsInRadius[UnityEngine.Random.Range(0, BatsInRadius.Count)], positionWhenCalled));

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

public class calledBatsEventArgs : EventArgs
{
    private Transform _positionWhenWhistled;
    private GameObject _calledBat;

    public GameObject getCalledBat()
    {
        return _calledBat;
    }

    public void setCalledEnemy(GameObject CalledBat)
    {
        _calledBat = CalledBat;
    }

    public Transform getPosition()
    {
        return _positionWhenWhistled;
    }

    public void getSetPosition(Transform calledPosition)
    {
        _positionWhenWhistled = calledPosition;
    }
    public calledBatsEventArgs(GameObject calledBat, Transform positionWhenWhistled)
    {
        _calledBat = calledBat;
        _positionWhenWhistled = positionWhenWhistled;
    }
}