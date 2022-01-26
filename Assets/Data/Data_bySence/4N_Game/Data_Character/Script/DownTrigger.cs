using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrigger : MonoBehaviour
{
    public Animator self_anim;
    public CharacterData Data;
    
    void OnTriggerEnter(Collider top)
    {
        bool check = false;
        if (top.transform.tag == "Top" && top.transform.parent.transform.parent.GetComponent<Animator>().GetBool("Dead") == false)
        {
            if (Data.character_Profession == 1)
            {
                if(Data.ccall.animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack") !=true)
                {
                    check = true;
                }
            }else
            {
                check = true;
            }
        }
        //if (down.transform.parent.transform.parent.tag != self.tag)
        {
            if (check)
            {
                if (self_anim.GetBool("BeAttack") == false)
                {
                    Data.character_Ymove.y = 5.0f;
                    self_anim.SetBool("InAir", true);
                    self_anim.Play("Jump_Start");
                    Data.character_Ymove.y = 5.0f;
                    if (Data.GC.GI.Gamemode_choice == 2 && Data.GC.GameSet == 0)
                    {
                        if( (Data.characterTeam==0||top.transform.parent.transform.parent.GetComponent<CharacterData>().characterTeam!=Data.characterTeam)) Data.Character_Point++;
                        if (Data.GC.GameSet_Already[1] == true)
                        {
                            if(Data.characterTeam==0)
                            {
                                foreach(var tag in Data.GC.setCharacter_point)
                                {
                                    if(tag==Data.tag)
                                    {
                                        Data.GC.GameSet = 3;
                                        Data.GC.GAMESTART[0].GetComponent<Camera_Game>().gameset_target[0] = Data.gameObject;
                                        Data.GC.GAMESTART[0].GetComponent<Camera_Game>().gameset_target[1] = top.transform.parent.transform.parent.gameObject;
                                        Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if((Data.GC.Team_Point[Data.characterTeam-1]+1)>= Data.GC.GamePointMax)
                                {
                                    Data.GC.GameSet = 3;
                                    Data.GC.GAMESTART[0].GetComponent<Camera_Game>().gameset_target[0] = Data.gameObject;
                                    Data.GC.GAMESTART[0].GetComponent<Camera_Game>().gameset_target[1] = top.transform.parent.transform.parent.gameObject;
                                    Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    self_anim.SetBool("InAir", true);
                    Data.character_speed = 1.0f;
                    Data.character_Ymove.y = 4.0f;
                    if (Data.character_NowHP > 0) self_anim.Play("BeAttack(Knock)_start");
                    if (Data.character_XZmove.z == 0 && Data.character_XZmove.x == 0)
                    {
                        Data.character_XZmove.x += Random.Range((-0.1f), (1.0f));
                        Data.character_XZmove.z += Random.Range((-0.1f), (1.0f));
                    }
                }
                Data.audiosource.PlayOneShot(Data.GC.effect[3]);
                Instantiate(Data.Character_Effect_Prefab[3], gameObject.transform.position, Quaternion.identity);
            }
        }
    }
    void OnTriggerStay(Collider top)
    {
        bool check = false;
        if (top.transform.tag == "Top" && top.transform.parent.transform.parent.GetComponent<Animator>().GetBool("Dead") == false)
        {
            check = true;

        }

        //if (down.transform.parent.transform.parent.tag != self.tag)
        {
            if (check)
            {
                if (self_anim.GetBool("BeAttack") == false)
                {
                    Data.character_Ymove.y = 5.0f;
                    self_anim.SetBool("InAir", true);
                    self_anim.Play("Jump_Start");
                    Data.character_Ymove.y = 5.0f;
                }
                else
                {
                    self_anim.SetBool("InAir", true);
                    Data.character_speed = 1.0f;
                    Data.character_Ymove.y = 2.0f;
                    if (Data.character_NowHP > 0) self_anim.Play("BeAttack(Knock)_start");

                }

            }
        }
    }
}
