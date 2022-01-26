using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AttackFront : MonoBehaviour
{
    public Vector3 moveway;
    public GameObject hiteffect;
    public int characterNumber;
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += 10f*moveway * Time.deltaTime;
        if (gameObject.transform.position.z >= 5.0f|| gameObject.transform.position.z <= -5.0f|| gameObject.transform.position.x >= 13f|| gameObject.transform.position.x <= -13f || gameObject.transform.position.y<=0)
        {
            Instantiate(hiteffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
