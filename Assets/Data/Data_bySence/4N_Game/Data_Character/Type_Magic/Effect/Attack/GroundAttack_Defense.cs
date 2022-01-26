using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttack_Defense : MonoBehaviour
{
    public GameObject breakeffect;
    public Vector3 Attackway;
    public int characterNumber;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
        Invoke("Breakthis", 4.9f);
    }

    // Update is called once per frame
    void Breakthis()
    {
        Instantiate(breakeffect, this.transform.position, Quaternion.identity);
    }
}
