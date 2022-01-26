using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetControler : MonoBehaviour
{
    public GameInfo GI;
    public GameObject BackToScene1;
    public GameObject NextScenel;
    public GameObject[] cam;
    public AudioClip[] effect;
    public AudioSource audiosource;
    //SET1=================================================================================
    //キャラクターの数量とゲームモードの選択STEP
    public GameObject[] set1;
    public Animator[] set1_ani;
    public Animator[] set1_UpDown;
    //SET2=================================================================================
    //キャラクターの職業の選択STEP
    public Set2[] PC;
    public GameObject[] player_stage;
    public int[] playerchioce;
    public GameObject[] playerchioce_mark;
    public GameObject[] playerchioce_P1mark_POS;
    public GameObject[] playerchioce_P2mark_POS;
    public GameObject[] playerchioce_P3mark_POS;
    public GameObject[] playerchioce_P4mark_POS;
    public bool[] playerchioce_OK;
    public GameObject chioce_effect;
    public GameObject chiocecancel_effect;
    public Animator ballandSet3;
    public bool[] JoyCheck;
    //SET3=================================================================================
    public Animator TopC;
    public Set3 set3;
    public int TeamAnum;
    public int TeamBnum;
    public int set3_chioce_now;     //現在選択している選択目標  0~3 COMMON変数 4プレイヤ　5CPU 6MAP 7確認ボタン
    public int set3_chioce_now_P;   //プレイヤsettingエリア　今選択した目標は
    public int set3_chioce_now_C;   //CPUsettingエリア　今選択した目標は
    public int set3_chioce_now_C_area;   //CPUsettingエリア中のチーム選択エリアに　今選択した目標は
    public int set3_chioce_now_C_teammark;   //CPUsettingエリア中のチーム選択エリアに　今選択した目標は
    public int set3_chioce_now_C_teammark_num; //現在移動中のMARKナンバー
    public int set3_chioce_now_C_teammark_num_inteam; //現在移動中のMARKナンバー
    public int set3_chioce_now_C_teammark_fromteam; //現在移動中のMARK元チームは
    public int set3_chioce_now_C_teammark_job; //現在移動中のMARKのJOB
    public int set3_chioce_now_M;   //MAPsettingエリア　今選択した目標は
    public int[] set3_common;       //選択した変数の保存 common 
    //↑　0:命数　1.MP　2.勝利条件　3.TIME　4.ITEM
    public int[] set3_P;         //選択した変数の保存　PLAYERのチーム
    public int[] set3_C_job;         //選択した変数の保存　CPU職業数量  0:S 1:P 2:M 3:G 4:A　
    public List<int> set3_C_team_N = new List<int>();       //現在NOチーム中CPUの職業 0:S 1:P 2:M 3:G 4:A　
    public List<int> set3_C_team_A = new List<int>();       //現在Aチーム中CPUの職業 0:S 1:P 2:M 3:G 4:A　
    public List<int> set3_C_team_B = new List<int>();       //現在Bチーム中CPUの職業 0:S 1:P 2:M 3:G 4:A　
    public List<int> set3_C_team_C = new List<int>();       //現在Cチーム中CPUの職業 0:S 1:P 2:M 3:G 4:A　
    public List<int> set3_C_team_D = new List<int>();       //現在Dチーム中CPUの職業 0:S 1:P 2:M 3:G 4:A　
    public int set3_map;        //選択した変数の保存　MAP
    public bool set3_chioceing;             //選択中の判定
    public bool set3_chioceing_CPU;                 //CPU エリア選択中の判定
    public bool set3_chioceing_CPUteamarea;         //CPU エリア中のチームエリア選択中の判定
    public bool set3_chioceing_CPUteam_move;        //CPU エリア中のチームエリア中のチームMARK選択した移動中の判定
    public Set3_CPU set3_cpu;
    private void Awake()
    {
        GI = GameInfo.game_data;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam[GI.Gamemode_choice].SetActive(true);
        playerchioce = new int[4];
        playerchioce_OK = new bool[4];
        JoyCheck = new bool[4];
        set3_common = new int[4];
        set3_P = new int[4];
        set3_C_job = new int[5];
        for (int i = 0; i < 4; i++)
        {
            set3_common[i] = 0;
            set3_P[i] = 0;
            JoyCheck[i] = false;
            playerchioce[i] = 0;
            playerchioce_OK[i] = false;
        }
        for (int i = 0; i < 5; i++) set3_C_job[i] = 0;
        set3_map = 0;
        TeamAnum= 0;
        TeamBnum = 0;
        set3_chioce_now = 0;
        set3_chioce_now_P = 0;
        set3_chioce_now_C = 0;
        set3_chioce_now_M = 0;
        set3_chioceing = false;
        set3_chioceing_CPU = false;
        set3_chioceing_CPUteamarea = false;
        set3_chioceing_CPUteam_move = false;
        set3_chioce_now_C_area = 0;
        set3_chioce_now_C_teammark = 0;
        set3_chioce_now_C_teammark_num = 0;
        set3_chioce_now_C_teammark_fromteam = 0;
        set3_chioce_now_C_teammark_num_inteam = 0;
        set3_chioce_now_C_teammark_job = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (GI.SceneChoiceOK == false)
        {
            if (GI.chioce_now <= 0) GI.chioce_now = 0;
            GI.player_num = GI.MAXandMin(GI.player_num, 0, 4);
            GI.cpu_num = GI.MAXandMin(GI.cpu_num, 0, 12);
            if (GI.Gamemode_choice == 0) GI.game_mode_chioce = GI.MAXandMin(GI.game_mode_chioce, 0, 1);
            else GI.game_mode_chioce = GI.MAXandMin(GI.game_mode_chioce, 0, 2);

            if (GI.setingstate != 6)
            {
                for (int i = 0; i < 4; i++) playerchioce_mark[i].SetActive(false);
            }
            


            switch (GI.setingstate)
            {
                //SET1開始=================================================================================
                //0 ~1 開場動画
                case 0:
                    GI.player_Profession.Clear();
                    GI.cpu_Profession.Clear();
                    GI.character_team.Clear();
                    set3_C_team_N.Clear();
                    set3_C_team_A.Clear();
                    set3_C_team_B.Clear();
                    set3_C_team_C.Clear();
                    set3_C_team_D.Clear();
                    set1[0].SetActive(true);
                    break;
                case 1:
                    set1[1].SetActive(true);
                    break;
                case 2://開場動画は終了後
                    if ((GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f) || Input.GetButtonDown("Vertical player1") || Input.GetButtonDown("Vertical player2") || Input.GetButtonDown("Vertical player3") || Input.GetButtonDown("Vertical player4"))
                    {
                        if(GI.InputJoycheck_all[0])
                        {
                            if (GI.input_v_All_Joy < (-0.6f))
                            {
                                GI.chioce_now++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            else if (GI.input_v_All_Joy > 0.6f)
                            {
                                GI.chioce_now--;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            if (GI.chioce_now >= 3)
                            {
                                GI.chioce_now = 3;
                                
                            }
                            GI.input_v_All_Joy = 0;
                            GI.InputJoycheck_all[0] = false;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }
                        else
                        {
                            if (GI.input_v_All < 0)
                            {
                                GI.chioce_now++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            else if (GI.input_v_All > 0)
                            {
                                GI.chioce_now--;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            if (GI.chioce_now >= 3)
                            {
                                GI.chioce_now = 3;
                            }
                            
                        }
                        
                    }
                    else if ((GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f) || Input.GetButtonDown("Horizontal player1") || Input.GetButtonDown("Horizontal player2") || Input.GetButtonDown("Horizontal player3") || Input.GetButtonDown("Horizontal player4"))
                    {
                        if(GI.InputJoycheck_all[1])
                        {
                            switch (GI.chioce_now)
                            {
                                case 0:
                                    if (GI.input_h_All_Joy > 0.6f)
                                    {
                                        GI.player_num++;
                                        set1_UpDown[0].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.6f))
                                    {
                                        if ((GI.player_num + GI.cpu_num) >= 3) GI.player_num--;
                                        set1_UpDown[1].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    break;
                                case 1:
                                    if (GI.input_h_All_Joy > 0.6f)
                                    {
                                        GI.cpu_num++;
                                        set1_UpDown[2].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.6f))
                                    {
                                        if ((GI.player_num + GI.cpu_num) >= 3) GI.cpu_num--;
                                        set1_UpDown[3].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    break;
                                case 2:
                                    if (GI.input_h_All_Joy > 0.6f)
                                    {
                                        GI.game_mode_chioce++;
                                        set1_UpDown[4].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.6f))
                                    {
                                        GI.game_mode_chioce--;
                                        set1_UpDown[5].Play("push");
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    break;
                            }
                            GI.InputJoycheck_all[1] = false;
                            GI.input_h_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }
                        else
                        {
                            switch (GI.chioce_now)
                            {
                                case 0:
                                    if (GI.input_h_All > 0)
                                    {
                                        GI.player_num++;
                                        set1_UpDown[0].Play("push");
                                    }
                                    else if (GI.input_h_All <0)
                                    {
                                        if ((GI.player_num + GI.cpu_num) >= 3) GI.player_num--;
                                        set1_UpDown[1].Play("push");
                                    }
                                    break;
                                case 1:
                                    if (GI.input_h_All > 0)
                                    {
                                        GI.cpu_num++;
                                        set1_UpDown[2].Play("push");
                                    }
                                    else if (GI.input_h_All < 0)
                                    {
                                        if ((GI.player_num + GI.cpu_num) >= 3) GI.cpu_num--;
                                        set1_UpDown[3].Play("push");
                                    }
                                    break;
                                case 2:
                                    if (GI.input_h_All >0)
                                    {
                                        GI.game_mode_chioce++;
                                        set1_UpDown[4].Play("push");
                                    }
                                    else if (GI.input_h_All < 0)
                                    {
                                        GI.game_mode_chioce--;
                                        set1_UpDown[5].Play("push");
                                    }
                                    break;
                            }
                            audiosource.PlayOneShot(effect[0]);
                        }
                        
                    }
                    if (GI.chioce_now == 3)
                    {
                        if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                        {
                            set1_ani[1].SetBool("OK", true);
                            audiosource.PlayOneShot(effect[1]);
                        }
                    }
                    if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                    {
                        BackToScene1.SetActive(true);
                        GI.SceneChoiceOK = true;
                        audiosource.PlayOneShot(effect[2]);
                    }
                    break;
                case 3:
                    set1[1].SetActive(false);
                    set1_ani[0].SetBool("OK", true);
                    set3_C_job[0] = GI.cpu_num;
                    for (int i = 1; i < 5; i++) set3_C_job[i] = 0;
                    break;
                //2末~4退場動画
                case 4:
                    if (GI.Gamemode_choice != 1)
                    {
                        for (int i = 0; i < GI.cpu_num; i++) set3_C_team_N.Add(0);
                    }
                    else
                    {
                        for (int i = 0; i < (GI.cpu_num / 2); i++) set3_C_team_A.Add(0);
                        for (int i = (GI.cpu_num / 2); i < GI.cpu_num; i++) set3_C_team_B.Add(0);
                    }
                    //POINT 上限SET
                    if(GI.Gamemode_choice==1)
                    {
                        if (GI.game_mode_chioce == 0) GI.game_point_max = 20;
                        else if (GI.game_mode_chioce == 1) GI.game_point_max = 40;
                        else if (GI.game_mode_chioce == 2) GI.game_point_max = 99;
                    }
                    else if (GI.Gamemode_choice == 2)
                    {
                        if (GI.game_mode_chioce == 0) GI.game_point_max = 20;
                        else if (GI.game_mode_chioce == 1) GI.game_point_max = 40;
                        else if (GI.game_mode_chioce == 2) GI.game_point_max = 99;
                    }
                    set1[0].SetActive(false);
                    GI.setingstate++;
                    break;
                //SET2開始=================================================================================
                //キャラクターの選択
                case 5:
                    if (GI.player_num > 0)
                    {
                        for (int i = 0; i < GI.player_num; i++)
                        {
                            player_stage[i].SetActive(true);
                            player_stage[i].GetComponent<Animator>().SetBool("BACK", false);
                            playerchioce_mark[i].SetActive(true);
                        }
                        GI.setingstate++;
                    }
                    else
                    {
                        //Jump to GAMESET
                        GI.setingstate = 8;
                    }
                    break;
                case 6:
                    bool inputcheck = false;
                    bool inputcheck2 = false;
                    for (int i = 0; i < GI.player_num; i++)
                    {
                        if (playerchioce[i] <= 0) playerchioce[i] = 0;
                        else if (playerchioce[i] >= 5) playerchioce[i] = 5;
                        if (i == 0) playerchioce_mark[i].transform.position = playerchioce_P1mark_POS[playerchioce[i]].transform.position;
                        else if (i == 1) playerchioce_mark[i].transform.position = playerchioce_P2mark_POS[playerchioce[i]].transform.position;
                        else if (i == 2) playerchioce_mark[i].transform.position = playerchioce_P3mark_POS[playerchioce[i]].transform.position;
                        else if (i == 3) playerchioce_mark[i].transform.position = playerchioce_P4mark_POS[playerchioce[i]].transform.position;

                    }
                    //P1
                    if (GI.player_num > 0)
                    {
                        PC[0].JoyInput(0);
                        if (((GI.input_h_joy_check[0]&& JoyCheck[0] == false) || Input.GetButtonDown("Horizontal player1")) && playerchioce_OK[0] != true)
                        {
                            if (GI.input_h_joy_check[0] && JoyCheck[0] == false)
                            {
                                if (GI.input_h_Joy[0] < (-0.6f))
                                {
                                    playerchioce[0]--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_Joy[0] > 0.6f)
                                {
                                    playerchioce[0]++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                GI.input_h_joy_check[0] = false;
                                GI.input_h_Joy[0] = 0;
                                GI.input_h_All = 0;
                                JoyCheck[0] = true;
                                PC[0].JoyCheck(0);
                            }
                            else
                            {
                                if (GI.input_h_Joy[0] < 0) playerchioce[0]--;
                                else if (GI.input_h_Joy[0] > 0) playerchioce[0]++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            
                        }
                        if ((Input.GetButtonDown("Attack1 player1") || Input.GetKeyDown(KeyCode.Return)) && playerchioce_OK[0] == false)
                        {
                            inputcheck2 = true;
                            playerchioce_OK[0] = true;
                            if (playerchioce[0] == 5) playerchioce[0] = Random.Range(0, 5);
                            PC[0].job_prefab[playerchioce[0]].SetActive(true);
                            Instantiate(chioce_effect, PC[0].job_prefab[playerchioce[0]].transform.position, Quaternion.identity);
                            playerchioce_mark[0].SetActive(false);
                            audiosource.PlayOneShot(effect[3]);
                        }
                        if ((Input.GetButtonDown("Attack2 player1") || Input.GetKeyDown(KeyCode.Escape)) && playerchioce_OK[0] == true)
                        {
                            playerchioce_OK[0] = false;
                            playerchioce_mark[0].SetActive(true);
                            Instantiate(chiocecancel_effect, PC[0].job_prefab[playerchioce[0]].transform.position, Quaternion.identity);
                            PC[0].job_prefab[playerchioce[0]].SetActive(false);
                            inputcheck = true;
                            audiosource.PlayOneShot(effect[4]);
                        }
                    }
                    //P2w
                    if (GI.player_num > 1)
                    {
                        PC[0].JoyInput(1);
                        if (((GI.input_h_joy_check[1]&& JoyCheck[1] == false) || Input.GetButtonDown("Horizontal player2")) && playerchioce_OK[1] != true)
                        {
                            if (GI.input_h_joy_check[1] && JoyCheck[1] == false)
                            {
                                if (GI.input_h_Joy[1] < (-0.6f))
                                {
                                    playerchioce[1]--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_Joy[1] > 0.6f)
                                {
                                    playerchioce[1]++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                GI.input_h_joy_check[1] = false;
                                GI.input_h_Joy[1] = 0;
                                GI.input_h_All = 0;
                                JoyCheck[1] = true;
                                PC[0].JoyCheck(1);
                            }
                            else
                            {
                                if (GI.input_h_Joy[1] < 0 ) playerchioce[1]--;
                                else if (GI.input_h_Joy[1] > 0 ) playerchioce[1]++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                        }
                        if (Input.GetButtonDown("Attack1 player2") && playerchioce_OK[1] == false)
                        {
                            inputcheck2 = true;
                            playerchioce_OK[1] = true;
                            if (playerchioce[1] == 5) playerchioce[1] = Random.Range(0, 5);
                            PC[1].job_prefab[playerchioce[1]].SetActive(true);
                            Instantiate(chioce_effect, PC[1].job_prefab[playerchioce[1]].transform.position, Quaternion.identity);
                            playerchioce_mark[1].SetActive(false);
                            audiosource.PlayOneShot(effect[3]);
                        }
                        if (Input.GetButtonDown("Attack2 player2") && playerchioce_OK[1] == true)
                        {
                            playerchioce_OK[1] = false;
                            playerchioce_mark[1].SetActive(true);
                            Instantiate(chiocecancel_effect, PC[1].job_prefab[playerchioce[1]].transform.position, Quaternion.identity);
                            PC[1].job_prefab[playerchioce[1]].SetActive(false);
                            inputcheck = true;
                            audiosource.PlayOneShot(effect[4]);
                        }
                    }
                    //P3
                    if (GI.player_num > 2)
                    {
                        PC[0].JoyInput(2);
                        if (( (GI.input_h_joy_check[2]&& JoyCheck[2] == false ) || Input.GetButtonDown("Horizontal player3")) && playerchioce_OK[2] != true)
                        {
                            if (GI.input_h_joy_check[2] && JoyCheck[2] == false)
                            {
                                if (GI.input_h_Joy[2] < (-0.6f))
                                {
                                    playerchioce[2]--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_Joy[2] > 0.6f)
                                {
                                    playerchioce[2]++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                GI.input_h_joy_check[2] = false;
                                GI.input_h_Joy[2] = 0;
                                GI.input_h_All = 0;
                                JoyCheck[2] = true;
                                PC[0].JoyCheck(2);
                            }
                            else
                            {
                                if (GI.input_h_Joy[2] < 0) playerchioce[2]--;
                                else if (GI.input_h_Joy[2] > 0) playerchioce[2]++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                        }
                        if (Input.GetButtonDown("Attack1 player3") && playerchioce_OK[2] == false)
                        {
                            inputcheck2 = true;
                            playerchioce_OK[2] = true;
                            if (playerchioce[2] == 5) playerchioce[2] = Random.Range(0, 5);
                            PC[2].job_prefab[playerchioce[2]].SetActive(true);
                            Instantiate(chioce_effect, PC[2].job_prefab[playerchioce[2]].transform.position, Quaternion.identity);
                            playerchioce_mark[2].SetActive(false);
                            audiosource.PlayOneShot(effect[3]);
                        }
                        if (Input.GetButtonDown("Attack2 player3") && playerchioce_OK[2] == true)
                        {
                            playerchioce_OK[2] = false;
                            playerchioce_mark[2].SetActive(true);
                            Instantiate(chiocecancel_effect, PC[2].job_prefab[playerchioce[2]].transform.position, Quaternion.identity);
                            PC[2].job_prefab[playerchioce[2]].SetActive(false);
                            inputcheck = true;
                            audiosource.PlayOneShot(effect[4]);
                        }
                    }
                    //P4
                    if (GI.player_num > 3)
                    {
                        PC[0].JoyInput(3);
                        if (((GI.input_h_joy_check[3] && JoyCheck[3] == false) || Input.GetButtonDown("Horizontal player4")) && playerchioce_OK[3] != true)
                        {
                            if (GI.input_h_joy_check[3] && JoyCheck[3] == false)
                            {
                                if (GI.input_h_Joy[3] < (-0.6f))
                                {
                                    playerchioce[3]--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_Joy[3] > 0.6f)
                                {
                                    playerchioce[3]++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                GI.input_h_joy_check[3] = false;
                                GI.input_h_Joy[3] = 0;
                                GI.input_h_All = 0;
                                JoyCheck[3] = true;
                                PC[0].JoyCheck(3);
                            }
                            else
                            {
                                if (GI.input_h_Joy[3] < 0) playerchioce[3]--;
                                else if (GI.input_h_Joy[3] > 0) playerchioce[3]++;
                                audiosource.PlayOneShot(effect[0]);
                            }
                        }
                        if (Input.GetButtonDown("Attack1 player4") && playerchioce_OK[3] == false)
                        {
                            inputcheck2 = true;
                            playerchioce_OK[3] = true;
                            if (playerchioce[3] == 5) playerchioce[3] = Random.Range(0, 5);
                            PC[3].job_prefab[playerchioce[3]].SetActive(true);
                            playerchioce_mark[3].SetActive(false);
                            Instantiate(chioce_effect, PC[3].job_prefab[playerchioce[3]].transform.position, Quaternion.identity);
                            audiosource.PlayOneShot(effect[3]);
                        }
                        if (Input.GetButtonDown("Attack2 player4") && playerchioce_OK[3] == true)
                        {
                            playerchioce_OK[3] = false;
                            playerchioce_mark[3].SetActive(true);
                            Instantiate(chiocecancel_effect, PC[3].job_prefab[playerchioce[3]].transform.position, Quaternion.identity);
                            PC[3].job_prefab[playerchioce[3]].SetActive(false);
                            inputcheck = true;
                            audiosource.PlayOneShot(effect[4]);
                        }
                    }
                    int check = GI.player_num;
                    for (int i = 0; i < GI.player_num; i++) if (playerchioce_OK[i] == false) check--;
                    if (check == 0 && inputcheck == false)
                    {
                        if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                        {
                            for (int i = 0; i < GI.player_num; i++)
                            {
                                playerchioce_mark[i].SetActive(true);
                                playerchioce[i] = 0;
                                player_stage[i].GetComponent<Animator>().SetBool("BACK", true);
                            }
                            GI.setingstate = 7;
                            audiosource.PlayOneShot(effect[4]);
                        }
                    }
                    else if (check == GI.player_num && inputcheck2 == false)
                    {
                        if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                        {
                            for (int i = 0; i < GI.player_num; i++) GI.player_Profession.Add(playerchioce[i]);
                            GI.setingstate = 8;
                            audiosource.PlayOneShot(effect[1]);
                        }
                    }
                    break;
                case 7:
                    //BACK
                    GI.setingstate = 0;
                    break;
                case 8:
                    //ALL CHIOCE OK
                    if (GI.player_num > 0)
                    {
                        for (int i = 0; i < GI.player_num; i++)
                        {
                            PC[i].self.SetBool("OK", true);
                            PC[i].P[playerchioce[i]].SetBool("OK", true);
                        }
                    }
                    ballandSet3.SetBool("OK", true);
                    TopC.SetBool("OK", true);
                    break;
                case 9:
                    if (GI.player_num > 0)
                    {
                        TeamAnum = 0;
                        TeamBnum = 0;
                        for (int i = 0; i < GI.player_num; i++)
                        {
                            PC[i].self.SetBool("OK", false);
                            PC[i].P[playerchioce[i]].SetBool("OK", false);
                            PC[i].job_prefab[playerchioce[i]].SetActive(false);
                            PC[i].Active_false();
                            if(GI.Gamemode_choice==1)
                            {
                                if (i % 2 == 0)
                                {
                                    set3_P[i] = 1;
                                    TeamAnum++;
                                }
                                else
                                {
                                    set3_P[i] = 2;
                                    TeamBnum++;
                                }
                            }
                        }
                        
                    }

                    TeamAnum += set3_C_team_A.Count;
                    TeamBnum += set3_C_team_B.Count;
                    GI.character_team.Clear();
                    set3.gameObject.SetActive(true);
                    set3_chioce_now = 0;
                    break;
                //SET3開始=================================================================================
                case 10:
                    //CPUの数量制限INPUT
                    if ((GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f) || Input.GetButtonDown("Horizontal player1")|| Input.GetButtonDown("Horizontal player2")|| Input.GetButtonDown("Horizontal player3")|| Input.GetButtonDown("Horizontal player4"))
                    {
                        if(GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f)
                        {
                            //左右
                            if (set3_chioceing)
                            {
                                if (GI.input_h_All_Joy < (-0.6f))//左
                                {
                                    if (set3_chioce_now <= 3) set3_common[set3_chioce_now]--;
                                    else if (set3_chioce_now == 6) set3_chioce_now_M++;
                                    else if (set3_chioce_now == 4)
                                    {
                                        if (GI.Gamemode_choice != 1) set3_P[set3_chioce_now_P]--;
                                        else
                                        {
                                            if ((set3_P[set3_chioce_now_P] == 2 && (TeamBnum - 1) > 0) || (set3_P[set3_chioce_now_P] == 1 && (TeamAnum - 1) > 0)) set3_P[set3_chioce_now_P]--;
                                        }
                                    }
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        set3_chioce_now_C_area--;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        set3_chioce_now_C_teammark--;
                                                        if (set3_chioce_now_C_teammark <= 0) set3_chioce_now_C_teammark = 0;
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    set3_chioce_now_C_area--;
                                                    if (GI.Gamemode_choice == 1) if (set3_chioce_now_C_area == 3) set3_chioce_now_C_area = 1;
                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業数量変更
                                                set3_C_job[(set3_chioce_now_C - 2)]--;
                                                set3_cpu.c_word_chioce[0].Play("push");
                                                if (set3_C_job[(set3_chioce_now_C - 2)] <= 0) set3_C_job[(set3_chioce_now_C - 2)] = 0;
                                            }
                                        }
                                        else
                                        {
                                            //左右無意味
                                        }
                                    }
                                    if (set3_chioce_now <= 4) set3.changeg_word_chioce[0].Play("push");
                                    if(set3_chioce_now==1|| set3_chioce_now == 3)
                                    {
                                        audiosource.PlayOneShot(effect[6]);
                                    }
                                    else
                                    {
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    
                                }
                                else if (GI.input_h_All_Joy > 0.6f)//右
                                {
                                    if (set3_chioce_now <= 3) set3_common[set3_chioce_now]++;
                                    else if (set3_chioce_now == 4)
                                    {
                                        if (GI.Gamemode_choice != 1) set3_P[set3_chioce_now_P]++;
                                        else
                                        {
                                            if ((set3_P[set3_chioce_now_P] == 2 && (TeamBnum - 1) > 0) || (set3_P[set3_chioce_now_P] == 1 && (TeamAnum - 1) > 0)) set3_P[set3_chioce_now_P]++;
                                        }
                                    }
                                    else if (set3_chioce_now == 6) set3_chioce_now_M++;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        set3_chioce_now_C_area++;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        set3_chioce_now_C_teammark++;
                                                        switch (set3_chioce_now_C_area)
                                                        {
                                                            case 2:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_N.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_N.Count - 1);
                                                                break;
                                                            case 3:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_A.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_A.Count - 1);
                                                                break;
                                                            case 4:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_B.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_B.Count - 1);
                                                                break;
                                                            case 5:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_C.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_C.Count - 1);
                                                                break;
                                                            case 6:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_D.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_D.Count - 1);
                                                                break;
                                                        }
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (GI.Gamemode_choice != 1)
                                                    {
                                                        if (set3_chioce_now_C_area == 0) set3_chioce_now_C_area = 2;
                                                        else set3_chioce_now_C_area++;
                                                    }
                                                    else
                                                    {

                                                        if (set3_chioce_now_C_area > 1) set3_chioce_now_C_area++;
                                                    }

                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_C_job[set3_chioce_now_C - 2]++;
                                                set3_cpu.c_word_chioce[1].Play("push");
                                                int sum = 0;
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    if ((set3_chioce_now_C - 2) != i) sum += set3_C_job[i];
                                                }
                                                if (set3_C_job[(set3_chioce_now_C - 2)] >= (GI.cpu_num - sum)) set3_C_job[(set3_chioce_now_C - 2)] = (GI.cpu_num - sum);
                                            }
                                        }
                                        else
                                        {
                                            //左右無意味
                                        }
                                    }
                                    if (set3_chioce_now <= 4) set3.changeg_word_chioce[1].Play("push");
                                    if (set3_chioce_now == 1 || set3_chioce_now == 3)
                                    {
                                        audiosource.PlayOneShot(effect[7]);
                                    }
                                    else
                                    {
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                }
                            }
                            else
                            {
                                if (GI.input_h_All_Joy < (-0.6f))
                                {
                                    if (set3_chioce_now == 2) set3_chioce_now = 0;
                                    else if (set3_chioce_now == 3) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 5 & GI.player_num != 0) set3_chioce_now = 4;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_All_Joy > 0.6f)
                                {
                                    if (set3_chioce_now == 0) set3_chioce_now = 2;
                                    else if (set3_chioce_now == 1) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 4 && GI.cpu_num != 0) set3_chioce_now = 5;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            GI.InputJoycheck_all[1] = false;
                            GI.input_h_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }
                        else
                        {
                            //左右
                            if (set3_chioceing)
                            {
                                if (GI.input_h_All < 0)//左
                                {
                                    if (set3_chioce_now <= 3) set3_common[set3_chioce_now]--;
                                    else if (set3_chioce_now == 6) set3_chioce_now_M++;
                                    else if (set3_chioce_now == 4)
                                    {
                                        if (GI.Gamemode_choice != 1) set3_P[set3_chioce_now_P]--;
                                        else
                                        {
                                            if ((set3_P[set3_chioce_now_P] == 2 && (TeamBnum - 1) > 0) || (set3_P[set3_chioce_now_P] == 1 && (TeamAnum - 1) > 0)) set3_P[set3_chioce_now_P]--;
                                        }
                                    }
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        set3_chioce_now_C_area--;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        set3_chioce_now_C_teammark--;
                                                        if (set3_chioce_now_C_teammark <= 0) set3_chioce_now_C_teammark = 0;
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    set3_chioce_now_C_area--;
                                                    if (GI.Gamemode_choice == 1) if (set3_chioce_now_C_area == 3) set3_chioce_now_C_area = 1;
                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業数量変更
                                                set3_C_job[(set3_chioce_now_C - 2)]--;
                                                set3_cpu.c_word_chioce[0].Play("push");
                                                if (set3_C_job[(set3_chioce_now_C - 2)] <= 0) set3_C_job[(set3_chioce_now_C - 2)] = 0;
                                            }
                                        }
                                        else
                                        {
                                            //左右無意味
                                        }
                                    }
                                    if (set3_chioce_now <= 4)
                                    {
                                        if (set3_chioce_now != 0)
                                        {
                                            set3.changeg_word_chioce[0].Play("push");
                                        }
                                        else
                                        {
                                            if (GI.Gamemode_choice != 0)
                                            {
                                                set3.changeg_word_chioce[0].Play("push");
                                            }
                                            else
                                            {
                                                if (GI.game_mode_chioce != 1) set3.changeg_word_chioce[0].Play("push");
                                            }
                                        }
                                    }
                                }
                                else if (GI.input_h_All > 0)//右
                                {
                                    if (set3_chioce_now <= 3) set3_common[set3_chioce_now]++;
                                    else if (set3_chioce_now == 4)
                                    {
                                        if (GI.Gamemode_choice != 1) set3_P[set3_chioce_now_P]++;
                                        else
                                        {
                                            if ((set3_P[set3_chioce_now_P] == 2 && (TeamBnum - 1) > 0) || (set3_P[set3_chioce_now_P] == 1 && (TeamAnum - 1) > 0)) set3_P[set3_chioce_now_P]++;
                                        }
                                    }
                                    else if (set3_chioce_now == 6) set3_chioce_now_M++;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        set3_chioce_now_C_area++;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        set3_chioce_now_C_teammark++;
                                                        switch (set3_chioce_now_C_area)
                                                        {
                                                            case 2:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_N.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_N.Count - 1);
                                                                break;
                                                            case 3:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_A.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_A.Count - 1);
                                                                break;
                                                            case 4:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_B.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_B.Count - 1);
                                                                break;
                                                            case 5:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_C.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_C.Count - 1);
                                                                break;
                                                            case 6:
                                                                if (set3_chioce_now_C_teammark >= (set3_C_team_D.Count - 1)) set3_chioce_now_C_teammark = (set3_C_team_D.Count - 1);
                                                                break;
                                                        }
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (GI.Gamemode_choice != 1)
                                                    {
                                                        if (set3_chioce_now_C_area == 0) set3_chioce_now_C_area = 2;
                                                        else set3_chioce_now_C_area++;
                                                    }
                                                    else
                                                    {

                                                        if (set3_chioce_now_C_area > 1) set3_chioce_now_C_area++;
                                                    }

                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_C_job[set3_chioce_now_C - 2]++;
                                                set3_cpu.c_word_chioce[1].Play("push");
                                                int sum = 0;
                                                for (int i = 0; i < 5; i++)
                                                {
                                                    if ((set3_chioce_now_C - 2) != i) sum += set3_C_job[i];
                                                }
                                                if (set3_C_job[(set3_chioce_now_C - 2)] >= (GI.cpu_num - sum)) set3_C_job[(set3_chioce_now_C - 2)] = (GI.cpu_num - sum);
                                            }
                                        }
                                        else
                                        {
                                            //左右無意味
                                        }
                                    }
                                    if (set3_chioce_now <= 4)
                                    {
                                        if (set3_chioce_now != 0)
                                        {
                                            set3.changeg_word_chioce[0].Play("push");
                                        }
                                        else
                                        {
                                            if (GI.Gamemode_choice != 0 )
                                            {
                                                set3.changeg_word_chioce[0].Play("push");
                                            }else
                                            {
                                                if(GI.game_mode_chioce != 1) set3.changeg_word_chioce[0].Play("push");
                                            }
                                        }
                                    }
                                }
                                if (set3_chioce_now == 1 || set3_chioce_now == 3)
                                {
                                    if(set3_common[set3_chioce_now]>=1) audiosource.PlayOneShot(effect[7]);
                                    else if (set3_common[set3_chioce_now] <= 0) audiosource.PlayOneShot(effect[6]);
                                }
                                else
                                {
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            else
                            {
                                if (GI.input_h_All < 0)
                                {
                                    if (set3_chioce_now == 2) set3_chioce_now = 0;
                                    else if (set3_chioce_now == 3) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 5 & GI.player_num != 0) set3_chioce_now = 4;
                                }
                                else if (GI.input_h_All > 0)
                                {
                                    if (set3_chioce_now == 0) set3_chioce_now = 2;
                                    else if (set3_chioce_now == 1) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 4 && GI.cpu_num != 0) set3_chioce_now = 5;
                                }
                                audiosource.PlayOneShot(effect[0]);
                            }
                        }
                    }
                    else if ((GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f) || Input.GetButtonDown("Vertical player1") || Input.GetButtonDown("Vertical player2") || Input.GetButtonDown("Vertical player3") || Input.GetButtonDown("Vertical player4"))
                    {
                        if(GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f)
                        {
                            //上下
                            if (set3_chioceing)
                            {
                                if (GI.input_v_All_Joy > 0.6f)//上
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now_P--;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 2;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        if ((set3_chioce_now_C_teammark - 3) >= 0 && set3_chioce_now_C_area != 2) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark - 3);
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (GI.Gamemode_choice != 1)
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 2;
                                                        else set3_chioce_now_C_area--;
                                                    }
                                                    else
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 1;
                                                        else set3_chioce_now_C_area--;
                                                    }


                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_chioce_now_C--;
                                                set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 2, 6);
                                            }
                                        }
                                        else
                                        {
                                            set3_chioce_now_C--;
                                            set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 0, 1);
                                        }
                                    }
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_v_All_Joy < (-0.6f))//下
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now_P++;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        if (set3_chioce_now_C_area == 2) set3_chioce_now_C_area = 3;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        switch (set3_chioce_now_C_area)
                                                        {
                                                            case 3://A
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_A.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 4://B
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_B.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 5://C
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_C.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 6://D
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_D.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                        }
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (set3_chioce_now_C_area == 1) set3_chioce_now_C_area = 3;
                                                    else set3_chioce_now_C_area++;
                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_chioce_now_C++;
                                                set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 2, 6);
                                            }
                                        }
                                        else
                                        {
                                            set3_chioce_now_C++;
                                            set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 0, 1);
                                        }
                                    }
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            else
                            {
                                if (GI.input_v_All_Joy < (-0.6f))
                                {
                                    if (set3_chioce_now == 0) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 1)
                                    {
                                        if (GI.player_num != 0) set3_chioce_now = 4;
                                        else if (GI.cpu_num != 0) set3_chioce_now = 5;
                                    }
                                    else if (set3_chioce_now == 2) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 3)
                                    {
                                        if (GI.cpu_num != 0) set3_chioce_now = 5;
                                        else if (GI.player_num != 0) set3_chioce_now = 4;
                                    }
                                    else if (set3_chioce_now == 4 || set3_chioce_now == 5) set3_chioce_now = 6;
                                    else set3_chioce_now++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_v_All_Joy > 0.6f)
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 5) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 6)
                                    {
                                        if (GI.player_num != 0) set3_chioce_now = 4;
                                        else if (GI.player_num == 0) set3_chioce_now = 5;
                                    }
                                    else set3_chioce_now--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            GI.InputJoycheck_all[0] = false;
                            GI.input_v_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }else
                        {
                            //上下
                            if (set3_chioceing)
                            {
                                if (GI.input_v_All > 0)//上
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now_P--;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 2;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        if ((set3_chioce_now_C_teammark - 3) >= 0 && set3_chioce_now_C_area != 2) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark - 3);
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (GI.Gamemode_choice != 1)
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 2;
                                                        else set3_chioce_now_C_area--;
                                                    }
                                                    else
                                                    {
                                                        if (set3_chioce_now_C_area > 2) set3_chioce_now_C_area = 1;
                                                        else set3_chioce_now_C_area--;
                                                    }


                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_chioce_now_C--;
                                                set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 2, 6);
                                            }
                                        }
                                        else
                                        {
                                            set3_chioce_now_C--;
                                            set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 0, 1);
                                        }
                                    }
                                }
                                else if (GI.input_v_All < 0)//下
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now_P++;
                                    else if (set3_chioce_now == 5)
                                    {
                                        if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                        {
                                            if (set3_chioce_now_C == 0)//TEAM選択画面
                                            {
                                                if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                                {
                                                    if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                                    {
                                                        if (set3_chioce_now_C_area == 2) set3_chioce_now_C_area = 3;
                                                    }
                                                    else//Team Mark選択だけ
                                                    {
                                                        switch (set3_chioce_now_C_area)
                                                        {
                                                            case 3://A
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_A.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 4://B
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_B.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 5://C
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_C.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                            case 6://D
                                                                if ((set3_chioce_now_C_teammark + 3) <= (set3_C_team_D.Count - 1)) set3_chioce_now_C_teammark = (set3_chioce_now_C_teammark) + 3;
                                                                break;
                                                        }
                                                    }
                                                }
                                                else//チームエリアをまだ選択していない
                                                {
                                                    if (set3_chioce_now_C_area == 1) set3_chioce_now_C_area = 3;
                                                    else set3_chioce_now_C_area++;
                                                }
                                            }
                                            else if (set3_chioce_now_C >= 1)//JOB選択画面
                                            {
                                                //CPUの職業変更
                                                set3_chioce_now_C++;
                                                set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 2, 6);
                                            }
                                        }
                                        else
                                        {
                                            set3_chioce_now_C++;
                                            set3_chioce_now_C = GI.MAXandMin(set3_chioce_now_C, 0, 1);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (GI.input_v_All < 0)
                                {
                                    if (set3_chioce_now == 0) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 1)
                                    {
                                        if (GI.player_num != 0) set3_chioce_now = 4;
                                        else if (GI.cpu_num != 0) set3_chioce_now = 5;
                                    }
                                    else if (set3_chioce_now == 2) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 3)
                                    {
                                        if (GI.cpu_num != 0) set3_chioce_now = 5;
                                        else if (GI.player_num != 0) set3_chioce_now = 4;
                                    }
                                    else if (set3_chioce_now == 4 || set3_chioce_now == 5) set3_chioce_now = 6;
                                    else set3_chioce_now++;
                                }
                                else if (GI.input_v_All > 0)
                                {
                                    if (set3_chioce_now == 4) set3_chioce_now = 1;
                                    else if (set3_chioce_now == 5) set3_chioce_now = 3;
                                    else if (set3_chioce_now == 6)
                                    {
                                        if (GI.player_num != 0) set3_chioce_now = 4;
                                        else if (GI.player_num == 0) set3_chioce_now = 5;
                                    }
                                    else set3_chioce_now--;
                                }
                            }
                            audiosource.PlayOneShot(effect[0]);
                        }
                        
                    }
                    else if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                    {
                        if (set3_chioceing)
                        {
                            if (set3_chioce_now == 6) set3_map = set3_chioce_now_M;
                            else if (set3_chioce_now == 5)
                            {
                                if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                {
                                    if (set3_chioce_now_C == 0)//TEAM選択画面
                                    {
                                        if (set3_chioceing_CPUteamarea)//各チームエリア入る
                                        {
                                            if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                            {
                                                if (set3_chioce_now_C_area == set3_chioce_now_C_teammark_fromteam||(GI.Gamemode_choice==1&&set3_chioce_now_C_area==3&& (TeamBnum-1)==0) || (GI.Gamemode_choice == 1 && set3_chioce_now_C_area == 4 && (TeamAnum - 1) == 0))
                                                {
                                                    //移動中のMARKのIMAGE位置回復
                                                    switch (set3_chioce_now_C_teammark_fromteam)
                                                    {
                                                        case 2:
                                                            set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.N_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                            break;
                                                        case 3:
                                                            set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.A_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                            break;
                                                        case 4:
                                                            set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.B_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                            break;
                                                        case 5:
                                                            set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.C_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                            break;
                                                        case 6:
                                                            set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.D_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    //移動中のMARKの職業は新しいエリアのLIST中に配置
                                                    switch (set3_chioce_now_C_area)
                                                    {
                                                        case 2:
                                                            set3_C_team_N.Add(set3_chioce_now_C_teammark_job);
                                                            break;
                                                        case 3:
                                                            set3_C_team_A.Add(set3_chioce_now_C_teammark_job);
                                                            break;
                                                        case 4:
                                                            set3_C_team_B.Add(set3_chioce_now_C_teammark_job);
                                                            break;
                                                        case 5:
                                                            set3_C_team_C.Add(set3_chioce_now_C_teammark_job);
                                                            break;
                                                        case 6:
                                                            set3_C_team_D.Add(set3_chioce_now_C_teammark_job);
                                                            break;
                                                    }
                                                    //移動中のMARKの職業の元LIST中に消し
                                                    switch (set3_chioce_now_C_teammark_fromteam)
                                                    {
                                                        case 2:
                                                            set3_C_team_N.RemoveAt(set3_chioce_now_C_teammark_num_inteam);
                                                            break;
                                                        case 3:
                                                            set3_C_team_A.RemoveAt(set3_chioce_now_C_teammark_num_inteam);
                                                            break;
                                                        case 4:
                                                            set3_C_team_B.RemoveAt(set3_chioce_now_C_teammark_num_inteam);
                                                            break;
                                                        case 5:
                                                            set3_C_team_C.RemoveAt(set3_chioce_now_C_teammark_num_inteam);
                                                            break;
                                                        case 6:
                                                            set3_C_team_D.RemoveAt(set3_chioce_now_C_teammark_num_inteam);
                                                            break;
                                                    }
                                                    //配列の整理
                                                    set3_cpu.Sorting_check = true;
                                                }
                                                set3_chioceing_CPUteam_move = false;
                                                set3_chioceing_CPUteamarea = false;
                                                set3_chioce_now_C_teammark = 0;
                                            }
                                            else//Team Mark選択だけ
                                            {
                                                set3_chioceing_CPUteam_move = true;
                                                int num = 0;
                                                for (int i = 2; i < 6; i++)
                                                {
                                                    if (i == set3_chioce_now_C_area)
                                                    {
                                                        set3_chioce_now_C_teammark_num = set3_chioce_now_C_teammark + num;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        switch (i)
                                                        {
                                                            case 2:
                                                                num += set3_C_team_N.Count;
                                                                break;
                                                            case 3:
                                                                num += set3_C_team_A.Count;
                                                                break;
                                                            case 4:
                                                                num += set3_C_team_B.Count;
                                                                break;
                                                            case 5:
                                                                num += set3_C_team_C.Count;
                                                                break;
                                                            case 6:
                                                                num += set3_C_team_D.Count;
                                                                break;
                                                        }
                                                    }
                                                }
                                                switch (set3_chioce_now_C_area)
                                                {
                                                    case 2:
                                                        set3_chioce_now_C_teammark_job = set3_C_team_N[set3_chioce_now_C_teammark];
                                                        break;
                                                    case 3:
                                                        set3_chioce_now_C_teammark_job = set3_C_team_A[set3_chioce_now_C_teammark];
                                                        break;
                                                    case 4:
                                                        set3_chioce_now_C_teammark_job = set3_C_team_B[set3_chioce_now_C_teammark];
                                                        break;
                                                    case 5:
                                                        set3_chioce_now_C_teammark_job = set3_C_team_C[set3_chioce_now_C_teammark];
                                                        break;
                                                    case 6:
                                                        set3_chioce_now_C_teammark_job = set3_C_team_D[set3_chioce_now_C_teammark];
                                                        break;
                                                }

                                                set3_chioce_now_C_teammark_fromteam = set3_chioce_now_C_area;
                                                set3_chioce_now_C_teammark_num_inteam = set3_chioce_now_C_teammark;
                                            }
                                        }
                                        else//チームエリアをまだ選択していない
                                        {
                                            if (set3_chioce_now_C_area == 0)
                                            {
                                                if (GI.Gamemode_choice != 1) set3_cpu.NoTeam_mark();
                                            }
                                            else if (set3_chioce_now_C_area == 1) set3_cpu.Average_mark();
                                            else set3_chioceing_CPUteamarea = true;
                                        }
                                    }
                                    else if (set3_chioce_now_C >= 1)//JOB選択画面
                                    {
                                        set3_chioce_now_C = 1;
                                        set3_chioceing_CPU = false;
                                        set3_cpu.Sorting_job();
                                    }
                                }
                                else
                                {
                                    if (set3_chioce_now_C == 1) set3_chioce_now_C = 2;
                                    set3_chioceing_CPU = true;
                                }
                            }
                            else
                            {
                                set3_chioceing = false;
                            }
                            audiosource.PlayOneShot(effect[3]);
                        }
                        else
                        {
                            set3_chioceing = true;
                            if (set3_chioce_now == 5) set3.cpu_seting_area.SetActive(true);
                            audiosource.PlayOneShot(effect[3]);
                        }
                        if(set3_chioce_now == 7)
                        {
                            //ゲーム開始動画！
                            NextScenel.SetActive(true);
                            GI.setingstate = 13;
                            audiosource.PlayOneShot(effect[5]);
                        }
                    }
                    else if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (set3_chioceing)
                        {
                            if (set3_chioce_now == 5)
                            {
                                if (set3_chioceing_CPU)//TEAM選択画面orJOB選択画面エリア入る
                                {
                                    if (set3_chioce_now_C == 0)//TEAM選択画面
                                    {
                                        if (set3_chioceing_CPUteamarea)//チームエリア入る
                                        {
                                            if (set3_chioceing_CPUteam_move)//Team Mark移動中
                                            {
                                                //移動中のMARKのIMAGE位置回復
                                                switch (set3_chioce_now_C_teammark_fromteam)
                                                {
                                                    case 2:
                                                        set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.N_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                        break;
                                                    case 3:
                                                        set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.A_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                        break;
                                                    case 4:
                                                        set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.B_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                        break;
                                                    case 5:
                                                        set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.C_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                        break;
                                                    case 6:
                                                        set3_cpu.teammark[set3_chioce_now_C_teammark_num].transform.position = set3_cpu.D_POS[set3_chioce_now_C_teammark_num_inteam].transform.position;
                                                        break;
                                                }
                                                set3_chioceing_CPUteam_move = false;
                                                set3_chioceing_CPUteamarea = false;
                                                set3_chioce_now_C_teammark = 0;
                                            }
                                            else//NO　移動中
                                            {
                                                set3_chioceing_CPUteamarea = false;
                                            }
                                        }
                                        else//チームエリアをまだ選択していない
                                        {
                                            set3_chioceing_CPU = false;
                                        }
                                    }
                                    else if (set3_chioce_now_C >= 1)//JOB選択画面
                                    {
                                        //CPUの職業変更
                                        set3_chioceing_CPU = false;
                                        set3_cpu.Sorting_job();
                                    }
                                }
                                else
                                {
                                    if(GI.Gamemode_choice!=1) set3_chioce_now_C_area = 0;
                                    else set3_chioce_now_C_area = 1;
                                    set3_cpu.Sorting_job();
                                    set3_chioceing = false;
                                    set3.cpu_seting_area_ani.SetBool("OK", true);
                                }
                            }
                            else
                            {
                                set3_chioceing = false;
                            }
                        }
                        else
                        {
                            //toCASE11
                            set3.animator_SETOK.SetBool("OK",false);
                            set3.GetComponent<Animator>().SetBool("OK",true);
                            set3.cpu_seting_area_ani.gameObject.SetActive(false);
                            //↑GI.setingstate++
                        }
                        audiosource.PlayOneShot(effect[4]);
                    }
                    if(GI.Gamemode_choice == 1)
                    {
                        bool checkteam = false ;
                        if(set3_C_team_N.Count>0)
                        {
                            for(int i=0;i< set3_C_team_N.Count;i++)
                            {
                                set3_C_team_A.Add(set3_C_team_N[i]);
                                set3_C_team_N.RemoveAt(i);
                                i--;
                            }
                            checkteam = true;
                        }
                        if (set3_C_team_C.Count > 0)
                        {
                            for (int i = 0; i < set3_C_team_C.Count; i++)
                            {
                                set3_C_team_B.Add(set3_C_team_C[i]);
                                set3_C_team_C.RemoveAt(i);
                                i--;
                            }
                            checkteam = true;
                        }
                        if (set3_C_team_D.Count > 0)
                        {
                            for (int i = 0; i < set3_C_team_D.Count; i++)
                            {
                                set3_C_team_A.Add(set3_C_team_D[i]);
                                set3_C_team_D.RemoveAt(i);
                                i--;
                            }
                            checkteam = true;
                        }
                        if(checkteam == true)set3_cpu.Sorting_mark();
                    }
                    break;
                case 11:
                    ballandSet3.SetBool("OK", false);
                    set3.GetComponent<Animator>().SetBool("OK", false);
                    set3.gameObject.SetActive(false);
                    set3_chioceing = false;
                    for (int i = 0; i < 4; i++) playerchioce_OK[i] = false;
                    break;
                case 12:
                    //BACK TO STEP2
                    TopC.SetBool("OK", false);
                    if (GI.player_num > 0) GI.setingstate = 5;
                    else GI.setingstate = 0;
                    break;
                case 13:
                    //画面遷移動画
                    for (int i = 0; i < GI.player_num; i++) GI.character_team.Add(set3_P[i]);
                    if(set3_C_team_N.Count>0)
                    {
                        foreach (var num in set3_C_team_N)
                        {
                            GI.cpu_Profession.Add(num);
                            GI.character_team.Add(0);
                        }
                    }
                    if (set3_C_team_A.Count > 0)
                    {
                        foreach (var num in set3_C_team_A)
                        {
                            GI.cpu_Profession.Add(num);
                            GI.character_team.Add(1);
                        }
                    }
                    if (set3_C_team_B.Count > 0)
                    {
                        foreach (var num in set3_C_team_B)
                        {
                            GI.cpu_Profession.Add(num);
                            GI.character_team.Add(2);
                        }
                    }
                    if (set3_C_team_C.Count > 0)
                    {
                        foreach (var num in set3_C_team_C)
                        {
                            GI.cpu_Profession.Add(num);
                            GI.character_team.Add(3);
                        }
                    }
                    if (set3_C_team_D.Count > 0)
                    {
                        foreach (var num in set3_C_team_D)
                        {
                            GI.cpu_Profession.Add(num);
                            GI.character_team.Add(4);
                        }
                    }
                    GI.player_life_max = set3_common[0];
                    GI.game_MPcheck = set3_common[1];
                    GI.game_time = set3_common[2];
                    GI.game_Itemcheck = set3_common[3];
                    GI.Stage_num = set3_map;
                    
                    GI.setingstate++;
                    break;
                case 15:
                    //動画終了　SCENE遷移開始
                    GI.Scene_now = 4;
                    GI.LoadScene_OK();
                    break;
            }
        }
    }
}
