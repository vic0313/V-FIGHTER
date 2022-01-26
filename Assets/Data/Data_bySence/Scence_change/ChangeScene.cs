using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public GameInfo GI;
    // Start is called before the first frame update
    void Start()
    {
        GI = GameInfo.game_data;
    }

    // Update is called once per frame
    void Update()
    {
        GI = GameInfo.game_data;
    }
    void ToMainScene()
    {
        GI.SceneChoiceOK = false;
        GI.Scene_now = 1;
        GI.setingstate = 0;
        GI.chioce_now = 0;
        GI.LoadScene_OK();
    }
    void ToSetScene()
    {
        GI.SceneChoiceOK = false;
        GI.Scene_now = 2;
        GI.setingstate = 0;
        GI.chioce_now = 0;
        GI.LoadScene_OK();
    }
    void ToAllSetingScene()
    {
        GI.SceneChoiceOK = false;
        GI.Scene_now = 3;
        GI.setingstate = 0;
        GI.chioce_now = 0;
        GI.LoadScene_OK();
    }
    void ToGameScene()
    {
        GI.SceneChoiceOK = false;
        GI.Scene_now = 4;
        GI.setingstate = 0;
        GI.chioce_now = 0;
        GI.LoadScene_OK();
    }
    void ToResultScene()
    {
        GI.SceneChoiceOK = false;
        GI.Scene_now = 4;
        GI.setingstate = 0;
        GI.chioce_now = 0;
        GI.LoadScene_OK();
    }
}
