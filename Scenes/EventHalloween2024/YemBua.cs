
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class MenuEventHalloween2024 : EventManager
{
    [Header("----Giao diện yểm bùa-----")]
    [SerializeField] private Transform[] allTxtAnim;

    private bool DuocYemBua = true;
    public void ExitYemBua()
    {
     //   if (!DuocYemBua) return;
        GameObject btnchon = EventSystem.current.currentSelectedGameObject;
        btnchon.gameObject.SetActive(false);
    }
    public void YemBua()
    {
        if (!DuocYemBua) return;
        JSONClass datasend = new JSONClass();
        datasend["class"] =nameEvent;
        datasend["method"] = "YemBua";
        NetworkManager.ins.SendServer(datasend.ToString(), Ok);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {

        
                DuocYemBua = false;
                debug.Log(json.ToString());

                Transform PanelYemBua = transform.Find("PanelYemBua");
                Animator anim = PanelYemBua.transform.GetChild(0).transform.Find("animBua").GetComponent<Animator>();
                anim.Play("anim");
                GameObject alleff = PanelYemBua.transform.GetChild(0).transform.Find("alleff").gameObject;
                StartCoroutine(delay());
                IEnumerator delay()
                {
                    yield return new WaitForSeconds(0.4f);
                    alleff.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.6f);
                    alleff.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(true);
                        allTxtAnim[i].transform.GetChild(0).GetComponent<Text>().text = "+" + json["RandomCongBua"][i].AsString;
                    }

                    foreach (KeyValuePair<string, JSONNode> key in json["allitemUpdate"].AsObject)
                    {
                        SetItem(key.Key, key.Value.AsInt);
                    }

                    yield return new WaitForSeconds(0.3f);

                    DuocYemBua = true;

                    yield return new WaitForSeconds(1.3f);

                    for (int i = 0; i < 3; i++)
                    {
                        allTxtAnim[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }
      
    }
}
