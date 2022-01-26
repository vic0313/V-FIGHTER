using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AllSetting_Controller : MonoBehaviour
{
    public GameInfo GI;
    public GameObject SceneBack1;
    private int chiocenow;
    private bool chioceOK;
    private bool scenechange;
    private int itemchioce_now;
    private bool itemchioce_check;
    private bool changeok;
    public CanvasGroup[] manu; //0 Item 1Boton 2Stage 3Back
    public Transform manu_scale;
    public Transform manu_scale_change;
    public CanvasGroup[] manu_text;
    public Image[] manu_chioce;
    public Transform[] manu_pos;
    public GameObject[] SetManu;
    public Text[] ItemProbability;
    public GameObject ItemChioce;
    public Transform[] ItemChioce_pos;
    public GameObject ItempUpDown;
    public Image[] ItempUpDown_Image;
    public Image ItemBack;
    public Image ItemOther;
    private int botoncheck;
    private int ItemOther_chiocenow;
    public GameObject ItemOther_obj;
    public AudioClip[] effect;
    public AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
        chiocenow = 0;
        itemchioce_now = 0;
        botoncheck = 0;
        chioceOK = false;
        changeok = false;
        scenechange = false;
        itemchioce_check = false;
        ItemOther_chiocenow = 0;
        for (int i=0;i< manu.Length;i++)
        {
            manu_pos[i].transform.position=manu[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(scenechange==false)
        {
            if (chioceOK == false)
            {
                for (int i = 0; i < SetManu.Length; i++) SetManu[i].SetActive(false);
                 itemchioce_now = 0;
                itemchioce_check = false;
                if (changeok)
                {
                    for (int i = 0; i < manu.Length; i++)
                    {
                        manu_chioce[i].enabled = false;
                        if (i == chiocenow)
                        {
                            manu[i].transform.position += (manu_pos[i].position - manu_pos[4].position) * Time.deltaTime;
                            manu[i].transform.localScale += (manu_scale.transform.localScale-manu_scale_change.transform.localScale) * Time.deltaTime;
                            manu_text[i].alpha += 1 * Time.deltaTime;
                            if (manu_text[i].alpha >= 1) manu_text[i].alpha = 1;
                            if ((manu_pos[i].position - manu[i].transform.position).magnitude <= 5)
                            {
                                manu[i].transform.position = manu_pos[i].position;
                                manu[i].transform.localScale = manu_scale.transform.localScale;
                                changeok = false;
                            }
                        }
                        else
                        {
                            manu[i].alpha += 0.5f * Time.deltaTime;
                            if (manu[i].alpha >= 1) manu[i].alpha = 1;
                        }
                    }
                }
                else
                {
                    //chioceUI
                    for (int i = 0; i < manu.Length; i++)
                    {
                        manu_chioce[i].enabled = true;
                        manu[i].alpha = 1;
                        if (i == chiocenow) manu_chioce[i].enabled = true;
                        else manu_chioce[i].enabled = false;
                    }
                    if ((GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f) ||Input.GetButtonDown("Vertical player1") || Input.GetButtonDown("Vertical player2") || Input.GetButtonDown("Vertical player3") || Input.GetButtonDown("Vertical player4"))
                    {
                        if(GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f)
                        {
                            //上
                            if (GI.input_v_All_Joy > 0.3f)
                            {
                                chiocenow = 0;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            //下
                            else if (GI.input_v_All_Joy < (-0.3f))
                            {
                                chiocenow = 3;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            GI.InputJoycheck_all[0] = false;
                            GI.input_v_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }
                        else
                        {
                            //上
                            if (GI.input_v_All > 0) chiocenow = 0;
                            //下
                            else if (GI.input_v_All < 0) chiocenow = 3;
                            audiosource.PlayOneShot(effect[0]);
                        }
                    }
                    else if ((GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f) ||Input.GetButtonDown("Horizontal player1") || Input.GetButtonDown("Horizontal player2") || Input.GetButtonDown("Horizontal player3") || Input.GetButtonDown("Horizontal player4"))
                    {
                        if(GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f)
                        {
                            //右
                            if (GI.input_h_All_Joy > 0.3f)
                            {
                                chiocenow = 1;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            //左
                            else if (GI.input_h_All_Joy < (-0.3f))
                            {
                                chiocenow = 2;
                                audiosource.PlayOneShot(effect[0]);
                            }
                            GI.InputJoycheck_all[1] = false;
                            GI.input_h_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }else
                        {
                            //右
                            if (GI.input_h_All > 0) chiocenow = 1;
                            //左
                            else if (GI.input_h_All < 0) chiocenow = 2;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        
                    }
                    if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                    {
                        if (chiocenow == 3)
                        {
                            SceneBack1.SetActive(true);
                            scenechange = true;
                            audiosource.PlayOneShot(effect[2]);
                        }
                        else
                        {
                            chioceOK = true;
                            audiosource.PlayOneShot(effect[1]);
                        }

                    }
                    else if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (chiocenow != 3)
                        {
                            chiocenow = 3;
                        }
                        audiosource.PlayOneShot(effect[3]);
                    }
                }

            }
            else
            {
                itemchioce_now = GI.MAXandMin(itemchioce_now, 0, 13);
                ItemOther_chiocenow = GI.MAXandMin(ItemOther_chiocenow, 0, 2);
                GI.ItemMax= GI.MAXandMin(GI.ItemMax, 0, 2);
                GI.Item_bron_time = GI.MAXandMin(GI.Item_bron_time, 0, 2);
                GI.Item_born_OneTime = GI.MAXandMin(GI.Item_born_OneTime, 0, 2);
                for (int i = 0; i < manu.Length; i++)
                {
                    manu_chioce[i].enabled = false;
                    if (changeok == false)
                    {
                        if (i == chiocenow)
                        {
                            manu[i].transform.position += (manu_pos[4].transform.position - manu_pos[i].position) * Time.deltaTime;
                            manu[i].transform.localScale += (manu_scale_change.transform.localScale - manu_scale.transform.localScale) * Time.deltaTime;
                            manu_text[i].alpha -= 1 * Time.deltaTime;
                            if (manu_text[i].alpha <= 0) manu_text[i].alpha = 0;

                            if ((manu[i].transform.position - manu_pos[4].transform.position).magnitude <= 5)
                            {
                                SetManu[i].SetActive(true);
                                manu[i].transform.position = manu_pos[4].transform.position;
                                manu[i].transform.localScale = manu_scale_change.transform.localScale;
                                manu_text[i].alpha = 0;
                                changeok = true;
                            }
                        }
                        else
                        {
                            manu[i].alpha -= 1.5f * Time.deltaTime;
                            if (manu[i].alpha <= 0) manu[i].alpha = 0;
                        }
                    }
                    else
                    {
                         if (i == chiocenow) manu[i].alpha = 1;
                         else manu[i].alpha = 0;
                    }
                }
                
                if (chiocenow != 0)
                {
                    if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4")
                        || Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        chioceOK = false;
                        audiosource.PlayOneShot(effect[3]);
                    }
                }
                else
                {
                    int Sum = 0;
                    for (int i = 0; i < (ItemProbability.Length-3); i++)
                    {
                        ItemProbability[i].text = "" + GI.item_Probability[i];
                        Sum+= GI.item_Probability[i];
                    }
                    if (itemchioce_now <= 11)
                    {
                        ItemChioce.SetActive(true);
                        ItemChioce.transform.position = ItemChioce_pos[itemchioce_now].position;
                    }else ItemChioce.SetActive(false);
                    if (itemchioce_now == 12)
                    {
                        ItemOther.enabled = true;
                        ItemBack.enabled = false;
                        botoncheck = 0;
                    }
                    else if (itemchioce_now == 13)
                    {
                        ItemOther.enabled = false;
                        ItemBack.enabled = true;
                        botoncheck = 0;
                    }
                    else
                    {
                        ItemOther.enabled = false;
                        ItemBack.enabled = false;
                    }
                    if (itemchioce_check == false)ItempUpDown.SetActive(false);
                    else
                    {
                        ItempUpDown.SetActive(true);
                        if (itemchioce_now<=11)
                        {
                            ItempUpDown.transform.position = ItemProbability[itemchioce_now].transform.position;
                            if (GI.item_Probability[itemchioce_now] >= 100) ItempUpDown_Image[1].enabled = false;
                            else ItempUpDown_Image[1].enabled = true;
                            if (GI.item_Probability[itemchioce_now] <= 0) ItempUpDown_Image[0].enabled = false;
                            else ItempUpDown_Image[0].enabled = true;
                            if (Sum <= 1) ItempUpDown_Image[0].enabled = false;
                        }else if (itemchioce_now == 12)
                        {
                            ItempUpDown.transform.position = ItemProbability[12+ ItemOther_chiocenow].transform.position;
                            if(ItemOther_chiocenow == 0)
                            {
                                if (GI.Item_bron_time >= 2 ) ItempUpDown_Image[1].enabled = false;
                                else ItempUpDown_Image[1].enabled = true;
                                if (GI.Item_bron_time <= 0 ) ItempUpDown_Image[0].enabled = false;
                                else ItempUpDown_Image[0].enabled = true;
                            }else if (ItemOther_chiocenow == 1)
                            {
                                if (GI.Item_born_OneTime >= 2) ItempUpDown_Image[1].enabled = false;
                                else ItempUpDown_Image[1].enabled = true;
                                if (GI.Item_born_OneTime <= 0) ItempUpDown_Image[0].enabled = false;
                                else ItempUpDown_Image[0].enabled = true;
                            }
                            else if (ItemOther_chiocenow == 2)
                            {
                                if (GI.ItemMax >= 2) ItempUpDown_Image[1].enabled = false;
                                else ItempUpDown_Image[1].enabled = true;
                                if (GI.ItemMax <= 0) ItempUpDown_Image[0].enabled = false;
                                else ItempUpDown_Image[0].enabled = true;
                            }
                        }
                    }
                    if (itemchioce_check && itemchioce_now == 12)
                    {
                        ItemOther_obj.SetActive(true);
                        switch (GI.Item_bron_time)
                        {
                            case 0:
                                ItemProbability[12].text ="5秒";
                                break;
                            case 1:
                                ItemProbability[12].text = "10秒";
                                break;
                            case 2:
                                ItemProbability[12].text = "15秒";
                                break;
                        }
                        
                        switch (GI.Item_born_OneTime)
                        {
                            case 0:
                                ItemProbability[13].text = "1つ";
                                break;
                            case 1:
                                ItemProbability[13].text = "3つ";
                                break;
                            case 2:
                                ItemProbability[13].text = "5つ";
                                break;
                        }
                        switch (GI.ItemMax)
                        {
                            case 0:
                                ItemProbability[14].text = "6つ";
                                break;
                            case 1:
                                ItemProbability[14].text = "8つ";
                                break;
                            case 2:
                                ItemProbability[14].text = "10個";
                                break;
                        }
                    }
                    else ItemOther_obj.SetActive(false);
                    if (itemchioce_check)
                    {
                        if (Input.GetButton("Horizontal player1") || Input.GetButton("Horizontal player2") || Input.GetButton("Horizontal player3") || Input.GetButton("Horizontal player4"))
                        {
                            if (itemchioce_now <= 11 && botoncheck == 0)
                            {
                                if (GI.input_h_All > 0.3f)
                                {
                                    botoncheck = 1;
                                    InvokeRepeating("ProbabilityUp", 1, 0.02f);
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                else if (GI.input_h_All < (-0.3f) && Sum > 1)
                                {
                                    botoncheck = 2;
                                    InvokeRepeating("ProbabilityDown", 1, 0.02f);
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                        }
                        if (Input.GetButtonUp("Horizontal player1") || Input.GetButtonUp("Horizontal player2") || Input.GetButtonUp("Horizontal player3") || Input.GetButtonUp("Horizontal player4"))
                        {
                            if (itemchioce_now <= 11)
                            {
                                if (botoncheck != 0)
                                {
                                    if (botoncheck == 1) CancelInvoke("ProbabilityUp");
                                    else if (botoncheck == 2) CancelInvoke("ProbabilityDown");
                                    botoncheck = 0;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                        }
                        if(Sum <= 1&& botoncheck == 2)
                        {
                            CancelInvoke("ProbabilityDown");
                            botoncheck = 0;
                        }
                    }
                    else
                    {
                        ItemOther_chiocenow = 0;
                        if (botoncheck == 1) CancelInvoke("ProbabilityUp");
                        else if (botoncheck == 2) CancelInvoke("ProbabilityDown");
                        botoncheck = 0;
                    }
                    if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
                    {
                        if (itemchioce_now <= 12)
                        {
                            itemchioce_check = !itemchioce_check;
                            audiosource.PlayOneShot(effect[4]);
                        }
                        else
                        {
                            chioceOK = false;
                            audiosource.PlayOneShot(effect[3]);
                        }
                        
                    }
                    if (Input.GetButtonDown("Attack2 player1") || Input.GetButtonDown("Attack2 player2") || Input.GetButtonDown("Attack2 player3") || Input.GetButtonDown("Attack2 player4") || Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (itemchioce_check == true)
                        {
                            if (itemchioce_now <= 12)
                            {
                                itemchioce_check = false;
                                audiosource.PlayOneShot(effect[3]);
                            }
                        }
                        else
                        {
                            if (itemchioce_now < 13) itemchioce_now = 13;
                            else chioceOK = false;
                            audiosource.PlayOneShot(effect[3]);
                        }
                    }
                    //Item設定
                    if ((GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f) || Input.GetButtonDown("Vertical player1") || Input.GetButtonDown("Vertical player2") || Input.GetButtonDown("Vertical player3") || Input.GetButtonDown("Vertical player4"))
                    {
                        if(GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.95f)
                        {
                            if (itemchioce_check == false)
                            {
                                //上
                                if (GI.input_v_All_Joy > 0.3f)
                                {
                                    itemchioce_now--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                //下
                                else if (GI.input_v_All_Joy < (-0.3f))
                                {
                                    itemchioce_now++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            else if (itemchioce_now == 12 && itemchioce_check)
                            {
                                //上
                                if (GI.input_v_All_Joy > 0.3f)
                                {
                                    ItemOther_chiocenow--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                //下
                                else if (GI.input_v_All_Joy < (-0.3f))
                                {
                                    ItemOther_chiocenow++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            GI.InputJoycheck_all[0] = false;
                            GI.input_v_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }else
                        {
                            if (itemchioce_check == false)
                            {
                                //上
                                if (GI.input_v_All > 0)
                                {
                                    itemchioce_now--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                //下
                                else if (GI.input_v_All < 0)
                                {
                                    itemchioce_now++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                            else if (itemchioce_now == 12 && itemchioce_check)
                            {
                                //上
                                if (GI.input_v_All > 0)
                                {
                                    ItemOther_chiocenow--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                //下
                                else if (GI.input_v_All < 0)
                                {
                                    ItemOther_chiocenow++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                            }
                        }
                        
                    }
                    else if ((GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f) || Input.GetButtonDown("Horizontal player1") || Input.GetButtonDown("Horizontal player2") || Input.GetButtonDown("Horizontal player3") || Input.GetButtonDown("Horizontal player4"))
                    {
                        if(GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.95f)
                        {
                            if (itemchioce_now <= 11 && itemchioce_check)
                            {
                                //右
                                if (GI.input_h_All_Joy > 0.3f)
                                {
                                    GI.item_Probability[itemchioce_now]++;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                //左
                                else if (GI.input_h_All_Joy < (-0.3f) && Sum > 1)
                                {
                                    GI.item_Probability[itemchioce_now]--;
                                    audiosource.PlayOneShot(effect[0]);
                                }
                                GI.item_Probability[itemchioce_now] = GI.MAXandMin(GI.item_Probability[itemchioce_now], 0, 100);
                            }
                            else if (itemchioce_now == 12 && itemchioce_check)
                            {
                                if (ItemOther_chiocenow == 0)
                                {
                                    if (GI.input_h_All_Joy > 0.3f)
                                    {
                                        GI.Item_bron_time++;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.3f))
                                    {
                                        GI.Item_bron_time--;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                }
                                else if (ItemOther_chiocenow == 1)
                                {
                                    if (GI.input_h_All_Joy > 0.3f)
                                    {
                                        GI.Item_born_OneTime++;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.3f))
                                    {
                                        GI.Item_born_OneTime--;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                }
                                else if (ItemOther_chiocenow == 2)
                                {
                                    if (GI.input_h_All_Joy > 0.3f)
                                    {
                                        GI.ItemMax++;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                    else if (GI.input_h_All_Joy < (-0.3f))
                                    {
                                        GI.ItemMax--;
                                        audiosource.PlayOneShot(effect[0]);
                                    }
                                }
                            }
                            GI.InputJoycheck_all[1] = false;
                            GI.input_h_All_Joy = 0;
                            GI.JoycheckStart = true;
                            GI.InvokeJoyCheck();
                        }
                        else
                        {
                            if (itemchioce_now <= 11 && itemchioce_check)
                            {
                                //右
                                if (GI.input_h_All > 0)
                                {
                                    GI.item_Probability[itemchioce_now]++;
                                }
                                //左
                                else if (GI.input_h_All < 0 && Sum > 1)
                                {
                                    GI.item_Probability[itemchioce_now]--;
                                }
                                GI.item_Probability[itemchioce_now] = GI.MAXandMin(GI.item_Probability[itemchioce_now], 0, 100);
                            }
                            else if (itemchioce_now == 12 && itemchioce_check)
                            {
                                if (ItemOther_chiocenow == 0)
                                {
                                    if (GI.input_h_All > 0)
                                    {
                                        GI.Item_bron_time++;
                                    }
                                    else if (GI.input_h_All < 0)
                                    {
                                        GI.Item_bron_time--;
                                    }
                                }
                                else if (ItemOther_chiocenow == 1)
                                {
                                    if (GI.input_h_All > 0)
                                    {
                                        GI.Item_born_OneTime++;
                                    }
                                    else if (GI.input_h_All < 0)
                                    {
                                        GI.Item_born_OneTime--;
                                    }
                                }
                                else if (ItemOther_chiocenow == 2)
                                {
                                    if (GI.input_h_All > 0)
                                    {
                                        GI.ItemMax++;
                                    }
                                    else if (GI.input_h_All < 0)
                                    {
                                        GI.ItemMax--;
                                    }
                                }
                            }
                            audiosource.PlayOneShot(effect[0]);
                        }
                        
                    }
                }
            }
        }
        
    }
    void ProbabilityUp()
    {
        if(GI.item_Probability[itemchioce_now]<100) GI.item_Probability[itemchioce_now]++;
    }
    void ProbabilityDown()
    {
        if (GI.item_Probability[itemchioce_now] > 0) GI.item_Probability[itemchioce_now]--;
    }
}
