using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Set3 : MonoBehaviour
{
    public SetControler SC;
    public Text[] Title_word;
    public GameObject[] bg;
    public GameObject[] no_use;     //P_NUM or C_num=0の時XX
    public GameObject changeg_word_chioce_obj;        //0:All 1:+  2:-
    public Image[] changeg_word_chioce_12;        //0:All 1:+  2:-
    public Animator[] changeg_word_chioce;
    public int[] teamTest;
    //COMMON SET
    public Text[] common_word;
    public Animator[] animator_common;
    //PLAYER SET
    public GameObject[] p_POS;
    public Text[] p_set_word;
    public Animator[] animator_player;
    //CPU SET
    public Set3_CPU S3_C;
    public GameObject cpu_seting_area;
    public Animator cpu_seting_area_ani;//エリアの顕現
    public Text[] c_set_word; //JOBの人数

    //MAP　SET
    public GameObject[] Map;
    public Image[] Map_Image;
    public GameObject mapchioce;
    public Animator[] animator_MAP;
    public Sprite[] NMap_Image;
    public Sprite[] BMap_Image;

    public Animator animator_SETOK;

    private void Start()
    {
        
        switch (SC.GI.Gamemode_choice)
        {
            case 0:
                //V battle 
                Title_word[0].text = "V battle";
                if (SC.GI.game_mode_chioce == 0) Title_word[1].text = "Normal  モード";
                else Title_word[1].text = "Zombie  モード";
                //勝利条件　1.時間終わった時命とHPが一番高い人とチーム　2.敵チーム全滅時
                for (int i = 0; i < Map_Image.Length; i++) Map_Image[i].sprite = NMap_Image[i];
                break;
            case 1:
                //BB battle
                Title_word[0].text = "BB battle";
                if (SC.GI.game_mode_chioce == 0) Title_word[1].text = "20点勝利  モード";
                else if (SC.GI.game_mode_chioce == 1) Title_word[1].text = "40点勝利  モード";
                else if (SC.GI.game_mode_chioce == 2) Title_word[1].text = "無限  モード";
                //勝利条件　1.時間終わった時点数高いチーム　2.点数が足りる時
                for (int i = 0; i < Map_Image.Length; i++) Map_Image[i].sprite = BMap_Image[i];
                break;
            case 2:
                //J battle
                Title_word[0].text = "J battle";
                if (SC.GI.game_mode_chioce == 0) Title_word[1].text = "20点勝利  モード";
                else if (SC.GI.game_mode_chioce == 1) Title_word[1].text = "40点勝利  モード";
                else if (SC.GI.game_mode_chioce == 2) Title_word[1].text = "無限  モード";
                //勝利条件　1.時間終わった時点数高いチーム　2.点数が足りる時
                for (int i = 0; i < Map_Image.Length; i++) Map_Image[i].sprite = NMap_Image[i];
                break;
        }
        teamTest = new int[5];//NABCD
        for (int i = 0; i < 5; i++) teamTest[i] = 0;
    }
    private void Update()
    {
        SC.set3_chioce_now=SC.GI.MAXandMin(SC.set3_chioce_now, 0, 7);
        SC.set3_chioce_now_P=SC.GI.MAXandMin(SC.set3_chioce_now_P, 0, (SC.GI.player_num-1));
        SC.set3_chioce_now_C=SC.GI.MAXandMin(SC.set3_chioce_now_C, 0, 9);
        //もしBBモードならばチーム二つ限定
        if (SC.GI.Gamemode_choice != 1)
        {
            teamTest[0] = 0;
            for (int i = 0; i < SC.GI.player_num; i++)
            { 
                SC.set3_P[i] = SC.GI.MAXandMin(SC.set3_P[i], 0, 4);
                if(SC.set3_P[i]==0) teamTest[0]++;
            }
            if(SC.set3_chioceing==false||SC.set3_chioceing_CPU==true)
            {
                TeamCheck();
            }
            SC.set3_chioce_now_C_area = SC.GI.MAXandMin(SC.set3_chioce_now_C_area, 0, 6);//NABCD
        }
        else
        {
            for (int i = 0; i < SC.GI.player_num; i++) SC.set3_P[i] = SC.GI.MAXandMin(SC.set3_P[i], 1, 2);
            if(SC.set3_chioceing_CPUteamarea)SC.set3_chioce_now_C_area = SC.GI.MAXandMin(SC.set3_chioce_now_C_area, 3, 4);//AB
            else SC.set3_chioce_now_C_area = SC.GI.MAXandMin(SC.set3_chioce_now_C_area, 0, 4);
            int sumA = 0;
            int sumB = 0;
            for (int i = 0; i < SC.GI.player_num; i++)
            {
                if (SC.set3_P[i] % 2 != 0) sumA++;
                else sumB++;
            }
            SC.TeamAnum = sumA + SC.set3_C_team_A.Count;
            SC.TeamBnum = sumB + SC.set3_C_team_B.Count;
        }

        if (SC.set3_map <= 0) SC.set3_map = 0;
        if(SC.set3_chioceing&&SC.set3_chioce_now<=4)
        {
            changeg_word_chioce_obj.SetActive(true);
            if(SC.set3_chioce_now==4)
            {
                changeg_word_chioce_obj.transform.position = p_set_word[SC.set3_chioce_now_P].transform.position;
            }else
            {
                changeg_word_chioce_obj.transform.position = common_word[SC.set3_chioce_now].transform.position;
            }
            
            //<>UI
            if (SC.set3_chioce_now <= 3)
            {
                if (SC.set3_common[SC.set3_chioce_now] <= 0) changeg_word_chioce_12[0].enabled = false;
                else changeg_word_chioce_12[0].enabled = true;
                switch (SC.set3_chioce_now)
                {
                    case 0:
                        if (SC.set3_common[0] <= 1) changeg_word_chioce_12[0].enabled = false;
                        else if (SC.set3_common[0] > 1) changeg_word_chioce_12[0].enabled = true;
                        if (SC.set3_common[0] >= 4) changeg_word_chioce_12[1].enabled = false;
                        else changeg_word_chioce_12[1].enabled = true;
                        break;
                    case 1:
                        if (SC.set3_common[SC.set3_chioce_now] >= 1) changeg_word_chioce_12[1].enabled = false;
                        else changeg_word_chioce_12[1].enabled = true;
                        break;
                    case 2:
                        if (SC.set3_common[2] >= 2) changeg_word_chioce_12[1].enabled = false;
                        else changeg_word_chioce_12[1].enabled = true;
                        if (SC.GI.Gamemode_choice == 0)
                        {
                            if (SC.set3_common[0] == 4 && SC.set3_common[2] >= 1) changeg_word_chioce_12[1].enabled = false;
                            else if (SC.set3_common[0] == 4 && SC.set3_common[2] == 0) changeg_word_chioce_12[1].enabled = true;
                        }
                        else
                        {
                            if (SC.GI.game_mode_chioce == 2 && SC.set3_common[2] >= 1) changeg_word_chioce_12[1].enabled = false;
                            else if (SC.GI.game_mode_chioce == 2 && SC.set3_common[2] == 0) changeg_word_chioce_12[1].enabled = true;
                        }
                        break;
                    case 3:
                        if (SC.set3_common[SC.set3_chioce_now] >= 1) changeg_word_chioce_12[1].enabled = false;
                        else changeg_word_chioce_12[1].enabled = true;
                        break;
                }
                
            }
            else if (SC.set3_chioce_now == 4)
            {
                
                if(SC.GI.Gamemode_choice==1)
                {
                    if (SC.set3_P[SC.set3_chioce_now_P] < 2) changeg_word_chioce_12[0].enabled = false;
                    else changeg_word_chioce_12[0].enabled = true;
                    if (SC.set3_P[SC.set3_chioce_now_P] > 1) changeg_word_chioce_12[1].enabled = false;
                    else changeg_word_chioce_12[1].enabled = true;
                }
                else
                {
                    if (SC.set3_P[SC.set3_chioce_now_P] <= 0) changeg_word_chioce_12[0].enabled = false;
                    else changeg_word_chioce_12[0].enabled = true;
                    if (SC.set3_P[SC.set3_chioce_now_P] >= 4) changeg_word_chioce_12[1].enabled = false;
                    else changeg_word_chioce_12[1].enabled = true;
                }
            }
            if (SC.set3_chioce_now == 0 && SC.GI.Gamemode_choice == 0 && SC.GI.game_mode_chioce == 1) changeg_word_chioce_obj.SetActive(false);
        }
        else changeg_word_chioce_obj.SetActive(false);
        
        //common変数============================
        if (SC.GI.Gamemode_choice == 0)
        {
            SC.set3_common[0] = SC.GI.MAXandMin(SC.set3_common[0], 1, 4);//命数　　player_life_max関わる;
            SC.set3_common[2] = SC.GI.MAXandMin(SC.set3_common[2], 0, 2);//ゲームTIMEの選択肢数量
            if (SC.set3_common[0]==4) SC.set3_common[2] = SC.GI.MAXandMin(SC.set3_common[2], 0, 1);
            else if (SC.set3_common[2] == 2) SC.set3_common[0] = SC.GI.MAXandMin(SC.set3_common[0], 1, 3);
        }
        else
        {
            SC.set3_common[0] = SC.GI.MAXandMin(SC.set3_common[0], 1, 4);
            SC.set3_common[2] = SC.GI.MAXandMin(SC.set3_common[2], 0, 2);
            if (SC.GI.game_mode_chioce == 2) SC.set3_common[2] = SC.GI.MAXandMin(SC.set3_common[2], 0, 1);
        }

        SC.set3_chioce_now_M = SC.GI.MAXandMin(SC.set3_chioce_now_M, 0, 0);
        SC.set3_common[1]=SC.GI.MAXandMin(SC.set3_common[1], 0, 1);//MP
        SC.set3_common[3] = SC.GI.MAXandMin(SC.set3_common[3], 0, 1);//ITEM    
        //BGの処理==============================================================
        if (SC.set3_chioce_now == 4)bg[4].SetActive(true);
        else if (SC.set3_chioce_now != 4)bg[4].SetActive(false);
        if (SC.set3_chioce_now == 5) bg[5].SetActive(true);
        else if (SC.set3_chioce_now != 5) bg[5].SetActive(false);
        if (SC.set3_chioceing)bg[6].SetActive(true);
        else for(int i=6;i<8;i++) bg[i].SetActive(false);
        //選択ゲージ
        if(SC.set3_chioce_now< 4)
        {
            for(int i=0;i< 4;i++)
            {
                if(i== SC.set3_chioce_now) bg[i].SetActive(true);
                else bg[i].SetActive(false);
            }
        }else for (int i = 0; i < 4; i++) bg[i].SetActive(false);
        if (SC.set3_chioceing && SC.set3_chioce_now == 4)
        {
            bg[7].SetActive(true);
            bg[7].transform.position = p_POS[SC.set3_chioce_now_P].transform.position;
        } else bg[7].SetActive(false);
        
        //変数の処理==============================================================
        //COMMON
        if (SC.GI.game_mode_chioce == 1&& SC.GI.Gamemode_choice==0) SC.set3_common[0] = 1;
        if (SC.set3_common[0] <= 3) common_word[0].text = "" + SC.set3_common[0];
        else common_word[0].text = "∞";
        if (SC.set3_common[1] == 0) common_word[1].text = "ON";
        else if (SC.set3_common[1] == 1) common_word[1].text = "OFF";
        if (SC.set3_common[2] == 0) common_word[2].text = "2:00";
        else if (SC.set3_common[2] == 1) common_word[2].text = "4:00";
        else if (SC.set3_common[2] == 2) common_word[2].text = "∞";
        if (SC.set3_common[3] == 0) common_word[3].text = "ON";
        else if (SC.set3_common[3] == 1) common_word[3].text = "OFF";
        //勝利条件の変数
        if(SC.GI.Gamemode_choice==0) common_word[4].text = "[敵を全滅する]or[時間制限終わるまで生きる]";
        else  common_word[4].text = "時間制限終了orチーム全滅まで得点高い";
        //PLAYER
        if(SC.GI.player_num>0)
        {
            no_use[0].SetActive(false);
            for (int i = 0; i < SC.GI.player_num; i++)
            {
                if (SC.set3_P[i] == 0)
                {
                    p_set_word[i].text = "NO";
                    p_set_word[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else if (SC.set3_P[i] == 1)
                {
                    p_set_word[i].text = "赤";
                    p_set_word[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                }
                else if (SC.set3_P[i] == 2)
                {
                    p_set_word[i].text = "青";
                    p_set_word[i].color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                }
                else if (SC.set3_P[i] == 3)
                {
                    p_set_word[i].text = "紫";
                    p_set_word[i].color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                }
                else if (SC.set3_P[i] == 4)
                {
                    p_set_word[i].text = "黄";
                    p_set_word[i].color = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                }
            }
            for(int i= SC.GI.player_num;i<4;i++)
            {
                p_set_word[i].text = "未参加";
                p_set_word[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            }
        }
        else no_use[0].SetActive(true);
        //CPU
        if (SC.GI.cpu_num > 0)
        {
            no_use[1].SetActive(false);
        }
        else no_use[1].SetActive(true);
        c_set_word[0].text = "" + SC.set3_C_job[0];
        c_set_word[1].text = "" + SC.set3_C_job[1];
        c_set_word[2].text = "" + SC.set3_C_job[2];
        c_set_word[3].text = "" + SC.set3_C_job[3];
        c_set_word[4].text = "" + SC.set3_C_job[4];
        c_set_word[5].text = "" + SC.set3_C_team_N.Count;
        c_set_word[6].text = "" + SC.set3_C_team_A.Count;
        c_set_word[7].text = "" + SC.set3_C_team_B.Count;
        c_set_word[8].text = "" + SC.set3_C_team_C.Count;
        c_set_word[9].text = "" + SC.set3_C_team_D.Count;
        c_set_word[10].text = "" + SC.GI.cpu_num;
        //MAP
        if (SC.set3_chioce_now==6&&SC.set3_chioceing==false) mapchioce.SetActive(true);
        else if (SC.set3_chioce_now != 6|| SC.set3_chioceing==true) mapchioce.SetActive(false);
        Map[SC.set3_map].SetActive(false);//黒効果クロス
        //多MAPの処理追加


        //ANIMETORの処理==============================================================
        if(SC.set3_chioceing)
        {
            if(SC.set3_chioce_now<=3)       //COMMON
            {
                for(int i=0;i<4;i++)
                {
                    if (i == SC.set3_chioce_now) animator_common[i].SetBool("OK", true);
                    else animator_common[i].SetBool("OK", false);
                }
            }
            else if (SC.set3_chioce_now == 4)//PLAYER
            {
                for (int i = 0; i < 4; i++)     
                {
                    if (i == SC.set3_chioce_now_P) animator_player[i].SetBool("OK", true);
                    else animator_player[i].SetBool("OK", false);
                }
            }
            else if (SC.set3_chioce_now == 5)   //CPU
            {
               
            }
            if (SC.set3_chioce_now == 6)   //MAP
            {
                for (int i = 0; i < animator_MAP.Length; i++)
                {
                    if(i== SC.set3_chioce_now_M) animator_MAP[i].SetBool("OK", true);
                    else animator_MAP[i].SetBool("OK", false);
                }
            }else
            {
                for(int i=0;i< animator_MAP.Length;i++) animator_MAP[i].SetBool("OK", false);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                animator_common[i].SetBool("OK", false);
                animator_player[i].SetBool("OK", false);
            }
            
            animator_MAP[0].SetBool("OK", false);
        }
        if (SC.set3_chioce_now == 7&& SC.set3_chioceing==false) animator_SETOK.SetBool("OK", true);
        else if (SC.set3_chioce_now != 7)animator_SETOK.SetBool("OK", false);
        else if (SC.set3_chioce_now == 7 && SC.set3_chioceing == true) animator_SETOK.SetBool("OK", true);
    }
    public void TeamCheck()
    {
        if (SC.set3_C_team_N.Count == 0 && teamTest[0] == 0)
        {
            for (int i = 0; i < SC.GI.player_num; i++)
            {
                switch (SC.set3_P[i])
                {
                    case 1:
                        teamTest[1]++;
                        break;
                    case 2:
                        teamTest[2]++;
                        break;
                    case 3:
                        teamTest[3]++;
                        break;
                    case 4:
                        teamTest[4]++;
                        break;
                }
            }
            teamTest[1] += SC.set3_C_team_A.Count;
            teamTest[2] += SC.set3_C_team_B.Count;
            teamTest[3] += SC.set3_C_team_C.Count;
            teamTest[4] += SC.set3_C_team_D.Count;
            if (teamTest[1] == 0)
            {
                if (teamTest[2] == 0)
                {
                    if (teamTest[3] == 0)
                    {
                        if (SC.GI.cpu_num != 0)
                        {
                            SC.set3_C_team_N.Add(SC.set3_C_team_D[SC.set3_C_team_D.Count - 1]);
                            SC.set3_C_team_D.RemoveAt(SC.set3_C_team_D.Count - 1);
                            S3_C.Sorting_mark();
                        }
                        else
                        {
                            SC.set3_P[0] = 0;
                        }
                    }
                    else
                    {
                        if (teamTest[4] == 0)
                        {
                            if (SC.GI.cpu_num != 0)
                            {
                                SC.set3_C_team_N.Add(SC.set3_C_team_C[SC.set3_C_team_C.Count - 1]);
                                SC.set3_C_team_C.RemoveAt(SC.set3_C_team_C.Count - 1);
                                S3_C.Sorting_mark();
                            }
                            else
                            {
                                SC.set3_P[0] = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (teamTest[3] == 0)
                    {
                        if (teamTest[4] == 0)
                        {
                            if (SC.GI.cpu_num != 0)
                            {
                                SC.set3_C_team_N.Add(SC.set3_C_team_B[SC.set3_C_team_B.Count - 1]);
                                SC.set3_C_team_B.RemoveAt(SC.set3_C_team_B.Count - 1);
                                S3_C.Sorting_mark();
                            }
                            else
                            {
                                SC.set3_P[0] = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                if (teamTest[2] == 0)
                {
                    if (teamTest[3] == 0)
                    {
                        if (teamTest[4] == 0)
                        {
                            if (SC.GI.cpu_num != 0)
                            {
                                SC.set3_C_team_N.Add(SC.set3_C_team_A[SC.set3_C_team_A.Count - 1]);
                                SC.set3_C_team_A.RemoveAt(SC.set3_C_team_A.Count - 1);
                                S3_C.Sorting_mark();
                            }
                            else
                            {
                                SC.set3_P[0] = 0;
                            }
                        }
                    }
                }
            }
        }
    }
    public void Active_false()
    {
        this.gameObject.SetActive(false);
    }
    public void stepUp()
    {
        SC.GI.setingstate++;
    }
    

}
