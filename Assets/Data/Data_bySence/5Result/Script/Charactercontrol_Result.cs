using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactercontrol_Result : MonoBehaviour
{
    
    public SkinnedMeshRenderer[] Hair_obj;
    public SkinnedMeshRenderer Clothes_obj;
    public Material[] teamType;
    public int characterTeam;
    public int character_Profession;                //キャラの職業
    public Material[] characterType;
    public Material CPU_Clothe;
    //現在ゲームの職業の数量
    public int Sword_num;
    public int Pounch_num;
    public int Magic_num;
    public int Gun_num;
    public int Assassin_num;
    public GameObject winnerEffect;
    enum Profession
    {
        Sword = 0,
        Pounch,
        Magic,
        Gun,
        Assassin,
    }
    // Start is called before the first frame update
    void Start()
    {
        Hair_obj[0].material = teamType[characterTeam];
        Hair_obj[1].material = teamType[characterTeam];
        if (gameObject.tag == "Player1" || gameObject.tag == "Player2" || gameObject.tag == "Player3" || gameObject.tag == "Player4")
        {
            switch (character_Profession)
            {
                case (int)Profession.Sword:
                    Clothes_obj.material = characterType[Sword_num];
                    break;
                case (int)Profession.Pounch:
                    Clothes_obj.material = characterType[Pounch_num];
                    break;
                case (int)Profession.Magic:
                    Clothes_obj.material = characterType[Magic_num];
                    break;
                case (int)Profession.Gun:
                    Clothes_obj.material = characterType[Gun_num];
                    break;
                case (int)Profession.Assassin:
                    Clothes_obj.material = characterType[Assassin_num];
                    break;
            }
        }
        else
        {
            Clothes_obj.material = CPU_Clothe;
        }
    }

    
}
