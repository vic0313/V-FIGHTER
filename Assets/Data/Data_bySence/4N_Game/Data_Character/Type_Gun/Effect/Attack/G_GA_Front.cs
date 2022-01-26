using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GA_Front : MonoBehaviour
{
    public Vector3 moveway;
    public GameObject hiteffect;
    public int characterNumber;
    public AudioClip[] effect;
    public AudioSource audiosource;
    private void Start()
    {
        audiosource.PlayOneShot(effect[0]);
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += 10f * moveway * Time.deltaTime;
        if (gameObject.transform.position.z >= 5.0f || gameObject.transform.position.z <= -5.0f || gameObject.transform.position.x >= 13f || gameObject.transform.position.x <= -13f || gameObject.transform.position.y <= 0)
        {
            DestroyHit();
        }
    }
    public void DestroyHit()
    {
        Instantiate(hiteffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
