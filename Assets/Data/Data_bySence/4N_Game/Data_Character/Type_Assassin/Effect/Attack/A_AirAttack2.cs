using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_AirAttack2 : MonoBehaviour
{
    public Vector3 moveway;
    public Animator animator;
    public int characterNumber;
    

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += 10f * moveway * Time.deltaTime;
        if (gameObject.transform.position.z >= 5.0f || gameObject.transform.position.z <= -5.0f || gameObject.transform.position.x >= 13f || gameObject.transform.position.x <= -13f || gameObject.transform.position.y <= 0)
        {
            animator.SetBool("OK", true);
        }
    }
    
    void Destroyobj()
    {
        Destroy(this.gameObject);
    }
}
