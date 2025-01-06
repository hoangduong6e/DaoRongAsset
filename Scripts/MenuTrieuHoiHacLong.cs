using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuTrieuHoiHacLong : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnTrieuHoi;
    public RuntimeAnimatorController animOVangSang;
    public GameObject animmokhoakinang;
    void Start()
    {
        GameObject g = transform.GetChild(0).gameObject;
        GameObject allO = g.transform.Find("allO").gameObject;
        GameObject AllManh = g.transform.Find("AllManh").gameObject;
        int comanh = 0;
        for (int i = 0; i < AllManh.transform.childCount; i++)
        {
            Image img = AllManh.transform.GetChild(i).GetComponent<Image>();
            string namee = allO.transform.GetChild(i).name;
           // debug.Log(namee);
            if (NetworkManager.ins.inventory.ListItemThuong.ContainsKey("item" + namee))
            {
                img.sprite = Inventory.LoadSprite(namee);
                allO.transform.GetChild(i).GetComponent<Animator>().runtimeAnimatorController = animOVangSang;
                comanh += 1;
            }
        }
        if(comanh >= 6)
        {
            btnTrieuHoi.gameObject.SetActive(true);
        }    
    }
    public void TrieuHoi()
    {
        GameObject g = Instantiate(DownLoadAssetBundle.MenuBundle["paneltrieuhoihaclong"], transform.position, Quaternion.identity);
        AnimCodeFrame animCodeFrame = g.GetComponent<AnimCodeFrame>();
        GameObject g2 = AllMenu.ins.GetRonggdGiaoDien("RongHacLong2", transform.GetChild(1), 1, false, "RongGiaoDien", new Vector3(65, 65));
        EventManager.StartDelay2(() => {
            transform.GetChild(0).gameObject.SetActive(false);

        }, 2f);
      

        g2.gameObject.SetActive(false);
        g.transform.SetParent(CrGame.ins.trencung.transform, false);
        g.gameObject.SetActive(true);
     //   Volume volume = GameObject.Find("bloom").GetComponent<Volume>();  // Tham chiếu đến Volume
      //  Bloom bloomEffect;  // Tham chiếu đến hiệu ứng Bloom


        void AnimTrieuHoiDone(string s)
        {
            Debug.Log("trieu hoi done");
            animmokhoakinang.gameObject.SetActive(true);
            g2.gameObject.SetActive(true);
            g2.GetComponent<Animator>().Play("Flying");
            g2.transform.LeanScale(new Vector3(90, 90), 0.5f);
            Vector3 vechientai = g2.transform.position;
            g2.transform.LeanMove(new Vector3(vechientai.x, vechientai.y + 1.5f, vechientai.z), 2f);
            Destroy(g);
        }
        animCodeFrame.action = AnimTrieuHoiDone;
        //StartCoroutine(delay());
        //IEnumerator delay()
        //{
        //    yield return new WaitForSeconds(2f);
        //    transform.GetChild(0).gameObject.SetActive(false);
        //    yield return new WaitForSeconds(3.25f);

        //    yield return new WaitForSeconds(0.05f);
        //    bloomEffect.threshold.value = 0f;
        //    EventManager.StartDelay2(() => { bloomEffect.threshold.value = 1.3f; }, 0.3f);
        //    g2.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(0.3f);


        //    g2.transform.LeanScale(new Vector3(80, 80),0.25f);// nhỏ dần
        //    yield return new WaitForSeconds(0.25f); // nhỏ dần xong

        //    g2.transform.LeanScale(new Vector3(100, 100), 0.5f);// to lên




        //}
    }
    public void ExitMenu()
    {
        AllMenu.ins.DestroyMenu("MenuTrieuHoiHacLong");
    }    
}
