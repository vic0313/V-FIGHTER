using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour
{
    //ゲームの資料保存変数======================================================================
    public GameInfo GI;
    public GameObject[] GAMESTART;
    public AudioClip[] effect;
    public AudioSource audiosource;
    public int Gamemode_num;                            //ゲームモード選択した後ステージの番号
    public int Gamemode2_num;                          //NORMAL OR ZOMBIE
    public int Game_Time;                               //0:2:00 1:4:00 2:∞
    public int Game_Mp;                                 //MP減らす状況   0:ON 1:OFF
    public int Game_Itemuse;                            //アイテム使用   0:ON 1:OFF
    public int GameTimeNow;
    public GameObject[] Gamemode_choice;                //ゲームの種類によって、モード物件を保存用変数　0.Normal 1.Pinball 2.StepAndJump
    public Transform[] Rebron_pos = new Transform[16];   //キャラクター再生位置
    public int Stage_num;                               //ゲームモード選択した後ステージの番号
    public GameObject[] NormalAndJump_Stage_Date;              //ステージOBJを保存用変数
    public GameObject[] Pinball_Stage_Date;             //ステージOBJを保存用変数
    public GameObject UI_MAP;                           //ゲームUIのOBJ
    private int player_num;                             //プレイヤの数量
    private int cpu_num;                                //CPUの数量
    public int Gamestate;                               //現在ゲームの状況
    public GameObject BB_Ball;
    //一時停止使用した変数
    public bool Pause;
    public int Pause_chioce;
    public bool Pause_chiocing;
    public bool Pause_watchGame;
    //アイテム機率変数
    public int[] item_Probability;
    
    //キャラの資料保存変数======================================================================
    public int[] player_Profession;                     //プレイヤの職業の番号
    public int[] cpu_Profession;                        //CPUの職業の番号
    public GameObject[] Profession_prefeb;              //職業の物件
    public GameObject[] Character_info;                 //生成の物件のGameObjの指向
    public CharacterData[] Character_Data;              //生成の物件のGameObjのData Scriptの指向
    public int[] character_team;                        //キャラのチーム番号
    public GameObject[] UIobj;                          //キャラのを使用したUIOBJ
    public int ItemKill;
    public int[] TeamKill;                              //TeamのKILL総数　0.Aチーム　1.Bチーム　2.Cチーム　3.Dチーム
    public int[] Remaining_num;                         //各チーム残りメンバー総数　0.Nチーム 1.Aチーム　2.Bチーム　3.Cチーム　4.Dチーム　
    //ゲームui用変数
    public string Advantage;
    public string Disadvantages;
    public GameUI G_UI;
    public int PointnowUp;
    public int PointnowDown;
    public int killnowUp;
    public int killnowDown;
    //V Battle Zombieモード使用した変数
    public bool[] DeadTeam;
    //bbモードとjモード使用した変数
    public int[] Team_Point;
    public int[] TeamPointPlus;         //0:A 1:B
    public int GamePointMax;

    public int GameSet;                                 //ゲーム終了の種類　1.時間制限　2.AllKill 3.point到達
    public bool[] GameSet_Already;                      //ゲーム終了の種類　0.AllKill 2.point到達
    public List<int> setTeam = new List<int>();
    public List<string> setCharacter = new List<string>();
    public int[] setTeam_point ;
    public string[] setCharacter_point;
    public bool sounduse;
    // Start is called before the first frame update
    enum Profession
    {
        Sword=0,
        Pounch,
        Magic,
        Gun,
        Assassin,
    }
    enum Team
    {
        NOteam = 0,
        ONEteam,
        TWOteam,
        THREEteam,
        FOURteam,
    }
    void Start()
    {
        
        GI = GameInfo.game_data;
        Gamemode_num = GI.Gamemode_choice;
        Gamemode2_num = GI.game_mode_chioce;
        Stage_num = GI.Stage_num;
        Game_Time = GI.game_time;
        switch (Game_Time)
        {
            case 0:
                GameTimeNow = 120;
                break;
            case 1:
                GameTimeNow = 240;
                break;
            case 2:
                GameTimeNow = 500;
                break;
        }
        Game_Mp = GI.game_MPcheck;
        Game_Itemuse = GI.game_Itemcheck;

        player_num = GI.player_num;
        cpu_num = GI.cpu_num;
        player_Profession = GI.player_Profession.ToArray();
        character_team = GI.character_team.ToArray();
        cpu_Profession = GI.cpu_Profession.ToArray();
        item_Probability = GI.item_Probability;

        Character_info = new GameObject[player_num + cpu_num];
        Character_Data = new CharacterData[(player_num + cpu_num)];
        Gamestate = 0;
        ItemKill = 0;
        setTeam_point = new int[4];
        DeadTeam = new bool[4];
        setCharacter_point = new string[(player_num + cpu_num)];
        for (int i = 0; i < 4; i++)
        {
            TeamKill[i] = 0;
            Team_Point[i] = 0;
            DeadTeam[i] = false;
        }
        TeamPointPlus = new int[2];
        TeamPointPlus[0] = 0;
        TeamPointPlus[1] = 0;
        Advantage = "均衡";
        Disadvantages = "均衡";
        for (int i = 0; i < 3; i++) GAMESTART[i].SetActive(true);
        Pause = false;
        Pause_chioce = 0;
        Pause_chiocing = false;
        GamePointMax = GI.game_point_max;
        GameSet = 0;
        GameSet_Already = new bool[2];
        Remaining_num = new int[5];
        for (int i = 0; i < 2; i++) GameSet_Already[i] = false;
        for (int i = 0; i < 5; i++) Remaining_num[i] = 0;
        PointnowUp=0;
        PointnowDown=0;
        killnowUp = 0;
        killnowDown = 0;
        sounduse = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Gamestate)
        {
            case 0:
                //inSense 選択
                //他のクラスで選択後
                //ステージの生成
                SetStage();
                //キャラクタの生成
                SetCharacter();
                //UIの生成
                
                Gamestate = 1;
                break;
            case 1:
                //READY~ FIGHT
                break;
            case 2:
                if(Game_Time!=2) InvokeRepeating("TimeControler",1,1);    //時間開始
                audiosource.PlayOneShot(effect[0]);
                Gamestate = 3;
                break;
            case 3:
                PasueManu();
                SetKill();
                if (GI.Gamemode_choice > 0) SetPoint();
                //勝利の条件を判定
                Game_Set();
                if (GameSet != 0) Gamestate = 4;
                break;
            case 4:
                if (Game_Time != 2) CancelInvoke("TimeControler");
                SetKill();
                if (GI.Gamemode_choice > 0) SetPoint();
                switch (GameSet)
                {
                    case 1://時間制限
                        Gamestate = 5;
                        break;
                    case 2://AllKill
                        Time.timeScale = 0.2f;
                        break;
                    case 3://point到達
                        Time.timeScale = 0.2f;
                        if (Gamemode_num == 1)
                        {
                            Gamestate = 5;
                        }
                        break;
                }
                //勝利の条件を達成後 動画開始
                //Gamestate = 5;
                break;
            case 5:
                //勝利の条件を達成後 動画終わったら　Gamestate++
                if (sounduse == false)
                {
                    audiosource.PlayOneShot(effect[6]);
                    sounduse = true;
                }
                Time.timeScale = 0.5f;
                break;
            case 6:
                GI.character_Kill_sum.Clear();
                GI.character_BeKill_sum.Clear();
                GI.character_Point_sum.Clear();
                GI.character_Lifenow.Clear();
                for (int i=0;i<Character_Data.Length;i++)
                {
                    GI.character_Kill_sum.Add(Character_Data[i].killnum);
                    GI.character_BeKill_sum.Add(Character_Data[i].bekillnum);
                    GI.character_Point_sum.Add(Character_Data[i].Character_Point);
                    if(GI.player_life_max!=4)
                    {
                        float sum = (Character_Data[i].character_NOW * Character_Data[i].character_MaxHP)+ Character_Data[i].character_NowHP;
                        GI.character_Lifenow.Add(sum);
                    }
                }
                GI.Item_kill = ItemKill;
                GI.TeamKillAll = TeamKill;
                GI.TeamPointAll = Team_Point;
                GI.Scene_now = 5;
                Time.timeScale = 1f;
                GI.LoadScene_OK();
                break;
            case 7:
                //PASUE画面用 GameReset動画開始 終わったらRESET
                GAMESTART[3].SetActive(true);
                break;
            case 8:
                //PASUE画面用 BackToMAin動画開始 終わったらScenechange
                GAMESTART[4].SetActive(true);
                break;
        }
        
    }
    void SetCharacter()
    {
        UIobj=new GameObject[(player_num + cpu_num)];
        //キャラクタの生成
        int p_now = 0;
        if(player_num!=0)
        {
            int Sword_num = 0;
            int Pounch_num = 0;
            int Magic_num = 0;
            int Gun_num = 0;
            int Assassin_num = 0;
            foreach (var iNum in player_Profession)
            {
                Character_info[p_now] = Instantiate(Profession_prefeb[iNum], Rebron_pos[p_now].position, Rebron_pos[p_now].rotation);
                Character_Data[p_now] = Character_info[p_now].GetComponent<CharacterData>();
                Character_Data[p_now].characterTeam = character_team[p_now];
                Character_Data[p_now].RebornPoint = Rebron_pos[p_now].position;
                Character_Data[p_now].GC = this;
                Character_Data[p_now].characterNumber = p_now;
                UIobj[p_now] = UI_MAP.GetComponent<CharacterUI>().UI_OBJ[p_now];
                UIobj[p_now].SetActive(true);
                UIobj[p_now].GetComponent<C_UI>().Character = Character_info[p_now];
                switch (iNum)
                {
                    case (int)Profession.Sword:
                        Character_Data[p_now].Sword_num = Sword_num;
                        Character_Data[p_now].character_Profession = (int)Profession.Sword;
                        Sword_num++;
                        
                        break;
                    case (int)Profession.Pounch:
                        Character_Data[p_now].Pounch_num = Pounch_num;
                        Character_Data[p_now].character_Profession = (int)Profession.Pounch;
                        Pounch_num++;
                        break;
                    case (int)Profession.Magic:
                        Character_Data[p_now].Magic_num = Magic_num;
                        Character_Data[p_now].character_Profession = (int)Profession.Magic;
                        Magic_num++;
                        break;
                    case (int)Profession.Gun:
                        Character_Data[p_now].Gun_num = Gun_num;
                        Character_Data[p_now].character_Profession = (int)Profession.Gun;
                        Gun_num++;
                        break;
                    case (int)Profession.Assassin:
                        Character_Data[p_now].Assassin_num = Assassin_num;
                        Character_Data[p_now].character_Profession = (int)Profession.Assassin;
                        Assassin_num++;
                        break;
                }
                
                switch (p_now)
                {
                    case 0:
                        Character_info[p_now].tag = "Player1";
                        break;
                    case 1:
                        Character_info[p_now].tag = "Player2";
                        break;
                    case 2:
                        Character_info[p_now].tag = "Player3";
                        break;
                    case 3:
                        Character_info[p_now].tag = "Player4";
                        break;
                }
                p_now++;
            }
        }
        p_now = 0;
        if(cpu_num != 0)
        {
            foreach (var iNum in cpu_Profession)
            {
                Character_info[p_now + player_num] = Instantiate(Profession_prefeb[iNum], Rebron_pos[p_now + player_num].position, Rebron_pos[p_now + player_num].rotation);
                Character_Data[p_now + player_num] = Character_info[p_now + player_num].GetComponent<CharacterData>();
                Character_Data[p_now + player_num].characterTeam = character_team[p_now + player_num];
                Character_Data[p_now + player_num].GC = this;
                Character_Data[p_now + player_num].characterNumber = p_now + player_num;
                UIobj[p_now + player_num] = UI_MAP.GetComponent<CharacterUI>().UI_OBJ[p_now + player_num];
                UIobj[p_now + player_num].SetActive(true);
                UIobj[p_now + player_num].GetComponent<C_UI>().Character = Character_info[p_now + player_num];
                switch (iNum)
                {

                    case (int)Profession.Sword:
                        Character_Data[p_now + player_num].character_Profession = (int)Profession.Sword;
                        break;
                    case (int)Profession.Pounch:
                        Character_Data[p_now + player_num].character_Profession = (int)Profession.Pounch;
                        break;
                    case (int)Profession.Magic:
                        Character_Data[p_now + player_num].character_Profession = (int)Profession.Magic;
                        break;
                    case (int)Profession.Gun:
                        Character_Data[p_now + player_num].character_Profession = (int)Profession.Gun;
                        break;
                    case (int)Profession.Assassin:
                        Character_Data[p_now + player_num].character_Profession = (int)Profession.Assassin;
                        break;
                }
                switch (p_now)
                {
                    case 0:
                        Character_info[p_now + player_num].tag = "CPU1";
                        break;
                    case 1:
                        Character_info[p_now + player_num].tag = "CPU2";
                        break;
                    case 2:
                        Character_info[p_now + player_num].tag = "CPU3";
                        break;
                    case 3:
                        Character_info[p_now + player_num].tag = "CPU4";
                        break;
                    case 4:
                        Character_info[p_now + player_num].tag = "CPU5";
                        break;
                    case 5:
                        Character_info[p_now + player_num].tag = "CPU6";
                        break;
                    case 6:
                        Character_info[p_now + player_num].tag = "CPU7";
                        break;
                    case 7:
                        Character_info[p_now + player_num].tag = "CPU8";
                        break;
                    case 8:
                        Character_info[p_now + player_num].tag = "CPU9";
                        break;
                    case 9:
                        Character_info[p_now + player_num].tag = "CPU10";
                        break;
                    case 10:
                        Character_info[p_now + player_num].tag = "CPU11";
                        break;
                    case 11:
                        Character_info[p_now + player_num].tag = "CPU12";
                        break;
                    case 12:
                        Character_info[p_now + player_num].tag = "CPU13";
                        break;
                    case 13:
                        Character_info[p_now + player_num].tag = "CPU14";
                        break;
                    case 14:
                        Character_info[p_now + player_num].tag = "CPU15";
                        break;
                    case 15:
                        Character_info[p_now + player_num].tag = "CPU16";
                        break;
                }
                p_now++;
            }
        }
        
    }
    void SetStage()
    {
        Gamemode_choice[Gamemode_num].SetActive(true) ;
        if(Gamemode_num!=1)
        {
            NormalAndJump_Stage_Date[Stage_num].SetActive(true);
            for (int i = 0; i < 16; i++)
            {
                Rebron_pos[i] = NormalAndJump_Stage_Date[Stage_num].GetComponent<Stage_RebronPos>().RebornPos[i].transform;
            }
        }else
        {
            Pinball_Stage_Date[Stage_num].SetActive(true);
            for (int i = 0; i < 16; i++)
            {
                Rebron_pos[i] = Pinball_Stage_Date[Stage_num].GetComponent<Stage_RebronPos>().RebornPos[i].transform;
            }
        }

    }

    void Game_Set()
    {
        //時間制限
        if (GameTimeNow <= 0) GameSet = 1;
        //全滅
        for (int i = 0; i < 5; i++) Remaining_num[i] = 0;
        int liveman = 0;
        for (int i = 0; i < Character_Data.Length; i++)
        {
            if (Gamemode_num == 0 && Gamemode2_num == 1)
            {
                if (Character_Data[i].character_NowHP > 0)
                {
                    Remaining_num[Character_Data[i].characterTeam]++;
                    liveman++;
                }
            }
            else
            {
                if (Character_Data[i].character_NOW > 0)
                {
                    Remaining_num[Character_Data[i].characterTeam] += Character_Data[i].character_NOW;
                    liveman++;
                }
            }
        }
        if (liveman<=1&& GameSet==0) GameSet = 1;
        int alive = 0;
        for (int i = 0; i < 5; i++)
        {
            if (Remaining_num[i] > 0) alive++;  //現在存在のチーム数量
            if (alive > 2 || Remaining_num[0] > 2) break;
        }
        //チームが一つだけ(NOチーム)　メンバーは2以下　　　　2二つチームそしてNOチームの人数は一人以下
        if ((alive == 1 && liveman <= 2)|| (alive == 2 && Remaining_num[0] <= 1))
        {
            int check = 0;
            //チームが二つならば存在の人数をCHECKする
            if (alive == 2 && Remaining_num[0]==0)
            {
                foreach (var num in Remaining_num)
                {
                    if (num > 1) check++;
                }
            }
            if (check < 2)
            {
                setCharacter.Clear();
                setTeam.Clear();
                if (alive == 1)//Nチームのみ
                {
                    for (int i = 0; i < Character_Data.Length; i++)
                    {
                        if (Gamemode_num == 0 && Gamemode2_num == 1)
                        {
                            if (Character_Data[i].character_NowHP > 0)
                            {
                                setCharacter.Add(Character_Data[i].tag);
                                GameSet_Already[0] = true;
                            }
                        }
                        else
                        {
                            if (Character_Data[i].character_NOW == 1)
                            {
                                setCharacter.Add(Character_Data[i].tag);
                                GameSet_Already[0] = true;
                            }
                        }
                        if (Character_Data[i].killnum >= 98) GameSet_Already[0] = true;
                    }
                }
                else if (alive == 2)//複数チーム　
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (Remaining_num[i] > 1) continue;
                        for (int j = 0; j < Character_Data.Length; j++)
                        {
                            if (Gamemode_num == 0 && Gamemode2_num == 1)
                            {
                                if (Character_Data[j].character_NowHP > 0)
                                {
                                    if (i == 0) setCharacter.Add(Character_Data[j].tag);
                                    else setTeam.Add(Character_Data[j].characterTeam);
                                    GameSet_Already[0] = true;
                                }
                            }
                            else
                            {
                                if (Character_Data[j].character_NOW == 1)
                                {
                                    if (i == 0) setCharacter.Add(Character_Data[j].tag);
                                    else setTeam.Add(Character_Data[j].characterTeam);
                                    GameSet_Already[0] = true;
                                }
                            }
                            if (Character_Data[j].killnum >= 98) GameSet_Already[0] = true;
                        }
                    }
                }
            }
            else
            {
                setCharacter.Clear();
                setTeam.Clear();
                GameSet_Already[0] = false;
            }
        }
        else
        {
            setCharacter.Clear();
            setTeam.Clear();
            GameSet_Already[0] = false;
        }
        //点数判定
        if (Gamemode_num!=0)
        {
            for (int i = 0; i < Character_Data.Length; i++)
            {
                int setpoint = GamePointMax - 1;
                if (Gamemode_num == 1) setpoint = GamePointMax - 3;
                if (Character_Data[i].characterTeam == 0 && (Character_Data[i].Character_Point >= (setpoint) || Character_Data[i].Character_Point >= 98))
                {
                    setCharacter_point[i] = Character_Data[i].tag;
                    GameSet_Already[1] = true;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int setpoint = GamePointMax - 1;
                if (Gamemode_num == 1) setpoint = GamePointMax - 3;
                if (Team_Point[i] >= setpoint || Team_Point[i] >= 98)
                {
                    GameSet_Already[1] = true;
                    break;
                }
            }
            if ( Gamemode_num==1)
            {
                int teamA = 0;
                int teamB = 0;
                for (int i = 0; i < Character_Data.Length; i++)
                {
                    if (Character_Data[i].characterTeam == 1) teamA += Character_Data[i].Character_Point;
                    else if (Character_Data[i].characterTeam == 2) teamB += Character_Data[i].Character_Point;
                }
                
            }
        }
    }
    void SetKill()
    {
        killnowUp = 0;
        killnowDown = 0;
        for (int i = 0; i < 4; i++) TeamKill[i] = 0;
        for (int i = (Character_info.Length-1); i >= 0; i--)
        {
            switch (Character_Data[i].characterTeam)
            {
                case 0:
                    if (Character_Data[i].killnum >= killnowUp)
                    {
                        Advantage = Character_Data[i].tag;
                        killnowUp = Character_Data[i].killnum;
                    }
                    if (Character_Data[i].killnum <= killnowDown)
                    {
                        Disadvantages = Character_Data[i].tag;
                        killnowDown = Character_Data[i].killnum;
                    }
                    break;
                case 1://Aチーム
                    TeamKill[0] += Character_Data[i].killnum;
                    break;
                case 2://Bチーム
                    TeamKill[1] += Character_Data[i].killnum;
                    break;
                case 3://Cチーム
                    TeamKill[2] += Character_Data[i].killnum;
                    break;
                case 4://Dチーム
                    TeamKill[3] += Character_Data[i].killnum;
                    break;
            }
        }
        for(int i=0;i<4;i++)
        {
            if(TeamKill[i]!=0)
            {
                if (TeamKill[i] > killnowUp)
                {
                    if (i == 0) Advantage = "赤チーム";
                    else if (i == 1) Advantage = "青チーム";
                    else if (i == 2) Advantage = "紫チーム";
                    else if (i == 3) Advantage = "黄チーム";
                    killnowUp = TeamKill[i];
                }
                if(killnowDown!=0)
                {
                    if (TeamKill[i] <= killnowDown)
                    {
                        if (i == 0) Disadvantages = "赤チーム";
                        else if (i == 1) Disadvantages = "青チーム";
                        else if (i == 2) Disadvantages = "紫チーム";
                        else if (i == 3) Disadvantages = "黄チーム";
                        killnowDown = TeamKill[i];
                    }
                }else
                {
                    int[] teamuse = new int[4];
                    for (int j = 0; j < GI.character_team.Count; j++)
                    {
                        if (GI.character_team[j] == 1) teamuse[0]++;
                        else if (GI.character_team[j] == 2) teamuse[1]++;
                        else if (GI.character_team[j] == 3) teamuse[2]++;
                        else if (GI.character_team[j] == 4) teamuse[3]++;
                    }
                    bool setok = false;
                    for (int j = 0; j < 4; j++)
                    {
                        if (teamuse[j] > 0&& setok==false)
                        {
                            killnowDown = TeamKill[j];
                            if (j == 0) Disadvantages = "赤チーム";
                            else if (j == 1) Disadvantages = "青チーム";
                            else if (j == 2) Disadvantages = "紫チーム";
                            else if (j == 3) Disadvantages = "黄チーム";
                            setok = true;
                        }
                        if (setok && teamuse[j] > 0)
                        {
                            if (TeamKill[j] < killnowDown)
                            {
                                if (j == 1) Disadvantages = "青チーム";
                                else if (j == 2) Disadvantages = "紫チーム";
                                else if (j == 3) Disadvantages = "黄チーム";
                            }
                        }
                    }
                }
                
            }
        }

        if(Gamemode_num==0&&Gamemode2_num==1)
        {
            int teamALife = 0;
            int teamBLife = 0;
            int teamCLife = 0;
            int teamDLife = 0;
            for (int i=0;i<Character_Data.Length;i++)
            {
                if(Character_Data[i].characterTeam!=0&& Character_Data[i].character_NowHP>0)
                {
                    switch(Character_Data[i].characterTeam)
                    {
                        case 1:
                            teamALife++;
                            break;
                        case 2:
                            teamBLife++;
                            break;
                        case 3:
                            teamCLife++;
                            break;
                        case 4:
                            teamDLife++;
                            break;
                    }
                }
            }
            if (teamALife == 0)DeadTeam[0] = true;
            else DeadTeam[0] = false;
            if (teamBLife == 0) DeadTeam[1] = true;
            else DeadTeam[1] = false;
            if (teamCLife == 0) DeadTeam[2] = true;
            else DeadTeam[2] = false;
            if (teamDLife == 0) DeadTeam[3] = true;
            else DeadTeam[3] = false;
        }
    }
    void SetPoint()
    {
        PointnowUp = 0;
        PointnowDown = 0;
        for (int i = 0; i < 4; i++) Team_Point[i] = 0;
        int zeropoint=-1;
        for (int i = (Character_info.Length - 1); i >= 0; i--)
        {
            switch (Character_Data[i].characterTeam)
            {
                case 0:
                    if (Character_Data[i].Character_Point >= PointnowUp)
                    {
                        Advantage = Character_Data[i].tag;
                        PointnowUp = Character_Data[i].Character_Point;
                    }
                    if (Character_Data[i].Character_Point <= PointnowDown)
                    {
                        Disadvantages = Character_Data[i].tag;
                        PointnowDown = Character_Data[i].Character_Point;
                    }else
                    {
                        if (Character_Data[i].Character_Point == 0)
                        {
                            zeropoint = i;
                        }
                    }
                    break;
                case 1://Aチーム
                    Team_Point[0] += Character_Data[i].Character_Point;
                    break;
                case 2://Bチーム
                    Team_Point[1] += Character_Data[i].Character_Point;
                    break;
                case 3://Cチーム
                    Team_Point[2] += Character_Data[i].Character_Point;
                    break;
                case 4://Dチーム
                    Team_Point[3] += Character_Data[i].Character_Point;
                    break;
            }
        }
        if(zeropoint!=-1)
        {
            PointnowDown = Character_Data[zeropoint].Character_Point;
            Disadvantages = Character_Data[zeropoint].tag;
        }
        Team_Point[0] += TeamPointPlus[0];
        Team_Point[1] += TeamPointPlus[1];
        for (int i = 0; i < 4; i++)
        {
            if (Team_Point[i] != 0)
            {
                if (Team_Point[i] > PointnowUp)
                {
                    if (i == 0)
                    {
                        Advantage = "赤チーム";
                        if (Gamemode_num == 1)
                        {
                            Disadvantages = "青チーム";
                            PointnowDown = Team_Point[1];
                        }
                    }
                    else if (i == 1)
                    {
                        Advantage = "青チーム";
                        if (Gamemode_num == 1)
                        {
                            Disadvantages = "赤チーム";
                            PointnowDown = Team_Point[0];
                        }
                    }
                    else if (i == 2) Advantage = "紫チーム";
                    else if (i == 3) Advantage = "黄チーム";
                    PointnowUp = Team_Point[i];
                }
                if (PointnowDown != 0)
                {
                    if (Team_Point[i] <= PointnowDown)
                    {
                        if (i == 0) Disadvantages = "赤チーム";
                        else if (i == 1) Disadvantages = "青チーム";
                        else if (i == 2) Disadvantages = "紫チーム";
                        else if (i == 3) Disadvantages = "黄チーム";
                        PointnowDown = Team_Point[i];
                    }
                }
                else
                {
                    int[] teamuse = new int[4];
                    for (int j = 0; j < GI.character_team.Count; j++)
                    {
                        if (GI.character_team[j] == 1) teamuse[0]++;
                        else if (GI.character_team[j] == 2) teamuse[1]++;
                        else if (GI.character_team[j] == 3) teamuse[2]++;
                        else if (GI.character_team[j] == 4) teamuse[3]++;
                    }
                    bool setok = false;
                    for (int j = 0; j < 4; j++)
                    {
                        if (teamuse[j] > 0 && setok == false)
                        {
                            PointnowDown = Team_Point[j];
                            if (j == 0) Disadvantages = "赤チーム";
                            else if (j == 1) Disadvantages = "青チーム";
                            else if (j == 2) Disadvantages = "紫チーム";
                            else if (j == 3) Disadvantages = "黄チーム";
                            setok = true;
                        }
                        if (setok && teamuse[j] > 0)
                        {
                            if (Team_Point[j] < PointnowDown)
                            {
                                if (j == 1) Disadvantages = "青チーム";
                                else if (j == 2) Disadvantages = "紫チーム";
                                else if (j == 3) Disadvantages = "黄チーム";
                            }
                        }
                    }
                }

            }
        }
        if (Gamemode_num == 1&&(Team_Point[0] >= GamePointMax || Team_Point[1] >= GamePointMax))
        {
            GameSet = 3;
        }
    }
    void PasueManu()
    {
        if (Pause)
        {
            if (Pause_watchGame==false&& Pause_chiocing==false)
            {
                if (Input.GetButtonDown("Up") || Input.GetButtonDown("JoyUp"))
                {
                    Pause_chioce--;
                    audiosource.PlayOneShot(effect[3]);
                }
                if (Input.GetButtonDown("Down")|| Input.GetButtonDown("JoyDown"))
                {
                    Pause_chioce++;
                    audiosource.PlayOneShot(effect[3]);
                }
                
            }
            if (Pause_chiocing == false)
            {
                if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                {
                    Pause_chiocing = true;
                    if(Pause_chioce==1)
                    {
                        Pause_watchGame = true;
                        audiosource.PlayOneShot(effect[4]);
                    }
                }
                Time.timeScale = 0;
            }
            else
            {
                switch (Pause_chioce)
                {
                    case 0:
                        Pause_chiocing = false;
                        Pause = false;
                        G_UI.Pasue_manu[4].SetActive(false);
                        audiosource.PlayOneShot(effect[2]);
                        break;
                    case 1:
                        if (Pause_watchGame&& (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4")
                            || Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Return)))
                        {
                            Pause_watchGame = false;
                            Pause_chiocing = false;
                            audiosource.PlayOneShot(effect[4]);
                        }
                        break;
                    case 2:
                        Time.timeScale = 1;
                        Gamestate = 7;
                        audiosource.PlayOneShot(effect[5]);
                        break;
                    case 3:
                        Time.timeScale = 1;
                        Gamestate = 8;
                        audiosource.PlayOneShot(effect[5]);
                        break;
                }
            }
        }
        else
        {
            if(GI.SlowDownNow == 0) Time.timeScale = 1;
            Pause_chioce = 0;
            Pause_chiocing = false;
            Pause_watchGame = false;
        }

        if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Return))
        {
            Pause = !Pause;
            if (Pause) audiosource.PlayOneShot(effect[1]);
            else audiosource.PlayOneShot(effect[2]);
        }
        GI.MAXandMin(Pause_chioce, 0, 3);
    }
    void TimeControler()
    {
        GameTimeNow--;
    }
}
