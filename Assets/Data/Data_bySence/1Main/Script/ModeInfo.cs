using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeInfo : MonoBehaviour
{
    public GameInfo GI;
    public Text[] modeinfo;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void Update()
    {
        switch(GI.Gamemode_choice)
        {
            case 0://Normal
                modeinfo[0].text = "V Battle";
                modeinfo[1].text = "敵を倒して、最大値の撃破数を目指そう！";
                break;
            case 1://BB
                modeinfo[0].text = "BouncyBall Battle";
                modeinfo[1].text = "ボールを相手のゴールにシュゥゥゥーッ！！";
                break;
            case 2://j
                modeinfo[0].text = "Jump Battle";
                modeinfo[1].text = "敵の身体を踏んで、点数を手に入れろ！";
                break;
            case 3://AllSeting
                modeinfo[0].text = "Game Setting";
                modeinfo[1].text = "ゲーム設定と説明";
                break;
        }

    }
}
