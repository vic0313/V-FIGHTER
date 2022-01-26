using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterData : MonoBehaviour
{
    public GameControler GC;
    public CharacterControler_ALL ccall;
    public CharacterController cc;
    public AudioSource audiosource;
    //ゲームの変数======================================================================================
    //現在ゲームの職業の数量
    public int Sword_num;
    public int Pounch_num;
    public int Magic_num;
    public int Gun_num;
    public int Assassin_num;
    //キャラクタの変数==================================================================================
    public GameObject[] charactermodel;
    public int characterTeam;                       //キャラのチーム
    public int characterNumber;
    public int character_Profession;                //キャラの職業
    public int character_MAX;                       //キャラの残機MAX
    public int character_NOW;                       //キャラ現在の残機
    public float character_MaxHP;                   //キャラのMAXHP
    public float character_NowHP;                   //キャラ現在のHP
    public float character_MaxMP;                   //キャラのMAXMP
    public float character_NowMP;                   //キャラ現在のMP
    public float character_jumpHeight;              //キャラがジャンプの高さ
    public float nor_speed;                         //キャラの基本移動速
    public float character_speed;                   //キャラ移動の速さ
    public float character_sprint_speed;            //キャラダッシュの速さ
    public float character_sprint_distance;            //キャラダッシュの長さ
    public float character_action_mathf;         //キャラダッシュ計算用変数
    public float character_action_mathf_max;     //キャラダッシュ計算用変数2
    public float character_gravity;        //重力
    public float character_input_V;                 //キャラの移動量(Z)
    public float character_input_H;                 //キャラの移動量(X)
    public float character_angle;                   //キャラの移動角度
    public Vector3 RebornPoint;                     //キャラの再生位置
    public Vector3 character_Ymove;                 //キャラのY移動量
    public Vector3 character_XZmove;                //キャラのXZ移動量
    public int killnum;
    public int bekillnum;
    public int DeadStep;
    public int Character_Point;
    public int DeadNum;
    public int RebonTime;
    public player_groundUI PGUI;
    public int FinalHit_Num;
    //アイテムと拾いを関わった変数==================================================================================
    public GameObject Hand;                         //手の位置OBJ
    public GameObject Pickup_item;                  //現在拾ったアイテム
    public ITEM_Type Pickup_item_Script;            //現在拾ったアイテムのSCRIPT
    public bool pickrangein;                  //キャラがアイテムを拾い可能の範囲の判定
    public int pickup_already;                      //拾った→使用の遅れのために使用した変数
    public GameObject[] Item_Effect_Prefab;         //アイテムと触れ合いエフェクトの物件
    public GameObject PickItem6;                    //アイテム6を拾った時、手のアイテム描画、を使用する物件
    public GameObject PickItem7;                    //アイテム7を拾った時、手のアイテム描画、を使用する物件
    public GameObject Effect_Item6_POS;             //アイテム6の攻撃の位置
    public int Slowdown;                            //アイテム4の効果判定の変数
    public int Mushroom;                            //アイテム5の効果判定の変数
    public int Item10_fire;                         //アイテム10の効果判定の変数
    //エフェクトを関わった変数==================================================================================
    public GameObject Effectcenter;                 //キャラの中心点
    public GameObject[] Character_Effect_Prefab;    //キャラが使用するエフェクトの物件
    //public GameObject[] Profession_Effect_Prefab;   //職業によって使用するエフェクトの物件
    //職業によって変数==================================================================================
    public Vector3 AttackWay;
    //SWORD
    public float[] MP_sword;               //技の使用MP
    public float[] Damage_sword;          //技のダメージ
    public float[] Damage_sword_speed;   //技を受ける時、相手の移動速さ
    public float[] Damage_sword_Ymove ;  //技を受ける時、相手のY移動量
    public int[] Damage_sword_Break ;      //相手は防御中の時、技の防御量減らすの量
    //pouch
    public float[] MP_pouch ;               //技の使用MP
    public float[] Damage_pouch ;          //技のダメージ
    public float[] Damage_pouch_speed ;   //技を受ける時、相手の移動速さ
    public float[] Damage_pouch_Ymove ;  //技を受ける時、相手のY移動量
    public int[] Damage_pouch_Break ;      //相手は防御中の時、技の防御量減らすの量
    public float character_Attack1Front_speed;                    //キャラAttack1Frontの速さ
    public float character_Attack1Front_distance;
    public float character_AirAttack1_sprint;             //キャラダッシュの速さ
    //magic
    public float[] MP_magic ;               //技の使用MP
    public float[] Damage_magic;          //技のダメージ
    public float[] Damage_magic_speed;   //技を受ける時、相手の移動速さ
    public float[] Damage_magic_Ymove;  //技を受ける時、相手のY移動量
    public int[] Damage_magic_Break;      //相手は防御中の時、技の防御量減らすの量
    public bool M_Sprintnow;
    //gun
    public float[] MP_gun;               //技の使用MP
    public float[] Damage_gun;          //技のダメージ
    public float[] Damage_gun_speed;   //技を受ける時、相手の移動速さ
    public float[] Damage_gun_Ymove;  //技を受ける時、相手のY移動量
    public int[] Damage_gun_Break;      //相手は防御中の時、技の防御量減らすの量
    public float shooting_angle;
    public bool AirAttack2_use;
    //assassin
    public float[] MP_assassin;               //技の使用MP
    public float[] Damage_assassin;          //技のダメージ
    public float[] Damage_assassin_speed;   //技を受ける時、相手の移動速さ
    public float[] Damage_assassin_Ymove;  //技を受ける時、相手のY移動量
    public int[] Damage_assassin_Break;      //相手は防御中の時、技の防御量減らすの量
    public float character_GAD_speed;                    //キャラAttackDefenseの速さ
    public float character_GAD_distance;
    public bool JumpTwo;
    public float JumpTwo_speed;
    public bool A_GAD2;
    public bool A_GAF;
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
        
        ReSetCharacter();
        
        SetSword_All();
        SetPouch_All();
        SetMagic_All();
        SetGun_All();
        SetAssassin_All();
    }

    // Update is called once per frame
    void Update()
    {
        if(GC.Game_Mp==1)
        {
            character_NowMP = character_MaxMP;
        }
        if(character_NowHP<=0 && DeadStep==0)
        {
            DeadNow();
        }
        if(DeadStep!=0) DeadStep_animation();
        killnum=GC.GI.MAXandMin(killnum,0, 99);
        bekillnum = GC.GI.MAXandMin(bekillnum, 0, 99);
        Character_Point = GC.GI.MAXandMin(Character_Point, 0, 99);
    }

    void ReSetCharacter()
    {
        pickup_already = 0;
        if(GC.Gamestate < 2)
        {
            character_MAX = GC.GI.player_life_max;
            character_NOW = character_MAX;
            DeadStep = 0;
            killnum = 0;
            bekillnum = 0;
            character_MaxHP = 100;
            character_NowHP = character_MaxHP;
            character_MaxMP = 100;
            character_NowMP = character_MaxMP;
            Character_Point = 0;
            DeadNum = 5;
        }
        else
        {
            character_NowHP = character_MaxHP;
            character_NowMP = character_MaxMP;
        }
        character_gravity = -9.81f;
        character_action_mathf_max = 2;
        character_action_mathf = character_action_mathf_max;
        Slowdown = 0;
        Mushroom = 0;
        Item10_fire = 0;
        pickrangein = false;
        RebonTime = 5;
        FinalHit_Num = -1;
        switch (character_Profession)
        {
            case (int)Profession.Sword:
                SetSword();
                break;
            case (int)Profession.Pounch:
                SetPouch();
                break;
            case (int)Profession.Magic:
                SetMagic();
                break;
            case (int)Profession.Gun:
                SetGun();
                break;
            case (int)Profession.Assassin:
                SetAssassin();
                break;
        }
    }
    void DeadStep_animation()
    {
        switch (DeadStep)
        {
            case 1:
                //死の動画
                break;
            case 2:
                //死の動画途中
                Instantiate(Character_Effect_Prefab[4], gameObject.transform.position, Effect_Item6_POS.transform.rotation);
                DeadStep = 3;
                break;
            case 3:
                
                break;
            case 4:
                //死の動画終了
                for (int i = 0; i < charactermodel.Length; i++) charactermodel[i].SetActive(false);
                if (gameObject.transform.position.y < 0||Mathf.Abs(gameObject.transform.position.x)>13)
                {
                    gameObject.transform.position = GC.Rebron_pos[Random.Range(0, 16)].position;
                }
                DeadStep = 5;
                break;
            case 5:
                if (character_NOW > 0&& DeadNum!=0)
                {
                    DeadStep = 6;
                    PGUI.gameObject.SetActive(true);
                }
                break;
            case 6:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, (PGUI.transform.position.y + 3f), gameObject.transform.position.z);
                InvokeRepeating("RebonTime_change", 1, 1);
                DeadStep=7;
                break;
            case 7:
                if (RebonTime <= 0) Rebon();
                break;
            case 8:
                //Zombiemode用
                PGUI.gameObject.SetActive(false);
                break;
        }
        if(GC.Gamemode_num == 0 && GC.Gamemode2_num == 1&&characterTeam!=0&&DeadStep>4)
        {
            if(GC.DeadTeam[characterTeam-1])
            {
                if (DeadStep == 7) CancelInvoke("RebonTime_change");
                DeadStep = 8;
            }
        }
        if(GC.GameSet>0&&DeadStep==7) CancelInvoke("RebonTime_change");
    }
    public void DeadNow()
    {
        DeadStep = 1;
        //判断重生
        if (character_MAX != 4)
        {
            if (GC.Gamemode_num == 0)
            {
                if (GC.Gamemode2_num != 1)
                {
                    character_NOW--;
                }
            }
            else
            {
                character_NOW--;
            }
        }
        DeadNum--;
        character_NowHP = 0;
        gameObject.GetComponent<Animator>().Play("Dead");
        Instantiate(Character_Effect_Prefab[2], gameObject.transform.position, Quaternion.identity);
        gameObject.GetComponent<Animator>().SetBool("Dead", true);
        gameObject.GetComponent<CharacterController>().enabled = false;
        GC.audiosource.PlayOneShot(GC.effect[18]);
        Invoke("DeadStepDebug",2.7f);
    }
    void DeadStep_Up()
    {
        if(DeadStep==1)
        {
            DeadStep = 2;
        }else if (DeadStep == 3)
        {
            DeadStep = 4;
        }else
        {
            DeadStep = 4;
        }
    }
    void DeadStepDebug()
    {
        if (DeadStep < 4) DeadStep=4;
    }
    void Rebon()
    {
        PGUI.gameObject.SetActive(false);
        gameObject.GetComponent<Animator>().SetBool("Dead", false);
        gameObject.GetComponent<Animator>().Play("Jump_Air");
        gameObject.GetComponent<Animator>().SetBool("Invincible", true);
        gameObject.GetComponent<CharacterController>().enabled = true;
        for (int i = 0; i < charactermodel.Length; i++) charactermodel[i].SetActive(true);
        CancelInvoke("RebonTime_change");
        Invoke("RebonInvincible", 2);
        ReSetCharacter();
        if (GC.Gamemode_num == 0 && GC.Gamemode2_num == 1) character_NowHP = character_MaxHP * (((float)DeadNum) / 5);
        DeadStep = 0;
        character_Ymove.y = 1f;
        Instantiate(Character_Effect_Prefab[5], gameObject.transform.position, gameObject.transform.rotation);
    }
    void RebonTime_change()
    {
        RebonTime--;
    }
    void RebonInvincible()
    {
        gameObject.GetComponent<Animator>().SetBool("Invincible", false);
    }
    void SetSword()
    {
        MP_sword = new float[7];
        character_jumpHeight = 2.0f;
        character_speed = 5.0f;
        character_sprint_speed = 12.0f;
        character_sprint_distance = 4f;
        nor_speed = 3.0f;
        //MP
        MP_sword[0] = 0.2f;     //0.GroundAttack_Defense
        MP_sword[1] = 0.2f;    //1.GroundAttack_Front
        MP_sword[2] = 0.3f;     //2.GroundAttack2
        MP_sword[3] = 0.2f;     //3.GroundAttack2_Front
        MP_sword[4] = 0.2f;     //4.GroundAttack2_Defense
        MP_sword[5] = 0.3f;     //5.AirAttack2
        MP_sword[6] = 0.25f;     //6.Sprint
    }
    void SetSword_All()
    {
        Damage_sword = new float[10];
        Damage_sword_speed = new float[15];
        Damage_sword_Ymove = new float[10];
        Damage_sword_Break = new int[10];
        //ATK
        Damage_sword[0] = (0.05f * character_MaxHP);    //0.AirAttack
        Damage_sword[1] = (0.1f * character_MaxHP);     //1.AirAttack2
        Damage_sword[2] = (0.03f * character_MaxHP);    //2.GroundAttack(1)
        Damage_sword[3] = (0.05f * character_MaxHP);    //3.GroundAttack(2)
        Damage_sword[4] = (0.07f * character_MaxHP);    //4.GroundAttack(3)
        Damage_sword[5] = (0.07f * character_MaxHP);    //5.GroundAttack_Defense
        Damage_sword[6] = (0.04f * character_MaxHP);    //6.GroundAttack_Front
        Damage_sword[7] = (0.08f * character_MaxHP);    //7.GroundAttack2
        Damage_sword[8] = (0.06f * character_MaxHP);    //8.GroundAttack2_Front
        Damage_sword[9] = (0.06f * character_MaxHP);    //9.GroundAttack2_Defense
        //Break ATK
        Damage_sword_Break[0] = 1;    //0.AirAttack
        Damage_sword_Break[1] = 2;    //1.AirAttack2
        Damage_sword_Break[2] = 1;    //2.GroundAttack(1)
        Damage_sword_Break[3] = 1;    //3.GroundAttack(2)
        Damage_sword_Break[4] = 2;    //4.GroundAttack(3)
        Damage_sword_Break[5] = 2;    //6.GroundAttack_Front
        Damage_sword_Break[6] = 2;    //7.GroundAttack2
        Damage_sword_Break[7] = 2;    //8.GroundAttack2_Front
        Damage_sword_Break[8] = 2;    //9.GroundAttack2_Defense
        //BeATK Speed
        Damage_sword_speed[0] = 2.0f;        //0.AirAttack
        Damage_sword_speed[1] = 2.0f;       //0.AirAttack Knock
        Damage_sword_speed[2] = 0.0f;       //1.AirAttack2 InAir
        Damage_sword_speed[3] = 5.0f;       //1.AirAttack2 NotInAir
        Damage_sword_speed[4] = 3.0f;       //2.GroundAttack(1) InAir
        Damage_sword_speed[5] = 2.0f;       //2.GroundAttack(1) NotInAir
        Damage_sword_speed[6] = 3.0f;       //2.GroundAttack(2) InAir
        Damage_sword_speed[7] = 2.0f;       //2.GroundAttack(2) NotInAir
        Damage_sword_speed[8] = 2.0f;       //2.GroundAttack(3) 
        Damage_sword_speed[9] = 0.5f;       //3.GroundAttack_Defense
        Damage_sword_speed[10] = 3.0f;      //4.GroundAttack_Front InAir
        Damage_sword_speed[11] = 2.0f;      //4.GroundAttack_Front NotInAir 
        Damage_sword_speed[12] = 5.0f;      //7.GroundAttack2
        Damage_sword_speed[13] = 1.0f;      //8.GroundAttack2_Front
        Damage_sword_speed[14] = 2.0f;      //9.GroundAttack2_Defense
        //BeATK Ymove
        Damage_sword_Ymove[0] = -Mathf.Sqrt((2.0f) * -5.0f * (character_gravity));    //0.AirAttack2 InAir
        Damage_sword_Ymove[1] = 0.0f;                                                               //1.AirAttack2 NotInAir 
        Damage_sword_Ymove[2] = Mathf.Sqrt((0.5f) * -3.0f * (character_gravity));                   //3.GroundAttack(1) InAir
        Damage_sword_Ymove[3] = Mathf.Sqrt((0.5f) * -3.0f * (character_gravity));                   //3.GroundAttack(2) InAir
        Damage_sword_Ymove[4] = Mathf.Sqrt((0.5f) * -3.0f * (character_gravity));                   //3.GroundAttack(3) InAir
        Damage_sword_Ymove[5] = Mathf.Sqrt((0.5f) * -10.0f * (character_gravity));                  //4.GroundAttack_Defense
        Damage_sword_Ymove[6] = Mathf.Sqrt((0.5f) * -3.0f * (character_gravity));                   //5.GroundAttack_Front InAir
        Damage_sword_Ymove[7] = Mathf.Sqrt((0.5f) * -2.0f * (character_gravity));                   //7.GroundAttack2
        Damage_sword_Ymove[8] = Mathf.Sqrt((0.5f) * -10.0f * (character_gravity));                  //8.GroundAttack2_Front
        Damage_sword_Ymove[9] = Mathf.Sqrt((0.5f) * -7.0f * (character_gravity));                   //9.GroundAttack2_Defense
    }
    void SetPouch()
    {
        MP_pouch = new float[8];
        character_jumpHeight = 2.0f;
        character_speed = 5.0f;
        character_sprint_speed = 12.0f;
        character_sprint_distance = 4f;
        nor_speed = 3.0f;
        character_Attack1Front_speed = 12.0f;
        character_AirAttack1_sprint = 12;
        character_Attack1Front_distance = 5f;
        //MP
        MP_pouch[0] = 0.2f;     //0.GroundAttack_Defense
        MP_pouch[1] = 0.2f;    //1.GroundAttack_Front
        MP_pouch[2] = 0.4f;     //2.GroundAttack2
        MP_pouch[3] = 0.2f;     //3.GroundAttack2_Front
        MP_pouch[4] = 0.2f;     //4.GroundAttack2_Defense
        MP_pouch[5] = 0.2f;     //5.AirAttack
        MP_pouch[6] = 0.3f;     //5.AirAttack2
        MP_pouch[7] = 0.25f;     //6.Sprint
        
    }
    void SetPouch_All()
    {
        Damage_pouch = new float[10];
        Damage_pouch_speed = new float[10];
        Damage_pouch_Ymove = new float[10];
        Damage_pouch_Break = new int[10];
        //ATK
        Damage_pouch[0] = (0.05f * character_MaxHP);    //0.AirAttack
        Damage_pouch[1] = (0.1f * character_MaxHP);     //1.AirAttack2
        Damage_pouch[2] = (0.03f * character_MaxHP);    //2.GroundAttack(1)
        Damage_pouch[3] = (0.05f * character_MaxHP);    //3.GroundAttack(2)
        Damage_pouch[4] = (0.07f * character_MaxHP);    //4.GroundAttack(3)
        Damage_pouch[5] = (0.07f * character_MaxHP);    //5.GroundAttack_Defense
        Damage_pouch[6] = (0.04f * character_MaxHP);    //6.GroundAttack_Front
        Damage_pouch[7] = (0.08f * character_MaxHP);    //7.GroundAttack2
        Damage_pouch[8] = (0.06f * character_MaxHP);    //8.GroundAttack2_Front
        Damage_pouch[9] = (0f * character_MaxHP);    //9.GroundAttack2_Defense
        //Break ATK
        Damage_pouch_Break[0] = 1;    //0.AirAttack
        Damage_pouch_Break[1] = 2;    //1.AirAttack2
        Damage_pouch_Break[2] = 1;    //2.GroundAttack(1)
        Damage_pouch_Break[3] = 1;    //3.GroundAttack(2)
        Damage_pouch_Break[4] = 2;    //4.GroundAttack(3)
        Damage_pouch_Break[5] = 2;    //6.GroundAttack_Front
        Damage_pouch_Break[6] = 2;    //7.GroundAttack2
        Damage_pouch_Break[7] = 2;    //8.GroundAttack2_Front
        Damage_pouch_Break[8] = 2;    //9.GroundAttack2_Defense
        //BeATK Speed
        Damage_pouch_speed[0] = 3.0f;        //0.AirAttack
        Damage_pouch_speed[1] = 4.0f;       //0.AirAttack2
        Damage_pouch_speed[2] = 2.0f;       //2.GroundAttack(1)
        Damage_pouch_speed[3] = 2.0f;       //2.GroundAttack(2)
        Damage_pouch_speed[4] = 1f;         //2.GroundAttack(3) 
        Damage_pouch_speed[5] = 5f;         //3.GroundAttack_Defense
        Damage_pouch_speed[6] = 2.0f;       //GroundAttack_Front InAir
        Damage_pouch_speed[7] = 0f;       //2.GroundAttack2 
        Damage_pouch_speed[8] = 0f;      //GroundAttack2_Front
        Damage_pouch_speed[9] = 3.0f;      //GroundAttack2_Defense
        //BeATK Ymove
        Damage_pouch_Ymove[0] = Mathf.Sqrt((-1f) * (character_gravity));    //0.AirAttack1
        Damage_pouch_Ymove[1] = Mathf.Sqrt((-2.0f) * (character_gravity));    //1.AirAttack2 
        Damage_pouch_Ymove[2] = Mathf.Sqrt((-1.5f) * (character_gravity));                   //3.GroundAttack(1) 
        Damage_pouch_Ymove[3] = Mathf.Sqrt((-1.5f) * (character_gravity));                   //3.GroundAttack(2) 
        Damage_pouch_Ymove[4] = Mathf.Sqrt((-2.5f) * (character_gravity));                   //3.GroundAttack(3) 
        Damage_pouch_Ymove[5] = Mathf.Sqrt((-0.2f) * (character_gravity));                  //4.GroundAttack_Defense
        Damage_pouch_Ymove[6] = Mathf.Sqrt((-1.5f) * (character_gravity));                   //5.GroundAttack_Front InAir
        Damage_pouch_Ymove[7] = Mathf.Sqrt((-7f) * (character_gravity));                   //7.GroundAttack2
        Damage_pouch_Ymove[8] = Mathf.Sqrt((-5f) * (character_gravity));                  //8.GroundAttack2_Front
        Damage_pouch_Ymove[9] = Mathf.Sqrt((-0.2f) * (character_gravity));                   //9.GroundAttack2_Defense
    }
    void SetMagic()
    {
        MP_magic = new float[8];
        character_jumpHeight = 2.0f;
        character_speed = 5.0f;
        character_sprint_speed = 5.0f;
        M_Sprintnow = false;
        nor_speed = 3.0f;
        //MP
        MP_magic[0] = 0.2f;     //0.GroundAttack_Defense
        MP_magic[1] = 0.15f;    //1.GroundAttack_Front
        MP_magic[2] = 0.3f;     //2.GroundAttack2
        MP_magic[3] = 0.3f;     //3.GroundAttack2_Front
        MP_magic[4] = 0.3f;     //4.GroundAttack2_Defense
        MP_magic[5] = 0.2f;     //5.AirAttack1
        MP_magic[6] = 0.15f;     //5.AirAttack2
        MP_magic[7] = 0.35f;     //6.Sprint
    }
    void SetMagic_All()
    {
        Damage_magic = new float[9];
        Damage_magic_speed = new float[9];
        Damage_magic_Ymove = new float[9];
        Damage_magic_Break = new int[9];
        //ATK
        Damage_magic[0] = (0.08f * character_MaxHP);    //0.AirAttack
        Damage_magic[1] = (0.06f * character_MaxHP);     //1.AirAttack2
        Damage_magic[2] = (0.08f * character_MaxHP);    //2.GroundAttack(1)
        Damage_magic[3] = (0.08f * character_MaxHP);    //3.GroundAttack(2)
        Damage_magic[4] = (0.08f * character_MaxHP);    //5.GroundAttack_Defense
        Damage_magic[5] = (0.04f * character_MaxHP);    //6.GroundAttack_Front
        Damage_magic[6] = (0.12f * character_MaxHP);    //7.GroundAttack2
        Damage_magic[7] = (0.05f * character_MaxHP);    //8.GroundAttack2_Front
        Damage_magic[8] = (0.05f * character_MaxHP);    //9.GroundAttack2_Defense
        //Break ATK
        Damage_magic_Break[0] = 1;    //0.AirAttack
        Damage_magic_Break[1] = 2;    //1.AirAttack2
        Damage_magic_Break[2] = 1;    //2.GroundAttack(1)
        Damage_magic_Break[3] = 1;    //3.GroundAttack(2)
        Damage_magic_Break[4] = 2;    //6.GroundAttack_Front
        Damage_magic_Break[5] = 2;    //7.GroundAttack2
        Damage_magic_Break[6] = 2;    //8.GroundAttack2_Front
        Damage_magic_Break[7] = 2;    //9.GroundAttack2_Defense
        //BeATK Speed
        Damage_magic_speed[0] = 3.0f;        //0.AirAttack
        Damage_magic_speed[1] = 3.0f;       //0.AirAttack2
        Damage_magic_speed[2] = 4.0f;       //2.GroundAttack(1)
        Damage_magic_speed[3] = 5.0f;       //2.GroundAttack(2)
        Damage_magic_speed[4] = 5f;         //3.GroundAttack_Defense
        Damage_magic_speed[5] = 2.0f;       //GroundAttack_Front InAir
        Damage_magic_speed[6] = 4f;       //2.GroundAttack2 
        Damage_magic_speed[7] = 5f;      //GroundAttack2_Front
        Damage_magic_speed[8] = 3f;    //9.GroundAttack2_DefenseINAir
        //BeATK Ymove
        Damage_magic_Ymove[0] = Mathf.Sqrt((-1f) * (character_gravity));    //0.AirAttack1
        Damage_magic_Ymove[1] = Mathf.Sqrt((-1.0f) * (character_gravity));    //1.AirAttack2 
        Damage_magic_Ymove[2] = Mathf.Sqrt((-2f) * (character_gravity));                   //3.GroundAttack(1) 
        Damage_magic_Ymove[3] = Mathf.Sqrt((-3f) * (character_gravity));                   //3.GroundAttack(2) 
        Damage_magic_Ymove[4] = Mathf.Sqrt((-3f) * (character_gravity));                  //4.GroundAttack_Defense
        Damage_magic_Ymove[5] = Mathf.Sqrt((-1.5f) * (character_gravity));                   //5.GroundAttack_Front InAir
        Damage_magic_Ymove[6] = Mathf.Sqrt((-7f) * (character_gravity));                   //7.GroundAttack2
        Damage_magic_Ymove[7] = Mathf.Sqrt((-3f) * (character_gravity));                  //8.GroundAttack2_Front
        Damage_magic_Ymove[8] = Mathf.Sqrt((-1f) * (character_gravity));    //9.GroundAttack2_Defense INAir

    }
    void SetGun()
    {
        MP_gun = new float[9];
        character_jumpHeight = 2.0f;
        character_speed = 5.0f;
        character_sprint_speed = 12.0f;
        character_sprint_distance = 4f;
        M_Sprintnow = false;
        nor_speed = 3.0f;
        shooting_angle = transform.eulerAngles.y;
        AirAttack2_use=false;
        //MP
        MP_gun[0] = 0.2f;     //0.GroundAttack
        MP_gun[1] = 0.2f;     //1.GroundAttack_Defense
        MP_gun[2] = 0.01f;    //2.GroundAttack_Front
        MP_gun[3] = 0.2f;     //3.GroundAttack2
        MP_gun[4] = 0.25f;     //4.GroundAttack2_Front
        MP_gun[5] = 0.4f;     //5.GroundAttack2_Defense
        MP_gun[6] = 0.2f;     //6.AirAttack1
        MP_gun[7] = 0.3f;     //7.AirAttack2
        MP_gun[8] = 0.15f;     //8.Sprint
    }
    void SetGun_All()
    {
        Damage_gun = new float[7];
        Damage_gun_speed = new float[7];
        Damage_gun_Ymove = new float[7];
        Damage_gun_Break = new int[6];
        //ATK
        Damage_gun[0] = (0.06f * character_MaxHP);     //1.AirAttack1
        Damage_gun[1] = (0.07f * character_MaxHP);     //1.AirAttack2
        Damage_gun[2] = (0.06f * character_MaxHP);    //2.GroundAttack(2)
        Damage_gun[3] = (0.03f * character_MaxHP);    //6.GroundAttack_Front
        Damage_gun[4] = (0.07f * character_MaxHP);    //7.GroundAttack2
        Damage_gun[5] = (0.08f * character_MaxHP);    //8.GroundAttack2_Front
        Damage_gun[6] = (0.1f * character_MaxHP);    //9.GroundAttack2_Defense
        //Break ATK
        Damage_gun_Break[0] = 2;    //1.AirAttack1
        Damage_gun_Break[1] = 2;    //1.AirAttack2
        Damage_gun_Break[2] = 1;    //2.GroundAttack(2)
        Damage_gun_Break[3] = 2;    //6.GroundAttack_Front
        Damage_gun_Break[4] = 2;    //7.GroundAttack2
        Damage_gun_Break[5] = 2;    //8.GroundAttack2_Front
        //BeATK Speed
        Damage_gun_speed[0] = 3.0f;       //0.AirAttack1
        Damage_gun_speed[1] = 3.0f;       //0.AirAttack2
        Damage_gun_speed[2] = 4.0f;       //2.GroundAttack(2)
        Damage_gun_speed[3] = 2.0f;       //GroundAttack_Front 
        Damage_gun_speed[4] = 4f;       //2.GroundAttack2 
        Damage_gun_speed[5] = 4f;      //GroundAttack2_Front
        Damage_gun_speed[6] = 2;    //9.GroundAttack2_Defense
        //BeATK Ymove
        Damage_gun_Ymove[0] = Mathf.Sqrt((-2.0f) * (character_gravity));    //1.AirAttack1 
        Damage_gun_Ymove[1] = Mathf.Sqrt((-1.0f) * (character_gravity));    //1.AirAttack2 
        Damage_gun_Ymove[2] = Mathf.Sqrt((-3f) * (character_gravity));                   //3.GroundAttack(2) 
        Damage_gun_Ymove[3] = Mathf.Sqrt((-1.5f) * (character_gravity));                   //5.GroundAttack_Front InAir
        Damage_gun_Ymove[4] = Mathf.Sqrt((-3f) * (character_gravity));                   //7.GroundAttack2
        Damage_gun_Ymove[5] = Mathf.Sqrt((-3f) * (character_gravity));                  //8.GroundAttack2_Front
        Damage_gun_Ymove[6] = Mathf.Sqrt((-7f) * (character_gravity));                  //4.GroundAttack2_Defense
    }

    void SetAssassin()
    {
        MP_assassin = new float[8];
        character_jumpHeight = 2.0f;
        character_speed = 5.0f;
        character_sprint_speed = 12.0f;
        character_sprint_distance = 4f;
        nor_speed = 3.0f;
        character_Attack1Front_speed = 12.0f;
        character_Attack1Front_distance = 6f;
        character_GAD_speed = 3.0f;
        character_GAD_distance = 4.0f;
        JumpTwo = false;
        JumpTwo_speed = 7f;
        A_GAD2 = false;
        A_GAF = false;
        //MP
        MP_assassin[0] = 0.2f;     //0.GroundAttack_Defense
        MP_assassin[1] = 0.15f;    //1.GroundAttack_Front
        MP_assassin[2] = 0.3f;     //2.GroundAttack2
        MP_assassin[3] = 0.15f;     //3.GroundAttack2_Front
        MP_assassin[4] = 0.3f;     //4.GroundAttack2_Defense
        MP_assassin[5] = 0.1f;     //5.AirAttack
        MP_assassin[6] = 0.2f;     //5.AirAttack2
        MP_assassin[7] = 0.2f;     //6.Sprint

    }
    void SetAssassin_All()
    {
        Damage_assassin = new float[8];
        Damage_assassin_speed = new float[8];
        Damage_assassin_Ymove = new float[8];
        Damage_assassin_Break = new int[6];
        //ATK
        Damage_assassin[0] = (0.03f * character_MaxHP);    //0.AirAttack
        Damage_assassin[1] = (0.07f * character_MaxHP);     //1.AirAttack2
        Damage_assassin[2] = (0.04f * character_MaxHP);    //2.GroundAttack(1)
        Damage_assassin[3] = (0.03f * character_MaxHP);    //5.GroundAttack_Defense
        Damage_assassin[4] = (0.03f * character_MaxHP);    //6.GroundAttack_Front
        Damage_assassin[5] = (0.04f * character_MaxHP);    //7.GroundAttack2
        Damage_assassin[6] = (0.08f * character_MaxHP);    //8.GroundAttack2_Front
        Damage_assassin[7] = (0.08f * character_MaxHP);    //9.GroundAttack2_Defense
        //Break ATK
        Damage_assassin_Break[0] = 1;    //0.AirAttack
        Damage_assassin_Break[1] = 2;    //1.AirAttack2
        Damage_assassin_Break[2] = 1;    //2.GroundAttack(1)
        Damage_assassin_Break[3] = 2;    //6.GroundAttack_Front
        Damage_assassin_Break[4] = 2;    //7.GroundAttack2
        Damage_assassin_Break[5] = 2;    //9.GroundAttack2_Defense
        //BeATK Speed
        Damage_assassin_speed[0] = 2.0f;        //0.AirAttack
        Damage_assassin_speed[1] = 2.0f;       //0.AirAttack2
        Damage_assassin_speed[2] = 2.0f;       //2.GroundAttack(1) InAir
        Damage_assassin_speed[3] = 2.0f;       //4.GroundAttack_Defense  InAir
        Damage_assassin_speed[4] = 2.0f;       //GroundAttack_Front InAir
        Damage_assassin_speed[5] = 2f;       //2.GroundAttack2  InAir
        Damage_assassin_speed[6] = 0.2f;      //GroundAttack2_Front
        Damage_assassin_speed[7] = 3.0f;      //GroundAttack2_Defense
        //BeATK Ymove
        Damage_assassin_Ymove[0] = Mathf.Sqrt((-1f) * (character_gravity));    //0.AirAttack1   InAir
        Damage_assassin_Ymove[1] = Mathf.Sqrt((-2.0f) * (character_gravity));    //1.AirAttack2 
        Damage_assassin_Ymove[2] = Mathf.Sqrt((-1f) * (character_gravity));                   //3.GroundAttack(1) InAir
        Damage_assassin_Ymove[3] = Mathf.Sqrt((-0.2f) * (character_gravity));                  //4.GroundAttack_Defense  InAir
        Damage_assassin_Ymove[4] = Mathf.Sqrt((-1f) * (character_gravity));                   //5.GroundAttack_Front InAir
        Damage_assassin_Ymove[5] = Mathf.Sqrt((-1f) * (character_gravity));                   //7.GroundAttack2 InAir
        Damage_assassin_Ymove[6] = Mathf.Sqrt((-7f) * (character_gravity));                  //8.GroundAttack2_Front
        Damage_assassin_Ymove[7] = Mathf.Sqrt((-2f) * (character_gravity));                   //9.GroundAttack2_Defense
    }
}
