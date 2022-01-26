using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControler : MonoBehaviour
{
    public GameControler gc;
    public List<GameObject> ITEM=new List<GameObject>();        //現在のアイテムのLIST
    public List<Transform> ITEM_rbpos=new List<Transform>();    //再生位置
    public GameObject[] Prefeb;                                 //アイテムの物件保存
    public Transform[] reboronpos;                              //再生点
    public GameObject[] EffectPrefeb;                           //アイテム効果の物件
    public int ITEM_MAX;                                        //ゲーム中に存在できるアイテムの最大値
    public int ITEM_born_OneTime;                               //一回再生のアイテム数量
    public float ITEM_born_time;                                //アイテム再生の時間
    private bool born;                                          //アイテム再生開始
    private int item_Probability_MIX;                           //アイテムの機率の総合
    private GameObject BBobj;
    public GameObject BBobj_prefab;
    //アイテム9の数量制御===================================================================================================
    private int[] item9_pos;                                    //再生位置のITEM9の数量
    private List<bool> ITEM_use= new List<bool>();              //アイテムの使用
    private List<int> ITEM_posnum = new List<int>();            //アイテムの生成位置　(番号)
    private List<int> ITEM_num = new List<int>();               //アイテムの種類　(番号)
    
    // Start is called before the first frame update
    void Start()
    {
        //ステージを関わり再生のPOSをLIST中に配置
        foreach (var i in reboronpos)
        {
            ITEM_rbpos.Add(i);
        }
        born = false;
        item_Probability_MIX = 0;
        item9_pos = new int[ITEM_rbpos.Count];
        for(int i=0;i< ITEM_rbpos.Count;i++)
        {
            item9_pos[i] =0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gc.Game_Itemuse==0)
        {
            if (gc.Gamestate >= 2)
            {
                if (born == false)
                {
                    item_Probability_MIX = 0;
                    for (int i = 0; i < gc.item_Probability.Length; i++)
                    {
                        item_Probability_MIX += gc.item_Probability[i];
                    }
                    switch(gc.GI.Item_bron_time)
                    {
                        case 0:
                            ITEM_born_time = 5;
                            break;
                        case 1:
                            ITEM_born_time = 10;
                            break;
                        case 2:
                            ITEM_born_time = 15;
                            break;
                    }
                    switch (gc.GI.Item_born_OneTime)
                    {
                        case 0:
                            ITEM_born_OneTime = 1;
                            break;
                        case 1:
                            ITEM_born_OneTime = 3;
                            break;
                        case 2:
                            ITEM_born_OneTime = 5;
                            break;
                    }
                    switch (gc.GI.ItemMax)
                    {
                        case 0:
                            ITEM_MAX = 6;
                            break;
                        case 1:
                            ITEM_MAX = 8;
                            break;
                        case 2:
                            ITEM_MAX = 10;
                            break;
                    }
                    InvokeRepeating("ITEM_born", ITEM_born_time, ITEM_born_time);
                    if(gc.Gamemode_num==1) InvokeRepeating("BB_born", 1, 3);
                    born = true;
                }
            }
            else if (gc.Gamestate == 4)
            {
                CancelInvoke("ITEM_born");
                if (gc.Gamemode_num == 1) CancelInvoke("BB_born");
                born = false;
            }
        }
    }

    void ITEM_born()
    {
        //nullの物件を消す
        if (ITEM.Count >= ITEM_MAX)
        {
            for (int i = 0; i < ITEM.Count; i++)
            {
                if (ITEM[i] == null)
                {
                    if(ITEM_use[i]==true&& ITEM_num[i]==9)
                    {
                        item9_pos[ITEM_posnum[i]]--;
                    }
                    ITEM_use.RemoveAt(i);
                    ITEM_posnum.RemoveAt(i);
                    ITEM_num.RemoveAt(i);
                    ITEM.RemoveAt(i);  
                }
            }
        }
        //現在いるのアイテム数量はMAX以下でしたら、再生する
        if (ITEM.Count<ITEM_MAX)
        {
            if((ITEM.Count+ITEM_born_OneTime)<=ITEM_MAX)
            {
                int num = 0;                                        //計算用変数
                int[] Item_numI_pos = new int[ITEM_born_OneTime];   //一回再生の数量によって再生位置の配列を作る
                for (int i = 0; i < ITEM_born_OneTime; i++)
                {
                    //再生の位置を決定
                    num = Random.Range(0, (ITEM_rbpos.Count));
                    if (gc.Gamemode_num == 1)
                    {
                        if (num >= 8 && num <= 11) num = 12;
                        else if (num >= 20 && num <= 23) num = 24;
                    }
                    if (i>0)
                    {
                        //今回使用してない生成位置を探す
                        while (find(num, Item_numI_pos) == true)
                        {
                            num++;
                            if (gc.Gamemode_num == 1)
                            {
                                if (num >= 8 && num <= 11) num = 12;
                                else if (num >= 20 && num <= 23) num = 24;
                            }
                            if (num > (ITEM_rbpos.Count - 1))
                            {
                                num = 0;
                            }
                        }
                    }
                    //今回の使用位置を配列中に配置
                    Item_numI_pos[i] = num;
                    //ランダム機率でアイテムを再生
                    int Probability = Random.Range(0, (item_Probability_MIX));
                    
                    for(int j=0;j< gc.item_Probability.Length;j++)
                    {
                        if(Probability < gc.item_Probability[j])
                        {
                            //もしこのアイテムは9ならば、この再生位置のアイテム9を判断して、もし3個を超えたら、他の位置を変更
                            if(j==9)
                            {
                                if(item9_pos[num] <3)
                                {
                                    item9_pos[num]++;
                                }
                                else
                                {
                                    while(item9_pos[num] >=3)
                                    {
                                        num++;
                                    }
                                    Item_numI_pos[i] = num;
                                }
                            }
                            ITEM.Add(Instantiate(Prefeb[j], ITEM_rbpos[num].position, Quaternion.identity));
                            ITEM[(ITEM.Count-1)].GetComponent<ITEM_Type>().GC = gc;
                            ITEM_use.Add(true);
                            ITEM_posnum.Add(num);
                            ITEM_num.Add(j);
                            break;
                        }
                        else
                        {
                            Probability -= gc.item_Probability[j];
                        }
                    }
                }
            }
            else
            {
                int num = 0;
                int[] Item_numI_pos = new int[(ITEM_MAX- ITEM.Count)];
                for (int i = 0; i < (ITEM_MAX - ITEM.Count); i++)
                {
                    num = Random.Range(0, (ITEM_rbpos.Count));
                    if(gc.Gamemode_num==1)
                    {
                        if (num >= 8 && num <= 11) num = 12;
                        else if (num >= 20 && num <= 23) num = 24;
                    }
                    if (i > 0)
                    {
                        while (find(num, Item_numI_pos) == true)
                        {
                            num++;
                            if (gc.Gamemode_num == 1)
                            {
                                if (num >= 8 && num <= 11) num = 12;
                                else if (num >= 20 && num <= 23) num = 24;
                            }
                            if (num > (ITEM_rbpos.Count - 1))
                            {
                                num = 0;
                            }
                        }
                    }
                    Item_numI_pos[i] = num;
                    int Probability = Random.Range(0, (item_Probability_MIX));
                    for (int j = 0; j < gc.item_Probability.Length; j++)
                    {
                        if (Probability < gc.item_Probability[j])
                        {
                            if (j == 9)
                            {
                                if (item9_pos[num] < 3)
                                {
                                    item9_pos[num]++;
                                }
                                else
                                {
                                    while (item9_pos[num] >= 3)
                                    {
                                        num++;
                                    }
                                    Item_numI_pos[i] = num;
                                }
                            }
                            ITEM.Add(Instantiate(Prefeb[j], ITEM_rbpos[num].position, Quaternion.identity));
                            ITEM[(ITEM.Count - 1)].GetComponent<ITEM_Type>().GC = gc;
                            ITEM_use.Add(true);
                            ITEM_posnum.Add(num);
                            ITEM_num.Add(j);
                            break;
                        }
                        else
                        {
                            Probability -= gc.item_Probability[j];
                        }
                    }
                }
            }
        }
    }
    void BB_born()
    {
        if(BBobj==null)
        {
            BBobj = Instantiate(BBobj_prefab, new Vector3(0,15,0), Quaternion.identity);
            BBobj.GetComponent<ITEM_Type>().GC = gc;
            gc.BB_Ball = BBobj;
        }
    }
    private bool find(int a,int[] c)
    {
        foreach(var b in c)
        {
            if(a==b)
            {
                return true;
            }
        }    
        return false;
    }
}
