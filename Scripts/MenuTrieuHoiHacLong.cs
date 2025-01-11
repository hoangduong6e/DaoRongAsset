using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using SimpleJSON;

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
        if (comanh >= 6)
        {
          
            btnTrieuHoi.gameObject.SetActive(true);
            LoadHieuUng();
        }
    }

    public async void LoadHieuUng()
    {
        
        if (DownLoadAssetBundle.MenuBundle.ContainsKey("paneltrieuhoihaclong"))
        {
            CrGame.ins.panelLoadDao.SetActive(true);
            hieuungtrieuhoi = await DownLoadAssetBundle.OpenMenuBundleAsync("paneltrieuhoihaclong");
            CrGame.ins.panelLoadDao.SetActive(false);
        }

    }
    GameObject hieuungtrieuhoi;
    public void TrieuHoi()
    {
        if(hieuungtrieuhoi == null)
        {
            CrGame.ins.OnThongBaoNhanh("Hiệu ứng đang được xử lý!");
            return;
        }

        JSONClass datasend = new JSONClass();
        datasend["class"] = "TienHoaRong";
        datasend["method"] = "TrieuHoiHacLong";
        NetworkManager.ins.SendServer(datasend, Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                debug.Log(json.ToString());
                AudioManager.SoundBg.Stop();
                AudioManager.PlaySound("haclong");
                GameObject child0 = transform.GetChild(0).gameObject;
                GameObject allO = child0.transform.Find("allO").gameObject;
                GameObject AllManh = child0.transform.Find("AllManh").gameObject;
                for (int i = 0; i < AllManh.transform.childCount; i++)
                {
                    string namee = allO.transform.GetChild(i).name;
                    Inventory.ins.AddItem(namee,-1);
                }


                GameObject g = Instantiate(hieuungtrieuhoi, transform.position, Quaternion.identity);
                AnimCodeFrame animCodeFrame = g.GetComponent<AnimCodeFrame>();
                GameObject g2 = AllMenu.ins.GetRonggdGiaoDien("RongHacLong2", transform.GetChild(1), 1, false, "RongGiaoDien", new Vector3(65, 65));
                EventManager.StartDelay2(() => {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetComponent<Image>().color = new Color32(0, 0, 0, 225);
                    Camera.main.orthographicSize = 4;
                }, 2f);


                g2.gameObject.SetActive(false);
                g.transform.SetParent(CrGame.ins.trencung.transform, false);
                g.gameObject.SetActive(true);
                //   Volume volume = GameObject.Find("bloom").GetComponent<Volume>();  // Tham chiếu đến Volume
                //  Bloom bloomEffect;  // Tham chiếu đến hiệu ứng Bloom


                void AnimTrieuHoiDone(string s)
                {
                    Debug.Log("trieu hoi done");
                  
                    g2.gameObject.SetActive(true);
                    Animator anim = g2.GetComponent<Animator>();
                    anim.Play("CuongNo");
                    StartShake();
                    g2.transform.LeanScale(new Vector3(90, 90), 0.5f);

                    EventManager.StartDelay2(() => {
                      anim.Play("Flying");
                        Vector3 vechientai = g2.transform.position;
                        g2.transform.LeanMove(new Vector3(vechientai.x, vechientai.y + 1.5f, vechientai.z), 2f);
                        animmokhoakinang.gameObject.SetActive(true);
                        EventManager.StartDelay2(() => {
                            Destroy(g);
                           
                        }, 2f);
                   
                    }, 1f);
                    //
                    g.GetComponent<Image>().color = new Color32(255,255,255,0);

                  
                }

                animCodeFrame.action = AnimTrieuHoiDone;
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
            }
        }

      
    }
    public void ExitMenu()
    {
        AudioManager.SetSoundBg("nhacnen0");
        AllMenu.ins.DestroyMenu("MenuTrieuHoiHacLong");
    }


    public static void StartShake()
    {
        VienChinh.vienchinh.StartCoroutine(Shakee());
        IEnumerator Shakee()
        {
            for (int i = 0; i <= 10; i++)
            {
                yield return new WaitForSeconds(0.1f);
                if(i < 10) VienChinh.vienchinh.StartCoroutine(Shake());
                else CrGame.ins.transform.position = new Vector3(0, 0, CrGame.ins.transform.position.z);
            }

         
        }
    }


    public static IEnumerator Shake(float duration = 0.12f, float magnitude = 0.08f)
    {
        Vector3 originalPosition = CrGame.ins.transform.localPosition; // Lưu vị trí gốc của camera
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Tạo độ lệch ngẫu nhiên
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Tính toán vị trí rung
            Vector3 shakenPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            // Giới hạn vị trí rung bằng Clampcamera
            //   CrGame.ins.transform.localPosition = Clampcamera(shakenPosition);
           CrGame.ins.transform.localPosition = shakenPosition;

            elapsed += Time.deltaTime;

            yield return null; // Đợi đến khung hình tiếp theo
        }

        // Trả camera về vị trí gốc
        CrGame.ins.transform.localPosition = originalPosition;
    }
}
