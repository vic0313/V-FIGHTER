using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GA : MonoBehaviour
{
    public Animator amr;
    public int characterNumber;
    public GameObject effect;
    public AudioClip sound;
    public AudioSource audiosource;
    void Start()
    {
        Invoke("DestroyObj", 10f);
    }


    void DestroyObj()
    {
        Instantiate(effect, this.transform.position, Quaternion.identity);
        audiosource.PlayOneShot(sound);
        Destroy(this.gameObject);
    }
}
