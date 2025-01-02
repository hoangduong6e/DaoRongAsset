using SimpleJSON;
using System;
using System.Collections;
using System.IO.Compression;
using System.IO;
using System.Text;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;

public class ReplayData : MonoBehaviour
{
    //public static JSONClass ReplayDataTrieuHoi = new JSONClass();
    // public static JSONClass ReplayDataPosition = new JSONClass();

    public static JSONNode NodeDataTrieuHoi = new JSONNode();
    public static JSONNode NodeDataPosition = new JSONNode();

    // public static JSONObject ObjDataTrieuHoi = new JSONObject();

    public static JSONObject allDataReplaay = new JSONObject(); // newww
    public static float time;
    public static bool Record { get; set; }
    public static bool Replay { get; private set; }
    public static byte speedReplay = 1;
    public static byte done = 0;

    //  public static List<IEnumerator> allIeRongTrieuHoi;
    public static void ResetReplayData()
    {
        // ReplayDataTrieuHoi = new JSONClass();
        // ReplayDataPosition = new JSONClass();

        NodeDataTrieuHoi = new JSONNode();
        NodeDataPosition = new JSONNode();
        quayve = null;
        //   allIeRongTrieuHoi = new List<IEnumerator>();
        done = 0;

        allDataReplaay = new JSONObject();
        allDataReplaay.AddField("DataTrieuHoi", new JSONObject());
        allDataReplaay.AddField("DataPosition", new JSONObject());
    }
    public static void StartReplay(string idtrandau, string linkdata, dataHienPlayer data)
    {
        if (NodeDataPosition.Count > 0)
        {
            Replayy();
            return;
        }

        //  ResetReplayData();

        //CrGame.ins.menulogin.SetActive(true);
        //Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
        //Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
        //Progress.fillAmount = (float)1 / (float)100;
        //txtProgress.text = "1%";
   
        //{

        //    if (json["status"].AsString == "0")
        //    {
        //        AllMenu.ins.DestroyMenu("MenuNhatKiLoiDai");
        //        debug.Log(json.ToString());
        //        // VienChinh.vienchinh.chedodau = "Replay";
        //        VienChinh.vienchinh.enabled = true;
        //        GiaoDienPVP.ins.ResetAllHienRONG();
        //        float load = 1;
        //        float process = 0;


        //        //  NodeDataTrieuHoi = JSON.Parse(Decompress(json["data"]["DataTrieuHoi"].AsString));

        //        NodeDataTrieuHoi = json["data"]["DataTrieuHoi"];




        //        // debug.Log(NodeDataPosition.ToString());
        //        if (NodeDataTrieuHoi == null) return;
        //        NodeDataPosition = json["data"]["DataTrieuHoi"].AsObject;
        //        Replayy();
        //        //int count = json["data"]["DataTrieuHoi"].Count;

        //        //GamIns.ins.StartCoroutine(delayprocess());
        //        //IEnumerator delayprocess()
        //        //{
        //        //    for (byte i = 0; i < count; i++)
        //        //    {
        //        //        string id = json["data"]["DataTrieuHoi"][i]["id"].AsString;
        //        //        //  NodeDataPosition[i] = JSON.Parse(Decompress(NodeDataPosition[i].AsString));
        //        //        //   debug.Log("truoc khi giai ma " + json["data"]["DataPosition"][id].AsString);
        //        //        JSONNode decompress = JSON.Parse(Decompress(json["data"]["DataPosition"][id].AsString));
        //        //        //  debug.Log("saau khi giai ma " + decompress.ToString());z
        //        //        NodeDataPosition[id] = decompress;
        //        //        //    debug.Log(NodeDataPosition[id].ToString());
        //        //        yield return new WaitForSeconds(GamIns.getSecondsLoad());
        //        //        load += 1;
        //        //        process = (load / count) * 100;
        //        //        Progress.fillAmount = (float)process / (float)100;
        //        //        txtProgress.text = (int)process + "%";
        //        //    }
        //        //    Replayy();
        //        //}
        //    }
        //    else
        //    {
        //        CrGame.ins.menulogin.SetActive(false);
        //        CrGame.ins.menulogin.SetActive(false);
        //        QuayVe();
        //        CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        //    }
        //}
        CrGame.ins.menulogin.SetActive(true);
        float process = 0;
        Image maskload = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
        Text txtload = CrGame.ins.menulogin.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        CrGame.ins.StartCoroutine(DownDataReplay());
        IEnumerator DownDataReplay()
        {
            // Tạo yêu cầu GET để tải về tệp tin
            debug.Log("https://" + linkdata + "/DataReplay/" + idtrandau + ".txt");
            UnityWebRequest www = UnityWebRequest.Get("https://"+ linkdata + "/DataReplay/" + idtrandau+".txt");

            // Gửi yêu cầu và đợi phản hồi
            yield return www.SendWebRequest();

            while (!www.isDone)
            {
                process = 50 + (www.downloadProgress * 100f) / 2;
                debug.Log("Downloading... " + process + "%");
                txtload.text = "Đang tải dữ liệu: " + System.Math.Round(process, 2) + "%";
                maskload.fillAmount = (float)process / 100;
                yield return new WaitForSeconds(.01f);
            }

            // Kiểm tra lỗi
            if (www.result != UnityWebRequest.Result.Success)
            {

                CrGame.ins.menulogin.SetActive(false);
                CrGame.ins.menulogin.SetActive(false);
                QuayVe();
                if (data.btnchon != 1) CrGame.ins.OnThongBaoNhanh("Không thể tải dữ liệu trận đấu!");
                else CrGame.ins.OnThongBaoNhanh("Trận đấu đang được xử lý!");

                debug.LogError("Lỗi khi tải về tệp tin: " + www.error);
            }
            else
            {
                // Chuyển đổi dữ liệu tải về thành chuỗi
                string downloadedText = www.downloadHandler.text;


              //  debug.Log("Nội dung tệp tin tải về:\n" + downloadedText);


                JSONNode json = JSON.Parse(downloadedText);

                AllMenu.ins.DestroyMenu("MenuNhatKiLoiDai");
                // VienChinh.vienchinh.chedodau = "Replay";
                VienChinh.vienchinh.enabled = true;
                GiaoDienPVP.ins.ResetAllHienRONG();
         
                // float load = 1;
                //  float process = 0;


                //  NodeDataTrieuHoi = JSON.Parse(Decompress(json["data"]["DataTrieuHoi"].AsString));

                NodeDataTrieuHoi = json["DataTrieuHoi"];

                // debug.Log(NodeDataPosition.ToString());
//if (NodeDataTrieuHoi == null) yield return;

                NodeDataPosition = json["DataPosition"];
                CrGame.ins.menulogin.SetActive(false);
                Replayy();
                //int count = json["data"]["DataTrieuHoi"].Count;

                //GamIns.ins.StartCoroutine(delayprocess());
                //IEnumerator delayprocess()
                //{
                //    for (byte i = 0; i < count; i++)
                //    {
                //        string id = json["data"]["DataTrieuHoi"][i]["id"].AsString;
                //        //  NodeDataPosition[i] = JSON.Parse(Decompress(NodeDataPosition[i].AsString));
                //        //   debug.Log("truoc khi giai ma " + json["data"]["DataPosition"][id].AsString);
                //        JSONNode decompress = JSON.Parse(Decompress(json["data"]["DataPosition"][id].AsString));
                //        //  debug.Log("saau khi giai ma " + decompress.ToString());z
                //        NodeDataPosition[id] = decompress;
                //        //    debug.Log(NodeDataPosition[id].ToString());
                //        yield return new WaitForSeconds(GamIns.getSecondsLoad());
                //        load += 1;
                //        process = (load / count) * 100;
                //        Progress.fillAmount = (float)process / (float)100;
                //        txtProgress.text = (int)process + "%";
                //    }
                //    Replayy();
                //}


            }
        }

        void Replayy()
        {
            //  VienChinh.vienchinh.reset();
            VienChinh.vienchinh.ResetTru();
            VienChinh.vienchinh.chedodau = CheDoDau.Replay;
            Replay = true;
            Record = false;

            NetworkManager.ins.loidai.Clear();
            NetworkManager.ins.loidai.gameObject.SetActive(false);

            GiaoDienPVP.ins.btnSetting.gameObject.SetActive(true);
            GiaoDienPVP.ins.btnTrieuHoiSuPhu.gameObject.SetActive(false);//btnTrieuHoiNhanh
            GiaoDienPVP.ins.OSkill.gameObject.SetActive(false);//btnTrieuHoiNhanh
            GiaoDienPVP.ins.transform.Find("btnTrieuHoiNhanh").gameObject.SetActive(false);//btnTrieuHoiNhanh
                                                                                           // AudioManager.SetSoundBg("");
            VienChinh.vienchinh.StartCoroutine(VienChinh.vienchinh.delayGame("nhacloidai"));
            speedReplay = 1;
            GiaoDienPVP.ins.SettxtSpeedReplay();
            VienChinh.vienchinh.SetHienPlayer(data);
            AllMenu.ins.DestroyMenu("GiaoDienThongTin");

            time = 0;
            for (byte i = 0; i < NodeDataTrieuHoi.Count; i++)
            {
                //     JSONNode giaima = ReplayData.NodeDataTrieuHoi[i].AsString;
                // debug.Log("Giai ma " + ReplayData.NodeDataTrieuHoi[i]);
                //  debug.Log("id: " + );
                //ReplayData.NodeDataPosition[ReplayData.NodeDataTrieuHoi[i]["id"].AsString] = JSON.Parse(ReplayData.Decompress(ReplayData.NodeDataPosition[ReplayData.NodeDataTrieuHoi[i]["id"].AsString].AsString));
                //IEnumerator enumerator =;
                // allIeRongTrieuHoi.Add(enumerator);
                //    debug.Log(NodeDataTrieuHoi[i]["id"].AsString);
                // debug.Log(NodeDataTrieuHoi[i].ToString());
                CrGame.ins.DonDepDao();
                Friend.ins.DonDepDao();
                if (NodeDataTrieuHoi[i]["id"].ToString() != "")
                {
                    GamIns.ins.StartCoroutine(delayTrieuHoi(i));
                }
                else if (NodeDataTrieuHoi[i]["iconSkill"].ToString() != "")
                {
                    //debug.Log("Hien icon skill " + NodeDataTrieuHoi[i]["iconSkill"].AsString);
                    string[] cat = NodeDataTrieuHoi[i]["iconSkill"].AsString.Split("*");
                    CrGame.ins.StartCoroutine(delayIconSkill(float.Parse(cat[0]), cat[1], cat[2], float.Parse(cat[3])));
                }
                else if (NodeDataTrieuHoi[i]["HieuUngSkill"].ToString() != "")
                {
                    debug.Log("HieuUngSkill " + NodeDataTrieuHoi[i]["HieuUngSkill"].AsString);
                    string[] cat = NodeDataTrieuHoi[i]["HieuUngSkill"].AsString.Split("*");
                    CrGame.ins.StartCoroutine(delayHieuUngSkill(cat[0], float.Parse(cat[1])));
                }
            }
            Send();
            CrGame.ins.menulogin.SetActive(false);


            IEnumerator delayTrieuHoi(byte i)
            {
                float timee = NodeDataTrieuHoi[i]["time"].AsFloat;
                // yield return new WaitForSeconds(time);
                //yield return new WaitUntil(() => time >= timee) ;
                yield return new WaitUntil(GetTimeTrieuHoi);
                PVEManager.TrieuHoiDraReplay(NodeDataTrieuHoi[i]);

                bool GetTimeTrieuHoi()
                {
                    return time >= timee;
                }
            }

            IEnumerator delayIconSkill(float TimeHien, string Team, string NameIcon, float TimeTat)
            {
                yield return new WaitUntil(GetTimeHien);

                VienChinh.vienchinh.HienIconSkill(TimeTat / speedReplay, Team, NameIcon);

                bool GetTimeHien()
                {
                    return time >= TimeHien;
                }
            }
            IEnumerator delayHieuUngSkill(string nameskill, float timeSD)
            {
                yield return new WaitUntil(GetTimeHien);

                VienChinh.vienchinh.InstantiateHieuUngSkill(nameskill);

                bool GetTimeHien()
                {
                    return time >= timeSD;
                }
            }
            void Send()
            {
                JSONClass datasend = new JSONClass();
                datasend["class"] = "ReplayData";
                datasend["method"] = "GetTranDauReplay";
                datasend["data"]["idtrandau"] = idtrandau;
                NetworkManager.ins.SendServer(datasend, ok);
                void ok(JSONNode json)
                {

                }
            }
        }
    }



