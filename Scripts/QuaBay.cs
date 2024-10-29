using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class QuaBay : MonoBehaviour
{
    public GameObject vitribay;
    private GameObject add;
    string namevitribay = "";
    private void OnEnable()
    {
        if (transform.position.x <= CrGame.ins.transform.position.x - 4) transform.position = new Vector3(CrGame.ins.transform.position.x - 4, transform.position.y, transform.position.z);
        if (transform.position.x >= CrGame.ins.transform.position.x + 4) transform.position = new Vector3(CrGame.ins.transform.position.x + 4, transform.position.y, transform.position.z);

        Vector3 scale = transform.localScale;
        transform.LeanScale(new Vector2(scale.x / 1.5f, scale.y/1.5f), 0.7f);
        //vitribay.transform.SetParent(CrGame.ins.trencung.transform);



        if (vitribay == null)
        {
            vitribay = CrGame.ins.giaodien.transform.GetChild(0).gameObject;//GameObject.Find("ImageTuiDo");
        }

        
        // if (CrGame.ins.allHienQuaBay[vitribay.name]) return;
        if (vitribay.name != "TienVang")
        {
            if (CrGame.ins.allHienQuaBay.ContainsKey(vitribay.name)) return;
            CrGame.ins.StartCoroutine(delay());
        }
        IEnumerator delay()
        {
            namevitribay = vitribay.name;
            CrGame.ins.allHienQuaBay.Add(namevitribay,true);
            yield return new WaitForSeconds(0.1f);
            add = Instantiate(vitribay.gameObject, vitribay.transform.position, Quaternion.identity);
            add.name = namevitribay;
            add.SetActive(true);
            add.transform.SetParent(CrGame.ins.trencung.transform, false);
            add.transform.SetSiblingIndex(CrGame.ins.menulogin.transform.GetSiblingIndex() - 1);
          
            add.transform.position = vitribay.transform.position;
            yield return new WaitForSeconds(0.3f);

            CrGame.ins.allHienQuaBay.Remove(namevitribay);

        }
        //  vitribay.transform.LeanScale(new Vector2(vitribay.transform.position.x));
    }
    void Update()
    {
        if (vitribay == null)
        {
            Destroy(gameObject);
            Destroy(add, 0.6f);
            return;
        } 
            
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, vitribay.transform.position.z), vitribay.transform.position, 12f * Time.deltaTime);
       // debug.Log(transform.position + "    " + vitribay.transform.position);
        if (Vector2.Distance(transform.position, vitribay.transform.position) < 0.1f)
        {
            if (vitribay.name == "TienVang")
            {
                AudioManager.PlaySound("nhatvang");
            }
            else CrGame.ins.StartCoroutine(delayHopquaBay());

            Destroy(add,0.6f);
            Destroy(gameObject);
        }
    }
    private IEnumerator delayHopquaBay()
    {
        if (add == null) yield break;
        add.transform.SetAsLastSibling();
        add.transform.LeanScale(new Vector3(add.transform.localScale.x * 1.2f, add.transform.localScale.y * 1.2f, add.transform.position.z), 0.3f);
        yield return new WaitForSeconds(0.3f);
        add.transform.LeanScale(new Vector3(add.transform.localScale.x / 1.2f, add.transform.localScale.y / 1.2f, add.transform.position.z), 0.3f);
        //yield return new WaitForSeconds(0.3f);
    }
}
