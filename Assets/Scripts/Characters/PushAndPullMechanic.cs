using UnityEngine;

public class PushAndPullMechanic : MonoBehaviour
{
    private HumanPlayerController _controller;
    public static bool isPulling { get; set; } = false;

    private void Awake()
    {
        _controller = GetComponent<HumanPlayerController>();
    }
    private void Update()
    {
        if (isPulling)
        {
            _controller.maxSpeed = 2f;
        }
        else
            _controller.maxSpeed = 10f;
    }
}
