using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class QuayRong : MonoBehaviour
{
    // Start is called before the first frame update
    ScrollRect scrollRect;VongQuayRong vongquayrong;public GameObject RongQuayDuoc;
    bool destroy = false;bool quay = true;public Image imgRongQuay;QuaBay quabay;bool bay = false;
    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        vongquayrong = AllMenu.ins.menu["MenuQuayRong"].GetComponent<VongQuayRong>();
    }
    bool DuocQuay = false;
    private void OnEnable()
    {
        //  RongQuayDuoc = GameObject.Find("OitemRongDatLua");
        var pos = new Vector3(0f, 0, 0);
        scrollRect.content.localPosition = pos;
        Invoke("BatDauDung", 5f);
    }
    private void OnDisable()
    {
        CancelInvoke("duocbay");
        DuocQuay = false;
        destroy = false;
        quay = true; bay = false;
    }
    // Update is called once per frame OitemRongCayBang
    void Update()
    {
        //   debug.Log(RongQuayDuoc.transform.position.y);
        //debug.Log(RongQuayDuoc.name);
        if(RongQuayDuoc != null)
        {
            if (RongQuayDuoc.transform.position.y >= 0 && RongQuayDuoc.transform.position.y <= 0.1f && DuocQuay)
            {
                if (destroy == false)
                {
                    quay = false;
                 //   var pos = new Vector3(0f, 0, 0);
                    //scrollRect.content.localPosition = pos;
                    UI_InfiniteScroll ui = GetComponent<UI_InfiniteScroll>();
                    Destroy(ui);
                    destroy = true;
                }
                imgRongQuay.transform.SetParent(GameObject.Find("Canvasmenu").transform);
                //Invoke("duocbay", 0.5f);
                //if (bay)
                //{
                 
                //}
                vongquayrong.soquay -= 1;
                if (vongquayrong.soquay == 0)
                {
                    vongquayrong.btnExit.interactable = true;
                }
                quabay = imgRongQuay.GetComponent<QuaBay>(); quabay.enabled = true;
                QuayRong quayrong = GetComponent<QuayRong>(); quayrong.enabled = false;
            }
        }
        if(quay)
        {
            // var pos = new Vector2(0f, Mathf.Sin(Time.time * 0.004f) * 200000f);
            //    scrollRect.content.localPosition = pos;
            Move();
        }
    
    }
    void duocbay()
    {
        bay = true;
    }
    void BatDauDung()
    {
        DuocQuay = true;
    }
    private void Move()
    {
        Vector2 contentPosition = scrollRect.content.position;
        Vector2 newPosition = new Vector2(contentPosition.x, contentPosition.y + 16f * Time.deltaTime);
        scrollRect.content.position = newPosition;
    }
}
