using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class VongTronCongTrinh : MonoBehaviour
{
    // Start is called before the first frame update
    public void NangCap()
    {
        CrGame.ins.OpenmenuUpcongtrinh();
    }
    public void info()
    {
        CrGame.ins.XeminfoCongtrinh();
    }
    public void XemBanCongTrinh()
    {
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung")).GetComponent<ThongBaoChon>();
        tbc.txtThongBao.text = "Bạn muốn bán công trình này với giá 1000 vàng ?";
        tbc.btnChon.onClick.AddListener(BanCongTrinh);
    }
    public void BanCongTrinh()
    {
        CongTrinh congtrinh = CrGame.ins.VungCongTrinh.GetComponent<CongTrinh>();
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "BanCongTrinh";
        datasend["data"]["idcongtrinh"] = congtrinh.idCongtrinh.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                congtrinh.ResetCongTrinh();
                //   AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                AllMenu.ins.menu["VongTronCongtrinh"].SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].Value, 2);
                // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
            }
        }
        
    }
    bool busua = true;
    public void BuSua(string nameThanLong)
    {
        if (!busua) return;
        busua = false;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "ThanLongBuSua";
        datasend["data"]["name"] = nameThanLong;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].Value == "ok")
            {
                GameObject BeThanLong = CrGame.ins.AllDao.transform.GetChild(2).transform.Find("Be" + nameThanLong).gameObject;
                Animator anim = BeThanLong.transform.GetChild(0).GetComponent<Animator>();
                anim.SetBool("Doi", false);
                BeThanLong.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["status"].Value, 2);
            }
            AllMenu.ins.DestroyMenu("VongTronThanLong");
            busua = true;
        }
    }    
    public void XemNangCapThanLong(string nameThanLong)
    {
        CrGame.ins.XemNangCapCongTrinh("",0, nameThanLong);
    }
   public void DestroyGD()
    {
        AllMenu.ins.DestroyMenu("VongTronCongtrinh");
    }
}
