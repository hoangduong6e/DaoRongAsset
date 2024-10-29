using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnBanRong : MonoBehaviour
{
    // Start is called before the first frame update
    bool drag = false;
    Vector3 mousePosition;
    public GameObject muiten;
    Inventory inventory;
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        if (drag)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - muiten.transform.position;
            mousePosition.x -= 1f;
            mousePosition.z = 0;
            muiten.transform.Translate(mousePosition);
        }
    }
    public void Drag(bool b)
    {
        drag = b;
        muiten.SetActive(b);
        if(b == false)
        {
            int childcount = inventory.TuiRong.transform.childCount;
            for (int i = (inventory.trangtuirong - 1) * 12; i < inventory.trangtuirong * 12; i++)
            {
                if(i < childcount)
                {
                    if (Mathf.Abs(muiten.transform.position.x - inventory.TuiRong.transform.GetChild(i).transform.position.x) <= 1.5f &&
                      Mathf.Abs(muiten.transform.position.y - inventory.TuiRong.transform.GetChild(i).transform.position.y) <= 1.5f)
                    {
                        if (inventory.TuiRong.transform.GetChild(i).transform.childCount > 0)
                        {
                           // debug.Log(inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).name);
                            ItemDragon idra = inventory.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                            inventory.XemRongBan(idra.nameObjectDragon, idra.txtSao.text, idra.name,idra.transform.GetChild(0).GetComponent<Image>());
                            break;
                        }
                    }
                }
                else break;
            }
        }    
    }
}
