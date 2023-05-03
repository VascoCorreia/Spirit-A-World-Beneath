using UnityEngine;

//Debugging purposes General Class
public class DebuggingHelper : MonoBehaviour
{
    public static DebuggingHelper Instance { get; private set; }

    [SerializeField] SpiritPlayerController _playerController;
    [SerializeField] SpiritPossession _spiritPossession;
    [SerializeField] RoryManager _humanPlayerController;
    [SerializeField] float DistanceFromController;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    //void Update()
    //{
    //    //TrueDistanceFromPlayerToObjectRay(_playerController._camera, _playerController., _spiritPossession._possessableDistance, out DistanceFromController);

    //}

    //void TrueDistanceFromPlayerToObjectRay(Camera camera, CharacterController controller, float distanceToTest, out float DistanceFromController)
    //{
    //    DistanceFromController = distanceToTest - Vector3.Distance(camera.transform.position, controller.transform.position) - controller.radius;

    //    Debug.DrawRay(camera.transform.position, camera.transform.forward * distanceToTest, Color.yellow);
    //}
}
