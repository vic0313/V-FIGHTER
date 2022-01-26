using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITEM_Type : MonoBehaviour
{
    public GameControler GC;
    public ITEM item_script;
    public GameObject item_name;
    public Collider item_collider;
    public Collider HitRange;
    public Collider PickRange;
    public MeshRenderer skin;
    public GameObject[] Effect_prefab;
    private string[] hittag;
    private GameObject Item89_hit;
    public AudioSource audiosource;
    public AudioClip[] soundeffect;
    // Start is called before the first frame update
    void Start()
    {
        hittag = new string[2];
        hittag[0] = "030";
        hittag[1] = "030";
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //キャラクタの攻撃の処理
        if(other.transform.parent!=null)
        {
            if (item_script.bePickup == false&& (other.tag != "Down" && other.tag != "Top")
                && (other.transform.parent.tag == "AB_Sword" || other.transform.parent.tag == "AB_Pouch" || other.transform.parent.tag == "AB_Magic" || other.transform.parent.tag == "AB_Gun" || other.transform.parent.tag == "AB_Assassin"))
            {
                if(other.transform.parent.transform.parent!=null)
                {
                    item_script.moveway = gameObject.transform.position - other.transform.parent.transform.parent.position;
                    item_script.FinalHit_number = other.transform.parent.transform.parent.GetComponent<CharacterData>().characterNumber;
                }
                else
                {
                    item_script.moveway = gameObject.transform.position - other.transform.parent.position;
                    if (other.transform.parent.tag == "AB_Magic")
                    {
                        if (other.tag == "AirAttack2" || other.tag == "GroundAttack_Front")item_script.FinalHit_number = other.transform.parent.GetComponent<M_AttackFront>().characterNumber;
                        else if (other.tag == "GroundAttack_Defense") item_script.FinalHit_number = other.transform.parent.GetComponent<GroundAttack_Defense>().characterNumber;
                    }
                    else if (other.transform.parent.tag == "AB_Gun")
                    {
                        switch(other.tag)
                        {
                            case "GroundAttack_Front":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<G_GA_Front>().characterNumber;
                                audiosource.PlayOneShot(soundeffect[0]);
                                other.transform.parent.GetComponent<G_GA_Front>().DestroyHit();
                                break;
                            case "GroundAttack(1)":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<G_GA>().characterNumber;
                                break;
                            case "GroundAttack(2)":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<G_GA>().characterNumber;
                                break;
                            case "GroundAttack(3)":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<G_GA2D>().characterNumber;
                                break;
                            case "GroundAttack2_Defense":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<G_GA2D>().characterNumber;
                                break;
                            case "AirAttack":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<Grenade>().characterNumber;
                                break;
                        }
                    }
                    else if (other.transform.parent.tag == "AB_Assassin")
                    {
                        switch (other.tag)
                        {
                            case "GroundAttack_Defense":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<A_GD>().characterNumber;
                                break;
                            case "GroundAttack2":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<A_GA2>().characterNumber;
                                Destroy(other.transform.parent.gameObject);
                                break;
                            case "GroundAttack2_Defense":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<A_GroundAttack2Defense>().characterNumber;
                                break;
                            case "AirAttack2_start":
                                item_script.FinalHit_number = other.transform.parent.GetComponent<A_AirAttack2>().characterNumber;
                                break;
                        }
                    }
                }

                if (item_script.Item_num<=7)
                {
                    item_script.moveway.y = 0.3f;
                    item_script.moveway.Normalize();
                    item_script.self.AddForce(item_script.moveway * 0.00003f);
                }
                else if (item_script.Item_num == 8)
                {
                    item_script.moveway.y = 0.0f;
                    item_script.moveway.Normalize();
                    item_script.self.AddForce(item_script.moveway * 600f);
                    item_script.self.drag = 0;
                }
                else if (item_script.Item_num == 12)
                {
                    item_script.moveway.y = 0.0f;
                    item_script.moveway.Normalize();
                    item_script.self.AddForce(item_script.moveway * 500f);
                    item_script.self.drag = 0;
                }
                if (item_script.Item_num == 8 || item_script.Item_num == 9 || item_script.Item_num == 12)
                {
                    if(other.transform.parent.tag == "AB_Magic")
                    {
                        if (other.tag == "AirAttack2" || other.tag == "GroundAttack_Front")
                        {
                            Instantiate(other.transform.parent.GetComponent<M_AttackFront>().hiteffect, other.transform.parent.transform.position, Quaternion.identity);
                            Destroy(other.transform.parent.gameObject);
                        }
                    }
                    else if (other.transform.parent.tag == "AB_Gun")
                    {
                        if(other.tag== "GroundAttack(3)") other.transform.parent.GetComponent<G_GA2D>().animator.SetBool("OK", true);
                    }
                    if ((item_script.HP_durable - 1) <= 0)
                    {
                        item_script.HP_durable--;
                        Instantiate(Effect_prefab[1], transform.position, transform.rotation);
                    }
                    else if ((item_script.HP_durable-1) > 0)
                    {
                        if(Item89_hit==null|| (other.tag!= hittag[0]&& other.transform.parent.transform.tag== hittag[1])|| (other.tag == hittag[0] && other.transform.parent.transform.tag != hittag[1]))
                        {
                            
                            Item89_hit = Instantiate(Effect_prefab[0], transform.position, transform.rotation);
                            hittag[0] = other.tag;
                            hittag[1] = other.transform.parent.transform.tag;
                            item_script.HP_durable--;
                        }
                    }
                }
                else if (item_script.Item_num != 10&& item_script.Item_num != 11)
                {
                    if ((item_script.HP_durable - 1) <= 0)
                    {
                        item_script.HP_durable--;
                        Instantiate(Effect_prefab[1], transform.position, transform.rotation);
                    }
                    else
                    {
                        Instantiate(Effect_prefab[0], transform.position, transform.rotation);
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //
        if (other.transform.parent != null)
        {
            if (item_script.bePickup == false  && (other.tag != "Down" && other.tag != "Top")
                && (other.transform.parent.tag == "AB_Sword"|| other.transform.parent.tag == "AB_Pouch"|| other.transform.parent.tag == "AB_Magic"|| other.transform.parent.tag == "AB_Gun"|| other.transform.parent.tag == "AB_Assassin"))
            {
                //9と11は攻撃不能
                if (item_script.Item_num<=7)
                {
                    item_script.HP_durable--;
                }
            }
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        //ITEM6の効果
        if (other.tag == "Item6_Attack" && item_script.Item_num != 9 && item_script.Item_num != 11)
        {
            item_script.moveway = gameObject.transform.position - other.transform.parent.position;
            item_script.moveway.y = 0.0f;
            item_script.moveway.Normalize();
            if (Mathf.Abs(item_script.moveway.x) <= 0.05f)
            {
                item_script.moveway.x = 0.05f;
            }
            if (Mathf.Abs(item_script.moveway.x) <= 0.05f)
            {
                item_script.moveway.z = 0.05f;
            }
            if (item_script.Item_num==8)
            {
                item_script.self.AddForce(item_script.moveway * 1000f);
                item_script.self.drag = 0;
            }
            else if(item_script.Item_num<=7)
            {
                item_script.moveway.y = 1f;
                item_script.self.AddForce(item_script.moveway * 0.000001f);
            }
        }
        //ITEM9の下
        if (other.tag == "ITEM_HitRange" && other.gameObject.layer == 8&& (other.transform.position.y-1f)>= gameObject.transform.position.y)
        {
            item_script.moveway = gameObject.transform.position - other.transform.parent.position;
            item_script.moveway.y = 0.0f;
            item_script.moveway.Normalize();
            if (Mathf.Abs(item_script.moveway.x) <= 1f)
            {
                item_script.moveway.x = 1f;
            }
            if (Mathf.Abs(item_script.moveway.x) <= 1f)
            {
                item_script.moveway.z = 1f;
            }
            if (item_script.Item_num==8)
            {
                item_script.self.AddForce(item_script.moveway * 300f);
                item_script.self.drag = 0;
            }
            else if(item_script.Item_num<=7)
            {
                item_script.self.AddForce(item_script.moveway * 0.00001f);
            }
        }

        if (other.tag == "DeadSpace_gold" && GC.Gamestate <4)
        {
            if(GC.Gamemode_num == 1 && item_script.pointplus==false)
            {
                int getpoint = 0;
                if (item_script.Item_num == 8) getpoint = 1;
                else if (item_script.Item_num == 12) getpoint = 3;
                if (item_script.FinalHit_number >= 0)
                {
                    if (gameObject.transform.position.x < -13)
                    {
                        if (GC.Character_Data[item_script.FinalHit_number].characterTeam == 1) GC.TeamPointPlus[1]+= getpoint;
                        else GC.Character_Data[item_script.FinalHit_number].Character_Point += getpoint;
                    }
                    else if (gameObject.transform.position.x > 13)
                    {
                        if (GC.Character_Data[item_script.FinalHit_number].characterTeam == 2) GC.TeamPointPlus[0] += getpoint;
                        else GC.Character_Data[item_script.FinalHit_number].Character_Point += getpoint;
                    }
                }
                else
                {
                    if (gameObject.transform.position.x < -13) GC.TeamPointPlus[1] += getpoint;
                    else if (gameObject.transform.position.x > 13) GC.TeamPointPlus[0] += getpoint;
                }
                item_script.pointplus = true;
            } 
            Destroy(this.gameObject);
        }
        if(other.tag== "DeadSpace")
        {
            if(item_script.bePickup)
            {
                item_script.PickupOne.GetComponent<CharacterData>().pickrangein = false;
                item_script.PickupOne.GetComponent<Animator>().SetBool("PickUp", false);
            }
            Destroy(this.gameObject);
        }
    }
}
