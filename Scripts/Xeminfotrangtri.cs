using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.EventSystems;

public class Xeminfotrangtri : MonoBehaviour
{
    short length = 0;
    bool B;
    public void XemInfoTrangTri(bool b)
    {
        if (Friend.ins.QuaNha) return;
        B = b;
        if (!B)
        {
            CrGame.ins.OffThongBaoNhanh(length);
            return;
        }
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "XemItemTrangTri";
        datasend["data"]["nametrangtri"] = btnchon.transform.parent.name;
        datasend["data"]["dao"] =CrGame.ins.DangODao.ToString();
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                if (B)
                {
                    length = (short)json["message"].AsString.Length;
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2, false);
                }

            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
    }
}
