using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class BXHLevel : MonoBehaviour
{
    int trang = 1;
    public Sprite Top1, Top2, Top3, Top;
    private string tocchon = "Sam";
    public void SwichBxh()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        XemBXH();
        tocchon = btnchon.name;
    }    
    public void XemBXH()
    {
        AudioManager.PlaySound("soundClick");
        JSONClass datasend = new JSONClass();
        datasend["class"] = "LanSu";
        datasend["method"] = "SwichBxhLevel";
        datasend["data"]["trang"] = trang.ToString();
        datasend["data"]["toc"] = tocchon;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                GameObject menu = AllMenu.ins.GetCreateMenu("BxhLevel", CrGame.ins.trencung.gameObject);
                //GameObject menu = CrGame.ins.trencung.transform.Find("MenuTop").gameObject;
                //    menuevent["MenuTop"] = menu;
                // GameObject menu = GetCreateMenu("MenuTop",gameObject,false,3);
                GameObject menutop = menu.transform.GetChild(1).transform.Find("MenuTop").gameObject;
                if (json["alltop"].Count > 0)
                {
                    GameObject contentop = menutop.transform.GetChild(0).gameObject;
                    for (int i = 0; i < 10; i++)
                    {
                        contentop.transform.GetChild(i).gameObject.SetActive(false);
                        if (i < json["alltop"].Count)
                        {
                            // debug.Log(json["alltop"][i]["Name"].Value);
                            Image imgAvatar = contentop.transform.GetChild(i).transform.GetChild(1).GetComponent<Image>();
                            Image imgKhungAvatar = contentop.transform.GetChild(i).transform.GetChild(2).GetComponent<Image>(); imgKhungAvatar.name = json["alltop"][i]["idobject"].AsString;
                            //   debug.Log("ok1");
                            Image HuyHieu = contentop.transform.GetChild(i).transform.GetChild(3).GetComponent<Image>();
                            Text txtName = contentop.transform.GetChild(i).transform.GetChild(4).GetComponent<Text>();
                            //   debug.Log("ok2");
                            Text txtTop = contentop.transform.GetChild(i).transform.GetChild(5).GetComponent<Text>();
                            int sotop = json["alltop"][i]["top"].AsInt;
                            txtTop.text = sotop.ToString();
                            //   debug.Log("ok3");
                            NetworkManager.ins.friend.LoadAvtFriend(json["alltop"][i]["idfb"].Value, imgAvatar, imgKhungAvatar);
                            // imgKhungAvatar.sprite = Inventory.LoadSprite("Avatar" + json["alltop"][i]["Toc"].Value);
                            contentop.transform.GetChild(i).gameObject.SetActive(true);
                            debug.Log("ok4");
                            if (sotop > 3) HuyHieu.sprite = Top;
                            else if (sotop == 1) HuyHieu.sprite = Top1;
                            else if (sotop == 2) HuyHieu.sprite = Top2;
                            else if (sotop == 3) HuyHieu.sprite = Top3;
                            HuyHieu.SetNativeSize();
                            //  debug.Log("ok5");
                            txtName.text = json["alltop"][i]["Name"].Value;
                            Text txttimee = contentop.transform.GetChild(i).transform.GetChild(6).GetComponent<Text>(); txttimee.text =  "Lv." + json["alltop"][i]["level"].AsString;
                            // debug.Log("ok5.1");

                            contentop.transform.GetChild(i).gameObject.SetActive(true);

                            //   debug.Log("ok5.2");
                        }

                        //  contentop.transform.GetChild(i).transform.SetParent(imgtop.transform, false);
                        // CrGame.ins.OnThongBao(false);
                        //AllTop.SetActive(true);S
                        // txtTrang.text = trang + "/100";
                    }
                }
                else CrGame.ins.OnThongBaoNhanh("Chưa có xếp hạng");

                menu.SetActive(true);

                string nametoc = tocchon;
                string toc = GiaoDienLanSu.GetStrToc(nametoc);
                Text txtTenBxh = transform.GetChild(0).transform.GetChild(1).transform.Find("txtTenBxh").GetComponent<Text>();
                txtTenBxh.text = "Bảng Xếp Hạng Level Tộc " + toc;
            }
            else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
        }
    }
    public void sangtrangtop(int i)
    {
        AudioManager.PlaySound("soundClick");
        if (trang + i >= 1) trang += i;
        else return;
        XemBXH();
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuTop");
        trang = 1;
    }    

}
