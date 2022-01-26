using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_groundUI : MonoBehaviour
{
    public GameObject thisobj;
    public MeshRenderer selfMR;
    private Color a;
    private Vector3 posY;
    private RaycastHit hit;
    public Transform down;
    // Start is called before the first frame update
    void Start()
    {
        posY = new Vector3(0.0f,0.1f,0.0f);
        switch (thisobj.tag)
        {
            case "Player1":
                a = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                break;
            case "Player2":
                a = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                break;
            case "Player3":
                a = new Color(1.0f, 0.0f, 1.0f, 1.0f);
                break;
            case "Player4":
                a = new Color(1.0f, 0.92f, 0.0016f, 1.0f);
                break;
            default:
                a = new Color(0.3f, 0.3f, 0.3f, 1.0f);
                break;
        }
        selfMR.material.color = a;

        Vector3 fwd = (Vector3.down);
        Vector3 pos = down.position;
        pos.y += 0.05f;
        LayerMask mask = 1 << 8;
        if (Physics.Raycast(pos, fwd, out hit, 50f, mask))
        {
            posY.y = hit.point.y;
            posY.y += 0.5f;
        }
        else
        {
            posY.y = 0.1f;
        }

        posY.x = gameObject.transform.position.x;
        posY.z = gameObject.transform.position.z;
        gameObject.transform.position = posY;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fwd = (Vector3.down);
        Vector3 pos = down.position;
        pos.y += 0.05f;
        LayerMask mask = 1 << 8;
        if (Physics.Raycast(pos, fwd, out hit, 50f, mask))
        {
            posY.y = hit.point.y;
            posY.y += 0.5f;
        }
        else
        {
            posY.y = 0.1f;
        }
        
        posY.x = gameObject.transform.position.x;
        posY.z = gameObject.transform.position.z;
        gameObject.transform.position = posY;
    }
   
}
