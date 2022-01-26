using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour
{
    float y = 0;
    float x = 0;
    float z = 0;
    // Update is called once per frame
    void Update()
    {
        y += 0.5f;
        x += 0.5f;
        z += 0.5f;
        transform.localEulerAngles = new Vector3(x, y, z);
    }
}
