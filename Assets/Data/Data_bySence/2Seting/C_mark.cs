using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class C_mark : MonoBehaviour
{
    public GameObject chioceobj;
    public Image self;
    public GameObject self_obj;
    public void setColor(float r, float g, float b)
    {
        self.color = new Color(r, g, b, 1.0f);
    }
    public void chioceanim(bool cho)
    {
        chioceobj.SetActive(cho);
    }
    public void delete_this()
    {
        Destroy(this.gameObject);
    }
    
}
