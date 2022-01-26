using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Set3_CPU : MonoBehaviour
{
    public SetControler SC;
    public GameObject[] cpu_bg;
    public C_mark[] teammark;
    public int[] teammark_jobnow;
    public Transform[] Area_pos;
    public Transform[] N_POS;
    public Transform[] A_POS;
    public Transform[] B_POS;
    public Transform[] C_POS;
    public Transform[] D_POS;

    public GameObject[] c_word_POS;
    public GameObject c_word_chioce_obj;
    public Image[] c_word_chioce_objset;
    public Animator[] c_word_chioce;
    public Text[] c_setIN_word; //JOBの人数
    public Text[] c_Teamnum_word;
    public Animator[] animator_cpu_word;//JOBの人数のBIGGER
    public bool Sorting_check;
    public bool trun;
    // Start is called before the first frame update
    void Start()
    {
        teammark_jobnow = new int[12];
        Sorting_check = false;
        //使用したMARK
        for (int i = 0; i < SC.GI.cpu_num; i++)
        {
            teammark[i].self_obj.SetActive(true);
        }
        for (int i = SC.GI.cpu_num; i < 12; i++)
        {
            teammark[i].self_obj.SetActive(false);
        }

        c_setIN_word[0].text = "" + SC.set3_C_job[0];
        c_setIN_word[1].text = "" + SC.set3_C_job[1];
        c_setIN_word[2].text = "" + SC.set3_C_job[2];
        c_setIN_word[3].text = "" + SC.set3_C_job[3];
        c_setIN_word[4].text = "" + SC.set3_C_job[4];
        trun = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trun == false)
        {
            Sorting_job();
            trun = true;
        }
        for (int i = 0; i < SC.GI.cpu_num; i++)
        {
            teammark[i].self_obj.SetActive(true);
        }
        for (int i = SC.GI.cpu_num; i < 12; i++)
        {
            teammark[i].self_obj.SetActive(false);
        }
        //今はCPUSETTING
        if (SC.set3_chioce_now==5&&SC.set3_chioceing)
        {
            if (SC.set3_chioceing_CPU && SC.set3_chioce_now_C > 1)
            {
                c_word_chioce_obj.SetActive(true);
                c_word_chioce_obj.transform.position = c_setIN_word[(SC.set3_chioce_now_C - 2)].transform.position;
                if (SC.set3_C_job[SC.set3_chioce_now_C - 2] <= 0) c_word_chioce_objset[0].enabled = false;
                else c_word_chioce_objset[0].enabled = true;
                if (SC.set3_C_job[SC.set3_chioce_now_C - 2] >= SC.GI.cpu_num) c_word_chioce_objset[1].enabled = false;
                else c_word_chioce_objset[1].enabled = true;
            }
            else c_word_chioce_obj.SetActive(false);


            if (Sorting_check == true)
            {
                Sorting_mark();
            }
            //文字の変更
            
            c_setIN_word[0].text = "" + SC.set3_C_job[0];
            c_setIN_word[1].text = "" + SC.set3_C_job[1];
            c_setIN_word[2].text = "" + SC.set3_C_job[2];
            c_setIN_word[3].text = "" + SC.set3_C_job[3];
            c_setIN_word[4].text = "" + SC.set3_C_job[4];
            //IN　TEAMエリア
            if (SC.set3_chioceing_CPU == true && SC.set3_chioce_now_C == 0)
            {
                //chioceingbg処理
                cpu_bg[3].SetActive(true);  
                if(SC.set3_chioceing_CPUteamarea==true) cpu_bg[4].SetActive(true);
                else cpu_bg[4].SetActive(false);

                
                //TEAMMARKの選択効果
                if (SC.set3_chioceing_CPUteamarea == true&& SC.set3_chioceing_CPUteam_move==false)
                {
                    int num = 0;
                    switch (SC.set3_chioce_now_C_area)
                    {
                        case 2:
                            num = SC.set3_chioce_now_C_teammark;
                            break;
                        case 3:
                            num = SC.set3_C_team_N.Count + SC.set3_chioce_now_C_teammark;
                            break;
                        case 4:
                            num = SC.set3_C_team_N.Count + SC.set3_C_team_A.Count + SC.set3_chioce_now_C_teammark;
                            break;
                        case 5:
                            num = SC.set3_C_team_N.Count + SC.set3_C_team_A.Count + SC.set3_C_team_B.Count+SC.set3_chioce_now_C_teammark;
                            break;
                        case 6:
                            num = SC.set3_C_team_N.Count + SC.set3_C_team_A.Count + SC.set3_C_team_B.Count + SC.set3_C_team_C.Count+ SC.set3_chioce_now_C_teammark;
                            break;
                    }
                    for (int i = 0; i < SC.GI.cpu_num; i++)
                    {
                        if(i==num)teammark[i].chioceobj.SetActive(true);
                        else teammark[i].chioceobj.SetActive(false);
                    }
                }
                else for (int i = 0; i < SC.GI.cpu_num; i++) teammark[i].chioceobj.SetActive(false);
                //MARK移動中の位置制御
                if (SC.set3_chioceing_CPUteam_move)
                {
                    teammark[SC.set3_chioce_now_C_teammark_num].transform.position = Area_pos[SC.set3_chioce_now_C_area-2].transform.position;
                }

            }else cpu_bg[3].SetActive(false);
            //IN　JOBエリア
            if (SC.set3_chioceing_CPU == true && SC.set3_chioce_now_C >= 1)
            {
                cpu_bg[1].SetActive(true);  //chioceingbg処理
                for (int i = 0; i < 5; i++)
                {
                    if (i == (SC.set3_chioce_now_C-2)) animator_cpu_word[i].SetBool("OK", true);//文字の大きい化ON
                    else animator_cpu_word[i].SetBool("OK", false);
                }
            }else cpu_bg[1].SetActive(false);
            //NO CHIOCE
            if (SC.set3_chioceing_CPU==false)
            {
                for (int i = 1; i < 10; i++) cpu_bg[i].SetActive(false);
                //もし変更したら
                for (int i = 0; i < 5; i++) animator_cpu_word[i].SetBool("OK", false);//文字の大きい化OFF
            }
            //選択ゲージ
            if (SC.set3_chioceing_CPU == false||(SC.set3_chioceing_CPU == true&& SC.set3_chioce_now_C>=1))
            {
                cpu_bg[0].SetActive(true);
                cpu_bg[0].transform.position = c_word_POS[SC.set3_chioce_now_C].transform.position;
            }else cpu_bg[0].SetActive(false);
            if(SC.set3_chioceing_CPU == true && SC.set3_chioce_now_C == 0)
            {
                if(SC.set3_chioceing_CPUteamarea==false&&SC.set3_chioce_now_C_area<=1)
                {
                    cpu_bg[2].SetActive(true);
                    cpu_bg[2].transform.position = c_word_POS[7 + SC.set3_chioce_now_C_area].transform.position;
                }
                else cpu_bg[2].SetActive(false);
                if (SC.set3_chioceing_CPUteamarea == false && SC.set3_chioce_now_C_area >= 2)
                {
                    for(int i=5;i<10;i++)
                    {
                        if(i== SC.set3_chioce_now_C_area + 3) cpu_bg[i].SetActive(true);
                        else cpu_bg[i].SetActive(false);
                    }
                }
                else for (int i = 5; i < 10; i++) cpu_bg[i].SetActive(false);
                //else
            }
        }
        c_Teamnum_word[0].text = ""+SC.set3_C_team_N.Count;
        c_Teamnum_word[1].text = "" + SC.set3_C_team_A.Count;
        c_Teamnum_word[2].text = "" + SC.set3_C_team_B.Count;
        c_Teamnum_word[3].text = "" + SC.set3_C_team_C.Count;
        c_Teamnum_word[4].text = "" + SC.set3_C_team_D.Count;
    }
    void active_off()
    {
        this.gameObject.SetActive(false);
    }
    public void Sorting_job()
    {
        int[] num=new int[5];       //職業の総数　0:S 1:P 2:M 3:G 4:A
        int[] numS = new int[5];    //チーム別の職業数0:N 1:A 2:B 3:C 4:D
        int[] numP = new int[5];    //チーム別の職業数0:N 1:A 2:B 3:C 4:D
        int[] numM = new int[5];    //チーム別の職業数0:N 1:A 2:B 3:C 4:D
        int[] numG = new int[5];    //チーム別の職業数0:N 1:A 2:B 3:C 4:D
        int[] numA = new int[5];    //チーム別の職業数0:N 1:A 2:B 3:C 4:D
        for (int i = 0; i < 5; i++)
        {
            num[i] = 0;
            numS[i] = 0;
            numP[i] = 0;
            numM[i] = 0;
            numG[i] = 0;
            numA[i] = 0;
        }
        for (int i = 0; i < SC.set3_C_team_N.Count; i++)
        {
            switch((SC.set3_C_team_N[i]))
            {
                case 0:
                    num[0]++;
                    numS[0]++;
                    break;
                case 1:
                    num[1]++;
                    numP[0]++;
                    break;
                case 2:
                    num[2]++;
                    numM[0]++;
                    break;
                case 3:
                    num[3]++;
                    numG[0]++;
                    break;
                case 4:
                    num[4]++;
                    numA[0]++;
                    break;
            }
        }
        for (int i = 0; i < SC.set3_C_team_A.Count; i++)
        {
            switch ((SC.set3_C_team_A[i]))
            {
                case 0:
                    num[0]++;
                    numS[1]++;
                    break;
                case 1:
                    num[1]++;
                    numP[1]++;
                    break;
                case 2:
                    num[2]++;
                    numM[1]++;
                    break;
                case 3:
                    num[3]++;
                    numG[1]++;
                    break;
                case 4:
                    num[4]++;
                    numA[1]++;
                    break;
            }
        }
        for (int i = 0; i < SC.set3_C_team_B.Count; i++)
        {
            switch ((SC.set3_C_team_B[i]))
            {
                case 0:
                    num[0]++;
                    numS[2]++;
                    break;
                case 1:
                    num[1]++;
                    numP[2]++;
                    break;
                case 2:
                    num[2]++;
                    numM[2]++;
                    break;
                case 3:
                    num[3]++;
                    numG[2]++;
                    break;
                case 4:
                    num[4]++;
                    numA[2]++;
                    break;
            }
        }
        for (int i = 0; i < SC.set3_C_team_C.Count; i++)
        {
            switch ((SC.set3_C_team_C[i]))
            {
                case 0:
                    num[0]++;
                    numS[3]++;
                    break;
                case 1:
                    num[1]++;
                    numP[3]++;
                    break;
                case 2:
                    num[2]++;
                    numM[3]++;
                    break;
                case 3:
                    num[3]++;
                    numG[3]++;
                    break;
                case 4:
                    num[4]++;
                    numA[3]++;
                    break;
            }
        }
        for (int i = 0; i < SC.set3_C_team_D.Count; i++)
        {
            switch ((SC.set3_C_team_D[i]))
            {
                case 0:
                    num[0]++;
                    numS[4]++;
                    break;
                case 1:
                    num[1]++;
                    numP[4]++;
                    break;
                case 2:
                    num[2]++;
                    numM[4]++;
                    break;
                case 3:
                    num[3]++;
                    numG[4]++;
                    break;
                case 4:
                    num[4]++;
                    numA[4]++;
                    break;
            }
        }
        //S職
        while (num[0] != SC.set3_C_job[0])
        {
            if (num[0] > SC.set3_C_job[0])
            {
                if (numS[4] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_D.Count; i++)
                    {
                        if (SC.set3_C_team_D[i] == 0)
                        {
                            SC.set3_C_team_D.RemoveAt(i);
                            numS[4]--;
                            num[0]--;
                            i--;
                            if (num[0] != SC.set3_C_job[0])
                            {
                                if (SC.set3_C_team_D.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numS[3] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_C.Count; i++)
                    {
                        if (SC.set3_C_team_C[i] == 0)
                        {
                            SC.set3_C_team_C.RemoveAt(i);
                            numS[3]--;
                            num[0]--;
                            i--;
                            if (num[0] != SC.set3_C_job[0])
                            {
                                if (SC.set3_C_team_C.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numS[2] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_B.Count; i++)
                    {
                        if (SC.set3_C_team_B[i] == 0)
                        {
                            SC.set3_C_team_B.RemoveAt(i);
                            numS[2]--;
                            num[0]--;
                            i--;
                            if (num[0] != SC.set3_C_job[0])
                            {
                                if (SC.set3_C_team_B.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numS[1] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_A.Count; i++)
                    {
                        if (SC.set3_C_team_A[i] == 0)
                        {
                            SC.set3_C_team_A.RemoveAt(i);
                            numS[1]--;
                            num[0]--;
                            i--;
                            if (num[0] != SC.set3_C_job[0])
                            {
                                if (SC.set3_C_team_A.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numS[0] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_N.Count; i++)
                    {
                        if (SC.set3_C_team_N[i] == 0)
                        {
                            SC.set3_C_team_N.RemoveAt(i);
                            numS[0]--;
                            num[0]--;
                            i--;
                            if (num[0] != SC.set3_C_job[0])
                            {
                                if (SC.set3_C_team_N.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (num[0] < SC.set3_C_job[0])
            {
                if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(0);
                else SC.set3_C_team_N.Add(0);
                num[0]++;
            }
        }
        //P職
        while (num[1] != SC.set3_C_job[1])
        {
            if (num[1] > SC.set3_C_job[1])
            {
                if (numP[4] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_D.Count; i++)
                    {
                        if (SC.set3_C_team_D[i] == 1)
                        {
                            SC.set3_C_team_D.RemoveAt(i);
                            numP[4]--;
                            num[1]--;
                            i--;
                            if (num[1] != SC.set3_C_job[1])
                            {
                                if (SC.set3_C_team_D.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numP[3] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_C.Count; i++)
                    {
                        if (SC.set3_C_team_C[i] == 1)
                        {
                            SC.set3_C_team_C.RemoveAt(i);
                            numP[3]--;
                            num[1]--;
                            i--;
                            if (num[1] != SC.set3_C_job[1])
                            {
                                if (SC.set3_C_team_C.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numP[2] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_B.Count; i++)
                    {
                        if (SC.set3_C_team_B[i] == 1)
                        {
                            SC.set3_C_team_B.RemoveAt(i);
                            numP[2]--;
                            num[1]--;
                            i--;
                            if (num[1] != SC.set3_C_job[1])
                            {
                                if (SC.set3_C_team_B.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numP[1] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_A.Count; i++)
                    {
                        if (SC.set3_C_team_A[i] == 1)
                        {
                            SC.set3_C_team_A.RemoveAt(i);
                            numP[1]--;
                            num[1]--;
                            i--;
                            if (num[1] != SC.set3_C_job[1])
                            {
                                if (SC.set3_C_team_A.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numP[0] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_N.Count; i++)
                    {
                        if (SC.set3_C_team_N[i] == 1)
                        {
                            SC.set3_C_team_N.RemoveAt(i);
                            numP[0]--;
                            num[1]--;
                            i--;
                            if (num[1] != SC.set3_C_job[1])
                            {
                                if (SC.set3_C_team_N.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (num[1] < SC.set3_C_job[1])
            {
                while (num[1] != SC.set3_C_job[1])
                {
                    if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(1);
                    else SC.set3_C_team_A.Add(1);
                    num[1]++;
                }
            }
        }
        //M職
        while (num[2] != SC.set3_C_job[2])
        {
            if (num[2] > SC.set3_C_job[2])
            {
                if (numM[4] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_D.Count; i++)
                    {
                        if (SC.set3_C_team_D[i] == 2)
                        {
                            SC.set3_C_team_D.RemoveAt(i);
                            numM[4]--;
                            num[2]--;
                            i--;
                            if (num[2] != SC.set3_C_job[2])
                            {
                                if (SC.set3_C_team_D.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numM[3] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_C.Count; i++)
                    {
                        if (SC.set3_C_team_C[i] == 2)
                        {
                            SC.set3_C_team_C.RemoveAt(i);
                            numM[3]--;
                            num[2]--;
                            i--;
                            if (num[2] != SC.set3_C_job[2])
                            {
                                if (SC.set3_C_team_C.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numM[2] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_B.Count; i++)
                    {
                        if (SC.set3_C_team_B[i] == 2)
                        {
                            SC.set3_C_team_B.RemoveAt(i);
                            numM[2]--;
                            num[2]--;
                            i--;
                            if (num[2] != SC.set3_C_job[2])
                            {
                                if (SC.set3_C_team_B.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numM[1] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_A.Count; i++)
                    {
                        if (SC.set3_C_team_A[i] == 2)
                        {
                            SC.set3_C_team_A.RemoveAt(i);
                            numM[1]--;
                            num[2]--;
                            i--;
                            if (num[2] != SC.set3_C_job[2])
                            {
                                if (SC.set3_C_team_A.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numM[0] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_N.Count; i++)
                    {
                        if (SC.set3_C_team_N[i] == 2)
                        {
                            SC.set3_C_team_N.RemoveAt(i);
                            numM[0]--;
                            num[2]--;
                            i--;
                            if (num[2] != SC.set3_C_job[2])
                            {
                                if (SC.set3_C_team_N.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (num[2] < SC.set3_C_job[2])
            {
                while (num[2] != SC.set3_C_job[2])
                {
                    if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(2);
                    else SC.set3_C_team_A.Add(2);
                    num[2]++;
                }
            }
        }
        //G職
        while (num[3] != SC.set3_C_job[3])
        {
            if (num[3] > SC.set3_C_job[3])
            {
                if (numG[4] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_D.Count; i++)
                    {
                        if (SC.set3_C_team_D[i] == 3)
                        {
                            SC.set3_C_team_D.RemoveAt(i);
                            numG[4]--;
                            num[3]--;
                            i--;
                            if (num[3] != SC.set3_C_job[3])
                            {
                                if (SC.set3_C_team_D.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numG[3] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_C.Count; i++)
                    {
                        if (SC.set3_C_team_C[i] == 3)
                        {
                            SC.set3_C_team_C.RemoveAt(i);
                            numG[3]--;
                            num[3]--;
                            i--;
                            if (num[3] != SC.set3_C_job[3])
                            {
                                if (SC.set3_C_team_C.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numG[2] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_B.Count; i++)
                    {
                        if (SC.set3_C_team_B[i] == 3)
                        {
                            SC.set3_C_team_B.RemoveAt(i);
                            numG[2]--;
                            num[3]--;
                            i--;
                            if (num[3] != SC.set3_C_job[3])
                            {
                                if (SC.set3_C_team_B.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numG[1] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_A.Count; i++)
                    {
                        if (SC.set3_C_team_A[i] == 3)
                        {
                            SC.set3_C_team_A.RemoveAt(i);
                            numG[1]--;
                            num[3]--;
                            i--;
                            if (num[3] != SC.set3_C_job[3])
                            {
                                if (SC.set3_C_team_A.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numG[0] > 0)
                {
                    for (int i = 0; i < SC.set3_C_team_N.Count; i++)
                    {
                        if (SC.set3_C_team_N[i] == 3)
                        {
                            SC.set3_C_team_N.RemoveAt(i);
                            numG[0]--;
                            num[3]--;
                            i--;
                            if (num[3] != SC.set3_C_job[3])
                            {
                                if (SC.set3_C_team_N.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (num[3] < SC.set3_C_job[3])
            {
                while (num[3] != SC.set3_C_job[3])
                {
                    if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(3);
                    else SC.set3_C_team_A.Add(3);
                    num[3]++;
                }
            }
        }
        //A職
        while (num[4] != SC.set3_C_job[4])
        {
            if (num[4] > SC.set3_C_job[4])
            {
                if (numA[4] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_D.Count; i++)
                    {
                        if (SC.set3_C_team_D[i] == 4)
                        {
                            SC.set3_C_team_D.RemoveAt(i);
                            numA[4]--;
                            num[4]--;
                            i--;
                            if (num[4] != SC.set3_C_job[4])
                            {
                                if (SC.set3_C_team_D.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numA[3] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_C.Count; i++)
                    {
                        if (SC.set3_C_team_C[i] == 4)
                        {
                            SC.set3_C_team_C.RemoveAt(i);
                            numA[3]--;
                            num[4]--;
                            i--;
                            if (num[4] != SC.set3_C_job[4])
                            {
                                if (SC.set3_C_team_C.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numA[2] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_B.Count; i++)
                    {
                        if (SC.set3_C_team_B[i] == 4)
                        {
                            SC.set3_C_team_B.RemoveAt(i);
                            numA[2]--;
                            num[4]--;
                            i--;
                            if (num[4] != SC.set3_C_job[4])
                            {
                                if (SC.set3_C_team_B.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numA[1] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_A.Count; i++)
                    {
                        if (SC.set3_C_team_A[i] == 4)
                        {
                            SC.set3_C_team_A.RemoveAt(i);
                            numA[1]--;
                            num[4]--;
                            i--;
                            if (num[4] != SC.set3_C_job[4])
                            {
                                if (SC.set3_C_team_A.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
                else if (numA[0] != 0)
                {
                    for (int i = 0; i < SC.set3_C_team_N.Count; i++)
                    {
                        if (SC.set3_C_team_N[i] == 4)
                        {
                            SC.set3_C_team_N.RemoveAt(i);
                            numA[0]--;
                            num[4]--;
                            i--;
                            if (num[4] != SC.set3_C_job[4])
                            {
                                if (SC.set3_C_team_N.Count != 0) continue;
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            else if (num[4] < SC.set3_C_job[4])
            {
                while (num[4] != SC.set3_C_job[4])
                {
                    if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(4);
                    else SC.set3_C_team_A.Add(4);
                    num[4]++;
                }
            }
        }

        while((num[0]+ num[1]+ num[2]+ num[3]+ num[4])<SC.GI.cpu_num)
        {
            SC.set3_C_job[0]++;
            num[0]++;
            if(SC.GI.Gamemode_choice!=1) SC.set3_C_team_N.Add(0);
            else SC.set3_C_team_A.Add(0);

        }
        if(SC.GI.Gamemode_choice==1&& SC.set3_C_team_A.Count>=SC.GI.cpu_num)
        {
            SC.set3_C_team_B.Add(SC.set3_C_team_A[SC.set3_C_team_A.Count-1]);
            SC.set3_C_team_A.RemoveAt(SC.set3_C_team_A.Count - 1);
        }
        Sorting_mark();
    }
    public void Sorting_mark()
    {
        //チームの違うでチームエリアに配分
        int num = 0;
        for (int i = 0; i < SC.set3_C_team_N.Count; i++)
        {
            teammark[num].transform.position = N_POS[i].transform.position;
            num++;
        }
        for (int i = 0; i < SC.set3_C_team_A.Count; i++)
        {
            teammark[num].transform.position = A_POS[i].transform.position;
            num++;
        }
        for (int i = 0; i < SC.set3_C_team_B.Count; i++)
            {
                teammark[num].transform.position = B_POS[i].transform.position;
                num++;
            }
        for (int i = 0; i < SC.set3_C_team_C.Count; i++)
            {
                teammark[num].transform.position = C_POS[i].transform.position;
                num++;
            }
        for (int i = 0; i < SC.set3_C_team_D.Count; i++)
            {
                teammark[num].transform.position = D_POS[i].transform.position;
                num++;
            }
        //色の調整
        //N TEAMの排列
        num = 0;
        for (int j = 0; j < SC.set3_C_team_N.Count; j++)
        {
            if (SC.set3_C_team_N[j] == 0) //0:S職　青色
            {
                teammark[num].setColor(0.16f, 0.655f, 0.88f);
                teammark_jobnow[num] = 0;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_N.Count; j++)
        {
            if (SC.set3_C_team_N[j] == 1)   //1:P職　黃色
            {
                teammark[num].setColor(1.0f, 0.92f, 0.0016f);
                teammark_jobnow[num] = 1;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_N.Count; j++)
        {
            if (SC.set3_C_team_N[j] == 2)   //2:M職　紫色
            {
                teammark[num].setColor(0.808f, 0.459f, 0.678f);
                teammark_jobnow[num] = 2;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_N.Count; j++)
        {
            if (SC.set3_C_team_N[j] == 3)   //3:G職　褐色
            {
                teammark[num].setColor(0.835f, 0.522f, 0.094f);
                teammark_jobnow[num] = 3;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_N.Count; j++)
        {
            if (SC.set3_C_team_N[j] == 4)   //4:A職　深紅色
            {
                teammark[num].setColor(0.784f, 0.078f, 0.2f);
                teammark_jobnow[num] = 4;
                num++;
            }
        }
        for (int i = 0; i < num; i++) SC.set3_C_team_N[i] = teammark_jobnow[i];
        //A TEAMの排列
        int num_start = num;
        for (int j = 0; j < SC.set3_C_team_A.Count; j++)
        {
            if (SC.set3_C_team_A[j] == 0) //0:S職　青色
            {
                teammark[num].setColor(0.16f, 0.655f, 0.88f);
                teammark_jobnow[num] = 0;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_A.Count; j++)
        {
            if (SC.set3_C_team_A[j] == 1)   //1:P職　黃色
            {
                teammark[num].setColor(1.0f, 0.92f, 0.0016f);
                teammark_jobnow[num] = 1;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_A.Count; j++)
        {
            if (SC.set3_C_team_A[j] == 2)   //2:M職　紫色
            {
                teammark[num].setColor(0.808f, 0.459f, 0.678f);
                teammark_jobnow[num] = 2;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_A.Count; j++)
        {
            if (SC.set3_C_team_A[j] == 3)   //3:G職　褐色
            {
                teammark[num].setColor(0.835f, 0.522f, 0.094f);
                teammark_jobnow[num] = 3;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_A.Count; j++)
        {
            if (SC.set3_C_team_A[j] == 4)   //4:A職　深紅色
            {
                teammark[num].setColor(0.784f, 0.078f, 0.2f);
                teammark_jobnow[num] = 4;
                num++;
            }
        }
        int num_now = 0;
        for (int i = num_start; i < num; i++)
        {
            SC.set3_C_team_A[num_now] = teammark_jobnow[i];
            num_now++;
        }
        //B TEAMの排列
        num_start = num;
        for (int j = 0; j < SC.set3_C_team_B.Count; j++)
        {
            if (SC.set3_C_team_B[j] == 0) //0:S職　青色
            {
                teammark[num].setColor(0.16f, 0.655f, 0.88f);
                teammark_jobnow[num] = 0;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_B.Count; j++)
        {
            if (SC.set3_C_team_B[j] == 1)   //1:P職　黃色
            {
                teammark[num].setColor(1.0f, 0.92f, 0.0016f);
                teammark_jobnow[num] = 1;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_B.Count; j++)
        {
            if (SC.set3_C_team_B[j] == 2)   //2:M職　紫色
            {
                teammark[num].setColor(0.808f, 0.459f, 0.678f);
                teammark_jobnow[num] = 2;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_B.Count; j++)
        {
            if (SC.set3_C_team_B[j] == 3)   //3:G職　褐色
            {
                teammark[num].setColor(0.835f, 0.522f, 0.094f);
                teammark_jobnow[num] = 3;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_B.Count; j++)
        {
            if (SC.set3_C_team_B[j] == 4)   //4:A職　深紅色
            {
                teammark[num].setColor(0.784f, 0.078f, 0.2f);
                teammark_jobnow[num] = 4;
                num++;
            }
        }
        num_now = 0;
        for (int i = num_start; i < num; i++)
        {
            SC.set3_C_team_B[num_now] = teammark_jobnow[i];
            num_now++;
        }
        //C TEAMの排列
        num_start = num;
        for (int j = 0; j < SC.set3_C_team_C.Count; j++)
        {
            if (SC.set3_C_team_C[j] == 0) //0:S職　青色
            {
                teammark[num].setColor(0.16f, 0.655f, 0.88f);
                teammark_jobnow[num] = 0;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_C.Count; j++)
        {
            if (SC.set3_C_team_C[j] == 1)   //1:P職　黃色
            {
                teammark[num].setColor(1.0f, 0.92f, 0.0016f);
                teammark_jobnow[num] = 1;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_C.Count; j++)
        {
            if (SC.set3_C_team_C[j] == 2)   //2:M職　紫色
            {
                teammark[num].setColor(0.808f, 0.459f, 0.678f);
                teammark_jobnow[num] = 2;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_C.Count; j++)
        {
            if (SC.set3_C_team_C[j] == 3)   //3:G職　褐色
            {
                teammark[num].setColor(0.835f, 0.522f, 0.094f);
                teammark_jobnow[num] = 3;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_C.Count; j++)
        {
            if (SC.set3_C_team_C[j] == 4)   //4:A職　深紅色
            {
                teammark[num].setColor(0.784f, 0.078f, 0.2f);
                teammark_jobnow[num] = 4;
                num++;
            }
        }
        num_now = 0;
        for (int i = num_start; i < num; i++)
        {
            SC.set3_C_team_C[num_now] = teammark_jobnow[i];
            num_now++;
        }
        //D TEAMの排列
        num_start = num;
        for (int j = 0; j < SC.set3_C_team_D.Count; j++)
        {
            if (SC.set3_C_team_D[j] == 0) //0:S職　青色
            {
                teammark[num].setColor(0.16f, 0.655f, 0.88f);
                teammark_jobnow[num] = 0;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_D.Count; j++)
        {
            if (SC.set3_C_team_D[j] == 1)   //1:P職　黃色
            {
                teammark[num].setColor(1.0f, 0.92f, 0.0016f);
                teammark_jobnow[num] = 1;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_D.Count; j++)
        {
            if (SC.set3_C_team_D[j] == 2)   //2:M職　紫色
            {
                teammark[num].setColor(0.808f, 0.459f, 0.678f);
                teammark_jobnow[num] = 2;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_D.Count; j++)
        {
            if (SC.set3_C_team_D[j] == 3)   //3:G職　褐色
            {
                teammark[num].setColor(0.835f, 0.522f, 0.094f);
                teammark_jobnow[num] = 3;
                num++;
            }
        }
        for (int j = 0; j < SC.set3_C_team_D.Count; j++)
        {
            if (SC.set3_C_team_D[j] == 4)   //4:A職　深紅色
            {
                teammark[num].setColor(0.784f, 0.078f, 0.2f);
                teammark_jobnow[num] = 4;
                num++;
            }
        }
        num_now = 0;
        for (int i = num_start; i < num; i++)
        {
            SC.set3_C_team_D[num_now] = teammark_jobnow[i];
            num_now++;
        }

        Sorting_check = false;
        
        
    }
    public void Average_mark()
    {
        NoTeam_mark();
        if(SC.GI.Gamemode_choice!=1)
        {
            while (SC.set3_C_team_N.Count >= 4)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_B.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_C.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_D.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
            if (SC.set3_C_team_N.Count == 3)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_B.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_C.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
            else if (SC.set3_C_team_N.Count == 2)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_B.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
            else if (SC.set3_C_team_N.Count == 1)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
        }
        else
        {
            while (SC.set3_C_team_N.Count >= 2)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
                SC.set3_C_team_B.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
            if (SC.set3_C_team_N.Count == 1)
            {
                SC.set3_C_team_A.Add(SC.set3_C_team_N[SC.set3_C_team_N.Count - 1]);
                SC.set3_C_team_N.RemoveAt(SC.set3_C_team_N.Count - 1);
            }
        }
        
        Sorting_mark();
    }
    public void NoTeam_mark()
    {
        for (int i = 0; i < SC.set3_C_team_A.Count; i++)
        {
            SC.set3_C_team_N.Add(SC.set3_C_team_A[i]);
        }
        for (int i = 0; i < SC.set3_C_team_B.Count; i++)
        {
            SC.set3_C_team_N.Add(SC.set3_C_team_B[i]);
        }
        for (int i = 0; i < SC.set3_C_team_C.Count; i++)
        {
            SC.set3_C_team_N.Add(SC.set3_C_team_C[i]);
        }
        for (int i = 0; i < SC.set3_C_team_D.Count; i++)
        {
            SC.set3_C_team_N.Add(SC.set3_C_team_D[i]);
        }
        SC.set3_C_team_A.Clear();
        SC.set3_C_team_B.Clear();
        SC.set3_C_team_C.Clear();
        SC.set3_C_team_D.Clear();
        Sorting_mark();
    }
}
