
/**
 * 
 * Author Name: Abu Sabiq Mahdi
 * Date: 30 October 2019
 * Description: This is a script to controll the 2D environment dynamically.
 * 
**/

using UnityEngine;

// [ExecuteInEditMode]
public class DynamicEnvironment : MonoBehaviour {

	[Tooltip("Light source object of sun and moon")]
	public GameObject lightPivot,Sun,Moon,SunHalo,MoonHalo;

	[Tooltip("Rotation speed of fake light source")]
	public float lightSpeed;
	[Space(10)]

	public float maxSunSize;
	public float minSunSize;



	[Header("Environmental Objects")]
	public GameObject FarMountain;
	public GameObject Sky;
	public Material RiverMaterial;
	public GameObject BaseLight;

	[Header("Base light Color")]
	public Color baseDawn;
	public Color baseMidDay;
	public Color baseEvening;
	public Color baseNight;
	[Space(10)]

	[Header("Light Source Color")]
	public Color sunDawn;
	public Color sunMidDay;
	public Color sunEvening;
	public Color MoonNight;
	[Space(10)]

	[Header("Light Halo Color")]
	public Color sunHaloDawn;
	public Color sunHaloMidDay;
	public Color sunHaloEvening;
	public Color MoonHaloNight;
	public Color SunHaloNight;
	public Color MoonHaloDay;
	[Space(10)]

	[Header("Sky Color")]
		[Header("Dawn")]
		public Color topSkyDawn;
		public Color bottomSkyDawn;
		[Header("Mid Day")]
		public Color topSkyMidDay;
		public Color bottomSkyMidDay;
		[Header("Evening")]
		public Color topSkyEvening;
		public Color bottomSkyEvening;
		[Header("Night")]
		public Color topSkyNight;
		public Color bottomSkyNight;
		[Space(20)]

	[Header("Far Mountain Color")]
	[Header("Dawn")]
	public Color MountainTopDawn;
	public Color MountainBottomDawn;
	[Header("Mid Day")]
	public Color MountainTopDay;
	public Color MountainBottomDay;
	[Header("Evening")]
	public Color MountainTopEvening;
	public Color MountainBottomEvening;
	[Header("Night")]
	public Color MountainTopNight;
	public Color MountainBottomNight;
	[Space(20)]

	[Range(0,360)]	
	public float dawnTime;
	[Range(0,360)]	
	public float dayStart;
		[Range(0,360)]	
	public float dayEnd;
	[Range(0,360)]	
	public float eveningTime;
	[Range(0,360)]	
	public float nightStart;
	[Range(0,360)]
	public float nightEnd;

	[Header("River Color")]
		[Header("Dawn")]
		public Color RiverFarDawn;
		public Color RiverNearDawn;
		[Header("Mid Day")]
		public Color RiverFarDay;
		public Color RiverNearDay;
		[Header("Evening")]
		public Color RiverFarEvening;
		public Color RiverNearEvening;
		[Header("Night")]
		public Color RiverFarNight;
		public Color RiverNearNight;
		[Space(20)]

	//PRIVATE VARIABLES
	
	private Color _SunColor;
	private float _time;


	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		
		
		lightPivot.transform.Rotate(0,0,-lightSpeed);

		Sun.GetComponent<SpriteRenderer>().material.color = _SunColor;


		
		processEnvironment();


