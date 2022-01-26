using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_camera : MonoBehaviour
{
    public Skybox sb;
    float x ;
    float z;
    
    // Start is called before the first frame update
    void Start()
    {
        x = transform.rotation.x;
        z= transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        x -= 0.005f;
        z -= 0.005f;
        gameObject.transform.localEulerAngles = new Vector3(x, gameObject.transform.rotation.y, z);
        
    }
    
}
