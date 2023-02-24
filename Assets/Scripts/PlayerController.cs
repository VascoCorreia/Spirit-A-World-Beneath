using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 playerInput;
    private bool desiredJump;

    [SerializeField] private Vector3 velocity, desiredVelocity;
    [SerializeField, Range(0f, 10f)] private float maxJumpHeight = 2f;

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f;
    [SerializeField, Range(0f, 300f)] private float maxAcceleration = 200f, maxAirAcceleration = 200f;
    [SerializeField, Range(0f, 90f)] private float maxGroundAngle = 25f;

    private float minGroundDotProduct;

    private Rigidbody body;
    private bool onGround;
    private Collision _collision;
    
    //only get called on editor
    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        OnValidate(); //makes it work on builds too not just the editor
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        /* 
        is the same as "desiredJump = desiredJump | Input.GetButtonDown("Jump")"; If we dont invoke fixedUpdate next frame (which would make the player not jump since we applyForce there)
        it is still equal to true until we manually set it to false 
        */
        desiredJump |= Input.GetButtonDown("Jump");

        //makes character move the same speed in diagonals as in any other direction
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        //if onGround = true -> maxAcceleration else maxAirAcceleration
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //max speed change PER fixedTimestep (0.02 ms)    
        float maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        body.velocity = velocity;
        onGround = false;
    }

    private void Jump()
    {
        if (onGround)
        {
            velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * maxJumpHeight); //https://screenrec.com/share/62pUYiuKDW
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        //onGround = true;
       EvaluateCollision(collision);
    }


    void EvaluateCollision(Collision collision)
    {

        //since the normal vector and the up vector are both unit vectors we can say that the dot product between them = cos(angle) since A . B = ||A||*||B|| * cos(angle)
        _collision = collision;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround = onGround || normal.y >= minGroundDotProduct;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < _collision.contactCount; i++)
        {
            Vector3 normal = _collision.GetContact(i).normal;
            Debug.Log(normal.magnitude);

            //draws normals from collision
            Gizmos.DrawLine(_collision.GetContact(i).point, _collision.GetContact(i).point + normal);

        }
    }
}

