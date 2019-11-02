using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FloatingObject))]
public class PlayerController : MonoBehaviour
{

    const float PI = Mathf.PI;

    [Range(0.0f,1.0f)]
    public float force;

    public float maxSpeed;

    public float rotationalSpeed;

    Rigidbody rb;
    float rotationY;
    bool isColliding;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isColliding = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        ovrControllMove();
    }




    void ovrControllMove()
    {
        //OVRInput.FixedUpdate();
        if (rb.velocity.x < maxSpeed && rb.velocity.z < maxSpeed)
        {
            //do here
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                rb.AddForce(10 * force * Mathf.Sin(rotationY * PI / 180), 0, 10 * force * Mathf.Cos(rotationY * PI / 180));
            }
        }

        //Decelaration
        if (rb.velocity.x > -maxSpeed && rb.velocity.z > -maxSpeed)
        {
            if (OVRInput.Get(OVRInput.RawButton.RTouchpad))
            {
                rb.AddForce(-force * Mathf.Sin(rotationY * PI / 180), 0, force * -Mathf.Cos(rotationY * PI / 180));
            }
        }

    }





    void move()
    {
        //Rotate
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(0, -rotationalSpeed * Time.deltaTime, 0);

            rb.angularVelocity = new Vector3(0, 1, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Rotate(0, rotationalSpeed * Time.deltaTime, 0);
        }

        

        //Forward and backward movement

        rotationY = transform.localEulerAngles.y;
        //accelearation
        if (rb.velocity.x < maxSpeed && rb.velocity.z < maxSpeed)
        {
            //do here
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(10 * force * Mathf.Sin(rotationY * PI / 180), 0, 10 * force * Mathf.Cos(rotationY * PI / 180));
            }
        }

        //Decelaration
        if (rb.velocity.x > -maxSpeed && rb.velocity.z > -maxSpeed)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                rb.AddForce(-force * Mathf.Sin(rotationY * PI / 180), 0, force * -Mathf.Cos(rotationY * PI / 180));
            }
        }


        print(rb.angularVelocity.y);
    }


}
