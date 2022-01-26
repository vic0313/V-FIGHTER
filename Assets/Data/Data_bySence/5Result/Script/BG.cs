using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    public float speed = 0.5f;
    private float turn;
    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        turn += (Time.deltaTime * speed) / 10f;
        material.SetTextureOffset("_MainTex", new Vector3(turn, turn));
    }
}
