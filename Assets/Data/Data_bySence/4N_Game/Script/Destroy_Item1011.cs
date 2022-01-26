using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Item1011 : MonoBehaviour
{
    public Collider fire;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        Invoke ("getdown",9.5f);
        Destroy(gameObject, 10f);
    }
    void getdown()
    {
        self.transform.position=new Vector3(self.transform.position.x,-20f, self.transform.position.z);
    }
    
}
