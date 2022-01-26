using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int characterNumber;
    public AudioClip sound;
    public AudioSource audiosource;

    void Destroyobj()
    {
        Destroy(this.gameObject);
    }
    void Playsound()
    {
        audiosource.PlayOneShot(sound);
    }
}
