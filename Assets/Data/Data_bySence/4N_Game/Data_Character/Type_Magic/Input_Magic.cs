using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Magic : MonoBehaviour
{
    public CharacterInput CI;
    public CharacterData Data;
    public Animator animator;
    public bool GroundAttack_front_Start;
    public List<GameObject> GroundAttack_front_three;
    public GameObject GroundAttack_front_broneffect;
    public bool GroundAttack_Defense_start;
    public GameObject GroundAttack_Defense;
    private List<GameObject> AirAttack2;
    public Vector3 attackway;
    
    private void Start()
    {
        GroundAttack_front_Start = false;
        GroundAttack_front_three=new List<GameObject>();
        GroundAttack_Defense_start = false;
        AirAttack2 = new List<GameObject>();
    }
    private void Update()
    {
        for(int i=0;i< GroundAttack_front_three.Count;i++)
        {
            if(GroundAttack_front_three[i]==null)
            {
                GroundAttack_front_three.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < AirAttack2.Count; i++)
        {
            if (AirAttack2[i] == null)
            {
                AirAttack2.RemoveAt(i);
                i--;
            }
        }
    }
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
    public void AttackBron(string animatinName)
    {
        switch(animatinName)
        {
            case "GroundAttack_Front":
                if (GroundAttack_front_Start == false)
                {
                    Invoke("GroundAttack_front_born_Time", 0.25f);
                    GroundAttack_front_Start = true;
                }
                break;
            case "GroundAttack_Defense":
                attackway = Data.AttackWay;
                Invoke("GroundAttack_Defense_born_Time", 0.8f);
                GroundAttack_Defense_start = true;
                break;
            case "AirAttack2_start":
                if(AirAttack2.Count<1)
                {
                    float ROT = Mathf.Atan(Data.AttackWay.x / Data.AttackWay.z) / (Mathf.PI / 180);
                    ROT = Data.AttackWay.z < 0 ? ROT + 180 : ROT;
                    Vector3 bullet2 = Data.AttackWay;
                    bullet2.y = -1f;
                    AirAttack2.Add(Instantiate(Data.ccall.Profession_effect[7], gameObject.transform.position, Quaternion.identity));
                    AirAttack2[AirAttack2.Count-1].GetComponent<M_AttackFront>().moveway = bullet2;
                    AirAttack2[AirAttack2.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
                     bullet2 = new Vector3(Mathf.Sin((ROT+120f) * (Mathf.PI / 180)), -1, Mathf.Cos((ROT + 120f) * (Mathf.PI / 180)));
                    AirAttack2.Add(Instantiate(Data.ccall.Profession_effect[7], gameObject.transform.position, Quaternion.identity));
                    AirAttack2[AirAttack2.Count - 1].GetComponent<M_AttackFront>().moveway = bullet2;
                    AirAttack2[AirAttack2.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
                    bullet2 = new Vector3(Mathf.Sin((ROT - 120f) * (Mathf.PI / 180)), -1, Mathf.Cos((ROT - 120f) * (Mathf.PI / 180)));
                    AirAttack2.Add(Instantiate(Data.ccall.Profession_effect[7], gameObject.transform.position, Quaternion.identity));
                    AirAttack2[AirAttack2.Count - 1].GetComponent<M_AttackFront>().moveway = bullet2;
                    AirAttack2[AirAttack2.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
                    Instantiate(GroundAttack_front_broneffect, gameObject.transform.position, Quaternion.identity);
                }
                break;
        }
    }
    public void GroundAttack_Defense_born_Time()
    {
        if(GroundAttack_Defense!=null)
        {
            Instantiate(GroundAttack_Defense.GetComponent<GroundAttack_Defense>().breakeffect, GroundAttack_Defense.transform.position, Quaternion.identity);
            Destroy(GroundAttack_Defense);
            GroundAttack_Defense = Instantiate(Data.ccall.Profession_effect[5], Data.ccall.Profession_effect[4].transform.position, Data.ccall.Profession_effect[4].transform.rotation);
        }else
        {
            GroundAttack_Defense = Instantiate(Data.ccall.Profession_effect[5], Data.ccall.Profession_effect[4].transform.position, Data.ccall.Profession_effect[4].transform.rotation);
        }
        GroundAttack_Defense.GetComponent<GroundAttack_Defense>().Attackway = attackway;
        GroundAttack_Defense.GetComponent<GroundAttack_Defense>().characterNumber = Data.characterNumber;
        GroundAttack_Defense_start = false;
    }
    public void GroundAttack_Defense_Stop()
    {
        CancelInvoke("GroundAttack_Defense_born_Time");
        GroundAttack_Defense_start = false;
    }
    public void GroundAttack_front_stop()
    {
        GroundAttack_front_Start = false;
        CancelInvoke("GroundAttack_front_born_Time");
    }
    public void GroundAttack_front_born_Time()
    {
        GroundAttack_front_Start = false;
        if(GroundAttack_front_three.Count<=3)
        {
            float ROT = Mathf.Atan(Data.AttackWay.x / Data.AttackWay.z) / (Mathf.PI / 180);
            ROT = Data.AttackWay.z < 0 ? ROT + 180 : ROT;
            //1
            GroundAttack_front_three.Add(Instantiate(Data.ccall.Profession_effect[0], Data.ccall.Profession_effect[1].transform.position, Data.ccall.Profession_effect[1].transform.rotation));
            GroundAttack_front_three[(GroundAttack_front_three.Count - 1)].GetComponent<M_AttackFront>().moveway = Data.AttackWay;
            GroundAttack_front_three[GroundAttack_front_three.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
            //2
             Vector3 bullet2 = new Vector3(Mathf.Sin((ROT + 20f) * (Mathf.PI / 180)), 0, Mathf.Cos((ROT + 20f) * (Mathf.PI / 180)));
            GroundAttack_front_three.Add(Instantiate(Data.ccall.Profession_effect[0], Data.ccall.Profession_effect[2].transform.position, Data.ccall.Profession_effect[2].transform.rotation));
            GroundAttack_front_three[(GroundAttack_front_three.Count - 1)].GetComponent<M_AttackFront>().moveway = bullet2;
            GroundAttack_front_three[GroundAttack_front_three.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
            //3
            GroundAttack_front_three.Add(Instantiate(Data.ccall.Profession_effect[0], Data.ccall.Profession_effect[3].transform.position, Data.ccall.Profession_effect[3].transform.rotation));
            bullet2 = new Vector3(Mathf.Sin((ROT - 20f) * (Mathf.PI / 180)), 0, Mathf.Cos((ROT - 20f) * (Mathf.PI / 180)));
            GroundAttack_front_three[(GroundAttack_front_three.Count - 1)].GetComponent<M_AttackFront>().moveway = bullet2;
            GroundAttack_front_three[GroundAttack_front_three.Count - 1].GetComponent<M_AttackFront>().characterNumber = Data.characterNumber;
            Instantiate(GroundAttack_front_broneffect, Data.ccall.Profession_effect[1].transform.position, Data.ccall.Profession_effect[1].transform.rotation);
        }
    }
    public void Sprint_botan()
    {
        if (Data.character_NowMP >= Data.MP_magic[7] * Data.character_MaxMP)
        {
            if (CI.playercheck)
            {
                //CanSwitch状況ならば、第一押したボタンだけで判定
                if (animator.GetBool("CanSwitch") && CI.chackOK == false)
                {
                    if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Sprint]) )
                    {
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
                else if (animator.GetBool("CanSwitch") == false)
                {
                    //CanInput状況ならば、スイッチできるまで何回でも入力できる、但し最後の入力ボタンを判定する
                    if (Input.GetButtonDown(CI.player_Input[(int)C_Input.Sprint]) )
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
                    if (CI.Cpu_Input[(int)C_Input.Sprint] )
                    {
                        CI.Cpu_Input[(int)C_Input.Sprint] = false;
                        CI.chackOK = true;
                        CI.animation_next = "Sprint";
                    }
                }
                else if (animator.GetBool("CanSwitch") == false)
                {
                    //CanInput状況ならば、スイッチできるまで何回でも入力できる、但し最後の入力ボタンを判定する
                    if (CI.Cpu_Input[(int)C_Input.Sprint] )
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
                    if (Data.character_NowMP >= Data.MP_magic[5] * Data.character_MaxMP)
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
                            if (Data.character_NowMP >= Data.MP_magic[0] * Data.character_MaxMP)
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack_Defense";
                            }
                        }
                        else if (animator.GetBool("Inputcheck"))
                        {
                            if (Data.character_NowMP >= Data.MP_magic[1] * Data.character_MaxMP )
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack_Front";
                            }
                            else
                            {
                                CI.chackOK = true;
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)"))
                                {
                                    CI.animation_next = "GroundAttack(2)";
                                }
                                else
                                {
                                    CI.animation_next = "GroundAttack(1)";
                                }
                            }
                        }
                        else if (animator.GetBool("Defense") == false && animator.GetBool("Inputcheck") == false)
                        {
                            if (Data.pickrangein == false)
                            {
                                CI.chackOK = true;
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)"))
                                {
                                    CI.animation_next = "GroundAttack(2)";
                                }
                                else
                                {
                                    CI.animation_next = "GroundAttack(1)";
                                }
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
                    if (Data.character_NowMP >= Data.MP_magic[6] * Data.character_MaxMP)
                    {
                        CI.animation_next = "AirAttack2_start";
                        CI.chackOK = true;
                    }
                }
                else if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetBool("Defense") && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_magic[4] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Defense";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Inputcheck"))
                    {
                        if (Data.character_NowMP >= Data.MP_magic[3] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Front";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Defense") == false && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_magic[6] * Data.character_MaxMP)
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

    public void GroundAttack2_sound()
    {
        Data.audiosource.PlayOneShot(Data.GC.effect[10]);
    }
    public void GroundAttack2_Defense_sound()
    {
        Data.audiosource.PlayOneShot(Data.GC.effect[11]);
    }
}
