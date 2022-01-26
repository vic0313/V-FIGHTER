using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark_rot : MonoBehaviour
{
    public GameInfo GI;
    public Animator animator;
    public int mode_num;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void Update()
    {
        if(GI.Gamemode_choice== mode_num)
        {
            animator.SetBool("chioce", true);
        }else
        {
            animator.SetBool("chioce", false);
        }
    }
}
