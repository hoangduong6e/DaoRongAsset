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
        StartCoroutine(BanCt());
        IEnumerator BanCt()
        {
            CongTrinh congtrinh = CrGame.ins.VungCongTrinh.GetComponent<CongTrinh>();
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "BanCongTrinh/idcongtrinh/"+ congtrinh.idCongtrinh + "/taikhoan/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                if(www.downloadHandler.text == "0")
                {
                    congtrinh.ResetCongTrinh();
                 //   AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                    AllMenu.ins.menu["VongTronCongtrinh"].SetActive(false);
                }
                else
                {
                    CrGame.ins.OnThongBao(true, www.downloadHandler.text,true);
                   // AllMenu.ins.menu["MenuXacNhan"].SetActive(false);
                }
            }
        }
    }
    bool busua = true;
    public void BuSua(string nameThanLong)
    {
        if (!busua) return;
        busua = false;
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ThanLongBuSua/taikhoan/" + LoginFacebook.ins.id + "/name/" + nameThanLong);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.OnThongBaoNhanh("Lỗi!", 2);
                AllMenu.ins.DestroyMenu("VongTronThanLong");
                busua = true;
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject BeThanLong = CrGame.ins.AllDao.transform.GetChild(2).transform.Find("Be"+ nameThanLong).gameObject;
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
    }    
    public void XemNangCapThanLong(string nameThanLong)
    {
        StartCoroutine(CrGame.ins.XemNangCapCongTrinh("",0, nameThanLong));
    }
   public void DestroyGD()
    {
        AllMenu.ins.DestroyMenu("VongTronCongtrinh");
    }
}
