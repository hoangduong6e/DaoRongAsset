using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VongQuayMayMan : MonoBehaviour
{
    // Start is called before the first frame update
 //   XLua.LuaEnv luaEnv;

    float[] tiaSetZ = new float[] {180f,137f,90f,45f,0f,-45f,-90f,-135f};
    public GameObject TiaSet,VienVang,VienDo;public Sprite spriteVongQuayThuong, spriteVongQuayVip;
    public Sprite[] spritetuchat;
    string vongquayy = "vongquaythuong";
    bool load = true;
    private void OnEnable()
    {
        if (!load) return;
        CrGame.ins.GetComponent<ZoomCamera>().enabled = false;
        LoadVongQuay("vongquaythuong");
        //luaEnv = new XLua.LuaEnv();
        //luaEnv.DoString(@"
        //  a = 10;
        //  print(a+5)
        //");
    }
    private void OnDestroy()
    {
        CrGame.ins.GetComponent<ZoomCamera>().enabled = true;
    }
    public void ExitMenu()
    {
        if (!duocquay) return;
        AllMenu.ins.DestroyMenu("MenuVongQuayMayMan");
    }    
    public void ChuyenDoiVongQuay()
    {
        load = true;
        if (vongquayy == "vongquaythuong")
        {
            LoadVongQuay("vongquayvip");
        }   
        else
        {
            LoadVongQuay("vongquaythuong");
        }
    }
    public void NhanVeQuayVip()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "NhanVeQuayVipTichLuy/id/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            }
            else
            {
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);

                debug.Log(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject g = transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject objluottichluy = g.transform.GetChild(1).gameObject;
                    GameObject objallvequay = g.transform.GetChild(2).gameObject;
                    objluottichluy.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    objallvequay.transform.GetChild(3).GetComponent<Text>().text = int.Parse(objluottichluy.transform.GetChild(2).GetComponent<Text>().text) + int.Parse(objallvequay.transform.GetChild(3).GetComponent<Text>().text) + "";
                    objluottichluy.transform.GetChild(2).GetComponent<Text>().text = "0";
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    public void ResetVongQuay()
    {
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "ResetVongQuay/id/" + LoginFacebook.ins.id + "/vongquay/" + vongquayy);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
            }
            else
            {
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);

                debug.Log(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject g = transform.GetChild(0).transform.GetChild(0).gameObject;
                    LoadVq(json["vongquay"]);
                    g.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = json["thongtinreset"].Value;
                }
                else CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
    }
    void LoadVq(JSONNode json)
    {
        GameObject g = transform.GetChild(0).transform.GetChild(0).gameObject;
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject AlloQuay = g.transform.GetChild(0).gameObject;

        for (int i = 0; i < AlloQuay.transform.childCount; i++)
        {
            if (AlloQuay.transform.GetChild(i).transform.Find("vien")) Destroy(AlloQuay.transform.GetChild(i).transform.Find("vien").gameObject);
            if (json[i]["loai"].Value == "item" || json[i]["loai"].Value == "ngoc")
            {
                AlloQuay.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(json[i]["name"].Value);
                AlloQuay.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = json[i]["txtsoluong"].Value;
                AlloQuay.transform.GetChild(i).name = "item" + json[i]["name"].Value;
                AlloQuay.transform.GetChild(i).GetComponent<infoitem>().enabled = true;
            }
            else if (json[i]["loai"].Value == "rong")
            {
                AlloQuay.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(json[i]["name"].Value + "1");
                AlloQuay.transform.GetChild(i).transform.GetChild(1).GetComponent<Text>().text = json[i]["sao"].Value + "sao";
                AlloQuay.transform.GetChild(i).GetComponent<infoitem>().enabled = false;
            }
            if (json[i]["tuchat"].Value == "do")
            {
                GameObject vien = Instantiate(VienDo, transform.position, Quaternion.identity);
                vien.transform.SetParent(AlloQuay.transform.GetChild(i).transform, false);
                vien.transform.position = AlloQuay.transform.GetChild(i).transform.position;
                vien.transform.localScale = new Vector3(1.5f, 1.5f);
                vien.name = "vien";
                vien.SetActive(true);
            }
            else if (json[i]["tuchat"].Value == "vang")
            {
                GameObject vien = Instantiate(VienVang, transform.position, Quaternion.identity);
                vien.transform.SetParent(AlloQuay.transform.GetChild(i).transform, false);
                vien.transform.position = AlloQuay.transform.GetChild(i).transform.position;
                vien.transform.localScale = new Vector3(1.5f, 1.5f);
                vien.name = "vien";
                vien.SetActive(true);
            }
            if (json[i]["khoa"].Value == "khoa")
            {
                AlloQuay.transform.GetChild(i).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                AlloQuay.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                // AlloQuay.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                AlloQuay.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                AlloQuay.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                // AlloQuay.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
            }
            for (int j = 0; j < spritetuchat.Length; j++)
            {
                if (spritetuchat[j].name == json[i]["tuchat"].Value)
                {
                    AlloQuay.transform.GetChild(i).GetComponent<Image>().sprite = spritetuchat[j];
                    break;
                }
            }
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void LoadVongQuay(string vongquay)
    {
        vongquayy = vongquay;
        GameObject g = transform.GetChild(0).transform.GetChild(0).gameObject;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Get());
        IEnumerator Get()
        {
            //  CrGame.ins.OnThongBao(true, "Đang tải...", false);
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetVongQuayMayMan/id/" + LoginFacebook.ins.id + "/vongquay/" + vongquay);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                //gameObject.SetActive(true);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu!");
                AllMenu.ins.DestroyMenu("MenuVongQuayMayMan");
            }
            else
            {
                // Show results as text 
                debug.Log(www.downloadHandler.text);
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    LoadVq(json["data"]["vongquay"][vongquay]);
                    GameObject objluottichluy = g.transform.GetChild(1).gameObject;
                    objluottichluy.transform.GetChild(0).GetComponent<Text>().text = "Lượt tích lũy\n" + json["data"]["demnhanvequay"].Value + "/50";

                    objluottichluy.transform.GetChild(2).GetComponent<Text>().text = json["data"]["VeQuayVipChuaNhan"].Value;
                    //debug.Log("okkkkkkk");
                    if (json["data"]["VeQuayVipChuaNhan"].Value == "0")
                    {
                        objluottichluy.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    }
                    else objluottichluy.transform.GetChild(3).GetComponent<Button>().interactable = true;

                    GameObject objallvequay = g.transform.GetChild(2).gameObject;


                    if (Inventory.ins.ListItemThuong.ContainsKey("itemVeQuayThuong"))
                    {
                        objallvequay.transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemVeQuayThuong"].transform.GetChild(0).GetComponent<Text>().text;
                    }
                    else objallvequay.transform.GetChild(1).GetComponent<Text>().text = "0";

                    if (Inventory.ins.ListItemThuong.ContainsKey("itemVeQuayVip"))
                    {
                        objallvequay.transform.GetChild(3).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemVeQuayVip"].transform.GetChild(0).GetComponent<Text>().text;
                    }
                    else objallvequay.transform.GetChild(3).GetComponent<Text>().text = "0";

                    g.transform.GetChild(4).transform.GetChild(1).GetComponent<Text>().text = json["data"]["thongtinreset"].Value;

                    Image imgvongquay = g.GetComponent<Image>();
                    if (vongquay == "vongquaythuong")
                    {
                        imgvongquay.sprite = spriteVongQuayThuong;
                        g.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["data"]["luottichluy"].Value;
                        objluottichluy.transform.GetChild(7).GetComponent<Image>().sprite = objallvequay.transform.GetChild(0).GetComponent<Image>().sprite;
                        objluottichluy.transform.GetChild(8).GetComponent<Image>().sprite = objallvequay.transform.GetChild(0).GetComponent<Image>().sprite;
                    }
                    else
                    {
                        imgvongquay.sprite = spriteVongQuayVip;
                        g.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["data"]["luottichluyvip"].Value;
                        objluottichluy.transform.GetChild(7).GetComponent<Image>().sprite = objallvequay.transform.GetChild(2).GetComponent<Image>().sprite;
                        objluottichluy.transform.GetChild(8).GetComponent<Image>().sprite = objallvequay.transform.GetChild(2).GetComponent<Image>().sprite;
                    }
                    transform.GetChild(0).gameObject.SetActive(true);
                    load = false;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
 
    }
    bool duocquay = true;
    public void Quay(string solanquay)
    {
        if (!duocquay) return;
        duocquay = false;
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "QuayVongQuayMayMan/id/" + LoginFacebook.ins.id + "/vongquay/" + vongquayy + "/quayx/" + solanquay);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu!");
                duocquay = true;
            }
            else
            {
                // Show results as text
                JSONNode json = JSON.Parse(www.downloadHandler.text);

                debug.Log(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    GameObject g = transform.GetChild(0).transform.GetChild(0).gameObject;
                    GameObject objluottichluy = g.transform.GetChild(1).gameObject;
                    objluottichluy.transform.GetChild(0).GetComponent<Text>().text = "Lượt tích lũy\n" + json["demnhanvequay"].Value + "/50";

                    objluottichluy.transform.GetChild(2).GetComponent<Text>().text = json["VeQuayVipChuaNhan"].Value;

                    int tru = 1;
                    string svequay = "itemVeQuayThuong";
                    string svip = "luottichluy";
                    int vitri = 1;
                    //debug.Log("okkkkkkk");
                    if (json["VeQuayVipChuaNhan"].Value == "0")
                    {
                        objluottichluy.transform.GetChild(3).GetComponent<Button>().interactable = false;
                    }
                    else objluottichluy.transform.GetChild(3).GetComponent<Button>().interactable = true;
                    GameObject objallvequay = g.transform.GetChild(2).gameObject;
                    if (vongquayy == "vongquaythuong")
                    {
                        //g.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json["luottichluy"].Value;
                        //objallvequay.transform.GetChild(1).GetComponent<Text>().text = Inventory.ins.ListItemThuong["itemVeQuayThuong"].transform.GetChild(0).GetComponent<Text>().text;
                    }
                    else
                    {

                        svequay = "itemVeQuayVip";
                        svip = "luottichluyvip";
                        vitri = 3;
                    }
                    GameObject alloquay = g.transform.GetChild(0).gameObject;
                    if (solanquay == "X1")
                    {
                        int vitrii = int.Parse(json["vitriquay"][0].Value);
                        Vector3 vec = new Vector3(TiaSet.transform.rotation.x, TiaSet.transform.rotation.y, tiaSetZ[vitrii]);
                        TiaSet.transform.eulerAngles = vec;

                        Image img = alloquay.transform.GetChild(vitrii).GetComponent<Image>();
                        string nametuchat = img.sprite.name;
                        if (nametuchat == "tim" || nametuchat == "do" || nametuchat == "vang")
                        {
                            img.color = new Color32(100, 100, 100, 255);
                            img.transform.GetChild(0).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                        }
                        StartCoroutine(delay());
                        IEnumerator delay()
                        {
                            TiaSet.SetActive(true);
                            yield return new WaitForSeconds(0.5f);
                            TiaSet.SetActive(false);
                            StartCoroutine(delayhienqua(json["VatPhamQuayDuoc"]));
                        }
                    }
                    else
                    {
                        tru = 10;
                        StartCoroutine(delay());
                        IEnumerator delay()
                        {
                            for (int i = 0; i < 11; i++)
                            {
                                int vitrii = int.Parse(json["vitriquay"][i].Value);
                                GameObject tiaset = Instantiate(TiaSet, transform.position, Quaternion.identity);
                                
                                Vector3 vec = new Vector3(tiaset.transform.rotation.x, tiaset.transform.rotation.y, tiaSetZ[vitrii]);

                                tiaset.transform.SetParent(gameObject.transform, false);
                                tiaset.transform.position = TiaSet.transform.position;

                                tiaset.transform.eulerAngles = vec;

                                tiaset.transform.localScale = new Vector3(0.8f, 0.8f);
                                tiaset.SetActive(true);

                                Image img = alloquay.transform.GetChild(vitrii).GetComponent<Image>();
                                string nametuchat = img.sprite.name;
                                if (nametuchat == "tim" || nametuchat == "do" || nametuchat == "vang")
                                {
                                    img.color = new Color32(100, 100, 100, 255);
                                    img.transform.GetChild(0).GetComponent<Image>().color = new Color32(100, 100, 100, 255);
                                }
                                // yield return new WaitForSeconds(0.5f);
                                StartCoroutine(delaydestroy(tiaset));
                                yield return new WaitForSeconds(0.1f);

                            }
                            IEnumerator delaydestroy(GameObject d)
                            {
                                yield return new WaitForSeconds(0.5f);
                                Destroy(d);
                            }
                            StartCoroutine(delayhienqua(json["VatPhamQuayDuoc"]));
                        }
                    }
                    Text txt = Inventory.ins.ListItemThuong[svequay].transform.GetChild(0).GetComponent<Text>();
                    txt.text = (int.Parse(txt.text) - tru) + "";
                    g.transform.GetChild(5).transform.GetChild(0).GetComponent<Text>().text = json[svip].Value;
                    objallvequay.transform.GetChild(vitri).GetComponent<Text>().text = Inventory.ins.ListItemThuong[svequay].transform.GetChild(0).GetComponent<Text>().text;
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    duocquay = true;
                }
                CrGame.ins.panelLoadDao.SetActive(false);
            }
        }
        IEnumerator delayhienqua(JSONNode allqua)
        {
            GameObject panelnhanqua = transform.GetChild(1).gameObject;

            panelnhanqua.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            GameObject objallqua = panelnhanqua.transform.GetChild(1).gameObject;
            objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            objallqua.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            GameObject allquaa = objallqua.transform.GetChild(1).gameObject;
            GameObject objqua = allquaa.transform.GetChild(0).gameObject;
            allquaa.GetComponent<GridLayoutGroup>().enabled = true;
            for (int i = 0; i < allqua.Count; i++)
            {
                GameObject qua = Instantiate(objqua, transform.position, Quaternion.identity);
                qua.transform.SetParent(allquaa.transform, false);
                qua.SetActive(true);
                if (allqua.Count == 1)
                {
                    allquaa.GetComponent<GridLayoutGroup>().enabled = false;
                    qua.transform.position = new Vector3(allquaa.transform.position.x-0.5f, allquaa.transform.position.y+0.6f);
                }
                GameObject objtrong = qua.transform.GetChild(0).transform.GetChild(0).gameObject;
               // if (objtrong.transform.Find("vien")) Destroy(objtrong.transform.Find("vien"));
                if (allqua[i]["loai"].Value == "item" || allqua[i]["loai"].Value == "ngoc")
                {
                    objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSprite(allqua[i]["name"].Value);
                    objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["txtsoluong"].Value;
                    objtrong.name = "item" + allqua[i]["name"].Value;
                    objtrong.GetComponent<infoitem>().enabled = true;
                }
                else if (allqua[i]["loai"].Value == "rong")
                {
                    objtrong.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.LoadSpriteRong(allqua[i]["name"].Value + "1");
                    objtrong.transform.GetChild(1).GetComponent<Text>().text = allqua[i]["sao"].Value + "sao";
                    objtrong.GetComponent<infoitem>().enabled = false;
                }

                if (allqua[i]["tuchat"].Value == "do")
                {
                    GameObject vien = Instantiate(VienDo, transform.position, Quaternion.identity);
                    vien.transform.SetParent(objtrong.transform, false);
                    vien.transform.position = objtrong.transform.position;
                    vien.transform.localScale = new Vector3(1.5f, 1.5f);
                    vien.name = "vien";
                    vien.SetActive(true);
                }
                else if(allqua[i]["tuchat"].Value == "vang")
                {
                    GameObject vien = Instantiate(VienVang, transform.position, Quaternion.identity);
                    vien.transform.SetParent(objtrong.transform, false);
                    vien.transform.position = objtrong.transform.position;
                    vien.transform.localScale = new Vector3(1.5f, 1.5f);
                    vien.name = "vien";
                    vien.SetActive(true);
                }

                for (int j = 0; j < spritetuchat.Length; j++)
                {
                    if (spritetuchat[j].name == allqua[i]["tuchat"].Value)
                    {
                        objtrong.GetComponent<Image>().sprite = spritetuchat[j];
                        break;
                    }
                }

                yield return new WaitForSeconds(0.02f);
                qua.transform.GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
            objallqua.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
        }
    }
    public void XacNhanQua()
    {
        GameObject hopqua = GameObject.FindGameObjectWithTag("hopqua");
        StartCoroutine(delay());
        IEnumerator delay()
        {
            GameObject panelnhanqua = transform.GetChild(1).gameObject;
            GameObject objallqua = panelnhanqua.transform.GetChild(1).transform.GetChild(1).gameObject;
            panelnhanqua.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
     
            for (int i = 1; i < objallqua.transform.childCount; i++)
            {
                GameObject qua = objallqua.transform.GetChild(i).gameObject;
                StartCoroutine(delaydestroy(qua));
                yield return new WaitForSeconds(0.1f);
            }
            panelnhanqua.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            panelnhanqua.transform.GetChild(0).transform.LeanScale(new Vector3(0, 0.75f), 0.5f);
            yield return new WaitForSeconds(0.7f);
            panelnhanqua.SetActive(false);
            panelnhanqua.transform.GetChild(1).gameObject.SetActive(false);
            panelnhanqua.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            panelnhanqua.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
            duocquay = true;
            for (int i = 1; i < objallqua.transform.childCount; i++)
            {
                Destroy(objallqua.transform.GetChild(i).gameObject);
            }    
        }
        IEnumerator delaydestroy(GameObject qua)
        {
            yield return new WaitForSeconds(0.02f);
            GameObject quatrong = qua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
            quatrong.transform.SetParent(CrGame.ins.panelLoadDao.transform.parent.transform, false);
            QuaBay quabay = quatrong.AddComponent<QuaBay>();
            quabay.vitribay = hopqua;
            qua.SetActive(false);
          //  Destroy(qua, 5);
        }
    }
}
