using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_GroundAttack2Defense : MonoBehaviour
{
    public Animator amr;
    public int characterNumber;
    public GameObject effect;
    public AudioClip sound;
    public AudioSource audiosource;
    private void Start()
    {
        Vector3 pos = this.transform.position;
        pos.y += 0.5f;
        Instantiate(effect, pos, Quaternion.identity);
        Invoke("DestroyOBJ",10f);
    }
    private void Update()
    {
        if(amr.GetBool("OK"))
        {
            CancelInvoke("DestroyOBJ");
        }
    }
    void OnTriggerEnter(Collider Attackbox)
    {
        if(Attackbox.tag != "Down" && Attackbox.tag != "Top")
        {
            if (Attackbox.transform.parent.transform.parent != null)
            {
                if (Attackbox.transform.parent.transform.parent.tag != "Item_CanPick" && Attackbox.transform.parent.transform.parent.tag != "Item_CantPick")
                {
                    if (Attackbox.transform.parent.tag != "AB_Gun")
                    {
                        amr.SetBool("OK", true);
                        Destroy(this.gameObject, 1f);
                        if(amr.GetBool("OK")==false)audiosource.PlayOneShot(sound);
                    }
                    else
                    {
                        if (Attackbox.tag != "GroundAttack(1)" && Attackbox.tag != "GroundAttack(3)")
                        {
                            amr.SetBool("OK", true);
                            Destroy(this.gameObject, 1f);
                            if (amr.GetBool("OK") == false) audiosource.PlayOneShot(sound);
                        }
                    }
                }
            }
            else
            {
                if (Attackbox.transform.parent.tag == "AB_Gun")
                {
                    if (Attackbox.tag != "GroundAttack(1)" && Attackbox.tag != "GroundAttack(3)")
                    {
                        amr.SetBool("OK", true);
                        Destroy(this.gameObject, 1f);
                        if (amr.GetBool("OK") == false) audiosource.PlayOneShot(sound);
                    }
                }
                else if (Attackbox.transform.parent.tag == "AB_Magic")
                {
                    amr.SetBool("OK", true);
                    Destroy(this.gameObject, 1f);
                    if (amr.GetBool("OK") == false) audiosource.PlayOneShot(sound);
                }
                else if (Attackbox.transform.parent.tag == "AB_Assassin")
                {
                    amr.SetBool("OK", true);
                    Destroy(this.gameObject, 1f);
                    if (amr.GetBool("OK") == false) audiosource.PlayOneShot(sound);
                }
            }
        }
        
                
    }
   void DestroyOBJ()
    {
        Vector3 pos = this.transform.position;
        pos.y += 0.5f;
        Instantiate(effect, pos, Quaternion.identity);
        Destroy(this.gameObject);
    }
    
}
