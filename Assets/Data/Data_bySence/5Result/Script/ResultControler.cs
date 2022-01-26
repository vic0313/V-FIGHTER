using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultControler : MonoBehaviour
{
    public GameInfo GI;
    public AudioClip[] effect;
    public AudioSource audiosource;
    private int resultStep;
    public Text[] resultInfo;
    public Text[] resultInfo_team;
    public Text[] resultInfo_teamKill;
    public Text[] resultInfo_teamPoint;
    public Image[] resultInfo_teamImage;
    public Text[] resultInfo_character;
    public Text[] resultInfo_belongTeam;
    public Text[] resultInfo_Kill;
    public Text[] resultInfo_Point;
    public Text[] resultInfo_BeKill;
    public Text Itemkill;
    private int chiocenow;
    private int chiocenext;
    public GameObject[] chiocenextpos;
    private bool chiocecheck;
    public GameObject[] Profession_prefeb;              //職業の物件
    public GameObject[] Character_info;                 //生成の物件のGameObjの指向
    public Transform[] Rebron_pos_win = new Transform[16];   //キャラクター再生位置
    public Transform[] Rebron_pos_loss = new Transform[16];   //キャラクター再生位置
    public Charactercontrol_Result[] Character_Data;              //生成の物件のGameObjのData Scriptの指向
    public int winteam;             //0:A 1:B 2C 3D
    public int win_character_num;
    private int MaxKill;
    private int MaxPoint;
    private float MaxLife;
    public GameObject[] winner;
    public Text[] winner_text;
    public Image[] winner_bg;
    public GameObject[] losser;
    public Text[] losser_text;
    public Image[] losser_bg;
    public Transform[] Firepos;
    public GameObject[] fireworks_prefab;
    public Text winmark;
    public GameObject[] ScenceNext;
    public Image[] resultmenu;
    public Animator[] resultmenu_animator;
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
        chiocenow = 0;
        chiocenext = 0;
        chiocecheck = false;
        resultmenu[0].enabled = false;
        resultmenu[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(resultStep==0)
        {
            resultStep = 0;
            GI = GameInfo.game_data;
            MaxLife = 0;
            MaxKill = 0;
            MaxPoint = 0;
            winteam = 0;
             win_character_num = -1;
            Character_info = new GameObject[GI.player_num+GI.cpu_num];
            Character_Data = new Charactercontrol_Result[GI.player_num + GI.cpu_num];
            if (GI.Gamemode_choice == 0)
            {
                for (int i = 0; i < GI.TeamKillAll.Length; i++)
                {
                    if (GI.TeamKillAll[i] > MaxKill)
                    {
                        MaxKill = GI.TeamKillAll[i];
                        winteam = (i + 1);
                        MaxLife = 0;
                        for (int j = 0; j < GI.character_team.Count; j++)
                        {
                            if (GI.character_team[j] == winteam)MaxLife += GI.character_Lifenow[j];
                        }
                    }
                    else if (GI.TeamKillAll[i] == MaxKill)//同点数の状況
                    {
                        float sum = 0;
                        for(int j=0;j<GI.character_team.Count;j++)
                        {
                            if (GI.character_team[j] == (i+1))sum += GI.character_Lifenow[j];
                        }
                        if(sum> MaxLife)
                        {
                            MaxKill = GI.TeamKillAll[i];
                            winteam = (i + 1);
                            MaxLife = sum;
                        }
                    }
                }
                for (int i = (GI.character_Kill_sum.Count-1); i >= 0; i--)
                {
                    if (GI.character_team[i] == 0 && GI.character_Kill_sum[i] > MaxKill)
                    {
                        win_character_num = i;
                        MaxKill = GI.character_Kill_sum[i];
                        if(GI.player_life_max!=4) MaxLife= GI.character_Lifenow[i];
                    }
                    else if (GI.character_team[i] == 0 && GI.character_Kill_sum[i] == MaxKill&&GI.player_life_max != 4)
                    {
                        if (GI.character_Lifenow[i] > MaxLife)
                        {
                            win_character_num = i;
                            MaxKill = GI.character_Kill_sum[i];
                            MaxLife = GI.character_Lifenow[i];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GI.TeamPointAll.Length; i++)
                {
                    if (GI.TeamPointAll[i] > MaxPoint)
                    {
                        MaxPoint = GI.TeamPointAll[i];
                        winteam = (i + 1);
                        MaxLife = 0;
                        if(GI.player_life_max != 4)
                        {
                            for (int j = 0; j < GI.character_team.Count; j++)
                            {
                                if (GI.character_team[j] == winteam)
                                {
                                    MaxLife += GI.character_Lifenow[j];
                                }
                            }
                        }
                       
                    }
                    else if (GI.TeamPointAll[i] == MaxPoint&& GI.player_life_max != 4)
                    {
                        float sum = 0;
                        for (int j = 0; j < GI.character_team.Count; j++)
                        {
                            if (GI.character_team[j] == (i + 1)) sum += GI.character_Lifenow[j];
                        }
                        if (sum > MaxLife)
                        {
                            MaxPoint = GI.TeamPointAll[i];
                            winteam = (i + 1);
                            MaxLife = sum;
                        }
                    }
                }
                if (GI.Gamemode_choice == 2)
                {
                    for (int i = (GI.character_Point_sum.Count - 1); i >= 0; i--)
                    {
                        if (GI.character_team[i] == 0 && GI.character_Point_sum[i] > MaxPoint)
                        {
                            win_character_num = i;
                            MaxPoint = GI.character_Point_sum[i];
                            if (GI.player_life_max != 4) MaxLife = GI.character_Lifenow[i];
                        }
                        else if (GI.character_team[i] == 0 && GI.character_Point_sum[i] == MaxPoint&&GI.player_life_max != 4)
                        {
                            if (GI.character_Lifenow[i] > MaxLife)
                            {
                                win_character_num = i;
                                MaxPoint = GI.character_Point_sum[i];
                                MaxLife = GI.character_Lifenow[i];
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < winner.Length; i++)
            {
                winner[i].SetActive(false);
                losser[i].SetActive(false);
            }
            SetCharacter();
            InvokeRepeating("Fireworks",0,0.5f);
            if (win_character_num >= 0) winmark.text = Character_info[win_character_num].tag;
            else
            {
                switch (winteam)
                {
                    case 1://A
                        winmark.text = "赤チーム";
                        break;
                    case 2://B
                        winmark.text = "青チーム";
                        break;
                    case 3://C
                        winmark.text = "紫チーム";
                        break;
                    case 4://D
                        winmark.text = "黄チーム";
                        break;
                }
            }
            //resultInfo[0][1]
            for (int i = 0; i < GI.character_team.Count; i++)
            {
                resultInfo_character[i].gameObject.SetActive(true);
                resultInfo_belongTeam[i].gameObject.SetActive(true);
                resultInfo_Kill[i].gameObject.SetActive(true);
                resultInfo_Point[i].gameObject.SetActive(true);
                resultInfo_BeKill[i].gameObject.SetActive(true);
            }
            for (int i = GI.character_team.Count; i < (GI.player_num + GI.cpu_num); i++)
            {
                resultInfo_character[i].gameObject.SetActive(false);
                resultInfo_belongTeam[i].gameObject.SetActive(false);
                resultInfo_Kill[i].gameObject.SetActive(false);
                resultInfo_Point[i].gameObject.SetActive(false);
                resultInfo_BeKill[i].gameObject.SetActive(false);
            }
            switch (GI.Gamemode_choice)
            {
                case 0:
                    //V battle 
                    resultInfo[0].text = "V Battle";
                    if (GI.game_mode_chioce == 0) resultInfo[1].text = "Normal  モード";
                    else resultInfo[1].text = "Zombie  モード";
                    //勝利条件　1.時間終わった時命とHPが一番高い人とチーム　2.敵チーム全滅時
                    break;
                case 1:
                    //BB battle
                    resultInfo[0].text = "BB Battle";
                    if (GI.game_mode_chioce == 0) resultInfo[1].text = "20点勝利  モード";
                    else if (GI.game_mode_chioce == 1) resultInfo[1].text = "40点勝利  モード";
                    else if (GI.game_mode_chioce == 2) resultInfo[1].text = "無限  モード";
                    //勝利条件　1.時間終わった時点数高いチーム　2.点数が足りる時
                    break;
                case 2:
                    //J battle
                    resultInfo[0].text = "J Battle";
                    if (GI.game_mode_chioce == 0) resultInfo[1].text = "20点勝利  モード";
                    else if (GI.game_mode_chioce == 1) resultInfo[1].text = "40点勝利  モード";
                    else if (GI.game_mode_chioce == 2) resultInfo[1].text = "無限  モード";
                    //勝利条件　1.時間終わった時点数高いチーム　2.点数が足りる時
                    break;
            }
            resultInfo[2].text = "Player人数:" + "" + GI.player_num + "人   CPU人数:" + "" + GI.cpu_num;

            int[] teamuse = new int[4];
            for(int i=0;i<GI.character_team.Count;i++)
            {
                if (GI.character_team[i] == 1) teamuse[0]++;
                else if (GI.character_team[i] == 2) teamuse[1]++;
                else if (GI.character_team[i] == 3) teamuse[2]++;
                else if (GI.character_team[i] == 4) teamuse[3]++;
            }
            int teamnum = 0;
            for (int j = 0; j < teamuse.Length; j++)
            {
                if (teamuse[j] != 0)
                {
                    resultInfo_teamImage[teamnum].gameObject.SetActive(true);
                    switch (j)
                    {
                        case 0:
                            resultInfo_teamImage[teamnum].color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
                            resultInfo_team[teamnum].text = "赤チーム";
                            break;
                        case 1:
                            resultInfo_teamImage[teamnum].color = new Color(0.0f, 0.0f, 1.0f, 0.3f);
                            resultInfo_team[teamnum].text = "青チーム";
                            break;
                        case 2:
                            resultInfo_teamImage[teamnum].color = new Color(1.0f, 0.0f, 1.0f, 0.3f);
                            resultInfo_team[teamnum].text = "紫チーム";
                            break;
                        case 3:
                            resultInfo_teamImage[teamnum].color = new Color(1.0f, 0.92f, 0.0016f, 0.3f);
                            resultInfo_team[teamnum].text = "黄チーム";
                            break;
                    }
                    resultInfo_teamKill[teamnum].text = "" + GI.TeamKillAll[j] + "人";
                    if(GI.Gamemode_choice!=0)resultInfo_teamPoint[teamnum].text = "" + GI.TeamPointAll[j] + "点";
                    else resultInfo_teamPoint[teamnum].text = "--";
                    teamnum++;
                }
             }
            for(int i= teamnum;i<4;i++) resultInfo_teamImage[i].gameObject.SetActive(false);
            for (int i = 0; i < GI.character_team.Count; i++)
            {
                if(i<10)resultInfo_character[i].text=""+(i+1)+"  : "+ Character_info[i].tag;
                else if (i >= 10) resultInfo_character[i].text = "" + i + ": " + Character_info[i].tag;
                switch(Character_Data[i].characterTeam)
                {
                    case 0:
                        resultInfo_belongTeam[i].text = "無所属";
                        break;
                    case 1:
                        resultInfo_belongTeam[i].text = "赤チーム";
                        break;
                    case 2:
                        resultInfo_belongTeam[i].text = "青チーム";
                        break;
                    case 3:
                        resultInfo_belongTeam[i].text = "紫チーム";
                        break;
                    case 4:
                        resultInfo_belongTeam[i].text = "黄チーム";
                        break;
                }
                
                resultInfo_Kill[i].text=""+ GI.character_Kill_sum[i]+"人";
                if (GI.Gamemode_choice != 0) resultInfo_Point[i].text = "" + GI.character_Point_sum[i]+"点";
                else resultInfo_Point[i].text = "--";
                resultInfo_BeKill[i].text = "" + GI.character_BeKill_sum[i] + "回";
                
            }
            Itemkill.text = "アイテムの撃破数:" + "" + GI.Item_kill;


            Invoke("resultOK", 4);
            resultStep = 1;
        }
        else if(resultStep==2)
        {
            if(chiocecheck==false)
            {
                if (chiocenow == 1) resultmenu[0].enabled = true;
                else resultmenu[0].enabled = false;
                if (chiocenow == 2) resultmenu[1].enabled = true;
                else resultmenu[1].enabled = false;
                chiocenext = 0;
                if (Input.GetButtonDown("Left") || Input.GetButtonDown("JoyUp"))
                {
                    chiocenow--;
                    audiosource.PlayOneShot(effect[0]);
                }
                else if (Input.GetButtonDown("Right") || Input.GetButtonDown("JoyDown"))
                {
                    chiocenow++;
                    audiosource.PlayOneShot(effect[0]);
                }
                else if(GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f)
                {
                    if (GI.input_h_All_Joy > 0.3f)
                    {
                        chiocenow++;
                        audiosource.PlayOneShot(effect[0]);
                    }
                    else if (GI.input_h_All_Joy < (-0.3f))
                    {
                        chiocenow--;
                        audiosource.PlayOneShot(effect[0]);
                    }
                    GI.InputJoycheck_all[1] = false;
                    GI.input_h_All_Joy = 0;
                    GI.JoycheckStart = true;
                    GI.InvokeJoyCheck();
                }
                chiocenow = GI.MAXandMin(chiocenow, 1, 2);
                if (chiocenow!=0&&(Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return)))
                {
                    chiocecheck = true;
                    if(chiocenow==1)resultmenu_animator[0].SetBool("OK", true);
                    else if (chiocenow == 2) resultmenu_animator[1].SetBool("OK", true);
                    audiosource.PlayOneShot(effect[1]);
                }
            }
            else
            {
                resultmenu[0].enabled = false;
                resultmenu[1].enabled = false;

                if(chiocenow==2)
                {
                    chiocenextpos[0].transform.position = chiocenextpos[chiocenext + 1].transform.position;
                    if (Input.GetButtonDown("Up") || Input.GetButtonDown("JoyUp"))
                    {
                        chiocenext--;
                        audiosource.PlayOneShot(effect[0]);
                    }
                    else if (Input.GetButtonDown("Down") || Input.GetButtonDown("JoyDown"))
                    {
                        chiocenext++;
                        audiosource.PlayOneShot(effect[0]);
                    }
                    else if (GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f)
                    {
                        if (GI.input_v_All_Joy > 0.3f)
                        {
                            chiocenext--;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        else if (GI.input_v_All_Joy < (-0.3f))
                        {
                            chiocenext++;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        GI.InputJoycheck_all[0] = false;
                        GI.input_v_All_Joy = 0;
                        GI.JoycheckStart = true;
                        GI.InvokeJoyCheck();
                    }

                    chiocenext = GI.MAXandMin(chiocenext, 0, 1);
                }
                
                if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                {
                    if (chiocenow == 1)
                    {
                        chiocecheck = false;
                        chiocenow = 0;
                        resultmenu_animator[0].SetBool("OK", false);
                        audiosource.PlayOneShot(effect[2]);
                    }
                    else if (chiocenow == 2)
                    {
                        if(chiocenext==0)//ゲームRESET
                        {
                            ScenceNext[0].SetActive(true);
                        }
                        else if (chiocenext == 1)//BacktoMAin
                        {
                            ScenceNext[1].SetActive(true);
                        }
                        audiosource.PlayOneShot(effect[3]);
                        resultStep = 3;
                    }
                }
                    else if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                {
                    if (chiocenow == 1) resultmenu_animator[0].SetBool("OK", false);
                    else if (chiocenow == 2) resultmenu_animator[1].SetBool("OK", false);
                    audiosource.PlayOneShot(effect[2]);
                    chiocecheck = false;
                    chiocenow = 0;
                }
            }
        }
    }
    void SetCharacter()
    {
        //キャラクタの生成
        int p_now = 0;
        int winposnum = 0;
        int lossposnum = 0;
        if (GI.player_num != 0)
        {
            int Sword_num = 0;
            int Pounch_num = 0;
            int Magic_num = 0;
            int Gun_num = 0;
            int Assassin_num = 0;
            foreach (var iNum in GI.player_Profession)
            {
                if(win_character_num<0)//TEAM勝利
                {
                    if(GI.character_team[p_now]== winteam)winposnum=WinPlayer(p_now, iNum, winposnum);
                    else lossposnum = LossPlayer(p_now, iNum, lossposnum);
                }
                else
                {
                    if (p_now == win_character_num) winposnum = WinPlayer(p_now, iNum, winposnum); 
                    else lossposnum = LossPlayer(p_now, iNum, lossposnum);
                }
                
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
                p_now++;
            }
        }
        p_now = 0;
        if (GI.cpu_num != 0)
        {
            foreach (var iNum in GI.cpu_Profession)
            {
                if (win_character_num < 0)//TEAM勝利
                {
                    if (GI.character_team[p_now + GI.player_num] == winteam) winposnum = WinCPU(p_now, iNum, winposnum);
                    else lossposnum = LossCPU(p_now, iNum, lossposnum);
                }
                else
                {
                    if ((p_now + GI.player_num) == win_character_num) winposnum = WinCPU(p_now, iNum, winposnum);
                    else lossposnum = LossCPU(p_now, iNum, lossposnum);
                }
                
                switch (iNum)
                {
                    case (int)Profession.Sword:
                        Character_Data[p_now + GI.player_num].character_Profession = (int)Profession.Sword;
                        break;
                    case (int)Profession.Pounch:
                        Character_Data[p_now + GI.player_num].character_Profession = (int)Profession.Pounch;
                        break;
                    case (int)Profession.Magic:
                        Character_Data[p_now + GI.player_num].character_Profession = (int)Profession.Magic;
                        break;
                    case (int)Profession.Gun:
                        Character_Data[p_now + GI.player_num].character_Profession = (int)Profession.Gun;
                        break;
                    case (int)Profession.Assassin:
                        Character_Data[p_now + GI.player_num].character_Profession = (int)Profession.Assassin;
                        break;
                }
                p_now++;
            }
        }

    }
    void SetP_Tag(int p_now)
    {
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
    }
    void SetC_Tag(int p_now)
    {
        switch (p_now)
        {
            case 0:
                Character_info[p_now + GI.player_num].tag = "CPU1";
                break;
            case 1:
                Character_info[p_now + GI.player_num].tag = "CPU2";
                break;
            case 2:
                Character_info[p_now + GI.player_num].tag = "CPU3";
                break;
            case 3:
                Character_info[p_now + GI.player_num].tag = "CPU4";
                break;
            case 4:
                Character_info[p_now + GI.player_num].tag = "CPU5";
                break;
            case 5:
                Character_info[p_now + GI.player_num].tag = "CPU6";
                break;
            case 6:
                Character_info[p_now + GI.player_num].tag = "CPU7";
                break;
            case 7:
                Character_info[p_now + GI.player_num].tag = "CPU8";
                break;
            case 8:
                Character_info[p_now + GI.player_num].tag = "CPU9";
                break;
            case 9:
                Character_info[p_now + GI.player_num].tag = "CPU10";
                break;
            case 10:
                Character_info[p_now + GI.player_num].tag = "CPU11";
                break;
            case 11:
                Character_info[p_now + GI.player_num].tag = "CPU12";
                break;
            case 12:
                Character_info[p_now + GI.player_num].tag = "CPU13";
                break;
            case 13:
                Character_info[p_now + GI.player_num].tag = "CPU14";
                break;
            case 14:
                Character_info[p_now + GI.player_num].tag = "CPU15";
                break;
            case 15:
                Character_info[p_now + GI.player_num].tag = "CPU16";
                break;
        }
    }
    int WinPlayer(int p_now,int iNum,int winposnum)
    {
        Character_info[p_now] = Instantiate(Profession_prefeb[iNum], Rebron_pos_win[winposnum].position, Rebron_pos_win[winposnum].rotation);
        Character_info[p_now].GetComponent<Animator>().SetBool("Win", true);
        winner[winposnum].SetActive(true);
        SetP_Tag(p_now);
        winner_text[winposnum].text = Character_info[p_now].tag;
        Character_Data[p_now] = Character_info[p_now].GetComponent<Charactercontrol_Result>();
        Character_Data[p_now].characterTeam = GI.character_team[p_now];
        Character_Data[p_now].winnerEffect.SetActive(true);
        switch (GI.character_team[p_now])
        {
            case 0://NO_チーム
                winner_bg[winposnum].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case 1://A_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case 2://B_チーム
                winner_bg[winposnum].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 3://C_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 4://D_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                break;
        }
        winposnum++;
        return winposnum;
    }
    int LossPlayer(int p_now, int iNum, int lossposnum)
    {
        Character_info[p_now] = Instantiate(Profession_prefeb[iNum], Rebron_pos_loss[lossposnum].position, Rebron_pos_loss[lossposnum].rotation);
        Character_info[p_now].GetComponent<Animator>().SetBool("Loss", true);
        losser[lossposnum].SetActive(true);
        SetP_Tag(p_now);
        losser_text[lossposnum].text = Character_info[p_now].tag;
        Character_Data[p_now] = Character_info[p_now].GetComponent<Charactercontrol_Result>();
        Character_Data[p_now].characterTeam = GI.character_team[p_now];
        Character_Data[p_now].winnerEffect.SetActive(false);
        switch (GI.character_team[p_now])
        {
            case 0://NO_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case 1://A_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case 2://B_チーム
                losser_bg[lossposnum].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 3://C_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 4://D_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                break;
        }
        lossposnum++;
        return lossposnum;
    }
    int WinCPU(int p_now, int iNum, int winposnum)
    {
        Character_info[p_now + GI.player_num] = Instantiate(Profession_prefeb[iNum], Rebron_pos_win[winposnum].position, Rebron_pos_win[winposnum].rotation);
        Character_info[p_now + GI.player_num].GetComponent<Animator>().SetBool("Win", true);
        winner[winposnum].SetActive(true);
        SetC_Tag(p_now);
        winner_text[winposnum].text = Character_info[p_now + GI.player_num].tag;
        Character_Data[p_now + GI.player_num] = Character_info[p_now + GI.player_num].GetComponent<Charactercontrol_Result>();
        Character_Data[p_now + GI.player_num].characterTeam = GI.character_team[p_now + GI.player_num];
        Character_Data[p_now + GI.player_num].winnerEffect.SetActive(true);
        switch (GI.character_team[p_now + GI.player_num])
        {
            case 0://NO_チーム
                winner_bg[winposnum].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case 1://A_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case 2://B_チーム
                winner_bg[winposnum].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 3://C_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 4://D_チーム
                winner_bg[winposnum].color = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                break;
        }
        winposnum++;
        return winposnum;
    }
    int LossCPU(int p_now, int iNum, int lossposnum)
    {
        Character_info[p_now + GI.player_num] = Instantiate(Profession_prefeb[iNum], Rebron_pos_loss[lossposnum].position, Rebron_pos_loss[lossposnum].rotation);
        Character_info[p_now + GI.player_num].GetComponent<Animator>().SetBool("Loss", true);
        losser[lossposnum].SetActive(true);
        SetC_Tag(p_now);
        losser_text[lossposnum].text = Character_info[p_now + GI.player_num].tag;
        Character_Data[p_now + GI.player_num] = Character_info[p_now + GI.player_num].GetComponent<Charactercontrol_Result>();
        Character_Data[p_now + GI.player_num].characterTeam = GI.character_team[p_now + GI.player_num];
        Character_Data[p_now + GI.player_num].winnerEffect.SetActive(false);
        switch (GI.character_team[p_now + GI.player_num])
        {
            case 0://NO_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case 1://A_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case 2://B_チーム
                losser_bg[lossposnum].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 3://C_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case 4://D_チーム
                losser_bg[lossposnum].color = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                break;
        }
        lossposnum++;
        return lossposnum;
    }

    void Fireworks()
    {
        Instantiate(fireworks_prefab[Random.Range(0,2)], Firepos[Random.Range(0, (Firepos.Length))].position, Firepos[Random.Range(0, (Firepos.Length))].rotation);
    }
    void resultOK()
    {
        resultStep = 2;
    }
}
