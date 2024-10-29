using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EventValentineSupportEditor : EditorWindow
{

    [MenuItem("Event Support Editor/Sắp xếp lại thứ tự tên")]
    public static void SapXepTen()
    {
        GameObject menu = GameObject.Find("Canvasmenu").transform.Find("MenuEventVuonHoaThangTu").gameObject;
        GameObject MapTrongCay2 = menu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
        for (int i = 0; i < MapTrongCay2.transform.childCount; i++)
        {
            MapTrongCay2.transform.GetChild(i).gameObject.name = (i + 1).ToString();
        }
    }    
    public static void XepMocQuaGiaoDien2()
    {
        int[] allmoc = {1,3,6,10,15,20,30,40,50,60,70,80,90,100,120,140,160,180,200};
        int[] alltraitim = { 5, 5, 5, 1, 10, 10, 10, 10, 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20 };
        
        //static IEnumerator Delay()
        //{
        //    yield return new WaitForSeconds(1);
        //}
        GameObject menu = GameObject.Find("CanvasTrenCung").transform.Find("GiaoDienXucSac").gameObject;
        
        GameObject ScrollViewallMocQua = menu.transform.Find("ScrollViewallMocQua").gameObject;
        GameObject content = ScrollViewallMocQua.transform.GetChild(0).transform.GetChild(0).gameObject;

        //GameObject dau = content.transform.GetChild(0).gameObject;
        //GameObject giua = content.transform.GetChild(1).gameObject;
        //GameObject cuoi = content.transform.GetChild(1).gameObject;
        //for (int i = 0; i < allmoc.Length - 3; i++)
        //{
        //    GameObject ins = Instantiate(giua, content.transform.position, Quaternion.identity);
        //    ins.transform.SetParent(content.transform,false);
        //    ins.transform.SetSiblingIndex(1);
        //}

        for (int i = 1; i < content.transform.childCount;i++)
        {
            GameObject g = content.transform.GetChild(i).gameObject;
           // g.name = allmoc[i].ToString();
            GameObject imgqua = g.transform.Find("imgqua").gameObject;
            imgqua.transform.GetChild(0).GetComponent<Text>().text = allmoc[i - 1].ToString();
            if(g.transform.Find("traitimphale"))
            {
                GameObject traitimphale = g.transform.Find("traitimphale").gameObject;
                traitimphale.transform.GetChild(0).GetComponent<Text>().text = alltraitim[i - 1].ToString();

                if (i == content.transform.childCount - 1)
                {
                    GameObject imgqua2 = g.transform.Find("imgqua2").gameObject;
                    imgqua2.transform.GetChild(0).GetComponent<Text>().text = allmoc[i].ToString();
                    GameObject traitimphale2 = g.transform.Find("traitimphale2").gameObject;
                    traitimphale2.transform.GetChild(0).GetComponent<Text>().text = alltraitim[i].ToString();
                }
            }
       

            //    g.transform.Find("load").GetComponent<Image>().fillAmount = 0;
        }    
    }


   // public int numberOfItems = 10;
   // public float radius = 280;


    //public static void ShowWindow()
    //{
    //    GetWindow<EventValentineSupportEditor>("Bố cục vòng tròn");
    //    CreateCircle();
    //}

    //void OnGUI()
    //{
    //   // GUILayout.Label("Cài đặt bố cục", EditorStyles.boldLabel);

    // //   itemPrefab = (GameObject)EditorGUILayout.ObjectField("Item Prefab", itemPrefab, typeof(GameObject), false);
    //    numberOfItems = EditorGUILayout.IntField("Số object", numberOfItems);
    //    radius = EditorGUILayout.FloatField("Bán kính", radius);

    //    if (GUILayout.Button("Ok"))
    //    {
    //        CreateCircle();
    //    }
    //}
    [MenuItem("Event Support Editor/Sắp xếp lại vòng tròn")]
    public static void CreateCircle()
    {
        int numberOfItems = 10;
        float radius = 280;
        GameObject parent = GameObject.Find("Canvasmenu").transform.Find("GiaoDienChuyenHoaRong").transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).gameObject;

        for (int i = 0; i < numberOfItems; i++)
        {
            GameObject item = parent.transform.GetChild(i).gameObject;
            item.transform.SetParent(parent.transform);

            // Đặt các item theo vòng tròn
            float angle = i * Mathf.PI * 2 / numberOfItems;
            Vector3 newPos = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            item.transform.localPosition = newPos;

            // Xoay item sao cho hướng ra ngoài
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            item.SetActive(true);
        }
    }
    [MenuItem("Event Support Editor/Test chụm vào giữa")]

    public static void TestChumVaoGiua()
    {
        Transform imgRong = GameObject.Find("Canvasmenu").transform.Find("GiaoDienChuyenHoaRong").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0);
        GameObject parent = GameObject.Find("Canvasmenu").transform.Find("GiaoDienChuyenHoaRong").transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject objhieuung = parent.transform.GetChild(i).transform.GetChild(1).gameObject;
            objhieuung.gameObject.SetActive(false);
            //    objhieuung.transform.position = Vector3.zero;
            GameObject tiaset = objhieuung.transform.GetChild(1).gameObject;
            tiaset.GetComponent<Animator>().enabled = true;
            tiaset.gameObject.SetActive(false);
            // Tính toán hướng từ tia sét đến trung tâm
            Vector3 direction = imgRong.position - tiaset.transform.position;

            // Tính toán góc quay cần thiết cho tia sét
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Áp dụng góc quay vào transform của tia sét
            tiaset.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }


    [MenuItem("Event Support Editor/GetData Editor")]
    public static void ToggleGetData()
    {
        EditorScript.editorGetData = !EditorScript.editorGetData;
        debug.Log("Editor Get Data is now: " + EditorScript.editorGetData);
    }

    [MenuItem("Event Support Editor/GetData Editor", true)]
    public static bool ToggleGetDataValidate()
    {
        Menu.SetChecked("Event Support Editor/GetData Editor", EditorScript.editorGetData);
        return true;
    }
    #if UNITY_IOS //NGROK
      [MenuItem("Event Support Editor/Ngrock")]
    public static void ToggleNgrok()
    {
        EditorScript.ngrok = !EditorScript.ngrok;
        debug.Log("Ngrok: " + EditorScript.ngrok);
    }

    [MenuItem("Event Support Editor/Ngrock", true)]
    public static bool BToggleNgrok()
    {
        Menu.SetChecked("Event Support Editor/Ngrock", EditorScript.ngrok);
        return true;
    }
    #endif
}
