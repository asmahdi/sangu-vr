using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(FloatingObject))]
public class PlayerController : MonoBehaviour
{

    const float PI = Mathf.PI;

    [Tooltip("Given force to boat")]
    [Range(0.0f, 1.0f)] [SerializeField] private float force;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationalSpeed;
    [SerializeField] private GameObject boatMotor;
    [Range(0, 1)] [SerializeField] private float motorRotationSmoothFactor;
    [Range(0, 1)] [SerializeField] private float boatRotationMultiplier;
    [Range(1.0f, 1.1f)] [SerializeField] private float activeForcedDrag;


    //public TMP_Text text;


    Quaternion controllerRotationQ;
    Quaternion targetBoatMotorRotation;
    Quaternion currentBoatMotorRotation;
    Vector3 controllerRotationV;
    Vector3 currentBoatMotorRotationV;
    Vector3 targetBoatMotorRotationV;
    Vector3 forceVector;
    float rotationY;
    float motorRotationY;
    float controllerRotationY;
    float motorRotationOffset;


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


    int minFrameRate = 60;
    int frameCounter = 0;
    float timeCounter = 0;
    int framerate = 0;

    private void Update()
    {
        frameCounter++;
        timeCounter += Time.deltaTime;

        if (timeCounter >= 1)
        {
            framerate = frameCounter;
            frameCounter = 0;
            timeCounter = 0;
        }

        float fr = (1 / Time.deltaTime);

        if (fr < minFrameRate)
        {
            minFrameRate = (int)fr;
        }



        //text.text = "F " + framerate + "\nmin " + minFrameRate;
    }


    void OvrControllerIntrigation()
    {
        OVRInput.FixedUpdate();
        controllerRotationQ = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);



        //BoatEngineControll with Controller
        //controllerRotationV = controllerRotationQ.eulerAngles;

        //-------------------------------------------------------------------------
        //For Debug Purpose
        //text.text = controllerRotationQ.eulerAngles.ToString();
        if (OVRInput.GetDown(OVRInput.RawButton.Back))
        {
            minFrameRate = 60;
        }
        //------------------------------------------------------------------------------

        controllerRotationY = controllerRotationQ.eulerAngles.y;
        motorRotationY = boatMotor.transform.rotation.eulerAngles.y;
        if (controllerRotationY >= 180 && controllerRotationY <= 360)
        {
            controllerRotationY = controllerRotationY - 360;
        }

        if (motorRotationY >= 180 && motorRotationY <= 360)
        {
            motorRotationY = motorRotationY - 360;
        }

        motorRotationOffset = transform.rotation.eulerAngles.y;
        targetBoatMotorRotationV = new Vector3(0, controllerRotationY + motorRotationOffset, 0);
        targetBoatMotorRotation = Quaternion.Euler(targetBoatMotorRotationV);

        currentBoatMotorRotation = boatMotor.transform.rotation;

        


        //Rotate the Boat
        //Main Boat Controll
        targetBoatMotorRotation = Quaternion.Slerp(currentBoatMotorRotation, targetBoatMotorRotation, motorRotationSmoothFactor);
        boatMotor.transform.rotation = targetBoatMotorRotation;



        float boatVelocity = rb.velocity.magnitude;
        rb.angularVelocity = new Vector3(0, boatVelocity * Mathf.Sin(Mathf.Deg2Rad * controllerRotationY) * -0.5f * boatRotationMultiplier, 0);


        rotationY = transform.localEulerAngles.y;
        //OVRInput.FixedUpdate();
        if (rb.velocity.magnitude < maxSpeed)
        {
            //do here
            if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKey(KeyCode.A))
            {
                rb.AddForce(10 * force * Mathf.Sin(rotationY * PI / 180), 0, 10 * force * Mathf.Cos(rotationY * PI / 180));
            }
            else
            {
                float dragForceMagnitude = rb.velocity.magnitude * rb.drag * activeForcedDrag;
                Vector3 dragVector = new Vector3(dragForceMagnitude * rb.velocity.normalized.x, 0, dragForceMagnitude * rb.velocity.normalized.z);
                rb.AddForce(dragVector);
            }
        }

        //Decelaration
        if (rb.velocity.magnitude < maxSpeed)
        {
            //if (OVRInput.Get(OVRInput.RawButton.RTouchpad) || Input.GetKey(KeyCode.Z))
            //{
            //    rb.AddForce(-force * Mathf.Sin(rotationY * PI / 180), 0, force * -Mathf.Cos(rotationY * PI / 180));
            //}
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        //text.text = "Collided";
       // Destroy(gameObject);
    }


}