    //public static bool test()
    //{
    //    debug.Log("test " + time);
    //    return time >= 10;
    //}

    public static Action quayve;
    public static void DoneReplay()
    {
        if (!Replay) return;
        Replay = false;

        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung"), true, 2).GetComponent<ThongBaoChon>();
        tbc.btnChon.onClick.RemoveAllListeners();

        tbc.txtThongBao.text = "Trận đấu đã kết thúc, bạn có muốn xem lại?";
        tbc.btnChon.onClick.AddListener(delegate { StartReplay("","",new dataHienPlayer("")); });
        tbc.transform.Find("btnHuy").GetComponent<Button>().onClick.AddListener(QuayVe);
        //
    }
    public static void QuayVe()
    {
        quayve();
        VienChinh.vienchinh.ResetTru();
        if (GiaoDienPVP.ins != null) VienChinh.vienchinh.OffhienPlayer();
        ResetReplayData();


        //GamIns.ins.StopAllCoroutines();
        //CrGame.ins.giaodien.SetActive(true);
        //ResetReplayData();
        //if (Friend.ins.QuaNha)
        //{
        //    Friend.ins.GoHome();
        //    return;
        //}    
        //GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        //Dao.SetActive(true);
        //CrGame.ins.transform.position = new Vector3(Dao.transform.position.x, Dao.transform.position.y, -10);
    }
    //public static string test()
    //{
    //    return "abc";
    //}
    public static bool dangupdate = false;

