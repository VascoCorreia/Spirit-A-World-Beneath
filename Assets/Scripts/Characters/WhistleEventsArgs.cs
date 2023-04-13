using System;
using UnityEngine;

public class WhistleEventArgs : EventArgs
{
    private Vector3 _playerPositionWhenWhistled;
    private GameObject _calledBat;

    public GameObject getCalledBat()
    {
        return _calledBat;
    }

    public void setCalledEnemy(GameObject CalledBat)
    {
        _calledBat = CalledBat;
    }

    public Vector3 getPlayerPositionWhenWhistled()
    {
        return _playerPositionWhenWhistled;
    }

    public void setPlayerPositionWhenWhistled(Vector3 calledPosition)
    {
        _playerPositionWhenWhistled = calledPosition;
    }
    public WhistleEventArgs(GameObject calledBat, Vector3 playerPositionWhenWhistled)
    {
        _calledBat = calledBat;
        _playerPositionWhenWhistled = playerPositionWhenWhistled;
    }
}