using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    private GameObject[] _allBats;
    private GameObject _closestBat;

    void Start()
    {
        _allBats = GameObject.FindGameObjectsWithTag("Bat");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _allBats.Length - 1; i++)
        {
            if (Vector3.Distance(_allBats[i].transform.position, transform.parent.position) > Vector3.Distance(_allBats[i + 1].transform.position, transform.parent.position))
            {
                _closestBat = _allBats[i + 1];
            }
        }

        transform.LookAt(_closestBat.transform);
    }
}
