using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GA2 : MonoBehaviour
{
    public int characterNumber;
    public float rangle;
    public Transform center;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }
    private void Update()
    {
        rangle += 0.5f;
        transform.position = center.position + new Vector3(Mathf.Sin(rangle*(Mathf.PI/180)),0f, Mathf.Cos(rangle * (Mathf.PI / 180)) )* 0.5f;
    }
}
