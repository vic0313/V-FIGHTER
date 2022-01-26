using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTrigger : MonoBehaviour
{
    public GameObject self;
    public Animator animator;
    public CharacterData Data;
   
    void OnTriggerEnter(Collider down)
    {
        bool check = false;
        if (down.transform.tag == "Down" && animator.GetBool("Invincible") == false && animator.GetCurrentAnimatorStateInfo(0).IsName("Defense")==false && down.transform.parent.transform.parent.GetComponent<Animator>().GetBool("BeAttack") == false
            && (down.transform.parent.transform.parent.GetComponent<CharacterData>().characterTeam != Data.characterTeam || down.transform.parent.transform.parent.GetComponent<CharacterData>().characterTeam == 0))
        {   
            check = true;
        }
        if (check)
        {
            Vector3 a = down.transform.position;
            Vector3 b = self.transform.position;
            float c = Mathf.Abs(a.y - b.y);
            a.y = 0;
            b.y = 0;
            if ((animator.GetBool("BeAttack")==false|| animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)"))&& (a - b).magnitude <= 0.6f&& c>=0.7f)
            {
                Data.character_XZmove = self.transform.position - (down.transform.parent.position);
                Data.character_XZmove.y = 0f;
                Data.character_XZmove.Normalize();
                Data.character_speed = 2.0f;
                if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
            }
        }
    }
}
