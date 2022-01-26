using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_change : MonoBehaviour
{
    public GameInfo GI;
    public Animator animator;
    enum M_S
    {
        NextIsNormalSetingScence = 0,
        NextIsBBSetingScence,
        NextIsJumpSetingScence,
        NextIsAllSetingScence,
    }
    enum Scence
    {
        Open = 0,
        Main,
        Gameseting,
        All_Seting,
        Normal_game,
        BB_game,
        Jump_game,
    }
    // Update is called once per frame
    void Update()
    {
        GI = GameInfo.game_data;
        if (GI.SceneChoiceOK)
        {
            animator.SetBool("OK", true);
        }
    }
    void SceneNext()
    {
        if(GI.Gamemode_choice<3)
        {
            GI.Scene_now = (int)Scence.Gameseting;
        }
        else
        {
            GI.Scene_now = (int)Scence.All_Seting;
        }
        GI.LoadScene_OK();
    }
}
