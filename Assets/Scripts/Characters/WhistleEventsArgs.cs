using System;
using UnityEngine;

public class WhistleEventArgs : EventArgs
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
    public WhistleEventArgs(GameObject calledBat, Transform positionWhenWhistled)
    {
        _calledBat = calledBat;
        _positionWhenWhistled = positionWhenWhistled;
    }
}