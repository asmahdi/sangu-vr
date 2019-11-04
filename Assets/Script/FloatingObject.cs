using UnityEngine;

public class FloatingObject : MonoBehaviour {


	[Tooltip("Value of the floating baseline in Y axis")]
	public float floatingOffset;

	[Tooltip("Value of the additional uplift force")]
	public float upliftForceOffset=1 ;

	[Tooltip("Rotation pivot in X axis")]
	[Range(-1,1)]
	public float pivotOffsetX;

	[Tooltip("Rotation pivot in Z axis")]
	[Range(-1,1)]
	public float pivotOffsetZ;

	[Tooltip("Frequency of the object's angular movement")]
	public float rotationFrequency=1;

	
	public string waterBodyTag;

	private float waterLevel;
	private Vector3 upliftForce ;
	private Vector3 angularForce;
    

   

    void OnTriggerStay(Collider col)
	{
        if (col.tag != waterBodyTag)
		{
			return;
		}

		try {

            waterLevel = col.gameObject.transform.position.y;

			upliftForce.y = 9.8f*(waterLevel+floatingOffset-transform.position.y);

            
			
			if (gameObject.transform.position.y < waterLevel)
			{
                gameObject.GetComponent<Rigidbody>().AddForce(upliftForce, ForceMode.Acceleration);

			}
			if (gameObject.transform.position.y < waterLevel+floatingOffset)
			{
				gameObject.GetComponent<Rigidbody>().AddForce(upliftForce*upliftForceOffset, ForceMode.Acceleration);
			}


			angularForce.x = -1*rotationFrequency*  transform.rotation.x+pivotOffsetX ;
			angularForce.z = -1*rotationFrequency*  transform.rotation.z+pivotOffsetZ ;

			
			if (gameObject.transform.rotation.x < pivotOffsetX)
			{
				gameObject.GetComponent<Rigidbody>().AddTorque(angularForce, ForceMode.Acceleration);

			}
			if (gameObject.transform.rotation.x > pivotOffsetX)
			{
				gameObject.GetComponent<Rigidbody>().AddTorque(angularForce, ForceMode.Acceleration);
			}
			if (gameObject.transform.rotation.z < pivotOffsetZ)
			{
				gameObject.GetComponent<Rigidbody>().AddTorque(angularForce, ForceMode.Acceleration);

			}
			if (gameObject.transform.rotation.z > pivotOffsetZ)
			{
				gameObject.GetComponent<Rigidbody>().AddTorque(angularForce, ForceMode.Acceleration);
			}

		}

		





		catch(UnityException e){
			Debug.Log(e);
		}

		
	}
}

