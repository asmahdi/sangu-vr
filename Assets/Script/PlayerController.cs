using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(FloatingObject))]
public class PlayerController : MonoBehaviour
{

    const float PI = Mathf.PI;

    [Tooltip("Given force to boat")]
    [Range(0.0f, 1.0f)] [SerializeField] private float force;
    [SerializeField] private float maxSpeed;
    [SerializeField] private GameObject boatMotor;
    [Range(0, 1)] [SerializeField] private float motorRotationSmoothFactor;
    [Range(0, 1)] [SerializeField] private float boatRotationMultiplier;
    [Range(1.0f, 1.1f)] [SerializeField] private float activeForcedDrag;

    [Tooltip("Create empty GameObjects whose transform will be used for spawn")]
    [SerializeField] GameObject[] spawnPoints;

    [Tooltip("Add Collectable Stones Tag Here")]
    [SerializeField] string collectableStoneTag;
    [Tooltip("Add respawn Tag Here")]
    [SerializeField] string respawnTag;

    [Tooltip("Collected Stone will be store in these position")]
    [SerializeField] GameObject[] collectedStonesSotrage;

    [SerializeField] private AudioSource boatEngineAudio;
    [SerializeField] private float maxPitch,minPitch;
    [SerializeField] private GameObject endCanvas;


    public TMP_Text text;


    Quaternion controllerRotationQ;
    Quaternion targetBoatMotorRotation;
    Quaternion currentBoatMotorRotation;
    Vector3 controllerRotationV;
    Vector3 currentBoatMotorRotationV;
    Vector3 targetBoatMotorRotationV;
    Vector3 forceVector;
    Vector2 initialTouch;
    Vector2 finalTouch;
    float rotationY;
    float motorRotationY;
    float controllerRotationY;
    float motorRotationOffset;
    bool isBoatEngineActive;
    int closestPointIndex;
    int emptyStoneIndex;
    int touchCount;
    float pitchInterPolation;

    private bool activeControll;



    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        emptyStoneIndex = 0;
        isBoatEngineActive = true;
        activeControll = true;
        endCanvas.SetActive(false);

    }

    private void FixedUpdate()
    {
        
            OvrControllerIntrigation();
        
       if ( OVRInput.Get(OVRInput.RawButton.Back) || Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("Game");
        }
        
        Application.targetFrameRate = 72;
    }


    void OvrControllerIntrigation()
    {
        //Default Oculus Intrigation needed
        //for get input of the motion sensor
        OVRInput.FixedUpdate();
        controllerRotationQ = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);



        //This code is switching between the boat engine
        //mode and environment control mode;
        Vector2 ovrTochInput = OVRInput.Get(OVRInput.RawAxis2D.RTouchpad);

        if (OVRInput.Get(OVRInput.RawTouch.RTouchpad))
        {
            finalTouch = OVRInput.Get(OVRInput.RawAxis2D.RTouchpad);
            touchCount++;


        }
        else
        {
            if (touchCount > 8 && finalTouch.x - initialTouch.x > 0.6f)
            {
                isBoatEngineActive = !isBoatEngineActive;
            }

            touchCount = 0;
            initialTouch = OVRInput.Get(OVRInput.RawAxis2D.RTouchpad);
        }

        //text.text = isBoatEngineActive.ToString();



        //This code is responsible for rotating
        //the back motor of the boat 
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
        targetBoatMotorRotation = Quaternion.Slerp(currentBoatMotorRotation, targetBoatMotorRotation, motorRotationSmoothFactor);
        boatMotor.transform.rotation = targetBoatMotorRotation;


        //This little block of code
        //Rotates the boat with the controller input
        float boatVelocity = rb.velocity.magnitude;
        rb.angularVelocity = new Vector3(0, boatVelocity * Mathf.Sin(Mathf.Deg2Rad * controllerRotationY) * -0.5f * boatRotationMultiplier, 0);



        //This block is accelarating the boat
        //
        rotationY = transform.localEulerAngles.y;
        if (rb.velocity.magnitude < maxSpeed)
        {
            
                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKey(KeyCode.A) && activeControll)
                {
                
                    rb.AddForce(10 * force * Mathf.Sin(rotationY * PI / 180), 0, 10 * force * Mathf.Cos(rotationY * PI / 180));
                    EngineStart();
                    
                }
                else
                {
                    EngineStandby();
                    float dragForceMagnitude = rb.velocity.magnitude * rb.drag * activeForcedDrag;
                    Vector3 dragVector = new Vector3(dragForceMagnitude * rb.velocity.normalized.x, 0, dragForceMagnitude * rb.velocity.normalized.z);
                    rb.AddForce(dragVector);
                }
            
        }

        //Decelaration is currently disabled for better experience. 
        //After sometimes later it may give good experience by twiking the code little bit;

        /**
        if (rb.velocity.magnitude < maxSpeed)
        {
            if (OVRInput.Get(OVRInput.RawButton.RTouchpad) || Input.GetKey(KeyCode.Z))
            {
                rb.AddForce(-force * Mathf.Sin(rotationY * PI / 180), 0, force * -Mathf.Cos(rotationY * PI / 180));
            }
        }
        **/

    }




    //This Method is used for find the colosest spawn location
    void ClosestPoint(Vector3 position)
    {
        closestPointIndex = 0;
        float minDistance = 100000;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float distance = Vector3.Distance(position, spawnPoints[i].transform.position);
            if (minDistance > distance)
            {
                closestPointIndex = i;
                minDistance = distance;
            }
        }
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == respawnTag)
        {
            //Find the Closest SpawnLocation
            ClosestPoint(gameObject.transform.position);

            //Move gameobject or new place
            rb.velocity = Vector3.zero;
            gameObject.transform.position = spawnPoints[closestPointIndex].transform.position;
            gameObject.transform.rotation = spawnPoints[closestPointIndex].transform.rotation;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == collectableStoneTag)
        {
            GameObject stone = other.gameObject;
            Destroy(stone.GetComponent<BoxCollider>());
            stone.transform.position = collectedStonesSotrage[emptyStoneIndex].transform.position;
            stone.transform.parent = gameObject.transform;

            emptyStoneIndex++;
        }

        if (other.gameObject.tag == "End")
        {
            activeControll = false;
            endCanvas.SetActive(true);
        }
    }

    private void EngineStart()
    {

        if (pitchInterPolation < 1 )
        {
            pitchInterPolation += 0.01f;
        }
        
        boatEngineAudio.pitch = Mathf.Lerp(boatEngineAudio.pitch,maxPitch, pitchInterPolation);
    }
    private void EngineStandby()
    {

        if (pitchInterPolation > 0)
        {
            pitchInterPolation -= 0.01f;
        }

        boatEngineAudio.pitch = Mathf.Lerp(boatEngineAudio.pitch, minPitch, pitchInterPolation);
    }

}
