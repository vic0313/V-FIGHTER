using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControler : MonoBehaviour
{
    public GameInfo GI;
    public AudioClip[] effect;
    public AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void Update()
    {
        if ((GI.InputJoycheck_all[0] && GI.JoycheckStart == false&&Mathf.Abs(GI.input_v_All_Joy)>0.3f) || Input.GetButtonDown("Vertical player1") || Input.GetButtonDown("Vertical player2") || Input.GetButtonDown("Vertical player3") || Input.GetButtonDown("Vertical player4"))
        {
            if (GI.InputJoycheck_all[0] && GI.JoycheckStart == false && Mathf.Abs(GI.input_v_All_Joy) > 0.3f)
            {
                switch (GI.Gamemode_choice)
                {
                    case 0:
                        if (GI.input_v_All_Joy < (-0.3f))
                        {
                            GI.Gamemode_choice = 1;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 1:
                        if (GI.input_v_All_Joy > 0.3f)
                        {
                            GI.Gamemode_choice = 0;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 2:
                        if (GI.input_v_All_Joy < (-0.3f))
                        {
                            GI.Gamemode_choice = 3;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 3:
                        if (GI.input_v_All_Joy > 0.3f)
                        {
                            GI.Gamemode_choice = 2;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                }
                GI.InputJoycheck_all[0] = false;
                GI.input_v_All_Joy = 0;
                GI.JoycheckStart = true;
                GI.InvokeJoyCheck();
            }
            else
            {
                switch (GI.Gamemode_choice)
                {
                    case 0:
                        if (GI.input_v_All < 0)
                        {
                            GI.Gamemode_choice = 1;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 1:
                        if (GI.input_v_All > 0)
                        {
                            GI.Gamemode_choice = 0;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 2:
                        if (GI.input_v_All < 0)
                        {
                            GI.Gamemode_choice = 3;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 3:
                        if (GI.input_v_All > 0)
                        {
                            GI.Gamemode_choice = 2;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                }
                
            }
        }
        else if ((GI.InputJoycheck_all[1] && GI.JoycheckStart==false && Mathf.Abs(GI.input_h_All_Joy) > 0.3f) || Input.GetButtonDown("Horizontal player1") || Input.GetButtonDown("Horizontal player2") || Input.GetButtonDown("Horizontal player3") || Input.GetButtonDown("Horizontal player4"))
        {
            if (GI.InputJoycheck_all[1] && GI.JoycheckStart == false && Mathf.Abs(GI.input_h_All_Joy) > 0.3f)
            {
                switch (GI.Gamemode_choice)
                {
                    case 0:
                        if (GI.input_h_All_Joy > 0.3f)
                        {
                            GI.Gamemode_choice = 2;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 1:
                        if (GI.input_h_All_Joy > 0.3f)
                        {
                            GI.Gamemode_choice = 3;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 2:
                        if (GI.input_h_All_Joy < (-0.3f))
                        {
                            GI.Gamemode_choice = 0;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 3:
                        if (GI.input_h_All_Joy < (-0.3f))
                        {
                            GI.Gamemode_choice = 1;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                }
                GI.InputJoycheck_all[1] = false;
                GI.input_h_All_Joy = 0;
                GI.JoycheckStart = true;
                GI.InvokeJoyCheck();
            }
            else
            {
                switch (GI.Gamemode_choice)
                {
                    case 0:
                        if (GI.input_h_All > 0)
                        {
                            GI.Gamemode_choice = 2;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 1:
                        if (GI.input_h_All > 0)
                        {
                            GI.Gamemode_choice = 3;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 2:
                        if (GI.input_h_All < 0)
                        {
                            GI.Gamemode_choice = 0;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                    case 3:
                        if (GI.input_h_All < 0)
                        {
                            GI.Gamemode_choice = 1;
                            audiosource.PlayOneShot(effect[0]);
                        }
                        break;
                }
                audiosource.PlayOneShot(effect[0]);
            }
        }
        if (Input.GetButtonDown("Attack1 player1") || Input.GetButtonDown("Attack1 player2") || Input.GetButtonDown("Attack1 player3") || Input.GetButtonDown("Attack1 player4") || Input.GetKeyDown(KeyCode.Return))
        {
            GI.SceneChoiceOK = true;
            audiosource.PlayOneShot(effect[1]);
        }
    }
}
