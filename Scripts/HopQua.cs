using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HopQua : MonoBehaviour
{
    public GameObject Coqua, menuNhanQua;
    public Text txtSoQua; NetworkManager net;
    public int coqua; public GameObject SlotQua;public Button btnNhanQua;
    public GameObject Slot; public Text txtQua; public GameObject Thongbaoconnua;
    //quafriend
    public Image[] imgQua; public Sprite imgquaDuocNhan, ImgquaKhongDuocNhan, thedanhan, thevip; Friend friend;
    public byte SoquaFriend, SoquaDanhan; public GameObject coquafriend, menuQua; public Text txtNguoiTang, txtTenQua, TxtSoLuongQua;
    public Image imgQuaFriend; public string[] boolqua = new string[7];CrGame crgame;
    public bool hienlientiep = true;
    public static HopQua ins;
    // Start is called before the first frame update
    void Awake()
    {
        net = GameObject.Find("Main Camera").GetComponent<NetworkManager>();
        crgame = net.GetComponent<CrGame>();
        friend = net.GetComponent<Friend>();
        ins = this;
    }
    public void LoadQuaFriend()
    {
        if (SoquaFriend > 0)
        {
            coquafriend.SetActive(true);
            Text txtsoqua = coquafriend.transform.GetChild(0).GetComponent<Text>();
            txtsoqua.text = SoquaFriend.ToString();
        }
        else coquafriend.SetActive(false);
        for (int i = 0; i < 7; i++)
        {
            if (i < SoquaDanhan)
            {
                imgQua[i].sprite = ImgquaKhongDuocNhan;
                imgQua[i].GetComponent<Button>().enabled = false;
                imgQua[i].transform.GetChild(0).gameObject.SetActive(true);
                imgQua[i].transform.GetChild(0).GetComponent<Image>().sprite = thedanhan;
                //boolqua[i] = true;
                boolqua[i] = "danhan";
            }
            else
            {
                if (i < SoquaDanhan + SoquaFriend)
                {
                    imgQua[i].sprite = imgquaDuocNhan;
                    imgQua[i].GetComponent<Button>().enabled = true;
                    //boolqua[i] = false;
                    boolqua[i] = "coqua";
                }
            }
        }
    }
    sbyte indexqua = -1;
    public void XemQuaFriend(int i)
    {
        AudioManager.SoundClick();
        indexqua = -1;
        friend.quaxem = i;
        for (int j = 0; j <= i; j++)
        {
            // if (boolqua[j] == false)
            if (boolqua[j] == "coqua")
            {
                indexqua += 1;
            }
        }
        friend.quanhan = indexqua;
        debug.Log("Xem qua "+ indexqua);
        net.socket.Emit("xemquafriend", JSONObject.CreateStringObject(indexqua.ToString()));
    }
    #region QuaLenCap
    public void ThemQua(int soqua)
    {
        coqua = soqua;
        if (coqua > 0)
        {
            txtSoQua.text = "" + soqua;
            Coqua.SetActive(true);
        }
        else
        {
            Coqua.SetActive(false);
            CloseMenu();
        }
    }
    public void Them1Qua()
    {
        coqua += 1;
        if (coqua > 0)
        {
            txtSoQua.text = "" + coqua;
            Coqua.SetActive(true);
        }
        else
        {
            Coqua.SetActive(false);
            CloseMenu();
        }
    }
    public void XemQua()
    {
        if (coqua > 0)
        {
            net.socket.Emit("xemqua");
            // menuNhanQua.SetActive(true);
        }
    }
    public void NhanQua()
    {
        //   crgame.OnThongBao(true,"Đợi chút...");
        btnNhanQua.interactable = false;
        net.socket.Emit("nhanqua");
        Thongbaoconnua.SetActive(false);
    }
    public void XoaQua()
    {
        StartCoroutine(Xoa());
        IEnumerator Xoa()
        {
            crgame.OnThongBao(true,"Đang Hủy...",false);
            UnityWebRequest www = new UnityWebRequest(crgame.ServerName + "XoaQuaFriend/indexqua/" + indexqua + "/taikhoan/" + LoginFacebook.ins.id);
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
                if (www.downloadHandler.text == "0")
                {

                    boolqua[friend.quaxem] = "";
                    imgQua[friend.quaxem].sprite = ImgquaKhongDuocNhan;
                    imgQua[friend.quaxem].GetComponent<Button>().enabled = false;
                    //imgQua[friend.quaxem].transform.GetChild(0).gameObject.SetActive(true);
                   // imgQua[friend.quaxem].transform.GetChild(0).GetComponent<Image>().sprite = hopqua.thedanhan;
                    SoquaFriend -= 1;
                    if (SoquaFriend > 0)
                    {
                        coquafriend.SetActive(true);
                        Text txtsoqua = coquafriend.transform.GetChild(0).GetComponent<Text>();
                        txtsoqua.text = SoquaFriend.ToString();
                    }
                    else
                    {
                        coquafriend.SetActive(false);
                    }
                    crgame.OnThongBao(false);
                }
                else
                {
                    crgame.OnThongBao(true,"Lỗi",true);
                }
                menuQua.SetActive(false);
            }
        }
    }
    public void CloseMenu()
    {
        for (int i = 1; i < Slot.transform.childCount; i++)
        {
            Destroy(Slot.transform.GetChild(i).gameObject);
        }
        menuNhanQua.SetActive(false);
        Thongbaoconnua.SetActive(false);
    }
    public void AddSlotQua(string name, string soluong, string nametv, bool itemrong = false, int tienhoa = 0)
    {
        GameObject slot = Instantiate(SlotQua, transform.position, Quaternion.identity) as GameObject;
        slot.transform.SetParent(Slot.transform, false);
        Image img = slot.transform.GetChild(0).GetComponent<Image>();
        QuaBay quabay = slot.GetComponent<QuaBay>();
        switch (name)
        {
            case "Vang":
                if(GameObject.Find("TienVang")) quabay.vitribay = GameObject.Find("TienVang");
                break;
            default:
                if (GameObject.Find("Giaodien")) quabay.vitribay = GameObject.Find("Giaodien").transform.GetChild(0).gameObject;
                break;
        }
        if (itemrong == false)
        {
            //SpriteRenderer spriteItem = GameObject.Find("Sprite" + name).GetComponent<SpriteRenderer>();
            Sprite spriteItem = Inventory.LoadSprite(name);//GameObject.Find(name).GetComponent<Image>();
            img.sprite = spriteItem;
        }
        else
        {
          //  debug.Log(tienhoa);
          //  SpriteRong spriterong = GameObject.Find("Sprite" + name).GetComponent<SpriteRong>();
           // if (spriterong.spriteTienHoa[tienhoa - 1] == null && tienhoa >= 2)
          //  {
                img.sprite = Inventory.LoadSpriteRong(name + tienhoa);//spriterong.spriteTienHoa[1];

           // }
           // else
           // {
              //  img.sprite = spriterong.spriteTienHoa[tienhoa - 1];
          //  }
        }
        slot.name = "Qua" + name;
        img.SetNativeSize();
        Text txtNameItem = slot.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        Text txtSoluongItem = slot.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        txtNameItem.text = nametv;
        txtSoluongItem.text = soluong;
        slot.SetActive(true);
    }
    #endregion
}
