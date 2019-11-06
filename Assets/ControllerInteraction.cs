
using UnityEngine;

public class ControllerInteraction : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    private RaycastHit hit;

    private GameObject hitObject;

    void FixedUpdate()
    {
        

       transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");

            

            if( OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyUp(KeyCode.Space))
            {
                hitObject = hit.transform.gameObject;
                hitObject.transform.SetParent(transform);
                
                
            }
        }
    }


  
}
