using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    //使用したOBJ===================================================================================
    public CharacterControler_ALL CC_ALL;
    public Animator animator;
    public CharacterData Data;
    public Input_Sword sword;
    public Input_Pouch pouch;
    public Input_Magic magic;
    public Input_Gun gun;
    public Input_Assassin assassin;
    public CPU_AI cpuai;
    //使用した変数=================================================================================
    public string animation_next;                  //次のアニメーションの変数
    public bool chackOK;                           //アニメーションの選択判定
    public string[] player_Input = new string[7];  //プレイヤのINPUT名前
    public bool[] Cpu_Input=new bool[8];           //CPUのINPUT変数
    public bool playercheck;                       //プレイヤとCPUの判定
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
        if (gameObject.tag == "Player1" || gameObject.tag == "Player2" || gameObject.tag == "Player3" || gameObject.tag == "Player4")
        {
            playercheck = true;
        }
        else
        {
            playercheck = false;
        }
        Botan_choice();
        animation_next = "Stand";
        chackOK = false;
        
        Cpu_Input[(int)C_Input.Vertical] = false;
        Cpu_Input[(int)C_Input.Horizontal] = false;
        Cpu_Input[(int)C_Input.Attack1] = false;
        Cpu_Input[(int)C_Input.Attack2] = false;
        Cpu_Input[(int)C_Input.Defense] = false;
        Cpu_Input[(int)C_Input.Jump] = false;
        Cpu_Input[(int)C_Input.Sprint] = false;
        Cpu_Input[(int)C_Input.Defenseing] = false;
    }

    // Update is called once per frame
    void Update()
    {
        //死の時、RESET
        if (gameObject.GetComponent<Animator>().GetBool("Dead"))
        {
            animation_next = "Stand";
            chackOK = false;
        }
        if (Data.pickup_already == 1)
        {
            StopCoroutine("PickupAlready");
            Data.pickup_already = 2;
        }
        if (Data.GC.Gamestate>=2)
        {
            if (animator.GetBool("Dead") == false)
            {
                //プレイヤとCPUのINPUT判断
                if (playercheck)
                {
                    //player
                    Data.character_input_V = (Input.GetAxis(player_Input[(int)C_Input.Vertical]));
                    Data.character_input_H = Input.GetAxis(player_Input[(int)C_Input.Horizontal]);
                    
                    if (Input.GetButton(player_Input[(int)C_Input.Vertical]) || Input.GetButton(player_Input[(int)C_Input.Horizontal])||
                         Mathf.Abs(Data.character_input_V)>=0.6f || Mathf.Abs(Data.character_input_H)>=0.6f)
                    {
                        animator.SetBool("Inputcheck", true);
                    }
                    else
                    {
                        animator.SetBool("Inputcheck", false);
                    }
                    if (Input.GetButton(player_Input[(int)C_Input.Defense]))
                    {
                        animator.SetBool("Defense", true);
                    }
                    else
                    {
                        animator.SetBool("Defense", false);

                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
                    {
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.Pickup_item_Script.item_script.self.isKinematic = false;
                        }
                        chackOK = false;
                        if (Input.GetButtonUp(player_Input[(int)C_Input.Defense]))
                        {
                            animator.SetInteger("defense_LV", 0);
                        }
                    }
                }
                else
                {
                    //CPU
                    cpuai.CPUAI();
                    if(Data.character_Profession==3)
                    {
                        if (animator.GetBool("Shooting")) Cpu_Input[(int)C_Input.Defense] = true;
                    }
                    
                    if (Cpu_Input[(int)C_Input.Vertical] || Cpu_Input[(int)C_Input.Horizontal])
                    {
                        animator.SetBool("Inputcheck", true);
                    }
                    else
                    {
                        animator.SetBool("Inputcheck", false);
                    }
                    if (Cpu_Input[(int)C_Input.Defense])
                    {
                        animator.SetBool("Defense", true);
                    }
                    else
                    {
                        animator.SetBool("Defense", false);

                    }
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
                    {
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.Pickup_item_Script.item_script.self.isKinematic = false;
                        }
                        chackOK = false;
                        if (Cpu_Input[(int)C_Input.Defense]==false)
                        {
                            animator.SetInteger("defense_LV", 0);
                        }
                    }
                }
                if(Data.DeadStep==0)
                {
                    if (Data.character_Profession == 3)
                    {
                        if (Input.GetButtonDown(player_Input[(int)C_Input.Defense])|| Cpu_Input[(int)C_Input.Defense])
                        {
                            Cpu_Input[(int)C_Input.Defense] = false;
                            if (Data.character_input_V!=0&& Data.character_input_H!=0)
                            {
                                Data.shooting_angle = Mathf.Atan(Data.character_input_H / Data.character_input_V) / (Mathf.PI / 180);
                                Data.shooting_angle = Data.character_input_V < 0 ? Data.shooting_angle + 180 : Data.shooting_angle;
                            }else
                            {
                                Data.shooting_angle = Data.character_angle;
                            }
                        }
                    }
                    //Comon
                    if (animator.GetBool("BeAttack") == false)
                    {
                        //ボタンCHECK
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanInput"))
                        {
                            switch(Data.character_Profession)
                            {
                                case (int)Profession.Sword:
                                    sword.Sprint_botan();
                                    sword.Attack1_botan();
                                    sword.Attack2_botan();
                                    sword.Jump_botan();
                                    break;
                                case (int)Profession.Pounch:
                                    pouch.Sprint_botan();
                                    pouch.Attack1_botan();
                                    pouch.Attack2_botan();
                                    pouch.Jump_botan();
                                    break;
                                case (int)Profession.Magic:
                                    magic.Sprint_botan();
                                    magic.Attack1_botan();
                                    magic.Attack2_botan();
                                    magic.Jump_botan();
                                    break;
                                case (int)Profession.Gun:
                                    gun.Sprint_botan();
                                    gun.Attack1_botan();
                                    gun.Attack2_botan();
                                    gun.Jump_botan();
                                    break;
                                case (int)Profession.Assassin:
                                    assassin.Sprint_botan();
                                    assassin.Attack1_botan();
                                    assassin.Attack2_botan();
                                    assassin.Jump_botan();
                                    break;
                            }
                            
                        }
                        //アニメ実行
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            if (animation_next != "Stand" && chackOK == true)
                            {
                                Cpu_Input[(int)C_Input.Defense] = false;
                                Data.character_action_mathf = Data.character_action_mathf_max;
                                if (animation_next == "Jump_Start")
                                {
                                    Data.character_Ymove.y = 0f;
                                    Data.character_Ymove.y += Mathf.Sqrt((Data.character_jumpHeight) * -3.0f * (Data.character_gravity));
                                }
                                Data.AttackWay = Data.character_XZmove;
                                switch (Data.character_Profession)
                                {
                                    case (int)Profession.Sword:
                                        CC_ALL.MpUse_Sword(animation_next);
                                        if (animation_next == "GroundAttack(2)" || animation_next == "GroundAttack(3)")
                                        {
                                            if (animator.GetBool("CanAttackSwitch"))
                                            {
                                                Data.cc.Move(Data.character_XZmove * 0.3f);
                                                animator.Play(animation_next, 0, 0);
                                                transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle), transform.eulerAngles.z);
                                            }
                                        }
                                        else
                                        {
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        break;
                                    case (int)Profession.Pounch:
                                        CC_ALL.MpUse_Pouch(animation_next);
                                        if (animation_next == "GroundAttack(2)" || animation_next == "GroundAttack(3)")
                                        {
                                            if (animator.GetBool("CanAttackSwitch"))
                                            {
                                                Data.cc.Move(Data.character_XZmove * 0.3f);
                                                animator.Play(animation_next, 0, 0);
                                                transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle), transform.eulerAngles.z);
                                            }
                                        }
                                        else if (animation_next == "GroundAttack_Front" )
                                        {
                                           animator.Play(animation_next, 0, 0);
                                           transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle), transform.eulerAngles.z);
                                        }
                                        else if(animation_next== "GroundAttack2")
                                        {
                                            Data.character_Ymove.y = 20f;
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        else
                                        {
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        break;
                                    case (int)Profession.Magic:
                                        if(animation_next == "GroundAttack_Front")
                                        {
                                            magic.AttackBron("GroundAttack_Front");
                                            animator.Play(animation_next, 0, 0);
                                            Data.audiosource.PlayOneShot(Data.GC.effect[9]);
                                        }
                                        else if(animation_next == "GroundAttack_Defense")
                                        {
                                            magic.AttackBron("GroundAttack_Defense");
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        else if(animation_next== "AirAttack2_start")
                                        {
                                            magic.AttackBron("AirAttack2_start");
                                            Data.audiosource.PlayOneShot(Data.GC.effect[9]);
                                        }
                                        else if (animation_next == "Sprint")
                                        {
                                            Data.M_Sprintnow = true;
                                        }
                                        else
                                        {
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle), transform.eulerAngles.z);
                                        CC_ALL.MpUse_Magic(animation_next);
                                        break;
                                    case (int)Profession.Gun:
                                        if (animator.GetBool("Defense")&&animator.GetBool("InAir")==false)
                                        {
                                            Data.AttackWay= new Vector3(Mathf.Sin(Data.shooting_angle / (180 / Mathf.PI)), 0, Mathf.Cos(Data.shooting_angle / (180 / Mathf.PI)));
                                        }
                                        if(animation_next== "GroundAttack_Front")
                                        {
                                            animator.SetBool("Shooting", true);
                                            gun.AttackBron(animation_next);
                                        }
                                        else if(animation_next == "GroundAttack(1)"|| animation_next == "AirAttack")
                                        {
                                            if (gun.AirAttack_Grenade.Count > 2&& animation_next == "AirAttack")
                                            {
                                                Instantiate(Data.ccall.Profession_effect[4], this.transform.position, Quaternion.identity);
                                            }
                                            else if (gun.GroundAttack_Minefield.Count > 2 && animation_next == "GroundAttack(1)")
                                            {
                                                Instantiate(Data.ccall.Profession_effect[4], gun.GroundAttack_Pos.position, Quaternion.identity);
                                                
                                            }
                                            if (animator.GetBool("Shooting"))
                                            {
                                                animator.SetBool("Shooting", true);
                                                gun.StopAttack();
                                            }
                                            gun.AttackBron(animation_next);
                                        }
                                        else if ( animation_next == "AirAttack2_start")
                                        {
                                            if(gun.AirAttack2_use==false)
                                            {
                                                GameObject obj = Instantiate(Data.ccall.Profession_effect[6], this.transform.position, Quaternion.identity);
                                                obj.GetComponent<G_AA2>().characterNumber = Data.characterNumber;
                                                Data.character_Ymove.y = Mathf.Sqrt((Data.character_jumpHeight) * -2.0f * (Data.character_gravity));
                                                gun.AirAttack2_use = true;
                                                Data.audiosource.PlayOneShot(Data.GC.effect[12]);
                                            }
                                        }
                                        else 
                                        {
                                            if(animator.GetBool("Shooting"))
                                            {
                                                animator.SetBool("Shooting", true);
                                                gun.StopAttack();
                                            }
                                            if(animation_next == "GroundAttack2")
                                            {
                                                Data.audiosource.PlayOneShot(Data.GC.effect[12]);
                                            }
                                            animator.Play(animation_next, 0, 0);
                                            animator.SetBool("Shooting", false);
                                        }
                                        CC_ALL.MpUse_Gun(animation_next);
                                        break;
                                    case (int)Profession.Assassin:
                                        if(animation_next=="GroundAttack_Defense")
                                        {
                                            assassin.AttackBron(animation_next);
                                            Data.character_Ymove.y += Mathf.Sqrt((Data.character_jumpHeight) * -2f * (Data.character_gravity));
                                            animator.Play(animation_next, 0, 0);
                                            Data.audiosource.PlayOneShot(Data.GC.effect[11]);
                                        }
                                        else if (animation_next == "AirAttack2_start"|| animation_next == "GroundAttack2")
                                        {
                                            assassin.AttackBron(animation_next);
                                            animator.Play(animation_next, 0, 0);
                                            Data.audiosource.PlayOneShot(Data.GC.effect[9]);
                                        }
                                        else if (animation_next == "GroundAttack2_Defense")
                                        {
                                            if (assassin.GroundAttack2Defenseobj.Count <= 2)
                                            {
                                                assassin.AttackBron(animation_next);
                                                animator.Play(animation_next, 0, 0);
                                                Data.A_GAD2 = true;
                                                Data.audiosource.PlayOneShot(Data.GC.effect[9]);
                                            }
                                        }
                                        else if (animation_next == "Jump_Two")
                                        {
                                            Data.JumpTwo = true;
                                            Data.character_Ymove.y = 0;
                                        }else
                                        {
                                            Data.A_GAF = false;
                                            animator.Play(animation_next, 0, 0);
                                        }
                                        
                                        CC_ALL.MpUse_Assassin(animation_next);
                                        break;
                                        
                                }
                                cpuai.AttackStart = 0;
                                chackOK = false;
                                animation_next = "Stand";
                            }
                        }

                        
                    }
                    else
                    {
                        //受身
                        if (animator.GetBool("CanAttackSwitch"))
                        {
                            switch (Data.character_Profession)
                            {
                                case (int)Profession.Sword:
                                    sword.Jump_botan();
                                    break;
                                case (int)Profession.Pounch:
                                    pouch.Jump_botan();
                                    break;
                                case (int)Profession.Magic:
                                    magic.Jump_botan();
                                    break;
                                case (int)Profession.Gun:
                                    gun.Jump_botan();
                                    break;
                                case (int)Profession.Assassin:
                                    assassin.Jump_botan();
                                    break;
                            }
                        }
                        if (Data.character_Profession == (int)Profession.Magic  )
                        {
                            if(magic.GroundAttack_front_Start == true)magic.GroundAttack_front_stop();
                            if (magic.GroundAttack_Defense_start == true) magic.GroundAttack_Defense_Stop();
                            if (Data.M_Sprintnow) Data.M_Sprintnow = false;
                        }
                    }
                    
                }
            }
            if (Data.character_Profession == 3)
            {
                if (animator.GetBool("Shooting") && (animator.GetBool("BeAttack") || animator.GetBool("InAir") || animator.GetBool("Dead")))
                {
                    animator.SetBool("Shooting", false);
                    gun.StopAttack();
                }
            } 
        }
    }
    private void Botan_choice()
    {
        if (gameObject.tag == "Player1" || gameObject.tag == "Player2" || gameObject.tag == "Player3" || gameObject.tag == "Player4")
        {
            switch (gameObject.tag)
            {
                case "Player1":
                    player_Input[(int)C_Input.Vertical] = "Vertical player1";
                    player_Input[(int)C_Input.Horizontal] = "Horizontal player1";
                    player_Input[(int)C_Input.Attack1] = "Attack1 player1";
                    player_Input[(int)C_Input.Attack2] = "Attack2 player1";
                    player_Input[(int)C_Input.Defense] = "Defense player1";
                    player_Input[(int)C_Input.Jump] = "Jump player1";
                    player_Input[(int)C_Input.Sprint] = "Sprint player1";
                    break;
                case "Player2":
                    player_Input[(int)C_Input.Vertical] = "Vertical player2";
                    player_Input[(int)C_Input.Horizontal] = "Horizontal player2";
                    player_Input[(int)C_Input.Attack1] = "Attack1 player2";
                    player_Input[(int)C_Input.Attack2] = "Attack2 player2";
                    player_Input[(int)C_Input.Defense] = "Defense player2";
                    player_Input[(int)C_Input.Jump] = "Jump player2";
                    player_Input[(int)C_Input.Sprint] = "Sprint player2";
                    break;
                case "Player3":
                    player_Input[(int)C_Input.Vertical] = "Vertical player3";
                    player_Input[(int)C_Input.Horizontal] = "Horizontal player3";
                    player_Input[(int)C_Input.Attack1] = "Attack1 player3";
                    player_Input[(int)C_Input.Attack2] = "Attack2 player3";
                    player_Input[(int)C_Input.Defense] = "Defense player3";
                    player_Input[(int)C_Input.Jump] = "Jump player3";
                    player_Input[(int)C_Input.Sprint] = "Sprint player3";
                    break;
                case "Player4":
                    player_Input[(int)C_Input.Vertical] = "Vertical player4";
                    player_Input[(int)C_Input.Horizontal] = "Horizontal player4";
                    player_Input[(int)C_Input.Attack1] = "Attack1 player4";
                    player_Input[(int)C_Input.Attack2] = "Attack2 player4";
                    player_Input[(int)C_Input.Defense] = "Defense player4";
                    player_Input[(int)C_Input.Jump] = "Jump player4";
                    player_Input[(int)C_Input.Sprint] = "Sprint player4";
                    break;
            }
        }
        else
        {
            player_Input[(int)C_Input.Vertical] = "CPU";
            player_Input[(int)C_Input.Horizontal] = "CPU";
            player_Input[(int)C_Input.Attack1] = "CPU";
            player_Input[(int)C_Input.Attack2] = "CPU";
            player_Input[(int)C_Input.Defense] = "CPU";
            player_Input[(int)C_Input.Jump] = "CPU";
            player_Input[(int)C_Input.Sprint] = "CPU";
        }
            

    }
    //アイテム関わり==================================================================================
    public void Throw()
    {
        animator.SetBool("PickUp", false);
        Data.Pickup_item_Script.item_script.bePickup = false;
        Data.Pickup_item_Script.item_script.self.isKinematic = false;
        Data.pickrangein = false;
        Vector3 aa = Data.character_XZmove;
        if(Data.Pickup_item_Script.item_script.Item_num!=3)
        {
            //BANANA
            aa.y = 0.01f;
            Data.Pickup_item_Script.item_script.self.AddForce(aa * 0.0001f);
        }
        else
        {
            aa.y = 0;
            aa.Normalize();
            aa.y = 0.8f;
            Data.Pickup_item_Script.item_script.self.AddForce(aa * 0.00003f);
        }
        
        if(Data.Pickup_item_Script.item_script.Item_num==3)
        {
            Invoke("BananaOpen", 2f);
            Data.Pickup_item_Script.item_script.bananaing = true;
        }
    }
    private void OnTriggerStay(Collider ITEM)
    {
        if(ITEM.transform.parent != null)
        {
            if (ITEM.transform.parent.transform.parent != null)
            {
                //アイテムPICKUP
                if (ITEM.transform.parent.transform.parent.tag == "Item_CanPick" && animator.GetBool("PickUp") == false)
                {
                    ITEM itemget = ITEM.transform.parent.GetComponent<ITEM>();
                    if (itemget.bePickup == false && itemget.bananabotan == false&& itemget.bananaing==false)
                    {
                        Data.pickrangein = true;
                    }
                    if (itemget.speednow < 1.5f
                         && Input.GetButton(player_Input[(int)C_Input.Attack1]) && (animator.GetBool("Inputcheck") == false)
                         && itemget.bePickup == false && itemget.bananabotan == false&& itemget.bananaing == false
                         && (animator.GetCurrentAnimatorStateInfo(0).IsName("Stand")))
                    {
                        Data.Pickup_item = ITEM.transform.parent.transform.parent.gameObject;
                        Data.Pickup_item_Script = Data.Pickup_item.GetComponent<ITEM_Type>();
                        Data.Pickup_item_Script.item_script.bePickup = true;
                        animator.SetBool("PickUp", true);
                        itemget.PickupOne = this.gameObject;
                        //PICKUP效果
                        Instantiate(Data.Item_Effect_Prefab[2], Data.Effectcenter.transform.position, Quaternion.identity);
                        StartCoroutine("PickupAlready");
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider ITEM)
    {
        if(ITEM.transform.parent != null)
        {
            if (ITEM.transform.parent.transform.parent != null)
            {
                if (ITEM.transform.parent.transform.parent.tag == "Item_CanPick" && animator.GetBool("PickUp") == false)
                {
                    Data.pickrangein = false;
                }
            }
        }
    }
    IEnumerator PickupAlready()
    {
        yield return new WaitForSeconds(0.3f);
        Data.pickup_already = 1;
    }
    void Effect_Item6_Start()
    {
        GameObject aa = Instantiate(Data.Item_Effect_Prefab[9], Data.Effect_Item6_POS.transform.position, Data.Effect_Item6_POS.transform.rotation);
        aa.tag = gameObject.tag;
        if ((Data.Pickup_item_Script.item_script.HP_durable - 1) <= 0)
        {
            animator.SetBool("PickUp", false);
            Instantiate(Data.Item_Effect_Prefab[10], Data.Hand.transform.position, Data.Effect_Item6_POS.transform.rotation);
            Data.Pickup_item_Script.item_script.bePickup = false;
            Data.pickrangein = false;
            Data.pickup_already = 3;
            Invoke("Pickup_item_dead", 0.4f);
        }else Data.Pickup_item_Script.item_script.HP_durable--;
    }
    void Effect_Item7_Start()
    {
        if ((Data.Pickup_item_Script.item_script.HP_durable - 1) <= 0)
        {
            animator.SetBool("PickUp", false);
            Instantiate(Data.Item_Effect_Prefab[10], Data.Hand.transform.position, Data.Effect_Item6_POS.transform.rotation);
            Data.Pickup_item_Script.item_script.bePickup = false;
            Data.pickrangein = false;
            Data.pickup_already = 3;
            Invoke("Pickup_item_dead", 0.4f);
        }
        else Data.Pickup_item_Script.item_script.HP_durable--;


    }
    void Pickup_item_dead()
    {
        Data.Pickup_item_Script.item_script.HP_durable--;
    }
    void BananaOpen()
    {
        Data.Pickup_item_Script.item_script.bananabotan = true;
        
    }
}
