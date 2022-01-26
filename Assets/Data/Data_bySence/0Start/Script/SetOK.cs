using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOK : MonoBehaviour
{
    public Game_Open GO;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OK()
    {
        GO.setok();
    }
    void Off()
    {
        gameObject.SetActive(false);
    }
    void START_OK()
    {
        GO.open_anime = 1;
    }
}
