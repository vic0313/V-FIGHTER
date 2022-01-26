using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Game_Open : MonoBehaviour
{
    public Animator word;
    public int open_anime;
    public GameObject[] op;
    public AudioClip[] gamestart;
    public AudioSource audiosource;
    public bool play;
    // Start is called before the first frame update
    void Start()
    {
        open_anime = 0;
        play = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(open_anime)
        {
            case 1:
                op[0].SetActive(true);
                break;
            case 2:
                op[1].SetActive(true);
                break;
            case 3:
                op[2].SetActive(true);
                break;
            case 4:
                op[3].SetActive(true);
                break;
            case 5:
                op[4].SetActive(true);
                break;
            case 6:
                op[5].SetActive(true);
                break;
            case 7:
                op[6].SetActive(true);
                if (play == false)
                {
                    audiosource.PlayOneShot(gamestart[0]);
                    play = true;
                }
                break;
            case 8:
                op[7].SetActive(true);
                break;
            case 9:
                if(Input.anyKey)
                {
                    word.SetBool("OK", true);
                    audiosource.PlayOneShot(gamestart[1]);
                    open_anime++;
                }
                break;
            case 10:
                op[6].SetActive(true);
                break;
            case 12:
                op[8].SetActive(true);
                break;
            case 13:
                SceneManager.LoadScene(1);
                break;
        }
    }
    public void setok()
    {
        open_anime++;
    }
    
}
