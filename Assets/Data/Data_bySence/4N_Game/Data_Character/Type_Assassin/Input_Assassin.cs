using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Assassin : MonoBehaviour
{
    public CharacterInput CI;
    public CharacterData Data;
    public Animator animator;
    public List<GameObject> AirAttack2obj;
    public List<GameObject> GroundAttack2Defenseobj;
    public List<GameObject> GroundAttack2;
    public List<A_GA2> GroundAttack2_script;
    public enum C_Input
    {
        Vertical = 0,
        Horizontal,
        Attack1,
        Attack2,
        Defense,
        Jump,
        Sprint,
        Defenseing,
    }
    private void Update()
    {
        for (int i = 0; i < AirAttack2obj.Count; i++)
        {
            if (AirAttack2obj[i] == null)
            {
                AirAttack2obj.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < GroundAttack2Defenseobj.Count; i++)
        {
            if (GroundAttack2Defenseobj[i] == null)
            {
                GroundAttack2Defenseobj.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < GroundAttack2.Count; i++)
        {
            if (animator.GetBool("BeAttack"))
            {
                if (GroundAttack2[i] != null)
                {
                    Destroy(GroundAttack2[i]);
                }
                GroundAttack2.RemoveAt(i);
                i--;
            }
            else
            {
                if (GroundAttack2[i] == null)
                {
                    GroundAttack2.RemoveAt(i);
                    i--;
                }
            }
        }
        for (int i = 0; i < GroundAttack2_script.Count; i++)
        {
            if (GroundAttack2_script[i] == null)
            {
                GroundAttack2_script.RemoveAt(i);
                i--;
            }
            else
            {
                GroundAttack2_script[i].center = Data.Effectcenter.transform;
            }
        }
        
    }
    public void GroundAttack2_Start()
    {
        if (GroundAttack2.Count <= 3)
        {
           
            Vector3 pos = Data.Effectcenter.transform.position;
            pos.z += 0.5f;
            GroundAttack2.Add(Instantiate(Data.ccall.Profession_effect[6], pos, Quaternion.identity));
            GroundAttack2_script.Add(GroundAttack2[GroundAttack2.Count - 1].GetComponent<A_GA2>());
            GroundAttack2_script[GroundAttack2_script.Count - 1].characterNumber = Data.characterNumber;
            GroundAttack2_script[GroundAttack2_script.Count - 1].center = Data.Effectcenter.transform;
            GroundAttack2_script[GroundAttack2_script.Count - 1].rangle = 0;
            pos = Data.Effectcenter.transform.position;
            pos.x += 0.5f;
            GroundAttack2.Add(Instantiate(Data.ccall.Profession_effect[6], pos, Quaternion.identity));
            GroundAttack2_script.Add(GroundAttack2[GroundAttack2.Count - 1].GetComponent<A_GA2>());
            GroundAttack2_script[GroundAttack2_script.Count - 1].characterNumber = Data.characterNumber;
            GroundAttack2_script[GroundAttack2_script.Count - 1].center = Data.Effectcenter.transform;
            GroundAttack2_script[GroundAttack2_script.Count - 1].rangle = 90;
            pos = Data.Effectcenter.transform.position;
            pos.z -= 0.5f;
            GroundAttack2.Add(Instantiate(Data.ccall.Profession_effect[6], pos, Quaternion.identity));
            GroundAttack2_script.Add(GroundAttack2[GroundAttack2.Count - 1].GetComponent<A_GA2>());
            GroundAttack2_script[GroundAttack2_script.Count - 1].characterNumber = Data.characterNumber;
            GroundAttack2_script[GroundAttack2_script.Count - 1].center = Data.Effectcenter.transform;
            GroundAttack2_script[GroundAttack2_script.Count - 1].rangle = 180;
            pos = Data.Effectcenter.transform.position;
            pos.x -= 0.5f;
            GroundAttack2.Add(Instantiate(Data.ccall.Profession_effect[6], pos, Quaternion.identity));
            GroundAttack2_script.Add(GroundAttack2[GroundAttack2.Count - 1].GetComponent<A_GA2>());
            GroundAttack2_script[GroundAttack2_script.Count - 1].characterNumber = Data.characterNumber;
            GroundAttack2_script[GroundAttack2_script.Count - 1].center = Data.Effectcenter.transform;
            GroundAttack2_script[GroundAttack2_script.Count - 1].rangle = -90;
        }
    }
    public void GroundAttack2Effect_Start()
    {
        Instantiate(Data.ccall.Profession_effect[5], this.transform.position, Quaternion.identity);
    }
    public void AttackBron(string animatinName)
    {
        switch (animatinName)
        {
            case "GroundAttack_Defense":
                GameObject gad= Instantiate(Data.ccall.Profession_effect[4], this.transform.position, Quaternion.identity);
                gad.GetComponent<A_GD>().characterNumber = Data.characterNumber;
                break;
            case "AirAttack2_start":
                if (AirAttack2obj.Count <= 2)
                {
                    AirAttack2obj.Add(Instantiate(Data.ccall.Profession_effect[3], Data.ccall.Profession_effect[8].transform.position, Data.ccall.Profession_effect[8].transform.rotation));
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().characterNumber = Data.characterNumber;
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().moveway = new Vector3(Data.AttackWay.x,-0.7f, Data.AttackWay.z);
                    float rot = Mathf.Atan(Data.AttackWay.x / Data.AttackWay.z) / (Mathf.PI / 180);
                    rot = Data.AttackWay.z < 0 ? rot + 180 : rot;
                    AirAttack2obj.Add(Instantiate(Data.ccall.Profession_effect[3], Data.ccall.Profession_effect[9].transform.position, Data.ccall.Profession_effect[9].transform.rotation));
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().characterNumber = Data.characterNumber;
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().moveway = new Vector3(Mathf.Sin((rot + 33f) * (Mathf.PI / 180)), -0.7f, Mathf.Cos((rot + 33f) * (Mathf.PI / 180)));
                  
                    AirAttack2obj.Add(Instantiate(Data.ccall.Profession_effect[3], Data.ccall.Profession_effect[10].transform.position, Data.ccall.Profession_effect[10].transform.rotation));
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().characterNumber = Data.characterNumber;
                    AirAttack2obj[AirAttack2obj.Count - 1].GetComponent<A_AirAttack2>().moveway = new Vector3(Mathf.Sin((rot -33f) * (Mathf.PI / 180)), -0.7f, Mathf.Cos((rot -33f) * (Mathf.PI / 180)));

                }
                break;
            case "GroundAttack2_Defense":
                if (GroundAttack2Defenseobj.Count <= 2)
                {
                    GroundAttack2Defenseobj.Add(Instantiate(Data.ccall.Profession_effect[7], this.transform.position, this.transform.rotation));
                    GroundAttack2Defenseobj[GroundAttack2Defenseobj.Count - 1].GetComponent<A_GroundAttack2Defense>().characterNumber = Data.characterNumber;
                }
                break;
        }
    }
    public void Sprint_botan()
    {
        if (Data.character_NowMP >= Data.MP_assassin[7] * Data.character_MaxMP)
        {
            if (CI.playercheck)
            {
                //CanSwitch状況ならば、第一押したボタンだけで判定
                if (animator.GetBool("CanSwitch") && CI.chackOK == false)
                {
                    if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Sprint]) && animator.GetBool("InAir") == false)
                    {
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
                else if (animator.GetBool("CanSwitch") == false)
                {
                    //CanInput状況ならば、スイッチできるまで何回でも入力できる、但し最後の入力ボタンを判定する
                    if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Sprint]) && animator.GetBool("InAir") == false)
                    {
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
            }
            else
            {
                //CanSwitch状況ならば、第一押したボタンだけで判定
                if (animator.GetBool("CanSwitch") && CI.chackOK == false)
                {
                    if (CI.Cpu_Input[(int)C_Input.Sprint] && animator.GetBool("InAir") == false)
                    {
                        CI.Cpu_Input[(int)C_Input.Sprint] = false;
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
                else if (animator.GetBool("CanSwitch") == false)
                {
                    //CanInput状況ならば、スイッチできるまで何回でも入力できる、但し最後の入力ボタンを判定する
                    if (CI.Cpu_Input[(int)C_Input.Sprint] && animator.GetBool("InAir") == false)
                    {
                        CI.Cpu_Input[(int)C_Input.Sprint] = false;
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
            }
        }
    }
    public void Attack1_botan()
    {
        //CanSwitch状況ならば、第一押したボタンだけで判定
        if ((animator.GetBool("CanSwitch") && CI.chackOK == false) || animator.GetBool("CanSwitch") == false)
        {
            if ((Input.GetButtonDown(CI.player_Input[(int)C_Input.Attack1])) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack1]))
            {
                CI.Cpu_Input[(int)C_Input.Attack1] = false;
                if (animator.GetBool("InAir") && animator.GetBool("PickUp") == false)
                {
                    if(Data.character_NowMP >= Data.MP_assassin[5] * Data.character_MaxMP)
                    {
                        CI.chackOK = true;
                        CI.animation_next = "AirAttack";
                    }
                }
                else if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetBool("PickUp") == false)
                    {
                        if (animator.GetBool("Defense") && animator.GetBool("Inputcheck") == false)
                        {
                            if (Data.character_NowMP >= Data.MP_assassin[0] * Data.character_MaxMP)
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack_Defense";
                            }
                        }
                        else if (animator.GetBool("Inputcheck"))
                        {
                            if (Data.character_NowMP >= Data.MP_assassin[1] * Data.character_MaxMP && (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)") == false && animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(2)") == false))
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack_Front";
                            }
                            else
                            {
                                CI.chackOK = true;
                               CI.animation_next = "GroundAttack(1)";
                                
                            }
                        }
                        else if (animator.GetBool("Defense") == false && animator.GetBool("Inputcheck") == false)
                        {
                            if (Data.pickrangein == false)
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack(1)";
                            }
                        }
                    }
                    else if (animator.GetBool("PickUp") == true && animator.GetBool("Inputcheck") && animator.GetBool("Eating") == false)
                    {
                        CI.Throw();
                        animator.Play("Throw");
                        Data.pickup_already = 3;
                    }
                }
            }
        }
        //
        if (animator.GetBool("PickUp") == true && Data.pickup_already == 2)
        {
            if (Data.Pickup_item_Script.item_script.Item_num <= 4)
            {
                if (((animator.GetBool("CanSwitch") && CI.chackOK == false) || animator.GetBool("CanSwitch") == false)
                && animator.GetBool("Inputcheck") == false)
                {
                    if ((Input.GetButton(CI.player_Input[(int)C_Input.Attack1])) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack1]))
                    {

                        CI.Cpu_Input[(int)C_Input.Attack1] = false;
                        animator.SetBool("Eating", true);
                    }
                    else if ((Input.GetButton(CI.player_Input[(int)C_Input.Attack1])) == false && (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack1]) == false)
                    {
                        CI.Cpu_Input[(int)C_Input.Attack1] = false;
                        animator.SetBool("Eating", false);
                    }
                }
                else
                {
                    animator.SetBool("Eating", false);
                }
            }
            else if (Data.Pickup_item_Script.item_script.Item_num == 6)
            {
                if (((animator.GetBool("CanSwitch") && CI.chackOK == false) || animator.GetBool("CanSwitch") == false)
                && animator.GetBool("Inputcheck") == false)
                {
                    if ((Input.GetButtonDown(CI.player_Input[(int)C_Input.Attack1])) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack1]))
                    {
                        CI.Cpu_Input[(int)C_Input.Attack1] = false;
                        animator.SetTrigger("PickUp_A");
                    }
                }
            }
            else if (Data.Pickup_item_Script.item_script.Item_num == 7)
            {
                if (((animator.GetBool("CanSwitch") && CI.chackOK == false) || animator.GetBool("CanSwitch") == false)
                && animator.GetBool("Inputcheck") == false)
                {
                    if ((Input.GetButtonDown(CI.player_Input[(int)C_Input.Attack1])) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack1]))
                    {
                        CI.Cpu_Input[(int)C_Input.Attack1] = false;
                        animator.SetTrigger("PickUp_B");
                    }
                }
            }
        }
    }
    public void Attack2_botan()
    {
        if ((animator.GetBool("CanSwitch") && CI.chackOK == false || animator.GetBool("CanSwitch") == false) && animator.GetBool("PickUp") == false)
        {
            if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Attack2]) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Attack2]))
            {
                CI.Cpu_Input[(int)C_Input.Attack2] = false;
                if (animator.GetBool("InAir") && animator.GetBool("PickUp") == false)
                {
                    if (Data.character_NowMP >= Data.MP_assassin[6] * Data.character_MaxMP)
                    {
                        CI.animation_next = "AirAttack2_start";
                        CI.chackOK = true;
                    }
                }
                else if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetBool("Defense") && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_assassin[4] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Defense";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Inputcheck"))
                    {
                        if (Data.character_NowMP >= Data.MP_assassin[3] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Front";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Defense") == false && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_assassin[6] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2";
                            CI.chackOK = true;
                        }

                    }
                }
            }
        }
    }
    public void Jump_botan()
    {
        if (animator.GetBool("BeAttack") == false)
        {
            if ((animator.GetBool("CanSwitch") && CI.chackOK == false) || animator.GetBool("CanSwitch") == false)
            {
                if (animator.GetBool("InAir") == false && animator.GetCurrentAnimatorStateInfo(0).IsName("Defense") == false && (Input.GetButtonDown(CI.player_Input[(int)C_Input.Jump]) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Jump])))
                {
                    CI.Cpu_Input[(int)C_Input.Jump] = false;
                    CI.chackOK = true;
                    CI.animation_next = "Jump_Start";
                }else if(animator.GetBool("InAir") && Data.JumpTwo ==false&& (Input.GetButtonDown(CI.player_Input[(int)C_Input.Jump]) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Jump])))
                {
                    CI.Cpu_Input[(int)C_Input.Jump] = false;
                    CI.chackOK = true;
                    CI.animation_next = "Jump_Two";
                }
            }
        }
        else
        {
            if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Jump]) || (CI.playercheck == false && CI.Cpu_Input[(int)C_Input.Jump]))
            {
                CI.Cpu_Input[(int)C_Input.Jump] = false;
                if (Data.character_input_H == 0 && Data.character_input_V == 0)
                {
                    Data.character_angle = transform.eulerAngles.y;
                    Data.character_angle = (Data.character_angle >= 0) ? Data.character_angle - 180 : Data.character_angle + 180;
                }
                else
                {
                    Data.character_angle = Mathf.Atan(Data.character_input_H / Data.character_input_V) / (Mathf.PI / 180);
                    Data.character_angle = Data.character_input_V < 0 ? Data.character_angle + 180 : Data.character_angle;
                    if (Data.character_angle >= 0)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle - 180), transform.eulerAngles.z);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle + 180), transform.eulerAngles.z);
                    }
                }
                Data.character_XZmove = new Vector3(Mathf.Sin(Data.character_angle / (180 / Mathf.PI)), 0, Mathf.Cos(Data.character_angle / (180 / Mathf.PI)));
                animator.Play("WakeUp_speed", 0, 0);
            }
        }
    }
    public void A_GA2_S()
    {
        Data.audiosource.PlayOneShot(Data.GC.effect[13]);
    }
}
