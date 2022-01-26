using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITEM : MonoBehaviour
{
    public Rigidbody self;
    public ITEM_Type Item;
    public GameObject PickupOne;
    public int FinalHit_number;
    public Vector3 moveway;
    public float speednow;
    public bool bePickup;
    public int HP_durable;
    public int HPMAX_durable = 5;
    private bool gameopen;
    public float Hit_character_Damage;
    public int Item_num;
    public bool pointplus;
    public bool bananabotan;
    public bool bananaing;
    // Start is called before the first frame update
    void Start()
    {
        moveway = new Vector3(0, 0, 0);
        gameopen = false;
        bePickup = false;
        FinalHit_number = -1;
        pointplus = false;
        bananaing = false;
        switch (gameObject.tag)
        {
            case "Item_0":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 0;
                break;
            case "Item_1":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 1;
                break;
            case "Item_2":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 2;
                break;
            case "Item_3":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 3;
                break;
            case "Item_4":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 4;
                break;
            case "Item_5":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.02f;
                Item_num = 5;
                break;
            case "Item_6":
                Hit_character_Damage = 0.03f;
                HPMAX_durable = 5;
                Item_num = 6;
                break;
            case "Item_7":
                Hit_character_Damage = 0.03f;
                HPMAX_durable = 5;
                Item_num = 7;
                break;
            case "Item_N8":
                HPMAX_durable = 5;
                Hit_character_Damage = 0.05f;
                Item_num = 8;
                break;
            case "Item_N9":
                HPMAX_durable = 7;
                Hit_character_Damage = 0.05f;
                Item_num = 9;
                break;
            case "Item_N10":
                HPMAX_durable = 6;
                Item_num = 10;
                Quaternion rott = Item.transform.rotation;
                rott.x = -1f;
                Item.transform.rotation = rott;
                break;
            case "Item_N11":
                HPMAX_durable = 6;
                Item_num = 11;
                Quaternion rot = Item.transform.rotation;
                rot.x = -1f;
                Item.transform.rotation = rot;
                break;
            case "Item_12_BB":
                HPMAX_durable = 5000;
                Hit_character_Damage = 0.05f;
                Item_num = 12;
                break;
        }
        if (Item_num<=8)
        {
            speednow = Mathf.Sqrt(Mathf.Pow(self.velocity.x, 2) + Mathf.Pow(self.velocity.z, 2));
        }
        else
        {
            speednow = 0;
        }
            
        HP_durable = HPMAX_durable;
        bananabotan = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(HP_durable<=0)
        {
            if(bePickup)
            {
                PickupOne.GetComponent<CharacterData>().pickrangein = false;
                PickupOne.GetComponent<Animator>().SetBool("PickUp", false);
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
        //BANANA 
        if(bananabotan==true)
        {
            Item.item_collider.enabled = false;
            Item.PickRange.enabled = false;
            self.drag = 1000;
            self.angularDrag = 1000;
            Item.Effect_prefab[2].SetActive(true);
            if (gameObject.transform.parent.position.y < 0.1f)
            {
                Vector3 vec = gameObject.transform.parent.position;
                vec.y = 0.1f;
                gameObject.transform.parent.position = vec;
                self.useGravity = false;
            }
            if (speednow<3)
            {
                self.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            }
        }
        //10と11の位置制御
        if(Item_num==10|| Item_num == 11)
        {
            if(gameObject.transform.parent.position.y<0.1f)
            {
                Vector3 vec = gameObject.transform.parent.position;
                vec.y = 0.1f;
                gameObject.transform.parent.position = vec;
                self.useGravity = false;
            }
        }
        
        //
        if(bePickup==false)
        {
            if(Item_num <= 7)
            {
                if (self.useGravity == false)
                {
                    self.useGravity = true;
                    self.isKinematic = false;
                    Item.item_collider.enabled = true;
                    if (Item_num == 6 || Item_num == 7)
                    {
                        Item.skin.enabled = true;
                    }
                }
            }
            if (Item_num <= 8|| Item_num==12)
            {
                speednow = Mathf.Sqrt(Mathf.Pow(self.velocity.x, 2) + Mathf.Pow(self.velocity.z, 2));
            }
            //BUNCYBALLの空気力の変化
            if (Item_num==8)
            {
                moveway.y = 0.0f;
                if (gameopen == false)
                {
                    if (gameObject.transform.position.y < 0.8f)
                    {
                        self.drag = 5;
                        gameopen = true;
                    }
                }
                else
                {
                    self.drag += Time.deltaTime * 0.01f;
                    if (self.drag >= 10)
                    {
                        self.drag = 10;
                    }
                }
            }
            ////壁を当たった処理
            //if (gameObject.transform.position.y > 0.19f)
            //{
            //    bool hitwall = false;
            //    if(Item_num<=5)
            //    {
            //        if (gameObject.transform.position.z >= 4.0f)
            //        {
            //            hitwall = true;
            //            moveway.z = -1f;
            //        }
            //        else if (gameObject.transform.position.z <= -4.0f)
            //        {
            //            moveway.z = 1f;
            //            hitwall = true;
            //        }
            //        if (gameObject.transform.position.x >= 12f)
            //        {
            //            moveway.x = -1f;
            //            hitwall = true;
            //        }
            //        else if (gameObject.transform.position.x <= -12f)
            //        {
            //            moveway.x = 1f;
            //            hitwall = true;
            //        }
            //    }
            //    else if (Item_num == 6|| Item_num == 7)
            //    {
            //        if (gameObject.transform.position.z >= 4.2f)
            //        {
            //            hitwall = true;
            //            moveway.z = -1f;
            //        }
            //        else if (gameObject.transform.position.z <= -4.2f)
            //        {
            //            moveway.z = 1f;
            //            hitwall = true;
            //        }
            //        if (gameObject.transform.position.x >= 11.5f)
            //        {
            //            moveway.x = -1f;
            //            hitwall = true;
            //        }
            //        else if (gameObject.transform.position.x <= -11.5f)
            //        {
            //            moveway.x = 1f;
            //            hitwall = true;
            //        }
            //    }
            //    else if (Item_num == 8)
            //    {
            //        //if (gameObject.transform.position.z >= 4f)
            //        //{
            //        //    hitwall = true;
            //        //    moveway.z = -1f;
            //        //}
            //        //else if (gameObject.transform.position.z <= -4f)
            //        //{
            //        //    moveway.z = 1f;
            //        //    hitwall = true;
            //        //}
            //        //if (gameObject.transform.position.x >= 11.5f)
            //        //{
            //        //    moveway.x = -1f;
            //        //    hitwall = true;
            //        //}
            //        //else if (gameObject.transform.position.x <= -11.5f)
            //        //{
            //        //    moveway.x = 1f;
            //        //    hitwall = true;
            //        //}
            //    }
            //    if (hitwall == true)
            //    {
            //        if (Item_num == 8)
            //        {
            //            //self.AddForce(moveway * 2f);
            //            //self.drag = 0;
            //        }
            //        else
            //        {
            //            self.AddForce(moveway * 0.0000001f);
            //        }
            //    }
            //}
            
            ////アイテムが舞台外に落ちる処理
            //if (gameObject.transform.position.z >= 6.0f || gameObject.transform.position.z <= -6.0f || gameObject.transform.position.x >= 15f || gameObject.transform.position.x <= -15f
            //    || gameObject.transform.position.y >= 20f || gameObject.transform.position.y <= -20f)
            //{
            //    Vector3 vec = gameObject.transform.position;
            //    if (gameObject.transform.position.z >= 6.0f)vec.z = 4.0f;
            //    else if (gameObject.transform.position.z <= -6.0f)vec.z = -4.0f;

            //    if (Item.GC.Gamemode_num != 1&&gameObject.transform.position.x >= 15f)vec.x = 12.0f;
            //    else if (Item.GC.Gamemode_num != 1&&gameObject.transform.position.x <= -15f)vec.x = -12.0f;

            //    if (gameObject.transform.position.x >= 20f)vec.y = 13.0f;
            //    else if (gameObject.transform.position.x <= -20f)vec.y = 1.0f;
            //    if (Item.GC.Gamemode_num != 1)
            //    {
            //        gameObject.transform.parent.transform.position = vec;
            //    }else
            //    {
            //        if (Mathf.Abs(vec.x) > 50f) Destroy(this.gameObject);
            //        else gameObject.transform.parent.transform.position = vec;
            //    }
            //}
        }
        else
        {
            self.isKinematic = true;
            self.useGravity = false;
            Item.item_collider.enabled = false;
            if (Item_num == 6 || Item_num == 7)
            {
                Item.skin.enabled = false;
            }
        }
        
    }
    
}
