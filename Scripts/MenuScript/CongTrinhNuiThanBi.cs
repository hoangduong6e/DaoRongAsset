using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CongTrinhNuiThanBi : MonoBehaviour
{
    // Start is called before the first frame update

    public Text txtTime;
    float time;
    NuiLua nuilua;
    //private void Start()
    //{
    //  //  txtTime.transform.SetParent(CrGame.ins.trencung.transform);
    //}

    // Update is called once per frame
    void Update()
    {
        if (!Friend.ins.QuaNha)
        {
            time = CrGame.ins.timeNuiThanBi;
            nuilua = NuiLua.Instance;
        }
        else
        {
            time = Friend.ins.timeNuiThanBi;
            nuilua = Friend.nuiluaFriend;
        }
        if (time > 0)
        {
            txtTime.text = CrGame.ParseTime(time);
        }
        else
        {
            nuilua.MauNgoc = _mauNgoc.Base;
            txtTime.text = CrGame.ParseTime(0);
        } 
            
     //   txtTime.transform.position = transform.position;
    }
}
