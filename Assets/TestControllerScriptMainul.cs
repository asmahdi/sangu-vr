using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OVR;

public class TestControllerScriptMainul : MonoBehaviour
{
	Color[] colors = new Color[]{ Color.white, Color.red, Color.green };

	static int colorIndex, length;

    //public GameObject handle;
    public TMP_Text text;

    float timer;

    Quaternion qrot;

    // Start is called before the first frame update
    void Start()
    {
		colorIndex = 0;
		length = colors.Length;
		GetComponent<Renderer>().material.color = colors[colorIndex];
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ovrIntrigation();

        if (Input.GetKeyDown(KeyCode.A))
        {
            colorIndex++;
            if(colorIndex == 3)
            {
                colorIndex = 0;
            }
        }

        GetComponent<Renderer>().material.color = colors[colorIndex];
    }

    void ovrIntrigation()
    {
        OVRInput.Update();

        GetComponent<Renderer>().material.color = Color.blue;

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //Debug.Log("Button 1");
            //text.text= "Button 1";
            colorIndex++;
            if (colorIndex == 3)
            {
                colorIndex = 0;
            }
        }

        qrot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

        text.text = qrot.eulerAngles.y.ToString();
        //handle.transform.rotation = qrot;

        //if (OVRInput.Get(OVRInput.RawButton.RTouchpad))
        //{

        //    //Debug.Log("Button back");
        //    //text.text="Button Back";
        //    timer += Time.deltaTime;
        //    text.text = timer.ToString();
        //    colorIndex++;
        //    if (colorIndex == 3)
        //    {
        //        colorIndex = 0;
        //    }
        //}



        
    }
}
