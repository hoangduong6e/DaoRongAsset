using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRongVienChinh : MonoBehaviour
{
    public string idRong;public GameObject DaulauChet;
    public bool hoisinh = true;
    // Start is called before the first frame update
    public void TrieuHoi()
    {
        Text txthuyentinhhoisinh = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        int sohuyentinhcan = int.Parse(txthuyentinhhoisinh.text);

        if(sohuyentinhcan > 0)
        {
            if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("itemHuyenTinh"))
            {
                if (int.Parse(NetworkManager.ins.inventory.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text) >= sohuyentinhcan)
                {
                    TH();
                }
                else CrGame.ins.OnThongBaoNhanh("Không đủ Huyền Tinh ", 1f);
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh("Không đủ Huyền Tinh ", 1f);
            }
        }
        else
        {
            TH();
            GiaoDienPVP.ins.SoDoihinh += 1;
        }

        void TH()
        {
            GetComponent<Button>().interactable = false;
            NetworkManager.ins.socket.Emit("trieuhoirong", JSONObject.CreateStringObject(idRong));
            transform.GetChild(0).gameObject.SetActive(false);// huyen tinh
            DaulauChet.SetActive(false);
            DaulauChet.transform.GetChild(0).gameObject.SetActive(true);
        }
      
    }
}