    public static void UpdateReplayData(Action action, string thangthua)
    {
        if (dangupdate) return;
        // debug.Log("Record " + Record);

        VienChinh.vienchinh.dangdau = false;
        if (!Record)
        {
            action();
            return;
        }

       // CrGame.ins.menulogin.SetActive(true);
        Image Progress = CrGame.ins.menulogin.transform.GetChild(0).GetComponent<Image>();
        Text txtProgress = Progress.transform.GetChild(0).GetComponent<Text>();
        Progress.fillAmount = (float)1 / (float)100;
        txtProgress.text = "1%";

        dangupdate = true;
        Record = false;
        //  JSONObject datareplay = new JSONObject();

        //for (int i = 0; i < allDataReplaay["DataTrieuHoi"].Count; i++)
        //{
        //    allDataReplaay["DataTrieuHoi"][i] = JSONObject.CreateStringObject(Compress(allDataReplaay["DataTrieuHoi"][i].ToString()));
        //}

        //   datareplay["allrongtrieuhoi"] = allDataReplaay;
        //for(var i = 0; i < ObjDataTrieuHoi.Count;i++)
        //{

        //}    
        //datareplay["allrongtrieuhoi"] = new JSONObject();
        //for (int i = 0; i < ReplayDataTrieuHoi.Count; i++)
        //{
        //    string scount = i.ToString();
        //    datareplay["allrongtrieuhoi"].Add(Compress(ReplayDataTrieuHoi[scount].ToString()));
        //}
        //datareplay["allrongtrieuhoi"] = JSONObject.CreateStringObject(Compress(ReplayDataTrieuHoi.ToString()));
        //  datareplay["allposition"] = new JSONObject();//JSONObject.CreateStringObject(ReplayData.Compress(ReplayData.ReplayDataPosition.ToString()));
        allDataReplaay["thangthua"] = JSONObject.CreateStringObject(thangthua);
        float load = 0;
        float process = 1;





     //   TestUp(allDataReplaay.ToString());
        dangupdate = false;
        action();


        JSONClass datasend = new JSONClass();
        datasend["class"] = "ReplayData";
        datasend["method"] = "UpdateReplayData";
        datasend["data"]["thangthua"] = thangthua;
       // datasend["data"]["dao"] = dao;
        NetworkManager.ins.SendServer(datasend, Ok);
        void Ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
              //  return;
                CrGame.ins.StartCoroutine(StartSave(allDataReplaay.ToString(), json["idtrandau"].AsString));
            }
        }

    
        IEnumerator StartSave(string data, string filename)
        {
            // Chuỗi bạn muốn lưu vào tệp tin
            string content = data;

            // Chuyển đổi chuỗi thành mảng byte
            byte[] fileData = System.Text.Encoding.UTF8.GetBytes(content);
         //   debug.Log("Kích thước của fileData: " + fileData.Length + " byte");
            // Tạo UnityWebRequest POST
            UnityWebRequest www = UnityWebRequest.Post("https://daorongmobile.online/UpdateReplayData.php" + "?FileName=" + filename, "POST");
           // www.SetRequestHeader("FileName", filename);

            // Thêm dữ liệu tệp tin vào yêu cầu
            www.uploadHandler = new UploadHandlerRaw(fileData);

            // Gửi yêu cầu và đợi phản hồi
            yield return www.SendWebRequest();

            // Kiểm tra lỗi

            if (www.result != UnityWebRequest.Result.Success)
            {

                debug.LogError("Lỗi khi gửi tệp tin: " + www.error);
            }
            else
            {
                debug.Log("Tệp tin đã được gửi thành công! " + www.downloadHandler.text);
            }
        }

        //GamIns.ins.StartCoroutine(delay());
        //IEnumerator delay()
        //{

        //    for (int i = 0; i < allDataReplaay["DataPosition"].Count; i++)
        //    {
        //        allDataReplaay["DataPosition"][i] = JSONObject.CreateStringObject(Compress(allDataReplaay["DataPosition"][i].ToString()));

        //        yield return new WaitForSeconds(GamIns.getSecondsLoad());
        //        load += 1;
        //        process = (load / allDataReplaay["DataPosition"].Count) * 100;
        //        Progress.fillAmount = (float)process / 100;
        //        txtProgress.text = (int)process + "%";
        //        debug.Log("Process " + process);

        //    }
        //    //foreach (string id in allDataReplaay["DataPosition"].keys)
        //    //{
        //    //    allDataReplaay["DataPosition"][id] = JSONObject.CreateStringObject(Compress(allDataReplaay["DataPosition"][id].ToString()));

        //    //  //  datareplay["allposition"].AddField(id, Compress(allDataReplaay["DataPosition"][id].ToString()));

        //    //    yield return new WaitForSeconds(GamIns.getSecondsLoad());
        //    //    load += 1;
        //    //    process = (load / allDataReplaay["DataPosition"].Count) * 100;
        //    //    Progress.fillAmount = (float)process / 100;
        //    //    txtProgress.text = (int)process + "%";
        //    //    debug.Log("Process " + process);
        //    //}

        //    // ResetReplayData();

        //    //NetworkManager.ins.socket.Emit("AddReplay", allDataReplaay);
        //    TestUp(allDataReplaay.ToString());
        //     dangupdate = false;
        //    action();
        //}
    }
    //public static void TestUp(string s)
    //{
    //    CrGame.ins.StartCoroutine(StartTest(s));
    //    IEnumerator StartTest(string s)
    //    {
    //        // Chuỗi bạn muốn lưu vào tệp tin
    //        string content = s;

    //        // Chuyển đổi chuỗi thành mảng byte
    //        byte[] fileData = System.Text.Encoding.UTF8.GetBytes(content);

    //        // Tạo UnityWebRequest POST
    //        UnityWebRequest www = UnityWebRequest.Post("https://daorongmobile.online/test.php", "POST");

    //        // Thêm dữ liệu tệp tin vào yêu cầu
    //        www.uploadHandler = new UploadHandlerRaw(fileData);

    //        // Gửi yêu cầu và đợi phản hồi
    //        yield return www.SendWebRequest();

    //        // Kiểm tra lỗi
    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            debug.LogError("Lỗi khi gửi tệp tin: " + www.error);
    //        }
    //        else
    //        {
    //            debug.Log("Tệp tin đã được gửi thành công! " + www.downloadHandler.text);
    //        }
    //    }
    //}
    
    public static void AddNewDragon(string id,string Team,string nameobject, Vector3 Vec,string sao)// id rồng, Team, vị trí triệu hồi
    {
     //   int count = ObjDataTrieuHoi.Count;

        //JSONClass newdra = new JSONClass();
        //newdra["x"] = Vec.x.ToString();
        //newdra["y"] = Vec.y.ToString();
        //newdra["time"] = time.ToString();
        //newdra["Team"] = Team;
        //newdra["id"] = id;
        //newdra["nameobject"] = nameobject;
        //ReplayDataTrieuHoi.Add(count.ToString(), newdra);


        JSONObject newdra = new JSONObject();
        newdra["x"] = JSONObject.CreateStringObject(Vec.x.ToString());
        newdra["y"] = JSONObject.CreateStringObject(Vec.y.ToString());
        newdra["time"] = JSONObject.CreateStringObject(time.ToString());
        newdra["Team"] = JSONObject.CreateStringObject(Team);
        newdra["id"] = JSONObject.CreateStringObject(id);
        newdra["nameobject"] = JSONObject.CreateStringObject(nameobject);
        newdra["sao"] = JSONObject.CreateStringObject(sao);
        allDataReplaay["DataTrieuHoi"].Add(newdra);
     //   ReplayDataTrieuHoi.Add(count.ToString(), newdra);
    }
    public static void AddPositionDragon(string id, float x, string anim, string target = "")
    {
        if (!Record) return;
       // int count = 0;

        //string xtostring = x.ToString();

        //if (ReplayDataPosition[id][0].ToString() != "")
        //{
        //    count = ReplayDataPosition[id].Count;
        //}
        //else
        //{
        //    ReplayDataPosition[id][count]["x"] = xtostring;
        //    ReplayDataPosition[id][count]["anim"] = anim;
        //    return;
        //}

        //if (ReplayDataPosition[id][count - 1]["x"].AsString != xtostring)
        //{
        //    ReplayDataPosition[id][count]["x"] = xtostring;
        //}
        //if (ReplayDataPosition[id][count - 1]["anim"].AsString != anim)
        //{
        //    ReplayDataPosition[id][count]["anim"] = anim;
        //}
        //if (target != "")
        //{
        //    if (ReplayDataPosition[id][count - 1]["target"].AsString != target)
        //    {
        //        ReplayDataPosition[id][count]["target"] = target;
        //    }
        //}
        //if (ReplayDataPosition[id][count].ToString() == "")
        //{
        //    ReplayDataPosition[id][count]["0"] = "";
        //}

        // debug.Log(ReplayDataPosition[id][count].ToString());


        string xtostring = x.ToString();

        JSONObject newid = new JSONObject();

        if (!allDataReplaay["DataPosition"][id])
        {
            allDataReplaay["DataPosition"][id] = new JSONObject();

            newid.AddField("x", xtostring);// = JSONObject.CreateStringObject(xtostring);
            newid.AddField("anim", anim);
            allDataReplaay["DataPosition"][id].Add(newid);
            return;
        }

        int count = allDataReplaay["DataPosition"][id].Count;

        //if (ReplayDataPosition[id][0].ToString() != "")
        //{
        //    count = ReplayDataPosition[id].Count;
        //}
        //else
        //{
        //    ReplayDataPosition[id][count]["x"] = xtostring;
        //    ReplayDataPosition[id][count]["anim"] = anim;
        //    return;
        //}
        //newid.AddField("x", xtostring);

        if (allDataReplaay["DataPosition"][id][count - 1]["x"])
        {
            if (GamIns.CatDauNgoacKep(allDataReplaay["DataPosition"][id][count - 1]["x"].ToString()) != xtostring)
            {
                newid.AddField("x", xtostring);
                //  ReplayDataPosition[id][count]["x"] = xtostring;
            }
        }
        else newid.AddField("x", xtostring);

        if (allDataReplaay["DataPosition"][id][count - 1]["anim"])
        {
            if (GamIns.CatDauNgoacKep(allDataReplaay["DataPosition"][id][count - 1]["anim"].ToString()) != anim)
            {
                newid.AddField("anim", anim);
                //  ReplayDataPosition[id][count]["anim"] = anim;
            }
        }
        else newid.AddField("anim", anim);

        if (target != "")
        {
            if(allDataReplaay["DataPosition"][id][count - 1]["target"])
            {
                if (GamIns.CatDauNgoacKep(allDataReplaay["DataPosition"][id][count - 1]["target"].ToString()) != target)
                {
                    newid.AddField("target", target);
                }
            }
            else newid.AddField("target", target);

            //if (ReplayDataPosition[id][count - 1]["target"].AsString != target)
            //{
            //    ReplayDataPosition[id][count]["target"] = target;
            //}
        }

        //if (ReplayDataPosition[id][count].ToString() == "")
        //{
        //    ReplayDataPosition[id][count]["0"] = "";
        //}

        if (newid.IsNull)
        {
            newid.AddField("0", "0");
        }
      //  debug.LogError("new id count " + newid.Count + " null: "+ newid.IsNull);
         allDataReplaay["DataPosition"][id].Add(newid);
    }
    public static void addHp(string id, string fillhp)
    {
        if (Record)
        {
            //int count = ReplayDataPosition[id].Count - 1;
            //ReplayDataPosition[id][count]["fillhp"] = fillhp;

            if (allDataReplaay["DataPosition"][id])
            {
                int count = allDataReplaay["DataPosition"][id].Count - 1;

                allDataReplaay["DataPosition"][id][count].AddField("fillhp", fillhp);
            }    
        }
    }
    public static void AddAttackTarget(string id, string skill, string target)
    {
        if (Record)
        {
            //int count = ReplayDataPosition[id].Count - 1;
            //ReplayDataPosition[id][count]["skill"] = skill;
            //if(target != "dungdau") ReplayDataPosition[id][count]["target"] = target;

            int count = allDataReplaay["DataPosition"][id].Count - 1;
            allDataReplaay["DataPosition"][id][count].AddField("skill", skill);
            if (target != "dungdau")
            {
                allDataReplaay["DataPosition"][id][count].AddField("target", target);
            }
        }
    }
    public static void AddHieuUngChu(string id, string hieuung)
    {
        if (Record)
        {
            //int count = ReplayDataPosition[id].Count - 1;
            //ReplayDataPosition[id][count]["hieuungchu"] = hieuung;

            int count = allDataReplaay["DataPosition"][id].Count - 1;
            allDataReplaay["DataPosition"][id][count].AddField("hieuungchu", hieuung);
        }
    }
    public static void AddLamCham(string id, string time, string hieuung = "",string chia = "2",string cong = "0")
    {
        if (Record)
        {
            //int count = ReplayDataPosition[id].Count - 1;
            //if(hieuung != "")
            //{
            //    ReplayDataPosition[id][count]["hieuunglamcham"] = hieuung;
            //}    
            //ReplayDataPosition[id][count]["timelamcham"] = time;
            //if(cong == "0")
            //{
            //    ReplayDataPosition[id][count]["chialamcham"] = chia;
            //}
            //else
            //{
            //    ReplayDataPosition[id][count]["tangtoc"] = cong;
            //}
          //  JSONObject newdt
            int count = allDataReplaay["DataPosition"][id].Count - 1;
            if (hieuung != "")
            {
                allDataReplaay["DataPosition"][id][count].AddField("hieuunglamcham", hieuung); //["hieuunglamcham"] = JSONObject.CreateStringObject(hieuung);
            }

            allDataReplaay["DataPosition"][id][count].AddField("timelamcham", time);// ["timelamcham"] = //JSONObject.CreateStringObject(time);
            if (cong == "0")
            {
                allDataReplaay["DataPosition"][id][count].AddField("chialamcham", chia); //["chialamcham"] = JSONObject.CreateStringObject(chia);
            }
            else
            {
                allDataReplaay["DataPosition"][id][count].AddField("tangtoc", cong);
             //   allDataReplaay["DataPosition"][id][count]["tangtoc"] = JSONObject.CreateStringObject(cong);
            }
        }
    }
    public static void AddKillTru(string id)
    {
        if (Record)
        {
            //int count = ReplayDataPosition[id].Count - 1;
            //ReplayDataPosition[id][count]["killtru"] = "kill";

            int count = allDataReplaay["DataPosition"][id].Count - 1;

            allDataReplaay["DataPosition"][id][count].AddField("killtru", "kill");// = JSONObject.CreateStringObject("kill");
        }
    }    
    public static void AddIconSkill(string TimeHien_Team_NameIcon_TimeTat)
    {
        if (!Record) return;
        //int count = ReplayDataTrieuHoi.Count;

        //JSONClass newIcon = new JSONClass();
        //newIcon["iconSkill"] = TimeHien_Team_NameIcon_TimeTat;
        //ReplayDataTrieuHoi.Add(count.ToString(), newIcon);

        JSONObject newIcon = new JSONObject();
        newIcon["iconSkill"] = JSONObject.CreateStringObject(TimeHien_Team_NameIcon_TimeTat);
        allDataReplaay["DataTrieuHoi"].Add(newIcon);
    }
    public static void AddHieuUngSkill(string nameskill_Time)
    {
        if (!Record) return;
        //int count = ReplayDataTrieuHoi.Count;

        //JSONClass newHieuUng = new JSONClass();
        //newHieuUng["HieuUngSkill"] = nameskill_Time + "*" + time;
        //ReplayDataTrieuHoi.Add(count.ToString(), newHieuUng);

        JSONObject newHieuUng = new JSONObject();
        newHieuUng["HieuUngSkill"] = JSONObject.CreateStringObject(nameskill_Time + "*" + time);

        allDataReplaay["DataTrieuHoi"].Add(newHieuUng);
    }
    public static void AddBienCuu(string id, string timebiencuu)
    {
        if (Record)
        {
            debug.Log("add bien cuu " + id  + " " + timebiencuu);
            //int count = ReplayDataPosition[id].Count - 1;
            //ReplayDataPosition[id][count]["BienCuu"] = timebiencuu;

            int count = allDataReplaay["DataPosition"][id].Count - 1;
            allDataReplaay["DataPosition"][id][count].AddField("BienCuu", timebiencuu); //= JSONObject.CreateStringObject(timebiencuu);
        }
    }
    public static bool CheckData(string id, string data, int count)
    {
        bool ok = false;
        if (NodeDataPosition[id][count][data].ToString() != "")
        {
            ok = true;
        }
        return ok;
    }



    public static string Compress(string uncompressedString)
    {
        byte[] compressedBytes;

        using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
        {
            using (var compressedStream = new MemoryStream())
            {
                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, System.IO.Compression.CompressionLevel.Fastest, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }
        }

        return Convert.ToBase64String(compressedBytes);
    }

    /// <summary>
    /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
    /// </summary>
    /// <param name="compressedString">String to decompress.</param>
    public static string Decompress(string compressedString)
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

        using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);

                decompressedBytes = decompressedStream.ToArray();
            }
        }

        return Encoding.UTF8.GetString(decompressedBytes);
    }

}
