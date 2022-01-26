using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static GameInfo game_data;
    //[HideInInspector]
    //ALLScene使用した
    public int Scene_now;                                  //現在のScene
    public bool SceneChoiceOK;                              //次のSceneを移動決定
   
    
    //Main使用した変数
    public bool mainok;
    public float input_v_All;                                  //←→のINPUTを判定
    public float input_h_All;                                  //↑↓のINPUTを判定
    public float input_v_All_Joy;                                  //←→のINPUTを判定
    public float input_h_All_Joy;                                  //↑↓のINPUTを判定
    public float[] input_v_Joy;
    public float[] input_h_Joy;
    public bool[] input_v_joy_check;            //0~4 Joyの番号
    public bool[] input_h_joy_check;
    public bool[] InputJoycheck_all;            //0 上下 1左右
    public bool JoycheckStart;
    public int Gamemode_choice;                             //今選択したMODEを保存する変数
    public AudioClip effect;
    public AudioSource audiosource;

    //SETTING使用した変数
    public int setingstate;                                 //SETINGの階段　
    //0.開場animation 1.seting1 2.seting1退場animation 3.seting2:キャラクターの選択
    public int chioce_now;                                  //SETING用の変数
    //ゲーム変数
    public int game_mode_chioce;                            //今回ゲームモード
    public int game_time;
    public int game_MPcheck;
    public int game_Itemcheck;
    public int player_num;
    public List<int> player_Profession = new List<int>();
    public int cpu_num;
    public List<int> cpu_Profession = new List<int>();
    public List<int> character_team = new List<int>();
    public int Stage_num;                               //ゲームモード選択した後ステージの番号
    public int[] item_Probability ;        //12種類のアイテムの再生機率
    public int ItemMax;
    public int Item_bron_time;
    public int Item_born_OneTime;
    public int player_life_max;
    public bool text;
    public int game_point_max;

    public int SlowDownNow;
    //RESULT SCENE用
    public List<int> character_Kill_sum = new List<int>();
    public List<int> character_BeKill_sum = new List<int>();
    public List<int> character_Point_sum = new List<int>();
    public List<float> character_Lifenow = new List<float>();
    public int Item_kill;
    public int[] TeamKillAll;
    public int[] TeamPointAll;
    enum Scence
    {
        Open=0,
        Main,
        Gameseting,
        All_Seting,
        Normal_game,
        BB_game,
        Jump_game,
        GameResult,
    }

    enum M_S
    {
        NextIsNormalSetingScence = 0,
        NextIsBBSetingScence,
        NextIsJumpSetingScence,
        NextIsAllSetingScence,
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Gamemode_choice = 0;
        Scene_now = 1;
        SceneChoiceOK = false;
        setingstate=0;
        chioce_now=0;
        game_mode_chioce = 0;
        text = false;
        player_life_max = 1;
        mainok = false;
        game_time = 0;
        game_MPcheck = 0;
        game_Itemcheck = 0;
        game_point_max = 0;
        Item_kill = 0;
        TeamKillAll = new int[4];
        ItemMax=1;
        Item_bron_time=1;
        Item_born_OneTime = 1;
        InputJoycheck_all = new bool[2];
        InputJoycheck_all[0] = false;
        InputJoycheck_all[1] = false;
        input_v_Joy =new float[4];
        input_h_Joy=new float[4];
        input_v_joy_check = new bool[4];
        input_h_joy_check = new bool[4];
        player_num=2;
        cpu_num = 2;
        item_Probability = new int[12];
        for (int i = 0; i < item_Probability.Length; i++) item_Probability[i] = 50;
        for (int i = 0; i < 4; i++)
        {
            input_v_joy_check[i] = false;
            input_h_joy_check[i] = false;
            input_v_Joy[i] = 0;
            input_h_Joy[i] = 0;
        }
        SlowDownNow = 0;
        JoycheckStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        VH_Check();
        InputJoy_Check();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SlowDownNow == 1)
            {
                SlowDownNow = 0;
                Time.timeScale = 1f;
            }
            else
            {
                SlowDownNow = 1;
                Time.timeScale = 0.2f;
            }
            audiosource.PlayOneShot(effect);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SlowDownNow == 2)
            {
                SlowDownNow = 0;
                Time.timeScale = 1f;
            }
            else
            {
                SlowDownNow = 2;
                Time.timeScale = 0f;
            }
            audiosource.PlayOneShot(effect);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SlowDownNow = 0;
            Time.timeScale = 1f;
            audiosource.PlayOneShot(effect);
        }
    }
    private void Awake()
    {
        game_data = this;
    }
    
    public void LoadScene_OK()
    {
        SceneChoiceOK = false;
        SceneManager.LoadScene(Scene_now);
    }
    public void VH_Check()
    {
        //上下
        if (Input.GetAxis("Vertical player1") != 0)
        {
            input_v_All = Input.GetAxis("Vertical player1");
        }
        else if (Input.GetAxis("Vertical player2") != 0)
        {
            input_v_All = Input.GetAxis("Vertical player2");
        }
        else if (Input.GetAxis("Vertical player3") != 0)
        {
            input_v_All = Input.GetAxis("Vertical player3");
        }
        else if (Input.GetAxis("Vertical player4") != 0)
        {
            input_v_All = Input.GetAxis("Vertical player4");
        }
        //右左
        if (Input.GetAxis("Horizontal player1") != 0)
        {
            input_h_All = Input.GetAxis("Horizontal player1");
        }
        else if (Input.GetAxis("Horizontal player2") != 0)
        {
            input_h_All = Input.GetAxis("Horizontal player2");
        }
        else if (Input.GetAxis("Horizontal player3") != 0)
        {
            input_h_All = Input.GetAxis("Horizontal player3");
        }
        else if (Input.GetAxis("Horizontal player4") != 0)
        {
            input_h_All = Input.GetAxis("Horizontal player4");
        }
        //上下
        if (Input.GetAxis("Vertical Joy1") != 0)
        {
            input_v_All_Joy= Input.GetAxis("Vertical Joy1");
            input_v_Joy[0] = Input.GetAxis("Vertical Joy1");
        }
        if (Input.GetAxis("Vertical Joy2") != 0)
        {
            input_v_All_Joy = Input.GetAxis("Vertical Joy2");
            input_v_Joy[1] = Input.GetAxis("Vertical Joy2");
        }
        if (Input.GetAxis("Vertical Joy3") != 0)
        {
            input_v_All_Joy = Input.GetAxis("Vertical Joy3");
            input_v_Joy[2] = Input.GetAxis("Vertical Joy3");
        }
        if (Input.GetAxis("Vertical Joy4") != 0)
        {
            input_v_All_Joy = Input.GetAxis("Vertical Joy4");
            input_v_Joy[3] = Input.GetAxis("Vertical Joy4");
        }
        //右左
        if (Input.GetAxis("Horizontal Joy1") != 0)
        {
            input_h_All_Joy = Input.GetAxis("Horizontal Joy1");
            input_h_Joy[0] = Input.GetAxis("Horizontal Joy1");
        }
        if (Input.GetAxis("Horizontal Joy2") != 0)
        {
            input_h_All_Joy = Input.GetAxis("Horizontal Joy2");
            input_h_Joy[1] = Input.GetAxis("Horizontal Joy2");
        }
        if (Input.GetAxis("Horizontal Joy3") != 0)
        {
            input_h_All_Joy = Input.GetAxis("Horizontal Joy3");
            input_h_Joy[2] = Input.GetAxis("Horizontal Joy3");
        }
        if (Input.GetAxis("Horizontal Joy4") != 0)
        {
            input_h_All_Joy = Input.GetAxis("Horizontal Joy4");
            input_h_Joy[3] = Input.GetAxis("Horizontal Joy4");
        }
    }
    public void JoycheckStartOpen()
    {
        JoycheckStart = false;
    }
    public void InvokeJoyCheck()
    {
        CancelInvoke("JoycheckStartOpen");
        Invoke("JoycheckStartOpen", 0.3f);
    }
    public void InputJoy_Check()
    {
        for (int i = 0; i < 4; i++)
        {
            if (input_v_Joy[i] != 0)
            {
                input_v_joy_check[i] = true;
                InputJoycheck_all[0] = true;
            }
            else input_v_joy_check[i] = false;
            if (input_h_Joy[i] != 0)
            {
                input_h_joy_check[i] = true;
                InputJoycheck_all[1] = true;
            }
            else input_h_joy_check[i] = false;
        }
    }
    
    //SETING====================================================================
    public void Chioce_now_up()
    {
        chioce_now++;
    }
    public void Chioce_now_down()
    {
        chioce_now--;
    }
    public void Setingstate_up()
    {
        setingstate++;
    }
    public void Setingstate_down()
    {
        setingstate--;
    }
    public int MAXandMin(int num, int min_num, int max_num)
    {
        if (num <= min_num) num = min_num;
        else  if (num >= max_num) num = max_num;
        return num;
    }
}
