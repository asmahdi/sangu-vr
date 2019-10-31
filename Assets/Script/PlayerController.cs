using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    const float PI = Mathf.PI;

    [Range(0.1f,0.5f)]
    public float speed;

    [Range(1.0f,4.0f)]
    public float speedMultiplier;


    [Range(1.0f, 2.0f)]
    public float rotationalSpeed;

    Rigidbody rb;
    float rotationY;

   


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //moveAndRotate();
        move();
        //Debug.Log(Mathf.Sin(90*Mathf.PI/180));
        //print(transform.localEulerAngles);

    }



    void move()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -rotationalSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotationalSpeed * Time.deltaTime, 0);
        }


        rotationY = transform.localEulerAngles.y;


        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Down");
        } else
        {
            rb.velocity = new Vector3(speed * Mathf.Sin(rotationY * PI / 180), 0, speed * Mathf.Cos(rotationY * PI / 180));
            print(rb.velocity);
        }


        

    }
    

    //void moveAndRotate() 
    //{
    //    //movement
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        Debug.Log("Down");
    //        transform.Translate(0, 0, speed * speedMultiplier * Time.deltaTime);
    //    }
    //    else
    //    {
    //        transform.Translate(0, 0, speed * Time.deltaTime);
    //    }


    //    //Rotation
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        transform.Rotate(0, -rotationalSpeed * Time.deltaTime, 0);
    //    }
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        transform.Rotate(0, rotationalSpeed * Time.deltaTime, 0);
    //    }
    //}
}
