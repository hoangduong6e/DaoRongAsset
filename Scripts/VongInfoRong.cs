using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class VongInfoRong : MonoBehaviour
{
    void Update()
    {
        transform.position = CrGame.ins.TfrongInfo.transform.position;
    }
    public void XemKhamNgoc()
    {
       DragonIslandManager.XemKhamNgoc(CrGame.ins.TfrongInfo.gameObject.name, CrGame.ins.DangODao.ToString());
        DestroyGD();
    }
    public void PhongChienTuong()
    {
        DragonIslandManager.PhongChienTuong(CrGame.ins.TfrongInfo.gameObject.name);
    }
    public void XemInfoRong()
    {
        //DragonController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonController>();
        //NetworkManager.ins.socket.Emit("xeminforong", JSONObject.CreateStringObject(dra.name));

        DragonIslandManager.XemInfoRong(CrGame.ins.TfrongInfo.gameObject.name, CrGame.ins.DangODao.ToString());
    }
    public void CatRong()
    {
        //DragonController dra = CrGame.ins.TfrongInfo.gameObject.GetComponent<DragonController>();
        //NetworkManager.ins.socket.Emit("CatRong", JSONObject.CreateStringObject(dra.name));//name la id
        DragonIslandManager.CatRong(CrGame.ins.TfrongInfo.gameObject.name, CrGame.ins.DangODao.ToString());
    }
    public void XemTenRong()
    {
        DragonIslandManager.XemDoiTenRong(CrGame.ins.TfrongInfo.gameObject.name, CrGame.ins.DangODao.ToString()); 
        DestroyGD();
    }
    public void DestroyGD()
    {
        AllMenu.ins.DestroyMenu("MenuVongtronXemRong");
    }
}
