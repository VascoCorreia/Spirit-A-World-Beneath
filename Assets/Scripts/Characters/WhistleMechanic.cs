using System;
using System.Collections.Generic;
using UnityEngine;

public class WhistleMechanic : MonoBehaviour
{
    [SerializeField] private float _whisleCooldown = 2f;
    [SerializeField] private float _callingRadius;
    [SerializeField] private bool _canWhistle;

    private List<GameObject> BatsInRadius = new List<GameObject>();

    public Action<WhistleEventArgs> OnWhistleSucessfull;
    public Action OnWhistleFailed;


    private void Awake()
    {
        _canWhistle = true;
    }

    // Update is called once per frame
    void Update()
    {
        //R1
        Whistle();
    }

    private List<GameObject> GetBatsInRadius(List<GameObject> objectInRadius)
    {
        Collider[] Temporary = Physics.OverlapSphere(transform.position, _callingRadius);

        foreach (var item in Temporary)
        {
            if (item.gameObject.tag == "Bat")
            {
                objectInRadius.Add(item.gameObject);
            }
        }

        return objectInRadius;
    }

    private void Whistle()
    {
        if (Input.GetButtonDown("HumanWhistle"))
        {
            if (_canWhistle)
            {
                BatsInRadius = GetBatsInRadius(BatsInRadius);
                Transform positionWhenCalled = gameObject.transform;

                if (BatsInRadius.Count > 0)
                {
                    OnWhistleSucessfull?.Invoke(new WhistleEventArgs(BatsInRadius[UnityEngine.Random.Range(0, BatsInRadius.Count)], positionWhenCalled.position));
                }

                if (BatsInRadius.Count == 0)
                {
                    OnWhistleFailed?.Invoke();
                }

                StartCoroutine(Cooldowns.Cooldown(_whisleCooldown, (possessionFlag) => _canWhistle = possessionFlag));
            }
        }
    }
}
