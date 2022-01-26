using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GD : MonoBehaviour
{
    public int characterNumber;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

}
