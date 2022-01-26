using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Gun : MonoBehaviour
{
    public CharacterInput CI;
    public CharacterData Data;
    public Animator animator;
    public List<GameObject> GroundAttack_Bullet;
    public Transform GroundAttack_Pos;
    public List<GameObject> AirAttack_Grenade;
    public List<GameObject> GroundAttack_Minefield;
    public bool AirAttack2_use;
    private void Start()
    {
        AirAttack2_use = false;
    }
    private void Update()
    {
        for(int i=0;i< GroundAttack_Bullet.Count;i++)
        {
            if(GroundAttack_Bullet[i]==null)
            {
                GroundAttack_Bullet.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < AirAttack_Grenade.Count; i++)
        {
            if (AirAttack_Grenade[i] == null)
            {
                AirAttack_Grenade.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < GroundAttack_Minefield.Count; i++)
        {
            if (GroundAttack_Minefield[i] == null)
            {
                GroundAttack_Minefield.RemoveAt(i);
                i--;
            }
        }
        if(animator.GetBool("InAir") == false)
        {
            AirAttack2_use = false;
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
        switch (animatinName)
        {
            case "GroundAttack_Front":
                if(animator.GetBool("Shooting") == false)
                {
                    InvokeRepeating("GroundAttack_born", 0, 0.3f);
                    Invoke("StopAttack", 1f);
                    animator.SetBool("Shooting", true);
                }
                else
                {
                    CancelInvoke("GroundAttack_born");
                    InvokeRepeating("GroundAttack_born", 0.1f, 0.3f);
                    CancelInvoke("StopAttack");
                    Invoke("StopAttack", 1f);
                }
                break;
            case "GroundAttack(1)":
                if(GroundAttack_Minefield.Count<=2)
                {
                    GroundAttack_Minefield.Add(Instantiate(Data.ccall.Profession_effect[5], GroundAttack_Pos.position, GroundAttack_Pos.rotation));
                    GroundAttack_Minefield[GroundAttack_Minefield.Count - 1].GetComponent<G_GA>().characterNumber = Data.characterNumber;
                }
                break;
            case "AirAttack":
                if (AirAttack_Grenade.Count <= 2)
                {
                    AirAttack_Grenade.Add(Instantiate(Data.ccall.Profession_effect[3], this.transform.position, Quaternion.identity));
                    AirAttack_Grenade[AirAttack_Grenade.Count - 1].GetComponent<Grenade>().characterNumber = Data.characterNumber;
                }
                break;
        }
    }
    public void StopAttack()
    {
        CancelInvoke("GroundAttack_born");
        animator.SetBool("Shooting", false);
    }
    void GroundAttack_born()
    {
        GroundAttack_Bullet.Add(Instantiate(Data.ccall.Profession_effect[1], GroundAttack_Pos.position, Quaternion.identity));
        GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].GetComponent<G_GA_Front>().characterNumber = Data.characterNumber;
        if(animator.GetBool("Defense"))
        {
            GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].GetComponent<G_GA_Front>().moveway = new Vector3(Mathf.Sin((Data.shooting_angle) * (Mathf.PI / 180)), 0, Mathf.Cos((Data.shooting_angle) * (Mathf.PI / 180)));
            GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.localEulerAngles = new Vector3(GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.rotation.x, Data.shooting_angle, GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.rotation.z);
        }
        else
        { 
            GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].GetComponent<G_GA_Front>().moveway = Data.character_XZmove;
            GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.localEulerAngles = new Vector3(GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.rotation.x, Data.character_angle, GroundAttack_Bullet[GroundAttack_Bullet.Count - 1].transform.rotation.z);
        }
        
       Instantiate(Data.ccall.Profession_effect[2], GroundAttack_Pos.position, Quaternion.identity);
    }

    void GroundAttackDefense_Start()
    {
        GameObject GAD =Instantiate(Data.ccall.Profession_effect[7], GroundAttack_Pos.position, Quaternion.identity);
        GAD.GetComponent<G_GA2D>().characterNumber = Data.characterNumber;
        GAD.GetComponent<G_GA2D>().moveway = Data.AttackWay;
    }

    public void Sprint_botan()
    {
        if (Data.character_NowMP >= Data.MP_gun[8] * Data.character_MaxMP)
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
                    if (Data.character_NowMP >= Data.MP_gun[6] * Data.character_MaxMP)
                    {
                        CI.chackOK = true;
                        CI.animation_next = "AirAttack";
                    }
                }
                else if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetBool("PickUp") == false)
                    {
                        
                        if (animator.GetBool("Inputcheck")==false&& animator.GetBool("Defense")==false)
                        {
                            if (Data.pickrangein == false&& Data.character_NowMP >= Data.MP_gun[0] * Data.character_MaxMP)
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack(1)";
                            }
                        }else 
                        {
                            if (Data.character_NowMP >= Data.MP_gun[2] * Data.character_MaxMP)
                            {
                                CI.chackOK = true;
                                CI.animation_next = "GroundAttack_Front";
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
                    if (Data.character_NowMP >= Data.MP_gun[7] * Data.character_MaxMP)
                    {
                        CI.animation_next = "AirAttack2_start";
                        CI.chackOK = true;
                    }
                }
                else if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetBool("Defense") && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_gun[5] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Defense";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Inputcheck"))
                    {
                        if (Data.character_NowMP >= Data.MP_gun[4] * Data.character_MaxMP)
                        {
                            CI.animation_next = "GroundAttack2_Front";
                            CI.chackOK = true;
                        }

                    }
                    else if (animator.GetBool("Defense") == false && animator.GetBool("Inputcheck") == false)
                    {
                        if (Data.character_NowMP >= Data.MP_gun[3] * Data.character_MaxMP)
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
    public void G_GA2F_Sound()
    {
        Data.audiosource.PlayOneShot(Data.GC.effect[12]);
    }
}
