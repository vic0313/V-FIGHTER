using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot_C : MonoBehaviour
{
    float y;
    // Start is called before the first frame update
    void Start()
    {
        y = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        y += 0.5f;
        transform.localEulerAngles = new Vector3(transform.rotation.x, y, transform.rotation.z);
    }
}
