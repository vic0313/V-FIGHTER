using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_AI : MonoBehaviour
{
    public CharacterData Data;
    public CharacterInput Input;
    public Animator animator;
    public CharacterData characterTarget;
    public Animator characterTarget_animator;
    private Vector3 ballTarget_pos;
    private Vector3 Runway;
    private float MaxDistance_character;
    private float MinDistance_character;
    private float MaxDistance_BB;
    private bool gameStart;
    private int gamemode;
    private int characterNumber;
    private int characterTeam;
    private int character_Profession;
    private float TargetDistanceCheck;
    private float BallDistanceCheck;
    private int AttackChioce;
    private float AttackTime;
    public float AttackStart;
    private float TargetChangeTime;
    private float TargetChangeTime_Max;

    private float[] SwordAttackDistance;
    private float[] PounchAttackDistance;
    private float[] MagicAttackDistance;
    private float[] GunAttackDistance;
    private float[] AssassinAttackDistance;
    enum Profession
    {
        Sword = 0,
        Pounch,
        Magic,
        Gun,
        Assassin,
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
    private void Start()
    {
        TargetDistanceCheck=0;
        BallDistanceCheck=0;
        MinDistance_character = 0;
        MaxDistance_BB = 0;
        AttackChioce = 0;
        AttackTime = 0;
        AttackStart = 0;
        TargetChangeTime = 0;
        TargetChangeTime_Max = Random.Range(3, 10);
        if (Data.GC.Gamemode_num == 1)
        {
            MaxDistance_character = 2;
        }else
        {
            if (character_Profession != (int)Profession.Gun && character_Profession != (int)Profession.Magic) MaxDistance_character = 5;
            else MaxDistance_character = 13;
        }
            
        gameStart = false;
        //攻撃範囲===============================================================================
        Attackrange();
        
    }
    private void Update()
    {

        if (gameObject.tag != "Player1" && gameObject.tag != "Player2" && gameObject.tag != "Player3" && gameObject.tag != "Player4")
        {
            Attackrange();
            //開始のTarget
            if (Data.GC.Gamestate >= 2 && gameStart == false)
            {
                gamemode = Data.GC.Gamemode_num;
                characterNumber = Data.characterNumber;
                character_Profession = Data.character_Profession;
                characterTeam = Data.characterTeam;
                TargetCheck();
                if (Data.GC.Gamemode_num == 1) TargetCheck_BB();
                gameStart = true;
                InvokeRepeating("DistanceCheck", 2f, 10f);
                InvokeRepeating("AttackStart_change", 0, 1f);
            }
            else if(gameStart && Data.GC.Gamestate<4)
            {
                TargetChangeTime += Time.deltaTime;
                if(characterTarget!=null)
                {
                    //攻撃の目標の決定
                    Vector3 me = this.transform.position;
                    Vector3 you = characterTarget.transform.position;
                    me.y = 0;
                    you.y = 0;
                    MinDistance_character = (you - me).magnitude;
                    //ターゲットを変化
                    if (MinDistance_character > MaxDistance_character || TargetChangeTime > TargetChangeTime_Max || characterTarget.DeadStep > 0)
                    {
                        TargetCheck();
                    }
                }else
                {
                    TargetCheck();
                }
                
                if (Data.GC.Gamemode_num == 1)
                {
                    //BB Battle
                    TargetCheck_BB();
                }
            }
        }
    }
    //障害物があるの場合　ジャンプ判定
    void DistanceCheck()
    {
        if(character_Profession!= (int)Profession.Gun&& character_Profession != (int)Profession.Magic)
        {
            if (Data.GC.Gamemode_num != 1)
            {
                if (MinDistance_character >= TargetDistanceCheck)
                {
                    if (animator.GetBool("InAir") == false)
                    {
                        int type=Random.Range(0, 2);
                        if(type==0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                        else  Input.Cpu_Input[(int)C_Input.Sprint] = true;
                    }
                }
                TargetDistanceCheck = MinDistance_character;
            }
            if (Data.GC.Gamemode_num == 1 && MaxDistance_BB != 0)
            {
                if (MaxDistance_BB >= BallDistanceCheck)
                {
                    if (animator.GetBool("InAir") == false) Input.Cpu_Input[(int)C_Input.Jump] = true;
                }
                BallDistanceCheck = MaxDistance_BB;
            }
        }
    }
    void AttackStart_change()
    {
        AttackTime = Random.Range(1, 6);
    }
    void TargetCheck()
    {
        int turn = 0;
        int target = Random.Range(0, (Data.GC.Character_Data.Length));
        bool OK = false;
        //      ターゲットナンバーは自分        ターゲットは死ぬ                           確認回数                                         ターゲットのチーム判断
        while (OK == false)
        {
            if(target != characterNumber && Data.GC.Character_Data[target].DeadStep == 0 )
            {
                if (Data.GC.Character_Data[target].characterTeam == 0) OK = true;
                else if (Data.GC.Character_Data[target].characterTeam != 0 && characterTeam != Data.GC.Character_Data[target].characterTeam) OK = true;
            }
            if(OK ==false)
            {
                target++;
                turn++;
                if (target > (Data.GC.Character_Data.Length - 1)) target = 0;
                if (turn >= (Data.GC.Character_Data.Length - 1)) break;
            }
        }
        if (OK == true)
        {
            Vector3 me = this.transform.position;
            Vector3 you = Data.GC.Character_Data[target].transform.position;
            me.y = 0;
            you.y = 0;
            MinDistance_character = (you - me).magnitude;
            characterTarget = Data.GC.Character_Data[target];
            characterTarget_animator = characterTarget.gameObject.GetComponent<Animator>();
        }else
        {
            characterTarget = null;
            characterTarget_animator = null;
        }
        TargetChangeTime_Max = Random.Range(3, 11);
        TargetChangeTime = 0;
    }
    void TargetCheck_BB()
    {
        if (Data.GC.BB_Ball != null)
        {
            ballTarget_pos = Data.GC.BB_Ball.transform.position;
            Vector3 me = this.transform.position;
            Vector3 you = ballTarget_pos;
            me.y = 0;
            you.y = 0;
            MaxDistance_BB = (you - me).magnitude;
        }
        else
        {
            ballTarget_pos = this.transform.position;
            MaxDistance_BB = 0;
        }

    }
    public void CPUAI()
    {
        if (Data.GC.GameSet==0&&((gamemode == 0&& characterTarget!=null)|| (gamemode == 1 && characterTarget != null)|| (gamemode == 2 && (characterTarget != null|| Data.GC.BB_Ball != null))))
        {
            if (Data.DeadStep == 0)
            {
                AttackStart += Time.deltaTime * AttackTime;
                switch (character_Profession)
                {
                    case (int)Profession.Sword:
                        CPU_Sword();
                        break;
                    case (int)Profession.Pounch:
                        CPU_Pounch();
                        break;
                    case (int)Profession.Magic:
                        CPU_Magic();
                        break;
                    case (int)Profession.Gun:
                        CPU_Gun();
                        break;
                    case (int)Profession.Assassin:
                        CPU_Assassin();
                        break;
                }
            }
        }
    }
    void CPU_Sword()
    {
        if (gamemode == 0 || gamemode == 2)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                Runway = characterTarget.transform.position - this.transform.position;
                Target_moveway();
                //移動攻撃以外のモーションは移動RESET
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                {
                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                }
                if (animator.GetBool("InAir") == false)
                {
                    //連続攻撃
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)") || animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(2)"))
                    {
                        if (MinDistance_character < SwordAttackDistance[2])
                        {
                            //方向制御　　キャラクターの面向と移動
                            //転向
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            //攻撃
                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                        }
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                    {
                        //攻撃スイッチできる状態
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            bool jumpCheck = false;
                            if (gamemode == 2 && (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")))
                            {
                                jumpCheck = true;
                                Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                            //攻撃できる時間だ
                            if (AttackStart >= 5 && jumpCheck == false)
                            {
                                AttackChioce = Random.Range(3, 10);                 //攻撃CHIOCE

                                if (AttackChioce == 3 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else
                                {
                                    if (AttackChioce >= 2 && AttackChioce <= 4) AttackChioce = 2;
                                    if (MinDistance_character < SwordAttackDistance[AttackChioce])
                                    {

                                        switch (AttackChioce)
                                        {
                                            case 2:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 5:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 6:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 7:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 8:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 9:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (MinDistance_character > 1f)
                                        {
                                            //距離が足りない　移動する
                                            //移動
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                        }
                                    }
                                }
                            }
                            else if (AttackStart < 5 && jumpCheck == false)
                            {
                                if (MinDistance_character > 1f)
                                {
                                    //距離が足りない　移動する
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                }
                            }
                        }
                    }
                }
                else
                {
                    bool jumpcheck = false;
                    if (gamemode == 0)
                    {
                        jumpcheck = true;
                    }
                    else if (gamemode == 2)
                    {
                        if (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) jumpcheck = true;
                    }
                    if (jumpcheck)
                    {
                        //ジャンプ攻撃
                        AttackChioce = Random.Range(0, 2);
                        if (MinDistance_character < SwordAttackDistance[AttackChioce])
                        {
                            if (AttackChioce == 0 && (this.transform.position.y - characterTarget.transform.position.y) > -1 && (this.transform.position.y - characterTarget.transform.position.y) < 1f)
                            {
                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                            }
                            else if (AttackChioce == 1)
                            {
                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                            }
                        }
                        if (MinDistance_character >= 0.8f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                    else
                    {
                        //得点
                        if (MinDistance_character >= 0.2f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }


                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                    {
                        //距離を長くなる
                        Runway = this.transform.position - characterTarget.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
        else if (gamemode == 1)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if ((characterTarget == null || characterTarget.DeadStep > 0))
                {
                    if (MaxDistance_BB != 0)
                    {
                        if (Mathf.Abs(ballTarget_pos.x) <= 13)
                        {
                            //キャラクターがいないの場合はBALLだけ
                            AttackChioce = Random.Range(4, 10);
                            if (MaxDistance_BB < SwordAttackDistance[AttackChioce])
                            {
                                bool attackcheck = false;
                                if (Data.characterTeam == 1)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = false;
                                }
                                else if (Data.characterTeam == 2)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = false;
                                }
                                if (attackcheck)
                                {
                                    //攻撃
                                    switch (AttackChioce)
                                    {
                                        case 4:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 5:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 6:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 7:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 8:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 9:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Runway = ballTarget_pos - this.transform.position;
                                    Target_moveway();
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    int jump = Random.Range(0, 2);
                                    if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position; 
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    //キャラクターがいるの場合は距離を参考して、敵を攻撃ORBALLを選択
                    if ((MinDistance_character <= MaxDistance_character && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") == false
                            && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) || MaxDistance_BB == 0 || Mathf.Abs(ballTarget_pos.x) > 13)
                    {
                        //敵か近い 攻撃する
                        if (animator.GetBool("BeAttack") == false)
                        {
                            Runway = characterTarget.transform.position - this.transform.position;
                            Target_moveway();
                            //移動攻撃以外のモーションは移動RESET
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                            {
                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                            }
                            if (animator.GetBool("InAir") == false)
                            {
                                //連続攻撃
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)") || animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(2)"))
                                {
                                    if (MinDistance_character < SwordAttackDistance[2])
                                    {
                                        //方向制御　　キャラクターの面向と移動
                                        //転向
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        //攻撃
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                    }
                                }
                                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                                {
                                    //攻撃スイッチできる状態
                                    if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                                    {
                                        //攻撃できる時間だ
                                        if (AttackStart >= 5)
                                        {
                                            AttackChioce = Random.Range(3, 10);                 //攻撃CHIOCE
                                            if (AttackChioce == 3 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                            else
                                            {
                                                if (MinDistance_character < SwordAttackDistance[AttackChioce])
                                                {
                                                    switch (AttackChioce)
                                                    {
                                                        case 4:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 5:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 6:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 7:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 8:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 9:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (MinDistance_character > 1f)
                                                    {
                                                        //距離が足りない　移動する
                                                        //移動
                                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (MinDistance_character > 1f)
                                            {
                                                //距離が足りない　移動する
                                                //移動
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //ジャンプ攻撃
                                AttackChioce = Random.Range(0, 2);
                                if (MinDistance_character < SwordAttackDistance[AttackChioce])
                                {
                                    if (AttackChioce == 0 && (this.transform.position.y - characterTarget.transform.position.y) > -1 && (this.transform.position.y - characterTarget.transform.position.y) < 1f)
                                    {
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                    }
                                    else if (AttackChioce == 1)
                                    {
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                    }
                                }
                                if (MinDistance_character >= 0.8f)
                                {
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                }
                            }
                        }
                        else
                        {
                            //受身
                            if (AttackStart >= 5)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                                {
                                    //距離を長くなる
                                    Runway = this.transform.position - characterTarget.transform.position;
                                    Target_moveway();
                                    Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ボールを打ち合わせ
                        AttackChioce = Random.Range(4, 10);
                        if (MaxDistance_BB < SwordAttackDistance[AttackChioce])
                        {
                            bool attackcheck = false;
                            if (Data.characterTeam == 1)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = true;
                            }
                            else if (Data.characterTeam == 2)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = true;
                            }
                            if (attackcheck)
                            {
                                //攻撃
                                switch (AttackChioce)
                                {
                                    case 4:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 5:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 6:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 7:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 8:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 9:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                int jump = Random.Range(0, 2);
                                if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                        }
                        else
                        {
                            Runway = ballTarget_pos - this.transform.position;
                            if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                            else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                            Target_moveway();
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                    {
                        //ボールのどころ移動
                        Runway = ballTarget_pos - this.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
    }
    void CPU_Pounch()
    {
        if (gamemode == 0 || gamemode == 2)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                Runway = characterTarget.transform.position - this.transform.position;
                Target_moveway();
                ////移動攻撃以外のモーションは移動RESET
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                //{

                //}
                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                if (animator.GetBool("InAir") == false)
                {
                    //連続攻撃
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)") || animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(2)"))
                    {
                        if (MinDistance_character < PounchAttackDistance[2])
                        {
                            //方向制御　　キャラクターの面向と移動
                            //転向
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            //攻撃
                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                        }
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                    {
                        //攻撃スイッチできる状態
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            bool jumpCheck = false;
                            if (gamemode == 2 && (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")))
                            {
                                jumpCheck = true;
                                Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                            //攻撃できる時間だ
                            if (AttackStart >= 5 && jumpCheck == false)
                            {
                                AttackChioce = Random.Range(3, 10);                 //攻撃CHIOCE

                                if (AttackChioce == 3 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else
                                {
                                    if (MinDistance_character < PounchAttackDistance[AttackChioce])
                                    {

                                        switch (AttackChioce)
                                        {
                                            case 4:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 5:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 6:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 7:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 8:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 9:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (MinDistance_character > 1f)
                                        {
                                            //距離が足りない　移動する
                                            //移動
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                        }
                                    }
                                }
                            }
                            else if (AttackStart < 5 && jumpCheck == false)
                            {
                                if (MinDistance_character > 1f)
                                {
                                    //距離が足りない　移動する
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                }
                            }
                        }
                    }
                }
                else
                {
                    bool jumpcheck = false;
                    if (gamemode == 0)
                    {
                        jumpcheck = true;
                    }
                    else if (gamemode == 2)
                    {
                        if (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) jumpcheck = true;
                    }
                    if (jumpcheck)
                    {
                        //ジャンプ攻撃
                        AttackChioce = Random.Range(0, 2);
                        if (MinDistance_character < PounchAttackDistance[AttackChioce])
                        {
                            if (AttackChioce == 0 )
                            {
                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                            }
                            else if (AttackChioce == 1)
                            {
                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                            }
                        }
                        if (MinDistance_character >= 0.8f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                    else
                    {
                        //得点
                        if (MinDistance_character >= 0.2f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                    {
                        //距離を長くなる
                        Runway = this.transform.position - characterTarget.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
        else if (gamemode == 1)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if ((characterTarget == null || characterTarget.DeadStep > 0))
                {
                    if (MaxDistance_BB != 0)
                    {
                        if (Mathf.Abs(ballTarget_pos.x) <= 13)
                        {
                            //キャラクターがいないの場合はBALLだけ
                            AttackChioce = Random.Range(4, 10);
                            if (MaxDistance_BB < PounchAttackDistance[AttackChioce])
                            {
                                bool attackcheck = false;
                                if (Data.characterTeam == 1)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = false;
                                }
                                else if (Data.characterTeam == 2)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = false;
                                }
                                if (attackcheck)
                                {
                                    //攻撃
                                    switch (AttackChioce)
                                    {
                                        case 4:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 5:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 6:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 7:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 8:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 9:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Runway = ballTarget_pos - this.transform.position;
                                    if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                    else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                    Target_moveway();
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    int jump = Random.Range(0, 2);
                                    if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    //キャラクターがいるの場合は距離を参考して、敵を攻撃ORBALLを選択
                    if ((MinDistance_character <= MaxDistance_character && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") == false
                            && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) || MaxDistance_BB == 0 || Mathf.Abs(ballTarget_pos.x) > 13)
                    {
                        //敵か近い 攻撃する
                        if (animator.GetBool("BeAttack") == false)
                        {
                            Runway = characterTarget.transform.position - this.transform.position;
                            Target_moveway();
                            //移動攻撃以外のモーションは移動RESET
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                            {
                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                            }
                            if (animator.GetBool("InAir") == false)
                            {
                                //連続攻撃
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(1)") || animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack(2)"))
                                {
                                    if (MinDistance_character < PounchAttackDistance[2])
                                    {
                                        //方向制御　　キャラクターの面向と移動
                                        //転向
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        //攻撃
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                    }
                                }
                                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                                {
                                    //攻撃スイッチできる状態
                                    if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                                    {
                                        //攻撃できる時間だ
                                        if (AttackStart >= 5)
                                        {
                                            AttackChioce = Random.Range(3, 10);                 //攻撃CHIOCE
                                            if (AttackChioce == 3 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                            else
                                            {
                                                if (MinDistance_character < PounchAttackDistance[AttackChioce])
                                                {
                                                    switch (AttackChioce)
                                                    {
                                                        case 4:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 5:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 6:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 7:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 8:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 9:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (MinDistance_character > 1f)
                                                    {
                                                        //距離が足りない　移動する
                                                        //移動
                                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (MinDistance_character > 1f)
                                            {
                                                //距離が足りない　移動する
                                                //移動
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //ジャンプ攻撃
                                AttackChioce = Random.Range(0, 2);
                                if (MinDistance_character < PounchAttackDistance[AttackChioce])
                                {
                                    if (AttackChioce == 0  )
                                    {
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                    }
                                    else if (AttackChioce == 1)
                                    {
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                    }
                                }
                                if (MinDistance_character >= 0.8f)
                                {
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                }
                            }
                        }
                        else
                        {
                            //受身
                            if (AttackStart >= 5)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                                {
                                    //距離を長くなる
                                    Runway = this.transform.position - characterTarget.transform.position;
                                    Target_moveway();
                                    Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ボールを打ち合わせ
                        AttackChioce = Random.Range(4, 10);
                        if (MaxDistance_BB < PounchAttackDistance[AttackChioce])
                        {
                            bool attackcheck = false;
                            if (Data.characterTeam == 1)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = true;
                            }
                            else if (Data.characterTeam == 2)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = true;
                            }
                            if (attackcheck)
                            {
                                //攻撃
                                switch (AttackChioce)
                                {
                                    case 4:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 5:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 6:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 7:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 8:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 9:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                int jump = Random.Range(0, 2);
                                if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                        }
                        else
                        {
                            Runway = ballTarget_pos - this.transform.position;
                            if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                            else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                            Target_moveway();
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                    {
                        //ボールのどころ移動
                        Runway = ballTarget_pos - this.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
    }
    void CPU_Magic()
    {
        if (gamemode == 0 || gamemode == 2)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                Runway = characterTarget.transform.position - this.transform.position;
                Target_moveway();
                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                    {
                        //攻撃スイッチできる状態
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            bool jumpCheck = false;
                            if (gamemode == 2 && (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")))
                            {
                                jumpCheck = true;
                                Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                            //攻撃できる時間だ
                            if (AttackStart >= 5 && jumpCheck == false)
                            {
                                AttackChioce = Random.Range(2, 10);                 //攻撃CHIOCE

                                if (AttackChioce == 2 ) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else
                                {
                                    if (MinDistance_character < MagicAttackDistance[AttackChioce])
                                    {
                                        switch (AttackChioce)
                                        {
                                            case 3:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 4:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 5:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 6:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 7:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 8:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 9:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Sprint] = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (MinDistance_character > 3f)
                                        {
                                            //距離が足りない　移動する
                                            //移動
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                        }
                                    }
                                }
                            }
                            else if (AttackStart < 5 && jumpCheck == false)
                            {
                                if (MinDistance_character > 3f)
                                {
                                    //距離が足りない　移動する
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                }
                            }
                        }
                    }
                }
                else
                {
                    bool jumpcheck = false;
                    if (gamemode == 0)
                    {
                        jumpcheck = true;
                    }
                    else if (gamemode == 2)
                    {
                        if (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) jumpcheck = true;
                    }
                    if (jumpcheck&& AttackStart >= 3)
                    {
                        //ジャンプ攻撃
                        AttackChioce = Random.Range(0, 3);
                        if(AttackChioce==3) Input.Cpu_Input[(int)C_Input.Sprint] = true;
                        else
                        {
                            if (MinDistance_character < MagicAttackDistance[AttackChioce])
                            {
                                if (AttackChioce == 0)
                                {
                                    Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                }
                                else if (AttackChioce == 1)
                                {
                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                }
                            }
                        }
                        if (MinDistance_character >= 0.8f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                    else
                    {
                        //得点
                        if (MinDistance_character >= 0.2f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                    {
                        //距離を長くなる
                        Runway = this.transform.position - characterTarget.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
            if (Input.Cpu_Input[(int)C_Input.Sprint])
            {
                Runway = this.transform.position - characterTarget.transform.position;
                Target_moveway();
                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                Input.Cpu_Input[(int)C_Input.Vertical] = true;
            }
        }
        else if (gamemode == 1)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if ((characterTarget == null || characterTarget.DeadStep > 0))
                {
                    if (MaxDistance_BB != 0)
                    {
                        if (Mathf.Abs(ballTarget_pos.x) <= 13)
                        {
                            //キャラクターがいないの場合はBALLだけ
                            AttackChioce = Random.Range(3, 10);
                            if (MaxDistance_BB < MagicAttackDistance[AttackChioce])
                            {
                                bool attackcheck = false;
                                if (Data.characterTeam == 1)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = false;
                                }
                                else if (Data.characterTeam == 2)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = false;
                                }
                                if (attackcheck)
                                {
                                    //攻撃
                                    switch (AttackChioce)
                                    {
                                        case 3:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 4:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 5:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 6:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 7:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 8:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 9:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Sprint] = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Runway = ballTarget_pos - this.transform.position;
                                    if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                    else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                    Target_moveway();
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    int jump = Random.Range(0, 3);
                                    if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                    else if (jump == 1) Input.Cpu_Input[(int)C_Input.Sprint] = true;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    //キャラクターがいるの場合は距離を参考して、敵を攻撃ORBALLを選択
                    if ((MinDistance_character <= MaxDistance_character && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") == false
                            && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) || MaxDistance_BB == 0 || Mathf.Abs(ballTarget_pos.x) > 13)
                    {
                        //敵か近い 攻撃する
                        if (animator.GetBool("BeAttack") == false)
                        {
                            if (MinDistance_character > 5)
                            {
                                Runway = characterTarget.transform.position - this.transform.position;
                            }
                            else
                            {
                                Runway = this.transform.position - characterTarget.transform.position;
                            }
                            Target_moveway();

                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                            if (animator.GetBool("InAir") == false)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                                {
                                    //攻撃スイッチできる状態
                                    if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                                    {
                                        //攻撃できる時間だ
                                        if (AttackStart >= 5)
                                        {
                                            AttackChioce = Random.Range(3, 10);                 //攻撃CHIOCE
                                            if (AttackChioce == 3 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                            else
                                            {
                                                if (MinDistance_character < MagicAttackDistance[AttackChioce])
                                                {
                                                    switch (AttackChioce)
                                                    {
                                                        case 3:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 4:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 5:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 6:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 7:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 8:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 9:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Sprint] = true;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (MinDistance_character > 3f)
                                                    {
                                                        //距離が足りない　移動する
                                                        //移動
                                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (MinDistance_character > 3f)
                                            {
                                                //距離が足りない　移動する
                                                //移動
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if(AttackStart >= 3)
                                {
                                    //ジャンプ攻撃
                                    AttackChioce = Random.Range(0, 3);
                                    if (AttackChioce == 3) Input.Cpu_Input[(int)C_Input.Sprint] = true;
                                    else
                                    {
                                        if (MinDistance_character < MagicAttackDistance[AttackChioce])
                                        {
                                            if (AttackChioce == 0)
                                            {
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            }
                                            else if (AttackChioce == 1)
                                            {
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            }
                                        }
                                    }
                                    if (MinDistance_character >= 0.8f)
                                    {
                                        //移動
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //受身
                            if (AttackStart >= 5)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                                {
                                    //距離を長くなる
                                    Runway = this.transform.position - characterTarget.transform.position;
                                    Target_moveway();
                                    Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ボールを打ち合わせ
                        AttackChioce = Random.Range(4, 10);
                        if (MaxDistance_BB < MagicAttackDistance[AttackChioce])
                        {
                            bool attackcheck = false;
                            if (Data.characterTeam == 1)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = true;
                            }
                            else if (Data.characterTeam == 2)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = true;
                            }
                            if (attackcheck)
                            {
                                //攻撃
                                switch (AttackChioce)
                                {
                                    case 4:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 5:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 6:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 7:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 8:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 9:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                int jump = Random.Range(0, 3);
                                if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else if (jump == 1) Input.Cpu_Input[(int)C_Input.Sprint] = true;
                            }
                        }
                        else
                        {
                            Runway = ballTarget_pos - this.transform.position;
                            if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                            else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                            Target_moveway();
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            int jump = Random.Range(0, 3);
                            if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                            else if (jump == 1) Input.Cpu_Input[(int)C_Input.Sprint] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                    {
                        //ボールのどころ移動
                        Runway = ballTarget_pos - this.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
        
    }
    void CPU_Gun()
    {
        if (gamemode == 0 || gamemode == 2)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if(animator.GetBool("InAir"))
                {
                    Runway = this.transform.position-characterTarget.transform.position;
                }else
                {
                    Runway = characterTarget.transform.position - this.transform.position;
                }
                if(animator.GetBool("Shooting")|| Data.AirAttack2_use)
                {
                    if(Data.AirAttack2_use)
                    {
                        Runway.y = 0;
                        Runway = Runway.normalized;
                        Runway.x += Random.Range(-0.5f, 0.5f);
                        Runway.z += Random.Range(-0.5f, 0.5f);
                    }
                    if(animator.GetBool("Shooting")) Input.Cpu_Input[(int)C_Input.Defense] = true;
                }else
                {
                    Input.Cpu_Input[(int)C_Input.Defense] = false;
                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                }
                Target_moveway();
                if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                    {
                        //攻撃スイッチできる状態
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            bool jumpCheck = false;
                            if (gamemode == 2 && (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")))
                            {
                                jumpCheck = true;
                                Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                            if (animator.GetBool("Shooting") == false)
                            {
                                //攻撃できる時間だ
                                if (AttackStart >= 5 && jumpCheck == false)
                                {
                                    AttackChioce = Random.Range(1, 7);                 //攻撃CHIOCE
                                    if (AttackChioce == 1&& MinDistance_character>3f) AttackChioce = Random.Range(2, 7);
                                    if (AttackChioce == 1 ) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                    else
                                    {
                                        if (MinDistance_character <  GunAttackDistance[AttackChioce])
                                        {
                                            switch (AttackChioce)
                                            {
                                                case 2:
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                    Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                    Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                    break;
                                                case 3:
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                    Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                    Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                    break;
                                                case 4:
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                    Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                    break;
                                                case 5:
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                    Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                    break;
                                                case 6:
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                    Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (MinDistance_character > 4f)
                                            {
                                                //距離が足りない　移動する
                                                //移動
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                            }
                                        }
                                    }
                                }
                                else if (AttackStart < 5 && jumpCheck == false)
                                {
                                    if (MinDistance_character > 4f)
                                    {
                                        //距離が足りない　移動する
                                        //移動
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                    }
                                }
                            }
                            else
                            {
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    bool jumpcheck = false;
                    if (gamemode == 0)
                    {
                        jumpcheck = true;
                    }
                    else if (gamemode == 2)
                    {
                        if (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) jumpcheck = true;
                    }
                    if (jumpcheck)
                    {
                        if(Data.AirAttack2_use==false)
                        {
                            //ジャンプ攻撃
                            AttackChioce = Random.Range(0, 2);
                            if (MinDistance_character <  GunAttackDistance[AttackChioce])
                            {
                                if (AttackChioce == 0)
                                {
                                    Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                }
                                else if (AttackChioce == 1)
                                {
                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                }
                            }
                            if (MinDistance_character >= 0.8f)
                            {
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }else
                        {
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                        
                    }
                    else
                    {
                        //得点
                        if (MinDistance_character >= 0.2f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                    {
                        //距離を長くなる
                        Runway = this.transform.position - characterTarget.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
        else if (gamemode == 1)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if ((characterTarget == null || characterTarget.DeadStep > 0))
                {
                    if (MaxDistance_BB != 0)
                    {
                        if (Mathf.Abs(ballTarget_pos.x) <= 13)
                        {
                            //キャラクターがいないの場合はBALLだけ
                            AttackChioce = Random.Range(2, 7);
                            if (MaxDistance_BB <  GunAttackDistance[AttackChioce])
                            {
                                bool attackcheck = false;
                                if (Data.characterTeam == 1)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = false;
                                }
                                else if (Data.characterTeam == 2)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = false;
                                }
                                if (attackcheck)
                                {
                                    //攻撃
                                    switch (AttackChioce)
                                    {
                                        case 2:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 3:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 4:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 5:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 6:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Runway = ballTarget_pos - this.transform.position;
                                    if (Data.characterTeam == 1) Runway.x -= 3f;      //赤チーム　ボールの左目指す
                                    else Runway.x += 3f;                                  //青チーム　ボールの右目指す
                                    Target_moveway();
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 3f;      //赤チーム　ボールの左目指す
                                else Runway.x += 3f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    //キャラクターがいるの場合は距離を参考して、敵を攻撃ORBALLを選択
                    if ((MinDistance_character <= MaxDistance_character && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") == false
                            && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) || MaxDistance_BB == 0 || Mathf.Abs(ballTarget_pos.x) > 13)
                    {
                        //敵か近い 攻撃する
                        if (animator.GetBool("BeAttack") == false)
                        {
                            if (animator.GetBool("InAir") )
                            {
                                Runway = this.transform.position - characterTarget.transform.position;
                            }
                            else
                            {
                                Runway = characterTarget.transform.position - this.transform.position;
                            }
                            if (animator.GetBool("Shooting") || Data.AirAttack2_use)
                            {
                                if (Data.AirAttack2_use)
                                {
                                    Runway.y = 0;
                                    Runway = Runway.normalized;
                                    Runway.x += Random.Range(-0.5f, 0.5f);
                                    Runway.z += Random.Range(-0.5f, 0.5f);
                                }
                                if (animator.GetBool("Shooting")) Input.Cpu_Input[(int)C_Input.Defense] = true;
                            }
                            else
                            {
                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                            }
                            Target_moveway();
                            if (animator.GetBool("InAir") == false)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                                {
                                    //攻撃スイッチできる状態
                                    if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                                    {
                                        if(animator.GetBool("Shooting")==false)
                                        {
                                            //攻撃できる時間だ
                                            if (AttackStart >= 5)
                                            {
                                                AttackChioce = Random.Range(1, 7);                 //攻撃CHIOCE
                                                if (AttackChioce == 1 && MinDistance_character > 3f) AttackChioce = Random.Range(2, 7);
                                                if (AttackChioce == 1 ) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                                else
                                                {
                                                    if (MinDistance_character <  GunAttackDistance[AttackChioce])
                                                    {
                                                        switch (AttackChioce)
                                                        {
                                                            case 2:
                                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                                break;
                                                            case 3:
                                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                                break;
                                                            case 4:
                                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                                break;
                                                            case 5:
                                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                                break;
                                                            case 6:
                                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (MinDistance_character > 4f)
                                                        {
                                                            //距離が足りない　移動する
                                                            //移動
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (MinDistance_character > 4f)
                                                {
                                                    //距離が足りない　移動する
                                                    //移動
                                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                }
                                            }
                                        }
                                        else
                                        {
                                            //移動
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Data.AirAttack2_use == false)
                                {
                                    //ジャンプ攻撃
                                    AttackChioce = Random.Range(0, 2);
                                    if (MinDistance_character <  GunAttackDistance[AttackChioce])
                                    {
                                        if (AttackChioce == 0)
                                        {
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        }
                                        else if (AttackChioce == 1)
                                        {
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        }
                                    }
                                    if (MinDistance_character >= 0.8f)
                                    {
                                        //移動
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    }
                                }
                                else
                                {
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                }
                            }
                        }
                        else
                        {
                            //受身
                            if (AttackStart >= 5)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                                {
                                    //距離を長くなる
                                    Runway = this.transform.position - characterTarget.transform.position;
                                    Target_moveway();
                                    Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ボールを打ち合わせ
                        AttackChioce = Random.Range(2, 7);
                        if (MaxDistance_BB <  GunAttackDistance[AttackChioce])
                        {
                            bool attackcheck = false;
                            if (Data.characterTeam == 1)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = true;
                            }
                            else if (Data.characterTeam == 2)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = true;
                            }
                            if (attackcheck)
                            {
                                //攻撃
                                switch (AttackChioce)
                                {
                                    case 2:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 3:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 4:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 5:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 6:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 3f;      //赤チーム　ボールの左目指す
                                else Runway.x += 3f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                int jump = Random.Range(0, 2);
                                if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                        }
                        else
                        {
                            Runway = ballTarget_pos - this.transform.position;
                            if (Data.characterTeam == 1) Runway.x -= 3f;      //赤チーム　ボールの左目指す
                            else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                            Target_moveway();
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                    {
                        //ボールのどころ移動
                        Runway = ballTarget_pos - this.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
    }
    
    void CPU_Assassin()
    {
        if (gamemode == 0 || gamemode == 2)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                Runway = characterTarget.transform.position - this.transform.position;
                Target_moveway();
                //移動攻撃以外のモーションは移動RESET
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                {
                    Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                    Input.Cpu_Input[(int)C_Input.Vertical] = false;
                }
                
                if (animator.GetBool("InAir") == false)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                    {
                        //攻撃スイッチできる状態
                        if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                        {
                            bool jumpCheck = false;
                            if (gamemode == 2 && (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible")
                                    || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack")))
                            {
                                jumpCheck = true;
                                Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                            //攻撃できる時間だ
                            if (AttackStart >= 5 && jumpCheck == false)
                            {
                                AttackChioce = Random.Range(1, 8);                 //攻撃CHIOCE

                                if (AttackChioce == 1 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else
                                {
                                    if (MinDistance_character < AssassinAttackDistance[AttackChioce])
                                    {

                                        switch (AttackChioce)
                                        {
                                            case 2:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 3:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 4:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                break;
                                            case 5:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 6:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                            case 7:
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (MinDistance_character > 1f)
                                        {
                                            //距離が足りない　移動する
                                            //移動
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                        }
                                    }
                                }
                            }
                            else if (AttackStart < 5 && jumpCheck == false)
                            {
                                if (MinDistance_character > 1f)
                                {
                                    //距離が足りない　移動する
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                }
                            }
                        }
                    }
                }
                else
                {
                    bool jumpcheck = false;
                    if (gamemode == 0)
                    {
                        jumpcheck = true;
                    }
                    else if (gamemode == 2)
                    {
                        if (characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           || characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) jumpcheck = true;
                    }
                    if (jumpcheck)
                    {
                        //ジャンプ攻撃
                        AttackChioce = Random.Range(0, 3);
                        if(AttackChioce==3) Input.Cpu_Input[(int)C_Input.Jump] = true;
                        else
                        {
                            if (MinDistance_character < AssassinAttackDistance[AttackChioce])
                            {
                                if (AttackChioce == 0)
                                {
                                    if(MinDistance_character<0.5f) Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                    else Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                }
                                else if (AttackChioce == 1)
                                {
                                    Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                }
                            }
                        }
                        if (MinDistance_character >= 0.8f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                    else
                    {
                        //得点
                        if (MinDistance_character >= 0.2f)
                        {
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                    {
                        //距離を長くなる
                        Runway = this.transform.position - characterTarget.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
        else if (gamemode == 1)
        {
            if (animator.GetBool("BeAttack") == false)
            {
                if ((characterTarget == null || characterTarget.DeadStep > 0))
                {
                    if (MaxDistance_BB != 0)
                    {
                        if (Mathf.Abs(ballTarget_pos.x) <= 13)
                        {
                            //キャラクターがいないの場合はBALLだけ
                            AttackChioce = Random.Range(2, 8);
                            if (MaxDistance_BB < AssassinAttackDistance[AttackChioce])
                            {
                                bool attackcheck = false;
                                if (Data.characterTeam == 1)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = false;
                                }
                                else if (Data.characterTeam == 2)
                                {
                                    if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                    else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = false;
                                }
                                if (attackcheck)
                                {
                                    //攻撃
                                    switch (AttackChioce)
                                    {
                                        case 2:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 3:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 4:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            break;
                                        case 5:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 6:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                        case 7:
                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                            break;
                                    }
                                }
                                else
                                {
                                    Runway = ballTarget_pos - this.transform.position;
                                    if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                    else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                    Target_moveway();
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                    int jump = Random.Range(0, 2);
                                    if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                            }
                        }
                    }
                }
                else
                {
                    //キャラクターがいるの場合は距離を参考して、敵を攻撃ORBALLを選択
                    if ((MinDistance_character <= MaxDistance_character && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start") == false
                            && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_start_NoInvincible") == false
                           && characterTarget_animator.GetCurrentAnimatorStateInfo(0).IsName("DizzyBeAttack") == false) || MaxDistance_BB == 0 || Mathf.Abs(ballTarget_pos.x) > 13)
                    {
                        //敵か近い 攻撃する
                        if (animator.GetBool("BeAttack") == false)
                        {
                            Runway = characterTarget.transform.position - this.transform.position;
                            Target_moveway();
                            //移動攻撃以外のモーションは移動RESET
                            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GroundAttack_Front") == false)
                            {
                                Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                Input.Cpu_Input[(int)C_Input.Vertical] = false;
                            }
                            if (animator.GetBool("InAir") == false)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
                                {
                                    //攻撃スイッチできる状態
                                    if (animator.GetBool("CanSwitch") || animator.GetBool("CanAttackSwitch"))
                                    {
                                        //攻撃できる時間だ
                                        if (AttackStart >= 5)
                                        {
                                            AttackChioce = Random.Range(1, 8);                 //攻撃CHIOCE
                                            if (AttackChioce == 1 || (this.transform.position.y - characterTarget.transform.position.y) < -2f) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                            else
                                            {
                                                if (MinDistance_character < AssassinAttackDistance[AttackChioce])
                                                {
                                                    switch (AttackChioce)
                                                    {
                                                        case 2:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 3:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 4:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                                            break;
                                                        case 5:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 6:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = false;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                        case 7:
                                                            Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                                            Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                                            Input.Cpu_Input[(int)C_Input.Defense] = true;
                                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (MinDistance_character > 1f)
                                                    {
                                                        //距離が足りない　移動する
                                                        //移動
                                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (MinDistance_character > 1f)
                                            {
                                                //距離が足りない　移動する
                                                //移動
                                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                                Input.Cpu_Input[(int)C_Input.Vertical] = true;

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //ジャンプ攻撃
                                AttackChioce = Random.Range(0, 3);
                                if (AttackChioce == 3) Input.Cpu_Input[(int)C_Input.Jump] = true;
                                else
                                {
                                    if (MinDistance_character < AssassinAttackDistance[AttackChioce])
                                    {
                                        if (AttackChioce == 0)
                                        {
                                            if (MinDistance_character < 0.5f) Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                            else Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        }
                                        else if (AttackChioce == 1)
                                        {
                                            Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        }
                                    }
                                }
                                if (MinDistance_character >= 0.8f)
                                {
                                    //移動
                                    Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                    Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                }
                            }
                        }
                        else
                        {
                            //受身
                            if (AttackStart >= 5)
                            {
                                if (animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp"))
                                {
                                    //距離を長くなる
                                    Runway = this.transform.position - characterTarget.transform.position;
                                    Target_moveway();
                                    Input.Cpu_Input[(int)C_Input.Jump] = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        //ボールを打ち合わせ
                        AttackChioce = Random.Range(2, 8);
                        if (MaxDistance_BB < AssassinAttackDistance[AttackChioce])
                        {
                            bool attackcheck = false;
                            if (Data.characterTeam == 1)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) > 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) <= 0) attackcheck = true;
                            }
                            else if (Data.characterTeam == 2)
                            {
                                if ((ballTarget_pos.x - this.transform.position.x) < 0) attackcheck = true;
                                else if ((ballTarget_pos.x - this.transform.position.x) >= 0) attackcheck = true;
                            }
                            if (attackcheck)
                            {
                                //攻撃
                                switch (AttackChioce)
                                {
                                    case 2:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 3:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 4:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack1] = true;
                                        break;
                                    case 5:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 6:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                        Input.Cpu_Input[(int)C_Input.Defense] = false;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                    case 7:
                                        Input.Cpu_Input[(int)C_Input.Horizontal] = false;
                                        Input.Cpu_Input[(int)C_Input.Vertical] = false;
                                        Input.Cpu_Input[(int)C_Input.Defense] = true;
                                        Input.Cpu_Input[(int)C_Input.Attack2] = true;
                                        break;
                                }
                            }
                            else
                            {
                                Runway = ballTarget_pos - this.transform.position;
                                if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                                else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                                Target_moveway();
                                //移動
                                Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                                Input.Cpu_Input[(int)C_Input.Vertical] = true;
                                int jump = Random.Range(0, 2);
                                if (jump == 0) Input.Cpu_Input[(int)C_Input.Jump] = true;
                            }
                        }
                        else
                        {
                            Runway = ballTarget_pos - this.transform.position;
                            if (Data.characterTeam == 1) Runway.x -= 1.5f;      //赤チーム　ボールの左目指す
                            else Runway.x += 1.5f;                                  //青チーム　ボールの右目指す
                            Target_moveway();
                            //移動
                            Input.Cpu_Input[(int)C_Input.Horizontal] = true;
                            Input.Cpu_Input[(int)C_Input.Vertical] = true;
                        }
                    }
                }
            }
            else
            {
                //受身
                if (AttackStart >= 5)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end") || animator.GetCurrentAnimatorStateInfo(0).IsName("BeAttack(Knock)_end_NoInvincible"))
                    {
                        //ボールのどころ移動
                        Runway = ballTarget_pos - this.transform.position;
                        Target_moveway();
                        Input.Cpu_Input[(int)C_Input.Jump] = true;
                    }
                }
            }
        }
    }

    void Target_moveway()
    {
        Runway.y = 0;
        Runway = (Runway).normalized;
        Data.character_input_H = Runway.x;
        Data.character_input_V = Runway.z;
        if (Runway.z != 0)
        {
            Data.character_angle = Mathf.Atan(Runway.x / Runway.z) / (Mathf.PI / 180);
            Data.character_angle = Runway.z < 0 ? Data.character_angle + 180 : Data.character_angle;
            if (Data.character_angle > 180)
            {
                Data.character_angle -= 360;
            }
            else if (Data.character_angle < (-180))
            {
                Data.character_angle += 360;
            }
        }
        else
        {
            if (Runway.x > 0) Data.character_angle = 90;
            else if (Runway.x < 0) Data.character_angle = -90;
            else Data.character_angle = 0;
        }
        Data.character_XZmove = new Vector3(Mathf.Sin(Data.character_angle / (180 / Mathf.PI)), 0, Mathf.Cos(Data.character_angle / (180 / Mathf.PI)));
    }
    void Attackrange()
    {
        //Sword職
        SwordAttackDistance = new float[10];
        SwordAttackDistance[0] = 1f;    //0.AirAttack
        SwordAttackDistance[1] = 1.5f;     //1.AirAttack2
        SwordAttackDistance[2] = 2f;    //2.GroundAttack(1)
        SwordAttackDistance[3] = 2f;    //3.GroundAttack(2)
        SwordAttackDistance[4] = 2f;    //4.GroundAttack(3)
        SwordAttackDistance[5] = 2f;    //5.GroundAttack_Defense
        SwordAttackDistance[6] = 2f;    //6.GroundAttack_Front
        SwordAttackDistance[7] = 2f;    //7.GroundAttack2
        SwordAttackDistance[8] = 3f;    //8.GroundAttack2_Front
        SwordAttackDistance[9] = 2f;    //9.GroundAttack2_Defense
        //Pounch職
        PounchAttackDistance = new float[10];
        PounchAttackDistance[0] = 2;    //0.AirAttack
        PounchAttackDistance[1] = 5;    //1.AirAttack2
        PounchAttackDistance[2] = 1;    //2.GroundAttack(1)
        PounchAttackDistance[3] = 1;    //3.GroundAttack(2)
        PounchAttackDistance[4] = 1;    //4.GroundAttack(3)
        PounchAttackDistance[5] = 2;    //5.GroundAttack_Defense
        PounchAttackDistance[6] = 4;    //6.GroundAttack_Front
        PounchAttackDistance[7] = 1;    //7.GroundAttack2
        PounchAttackDistance[8] = 4;    //8.GroundAttack2_Front
        PounchAttackDistance[9] = 3; //9.GroundAttack2_Defense
        //Magic職
        MagicAttackDistance = new float[10];
        MagicAttackDistance[0] = 2f;    //0.AirAttack
        MagicAttackDistance[1] = 20f;    //1.AirAttack2
        MagicAttackDistance[2] = 1f;    //2.GroundAttack(1)
        MagicAttackDistance[3] = 1f;    //3.GroundAttack(2)
        MagicAttackDistance[4] = 20f;    //5.GroundAttack_Defense
        MagicAttackDistance[5] = 20f;    //6.GroundAttack_Front
        MagicAttackDistance[6] = 6f;    //7.GroundAttack2
        MagicAttackDistance[7] = 5f;    //8.GroundAttack2_Front
        MagicAttackDistance[8] = 3f; //9.GroundAttack2_Defense
        MagicAttackDistance[9] = 20f;    //9.Sprint
        //Gun職
        GunAttackDistance = new float[7];
        GunAttackDistance[0] = 20f;     //1.AirAttack1
        GunAttackDistance[1] = 20f;     //1.AirAttack2
        GunAttackDistance[2] = 20f;    //2.GroundAttack(2)
        GunAttackDistance[3] = 20f;    //6.GroundAttack_Front
        GunAttackDistance[4] = 2f;    //7.GroundAttack2
        GunAttackDistance[5] = 2f;    //8.GroundAttack2_Front
        GunAttackDistance[6] = 30f;   //9.GroundAttack2_Defense
        //Assassin職
        AssassinAttackDistance = new float[8];
        AssassinAttackDistance[0] = 2f;    //0.AirAttack
        AssassinAttackDistance[1] = 4f;     //1.AirAttack2
        AssassinAttackDistance[2] = 3f;    //2.GroundAttack(1)
        AssassinAttackDistance[3] = 2f;    //5.GroundAttack_Defense
        AssassinAttackDistance[4] = 2f;    //6.GroundAttack_Front
        AssassinAttackDistance[5] = 20f;    //7.GroundAttack2
        AssassinAttackDistance[6] = 1.5f;    //8.GroundAttack2_Front
        AssassinAttackDistance[7] = 6f;    //9.GroundAttack2_Defense
    }
}
