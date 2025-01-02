using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuNhatKiLoiDai : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite imgWin, imgLose, imglen, imgxuong;
    public GameObject objInfo, Content;
    public void ParseData(JSONNode json)
    {
        for (int i = json["alltrandau"].Count - 1; i >= 0; i--)
        {
            JSONNode trandau = json["alltrandau"][i];
        //    debug.Log(json["alltrandau"][i]);
            GameObject obj = Instantiate(objInfo, transform.position, Quaternion.identity);
            obj.transform.SetParent(Content.transform,false);
            obj.name = trandau["idtrandau"].AsString;
            obj.transform.GetChild(0).name = trandau["serverData"].AsString;
         //   obj.transform.GetChild(1).name = json["alltrandau"][i]["player"].AsString;

            GameObject player1 = obj.transform.Find("Player1").gameObject;
         
            player1.transform.GetChild(2).GetComponent<Text>().text = trandau["player1"]["name"].AsString;
            player1.transform.GetChild(3).GetComponent<Image>().sprite = GetSpriteThangThua(trandau["player1"]["thangthua"].AsString);
            player1.transform.GetChild(0).GetComponent<Image>().sprite = CrGame.ins.khungAvatar.transform.parent.GetComponent<Image>().sprite;


            if (CrGame.ins.khungAvatar.GetComponent<Animator>() != null)
            {
                Animator anim = player1.transform.GetChild(1).GetComponent<Animator>();
                anim.runtimeAnimatorController = CrGame.ins.khungAvatar.GetComponent<Animator>().runtimeAnimatorController;
                GamIns.SetNativeSizeAnimator(anim);
            }
            else player1.transform.GetChild(1).GetComponent<Image>().sprite = CrGame.ins.khungAvatar.sprite;

            //StartCoroutine(SetNativeSizeAnimator(anim));
            // Friend.ins.LoadAvtFriend(trandau["player1"]["id"].AsString,player1.transform.GetChild(0).GetComponent<Image>(), player1.transform.GetChild(1).GetComponent<Image>());

            GameObject player2 = obj.transform.Find("Player2").gameObject;
         //   
            player2.transform.GetChild(2).GetComponent<Text>().text = trandau["player2"]["name"].AsString;
            player2.transform.GetChild(3).GetComponent<Image>().sprite = GetSpriteThangThua(trandau["player2"]["thangthua"].AsString);
            Friend.ins.LoadAvtFriend(trandau["player2"]["id"].AsString, player2.transform.GetChild(0).GetComponent<Image>(), player2.transform.GetChild(1).GetComponent<Image>());

            player1.name = "Player" + trandau["player1"]["player"].AsString;
            player2.name = "Player" + trandau["player2"]["player"].AsString;

            obj.transform.Find("txttime").GetComponent<Text>().text = trandau["timeTranDau"].AsString;
            obj.transform.Find("txtphongthu").GetComponent<Text>().text = trandau["trangthaiphongthu"].AsString;
            Text txthang = obj.transform.Find("txtHang").GetComponent<Text>();
            if(trandau["topthaydoi"].AsInt == 0)
            {
                txthang.text = "Hạng: Không đổi";
            }
            else if (trandau["topthaydoi"].AsInt > 0)
            {
                txthang.text = "Hạng: <color=lime>" + trandau["topthaydoi"].AsString + "</color>";
                Image imgtangtop = txthang.transform.GetChild(0).GetComponent<Image>();
                imgtangtop.sprite = imglen;
                imgtangtop.gameObject.SetActive(true);
            }    
            else
            {
                txthang.text = "Hạng: <color=red>" + Mathf.Abs(trandau["topthaydoi"].AsInt) + "</color>";
                txthang.transform.GetChild(0).gameObject.SetActive(true);
              //  Image imgtangtop = txthang.transform.GetChild(0).GetComponent<Image>();
              // imgtangtop.sprite = imglen;
            }
            obj.SetActive(true);
        }
    }
    public void StartReplay()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        string idtrandau = btnchon.name;
        string linkdata = btnchon.transform.GetChild(0).name;
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();

     
        GameObject player1 = btnchon.transform.Find("Player1").gameObject;
        string nameplayer1 = player1.transform.GetChild(2).GetComponent<Text>().text;
        Image avtplayer1 = player1.transform.GetChild(0).GetComponent<Image>();
        GameObject khungavt1 = player1.transform.GetChild(1).gameObject;

        GameObject player2 = btnchon.transform.Find("Player2").gameObject;
        string nameplayer2 = player2.transform.GetChild(2).GetComponent<Text>().text;
        Image avtplayer2 = player2.transform.GetChild(0).GetComponent<Image>();
        GameObject khungavt2 = player2.transform.GetChild(1).gameObject;


        tbc.txtThongBao.text = "Bạn muốn xem lại trận đấu <color=lime>" + nameplayer1 + "</color> <color=red>VS</color> <color=lime>" + nameplayer2 + "</color>?";
 

        dataHienPlayer data = new dataHienPlayer(nameplayer1, khungavt1, avtplayer1.sprite, nameplayer2, khungavt2, avtplayer2.sprite, btnchon.transform.GetSiblingIndex());
      //  debug.Log("data " + data.nameplayer2);
        tbc.btnChon.onClick.AddListener(delegate { ReplayOk(idtrandau, linkdata, data); });
       // tbc.btnChon.onClick.AddListener(delegate { VienChinh.vienchinh.SetHienPlayer(data); });
    }
    
    private void ReplayOk(string idtrandau, string linkdata,dataHienPlayer data)
    {
        //   string idtrandau = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;

        //NetworkManager.ins.loidai.Clear();
        // NetworkManager.ins.loidai.gameObject.SetActive(false);
        VienChinh.vienchinh.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Inventory.LoadSprite("BGBanDem");
        ReplayData.quayve = QuayVe;
      
        ReplayData.StartReplay(idtrandau, linkdata,data);
    }
    public void QuayVe()
    {
        GiaoDienPVP.ins.gameObject.SetActive(false);
       // VienChinh.vienchinh.gameObject.SetActive(false);
        VienChinh.vienchinh.enabled = false;
        NetworkManager.ins.loidai.gameObject.SetActive(true);
    }
    //IEnumerator SetNativeSizeAnimator(Animator anim)
    //{
    //    yield return new WaitUntil(() => anim.runtimeAnimatorController != null);
    //    anim.GetComponent<Image>().SetNativeSize();
    //}
    private Sprite GetSpriteThangThua(string thangthua)
    {
        if (thangthua == "Thang")
        {
            return imgWin;
        }
        return imgLose;
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuNhatKiLoiDai");
    }
    public void OpenShare()
    {
        CrGame.ins.OnThongBaoNhanh("Tính năng chưa mở!");
        return;
        string idtrandau = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.name;
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.Find("CanvasTrenCung"), true, transform.GetSiblingIndex() + 1).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();

        tbc.txtThongBao.text = "Bạn muốn chia sẻ trận đấu này vào khoảnh khắc của mình?";
         tbc.btnChon.onClick.AddListener(delegate { Share(idtrandau); });
    }
    private void Share(string idtrandau)
    {
     
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ReplayData";
        datasend["method"] = "ShareTranDau";
        datasend["data"]["idtrandau"] = idtrandau;
        NetworkManager.ins.SendServer(datasend, ok);
        void ok(JSONNode json)
        {
            //if (json["status"].AsString == "0")
            //{
            //    MenuNhatKiLoiDai NhatKiLoiDai = AllMenu.ins.GetCreateMenu("MenuNhatKiLoiDai", GameObject.FindGameObjectWithTag("trencung"), true, 2).GetComponent<MenuNhatKiLoiDai>(); //AllMenu.ins.transform.Find("MenuNhatKiLoiDai").GetComponent<MenuNhatKiLoiDai>();
            //    NhatKiLoiDai.gameObject.SetActive(true);
            //    NhatKiLoiDai.ParseData(json["data"]);

            //    NhatKiLoiDai.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform);
            //    //  debug.Log(json["data"].ToString());
            //}
            //else
            //{
            //    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            //}
            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }

    }    
    //private string GetStringThangThua(string thangthua)
    //{
    //    if(thangthua == "Thang")
    //    {
    //        return "<color=yellow>Phòng thủ thành công</color>";
    //    }
    //    return "<color=red>Phòng thủ thất bại</color>";aaaaaaaaaaaaaaaaaaaaaaaaa
    //}aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
}
