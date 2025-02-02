using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFly : MonoBehaviour
{
    private bool duocClick = false, fly = false;
    private void OnEnable()
    {
        EventManager.StartDelay2(() => {

            duocClick = true;

            EventManager.StartDelay2(() => {
                if(!fly)
                {
                    Fly();
                }
            }, 3f);
        },1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        if (!duocClick) return;
        if (fly) return;
        fly = true;
        Fly();

    }
    private void Fly()
    {
        GameObject hopqua = GameObject.FindGameObjectWithTag("hopqua");
                  hopqua.GetComponent<HopQua>().ThemQua(1);
        transform.LeanMove(hopqua.transform.position, 1f).setOnComplete(() => {
  
            Destroy(gameObject);
            
        });
    }

    public static GameObject CreateItemFly(Sprite sprite,Vector3 vec = new Vector3())
    {
        GameObject ins = Instantiate(Inventory.ins.GetObj("ItemFly"), vec, Quaternion.identity, CrGame.ins.trencung);
        if(sprite != null)
        {
            Image img = ins.transform.GetChild(1).GetComponent<Image>();
            img.sprite = sprite;
            img.SetNativeSize();
            img.Resize(85);
        }
        return ins;
    }
}
