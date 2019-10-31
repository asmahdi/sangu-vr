using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBoat : MonoBehaviour
{
    public float waterLevel = 0.0f;
    public float floatingThreshold = 2.0f;
    public float waterDensity = 0.125f;
    public float downForce = 4.0f;

    public GameObject forcePoint;


    float forceFactor;
    Vector3 floatingForce;
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        forceFactor = 1.0f - ((transform.position.y - waterLevel) / floatingThreshold);

        if (forceFactor > 0.0f)
        {
            floatingForce = -Physics.gravity * (forceFactor - rb.velocity.y * waterDensity);
            floatingForce += new Vector3(0.0f, -downForce, 0.0f);

            rb.AddForceAtPosition(floatingForce, transform.position);
            //rb.AddForce(floatingForce, ForceMode.Acceleration);
        }
    }
}
