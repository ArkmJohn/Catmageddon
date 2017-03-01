using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {   // The agent class is the main component and will make use of the behaviours in order to create intelligent movement

    public float maxSpeed;  // the agent's maxspeed
    public float maxAccel;  // the agent's max acceleration
    public float orientation;  // the agent's orientation(where its facing)
    public float rotation; // the agent's rotation
    public Vector3 velocity; // the agent's velocity
    protected Steering steering; // steering that will be applied to the agent
    void Start()
    {
        velocity = Vector3.zero;  // setting the agent's velocity to zero at start
        steering = new Steering(); // Giving memory to the steering that will be applied to the agent
    }
    public void SetSteering(Steering steering)  // Function to set the current agent's steering
    {
        this.steering = steering;  // "this" reference is being used so the function calls on the agent that its attached to.
    }

    public virtual void Update()  // The update function is handling the movement based on 'current' values (i.e the current movement that needs to be carried out)
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        // we need to limit the orientation values
        // to be in the range (0 – 360)
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }

    public virtual void LateUpdate()  // The late update calculates the steering for the NEXT frame based on the current frames calculations
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        steering = new Steering();
    }


}
