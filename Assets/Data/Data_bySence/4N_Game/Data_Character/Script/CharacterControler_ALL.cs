using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler_ALL : MonoBehaviour
{
    //使用したScript============================================================================
    public CharacterData Data;
    //使用したGameobject========================================================================
    public Animator animator;
    public GameObject effect_Invincible; //無敵時間の効果
    //各職業使用したGameobject==================================================================
    public GameObject[] Profession_effect;
    //キャラクタ使用のSKIN変化==================================================================
    public SkinnedMeshRenderer[] Hair_obj;
    public SkinnedMeshRenderer Clothes_obj;
    public Material[] teamType;
    public Material[] characterType;
    public Material CPU_Clothe;
    //使用した変数==============================================================================
    private int Invincible_change;
    public bool character_isGround;
    private float speed_reduce;
    private float character_wakeupspeed_distance;
    //アイテム関わり
    private bool eatingstart;
    private bool MP_speedUp;
    private GameObject Item_Eating_Effect;
    private GameObject Item1_buff_Effect;
    private GameObject Item4_debuff_Effect;
    private GameObject Item5_debuff_Effect;
    enum Profession
    {
        Sword = 0,
        Pounch,
        Magic,
        Gun,
        Assassin,
    }
    // Start is called before the first frame update
    void Start()
    {
        Invincible_change = 0;
        eatingstart = false;
        MP_speedUp = false;
        Hair_obj[0].material = teamType[Data.characterTeam];
        Hair_obj[1].material = teamType[Data.characterTeam];
        switch (Data.character_Profession)
        {
            case (int)Profession.Sword:
                speed_reduce = 8.0f;
                character_wakeupspeed_distance = 2f;
                break;
            case (int)Profession.Pounch:
                speed_reduce = 8.0f;
                character_wakeupspeed_distance = 2f;
                break;
            case (int)Profession.Magic:
                speed_reduce = 8.0f;
                character_wakeupspeed_distance = 2f;
                break;
            case (int)Profession.Gun:
                speed_reduce = 8.0f;
                character_wakeupspeed_distance = 2f;
                break;
            case (int)Profession.Assassin:
                speed_reduce = 8.0f;
                character_wakeupspeed_distance = 2f;
                break;
        }

        if (gameObject.tag == "Player1" || gameObject.tag == "Player2" || gameObject.tag == "Player3" || gameObject.tag == "Player4")
        {
            switch (Data.character_Profession)
            {
                case (int)Profession.Sword:
                    Clothes_obj.material = characterType[Data.Sword_num];
                    break;
                case (int)Profession.Pounch:
                    Clothes_obj.material = characterType[Data.Pounch_num];
                    break;
                case (int)Profession.Magic:
                    Clothes_obj.material = characterType[Data.Magic_num];
                    break;
                case (int)Profession.Gun:
                    Clothes_obj.material = characterType[Data.Gun_num];
                    break;
                case (int)Profession.Assassin:
                    Clothes_obj.material = characterType[Data.Assassin_num];
                    break;
            }
        }
        else
        {
            Clothes_obj.material = CPU_Clothe;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //死の時、RESET
        if(Data.DeadStep!=0)
        {
            Invincible_change = 0;
            eatingstart = false;
            MP_speedUp = false;
            Data.Item_Effect_Prefab[12].SetActive(false);
            animator.SetBool("Invincible", false);
        }

        //アイテムの効果==================================================================================================
        //Item1の効果
        if (MP_speedUp == false)
        {
            Data.character_NowMP += 10.0f * Time.deltaTime;
        }
        else if (MP_speedUp == true)
        {
            Data.character_NowMP += 25.0f * Time.deltaTime;
            Item1_buff_Effect.transform.position = Data.Effectcenter.transform.position ;
        }
        //Eating関わり効果
        if (animator.GetBool("Eating")&& animator.GetBool("BeAttack")==false)
        {
            if (eatingstart == false)
            {
                eatingstart = true;
                InvokeRepeating("Eating", 0.5f, 0.7f);
                Item_Eating_Effect = Instantiate(Data.Item_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
            }
            if (Data.Pickup_item_Script.item_script.Item_num == 0)
            {
                Data.character_NowHP += 10.0f * Time.deltaTime;
            }
            
        }
        else
        {
            if (eatingstart == true)
            {
                CancelInvoke("Eating");
                Destroy(Item_Eating_Effect, 0);
                eatingstart = false;
            }
            eatingstart = false;
        }
        //Item4のDebuff
        if(Data.Slowdown==1)
        {
            Data.nor_speed = 1.5f;
            Invoke("Item4", 5);
            //食べ後のEFFECT
            Quaternion rott = Data.GC.transform.rotation;
            rott.x = -1f;
            Vector3 vec = gameObject.transform.position;
            vec.y += 0.1f;
            Item4_debuff_Effect = Instantiate(Data.Item_Effect_Prefab[5], vec, rott);
            
            Data.Slowdown = 2;
        }
        else if(Data.Slowdown==0)
        {
            Data.nor_speed = 3;
        }
        else if (Data.Slowdown == 3)
        {
            CancelInvoke("Item4");
            Invoke("Item4", 5);
            Data.Slowdown = 2;
        }
        if(Data.Slowdown>0&& Item4_debuff_Effect!=null)
        {
            Vector3 vec = gameObject.transform.position;
            vec.y += 0.1f;
            Item4_debuff_Effect.transform.position = vec;
        }
        //Item5のDebuff
        if (Data.Mushroom == 1)
        {
            Invoke("Item5", 5);
            //食べ後のEFFECT
            Item5_debuff_Effect = Instantiate(Data.Item_Effect_Prefab[8], Data.Effectcenter.transform.position, Data.Effectcenter.transform.rotation);

            Data.Mushroom = 2;
        }
        else if (Data.Mushroom == 3)
        {
            CancelInvoke("Item5");
            Invoke("Item5", 5);
            Data.Mushroom = 2;
        }
        if (Data.Mushroom > 0 && Item5_debuff_Effect != null)
        {
            Item5_debuff_Effect.transform.position = Data.Effectcenter.transform.position;
        }
        //HPとMP制御==================================================================================================
        if (Data.character_NowMP >= Data.character_MaxMP) Data.character_NowMP = Data.character_MaxMP;
        if (Data.character_NowMP <= 0f) Data.character_NowMP = 0f;
        if (Data.character_NowHP >= Data.character_MaxHP) Data.character_NowHP = Data.character_MaxMP;
        if (Data.character_NowHP <= 0f) Data.character_NowHP = 0f;
        //地面判定===================================================================================================
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack") == false) character_isGround = Data.cc.isGrounded;
        else
        {
            if (Data.character_Profession != 1)
            {
                 character_isGround = Data.cc.isGrounded;
            }else if (Data.character_Profession==2)
            {
                if(Profession_effect[8].activeSelf==false) character_isGround = Data.cc.isGrounded;
            }
        }

        if (Mathf.Abs(transform.position.x) <= 13 && transform.position.y <= 0.026f)
        {
            //穴がいないステージ
            character_isGround = true;
        }
        if (Data.cc.isGrounded && Data.character_Ymove.y < 0)
        {
            Data.character_Ymove.y = 0f;
        }

        if (character_isGround)
        {
            animator.SetBool("InAir", false);
            if (Data.character_Profession == (int)Profession.Assassin) Data.JumpTwo = false;
        }
        else
        {
            animator.SetBool("InAir", true);
        }
        //無敵時間===================================================================================================
        //無敵時間発生
        if (animator.GetBool("Invincible") && animator.GetBool("InvincibleOff") && character_isGround == true)
        {
            if (animator.GetBool("CantMove") == false)
            {
                animator.SetBool("InvincibleOff", false);
                StartCoroutine("Invincibletimelong");
                Invincible_change = 1;
            }
            else if (animator.GetBool("CantMove"))
            {
                animator.SetBool("InvincibleOff", false);
                StartCoroutine("Invincibletime");
                Invincible_change = 2;
            }
        }
        //無敵時間の続く時間
        if (animator.GetBool("Invincible") == false && Invincible_change != 0)
        {
            switch (Invincible_change)
            {
                case 1:
                    StopCoroutine("Invincibletimelong");
                    break;
                case 2:
                    StopCoroutine("Invincibletime");
                    break;
            }
            Invincible_change = 0;
        }
        //無敵時間の効果
        if (animator.GetBool("Invincible"))
        {
            effect_Invincible.SetActive(true);
        }
        else
        {
            effect_Invincible.SetActive(false);
        }
        //キャラクタXZ平面移動===================================================================================================
        XZ_Move();
        //キャラクタのXZ座標はバトルエリアの外の処理
        HitWall();
        //キャラクタY平面移動===================================================================================================
        Y_Move();
        //PICKUPの時の処理 (アイテム関わり)===================================================================================================
        if (animator.GetBool("PickUp") == true)
        {
            //角度と位置の制御
            if (Data.Pickup_item != null)
            {
                Data.Pickup_item.transform.position = Data.Hand.transform.position;
                //ボトルのモデルですので、特殊処理
                if (Data.Pickup_item_Script.item_script.Item_num == 0)
                {
                    Vector3 vec = Data.Pickup_item.transform.position;
                    vec.y += 0.3f;
                    Data.Pickup_item.transform.position = vec;
                }
                //PICKUPの時角度の制御
                if (Data.Pickup_item_Script.item_script.Item_num == 6 || Data.Pickup_item_Script.item_script.Item_num == 7)
                {
                    if (Data.Pickup_item_Script.item_script.Item_num == 7)
                    {
                        Data.PickItem7.SetActive(true);
                        Quaternion rot = gameObject.transform.rotation;
                        if (rot.y >= 0)
                        {
                            rot.y -= 0.5f;
                        }
                        else
                        {
                            rot.y += 0.5f;
                        }

                        Data.Pickup_item.transform.position += Data.character_XZmove * 0.2f;
                        Data.Pickup_item.transform.rotation = rot;
                    }
                    else
                    {
                        Data.PickItem6.SetActive(true);
                        Data.Pickup_item.transform.rotation = Data.Hand.transform.rotation;
                    }
                }
                else
                {
                    Quaternion rot = gameObject.transform.rotation;
                    rot.x += Data.Pickup_item.transform.rotation.x;
                    rot.y += Data.Pickup_item.transform.rotation.y;
                    rot.z += Data.Pickup_item.transform.rotation.z;
                    if (Data.Pickup_item_Script.item_script.Item_num == 0)
                    {
                        rot.x += -10f;
                    }
                    Data.Pickup_item.transform.rotation = rot;
                }
            }
            else animator.SetBool("PickUp", false);
        }
        else
        {
            Data.pickup_already = 0;
            Data.PickItem6.SetActive(false);
            Data.PickItem7.SetActive(false);
        }
        //===================================================
        //各職業特別処理
        switch (Data.character_Profession)
        {
            case (int)Profession.Sword:
                Sword_Update();
                break;
            case (int)Profession.Pounch:
                Pouch_Update();
                break;
            case (int)Profession.Magic:
                
                break;
            case (int)Profession.Gun:
                Gun_Update();
                break;
            case (int)Profession.Assassin:
                
                break;
        }
    }
    void XZ_Move()
    {
        if (Data.character_angle > 180)
        {
            Data.character_angle -= 360;
        }
        else if (Data.character_angle < (-180))
        {
            Data.character_angle += 360;
        }

        if (Data.GC.Gamestate >= 2)
        {
            if (Data.DeadStep == 0)
            {
                if (animator.GetBool("BeAttack") == false)
                {
                    character_wakeupspeed_distance = 4;
                    //方向制御と移動
                    if (gameObject.tag == "Player1" || gameObject.tag == "Player2" || gameObject.tag == "Player3" || gameObject.tag == "Player4")
                    {
                        if (Data.character_input_H == 0 && Data.character_input_V == 0)
                        {
                            Data.character_angle = transform.eulerAngles.y;
                        }
                        else
                        {
                            if (Data.Mushroom > 0)
                            {
                                Data.character_input_H *= -1;
                                Data.character_input_V *= -1;
                            }
                            Data.character_angle = Mathf.Atan(Data.character_input_H / Data.character_input_V) / (Mathf.PI / 180);
                            Data.character_angle = Data.character_input_V < 0 ? Data.character_angle + 180 : Data.character_angle;
                            if (Data.character_angle > 180)
                            {
                                Data.character_angle -= 360;
                            }
                            else if (Data.character_angle < (-180))
                            {
                                Data.character_angle += 360;
                            }
                        }
                        Data.character_XZmove = new Vector3(Mathf.Sin(Data.character_angle / (180 / Mathf.PI)), 0, Mathf.Cos(Data.character_angle / (180 / Mathf.PI)));
                    }
                    
                    if (animator.GetBool("CantMove") == false || animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
                    {
                        Data.character_speed = Data.nor_speed;
                        Data.character_action_mathf = Data.character_action_mathf_max;
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense") == false)
                        {
                            if (animator.GetBool("Eating") == false)
                            {
                                if (animator.GetBool("Inputcheck"))
                                {
                                    if(Time.timeScale != 0)
                                    {
                                        animator.SetBool("Move", true);
                                        if (Data.character_Profession == 3 && animator.GetBool("Defense") && animator.GetBool("CantMove") == false)
                                        {
                                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Data.shooting_angle, transform.eulerAngles.z);
                                        }
                                        else
                                        {
                                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Data.character_angle, transform.eulerAngles.z);
                                        }
                                        Vector3 move = new Vector3(Data.character_input_H, 0, Data.character_input_V);
                                        Data.cc.Move(move * Time.deltaTime * Data.character_speed);
                                    }
                                }
                                else
                                {
                                    animator.SetBool("Move", false);
                                }
                            }
                        }
                        else
                        {
                            if (animator.GetBool("Inputcheck"))
                            {
                                animator.SetBool("Move", true);
                                Data.character_angle = Mathf.Atan(Data.character_input_H / Data.character_input_V) / (Mathf.PI / 180);
                                Data.character_angle = Data.character_input_V < 0 ? Data.character_angle + 180 : Data.character_angle;
                                Data.character_angle += 360;
                                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Data.character_angle, transform.eulerAngles.z);
                            }
                            else
                            {
                                animator.SetBool("Move", false);
                            }
                        }
                        
                        if (Data.character_Profession == (int)Profession.Magic)
                        {
                            if (Data.M_Sprintnow)
                            {
                                Data.M_Sprintnow = false;
                                Data.cc.Move(Data.AttackWay * Data.character_sprint_speed);
                                Instantiate(Profession_effect[6], Data.Effectcenter.transform.position, Quaternion.identity);
                            }
                        }else if (Data.character_Profession == (int)Profession.Assassin)
                        {
                            if(Data.JumpTwo)
                            {
                                Data.cc.Move(Data.AttackWay * Data.JumpTwo_speed*Time.deltaTime);
                            }
                        }
                    }
                    else
                    {
                        
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint"))
                        {
                            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Data.character_angle, transform.eulerAngles.z);
                            animator.SetBool("Eating", false);
                            Data.character_action_mathf -= Time.deltaTime * Data.character_action_mathf_max*Data.character_sprint_distance;
                            Data.cc.Move(Data.character_XZmove * Time.deltaTime * Data.character_sprint_speed);
                            //移動距離を超える
                            if (Data.character_action_mathf <= 0)
                            {
                                animator.SetBool("CantMove", false);
                                Data.character_action_mathf = Data.character_action_mathf_max;
                            }
                        }
                        //身体移動技
                        switch (Data.character_Profession)
                        {
                            case (int)Profession.Pounch:
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack2_Front"))
                                {
                                    animator.SetBool("Eating", false);
                                    Data.character_action_mathf -= Time.deltaTime * Data.character_action_mathf_max * Data.character_Attack1Front_distance;
                                    Data.cc.Move(Data.AttackWay * Time.deltaTime * Data.character_Attack1Front_speed);
                                    if (Data.character_action_mathf <= 0)
                                    {
                                        animator.SetBool("CantMove", false);
                                        Data.character_action_mathf = Data.character_action_mathf_max;
                                    }
                                }
                                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
                                {
                                    Data.cc.Move(Data.AttackWay * Time.deltaTime * Data.character_AirAttack1_sprint);
                                }
                                break;
                            case (int)Profession.Magic:
                                if(Data.M_Sprintnow)
                                {
                                    Data.M_Sprintnow = false;
                                    Data.cc.Move(Data.AttackWay * Data.character_sprint_speed);
                                    Instantiate(Profession_effect[6], Data.Effectcenter.transform.position, Quaternion.identity);
                                }
                                break;
                            case (int)Profession.Gun:
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack2_Defense_2"))
                                {
                                    Data.character_speed = 5f;
                                    character_wakeupspeed_distance -= Time.deltaTime * 7f;
                                    if (character_wakeupspeed_distance <= 0)
                                    {
                                        animator.SetBool("CantMove", false);
                                        character_wakeupspeed_distance = 0;
                                        Data.character_speed = 0;
                                    }
                                    else
                                    {
                                        Data.cc.Move(Data.AttackWay * Time.deltaTime * Data.character_speed*(-1));
                                    }
                                }
                                break;
                            case (int)Profession.Assassin:
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)"))
                                {
                                    animator.SetBool("Eating", false);
                                    
                                    if (Data.character_action_mathf <= 0)
                                    {
                                        Data.character_action_mathf = Data.character_action_mathf_max;
                                        Data.A_GAF = true;
                                    }
                                    if(Data.A_GAF==false)
                                    {
                                        Data.character_action_mathf -= Time.deltaTime * Data.character_action_mathf_max * Data.character_Attack1Front_distance;
                                        Data.cc.Move(Data.AttackWay * Time.deltaTime * Data.character_Attack1Front_speed);
                                    }
                                }
                                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Defense"))
                                {
                                    animator.SetBool("Eating", false);
                                    if (character_wakeupspeed_distance <= 0)
                                    {
                                        Data.character_action_mathf = Data.character_action_mathf_max;
                                    }
                                    else
                                    {
                                        Data.character_action_mathf -= Time.deltaTime * Data.character_action_mathf_max * Data.character_GAD_distance;
                                        Data.cc.Move(Data.AttackWay * Time.deltaTime * Data.character_GAD_speed*(-1));
                                    }
                                }
                                if (Data.A_GAD2)
                                {
                                    Data.A_GAD2 = false;
                                    Data.cc.Move(Data.AttackWay * 4f * (-1));
                                }
                                break;

                        }
                    }
                }
                else
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)1"))
                    {
                        speed_reduce = 8.0f;
                        Data.character_speed -= speed_reduce * Time.deltaTime;
                        if (Data.character_speed <= 0)
                        {
                            Data.character_speed = 0;
                        }
                        Data.cc.Move(Data.character_XZmove * Time.deltaTime * Data.character_speed);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") 
                        || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                       || animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp") || animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp_speed"))
                    {
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                            || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                        {
                            if (character_isGround)
                            {
                                Data.character_speed -= speed_reduce * Time.deltaTime;
                            }
                            if (Data.character_speed <= 0)
                            {
                                Data.character_speed = 0;
                            }
                            Data.cc.Move(Data.character_XZmove * Time.deltaTime * Data.character_speed);
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp_speed"))
                        {
                            Data.character_speed = 5f;
                            character_wakeupspeed_distance -= Time.deltaTime * 7f;
                            if (character_wakeupspeed_distance <= 0)
                            {
                                animator.SetBool("CantMove", false);
                                character_wakeupspeed_distance = 0;
                                Data.character_speed = 0;
                            }
                            else
                            {
                                Data.cc.Move(Data.character_XZmove * Time.deltaTime * Data.character_speed);
                            }
                        }

                    }

                }
            }
                
        }
        
    }
    void Y_Move()
    {
        if(Data.DeadStep==0)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                //身体移動技
                switch (Data.character_Profession)
                {
                    case (int)Profession.Sword:
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack2_start"))
                        {
                            Data.character_Ymove.y = -13f;
                        }
                        else
                        {
                            Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                        }
                        break;
                    case (int)Profession.Pounch:
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack2"))
                        {
                            Data.character_Ymove.y += (Data.character_gravity)*5f * Time.deltaTime;
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
                        {
                            Data.character_Ymove.y = -10f;
                        }
                        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack2_start"))
                        {
                            Data.character_Ymove.y = -7f;
                        }
                        else
                        {
                            Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                        }
                        break;
                    case (int)Profession.Magic:
                        Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                        break;
                    case (int)Profession.Gun:
                        Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                        break;
                    case (int)Profession.Assassin:
                        Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                        break;
                }
                Data.cc.Move(Data.character_Ymove * Time.deltaTime);
                if (float.IsNaN(Data.character_Ymove.y))
                {
                    Debug.Log(Data.character_Ymove.y);
                    //Data.character_Ymove.y = 0;
                }
            }
            else
            {
                Data.character_Ymove.y += (Data.character_gravity) * Time.deltaTime;
                Data.cc.Move(Data.character_Ymove * Time.deltaTime);
                if (float.IsNaN(Data.character_Ymove.y))
                {
                    Debug.Log(Data.character_Ymove.y);
                    //Data.character_Ymove.y = 0;
                }
            }
        }
        else if (Data.DeadStep >= 2&& Data.DeadStep<5)
        {
            Vector3 move = transform.position;
            move.y += (Data.character_gravity) * Time.deltaTime;
            if (move.y <= 0) move.y = 0;
            transform.position= move;
        }
    }
    void HitWall()
    {
        if (animator.GetBool("BeAttack") && gameObject.transform.position.y > 0.25)
        {
            if (gameObject.transform.position.z >= 4.65f)
            {
                Data.character_XZmove.z = -1f;
                Data.character_speed = 3f;
            }
            else if (gameObject.transform.position.z <= -4.65f)
            {
                Data.character_XZmove.z = 1f;
                Data.character_speed = 3f;
            }
            if (gameObject.transform.position.z >= 12f)
            {
                Data.character_XZmove.x = -1f;
                Data.character_speed = 3f;
            }
            else if (gameObject.transform.position.z <= -12f)
            {
                Data.character_XZmove.x = 1f;
                Data.character_speed = 3f;
            }
        }
    }
    
    //無敵時間のIEnumerator==============================================================================
    IEnumerator Invincibletime()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Invincible", false);
    }
    IEnumerator Invincibletimelong()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("Invincible", false);
    }
    //アイテム処理==============================================================================
    void Eating()
    {
        if (Data.Pickup_item_Script.item_script.Item_num > 0)
        {
            Data.Pickup_item_Script.item_script.HP_durable = 1;
            
        }
        Data.Pickup_item_Script.item_script.HP_durable--;
        if (Data.Pickup_item_Script.item_script.HP_durable <= 0)
        {
            switch (Data.Pickup_item_Script.item_script.Item_num)
            {
                case 0:
                    //食べ後のEFFECT
                    Instantiate(Data.Item_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    break;
                case 1:
                    //食べ後のEFFECT
                    Instantiate(Data.Item_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    if(MP_speedUp==false)
                    {
                        Item1_buff_Effect=Instantiate(Data.Item_Effect_Prefab[6], Data.Effectcenter.transform.position, Quaternion.identity);
                    }else
                    {
                        CancelInvoke("Item1");
                    }
                    //5秒の効果
                    Invoke("Item1", 5);
                    MP_speedUp = true;
                    break;
                case 2:
                    //同じチーム回復
                    if(Data.characterTeam != 0)
                    {
                        for(int i=0;i< Data.GC.Character_info.Length;i++)
                        {
                            if(Data.GC.Character_Data[i].characterTeam == Data.characterTeam)
                            {
                                Data.GC.Character_Data[i].character_NowHP += 10f;
                            }
                        }
                    }
                    else
                    {
                        Data.character_NowHP += 10f;
                    }
                    //食べ後のEFFECT
                    Instantiate(Data.Item_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    break;
                case 3:
                    //BANANA回復
                    Data.character_NowMP += 30f;
                    //食べ後のEFFECT
                    Instantiate(Data.Item_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    break;
                case 4:
                    if(Data.Slowdown==0)
                    {
                        Data.Slowdown = 1;
                    }
                    else
                    {
                        Data.Slowdown = 3;
                    }
                    Instantiate(Data.Item_Effect_Prefab[4], Data.Effectcenter.transform.position, Data.GC.transform.rotation);
                    break;
            }
            
            animator.SetBool("PickUp", false);
            animator.SetBool("Eating", false);
            Data.pickrangein = false;
        }
    }

    void Item1()
    {
        MP_speedUp = false;
        Destroy(Item1_buff_Effect, 0);
    }
    void Item4()
    {
        Destroy(Item4_debuff_Effect, 0);
        Data.Slowdown = 0;
    }
    void Item5()
    {
        Destroy(Item5_debuff_Effect, 0);
        Data.Mushroom = 0;
    }
    
    //色々なbool変数スイッチ==============================================================================
    public void CanInput_On()
    {
        animator.SetBool("CanInput", true);
    }
    public void CanAttackSwitch_On()
    {
        animator.SetBool("CanAttackSwitch", true);
    }
    public void CanAttackSwitch_Off()
    {
        animator.SetBool("CanAttackSwitch", false);
    }
    //各職業特別処理==============================================================================
    void Sword_Update()
    {
        if (animator.GetBool("BeAttack") == false && animator.GetBool("CanSwitch") ==false)
        {
            Profession_effect[0].SetActive(true);
        }
        else
        {
            Profession_effect[0].SetActive(false);
        }
        //PickUpの時、剣の顕現はOFF
        if (animator.GetBool("PickUp")|| animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack"))
        {
            Profession_effect[1].SetActive(false);
        }
        else
        {
            Profession_effect[1].SetActive(true);
        }
    }
    void Pouch_Update()
    {
        if (animator.GetBool("BeAttack") == false )
        {
            Profession_effect[0].SetActive(true);
            Profession_effect[1].SetActive(true);
        }
        else
        {
            Profession_effect[0].SetActive(false);
            Profession_effect[1].SetActive(false);
        }
        //PickUpの時、火の顕現はOFF
        if (animator.GetBool("PickUp"))
        {
            Profession_effect[0].SetActive(false);
            Profession_effect[1].SetActive(true);
        }
        else
        {
            Profession_effect[0].SetActive(true);
            Profession_effect[1].SetActive(true);
        }
    }
    void Gun_Update()
    {
        //PickUpの時、銃の顕現はOFF
        if (animator.GetBool("PickUp"))
        {
            Profession_effect[0].SetActive(false);
        }
        else
        {
            Profession_effect[0].SetActive(true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack2_Defense")) animator.SetBool("BigShooting", true);
        else animator.SetBool("BigShooting", false);
    }
    void Assassin_Update()
    {
        if (animator.GetBool("BeAttack") == false && animator.GetBool("CanSwitch"))
        {
            Profession_effect[0].SetActive(true);
            Profession_effect[1].SetActive(true);
        }
        else
        {
            Profession_effect[0].SetActive(false);
            Profession_effect[1].SetActive(false);
        }
        //PickUpの時、剣の顕現はOFF
        if (animator.GetBool("PickUp"))
        {
            Profession_effect[2].SetActive(false);
        }
        else
        {
            Profession_effect[2].SetActive(true);
        }
    }
    //技のMP消耗
    public void MpUse_Sword(string animation_next)
    {
        if(Data.GC.Game_Mp==0)
        {
            switch (animation_next)
            {
                case "GroundAttack_Defense":
                    Data.character_NowMP -= Data.MP_sword[0] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Front":
                    Data.character_NowMP -= Data.MP_sword[1] * Data.character_MaxMP;
                    break;
                case "GroundAttack2":
                    Data.character_NowMP -= Data.MP_sword[2] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Front":
                    Data.character_NowMP -= Data.MP_sword[3] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Defense":
                    Data.character_NowMP -= Data.MP_sword[4] * Data.character_MaxMP;
                    break;
                case "AirAttack2_start":
                    Data.character_NowMP -= Data.MP_sword[5] * Data.character_MaxMP;
                    break;
                case "Sprint":
                    Data.character_NowMP -= Data.MP_sword[6] * Data.character_MaxMP;
                    break;
            }
        }
    }

    public void MpUse_Pouch(string animation_next)
    {
        if (Data.GC.Game_Mp == 0)
        {
            switch (animation_next)
            {
                case "GroundAttack_Defense":
                    Data.character_NowMP -= Data.MP_pouch[0] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Front":
                    Data.character_NowMP -= Data.MP_pouch[1] * Data.character_MaxMP;
                    break;
                case "GroundAttack2":
                    Data.character_NowMP -= Data.MP_pouch[2] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Front":
                    Data.character_NowMP -= Data.MP_pouch[3] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Defense":
                    Data.character_NowMP -= Data.MP_pouch[4] * Data.character_MaxMP;
                    break;
                case "AirAttack":
                    Data.character_NowMP -= Data.MP_pouch[5] * Data.character_MaxMP;
                    break;
                case "AirAttack2_start":
                    Data.character_NowMP -= Data.MP_pouch[6] * Data.character_MaxMP;
                    break;
                case "Sprint":
                    Data.character_NowMP -= Data.MP_pouch[7] * Data.character_MaxMP;
                    break;
            }
        }
    }
    public void MpUse_Magic(string animation_next)
    {
        if (Data.GC.Game_Mp == 0)
        {
            switch (animation_next)
            {
                case "GroundAttack_Defense":
                    Data.character_NowMP -= Data.MP_magic[0] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Front":
                    Data.character_NowMP -= Data.MP_magic[1] * Data.character_MaxMP;
                    break;
                case "GroundAttack2":
                    Data.character_NowMP -= Data.MP_magic[2] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Front":
                    Data.character_NowMP -= Data.MP_magic[3] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Defense":
                    Data.character_NowMP -= Data.MP_magic[4] * Data.character_MaxMP;
                    break;
                case "AirAttack":
                    Data.character_NowMP -= Data.MP_magic[5] * Data.character_MaxMP;
                    break;
                case "AirAttack2_start":
                    Data.character_NowMP -= Data.MP_magic[6] * Data.character_MaxMP;
                    break;
                case "Sprint":
                    Data.character_NowMP -= Data.MP_magic[7] * Data.character_MaxMP;
                    break;
            }
        }
    }
    public void MpUse_Gun(string animation_next)
    {
        if (Data.GC.Game_Mp == 0)
        {
            switch (animation_next)
            {
                case "GroundAttack(1)":
                    Data.character_NowMP -= Data.MP_gun[0] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Defense":
                    Data.character_NowMP -= Data.MP_gun[1] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Front":
                    Data.character_NowMP -= Data.MP_gun[2] * Data.character_MaxMP;
                    break;
                case "GroundAttack2":
                    Data.character_NowMP -= Data.MP_gun[3] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Front":
                    Data.character_NowMP -= Data.MP_gun[4] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Defense":
                    Data.character_NowMP -= Data.MP_gun[5] * Data.character_MaxMP;
                    break;
                case "AirAttack":
                    Data.character_NowMP -= Data.MP_gun[6] * Data.character_MaxMP;
                    break;
                case "AirAttack2_start":
                    Data.character_NowMP -= Data.MP_gun[7] * Data.character_MaxMP;
                    break;
                case "Sprint":
                    Data.character_NowMP -= Data.MP_gun[8] * Data.character_MaxMP;
                    break;
            }
        }
    }
    public void MpUse_Assassin(string animation_next)
    {
        if (Data.GC.Game_Mp == 0)
        {
            switch (animation_next)
            {
                case "GroundAttack_Defense":
                    Data.character_NowMP -= Data.MP_assassin[0] * Data.character_MaxMP;
                    break;
                case "GroundAttack_Front":
                    Data.character_NowMP -= Data.MP_assassin[1] * Data.character_MaxMP;
                    break;
                case "GroundAttack2":
                    Data.character_NowMP -= Data.MP_assassin[2] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Front":
                    Data.character_NowMP -= Data.MP_assassin[3] * Data.character_MaxMP;
                    break;
                case "GroundAttack2_Defense":
                    Data.character_NowMP -= Data.MP_assassin[4] * Data.character_MaxMP;
                    break;
                case "AirAttack":
                    Data.character_NowMP -= Data.MP_assassin[5] * Data.character_MaxMP;
                    break;
                case "AirAttack2_start":
                    Data.character_NowMP -= Data.MP_assassin[6] * Data.character_MaxMP;
                    break;
                case "Sprint":
                    Data.character_NowMP -= Data.MP_assassin[7] * Data.character_MaxMP;
                    break;
            }
        }
    }
    public void Sword_AttackEffect()
    {
        Instantiate(Data.ccall.Profession_effect[2], Profession_effect[0].transform.position, Data.GC.transform.rotation);
    }
}
