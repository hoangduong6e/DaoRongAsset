using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dao : MonoBehaviour
{
    // Start is called before the first frame update
    public bool load = false;
    public void XemThanLong()
    {
        CrGame.ins.XemThanLong();
    }    
    public void XemThongTinThanLong(bool ok)
    {
        CrGame.ins.XemthongtinThanLong(ok);
    }
    public void OpenMenu(string s)
    {
        AllMenu.ins.OpenMenu(s);
    }
    public void MapVienChinh()
    {
        CrGame.ins.OpenMapPhoBan();
    }
    public void XemNangCapDao()
    {
        CrGame.ins.XemNangCapDao();
    }    
    public void XemInfoDao(bool b)
    {
        CrGame.ins.InfoDao(b);
    }
    public void OpenDaoBay()
    {
        AllMenu.ins.OpenCreateMenu(nameof(GiaoDienChuyenHoaRong));
    }
    private bool bb = false;
    public void XemThongTinDaoBay(bool b)
    {
        bb = b;
        if (bb)
        {
            if (AllMenu.ins.menu.ContainsKey(nameof(GiaoDienChuyenHoaRong))) return;
            JSONClass datasend = new JSONClass();
            datasend["class"] = "ChuyenHoaPhuongHoang";
            datasend["method"] = "XemThongTinDaoBay";
            NetworkManager.ins.SendServer(datasend, ok);
            void ok(JSONNode json)
            {
                if (AllMenu.ins.menu.ContainsKey(nameof(GiaoDienChuyenHoaRong))) return;
                if (bb)
                {
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString,3,false);
                    StartCoroutine(delay());
                    IEnumerator delay()
                    {
                        yield return new WaitUntil(() => bb == false);
                        CrGame.ins.OffThongBaoNhanh((short)json["message"].AsString.Length);
                    }
                 

                }
            }
        }
    }
}
