using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NhiemVu : MonoBehaviour
{
    public GameObject ContentNVHangNgay,ContentNvRong,ContentNvExp;
    public GameObject QuaNhanRong, QuaNhanHangNgay;
    // Start is called before the first frame update
    public void UpdateNhiemVu(string namenv,string sonhiemvu,string nhiemvu)
    {
        GameObject allnv = null;
        if (nhiemvu == "nhiemvuhangngay") allnv = ContentNVHangNgay;
        else if (nhiemvu == "nhiemvurong") allnv = ContentNvRong;
        else if (nhiemvu == "nhiemvuexp") allnv = ContentNvExp;
        if (namenv != "Khoa")
        {
            for (int i = 0; i < allnv.transform.childCount; i++)
            {
                if (allnv.transform.GetChild(i).name == namenv)
                {
                    string[] cat = sonhiemvu.Split('/');
                    if (int.Parse(cat[0]) >= int.Parse(cat[1]))
                    {
                        allnv.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
                        if (nhiemvu == "nhiemvuexp")
                        {
                            allnv.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);
                        }    
                    }
                    Text txtsonhiemvu = allnv.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
                    txtsonhiemvu.text = sonhiemvu;
                    break;
                }
            }
        }
        else
        {
            if (nhiemvu == "nhiemvuhangngay") QuaNhanHangNgay.SetActive(false);
            else if (nhiemvu == "nhiemvurong") QuaNhanRong.SetActive(false);
            //allnv.transform.GetChild(allnv.transform.childCount - 1).gameObject.SetActive(true);
            GameObject khoa = allnv.transform.GetChild(allnv.transform.childCount - 1).gameObject;
            khoa.SetActive(true);
            if(sonhiemvu != "Khoa")
            {
                khoa.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = sonhiemvu;
                khoa.transform.GetChild(2).GetComponent<Text>().text = "Nhiệm vụ đang khóa, bạn hãy mở khóa nhé";
            }    
            else
            {
                khoa.transform.GetChild(1).gameObject.SetActive(false);
                khoa.transform.GetChild(2).GetComponent<Text>().text = "Đã hoàn thành hết nhiệm vụ trong ngày";
            }
        }
    }
    public void MoKhoaNhiemVu(string nv)
    {
        NetworkManager.ins.socket.Emit("MoKhoaNhiemVu",JSONObject.CreateStringObject(nv));
    }    
    public void AddinfoNhiemVuHangNgay(int i,string nv,string sonv,string keynv)
    {
        Text txtnv = ContentNVHangNgay.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>();
        txtnv.text = nv;
        Text txtsonhiemvu = ContentNVHangNgay.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        txtsonhiemvu.text = sonv;
        ContentNVHangNgay.transform.GetChild(i).name = keynv;
        string[] cat = sonv.Split('/');
        if (int.Parse(cat[0]) >= int.Parse(cat[1]))
        {
            ContentNVHangNgay.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
        }
    }
    public void AddinfoNhiemVuRong(int i, string nv, string sonv,string keynv)
    {
        Text txtnv = ContentNvRong.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>();
        txtnv.text = nv;
        Text txtsonhiemvu = ContentNvRong.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        txtsonhiemvu.text = sonv;
        ContentNvRong.transform.GetChild(i).name = keynv;
        string[] cat = sonv.Split('/');
        if (int.Parse(cat[0]) >= int.Parse(cat[1]))
        {
            ContentNvRong.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
        }
    }
    public void AddinfoNhiemVuExp(string nv, string sonv, string keynv)
    {
        debug.Log(nv);
        for (int i = 0; i < ContentNvExp.transform.childCount; i++)
        {
            if(ContentNvExp.transform.GetChild(i).name == keynv)
            {
                ContentNvExp.transform.GetChild(i).gameObject.SetActive(true);
                Text txtnv = ContentNvExp.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>();
                txtnv.text = nv;
                Text txtsonhiemvu = ContentNvExp.transform.GetChild(i).transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
                txtsonhiemvu.text = sonv;
                //ContentNvRong.transform.GetChild(i).name = keynv;
                string[] cat = sonv.Split('/');
                if (int.Parse(cat[0]) >= int.Parse(cat[1]))
                {
                    ContentNvExp.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(true);
                    ContentNvExp.transform.GetChild(i).transform.GetChild(4).gameObject.SetActive(false);
                }
                break;
            }
        }
    }
    public void XemNhiemVuHangNgay()
    {
        ContentNVHangNgay.SetActive(true);
        ContentNvRong.SetActive(false);
        ContentNvExp.SetActive(false);
    }    
    public void XemNhiemVuRong()
    {
        ContentNVHangNgay.SetActive(false);
        ContentNvRong.SetActive(true);
        ContentNvExp.SetActive(false);
    }
    public void XemNhiemVuExp()
    {
        ContentNVHangNgay.SetActive(false);
        ContentNvRong.SetActive(false);
        ContentNvExp.SetActive(true);
    }
    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }
}
