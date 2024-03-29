using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYoureGoingBehavior : Steering
{
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steering = new SteeringData();
        
        if(GetComponent<Rigidbody>().velocity.magnitude == 0)
        {
            return steering;
        }
        float angle = Mathf.Atan2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z) * Mathf.Rad2Deg;
        steering.angular = Mathf.LerpAngle(transform.rotation.eulerAngles.y, angle, steeringbase.maxAcceleration * Time.deltaTime);
        steering.linear = Vector3.zero;
        return steering;
    }
}
