using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ListFriend : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ofriend, ContentFriend; bool onDelete = false; public Button btnOnOff;
    public InputField inputId;
    private void Start()
    {
        //load
        CrGame.ins.panelLoadDao.SetActive(true);
        StartCoroutine(Load());
        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "GetListFriend/id/" + LoginFacebook.ins.id);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
                CrGame.ins.panelLoadDao.SetActive(false);
                CrGame.ins.OnThongBaoNhanh("Lỗi khi tải dữ liệu.");
                AllMenu.ins.DestroyMenu("menuFriend");
            }
            else
            {
                // Show results as text
                debug.Log(www.downloadHandler.text);

                JSONNode json = JSON.Parse(www.downloadHandler.text);
                if (json["status"].Value == "ok")
                {
                    JSONNode allfriend = json["data"]["allfriend"];
                    for (int i = 1; i < allfriend.Count; i++)
                    {
                        GameObject Offriend = Instantiate(Ofriend, ContentFriend.transform.position, Quaternion.identity) as GameObject;
                        Offriend.transform.SetParent(ContentFriend.transform, false);
                        Image Avatar = Offriend.transform.GetChild(0).GetComponent<Image>();
                      //  btnFriend btnfr = Offriend.GetComponent<btnFriend>();
                        //btnfr.idfb = allfriend[i]["idfb"].Value;
                       // btnfr.idObjectFriend = allfriend[i]["name"].Value;
                        Offriend.name = allfriend[i]["name"].AsString;
                        Offriend.transform.GetChild(0).name = allfriend[i]["idfb"].AsString;
                        Text txtname = Offriend.transform.GetChild(2).GetComponent<Text>();
                        txtname.text = allfriend[i]["name"].AsString;
                        if (txtname.text.Length > 8)
                        {
                            string newname = txtname.text.Substring(0, 8) + "...";
                            txtname.text = newname;
                        }
                        // friend.GetAvatarFriend(CatDauNgoacKep(allfriend[i]["idfb"].ToString()), Avatar);

                         Image Khung = Offriend.transform.GetChild(1).GetComponent<Image>();
                        //  Khung.sprite = Inventory.LoadSprite("Avatar" + CatDauNgoacKep(allfriend[i]["toc"].ToString()));
                        Friend.ins.LoadAvtFriend(allfriend[i]["objectId"].Value, Avatar, Khung);
                        Offriend.SetActive(true);
                    }
                }
                else
                {
                    CrGame.ins.OnThongBaoNhanh(json["status"].Value);
                    AllMenu.ins.DestroyMenu("menuFriend");
                }
                CrGame.ins.panelLoadDao.SetActive(false);
                //transform.GetChild(4).transform.GetChild(2).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 5 / 15;
            }
        }
    }
   
    public void Search()
    {
        if (inputId.text != "")
        {
            CrGame.ins.OnThongBao(true, "Đang tìm kiếm...", false);
            NetworkManager.ins.socket.Emit("AddFriend", JSONObject.CreateStringObject(inputId.text));
        }
    }
    public void OnOffDelete()
    {
        if (onDelete == false)
        {
            onDelete = true;
        }
        else
        {
            onDelete = false;
        }
        btnOnOff.interactable = onDelete;
        for (int i = 0; i < ContentFriend.transform.childCount; i++)
        {
            ContentFriend.transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(onDelete);
            //btnFri
            //btnFriend btn = ContentFriend.transform.GetChild(i).GetComponent<btnFriend>();
            //btn.btnXoa.SetActive(onDelete);
        }
    }
    public void Exit()
    {
        //CrGame.ins.allmenu.DestroyMenu("menuFriend");
        gameObject.SetActive(false);
    }
    public void DeleteFriend()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        NetworkManager.ins.socket.Emit("DeleteFriend", JSONObject.CreateStringObject(btnchon.transform.parent.name));
    }
    public void QuaNhaFriend()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if(transform.parent.name == "MenuEventTrungThu2024")
        {
            debug.Log("nameFriend: " + Friend.ins.nameFriend);
            Friend.ins.nameFriend = btnchon.name;
               Friend.ins.idFriend = btnchon.transform.GetChild(0).name;
               MenuEventTrungThu2024.inss.QuaNhaFriend();
            return;
        }
        if (Friend.ins.nameFriend != btnchon.name)
        {
            Friend.ins.nameFriend = btnchon.name;
            Friend.ins.idFriend = btnchon.transform.GetChild(0).name;
            Friend.ins.QuaNhaFriend();
        }
    }
}
