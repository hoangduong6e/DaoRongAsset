using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemroi : MonoBehaviour
{
    private void OnEnable()
    {
        if(GetComponent<Animator>())
        {
            StartCoroutine(delay());
            IEnumerator delay()
            {
                yield return new WaitForSeconds(0.6f);
                Destroy(GetComponent<Animator>());
            }
        }    
    }
    public void ButtonDown()
    {
        GameObject vitribay = CrGame.ins.txtVang.transform.parent.gameObject;
        if (vitribay == null)
        {
            Destroy(gameObject);
            return;
        }
        GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + CrGame.ins.DangODao).gameObject;
        GameObject allvangdao = CrGame.ins.FindObject(Dao, "itemroi");
        
        for (int i = 0; i < allvangdao.transform.childCount; i++)
        {
            itemroi item = allvangdao.transform.GetChild(i).GetComponent<itemroi>();
            item.Bay(vitribay);
        }
        NetworkManager.ins.socket.Emit("nhatvang");
    }
    public void Bay(GameObject transform)
    {
        debug.Log("Bay " + transform.name);
        QuaBay quabay = GetComponent<QuaBay>();
        quabay.vitribay = transform;
        quabay.enabled = true;
    }
}
