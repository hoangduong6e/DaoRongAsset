using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using XLua;
[LuaCallCSharp]

public class AllMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<string, GameObject> menu = new Dictionary<string, GameObject>();
    public static AllMenu ins;
    public GameObject GetRongGiaoDien(string namerong, Transform tf, float scaleX = 1, bool wps = true, string layername = "RongGiaoDien", Vector3 scale = new Vector3())
    {
        //GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/" + namemenu) as GameObject, transform.position, Quaternion.identity);
        if (tf != null)
        {
            for (int i = 0; i < tf.transform.childCount; i++)
            {
                Destroy(tf.transform.GetChild(i).gameObject);
            }
        }

        GameObject Rong = Instantiate(Inventory.GetObjRong(namerong), transform.position, Quaternion.identity);
        if (tf != null)
        {
            Rong.transform.SetParent(tf.transform, wps);
            Rong.transform.position = tf.transform.position;
        }

        SortingGroup group = Rong.GetComponent<SortingGroup>();
        group.sortingLayerName = layername;

        //GameObject allLayer = Rong.transform.GetChild(0).gameObject;
        //for (int i = 0; i < allLayer.transform.childCount; i++)
        //{
        //    if(allLayer.transform.GetChild(i).GetComponent<Renderer>()) allLayer.transform.GetChild(i).GetComponent<Renderer>().sortingLayerName = layername;

        //}


        Rong.transform.localScale = new Vector3(Rong.transform.localScale.x * scaleX, Rong.transform.localScale.y, Rong.transform.localScale.z);
        if (scale != Vector3.zero)
        {
            Rong.transform.localScale = new Vector3(scale.x * scaleX, scale.y, Rong.transform.localScale.z);
        }


        //  bong.transform.position = vecbongcu;
        return Rong;


        //GameObject Bong = Instantiate(bong, transform.position, Quaternion.identity);
        //Bong.transform.SetParent(tf.transform);
        //Bong.transform.position = bong.transform.position;
        //Vector3 vecBong = Bong.transform.position;
        //Rong.transform.position = vecBong;
        //Destroy(bong);
    }
    public GameObject GetRonggdGiaoDien(string namerong, Transform tf, float scaleX = 1, bool wps = true, string layername = "RongGiaoDien", Vector3 scale = new Vector3())
    {
        GameObject Rong = GetRongGiaoDien(namerong, tf, scaleX, wps, layername, scale);

        GameObject bong0 = Rong.transform.GetChild(0).transform.Find("bong").gameObject;
        // Vector3 vecbong0 = bong0.transform.position;

        GameObject bong = bong0.transform.GetChild(0).gameObject;
        //  float YVecBong = Mathf.Abs(tf.transform.position.y) - Mathf.Abs(bong.transform.position.y);
        //   Rong.transform.position = new Vector3(Rong.transform.position.x, YVecBong, Rong.transform.position.z);



        Vector3 vecbongcu = bong.transform.position;
        bong.transform.position = tf.transform.position;

        Rong.transform.position = bong.transform.position;

        Rong.transform.GetChild(0).transform.position = bong.transform.position;
        Rong.transform.GetChild(1).transform.position = bong.transform.position;

        bong.transform.position = vecbongcu;
        if (namerong == "RongNguSac1") bong.gameObject.SetActive(false);
        //Rong.transform.position = bong.transform.position;
        //bong.transform.position = Rong.transform.position;

        return Rong;


        //   Vector3 vecbong = bong.transform.position;

        // debug.Log("Transfrom bong " + YVecBong);
    }
    public void LoadRongGiaoDien(string namerong, Transform tf, float scaleX = 1, bool wps = true, string layername = "RongGiaoDien", Vector3 scale = new Vector3())
    {
        GetRonggdGiaoDien(namerong, tf, scaleX, wps, layername, scale);
    }
    private void Start()
    {
        ins = this;
    }
    public void DestroyallMenu(string tru = "")
    {
        string[] array = menu.Keys.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            //debug.Log(array[i]);
            if (array[i] != tru)
            {
                DestroyMenu(array[i]);
            }
        }
    }
    public void OpenMenu(string namemenu)
    {
        if (menu.ContainsKey(namemenu))
        {
            menu[namemenu].SetActive(true);
        }
        else
        {
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/" + namemenu) as GameObject, transform.position, Quaternion.identity);
            menu.Add(namemenu, mn);
            mn.transform.SetParent(gameObject.transform, false);
            mn.transform.SetSiblingIndex(1);
            mn.transform.position = gameObject.transform.position;
            mn.SetActive(true);
            //  Resources.UnloadAsset(mn);
            //   Resources.UnloadUnusedAssets();
        }
    }
    public void OpenMenuTrenCung(string namemenu)
    {
        if (menu.ContainsKey(namemenu))
        {
            menu[namemenu].SetActive(true);
        }
        else
        {
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            menu.Add(namemenu, mn);
            mn.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
            mn.transform.SetSiblingIndex(1);
            //   mn.transform.position = parnet.transform.position;
            mn.SetActive(true);
            // Resources.UnloadUnusedAssets();
        }
    }
    public void OpenCreateMenu(string namemenu, GameObject parnet = null, bool active = true)
    {
        if (menu.ContainsKey(namemenu))
        {
            menu[namemenu].SetActive(active);
        }
        else
        {
            if (parnet == null)
            {
                parnet = gameObject;
            }
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            menu.Add(namemenu, mn);
            mn.transform.SetParent(parnet.transform, false);
            mn.transform.SetSiblingIndex(1);
            mn.transform.position = parnet.transform.position;
            mn.SetActive(active);
            //  Resources.UnloadUnusedAssets();
        }
    }
    public GameObject GetCreateMenu(string namemenu, GameObject parnet = null, bool active = true, int index = 1)
    {
        GameObject g = null;
        if (menu.ContainsKey(namemenu))
        {
            menu[namemenu].SetActive(active);
            g = menu[namemenu];
            if (parnet != null)
            {
                g.transform.SetParent(parnet.transform, false);
                g.transform.position = parnet.transform.position;
            }

        }
        else
        {
            if (parnet == null)
            {
                parnet = gameObject;
            }
            GameObject mn = Instantiate(Inventory.LoadObjectResource("GameData/Menu/" + namemenu) as GameObject, transform.position, Quaternion.identity) as GameObject;
            menu.Add(namemenu, mn);
            mn.transform.SetParent(parnet.transform, false);
            mn.transform.position = parnet.transform.position;
            mn.SetActive(active);
            g = mn;
            //  Resources.UnloadUnusedAssets();
        }
        g.transform.SetSiblingIndex(index);
        return g;
    }
    public void CloseMenu(string namemenu)
    {
        if (menu.ContainsKey(namemenu))
        {
            menu[namemenu].SetActive(false);

        }
    }
    public void DestroyMenu(string namemenu)
    {
        if (menu.ContainsKey(namemenu))
        {
            Destroy(menu[namemenu]);
            menu.Remove(namemenu);

            Resources.UnloadUnusedAssets();
        }
    }
    public void OpenMenuGiapRong()
    {
        JSONClass datasend = new JSONClass();
        datasend["class"] = "GiapRong";
        datasend["method"] = "GetAll";
        datasend["data"]["taikhoan"] = LoginFacebook.ins.id;
        NetworkManager.ins.SendServer(datasend, ResultGiapRong);
    }
    void ResultGiapRong(JSONNode json)
    {
        // OpenMenu("MenuGiapRong");
        if (json["status"].AsString == "0")
        {
            //   MenuGiapRong giap = transform.GetChild(transform.childCount - 1).GetComponent<MenuGiapRong>();
            //     giap.gameObject.SetActive(true);
            OpenMenu("MenuGiapRong");
        }
        else
        {
            CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
    public void OpenMenuDisk(string id,Transform parent = null)
    {
        StartCoroutine(AssetBundleManager.ins.LoadAssetBundle(id,
            onSuccess: (assetBundle) =>
            {
           
                debug.Log("AssetBundle " + id + " loaded successfully name: " + AssetBundleManager.infoasbundle[id].ToString());
             //   debug.Log(AssetBundleManager.infoasbundle[id].ToString());
              
                // Sử dụng AssetBundle ở đây, ví dụ:
                GameObject prefab = assetBundle.LoadAsset<GameObject>(AssetBundleManager.infoasbundle[id]["name"].AsString);
                GameObject instan = Instantiate(prefab);

                if (parent != null)
                {
                    instan.transform.SetParent(parent,false);
                }
                else instan.transform.SetParent(ins.transform,false);
                instan.gameObject.SetActive(true);
            },
            onError: (error) => debug.LogError($"Load error: {error}")
        ));
    }
}
