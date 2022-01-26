using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackandHit : MonoBehaviour
{
    //使用しているOBJ===========================================================
    public Animator animator;
    public CharacterData Data;
    public CharacterInput input;
    public GameObject self;                 //現在のキャラOBJ
    public GameObject defenseBox;           //防御のOBJ
    public MeshRenderer defenseBoxcolor;    //防御のOBJの色OBJ
    public Camera_Game Cam;
    //使用している変数===========================================================
    private Color color;                    //防御の色変化使用した変数
    private bool colorchange;               //防御の色点滅使用した変数
    private int Knocknow;                   //飛びできない攻撃を連続受けの時、飛びの判定
    private int Knockcheck;                 //Knocknowの変化使用した変数
    private int Knocknum;                   //飛びできない=>飛び　の最大値
    private int defense_LVMAX;              //防御できるの最大値
    private int[] Item_BreakATK=new int[2];
    private int FireTime;
    // Start is called before the first frame update
    void Start()
    {
        colorchange = true;
        Knocknow = 0;
        Knockcheck = 0;
        Knocknum = 4;
        defense_LVMAX = 5;
        Item_BreakATK[0] = 1;//NUM8以下のアイテム
        Item_BreakATK[1] = 3;//NUM8のアイテム
        color = new Color(1f, 1.0f, 1.0f, 0.3f);
        Cam = Data.GC.GAMESTART[0].GetComponent< Camera_Game>();
        FireTime = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //死の時、RESET===================================================================
        if (gameObject.GetComponent<Animator>().GetBool("Dead"))
        {
            colorchange = true;
            Knocknow = 0;
            Knockcheck = 0;
            Knocknum = 4;
            defense_LVMAX = 5;
            color = new Color(1f, 1.0f, 1.0f, 0.3f);
        }
        //防御の時の処理===================================================================
        if (defenseBox.activeSelf == true)
        {
            switch(animator.GetInteger("defense_LV"))
            {
                case 1:
                    color.r=1.0f;
                    color.g=0.8f;
                    color.b= 0.8f;
                    break;
                case 2:
                    color.r = 0.8f;
                    color.g =0.6f;
                    color.b = 0.6f;
                    break;
                case 3:
                    color.r = 0.6f;
                    color.g = 0.4f;
                    color.b = 0.4f;
                    break;
                case 4:
                    color.r = 0.4f;
                    color.g = 0.2f;
                    color.b = 0.2f;
                    break;
            }
            if(animator.GetInteger("defense_LV")>= defense_LVMAX)
            {
                color.a = 0f;
            }
            else
            {
                //点滅
                if (colorchange == true)
                {
                    color.a += 0.01f;
                    if (color.a >= 0.7f)
                    {
                        color.a = 0.7f;
                        colorchange = false;
                    }
                }
                else
                {
                    color.a -= 0.01f;
                    if (color.a <= 0.3f)
                    {
                        color.a = 0.3f;
                        colorchange = true;
                    }
                }
            }
            defenseBoxcolor.material.color = color;
        }
        else
        {
            color = Color.white;
            color.a = 0.3f;
        }
        if (Knockcheck == 2)
        {
            StopCoroutine("Knockreset");
            Knocknow = 0;
            Knockcheck = 0;
        }
        if (Knockcheck == 1)
        {
            Knocknow++;
            StopCoroutine("Knockreset");
            StartCoroutine("Knockreset");
            Knockcheck = 3;
        }
    }
    void OnTriggerEnter(Collider Attackbox)
    {
        if (Attackbox.transform.parent != null&&Data.character_NowHP>0&&Data.DeadStep==0&& Attackbox.tag!="DeadSpace" && Attackbox.tag != "DeadSpace_gold" && Attackbox.tag != "slopleft" && Attackbox.tag != "slopright")
        {
            //キャラクタ攻撃
            if (Attackbox.transform.parent.transform.parent != null)
            {
                if (Attackbox.transform.parent.transform.parent.tag != "Item_CanPick" && Attackbox.transform.parent.transform.parent.tag != "Item_CantPick")
                {
                    if (Attackbox.transform.parent.transform.parent.tag != self.tag && animator.GetBool("Invincible") == false
                    && (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().characterTeam != Data.characterTeam || Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                    {
                        Data.FinalHit_Num = Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().characterNumber;
                        //BEATTACKの時、目指方向制御
                        if (animator.GetBool("HardMode") == false)
                        {
                            BeAttack_Direction(Attackbox);
                        }
                        switch(Attackbox.transform.parent.tag)
                        {
                            case "AB_Sword":
                                BeAttack_Sword(Attackbox);
                                break;
                            case "AB_Pouch":
                                BeAttack_Pouch(Attackbox);
                                break;
                            case "AB_Magic":
                                BeAttack_Magic(Attackbox);
                                break;
                            case "AB_Gun":
                                BeAttack_Gun(Attackbox);
                                break;
                            case "AB_Assassin":
                                BeAttack_Assassin(Attackbox);
                                break;
                        }
                    }
                }
            }
            else
            {
                if(Attackbox.transform.parent.tag== "AB_Magic")
                {
                    BeAttack_Magic(Attackbox);
                }else if (Attackbox.transform.parent.tag == "AB_Gun")
                {
                    BeAttack_Gun(Attackbox);
                }
                else if (Attackbox.transform.parent.tag == "AB_Assassin")
                {
                    BeAttack_Assassin(Attackbox);
                }
            }
            //アイテム衝撃
            ITEM item_script = Attackbox.transform.parent.GetComponent<ITEM>();
            if (Attackbox.tag == "ITEM_HitRange" && animator.GetBool("Invincible") == false && animator.GetBool("Dead") == false
                && item_script.bananabotan == false && item_script.speednow > 3f&& item_script.bePickup==false)
            {
                //目指方向制御
                if (animator.GetBool("HardMode") == false)
                {
                    BeAttack_Direction(Attackbox);
                }
                BeAttack_ItemTouch(Attackbox, item_script);
                
                Data.FinalHit_Num = -1;
            }
            //BANANAの滑る攻撃
            if (Attackbox.tag == "ITEM_HitRange" && animator.GetBool("Dead") == false )
            {
                if(item_script.bananabotan)
                {
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    Instantiate(Data.Item_Effect_Prefab[3], gameObject.transform.position, Quaternion.identity);
                    
                    Data.character_speed = 2.0f;
                    Data.character_Ymove.y = 2.0f;
                    if (animator.GetBool("PickUp"))
                    {
                        animator.SetBool("PickUp", false);
                        Data.Pickup_item_Script.item_script.bePickup = false;
                        Data.pickrangein = false;
                    }
                    if (animator.GetBool("Invincible") == false)
                    {
                        Data.character_NowHP -= (0.02f * Data.character_MaxHP);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (Data.character_NowHP <= 0)
                        {
                            if(Data.DeadStep == 0)if(Data.DeadStep == 0)Data.DeadNow();
                            if(Data.GC.GameSet == 0)
                            {
                                Data.GC.ItemKill++;
                                Data.bekillnum++;
                            }
                            if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0) 
                            {
                                foreach (var team in Data.GC.setTeam)
                                {
                                    if (Data.characterTeam == team)
                                    {
                                        Data.GC.GameSet = 2;
                                        Cam.gameset_target[0] = self;
                                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                        break;
                                    }
                                }
                                foreach (var tag in Data.GC.setCharacter)
                                {
                                    if (self.tag == tag)
                                    {
                                        Data.GC.GameSet = 2;
                                        Cam.gameset_target[0] = self;
                                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                        break;
                                    }
                                }
                            }
                        }
                        Data.FinalHit_Num = -1;
                    }
                    Destroy(Attackbox.transform.parent.transform.parent.gameObject, 0);
                }
            }
            //Item6の攻撃
            if (Attackbox.tag == "Item6_Attack" && animator.GetBool("Invincible") == false && animator.GetBool("Dead") == false
            && Attackbox.transform.parent.tag != gameObject.tag)
            {
                Data.character_NowHP -= (0.05f * Data.character_MaxHP);
                Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                if (Data.character_NowHP <= 0)
                {
                    if (Data.DeadStep == 0) Data.DeadNow();
                    if (Data.GC.GameSet == 0)
                    {
                        Data.bekillnum++;
                    }
                    if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                    {
                        foreach (var team in Data.GC.setTeam)
                        {
                            if (Data.characterTeam == team)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                break;
                            }
                        }
                        foreach (var tag in Data.GC.setCharacter)
                        {
                            if (self.tag == tag)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                break;
                            }
                        }
                    }
                }
            }
            //Item7の攻撃
            if (Attackbox.tag == "Item7_Attack" && animator.GetBool("Invincible") == false && animator.GetBool("Dead") == false)
            {
                if(Attackbox.transform.parent.transform.parent.tag != gameObject.tag)
                {
                    animator.SetBool("BeAttack", true);
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    Data.character_NowHP -= 0.06f * Data.character_MaxHP;
                    Data.FinalHit_Num = Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().characterNumber;
                    if (Data.character_NowHP <= 0)
                    {
                        if (Data.DeadStep == 0) Data.DeadNow();
                        if (Data.GC.GameSet == 0)
                        {
                            Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                            Data.bekillnum++;
                        }
                    }
                    Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                    Data.character_speed = 10.0f;
                    Data.character_Ymove.y = 8f;
                    if (animator.GetBool("PickUp"))
                    {
                        animator.SetBool("PickUp", false);
                        Data.Pickup_item_Script.item_script.bePickup = false;
                        Data.pickrangein = false;
                    }
                }
            }
        }
        
    }
    
    //通用処理===================================================================
    //目指方向制御
    void BeAttack_Direction(Collider Attackbox)
    {
        Vector3 moveway = self.transform.position - Attackbox.transform.parent.transform.position;
        if(Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>() != null)
        {
            if (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().character_Profession == 1)
            {
                if (Attackbox.transform.tag == "GroundAttack_Defense")
                {
                    moveway = Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().AttackWay;
                }
                else if (Attackbox.transform.tag == "GroundAttack2_Defense")
                {
                    moveway = Attackbox.transform.parent.transform.parent.transform.position - self.transform.position;
                }
            }
        }
        
        moveway.y = 0f;
        Data.character_angle = Mathf.Atan(moveway.x / moveway.z) / (Mathf.PI / 180);
        Data.character_angle = moveway.z < 0 ? Data.character_angle + 180 : Data.character_angle;
        moveway.Normalize();
        Data.character_XZmove = moveway;
        while (Mathf.Abs(Data.character_XZmove.x) < 0.1 && Data.character_XZmove.x > 0)
        {
            Data.character_XZmove.x *= 10;
        }
        while (Mathf.Abs(Data.character_XZmove.z) < 0.1 && Data.character_XZmove.z > 0)
        {
            Data.character_XZmove.z *= 10;
        }
        if (Data.character_XZmove.z == 0 && Data.character_XZmove.x == 0)
        {
            Data.character_XZmove.x += Random.Range((-0.1f), (1.0f));
            Data.character_XZmove.z += Random.Range((-0.1f), (1.0f));
        }
        if (Data.character_angle >= 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle - 180), transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle + 180), transform.eulerAngles.z);
        }
    }
    //飛べRESET
    IEnumerator Knockreset()
    {
        yield return new WaitForSeconds(1);
        if (Knocknow <= Knocknum)
        {
            Knockcheck = 2;
        }
    }
    //アイテム処理===================================================================
    void BeAttack_ItemTouch(Collider Attackbox,ITEM item_script)
    {
        if(Data.character_NowHP > 0 && Data.DeadStep == 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
            {
                if (item_script.Item_num == 4)
                {
                    Instantiate(Data.Item_Effect_Prefab[4], Data.Effectcenter.transform.position, Data.GC.transform.rotation);
                    if (Data.Slowdown == 2 || Data.Slowdown == 1)
                    {
                        Data.Slowdown = 3;
                    }
                    else if (Data.Slowdown == 0)
                    {
                        Data.Slowdown = 1;
                    }
                    item_script.HP_durable = 0;
                }
                if (item_script.Item_num == 5)
                {
                    Instantiate(Data.Item_Effect_Prefab[7], Data.Effectcenter.transform.position, Data.GC.transform.rotation);
                    if (Data.Mushroom == 2 || Data.Mushroom == 1)
                    {
                        Data.Mushroom = 3;
                    }
                    else if (Data.Mushroom == 0)
                    {
                        Data.Mushroom = 1;
                    }
                    item_script.HP_durable = 0;
                }
                if (animator.GetInteger("defense_LV") < defense_LVMAX)
                {
                    int nowLV = animator.GetInteger("defense_LV");
                    if (item_script.Item_num == 8|| item_script.Item_num == 12)
                    {
                        nowLV += Item_BreakATK[1];
                    }
                    else
                    {
                        nowLV += Item_BreakATK[0];
                    }
                    animator.SetInteger("defense_LV", nowLV);
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                else
                {
                    animator.SetInteger("defense_LV", 0);
                    animator.SetBool("BeAttack", true);
                    item_script.HP_durable--;
                    Data.character_NowHP -= item_script.Hit_character_Damage * Data.character_MaxHP;
                    if (Data.character_NowHP <= 0) 
                    {
                        if(Data.DeadStep == 0)Data.DeadNow();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.ItemKill++;
                            Data.bekillnum++;
                        }
                        if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0) 
                        {
                            foreach (var team in Data.GC.setTeam)
                            {
                                if (Data.characterTeam == team)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                            foreach (var tag in Data.GC.setCharacter)
                            {
                                if (self.tag == tag)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                        }
                    }
                    
                    if (item_script.Item_num != 8&& item_script.Item_num!=12)
                    {
                        if (animator.GetBool("InAir"))
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            Data.character_speed = 2.0f;
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            Data.character_speed = 2.0f;   //
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else if (item_script.Item_num == 8 || item_script.Item_num == 12)
                    {
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (item_script.HP_durable <= 0 )
                        {
                            Instantiate(Data.Item_Effect_Prefab[11], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = 3.0f;
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense") == false)
            {
                if (item_script.Item_num == 4)
                {
                    Instantiate(Data.Item_Effect_Prefab[4], Data.Effectcenter.transform.position, Data.GC.transform.rotation);
                    if (Data.Slowdown == 2 || Data.Slowdown == 1)
                    {
                        Data.Slowdown = 3;
                    }
                    else if (Data.Slowdown == 0)
                    {
                        Data.Slowdown = 1;
                    }
                    item_script.HP_durable = 0;
                }
                if (item_script.Item_num == 5)
                {
                    Instantiate(Data.Item_Effect_Prefab[7], Data.Effectcenter.transform.position, Data.GC.transform.rotation);
                    if (Data.Mushroom == 2 || Data.Mushroom == 1)
                    {
                        Data.Mushroom = 3;
                    }
                    else if (Data.Mushroom == 0)
                    {
                        Data.Mushroom = 1;
                    }
                    item_script.HP_durable = 0;
                }
                if (animator.GetBool("HardMode") == false)
                {
                    animator.SetBool("BeAttack", true);
                    item_script.HP_durable--;
                    Data.character_NowHP -= (item_script.Hit_character_Damage * Data.character_MaxHP);
                    if (Data.character_NowHP <= 0) 
                    {
                        if(Data.DeadStep == 0)Data.DeadNow();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.ItemKill++;
                            Data.bekillnum++;
                        }
                        if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0) 
                        {
                            foreach (var team in Data.GC.setTeam)
                            {
                                if (Data.characterTeam == team)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                            foreach (var tag in Data.GC.setCharacter)
                            {
                                if (self.tag == tag)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                        }
                    }
                    if (item_script.Item_num != 8&& item_script.Item_num != 12)
                    {
                        if (animator.GetBool("InAir"))
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            Data.character_speed = 2.0f;
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = 2.0f;   //
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                                
                        }
                    }
                    else
                    {
                        if (Knockcheck == 1) Knockcheck = 2;
                        if (item_script.HP_durable <= 0 )
                        {
                            Instantiate(Data.Item_Effect_Prefab[11], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = 3.0f;
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else
                {
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    item_script.HP_durable--;
                    if (item_script.HP_durable <= 0 && item_script.Item_num == 8)
                    {
                        Instantiate(Data.Item_Effect_Prefab[11], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    Data.character_NowHP -= (item_script.Hit_character_Damage * Data.character_MaxHP);
                    if (Data.character_NowHP <= 0) 
                    {
                        if(Data.DeadStep == 0)Data.DeadNow();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.ItemKill++;
                            Data.bekillnum++;
                        }
                        if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0) 
                        {
                            foreach (var team in Data.GC.setTeam)
                            {
                                if (Data.characterTeam == team)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                            foreach (var tag in Data.GC.setCharacter)
                            {
                                if (self.tag == tag)
                                {
                                    Data.GC.GameSet = 2;
                                    Cam.gameset_target[0] = self;
                                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                                    break;
                                }
                            }
                        }
                    }
                    
                }
            }
            if(item_script.Item_num == 4|| item_script.Item_num == 5) Data.audiosource.PlayOneShot(Data.GC.effect[20]);
            else Data.audiosource.PlayOneShot(Data.GC.effect[15]);
        }
    }
    //アイテムのTrigger処理
    private void OnTriggerStay(Collider ITEM)
    {
        if(Data.character_NowHP>0 && Data.DeadStep == 0)
        {
            //ITEM6の竜巻の中
            if (ITEM.tag == "Item6_Attack" && animator.GetBool("Invincible") == false && animator.GetBool("Dead") == false
                && ITEM.transform.parent.tag != gameObject.tag)
            {
                //BEATTACKの時、目指方向制御
                if (animator.GetBool("HardMode") == false)
                {
                    Vector3 moveway = gameObject.transform.position - ITEM.transform.parent.transform.position;
                    moveway.y = 0f;
                    Data.character_angle = Mathf.Atan(moveway.x / moveway.z) / (Mathf.PI / 180);
                    Data.character_angle = moveway.z < 0 ? Data.character_angle + 180 : Data.character_angle;
                    moveway.Normalize();
                    Data.character_XZmove = moveway;
                    while (Mathf.Abs(Data.character_XZmove.x) < 0.1 && Data.character_XZmove.x > 0)
                    {
                        Data.character_XZmove.x *= 10;
                    }
                    while (Mathf.Abs(Data.character_XZmove.z) < 0.1 && Data.character_XZmove.z > 0)
                    {
                        Data.character_XZmove.z *= 10;
                    }
                    if (Data.character_XZmove.z == 0 && Data.character_XZmove.x == 0)
                    {
                        Data.character_XZmove.x += Random.Range((-0.1f), (1.0f));
                        Data.character_XZmove.z += Random.Range((-0.1f), (1.0f));
                    }
                    if (Data.character_angle >= 0)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle - 180), transform.eulerAngles.z);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (Data.character_angle + 180), transform.eulerAngles.z);
                    }
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense"))
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += 1;
                        animator.SetInteger("defense_LV", nowLV);
                    }
                    else
                    {
                        animator.SetBool("BeAttack", true);
                        animator.SetInteger("defense_LV", 0);
                        if (Knockcheck == 1)
                        {
                            Knockcheck = 2;
                        }
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                        Data.character_speed = 2f;
                        Data.character_Ymove.y = 3f;
                    }

                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Defense") == false)
                {
                    if (Knockcheck == 1)
                    {
                        Knockcheck = 2;
                    }
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    Data.character_speed = 2f;
                    Data.character_Ymove.y = 3f;
                }
            }
            //ITEM9の下
            if (ITEM.tag == "ITEM_HitRange" && animator.GetBool("Dead") == false && ITEM.gameObject.layer == 8)
            {
                if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                Data.character_speed = 5.0f;
                if (animator.GetBool("PickUp"))
                {
                    animator.SetBool("PickUp", false);
                    Data.Pickup_item_Script.item_script.bePickup = false;
                    Data.pickrangein = false;
                }
            }
            //ITEM10の燃える攻撃
            if (ITEM.tag == "Item_N10")
            {
                Data.Item_Effect_Prefab[12].SetActive(true);
                FireTime = 3;
                if (Data.Item10_fire == 0)
                {
                    InvokeRepeating("Item10_damage", 0, 1f);
                    Data.Item10_fire = 2;
                }
                else if (Data.Item10_fire == 1)
                {
                    CancelInvoke("Item10_off");
                    Data.Item10_fire = 2;
                }
            }
            //ITEM11の転送効果
            if (ITEM.tag == "Item_N11"&& ITEM.GetComponent<ITEM>().HP_durable>0)
            {
                List<int> alife = new List<int>();
                for(int i=0;i< Data.GC.Character_Data.Length;i++)
                {
                    if (i == Data.characterNumber) continue;
                    if (Data.GC.Character_Data[i].DeadStep == 0) alife.Add(i);
                }
                int a = alife[Random.Range(0, (alife.Count))];
                Vector3 t_pos = Data.GC.Character_Data[a].transform.position;
                Vector3 m_pos = gameObject.transform.position;
                Instantiate(Data.Item_Effect_Prefab[13], Data.Effectcenter.transform.position, Quaternion.identity);
                Instantiate(Data.Item_Effect_Prefab[13], Data.GC.Character_Data[a].Effectcenter.transform.position, Quaternion.identity);
                Data.GC.Character_info[a].transform.position = m_pos;
                gameObject.transform.position = t_pos;
                Data.audiosource.PlayOneShot(Data.GC.effect[19]);
                Destroy(ITEM.transform.parent.gameObject);
            }
        }
        if(ITEM.tag== "DeadSpace_gold" && Data.DeadStep == 0)
        {
            Data.bekillnum++;
            if (animator.GetBool("PickUp")&&Data.Pickup_item != null) Destroy(Data.Pickup_item);
            if (Data.GC.Gamemode_num == 1 &&(animator.GetBool("BeAttack")==false||(animator.GetBool("BeAttack") && Data.FinalHit_Num<0)) )
            {
                if (gameObject.transform.position.x < -13)Data.GC.TeamPointPlus[1]+=3;
                else if (gameObject.transform.position.x > 13)Data.GC.TeamPointPlus[0]+=3;
            }
            else if (animator.GetBool("BeAttack")  )
            {
                if (Data.GC.Gamemode_num==1)
                {
                    if(gameObject.transform.position.x < -13)
                    {
                        if(Data.FinalHit_Num >= 0)
                        {
                            if (Data.GC.Character_Data[Data.FinalHit_Num].characterTeam == 1) Data.GC.Character_Data[Data.FinalHit_Num].Character_Point += 3;
                            else Data.GC.TeamPointPlus[1] += 3;
                        }
                        else Data.GC.TeamPointPlus[1] += 3;

                    }
                    else if (gameObject.transform.position.x > 13)
                    {
                        if (Data.FinalHit_Num >= 0)
                        {
                            if (Data.GC.Character_Data[Data.FinalHit_Num].characterTeam == 2) Data.GC.Character_Data[Data.FinalHit_Num].Character_Point += 3;
                            else Data.GC.TeamPointPlus[0] += 3;
                        }else Data.GC.TeamPointPlus[0] += 3;
                    }
                    if (Data.GC.GameSet_Already[1])
                    {
                        int teamA = 0;
                        int teamB = 0;
                        for (int i = 0; i < Data.GC.Character_Data.Length; i++)
                        {
                            if (Data.GC.Character_Data[i].characterTeam == 1) teamA += Data.GC.Character_Data[i].Character_Point;
                            else if (Data.GC.Character_Data[i].characterTeam == 2) teamB += Data.GC.Character_Data[i].Character_Point;
                        }
                        if (teamA >= Data.GC.GamePointMax || teamB >= Data.GC.GamePointMax) Data.GC.GameSet = 3;
                    }
                }
                Data.GC.Character_Data[Data.FinalHit_Num].killnum++;
            }
            if (animator.GetBool("PickUp") && Data.Pickup_item != null) Destroy(Data.Pickup_item);
            Data.DeadNow();
        }
        if(ITEM.tag == "DeadSpace" && Data.DeadStep == 0)
        {
            if (animator.GetBool("BeAttack") && Data.FinalHit_Num >= 0) Data.GC.Character_Data[Data.FinalHit_Num].killnum++;
            Data.bekillnum++;
            if (animator.GetBool("PickUp") && Data.Pickup_item != null) Destroy(Data.Pickup_item);
            Data.DeadNow();
        }
        if(ITEM.tag== "slopleft")
        {
            Vector3 leftslope = gameObject.transform.position;
            leftslope.x -= 2 * Time.deltaTime;
            gameObject.transform.position = leftslope;
        }
        else if (ITEM.tag == "slopright")
        {
            Vector3 leftslope = gameObject.transform.position;
            leftslope.x += 2 * Time.deltaTime;
            gameObject.transform.position = leftslope;
        }
    }
    private void OnTriggerExit(Collider ITEM)
    {
        //ITME10のエリアから出す後の燃える効果
        if (ITEM.tag == "Item_N10")
        {
            Data.Item10_fire = 1;
            Invoke("Item10_off", 2f);
        }
    }
    void Item10_off()
    {
        Data.Item_Effect_Prefab[12].SetActive(false);
        Data.Item10_fire = 0;
        FireTime = 3;
        CancelInvoke("Item10_damage");
    }
    void Item10_damage()
    {
        if(animator.GetBool("Invincible")==false)
        {
            Data.character_NowHP -= (0.02f * Data.character_MaxHP);
            FireTime--;
            if(FireTime<=0) Item10_off();
            if (Data.character_NowHP <= 0)
            {
                if(Data.DeadStep == 0)Data.DeadNow();
                Item10_off();
                if (Data.GC.GameSet == 0)
                {
                    Data.GC.ItemKill++;
                    Data.bekillnum++;
                }
                if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0)
                {
                    foreach(var team in Data.GC.setTeam)
                    {
                        if(Data.characterTeam==team)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = self;
                            break;
                        }
                    }
                    foreach (var tag in Data.GC.setCharacter)
                    {
                        if (self.tag == tag)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = self;
                            break;
                        }
                    }
                }
            }
            
        }
    }
    //職業によって攻撃を受ける処理===================================================================
    //SWORD
    void BeAttack_Sword(Collider Attackbox)
    {
        bool Defense_now = animator.GetCurrentAnimatorStateInfo(0).IsName("Defense");
        bool beKill = false;
        switch (Attackbox.transform.tag)
        {
            case "AirAttack":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV+=Data.Damage_sword_Break[0];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[0];
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)Knockcheck = 2;
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            Data.character_speed = Data.Damage_sword_speed[1];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[0];   //
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[0];
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            Data.character_speed = Data.Damage_sword_speed[1];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[0];   
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "AirAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[1];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[1];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetBool("BeAttack", true);
                        animator.SetInteger("defense_LV", 0);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[3];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[1];
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    if (Knockcheck == 1)Knockcheck = 2;
                    Data.character_NowHP -= Data.Damage_sword[1];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir"))
                        {
                            Data.character_speed = Data.Damage_sword_speed[2];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[0];
                        }
                        else
                        {
                            animator.SetBool("Invincible", true);
                            Data.character_speed = Data.Damage_sword_speed[3];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[1];
                        }
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(1)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV+=Data.Damage_sword_Break[2];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[2];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)Knockcheck = 2;
                            Data.character_speed = Data.Damage_sword_speed[4];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[2];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[5];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[2];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("HardMode") == false)
                    {
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)
                            {
                                Knockcheck = 2;
                            }
                            Data.character_speed = Data.Damage_sword_speed[4];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[2];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[5];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                }
                break;
            case "GroundAttack(2)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV+=Data.Damage_sword_Break[3];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[3];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        Data.character_speed = Data.Damage_sword_speed[7];
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)
                        {
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                            else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[3];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)Knockcheck = 2;
                            Data.character_speed = Data.Damage_sword_speed[6];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[3];
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[7];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(3)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[4];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[4];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[8];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[4];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[4];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[8];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[4];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack_Defense":
                Data.character_NowHP -= Data.Damage_sword[5];
                Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                if (Data.character_NowHP <= 0) beKill = true;
                animator.SetInteger("defense_LV", 0);
                animator.SetBool("BeAttack", true);
                if (Knockcheck == 1)Knockcheck = 2;
                Data.character_speed = Data.Damage_sword_speed[9];
                Data.character_Ymove.y = Data.Damage_sword_Ymove[5];
                if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                if (animator.GetBool("PickUp"))
                {
                    animator.SetBool("PickUp", false);
                    Data.Pickup_item_Script.item_script.bePickup = false;
                    Data.pickrangein = false;
                }
                break;
            case "GroundAttack_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[5];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[6];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        Data.character_speed = Data.Damage_sword_speed[11];
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)
                        {
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                            else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[6];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_sword_speed[10];
                            Data.character_Ymove.y = Data.Damage_sword_Ymove[6];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_sword_speed[11];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[6];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[7];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[12];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[7];
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[7];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[12];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[7];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[7];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[8];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[13];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[8];
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[8];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[13];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[8];
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Defense":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_sword_Break[8];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_sword[9];
                        Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_sword_speed[14];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[9];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_sword[9];
                    Data.audiosource.PlayOneShot(Data.GC.effect[14]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)
                        {
                            Knockcheck = 2;
                        }
                        Data.character_speed =Data.Damage_sword_speed[14];
                        Data.character_Ymove.y = Data.Damage_sword_Ymove[9];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
        }
        if(beKill==true)
        {
            if (Data.GC.GameSet == 0)
            {
                Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                if(Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum>=98)
                {
                    Data.GC.GameSet = 2;
                    Cam.gameset_target[0] = self;
                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                    Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                }
                Data.bekillnum++;
            }
            if (Data.DeadStep == 0)Data.DeadNow();
            if (Data.GC.GameSet_Already[0] == true&&Data.GC.GameSet == 0)
            {
                foreach (var team in Data.GC.setTeam)
                {
                    if (Data.characterTeam == team)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                        Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                        break;
                    }
                }

                foreach (var tag in Data.GC.setCharacter)
                {
                    if (self.tag == tag)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                        Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                        break;
                    }
                }

            }
        }
    }
    void BeAttack_Pouch(Collider Attackbox)
    {
        bool Defense_now = animator.GetCurrentAnimatorStateInfo(0).IsName("Defense");
        bool beKill = false;
        switch (Attackbox.transform.tag)
        {
            case "AirAttack":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[0];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[0];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        Data.character_speed = Data.Damage_pouch_speed[0];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[0];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[0];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = Data.Damage_pouch_speed[0];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[0];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "AirAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[1];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[1];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetBool("BeAttack", true);
                        animator.SetInteger("defense_LV", 0);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[1];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[1];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    if (Knockcheck == 1) Knockcheck = 2;
                    Data.character_NowHP -= Data.Damage_pouch[1];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        Data.character_speed = Data.Damage_pouch_speed[1];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[1];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(1)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[2];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[2];
                        Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_pouch_speed[2];
                            Data.character_Ymove.y = Data.Damage_pouch_Ymove[2];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_pouch_speed[2];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[2];
                    Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("HardMode") == false)
                    {
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)
                            {
                                Knockcheck = 2;
                            }
                            Data.character_speed = Data.Damage_pouch_speed[2];
                            Data.character_Ymove.y = Data.Damage_pouch_Ymove[2];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_pouch_speed[2];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(2)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[3];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[3];
                        Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        Data.character_speed = Data.Damage_pouch_speed[3];
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)
                        {
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                            else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[3];
                    Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_pouch_speed[3];
                            Data.character_Ymove.y = Data.Damage_pouch_Ymove[3];
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_pouch_speed[3];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(3)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[4];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[4];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[4];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[4];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[4];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[4];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[4];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack_Defense":
                Data.character_NowHP -= Data.Damage_pouch[5];
                Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                if (Data.character_NowHP <= 0) beKill = true;
                animator.SetInteger("defense_LV", 0);
                animator.SetBool("BeAttack", true);
                if (Knockcheck == 1) Knockcheck = 2;
                Data.character_speed = Data.Damage_pouch_speed[5];
                Data.character_Ymove.y = Data.Damage_pouch_Ymove[5];
                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                if (animator.GetBool("PickUp"))
                {
                    animator.SetBool("PickUp", false);
                    Data.Pickup_item_Script.item_script.bePickup = false;
                    Data.pickrangein = false;
                }
                break;
            case "GroundAttack_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[5];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[6];
                        Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        Data.character_speed = Data.Damage_pouch_speed[6];
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0)
                        {
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                            else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[6];
                    Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_pouch_speed[6];
                            Data.character_Ymove.y = Data.Damage_pouch_Ymove[6];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_pouch_speed[6];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[6];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[7];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[7];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[7];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[7];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[7];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[7];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[7];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[8];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[8];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[8];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[8];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[8];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[8];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Defense":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_pouch_Break[8];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_pouch[9];
                        Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[9];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[9];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start_NoInvincible");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_pouch[9];
                    Data.audiosource.PlayOneShot(Data.GC.effect[16]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)Knockcheck = 2;
                        Data.character_speed = Data.Damage_pouch_speed[9];
                        Data.character_Ymove.y = Data.Damage_pouch_Ymove[9];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start_NoInvincible");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
        }
        if (beKill == true)
        {
            if (Data.GC.GameSet == 0)
            {
                Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                if (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum >= 98)
                {
                    Data.GC.GameSet = 2;
                    Cam.gameset_target[0] = self;
                    Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                    Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                }
                Data.bekillnum++;
            }
            if (Data.DeadStep == 0) Data.DeadNow();
            if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
            {
                foreach (var team in Data.GC.setTeam)
                {
                    if (Data.characterTeam == team)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                        Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                        break;
                    }
                }

                foreach (var tag in Data.GC.setCharacter)
                {
                    if (self.tag == tag)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                        Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                        break;
                    }
                }

            }
        }
    }
    void BeAttack_Magic(Collider Attackbox)
    {
        bool Defense_now = animator.GetCurrentAnimatorStateInfo(0).IsName("Defense");
        bool beKill = false;
        switch (Attackbox.transform.tag)
        {
            case "AirAttack":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[0];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[0];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        Data.character_speed = Data.Damage_magic_speed[0];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[0];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[0];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = Data.Damage_magic_speed[0];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[0];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "AirAttack2":
                M_AttackFront MAF = Attackbox.transform.parent.GetComponent<M_AttackFront>();
                if (Data.GC.Character_Data[MAF.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[MAF.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[MAF.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[MAF.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_magic_Break[1];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_magic[2];
                            Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1) Knockcheck = 2;
                                Data.character_speed = Data.Damage_magic_speed[2];
                                Data.character_Ymove.y = Data.Damage_magic_Ymove[2];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_magic_speed[2];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_magic[1];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("HardMode") == false)
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1)
                                {
                                    Knockcheck = 2;
                                }
                                Data.character_speed = Data.Damage_magic_speed[1];
                                Data.character_Ymove.y = Data.Damage_magic_Ymove[1];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_magic_speed[1];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    Instantiate(MAF.hiteffect, MAF.transform.position, Quaternion.identity);
                    Destroy(MAF.gameObject);
                }
                break;
            case "GroundAttack(1)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[2];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[2];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        Data.character_speed = Data.Damage_magic_speed[2];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[2];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[2];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = Data.Damage_magic_speed[2];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[2];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack(2)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[3];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[3];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        Data.character_speed = Data.Damage_magic_speed[3];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[3];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[2];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Data.character_speed = Data.Damage_magic_speed[3];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[3];
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack_Defense":
                GroundAttack_Defense MAD = Attackbox.transform.parent.GetComponent<GroundAttack_Defense>();
                if (Data.GC.Character_Data[MAD.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[MAD.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[MAD.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[MAD.characterNumber].characterNumber;
                    Data.character_NowHP -= Data.Damage_magic[4];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    animator.SetInteger("defense_LV", 0);
                    animator.SetBool("BeAttack", true);
                    if (Knockcheck == 1) Knockcheck = 2;
                    Data.character_speed = Data.Damage_magic_speed[4];
                    Data.character_Ymove.y = Data.Damage_magic_Ymove[5];
                    if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                    Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("PickUp"))
                    {
                        animator.SetBool("PickUp", false);
                        Data.Pickup_item_Script.item_script.bePickup = false;
                        Data.pickrangein = false;
                    }
                }
                break;
            case "GroundAttack_Front":
                M_AttackFront MAG = Attackbox.transform.parent.GetComponent<M_AttackFront>();
                if (Data.GC.Character_Data[MAG.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[MAG.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[MAG.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[MAG.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_magic_Break[5];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_magic[5];
                            Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            Data.character_speed = Data.Damage_magic_speed[5];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }

                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_magic[5];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1) Knockcheck = 2;
                                Data.character_speed = Data.Damage_magic_speed[5];
                                Data.character_Ymove.y = Data.Damage_magic_Ymove[5];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_magic_speed[5];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    Instantiate(MAG.hiteffect, MAG.transform.position, Quaternion.identity);
                    Destroy(MAG.gameObject);
                }
                break;
            case "GroundAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[6];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[6];
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_magic_speed[6];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[6];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[6];
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_magic_speed[6];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[6];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[7];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[7];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_magic_speed[7];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[7];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[7];
                    Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_magic_speed[7];
                        Data.character_Ymove.y = Data.Damage_magic_Ymove[7];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Defense":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_magic_Break[8];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_magic[8];
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) animator.Play("DizzyBeAttack");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }

                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_magic[8];
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("InAir") )
                        {
                            Data.character_speed = Data.Damage_magic_speed[8];
                            Data.character_Ymove.y = Data.Damage_magic_Ymove[8];
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        }else
                        {
                            if (Data.character_NowHP > 0&& animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")==false) animator.Play("DizzyBeAttack");
                        }
                        
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
        }
        if (beKill == true)
        {
            if(Attackbox.transform.parent.transform.parent!=null)
            {
                if (Data.GC.GameSet == 0)
                {
                    Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                    if (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum >= 98)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                    }
                    Data.bekillnum++;
                }
                if (Data.DeadStep == 0) Data.DeadNow();
                if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                {
                    foreach (var team in Data.GC.setTeam)
                    {
                        if (Data.characterTeam == team)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            break;
                        }
                    }

                    foreach (var tag in Data.GC.setCharacter)
                    {
                        if (self.tag == tag)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            break;
                        }
                    }

                }
            }
            else
            {
                if (Attackbox.transform.parent.tag == "AB_Magic")
                {
                    if(Attackbox.tag=="AirAttack2"|| Attackbox.tag == "GroundAttack_Front")
                    {
                        M_AttackFront MAG = Attackbox.transform.parent.GetComponent<M_AttackFront>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[MAG.characterNumber].killnum++;
                            if (Data.GC.Character_Data[MAG.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }else if (Attackbox.tag == "GroundAttack_Defense")
                    {
                        GroundAttack_Defense MAG = Attackbox.transform.parent.GetComponent<GroundAttack_Defense>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[MAG.characterNumber].killnum++;
                            if (Data.GC.Character_Data[MAG.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    
                    if (Data.DeadStep == 0) Data.DeadNow();
                    if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                    {
                        foreach (var team in Data.GC.setTeam)
                        {
                            if (Data.characterTeam == team)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                        foreach (var tag in Data.GC.setCharacter)
                        {
                            if (self.tag == tag)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                    }
                }
            }
            
        }
    }
    void BeAttack_Gun(Collider Attackbox)
    {
        bool Defense_now = animator.GetCurrentAnimatorStateInfo(0).IsName("Defense");
        bool beKill = false;
        switch (Attackbox.transform.tag)
        {
            case "AirAttack":
                Grenade gaaa = Attackbox.transform.parent.GetComponent<Grenade>();
                if (Data.GC.Character_Data[gaaa.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[gaaa.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[gaaa.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[gaaa.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_gun_Break[0];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_gun[0];
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1) Knockcheck = 2;
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            Data.character_speed = Data.Damage_gun_speed[0];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[0];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_gun[0];
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1) Knockcheck = 2;
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Data.character_speed = Data.Damage_gun_speed[0];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[0];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                }
                    
                break;
            case "AirAttack2":
                G_AA2 gaa = Attackbox.transform.parent.GetComponent<G_AA2>();
                if (Data.GC.Character_Data[gaa.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[gaa.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[gaa.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[gaa.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_gun_Break[1];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_gun[1];
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_gun_speed[1];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[1];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_gun[1];
                        if (Data.character_NowHP <= 0) beKill = true;
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("HardMode") == false)
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_gun_speed[1];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[1];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                }
                   
                break;
            case "GroundAttack(1)":
                G_GA gga = Attackbox.transform.parent.GetComponent<G_GA>();
                if (Data.GC.Character_Data[gga.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[gga.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[gga.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    gga.amr.SetBool("OK", true);
                }
                break;
            case "GroundAttack(2)":
                G_GA ggaa = Attackbox.transform.parent.GetComponent<G_GA>();
                if (Data.GC.Character_Data[ggaa.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[ggaa.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[ggaa.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_gun_Break[2];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_gun[2];
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1) Knockcheck = 2;
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            Data.character_speed = Data.Damage_gun_speed[2];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[2];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_gun[2];
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1) Knockcheck = 2;
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Data.character_speed = Data.Damage_gun_speed[2];
                            Data.character_Ymove.y = Data.Damage_gun_Ymove[2];
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                }
                break;
            case "GroundAttack(3)":
                G_GA2D ggad = Attackbox.transform.parent.GetComponent<G_GA2D>();
                if (Data.GC.Character_Data[ggad.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                    && (Data.GC.Character_Data[ggad.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[ggad.characterNumber].characterTeam == 0)
                    && animator.GetBool("Dead") == false)
                {
                    ggad.animator.SetBool("OK", true);
                }
                break;
            case "GroundAttack_Front":
                G_GA_Front ggaf = Attackbox.transform.parent.GetComponent<G_GA_Front>();
                if (Data.GC.Character_Data[ggaf.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[ggaf.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[ggaf.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[ggaf.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_gun_Break[3];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_gun[3];
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            Data.character_speed = Data.Damage_gun_speed[3];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }

                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_gun[3];
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            animator.SetInteger("defense_LV", 0);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (animator.GetBool("InAir") )
                            {
                                animator.SetBool("BeAttack", true);
                                if (Knockcheck == 1) Knockcheck = 2;
                                Data.character_speed = Data.Damage_gun_speed[3];
                                Data.character_Ymove.y = Data.Damage_gun_Ymove[3];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else if(animator.GetBool("InAir")==false&& Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1) Knockcheck = 2;
                                animator.SetBool("BeAttack", true);
                                Data.character_speed = Data.Damage_gun_speed[3];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_gun_speed[3];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    Instantiate(ggaf.hiteffect, ggaf.transform.position, Quaternion.identity);
                    Destroy(ggaf.gameObject);
                }
                break;
            case "GroundAttack2":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_gun_Break[4];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_gun[4];
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_gun_speed[4];
                        Data.character_Ymove.y = Data.Damage_gun_Ymove[4];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_gun[4];
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_gun_speed[4];
                        Data.character_Ymove.y = Data.Damage_gun_Ymove[4];
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_gun_Break[5];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_gun[5];
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_gun_speed[5];
                        Data.character_Ymove.y = Data.Damage_gun_Ymove[5];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_gun[5];
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_speed = Data.Damage_gun_speed[5];
                        Data.character_Ymove.y = Data.Damage_gun_Ymove[5];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2_Defense":
                G_GA2D ggadd = Attackbox.transform.parent.GetComponent<G_GA2D>();
                if (Data.GC.Character_Data[ggadd.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[ggadd.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[ggadd.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[ggadd.characterNumber].characterNumber;
                    Data.character_NowHP -= Data.Damage_gun[6];
                    if (Data.character_NowHP <= 0) beKill = true;
                    animator.SetInteger("defense_LV", 0);
                    animator.SetBool("BeAttack", true);
                    if (Knockcheck == 1) Knockcheck = 2;
                    Data.character_speed = Data.Damage_gun_speed[6];
                    Data.character_Ymove.y = Data.Damage_gun_Ymove[6];
                    if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                    Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("PickUp"))
                    {
                        animator.SetBool("PickUp", false);
                        Data.Pickup_item_Script.item_script.bePickup = false;
                        Data.pickrangein = false;
                    }
                }
                break;
        }
        if (beKill == true)
        {
            if (Attackbox.transform.parent.transform.parent != null)
            {
                if (Data.GC.GameSet == 0)
                {
                    Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                    if (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum >= 98)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                    }
                    Data.bekillnum++;
                }
                if (Data.DeadStep == 0) Data.DeadNow();
                if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                {
                    foreach (var team in Data.GC.setTeam)
                    {
                        if (Data.characterTeam == team)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            break;
                        }
                    }

                    foreach (var tag in Data.GC.setCharacter)
                    {
                        if (self.tag == tag)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            break;
                        }
                    }

                }
            }
            else
            {
                if (Attackbox.transform.parent.tag == "AB_Gun")
                {
                    if (Attackbox.tag == "GroundAttack(2)")
                    {
                        G_GA ggaa = Attackbox.transform.parent.GetComponent<G_GA>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggaa.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggaa.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if (Attackbox.tag == "GroundAttack_Front")
                    {
                        G_GA_Front ggaf = Attackbox.transform.parent.GetComponent<G_GA_Front>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggaf.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggaf.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if (Attackbox.tag == "GroundAttack2_Defense")
                    {
                        G_GA2D ggadd = Attackbox.transform.parent.GetComponent<G_GA2D>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggadd.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggadd.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if(Attackbox.tag == "AirAttack")
                    {
                        Grenade gaaa = Attackbox.transform.parent.GetComponent<Grenade>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[gaaa.characterNumber].killnum++;
                            if (Data.GC.Character_Data[gaaa.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    if (Data.DeadStep == 0) Data.DeadNow();
                    if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                    {
                        foreach (var team in Data.GC.setTeam)
                        {
                            if (Data.characterTeam == team)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                        foreach (var tag in Data.GC.setCharacter)
                        {
                            if (self.tag == tag)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                    }
                }
            }

        }
    }
    void BeAttack_Assassin(Collider Attackbox)
    {
        bool Defense_now = animator.GetCurrentAnimatorStateInfo(0).IsName("Defense");
        bool beKill = false;
        switch (Attackbox.transform.tag)
        {
            case "AirAttack":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_assassin_Break[0];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_assassin[0];
                        Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_assassin_speed[0];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[0];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_assassin_speed[0];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_assassin[0];
                    Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("HardMode") == false)
                    {
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)
                            {
                                Knockcheck = 2;
                            }
                            Data.character_speed = Data.Damage_assassin_speed[0];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[0];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_assassin_speed[0];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "AirAttack2":
                A_AirAttack2 gaaa = Attackbox.transform.parent.GetComponent<A_AirAttack2>();
                if (Data.GC.Character_Data[gaaa.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[gaaa.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[gaaa.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[gaaa.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_assassin_Break[1];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_assassin[1];
                            Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetBool("BeAttack", true);
                            animator.SetInteger("defense_LV", 0);
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_assassin_speed[1];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[1];
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        if (Knockcheck == 1) Knockcheck = 2;
                        Data.character_NowHP -= Data.Damage_assassin[1];
                        Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            animator.SetBool("BeAttack", true);
                            Data.character_speed = Data.Damage_assassin_speed[1];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[1];
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                }
                break;
            case "GroundAttack(1)":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_assassin_Break[2];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_assassin[2];
                        Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (Data.character_NowHP > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false)
                        {
                            animator.Play("DizzyBeAttack");
                        }
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                        
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_assassin[2];
                    Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    if (animator.GetBool("HardMode") == false)
                    {
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 1)
                        {
                            Knockcheck = 2;
                        }
                        Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("PickUp"))
                        {
                            animator.SetBool("PickUp", false);
                            Data.Pickup_item_Script.item_script.bePickup = false;
                            Data.pickrangein = false;
                        }
                        if (animator.GetBool("InAir") )
                        {
                            Data.character_speed = Data.Damage_assassin_speed[2];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[2];
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                        }
                        else
                        {
                            if (Data.character_NowHP > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false)
                            {
                                animator.Play("DizzyBeAttack");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack_Defense":
                A_GD agd = Attackbox.transform.parent.GetComponent<A_GD>();
                if (Data.GC.Character_Data[agd.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[agd.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[agd.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[agd.characterNumber].characterNumber;
                    Data.character_NowHP -= Data.Damage_assassin[3];
                    if (Data.character_NowHP <= 0) beKill = true;
                    animator.SetInteger("defense_LV", 0);
                    animator.SetBool("BeAttack", true);
                    if (Knockcheck == 1) Knockcheck = 2;
                    if (animator.GetBool("PickUp"))
                    {
                        animator.SetBool("PickUp", false);
                        Data.Pickup_item_Script.item_script.bePickup = false;
                        Data.pickrangein = false;
                    }
                    Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("InAir") )
                    {
                        Data.character_speed = Data.Damage_assassin_speed[3];
                        Data.character_Ymove.y = Data.Damage_assassin_Ymove[3];
                        if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                    }
                    else
                    {
                        if (Data.character_NowHP > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) animator.Play("DizzyBeAttack");
                    }
                }
                break;
            case "GroundAttack_Front":
                if (Defense_now)
                {
                    if (animator.GetInteger("defense_LV") < defense_LVMAX)
                    {
                        int nowLV = animator.GetInteger("defense_LV");
                        nowLV += Data.Damage_assassin_Break[3];
                        animator.SetInteger("defense_LV", nowLV);
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        Data.character_NowHP -= Data.Damage_assassin[4];
                        Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_assassin_speed[4];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[4];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_assassin_speed[4];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                }
                else if (Defense_now == false)
                {
                    Data.character_NowHP -= Data.Damage_assassin[4];
                    Data.audiosource.PlayOneShot(Data.GC.effect[17]);
                    if (Data.character_NowHP <= 0) beKill = true;
                    Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    if (animator.GetBool("HardMode") == false)
                    {
                        if (Knockcheck == 0)
                        {
                            StartCoroutine("Knockreset");
                            Knockcheck = 1;
                        }
                        else
                        {
                            Knockcheck = 1;
                        }
                        animator.SetInteger("defense_LV", 0);
                        animator.SetBool("BeAttack", true);
                        if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                        {
                            if (Knockcheck == 1)
                            {
                                Knockcheck = 2;
                            }
                            Data.character_speed = Data.Damage_assassin_speed[4];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[4];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                        else
                        {
                            Data.character_speed = Data.Damage_assassin_speed[4];
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                            }
                        }
                    }
                    else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                }
                break;
            case "GroundAttack2":
                A_GA2 aga = Attackbox.transform.parent.GetComponent<A_GA2>();
                if (Data.GC.Character_Data[aga.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[aga.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[aga.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[aga.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_assassin_Break[4];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_assassin[5];
                            Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1) Knockcheck = 2;
                                Data.character_speed = Data.Damage_assassin_speed[5];
                                Data.character_Ymove.y = Data.Damage_assassin_Ymove[5];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_assassin_speed[5];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_assassin[5];
                        Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                        if (Data.character_NowHP <= 0) beKill = true;
                        Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        if (animator.GetBool("HardMode") == false)
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (animator.GetBool("InAir") || Knocknow >= Knocknum)
                            {
                                if (Knockcheck == 1)
                                {
                                    Knockcheck = 2;
                                }
                                Data.character_speed = Data.Damage_assassin_speed[5];
                                Data.character_Ymove.y = Data.Damage_assassin_Ymove[5];
                                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                                if (animator.GetBool("PickUp"))
                                {
                                    animator.SetBool("PickUp", false);
                                    Data.Pickup_item_Script.item_script.bePickup = false;
                                    Data.pickrangein = false;
                                }
                            }
                            else
                            {
                                Data.character_speed = Data.Damage_assassin_speed[5];
                                Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                                if (Data.character_NowHP > 0)
                                {
                                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(stand)")) animator.Play("BeAttack(stand)1");
                                    else if (Data.character_NowHP > 0) animator.Play("BeAttack(stand)");
                                }
                            }
                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                    Destroy(aga.gameObject);
                }
                break;
            case "GroundAttack2_Front":
                Data.character_NowHP -= Data.Damage_assassin[6];
                Data.audiosource.PlayOneShot(Data.GC.effect[15]);
                if (Data.character_NowHP <= 0) beKill = true;
                animator.SetInteger("defense_LV", 0);
                animator.SetBool("BeAttack", true);
                if (Knockcheck == 1) Knockcheck = 2;
                Data.character_speed = Data.Damage_assassin_speed[6];
                Data.character_Ymove.y = Data.Damage_assassin_Ymove[6];
                if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                if (animator.GetBool("PickUp"))
                {
                    animator.SetBool("PickUp", false);
                    Data.Pickup_item_Script.item_script.bePickup = false;
                    Data.pickrangein = false;
                }
                break;
            case "GroundAttack2_Defense":
                A_GroundAttack2Defense agad = Attackbox.transform.parent.GetComponent<A_GroundAttack2Defense>();
                if (Data.GC.Character_Data[ agad.characterNumber].tag != self.tag && animator.GetBool("Invincible") == false
                && (Data.GC.Character_Data[agad.characterNumber].characterTeam != Data.characterTeam || Data.GC.Character_Data[agad.characterNumber].characterTeam == 0)
                && animator.GetBool("Dead") == false)
                {
                    Data.FinalHit_Num = Data.GC.Character_Data[agad.characterNumber].characterNumber;
                    if (Defense_now)
                    {
                        if (animator.GetInteger("defense_LV") < defense_LVMAX)
                        {
                            int nowLV = animator.GetInteger("defense_LV");
                            nowLV += Data.Damage_assassin_Break[5];
                            animator.SetInteger("defense_LV", nowLV);
                            Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Data.character_NowHP -= Data.Damage_assassin[7];
                            if (Data.character_NowHP <= 0) beKill = true;
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            if (Knockcheck == 1) Knockcheck = 2;
                            Data.character_speed = Data.Damage_assassin_speed[7];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[7];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }
                        }
                    }
                    else if (Defense_now == false)
                    {
                        Data.character_NowHP -= Data.Damage_assassin[7];
                        if (Data.character_NowHP <= 0) beKill = true;
                        if (animator.GetBool("HardMode") == false)
                        {
                            if (Knockcheck == 0)
                            {
                                StartCoroutine("Knockreset");
                                Knockcheck = 1;
                            }
                            else
                            {
                                Knockcheck = 1;
                            }
                            animator.SetInteger("defense_LV", 0);
                            animator.SetBool("BeAttack", true);
                            if (Knockcheck == 1)
                            {
                                Knockcheck = 2;
                            }
                            Data.character_speed = Data.Damage_assassin_speed[7];
                            Data.character_Ymove.y = Data.Damage_assassin_Ymove[7];
                            Instantiate(Data.Character_Effect_Prefab[1], Data.Effectcenter.transform.position, Quaternion.identity);
                            if (Data.character_NowHP > 0) animator.Play("BeAttack(Knock)_start");
                            if (animator.GetBool("PickUp"))
                            {
                                animator.SetBool("PickUp", false);
                                Data.Pickup_item_Script.item_script.bePickup = false;
                                Data.pickrangein = false;
                            }

                        }
                        else Instantiate(Data.Character_Effect_Prefab[0], Data.Effectcenter.transform.position, Quaternion.identity);
                    }
                }
                break;
        }
        if (beKill == true)
        {
            if (Attackbox.transform.parent.transform.parent != null)
            {
                if (Data.GC.GameSet == 0)
                {
                    Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum++;
                    if (Attackbox.transform.parent.transform.parent.GetComponent<CharacterData>().killnum >= 98)
                    {
                        Data.GC.GameSet = 2;
                        Cam.gameset_target[0] = self;
                        Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                    }
                    Data.bekillnum++;
                }
                if (Data.DeadStep == 0) Data.DeadNow();
                if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                {
                    foreach (var team in Data.GC.setTeam)
                    {
                        if (Data.characterTeam == team)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            break;
                        }
                    }

                    foreach (var tag in Data.GC.setCharacter)
                    {
                        if (self.tag == tag)
                        {
                            Data.GC.GameSet = 2;
                            Cam.gameset_target[0] = self;
                            Cam.gameset_target[1] = Attackbox.transform.parent.transform.parent.gameObject;
                            Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            break;
                        }
                    }

                }
            }
            else
            {
                if (Attackbox.transform.parent.tag == "AB_Assassin")
                {
                    if (Attackbox.tag == "GroundAttack2_Defense")
                    {
                        A_GroundAttack2Defense ggadd = Attackbox.transform.parent.GetComponent<A_GroundAttack2Defense>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggadd.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggadd.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if (Attackbox.tag == "GroundAttack_Defense")
                    {
                        A_GD ggadd = Attackbox.transform.parent.GetComponent<A_GD>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggadd.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggadd.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if (Attackbox.tag == "GroundAttack2")
                    {
                        A_GA2 ggadd = Attackbox.transform.parent.GetComponent<A_GA2>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[ggadd.characterNumber].killnum++;
                            if (Data.GC.Character_Data[ggadd.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    else if (Attackbox.tag == "AirAttack2")
                    {
                        A_AirAttack2 gaaa = Attackbox.transform.parent.GetComponent<A_AirAttack2>();
                        if (Data.GC.GameSet == 0)
                        {
                            Data.GC.Character_Data[gaaa.characterNumber].killnum++;
                            if (Data.GC.Character_Data[gaaa.characterNumber].killnum >= 98)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                            }
                            Data.bekillnum++;
                        }
                    }
                    if (Data.DeadStep == 0) Data.DeadNow();
                    if (Data.GC.GameSet_Already[0] == true && Data.GC.GameSet == 0)
                    {
                        foreach (var team in Data.GC.setTeam)
                        {
                            if (Data.characterTeam == team)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                        foreach (var tag in Data.GC.setCharacter)
                        {
                            if (self.tag == tag)
                            {
                                Data.GC.GameSet = 2;
                                Cam.gameset_target[0] = self;
                                Cam.gameset_target[1] = self;
                                Data.audiosource.PlayOneShot(Data.GC.effect[7]);
                                break;
                            }
                        }

                    }
                }
            }
           
        }
    }
}
