using System;
using UnityEngine;
public class possessionEventArgs : EventArgs
{
    private GameObject _possessedEnemy;

    public GameObject getPossessedEnemy()
    {
        return _possessedEnemy;
    }

    public void setPossessedEnemy(GameObject possessedEnemy)
    {
        _possessedEnemy = possessedEnemy;
    }
    public possessionEventArgs(GameObject possessedEnemy)
    {
        _possessedEnemy = possessedEnemy;
    }
}

