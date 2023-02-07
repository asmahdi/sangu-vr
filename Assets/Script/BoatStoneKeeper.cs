using UnityEngine;


public class BoatStoneKeeper : MonoBehaviour
{

    [SerializeField]
    private string[] targetTags;
    [SerializeField]
    private Transform[] targetsTransform;
    [SerializeField]
    GameObject haloStone;
    private int i;




    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        for (i = 0; i < targetsTransform.Length; i++)
        {
            if (other.gameObject.tag == targetTags[i])
            {
                other.gameObject.transform.SetParent(null);
                other.gameObject.transform.position = targetsTransform[i].position;
                if (haloStone)
                {
                    haloStone.SetActive(false);
                }
            }
        }
    }

}
