using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamestart : MonoBehaviour
{

    public GameControler GC;
    private void Gamestart()
    {
        GC.Gamestate++;
    }
}
