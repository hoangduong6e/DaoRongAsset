using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuTrieuHoiHacLong : MonoBehaviour
{
    // Start is called before the first frame update
    public RuntimeAnimatorController animOVangSang;
    void Start()
    {
        GameObject g = transform.GetChild(0).gameObject;
        GameObject allO = g.transform.Find("allO").gameObject;
        GameObject AllManh = g.transform.Find("AllManh").gameObject;
        for (int i = 0; i < AllManh.transform.childCount; i++)
        {
            Image img = AllManh.transform.GetChild(i).GetComponent<Image>();
            string namee = allO.transform.GetChild(i).name;
           // debug.Log(namee);
            if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + namee))
            {
                img.sprite = Inventory.LoadSprite(namee);
                allO.transform.GetChild(i).GetComponent<Animator>().runtimeAnimatorController = animOVangSang;
            }
        }
    }
    public void ExitMenu()
    {
        AllMenu.ins.DestroyMenu("MenuTrieuHoiHacLong");
    }    
}
