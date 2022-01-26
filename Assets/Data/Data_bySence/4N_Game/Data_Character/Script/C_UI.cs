using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_UI : MonoBehaviour
{
    public  GameObject Character;
    private CharacterData Data;
    public  Camera camera_A;
    public Image HPUI, MPUI, HPUI_bg, MPUI_bg;
    private float M_Width, Height;
    public Image Playermark;
    public Image Teammark;
    private bool playercheck;
    private bool teamcheck;
    public Image lifemark1, lifemark2;
    public Text RebonTime;
    // Start is called before the first frame update
    void Start()
    {
        Data = Character.GetComponent<CharacterData>();
        M_Width = 80;
        Height = 5;
        HPUI_bg.rectTransform.sizeDelta = new Vector2(M_Width, Height);
        MPUI_bg.rectTransform.sizeDelta = HPUI_bg.rectTransform.sizeDelta;

        Playermark.rectTransform.sizeDelta = new Vector2(20, 20);
        Teammark.rectTransform.sizeDelta = new Vector2(70, 12);
        if(Character.tag=="Player1"|| Character.tag == "Player2" || Character.tag == "Player3" || Character.tag == "Player4")
        {
            playercheck = true;
            Playermark.enabled = true;
        }
        else Playermark.enabled = false;
        if(Data.characterTeam!=0)
        {
            teamcheck = true;
            Teammark.enabled = true;
            switch(Data.characterTeam)
            {
                case 1:
                    Teammark.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                    break;
                case 2:
                    Teammark.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
                    break;
                case 3:
                    Teammark.color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
                    break;
                case 4:
                    Teammark.color = new Color(1.0f, 0.92f, 0.0016f, 0.5f);
                    break;
            }
        }
        else Teammark.enabled = false;

        if (Data.character_NOW > 2) lifemark2.enabled = true;
        else lifemark2.enabled = false;
        if (Data.character_NOW>1) lifemark1.enabled = true;
        else lifemark1.enabled = false;

        RebonTime.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HPMP_UI();
        
    }

    void HPMP_UI()
    {
        if(Data.DeadStep>0 ||Data.GC.Gamestate>=4)
        {
            Color cr = HPUI.color;
            cr.a -= 2f * Time.deltaTime;
            HPUI.color = cr;
            cr = MPUI.color;
            cr.a -= 2f* Time.deltaTime;
            MPUI.color = cr;
            cr = HPUI_bg.color;
            cr.a -= 2f* Time.deltaTime;
            HPUI_bg.color = cr;
            cr = MPUI_bg.color;
            cr.a -= 2f* Time.deltaTime;
            MPUI_bg.color = cr;
            cr = Playermark.color;
            cr.a -= 2f* Time.deltaTime;
            Playermark.color = cr;
            cr = lifemark1.color;
            cr.a -= 2f* Time.deltaTime;
            lifemark1.color = cr;
            cr = lifemark2.color;
            cr.a -= 2f* Time.deltaTime;
            lifemark2.color = cr;
            if (Data.characterTeam != 0) Teammark.enabled = false;


        }
        else if (Data.DeadStep == 0 && Data.GC.Gamestate < 4)
        {
            Color cr = HPUI.color;
            cr.a = 1.0f;
            HPUI.color = cr;
            cr = MPUI.color;
            cr.a = 1.0f;
            MPUI.color = cr;
            cr = HPUI_bg.color;
            cr.a = 1.0f;
            HPUI_bg.color = cr;
            cr = MPUI_bg.color;
            cr.a = 1.0f;
            MPUI_bg.color = cr;
            cr = Playermark.color;
            cr.a = 1.0f;
            Playermark.color = cr;
            cr = lifemark1.color;
            cr.a = 1.0f;
            lifemark1.color = cr;
            cr = lifemark2.color;
            cr.a = 1.0f;
            lifemark2.color = cr;
            if (Data.characterTeam != 0) Teammark.enabled = true;
        }

        Vector3 pos = Character.transform.position;
        pos.y += 1.3f;
        //位置更新
        Vector2 player = camera_A.WorldToScreenPoint(pos);
        HPUI_bg.rectTransform.position = player;
        player.x -= (M_Width * ((Data.character_MaxHP - Data.character_NowHP) / Data.character_MaxHP) / 2);
        HPUI.rectTransform.position = player;
        player = camera_A.WorldToScreenPoint(pos);
        player.y -= 5;
        MPUI_bg.rectTransform.position = player;
        player.x -= (M_Width * ((Data.character_MaxMP - Data.character_NowMP) / Data.character_MaxMP) / 2);
        MPUI.rectTransform.position = player;
        //長さ更新
        HPUI.rectTransform.sizeDelta = new Vector2(M_Width * (Data.character_NowHP / Data.character_MaxHP), Height);
        MPUI.rectTransform.sizeDelta = new Vector2(M_Width * (Data.character_NowMP / Data.character_MaxMP), Height);

        if(playercheck)
        {
            //プレイヤMARK
            player = camera_A.WorldToScreenPoint(pos);
            player.x -= 31.5f;
            player.y += 8;
            Playermark.rectTransform.position = player;
        }
        if(teamcheck&& Data.GC.Gamestate < 4)
        {
            player = camera_A.WorldToScreenPoint(pos);
            player.x += 13f;
            player.y += 8f;
            Teammark.rectTransform.position = player;
            switch (Data.characterTeam)
            {
                case 1:
                    Teammark.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                    break;
                case 2:
                    Teammark.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
                    break;
                case 3:
                    Teammark.color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
                    break;
                case 4:
                    Teammark.color = new Color(1.0f, 0.92f, 0.0016f, 0.5f);
                    break;
            }
        }

        //プレイヤライフの星
        if (Data.character_NOW > 2&& Data.character_NOW != 4)
        {
            lifemark2.enabled = true;
            player = camera_A.WorldToScreenPoint(pos);
            player.x -= 15f;
            player.y += 12f;
            lifemark2.rectTransform.position = player;
        }
        else lifemark2.enabled = false;
        if (Data.character_NOW > 1 && Data.character_NOW != 4)
        {
            lifemark1.enabled = true;
            player = camera_A.WorldToScreenPoint(pos);
            player.x -= 4f;
            player.y += 12f;
            lifemark1.rectTransform.position = player;
        }
        else lifemark1.enabled = false;
        //
        if(Data.DeadStep==6|| Data.DeadStep == 7)
        {
            RebonTime.gameObject.SetActive(true);
            RebonTime.text = "" + Data.RebonTime;
            pos = Character.transform.position;
            pos.y = 0.5f;
            player = camera_A.WorldToScreenPoint(pos);
            RebonTime.transform.position = player;
        }
        else RebonTime.gameObject.SetActive(false);
        if(Data.GC.Gamestate>=4)
        {
            Color cr = RebonTime.color;
            cr.a = 0f;
            RebonTime.color = cr;
        }
    }

}
