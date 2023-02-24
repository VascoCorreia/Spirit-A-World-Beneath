using UnityEngine;

//The character as of now is 1.56m long in real world units or 0.78 in Unity capsule units. (normal height for 13 year old)
public class HumanPlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool onGround;
    [SerializeField, Range(0f, 10f)] private float maxJumpHeight = 2f;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;

    private CharacterController controller;
    private Vector2 playerInput;
    private float gravityValue;
    private float ySpeed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        gravityValue = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        onGround = controller.isGrounded;

        ySpeed += gravityValue * Time.deltaTime;

        //if we set it to zero the controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (onGround)
        {
            ySpeed = -0.01f;
        }

        Vector3 movementDirection = new Vector3(playerInput.x, 0, playerInput.y);

        //clamping the magnitude to 1 prevents the character from moving faster diagonally.
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * maxSpeed;

        //after calculating the magnitude we can normalize the movement direction vector to get the direction of movement
        movementDirection.Normalize();
     
        if (onGround && Input.GetButtonDown("Jump"))
            ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * gravityValue); //https://screenrec.com/share/62pUYiuKDW

        velocity = movementDirection * magnitude;
        velocity.y = ySpeed;
        velocity = AdjustVelocityToSlope(velocity);

        controller.Move(velocity * Time.deltaTime);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        Debug.DrawRay(transform.position, Vector3.down);

        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1f))
        {
            var rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = rotation * velocity ;

            //only adjust the velocity if were moving down a slope which means out Y component of velocity must be negative, otherwise dont change velocity
            if(adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }
}
