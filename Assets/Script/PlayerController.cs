using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(FloatingObject))]
public class PlayerController : MonoBehaviour
{

    const float PI = Mathf.PI;

    [Range(0.0f,1.0f)]
    public float force;

    public float maxSpeed;

    public float rotationalSpeed;

    public GameObject boatMotor;

    [Range(0, 1)]
    public float rotationSmoothFactor;


    public TMP_Text text;
    

    Quaternion controllerRotationQ;
    Quaternion boatMotorRotationQ;
    Vector3 controllerRotationV;
    Vector3 boatMotorRotationV;
    float rotationY;
    float motorRotationY;
    float controllerRotationY;


    bool isColliding;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isColliding = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move();
        OvrControllerIntrigation();
    }




    void OvrControllerIntrigation()
    {
        OVRInput.FixedUpdate();



        //BoatEngineControll with Controller
        controllerRotationQ = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        //controllerRotationV = controllerRotationQ.eulerAngles;

        text.text = controllerRotationQ.eulerAngles.z.ToString();

        controllerRotationY = controllerRotationQ.eulerAngles.y;
        if (controllerRotationY >= 180 && controllerRotationY <= 360)
        {
            controllerRotationY = controllerRotationY - 360;
        }



        motorRotationY = boatMotor.transform.rotation.eulerAngles.y;
        if (motorRotationY >= 180 && motorRotationY <= 360)
        {
            motorRotationY = motorRotationY - 360;
        }

        boatMotorRotationQ = Quaternion.Euler(0, Mathf.SmoothDamp(motorRotationY, controllerRotationY, ref rotationalSpeed, rotationSmoothFactor), 0);
        boatMotor.transform.rotation = boatMotorRotationQ;



        rotationY = transform.localEulerAngles.y;
        //OVRInput.FixedUpdate();
        if (rb.velocity.x < maxSpeed && rb.velocity.z < maxSpeed)
        {
            //do here
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                rb.AddForce(10 * force * Mathf.Sin(rotationY * PI / 180), 0, 10 * force * Mathf.Cos(rotationY * PI / 180));
            }
            if (controllerRotationQ.eulerAngles.z >= 30 && controllerRotationQ.eulerAngles.z <= 100)
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

            if (controllerRotationQ.eulerAngles.z >= 250 && controllerRotationQ.eulerAngles.z <= 300)
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
