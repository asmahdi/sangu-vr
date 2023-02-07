using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollusionManager : MonoBehaviour
{
    [SerializeField] GameObject boatPrefab;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject[] spwnLocations; 
    //[SerializeField] private string objectName;



    GameObject boat;
    GameObject newBoat;
    int closestPointIndex;

    // Update is called once per frame
    void Update()
    {
        boat = GameObject.Find("Boat");
        newBoat = GameObject.Find("Boat(Clone)");


        if(boat != null)
        {
            ClosestPoint(boat.transform.position);
        }
        else if (newBoat != null)
        {
            ClosestPoint(newBoat.transform.position);
        }
        else
        {
            Instantiate(boatPrefab, spwnLocations[closestPointIndex].transform.position, spwnLocations[closestPointIndex].transform.rotation);
        }
    }

    void ClosestPoint(Vector3 position)
    {
        closestPointIndex = 0;
        float minDistance = 100000;

        for(int i=0; i< spwnLocations.Length; i++)
        {
            float distance = Vector3.Distance(position, spwnLocations[i].transform.position);
            if (minDistance > distance)
            {
                closestPointIndex = i;
                minDistance = distance;
            }
        }
    }
}
