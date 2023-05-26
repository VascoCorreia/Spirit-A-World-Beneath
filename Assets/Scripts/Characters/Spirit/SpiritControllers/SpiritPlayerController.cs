using UnityEngine;

public class SpiritPlayerController : MonoBehaviour
{
    [field: SerializeField] public Camera _camera { get; private set; }
    [field: SerializeField] protected SpiritPossession _spiritPossession { get; private set; }
    [field: SerializeField] protected CharacterController _controller { get; private set; }


    protected Vector2 playerInput;

    protected virtual void Awake()
    {
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();
        _controller = GetComponent<CharacterController>();
    }
    protected virtual void OnEnable()
    {
        _camera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
    }

    protected virtual void Update()
    {
        return;
    }

    //All child classes require this inputs
    protected virtual void GetPlayerInput()
    {
        playerInput.x = Input.GetAxis("SpiritHorizontal");
        playerInput.y = Input.GetAxis("SpiritVertical");

        //R1
        if (Input.GetButtonDown("SpiritPossession"))
        {
            if (!_spiritPossession.alreadyInPossession)
            {
                _spiritPossession.tryPossession();

            }
            else
            {
                _spiritPossession.ExitPossession();

            }
            //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);
        }
    }

    protected virtual void Actions()
    {
        return;
    }
}
