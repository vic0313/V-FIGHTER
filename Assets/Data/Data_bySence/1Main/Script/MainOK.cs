using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOK : MonoBehaviour
{
    public GameInfo GI;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void setOK()
    {
        GI.mainok = true;
    }
    void setNotOK()
    {
        GI.mainok = false;
    }
}
