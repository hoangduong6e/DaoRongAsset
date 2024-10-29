using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQuaNapDau : MonoBehaviour
{
    public void OpenMenuNap()
    {
        AllMenu.ins.GetCreateMenu("MenuNapThe",null,true,transform.GetSiblingIndex() + 1);
    }    
    public void DonateBank()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Chuyển hướng đến Fanpage, bạn vui lòng inbox để biết cách thức donate qua Bank/Momo.";
        tbc.btnChon.onClick.AddListener(CrGame.ins.LinkFanpage);
    }    
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuQuaNapDau");
    }    
}