		print(lightPivot.transform.rotation.eulerAngles.z +"---"+ _time);
	}


	 void processEnvironment()
	{
		
		//START OF THE DAWN TO MID DAY
		if (lightPivot.transform.rotation.eulerAngles.z < dawnTime && lightPivot.transform.rotation.eulerAngles.z > dayStart)
		{
			_time = Mapf.Map(lightPivot.transform.rotation.eulerAngles.z,dawnTime,dayStart,0,1);
			_SunColor = Color.Lerp(sunDawn,sunMidDay,_time);
			SunHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(sunHaloDawn,sunHaloMidDay,_time);
			
			Sky.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(topSkyDawn,topSkyMidDay,_time));
			Sky.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(bottomSkyDawn,bottomSkyMidDay,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(MountainTopDawn,MountainTopDay,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(MountainBottomDawn,MountainBottomDay,_time));
			
			RiverMaterial.SetColor("_BaseColor", Color.Lerp(RiverNearDawn,RiverNearDay,_time));
			RiverMaterial.SetColor("_ReflectionColor", Color.Lerp(RiverFarDawn,RiverFarDay,_time));

			Sun.transform.localScale = Vector3.Lerp(new Vector3(maxSunSize,maxSunSize,maxSunSize),new Vector3(minSunSize,minSunSize,minSunSize),_time);
			// BaseLight.GetComponent<Light>().color = Color.Lerp()
		}

		//START OF MIDDAY TO EVENING
		 if (lightPivot.transform.rotation.eulerAngles.z < dayEnd&& lightPivot.transform.rotation.eulerAngles.z > eveningTime)
		{
			_time = Mapf.Map(lightPivot.transform.rotation.eulerAngles.z,dayEnd,eveningTime,0,1);
			_SunColor = Color.Lerp(sunMidDay,sunEvening,_time);
			SunHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(sunHaloMidDay,sunHaloEvening,_time);
			
			Sky.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(topSkyMidDay,topSkyEvening,_time));
			Sky.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(bottomSkyMidDay,bottomSkyEvening,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(MountainTopDay,MountainTopEvening,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(MountainBottomDay,MountainBottomEvening,_time));

			RiverMaterial.SetColor("_BaseColor", Color.Lerp(RiverNearDay,RiverNearEvening,_time));
			RiverMaterial.SetColor("_ReflectionColor", Color.Lerp(RiverFarDay,RiverFarEvening,_time));
			
			Sun.transform.localScale = Vector3.Lerp(new Vector3(minSunSize,minSunSize,minSunSize),new Vector3(maxSunSize,maxSunSize,maxSunSize),_time);
			
		}

		//START OF EVENING TO NIGHT
		if (lightPivot.transform.rotation.eulerAngles.z < eveningTime && lightPivot.transform.rotation.eulerAngles.z > nightStart)
		{
			_time = Mapf.Map(lightPivot.transform.rotation.eulerAngles.z,eveningTime,nightStart,0,1);
			_SunColor = Color.Lerp(sunEvening,sunDawn,_time);
			SunHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(sunHaloEvening,SunHaloNight,_time);
			MoonHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(MoonHaloDay,MoonHaloNight,_time);
			Sky.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(topSkyEvening,topSkyNight,_time));
			Sky.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(bottomSkyEvening,bottomSkyNight,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(MountainTopEvening,MountainTopNight,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(MountainBottomEvening,MountainBottomNight,_time));

			RiverMaterial.SetColor("_BaseColor", Color.Lerp(RiverNearEvening,RiverNearNight,_time));
			RiverMaterial.SetColor("_ReflectionColor", Color.Lerp(RiverFarEvening,RiverFarNight,_time));
		}

		//START OF NIGHT TO DAWN
		if (lightPivot.transform.rotation.eulerAngles.z < nightEnd && lightPivot.transform.rotation.eulerAngles.z > dawnTime)
		{
			_time = Mapf.Map(lightPivot.transform.rotation.eulerAngles.z,nightEnd,dawnTime,0,1);
			_SunColor = Color.Lerp(sunEvening,sunDawn,_time);
			SunHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(SunHaloNight,sunHaloDawn,_time);
			MoonHalo.GetComponent<SpriteRenderer>().color = Color.Lerp(MoonHaloNight,MoonHaloDay,_time);
			Sky.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(topSkyNight,topSkyDawn,_time));
			Sky.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(bottomSkyNight,bottomSkyDawn,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_TopColor", Color.Lerp(MountainTopNight,MountainTopDawn,_time));
			FarMountain.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", Color.Lerp(MountainBottomNight,MountainBottomDawn,_time));

			RiverMaterial.SetColor("_BaseColor", Color.Lerp(RiverNearNight,RiverNearDawn,_time));
			RiverMaterial.SetColor("_ReflectionColor", Color.Lerp(RiverFarNight,RiverFarDawn,_time));
		}






		

		
	} 
}
