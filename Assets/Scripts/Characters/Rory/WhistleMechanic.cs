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

    private RoryAnimatorManager _roryAnimatorManager;

    private void Awake()
    {
        _canWhistle = true;
        _roryAnimatorManager = GetComponent<RoryAnimatorManager>();
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
                //play animation
                _roryAnimatorManager.RoryWhistleAnimation();

                //Check if there are bats in the radius around rory
                BatsInRadius = GetBatsInRadius(BatsInRadius);

                //store the position of the whistle for bats to know where they need to go
                Transform positionWhenCalled = gameObject.transform;

                //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);

                //If there is at least one bat -> sucess
                if (BatsInRadius.Count > 0)
                {
                    OnWhistleSucessfull?.Invoke(new WhistleEventArgs(BatsInRadius[UnityEngine.Random.Range(0, BatsInRadius.Count)], positionWhenCalled.position));
                }

                //If there are no bats -> Failure
                if (BatsInRadius.Count == 0)
                {
                    OnWhistleFailed?.Invoke();
                }

                StartCoroutine(Cooldowns.Cooldown(_whisleCooldown, (possessionFlag) => _canWhistle = possessionFlag));
            }
        }
    }
}
