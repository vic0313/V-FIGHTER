using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public GameControler GC;
    public Camera cameraget;
    public Text timeUI;
    private int time_min;
    private int time_sec;
    //NORMAL MODE
    public CanvasGroup[] NormalUI;
    public GameObject[] PlayerUI;
    public Text[] PlayerUI_text;
    public Image[] Playernum;
    public GameObject[] PlayerUI_10;
    public Image[] Playernum_10;
    public Image[] Playernum_01;
    public Sprite[] Number_prefab;
    public Text Advantage_word;
    public Text Advantage_word_point;
    public Text Disadvantages_word;
    public Text Disadvantages_word_point;
    public GameObject[] PlayerDead;

    public GameObject[] Pasue_manu;
    public GameObject[] Gameset_UI;

    public Text[] BattleInfo_main;
    public Text[] BattleInfo;
    public Text[] BattleInfo2;

    private List<int> useteam = new List<int>();
    private int[] teamnum ;
    private bool setok;
    // Update is called once per frame
    private void Start()
    {
        for (int i = 0; i < (GC.GI.player_num); i++)
        {
            PlayerUI[i].SetActive(true);
            PlayerUI_10[i].SetActive(false);
            PlayerDead[i].SetActive(false);
            
        }
        for (int i = (GC.GI.player_num); i < 4; i++)
        {
            PlayerUI[i].SetActive(false);
            PlayerUI_10[i].SetActive(false);
            PlayerDead[i].SetActive(false);
        }
        if (GC.Gamemode_num == 0)
        {
            PlayerUI_text[0].text = "P1撃破数";
            PlayerUI_text[1].text = "P2撃破数";
            PlayerUI_text[2].text = "P3撃破数";
            PlayerUI_text[3].text = "P4撃破数";
        }
        else
        {
            PlayerUI_text[0].text = "P1得点";
            PlayerUI_text[1].text = "P2得点";
            PlayerUI_text[2].text = "P3得点";
            PlayerUI_text[3].text = "P4得点";
        }


        if (GC.Game_Time != 2)
        {
            time_min = (int)(GC.GameTimeNow / 60);
            time_sec = GC.GameTimeNow % 60;
            timeUI.text = string.Format("{0}:{1}", time_min.ToString("00"), time_sec.ToString("00"));
        }
        else timeUI.text = "∞";
        Advantage_word.text = "均衡";
        Disadvantages_word.text = "均衡";
        Advantage_word_point.text = "";
        Disadvantages_word_point.text = "";
        Pasue_manu[4].SetActive(false);
        Gameset_UI[0].SetActive(false);
        Gameset_UI[1].SetActive(false);
        teamnum = new int[4];
        setok = false;

    }
    void Update()
    {
        if(GC.Gamestate==1&& setok==false)
        {
            for (int i = 0; i < 4; i++) teamnum[i] = 0;
            for (int i = 0; i < GC.Character_Data.Length; i++)
            {
                BattleInfo[i].gameObject.SetActive(true);
                if (GC.Character_Data[i].characterTeam == 1) teamnum[0]++;
                else if (GC.Character_Data[i].characterTeam == 2) teamnum[1]++;
                else if (GC.Character_Data[i].characterTeam == 3) teamnum[2]++;
                else if (GC.Character_Data[i].characterTeam == 4) teamnum[3]++;
            }
            for (int i = GC.Character_Data.Length; i < 16; i++) BattleInfo[i].gameObject.SetActive(false);

            if (teamnum[0] != 0) useteam.Add(0);
            if (teamnum[1] != 0) useteam.Add(1);
            if (teamnum[2] != 0) useteam.Add(2);
            if (teamnum[3] != 0) useteam.Add(3);
            setok = true;
        }
        else if(GC.Gamestate>1)
        {
            //timeUI
            if (GC.Game_Time != 2)
            {
                time_min = (int)(GC.GameTimeNow / 60);
                time_sec = GC.GameTimeNow % 60;
                timeUI.text = string.Format("{0}:{1}", time_min.ToString("00"), time_sec.ToString("00"));
                if (time_min == 0 && time_sec < 30)
                {
                    timeUI.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    timeUI.gameObject.GetComponent<Animator>().SetBool("OK", true);
                }
            }
            else timeUI.text = "∞";
            //PlayerUI
            bool UIcheck_left = false;
            bool UIcheck_right = false;
            bool UIcheck_center = false;
            for (int i=0;i<GC.Character_Data.Length;i++)
            {
                Vector2 player = cameraget.WorldToScreenPoint(GC.Character_Data[i].transform.position);
                if (UIcheck_left==false)
                {
                    if (player.y > 590f && player.x < 710f)
                    {
                        UIcheck_left = true;
                    }
                }
                if (UIcheck_right == false)
                {
                    if (player.y > 590f && player.x > 890f)
                    {
                        UIcheck_right = true;
                    }
                }
                if (UIcheck_center == false)
                {
                    if (player.y > 500f && (player.x < 1000f && player.x > 600f))
                    {
                        UIcheck_center = true;
                    }
                }
                Debug.Log(player);
                if (UIcheck_left&& UIcheck_right&& UIcheck_center) break;
            }
            if (UIcheck_left)
            {
                NormalUI[0].alpha -= 2 * Time.deltaTime;
                if (NormalUI[0].alpha <= 0.5f) NormalUI[0].alpha = 0.5f;
            }
            else
            {
                NormalUI[0].alpha += 2 * Time.deltaTime;
                if (NormalUI[0].alpha >= 1) NormalUI[0].alpha = 1;
            }
            if (UIcheck_right)
            {
                NormalUI[1].alpha -= 2 * Time.deltaTime;
                if (NormalUI[1].alpha <= 0.5f) NormalUI[1].alpha = 0.5f;
            }
            else
            {
                NormalUI[1].alpha += 2 * Time.deltaTime;
                if (NormalUI[1].alpha >= 1) NormalUI[1].alpha = 1;
            }
            
            if (GC.GI.player_num > 0)
            {
                for (int i = 0; i < GC.GI.player_num; i++)
                {
                    if (GC.Gamemode_num == 0)
                    {
                        if (GC.Character_Data[i].killnum < 10) Playernum[i].sprite = Number_prefab[GC.Character_Data[i].killnum];
                        else
                        {
                            Playernum[i].gameObject.SetActive(false);
                            PlayerUI_10[i].SetActive(true);
                            Playernum_10[i].sprite = Number_prefab[(int)(GC.Character_Data[i].killnum / 10)];
                            Playernum_01[i].sprite = Number_prefab[GC.Character_Data[i].killnum % 10];
                        }
                        if (GC.Character_Data[i].character_NOW <= 0) PlayerDead[i].SetActive(true);
                    }
                    else
                    {
                        if (GC.Character_Data[i].Character_Point < 10) Playernum[i].sprite = Number_prefab[GC.Character_Data[i].Character_Point];
                        else
                        {
                            Playernum[i].gameObject.SetActive(false);
                            PlayerUI_10[i].SetActive(true);
                            Playernum_10[i].sprite = Number_prefab[(int)(GC.Character_Data[i].Character_Point / 10)];
                            Playernum_01[i].sprite = Number_prefab[GC.Character_Data[i].Character_Point % 10];
                        }
                        if (GC.Character_Data[i].character_NOW <= 0) PlayerDead[i].SetActive(true);
                    }
                }
            }
            Advantage_word.text = GC.Advantage;
            Disadvantages_word.text = GC.Disadvantages;
            if(GC.Gamemode_num==0)
            {
                Advantage_word_point.text = "" + GC.killnowUp+"体";
                Disadvantages_word_point.text = "" + GC.killnowDown + "体";
            }
            else
            {
                Advantage_word_point.text = "" + GC.PointnowUp + "点";
                Disadvantages_word_point.text = "" + GC.PointnowDown + "点";
            }
            
            //PASUE
            if (GC.Pause)
            {
                if (GC.Pause_chiocing)
                {
                    if (GC.Pause_watchGame)
                    {
                        Pasue_manu[4].SetActive(false);
                        for (int i = 0; i < GC.UIobj.Length; i++) GC.UIobj[i].SetActive(true);
                    }
                    else
                    {
                        Pasue_manu[4].SetActive(true);
                        for (int i = 0; i < GC.UIobj.Length; i++) GC.UIobj[i].SetActive(false);
                    }
                }
                else
                {
                    GC.Pause_chioce=GC.GI.MAXandMin(GC.Pause_chioce, 0, 3);
                    for (int i = 0; i < GC.UIobj.Length; i++) GC.UIobj[i].SetActive(false);
                    Pasue_manu[4].SetActive(true);
                    Pasue_manu[5].transform.position = Pasue_manu[GC.Pause_chioce].transform.position;
                    Pasue_manu[6].SetActive(true);
                    //BATTLE INFO
                    Pasue_manu[7].SetActive(true);
                    //左
                    for (int i = GC.Character_Data.Length; i < 16; i++) BattleInfo[i].gameObject.SetActive(false);
                    for (int i = 0; i < GC.Character_Data.Length; i++)
                    {
                        BattleInfo[i].gameObject.SetActive(true);
                        if (GC.Gamemode_num == 0)
                        {
                            BattleInfo_main[0].text = "撃破数";
                            switch(GC.Character_Data[i].characterTeam)
                            {
                                case 0:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].killnum + "    NOチーム";
                                    break;
                                case 1:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].killnum + "    赤チーム";
                                    break;
                                case 2:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].killnum + "    青チーム";
                                    break;
                                case 3:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].killnum + "    紫チーム";
                                    break;
                                case 4:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].killnum + "    黄チーム";
                                    break;
                            }
                            
                        }
                        else 
                        {
                            BattleInfo_main[0].text = "得点";
                            switch (GC.Character_Data[i].characterTeam)
                            {
                                case 0:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].Character_Point + "    NOチーム";
                                    break;
                                case 1:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].Character_Point + "    赤チーム";
                                    break;
                                case 2:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].Character_Point + "    青チーム";
                                    break;
                                case 3:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].Character_Point + "    紫チーム";
                                    break;
                                case 4:
                                    BattleInfo[i].text = GC.Character_Data[i].tag + " : " + "" + GC.Character_Data[i].Character_Point + "    黄チーム";
                                    break;
                            }
                        }
                    }
                    //右
                    if (GC.Gamemode_num == 0) BattleInfo_main[1].text = "チーム撃破数";
                    else BattleInfo_main[1].text = "チーム得点";
                    for (int i = 0; i < useteam.Count; i++)
                    {
                        BattleInfo2[i].gameObject.SetActive(true);
                        if (GC.Gamemode_num == 0)
                        {
                            if (useteam[i] == 0) BattleInfo2[i].text = "赤チーム : " + "" +GC.TeamKill[0];
                            else if (useteam[i] == 1) BattleInfo2[i].text = "青チーム : " + "" + GC.TeamKill[1];
                            else if (useteam[i] == 2) BattleInfo2[i].text = "紫チーム : " + "" + GC.TeamKill[2];
                            else if (useteam[i] == 3) BattleInfo2[i].text = "黄チーム : " + "" + GC.TeamKill[3];
                        }
                        else
                        {
                            if (useteam[i] == 0) BattleInfo2[i].text = "赤チーム : " + "" + GC.Team_Point[0];
                            else if (useteam[i] == 1) BattleInfo2[i].text = "青チーム : " + "" + GC.Team_Point[1];
                            else if (useteam[i] == 2) BattleInfo2[i].text = "紫チーム : " + "" + GC.Team_Point[2];
                            else if (useteam[i] == 3) BattleInfo2[i].text = "黄チーム : " + "" + GC.Team_Point[3];
                        }
                    }
                    for (int i = useteam.Count; i < 4; i++) BattleInfo2[i].gameObject.SetActive(false);
                    if (GC.Gamemode_num == 0)
                    {
                        BattleInfo2[4].text = GC.Advantage+" : "+""+GC.killnowUp +" 人";
                        BattleInfo2[5].text = GC.Disadvantages + " : " + "" + GC.killnowDown + " 人";
                    }
                    else
                    {
                        BattleInfo2[4].text = GC.Advantage + " : " + "" + GC.PointnowUp + " 点";
                        BattleInfo2[5].text = GC.Disadvantages + " : " + "" + GC.PointnowDown + " 点";
                    }
                }
            }
            else
            {
                Pasue_manu[4].SetActive(false);
                for (int i = 0; i < GC.UIobj.Length; i++) GC.UIobj[i].SetActive(true);
            }
            //Gamestate
            if(GC.GameSet_Already[0]==true && GC.Gamestate<4 &&GC.Gamemode_num==0)
            {
                Gameset_UI[2].SetActive(true);
            }else if ( GC.GameSet_Already[1] == true && GC.Gamestate < 4 && GC.Gamemode_num>0)
            {
                Gameset_UI[2].SetActive(true);
            }
            else  Gameset_UI[2].SetActive(false);
            if (GC.Gamestate==4)
            {
                if (GC.GameSet == 2)
                {
                    Gameset_UI[0].SetActive(true);
                }else if (GC.GameSet == 3)
                {
                    if(GC.Gamemode_num!=1) Gameset_UI[0].SetActive(true);
                }
            }
            if(GC.Gamestate == 5)
            {
                Gameset_UI[0].SetActive(false);
                Gameset_UI[1].SetActive(true);
            }
            
            if (UIcheck_center)
            {
                NormalUI[2].alpha -= 2 * Time.deltaTime;
                if (NormalUI[2].alpha <= 0.5f) NormalUI[2].alpha = 0.5f;
            }
            else
            {
                NormalUI[2].alpha += 2 * Time.deltaTime;
                if (NormalUI[2].alpha >= 1) NormalUI[2].alpha = 1;
            }
        }
    }
    
}
