using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MenuTrieuHoiRongVangBac : MonoBehaviour
{
    // Start is called before the first frame update
    private string namerong;
    public string Setnamerong { set { namerong = value; } }
    private void OnEnable()
    {
        Button btnTrieuHoi = transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
        GameObject AllmanhRong = transform.GetChild(0).transform.GetChild(2).gameObject;
        if (namerong == "vang")
        {
            string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
                {
                    debug.Log(allnamemanh[i]);
                    img.color = new Color32(255, 255, 255, 255);
                    soluongmanhco += 1;
                }
                else img.color = new Color32(125, 125, 125, 181);
                if (soluongmanhco == 5) btnTrieuHoi.interactable = true;
                else btnTrieuHoi.interactable = false;
            }
        }
        else if (namerong == "bac")
        {
            string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
            int soluongmanhco = 0;
            for (int i = 0; i < AllmanhRong.transform.childCount; i++)
            {
                Image img = AllmanhRong.transform.GetChild(i).GetComponent<Image>();
                img.sprite = Inventory.LoadSprite(allnamemanh[i]);
                if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + allnamemanh[i]))
                {
                    img.color = new Color32(255, 255, 255, 255);
                    soluongmanhco += 1;
                }
                else img.color = new Color32(125, 125, 125, 181);
                if (soluongmanhco == 5) btnTrieuHoi.interactable = true;
                else btnTrieuHoi.interactable = false;
            }
        }
        AllmanhRong.name = namerong;
    }

    public void TrieuHoiRong()
    {
        Button btndoi = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        btndoi.interactable = false;
        GameObject AllmanhRong = transform.GetChild(0).transform.GetChild(2).gameObject;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "TrieuHoiRongEventTet";
        datasend["data"]["namerong"] = AllmanhRong.name;

        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                transform.GetChild(0).transform.GetChild(4).gameObject.SetActive(true);
                StartCoroutine(HieuUngTrieuHoi());
                if (AllmanhRong.name == "vang")
                {
                    string[] allnamemanh = new string[] { "DauRongVang", "CanhRongVang", "ChanRongVang", "ThanRongVang", "DuoiRongVang" };
                    for (int i = 0; i < allnamemanh.Length; i++)
                    {
                        NetworkManager.ins.inventory.AddItem(allnamemanh[i], -1);
                    }
                }
                else if (AllmanhRong.name == "bac")
                {
                    string[] allnamemanh = new string[] { "DauRongBac", "CanhRongBac", "ChanRongBac", "ThanRongBac", "DuoiRongBac" };
                    for (int i = 0; i < allnamemanh.Length; i++)
                    {
                        NetworkManager.ins.inventory.AddItem(allnamemanh[i], -1);
                    }
                }
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString, 2);
            IEnumerator HieuUngTrieuHoi()
            {
                yield return new WaitForSeconds(0.5f);
                GameObject g = Instantiate(transform.GetChild(1).gameObject, transform.position, Quaternion.identity);//
                g.transform.SetParent(GameObject.FindGameObjectWithTag("trencung").transform, false);
                g.gameObject.SetActive(true);
                Image img = g.GetComponent<Image>();
                img.sprite = Inventory.LoadSpriteRong(json["namerong"].AsString);
                img.SetNativeSize();
                transform.GetChild(0).transform.GetChild(4).gameObject.SetActive(false);//
                yield return new WaitForSeconds(0.1f);
                QuaBay quabay = img.GetComponent<QuaBay>();
                quabay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                quabay.enabled = true;
                // gameObject.SetActive(false);//
                AllMenu.ins.DestroyMenu("MenuTrieuHoiRongVangBac");
            }
        }
    }

    public void Close()
    {
        AllMenu.ins.DestroyMenu("MenuTrieuHoiRongVangBac");
    }
}
