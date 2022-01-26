using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Game : MonoBehaviour
{
    public float cspeed = 2;
    public GameControler GC;
    private float Min_XZ=6.5f;
    private float Max_XZ=12f;
    private float Now_XZ;
    private float Min_Y = 4.5f;
    private float Max_Y = 8.5f;
    private float Now_Y;
    private Vector3 cameraPosmin= new Vector3(0f, 9f, -5.43f);
    private Vector3 cameraPosmax = new Vector3(0f, 14f, -5.46f);
    private Vector3 cameraPosnext ;
    public GameObject[] gameset_target;
    private Vector3 now;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = cameraPosmin;
        gameset_target = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (GC.Gamestate > 1&&GC.Gamestate<4)
        {
            float XZ_now = Min_XZ;
            float Y_now = Min_Y;
            foreach (var gc in GC.Character_Data)
            {
                if(gc.DeadStep==0)
                {
                    if (Mathf.Abs(gc.transform.position.y) >= Min_Y && Mathf.Abs(gc.transform.position.y) >= Y_now)
                    {
                        Y_now = Mathf.Abs(gc.transform.position.y);
                        if (Mathf.Abs(gc.transform.position.y) >= Max_Y)
                        {
                            Y_now = Max_Y;
                        }
                    }
                    if (Mathf.Abs(gc.transform.position.x) >= Min_XZ && Mathf.Abs(gc.transform.position.x) >= XZ_now)
                    {
                        XZ_now = Mathf.Abs(gc.transform.position.x);
                        if (Mathf.Abs(gc.transform.position.x) >= Max_XZ)
                        {
                            XZ_now = Max_XZ;
                        }
                    }
                }
                
            }
            Now_XZ = XZ_now;
            Now_Y = Y_now;
            if (Now_Y > Min_Y || Now_XZ > Min_XZ)
            {
                float XZ = (Now_XZ - Min_XZ) / (Max_XZ - Min_XZ);
                float Y = (Now_Y - Min_Y) / (Max_Y - Min_Y);
                if (XZ > Y)
                {
                    float y_move = cameraPosmin.y + ((cameraPosmax.y - cameraPosmin.y) * ((Now_XZ - Min_XZ) / (Max_XZ - Min_XZ)));
                    float z_mpve = cameraPosmin.z + ((cameraPosmax.z - cameraPosmin.z) * ((Now_XZ - Min_XZ) / (Max_XZ - Min_XZ)));
                    cameraPosnext = new Vector3(0f, y_move, z_mpve);
                    transform.position = Vector3.Lerp(transform.position, cameraPosnext, cspeed * Time.deltaTime);
                }
                else
                {
                    float y_move = cameraPosmin.y + ((cameraPosmax.y - cameraPosmin.y) * ((Now_Y - Min_Y) / (Max_Y - Min_Y)));
                    float z_mpve = cameraPosmin.z + ((cameraPosmax.z - cameraPosmin.z) * ((Now_Y - Min_Y) / (Max_Y - Min_Y)));
                    cameraPosnext = new Vector3(0f, y_move, z_mpve);
                    transform.position = Vector3.Lerp(transform.position, cameraPosnext, cspeed * Time.deltaTime);
                }
            }
            now = transform.position;
            
        }
        else if(GC.Gamestate==4)
        {
            if(GC.GameSet==1|| (GC.GameSet == 3&& GC.Gamemode_num==1))
            {
                //時間制限
            }
            else
            {
                //AllKill OR point到達
                Vector3 kyori = gameset_target[0].transform.position + ((gameset_target[1].transform.position - gameset_target[0].transform.position) / 2);
                transform.position = new Vector3(kyori.x, kyori.y + 3, kyori.z - 2);
            }
        }
        else if (GC.Gamestate >= 5)
        {
            transform.position = now;
            float XZ_now = Min_XZ;
            float Y_now = Min_Y;
            foreach (var gc in GC.Character_Data)
            {
                if (gc.DeadStep == 0)
                {
                    if (Mathf.Abs(gc.transform.position.y) >= Min_Y && Mathf.Abs(gc.transform.position.y) >= Y_now)
                    {
                        Y_now = Mathf.Abs(gc.transform.position.y);
                        if (Mathf.Abs(gc.transform.position.y) >= Max_Y)
                        {
                            Y_now = Max_Y;
                        }
                    }
                    if (Mathf.Abs(gc.transform.position.x) >= Min_XZ && Mathf.Abs(gc.transform.position.x) >= XZ_now)
                    {
                        XZ_now = Mathf.Abs(gc.transform.position.x);
                        if (Mathf.Abs(gc.transform.position.x) >= Max_XZ)
                        {
                            XZ_now = Max_XZ;
                        }
                    }
                }

            }
            Now_XZ = XZ_now;
            Now_Y = Y_now;
            if (Now_Y > Min_Y || Now_XZ > Min_XZ)
            {
                float XZ = (Now_XZ - Min_XZ) / (Max_XZ - Min_XZ);
                float Y = (Now_Y - Min_Y) / (Max_Y - Min_Y);
                if (XZ > Y)
                {
                    float y_move = cameraPosmin.y + ((cameraPosmax.y - cameraPosmin.y) * ((Now_XZ - Min_XZ) / (Max_XZ - Min_XZ)));
                    float z_mpve = cameraPosmin.z + ((cameraPosmax.z - cameraPosmin.z) * ((Now_XZ - Min_XZ) / (Max_XZ - Min_XZ)));
                    cameraPosnext = new Vector3(0f, y_move, z_mpve);
                    transform.position = Vector3.Lerp(transform.position, cameraPosnext, cspeed * Time.deltaTime);
                }
                else
                {
                    float y_move = cameraPosmin.y + ((cameraPosmax.y - cameraPosmin.y) * ((Now_Y - Min_Y) / (Max_Y - Min_Y)));
                    float z_mpve = cameraPosmin.z + ((cameraPosmax.z - cameraPosmin.z) * ((Now_Y - Min_Y) / (Max_Y - Min_Y)));
                    cameraPosnext = new Vector3(0f, y_move, z_mpve);
                    transform.position = Vector3.Lerp(transform.position, cameraPosnext, cspeed * Time.deltaTime);
                }
            }
            now = transform.position;
        }
    }
}
