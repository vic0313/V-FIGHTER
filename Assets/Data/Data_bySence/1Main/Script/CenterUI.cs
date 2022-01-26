using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterUI : MonoBehaviour
{
    public GameInfo GI;
    public GameObject[] UI;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void Update()
    {
        UI[0].SetActive(true);
        switch(GI.Gamemode_choice)
        {
            case 0:
                UI[0].SetActive(true);
                UI[1].SetActive(false);
                UI[2].SetActive(false);
                UI[3].SetActive(false);
                break;
            case 1:
                UI[1].SetActive(true);
                UI[0].SetActive(false);
                UI[2].SetActive(false);
                UI[3].SetActive(false);
                break;
            case 2:
                UI[2].SetActive(true);
                UI[0].SetActive(false);
                UI[1].SetActive(false);
                UI[3].SetActive(false);
                break;
            case 3:
                UI[3].SetActive(true);
                UI[0].SetActive(false);
                UI[1].SetActive(false);
                UI[2].SetActive(false);
                break;
        }
    }
}
