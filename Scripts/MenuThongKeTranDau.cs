using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuThongKeTranDau : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Obj,ObjXemThem,contentXemThem;
    string[] allteam = new string[] { "TeamXanh", "TeamDo" };
    public void ParseData()
    {
        for (int i = 0; i < allteam.Length; i++)
        {
           // debug.Log(allteam[i] + ": DATA: " + ThongKeDame.Data[allteam[i]].ISNull);
           // debug.Log(allteam[i] + ": DATA tesst: " + ThongKeDame.Data[allteam[i]].test);

            if (ThongKeDame.Data[allteam[i]].ISNull) break;
            
            GameObject content = transform.GetChild(0).transform.Find("ScrollView" + allteam[i]).transform.GetChild(0).transform.GetChild(0).gameObject;
            content.GetComponent<GridLayoutGroup>().constraintCount = ThongKeDame.Data[allteam[i]].Count + 1;
            foreach (KeyValuePair<string, JSONNode> loairong in ThongKeDame.Data[allteam[i]].AsObject)
            {
                //   debug.Log("key loai rong: " + loairong.Key + " value: " + loairong.Value);
                GameObject obj = Instantiate(Obj, transform.position, Quaternion.identity);
                obj.transform.SetParent(content.transform, false);
                obj.SetActive(true);
                obj.name = loairong.Key + "/" + allteam[i];
                float dame = 0;
                float hoiphuc = 0;
                float chongchiu = 0;
                foreach (KeyValuePair<string, JSONNode> rong in ThongKeDame.Data[allteam[i]][loairong.Key].AsObject)
                {
                    //    debug.Log("key rong: " + rong.Key + " value: " + rong.Value);
                    dame += ThongKeDame.Data[allteam[i]][loairong.Key][rong.Key]["dame"].AsFloat;
                    hoiphuc += ThongKeDame.Data[allteam[i]][loairong.Key][rong.Key]["hoiphuc"].AsFloat;
                    chongchiu += ThongKeDame.Data[allteam[i]][loairong.Key][rong.Key]["chongchiu"].AsFloat;
                }
                obj.transform.Find("txtdame").GetComponent<Text>().text = GetText("Sát thương", dame);
                obj.transform.Find("txtsoluong").GetComponent<Text>().text = "x" + ThongKeDame.Data[allteam[i]][loairong.Key].Count;

                obj.transform.Find("txthoiphuc").GetComponent<Text>().text = GetText("Hồi phục", hoiphuc);
                obj.transform.Find("txtchongchiu").GetComponent<Text>().text = GetText("Chống chịu", chongchiu);

                Image imgavt = obj.transform.Find("imgavt").GetComponent<Image>();
                Friend.ins.LoadImage("avtthongke", NetworkManager.ins.CatDauNgoacKep(loairong.Key + "2"), imgavt);
            }
        }
       
        gameObject.SetActive(true);
    }
    
    public void Exit()
    {
        AllMenu.ins.DestroyMenu("MenuThongKeTranDau");
    }
    public void XemThem()
    {
        GameObject btnchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string[] cat = btnchon.transform.parent.name.Split('/');// cat1 team cat0 nameobjectrong

        float dametonhat = 0;
        float netranhtonhat = 0;
        float chimangtonhat = 0;
        float hoiphuctonhat = 0;
        float chongchiutonhat = 0;
        debug.Log(cat[0] + " " + cat[1] + " " + ThongKeDame.Data[cat[1]][cat[0]]);
        foreach (KeyValuePair<string, JSONNode> rong in ThongKeDame.Data[cat[1]][cat[0]].AsObject)
        {
            if (ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["dame"].AsFloat > dametonhat) dametonhat = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["dame"].AsFloat;
            if (ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["netranh"].AsFloat > netranhtonhat) netranhtonhat = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["netranh"].AsFloat;
            if (ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chimang"].AsFloat > chimangtonhat) chimangtonhat = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chimang"].AsFloat;
            if (ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["hoiphuc"].AsFloat > chimangtonhat) hoiphuctonhat = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["hoiphuc"].AsFloat;
            if (ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chongchiu"].AsFloat > chongchiutonhat) chongchiutonhat = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chongchiu"].AsFloat;
        }
        foreach (KeyValuePair<string, JSONNode> rong in ThongKeDame.Data[cat[1]][cat[0]].AsObject)
        {
            GameObject obj = Instantiate(ObjXemThem, transform.position, Quaternion.identity);
            obj.transform.SetParent(contentXemThem.transform, false);
            obj.SetActive(true);


           
            obj.transform.Find("txtnamerong").GetComponent<Text>().text = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["tenhienthi"].AsString;
            Image imgavt = obj.transform.Find("imgavt").GetComponent<Image>();
            imgavt.sprite = btnchon.transform.parent.transform.Find("imgavt").GetComponent<Image>().sprite;


            obj.transform.Find("txtdame").GetComponent<Text>().text = GetText("Sát thương", ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["dame"].AsFloat);

            obj.transform.Find("filldame").GetComponent<Image>().fillAmount = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["dame"].AsFloat / dametonhat;

            obj.transform.Find("txtnetranh").GetComponent<Text>().text = GetText("Né tránh", ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["netranh"].AsFloat);
            obj.transform.Find("fillnetranh").GetComponent<Image>().fillAmount = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["netranh"].AsFloat / netranhtonhat;

            obj.transform.Find("txtchimang").GetComponent<Text>().text = GetText("Chí mạng", ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chimang"].AsFloat);
            obj.transform.Find("fillchimang").GetComponent<Image>().fillAmount = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chimang"].AsFloat / chimangtonhat;

            obj.transform.Find("txthoiphuc").GetComponent<Text>().text = GetText("Hồi phục", ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["hoiphuc"].AsFloat);
            obj.transform.Find("fillhoiphuc").GetComponent<Image>().fillAmount = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["hoiphuc"].AsFloat / hoiphuctonhat;

            obj.transform.Find("txtchongchiu").GetComponent<Text>().text = GetText("Chống chịu", ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chongchiu"].AsFloat);
            obj.transform.Find("fillchongchiu").GetComponent<Image>().fillAmount = ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chongchiu"].AsFloat / chongchiutonhat;

            //debug.Log("sát thương: " + ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["dame"].AsFloat);
            //debug.Log("né tránh: " + ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["netranh"].AsFloat);
            //debug.Log("chí mạng: " + ThongKeDame.Data[cat[1]][cat[0]][rong.Key]["chimang"].AsFloat);
            //  Friend.ins.LoadImage("avt", NetworkManager.ins.CatDauNgoacKep(cat[0] + "2"), imgavt);

        }
        transform.Find("XemThem").gameObject.SetActive(true);
    }
    private string GetText(string txt, float value)
    {
        if (txt.Length >= 7) return txt + ": " + CrGame.FormatCash(Mathf.Floor(value));
        else return txt + ": " + value;
    }    
    public void ExitXemThem()
    {
        for (int i = 0; i < contentXemThem.transform.childCount; i++)
        {
            Destroy(contentXemThem.transform.GetChild(i).gameObject);
        }
        transform.Find("XemThem").gameObject.SetActive(false);
    }
}
