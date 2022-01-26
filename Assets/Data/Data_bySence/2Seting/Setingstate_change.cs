using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setingstate_change : MonoBehaviour
{
    public SetControler SC;
    // Start is called before the first frame update
    public void Setstate_up()
    {
        SC.GI.Setingstate_up();
    }
    public void Setstate_down()
    {
        SC.GI.Setingstate_down();
    }
}
