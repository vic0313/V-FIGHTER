using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_camera : MonoBehaviour
{
    public Skybox sb;
    public float num;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        num = sb.material.GetFloat("_Rotation");
        sb.material.SetFloat("_Rotation", num + 0.03f);
        
    }
}
