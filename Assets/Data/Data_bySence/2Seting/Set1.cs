using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Set1 : MonoBehaviour
{
    public SetControler SC;
    public Image[] ig;
    public Text[] tt;
    public Transform[] pos;
    
    // Update is called once per frame
    void Update()
    {
        if (SC.GI.setingstate == 2)
        {
            if  (SC.GI.chioce_now >= 0 && SC.GI.chioce_now <= 3)
            {
                ig[6].transform.position = pos[SC.GI.chioce_now].transform.position;
                tt[0].text = "" + SC.GI.player_num;
                tt[1].text = "" + SC.GI.cpu_num;
                switch(SC.GI.Gamemode_choice)
                {
                    case 0://Normal
                        switch (SC.GI.game_mode_chioce)
                        {
                            case 0:
                                tt[2].text = "Normal";
                                tt[3].text = "PS:99撃破の時強制終了！";
                                break;
                            case 1:
                                tt[2].text = "Zombie";
                                tt[3].text = "PS:倒したらHP減らす状況で復活！エリア中残っている敵はないならば強制終了！";
                                break;
                        }
                        break;
                    case 1://BB
                        switch (SC.GI.game_mode_chioce)
                        {
                            case 0:
                                tt[2].text = "20点勝利";
                                break;
                            case 1:
                                tt[2].text = "40点勝利";
                                break;
                            case 2:
                                tt[2].text = "無限";
                                break;
                        }
                        tt[3].text = "PS:敵が全滅or99得点の時強制終了！";
                        break;
                    case 2://Jump
                        switch (SC.GI.game_mode_chioce)
                        {
                            case 0:
                                tt[2].text = "20点勝利";
                                break;
                            case 1:
                                tt[2].text = "40点勝利";
                                break;
                            case 2:
                                tt[2].text = "無限";
                                break;
                        }
                        tt[3].text = "PS:敵が全滅or99得点の時強制終了！";
                        break;
                }
                

            }

        }
        
    }
}
