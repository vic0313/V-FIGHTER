using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GA2D : MonoBehaviour
{
    public Animator animator;
    public int characterNumber;
    public Vector3 moveway;
    public AudioClip[] effect;
    public AudioSource audiosource;
    public bool HitWall;
    private void Start()
    {
        audiosource.PlayOneShot(effect[0]);
        HitWall = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(animator.GetBool("OK")==false)gameObject.transform.position += 5f * moveway * Time.deltaTime;
        if (gameObject.transform.position.z >= 5.0f || gameObject.transform.position.z <= -5.0f || gameObject.transform.position.x >= 13f || gameObject.transform.position.x <= -13f || gameObject.transform.position.y <= 0)
        {
            animator.SetBool("OK", true);
            if(HitWall==false)
            {
                HitWall = true;
                Bonbsound();
            }
        }
    }
    void Destroyobj()
    {
        Destroy(this.gameObject);
    }
   
    void Bonbsound()
    {
        audiosource.PlayOneShot(effect[1]);
    }
}
