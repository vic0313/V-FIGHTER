using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set2 : MonoBehaviour
{
    public SetControler SC;
    public GameObject[] job_prefab;
    public Animator self;
    public Animator[] P;
    public void Active_false()
    {
        this.gameObject.SetActive(false);
    }
    public void stepUp()
    {
        SC.GI.setingstate++;
    }
    public void JoyInput(int p_num)
    {
        //右左
        switch (p_num)
        {
            case 0:
                if (Input.GetAxis("Horizontal player1") != 0)
                {
                    SC.GI.input_h_Joy[0] = Input.GetAxis("Horizontal player1");
                }
                break;
            case 1:
                if (Input.GetAxis("Horizontal player2") != 0)
                {
                    SC.GI.input_h_Joy[1] = Input.GetAxis("Horizontal player2");
                }
                break;
            case 2:
                if (Input.GetAxis("Horizontal player3") != 0)
                {
                    SC.GI.input_h_Joy[2] = Input.GetAxis("Horizontal player3");
                }
                break;
            case 3:
                if (Input.GetAxis("Horizontal player4") != 0)
                {
                    SC.GI.input_h_Joy[3] = Input.GetAxis("Horizontal player4");
                }
                break;
        }
    }
    public void JoyCheck(int P_num)
    {
        if(SC.JoyCheck[P_num] ==true)
        {
            switch(P_num)
            {
                case 0:
                    Invoke("Joy1Start", 0.3f);
                    break;
                case 1:
                    Invoke("Joy2Start", 0.3f);
                    break;
                case 2:
                    Invoke("Joy3Start", 0.3f);
                    break;
                case 3:
                    Invoke("Joy4Start", 0.3f);
                    break;
            }
            
        }
    }
    private void Joy1Start()
    {
        SC.JoyCheck[0] = false;
    }
    private void Joy2Start()
    {
        SC.JoyCheck[1] = false;
    }
    private void Joy3Start()
    {
        SC.JoyCheck[2] = false;
    }
    private void Joy4Start()
    {
        SC.JoyCheck[3] = false;
    }
}
