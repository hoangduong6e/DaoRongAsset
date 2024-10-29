using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiaoDienChuyenHoaRong : MonoBehaviour
{
    public Image displayImage;
    private Sprite[] sprites;
    private Sprite[] spritesRongChon;
    private int currentIndex = 0;
    private bool isSwitching = false;
    private List<string> dataRongchon;
    public Text txtyeucau;
    private string[] nameSpriteRongChon = new string[] {"RongBac2","RongVang2" };
    private string[] YeuCau = new string[] {"1","2" };
    private string[] nameRongTv = new string[] {"<color=yellow>Rồng Bạc</color>","<color=yellow>Rồng Vàng</color>" };
    private int[] yeucautoclenh = new int[] {10,20};
    private int[] RongCan = new int[] {10,6};
    // Start is called before the first frame update

    void Start()
    {
      //  AllMenu.ins.LoadRongGiaoDien("RongPhuongHoangBang1", displayImage.transform);
        dataRongchon = new List<string>();
        sprites = new Sprite[] {Inventory.LoadSpriteRong("RongPhuongHoangBang1"), Inventory.LoadSpriteRong("RongPhuongHoangLua1")};
        spritesRongChon = new Sprite[] { Inventory.LoadSpriteRong("RongBac1"), Inventory.LoadSpriteRong("RongVang1") };
        displayImage.sprite = sprites[currentIndex];
        SetTextYeuCau();
        SetCircle = RongCan[currentIndex];
    }
    private void OnEnable()
    {
        GetDraInventory();
    }
    public void ChuyenHoaRong()
    {
        Button btn =  transform.GetChild(0).transform.Find("btnDoiRong").GetComponent<Button>();
        btn.interactable = false;
        string sdata = "";
        for(int i = 0; i < dataRongchon.Count;i++)
        {
            sdata += dataRongchon[i] + "!";
        }    
        JSONClass datasend = new JSONClass();
        datasend["class"] = "ChuyenHoaPhuongHoang";
        datasend["method"] = "ChuyenHoa";
        datasend["data"]["allrong"] = sdata;
        datasend["data"]["rongchon"] = nameSpriteRongChon[currentIndex];
        NetworkManager.ins.SendServer(datasend.ToString(), ok);
        void ok(JSONNode json)
        {
            debug.Log(json.ToString());
            if (json["status"].AsString == "0")
            {
                for (int i = 0; i < dataRongchon.Count; i++)
                {
                    for (int j = 0; j < Inventory.ins.TuiRong.transform.childCount; j++)
                    {
                        Transform childi = Inventory.ins.TuiRong.transform.GetChild(j);
                        if (childi.transform.childCount > 0)
                        {
                            if(childi.transform.GetChild(0).name == dataRongchon[i])
                            {
                                Destroy(childi.transform.GetChild(0).gameObject);
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < contentRong.transform.childCount; j++)
                    {
                        GameObject childi = contentRong.transform.GetChild(j).gameObject;
                        if (childi.name == dataRongchon[i])
                        {
                            Destroy(childi);
                            break;
                        }
                    }
                }
                dataRongchon.Clear();
                Transform parent = transform.GetChild(0).transform.GetChild(0).transform.GetChild(3);
                foreach(Transform child in parent)
                {
                    GameObject imgRong = child.transform.GetChild(0).gameObject;
                    imgRong.SetActive(false);
                    GameObject objHieuUng = child.transform.GetChild(1).gameObject;
                    objHieuUng.SetActive(true);
                    StartCoroutine(delayTatTiaSet(objHieuUng));
                }
                IEnumerator delayTatTiaSet(GameObject objhieuung)
                {
                    yield return new WaitForSeconds(1);
                    Transform tiaset = objhieuung.transform.GetChild(1);
                    tiaset.gameObject.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    tiaset.gameObject.SetActive(false);
                    objhieuung.SetActive(false);
                //    tiaset.transform.GetChild(1).gameObject.SetActive(false);
                }
                GameObject animxong1 = transform.GetChild(0).transform.Find("animXong1").gameObject;
                animxong1.transform.GetChild(0).gameObject.SetActive(false);
                animxong1.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                StartCoroutine(delayanimXong());
                IEnumerator delayanimXong()
                {
                    yield return new WaitForSeconds(1.2f);
                    animxong1.gameObject.SetActive(true);
                    yield return new WaitForSeconds(3f);
                    animxong1.gameObject.SetActive(false);

                    GameObject quabay = Instantiate(displayImage.gameObject, transform.position, Quaternion.identity);
                    quabay.transform.SetParent(CrGame.ins.trencung.transform,false);
                    quabay.transform.position = displayImage.transform.position;
                    QuaBay quapay = quabay.AddComponent<QuaBay>();
                    quapay.vitribay = GameObject.FindGameObjectWithTag("hopqua");
                    CrGame.ins.OnThongBaoNhanh(json["message"].AsString);

                    SetTextYeuCau();
                }
              
            }
            else
            {
                CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
                btn.interactable = true;
            }
        }
    }
    public void XemCs()
    {
        GameObject rongchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        CrGame.ins.ChiSoRong(rongchon.transform.parent.name);
    }
    public void Exit()
    {
        AllMenu.ins.DestroyMenu(nameof(GiaoDienChuyenHoaRong));
    }    
    public void ChonRong()
    {
        GameObject rongchon = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.gameObject;
        string idrong = rongchon.transform.parent.name;
        Image imgg = rongchon.GetComponent<Image>();
        GameObject parent = transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;
        Button btnDoiRong = transform.GetChild(0).transform.Find("btnDoiRong").GetComponent<Button>();
        //  debug.Log("color a: " + imgg.color.a);
        if (imgg.color.a == 1)
        {
            if (dataRongchon.Contains("idrong"))
            {
                CrGame.ins.OnThongBaoNhanh("Đã chọn rồng này rồi!");
                return;
            }

            if (dataRongchon.Count >= RongCan[currentIndex])
            {
                btnDoiRong.interactable = true;
                return;
            } 
                
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject item = parent.transform.GetChild(i).gameObject;
                if (item.activeSelf)
                {
                    Transform child0 = item.transform.GetChild(0);
                    if (!child0.gameObject.activeSelf)
                    {
                        item.name = idrong;
                        child0.gameObject.SetActive(true);
                        Image img = child0.GetComponent<Image>();
                        img.sprite = spritesRongChon[currentIndex];
                        img.SetNativeSize();
                        imgg.color = new Color32(255,255,255,100);
                     //   rongchon.GetComponent<Button>().interactable = false;
                        break;
                    }
                }
            }
            dataRongchon.Add(idrong);
        }
        else
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject item = parent.transform.GetChild(i).gameObject;
                if(item.name == idrong)
                {
                    item.transform.GetChild(0).gameObject.SetActive(false);
                    dataRongchon.Remove(idrong);
                    imgg.color = new Color32(255, 255, 255, 255);
                    break;
                }
            }
        }
        if (dataRongchon.Count >= RongCan[currentIndex]) btnDoiRong.interactable = true;
        else btnDoiRong.interactable = false;

        debug.Log(dataRongchon.Count.ToString());
    }
    public void SwitchLeft()
    {
        if (!isSwitching)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = sprites.Length - 1;
            }
            StartCoroutine(SwitchSprite());
        }
    }

    public void SwitchRight()
    {
        if (!isSwitching)
        {
            currentIndex++;
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
            }
            StartCoroutine(SwitchSprite());
        }
    }

    private IEnumerator SwitchSprite()
    {
   
        isSwitching = true;
        // Fade out
        yield return StartCoroutine(FadeOut());

        // Change sprite
        displayImage.sprite = sprites[currentIndex];

        SetCircle = RongCan[currentIndex];
        
        // Fade in
        yield return StartCoroutine(FadeIn());
        isSwitching = false;
    }

    private IEnumerator FadeOut()
    {
        Color color = displayImage.color;
        while (displayImage.color.a > 0)
        {
            color.a -= Time.deltaTime *5; // Speed of fade out
            displayImage.color = color;
            yield return null;
        }
        color.a = 0;
        displayImage.color = color;
    }

    private IEnumerator FadeIn()
    {
        Color color = displayImage.color;
        while (displayImage.color.a < 1)
        {
            color.a += Time.deltaTime * 5; // Speed of fade in
            displayImage.color = color;
            yield return null;
        }
        color.a = 1;
        displayImage.color = color;
    }

    public int SetCircle 
    {
        set 
        {
             if (value > 10) value = 10; CreateCircle(value);
            dataRongchon.Clear();
        }
    }

    void CreateCircle(int Circle)
    {
 //       float radius = 280;
        GameObject parent = transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).gameObject;

        for (int i = 0; i < Circle; i++)
        {
            GameObject item = parent.transform.GetChild(i).gameObject;
            item.transform.SetParent(parent.transform);
            item.transform.GetChild(0).gameObject.SetActive(false);
            // Đặt các item theo vòng tròn
            float angle = i * Mathf.PI * 2 / Circle;
            Vector3 newPos = new Vector3(Mathf.Sin(angle) * 280, Mathf.Cos(angle) * 280, 0);
            item.transform.localPosition = newPos;

            // Xoay item sao cho hướng ra ngoài
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            item.SetActive(true);

            //GameObject tiaset = item.transform.GetChild(1).gameObject;
            //// Tính toán hướng từ tia sét đến trung tâm
            //Vector3 direction = displayImage.transform.position - tiaset.transform.position;

            //// Tính toán góc quay cần thiết cho tia sét
            //float angletiaset = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //// Áp dụng góc quay vào transform của tia sét
            //tiaset.transform.rotation = Quaternion.Euler(0, 0, angletiaset + 90);
        }

        if (Circle < parent.transform.childCount)
        {
            for (int i = Circle; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
                parent.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject objhieuung = parent.transform.GetChild(i).transform.GetChild(1).gameObject;
            //    objhieuung.transform.position = Vector3.zero;
            GameObject tiaset = objhieuung.transform.GetChild(1).gameObject;
          //  tiaset.GetComponent<Animator>().enabled = false;
            // Tính toán hướng từ tia sét đến trung tâm
            Vector3 direction = displayImage.transform.position - tiaset.transform.position;

            // Tính toán góc quay cần thiết cho tia sét
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Áp dụng góc quay vào transform của tia sét
            tiaset.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }

        GetDraInventory();
        SetTextYeuCau();
    }
    private void SetTextYeuCau()
    {
        int huyentinhco = 0;
        int toclenhco = 0;
        if (Inventory.ins.ListItemThuong.ContainsKey("itemHuyenTinh")) huyentinhco = int.Parse(Inventory.ins.ListItemThuong["itemHuyenTinh"].transform.GetChild(0).GetComponent<Text>().text);
        if (Inventory.ins.ListItemThuong.ContainsKey("itemTocLenh")) toclenhco = int.Parse(Inventory.ins.ListItemThuong["itemTocLenh"].transform.GetChild(0).GetComponent<Text>().text);

        string shuyentinhco = (huyentinhco >= 100000) ? "<color=lime>" + GamIns.FormatCash(huyentinhco) + "</color>" : "<color=red>" + GamIns.FormatCash(huyentinhco) + "</color>";
        string stoclenhco = (toclenhco >= yeucautoclenh[currentIndex]) ? "<color=lime>" + GamIns.FormatCash(toclenhco) + "</color>" : "<color=red>" + GamIns.FormatCash(toclenhco) + "</color>";

        txtyeucau.text =
            "Nguyên liệu cần:\n-" + nameRongTv[currentIndex] + " trưởng thành <color=yellow>15 sao</color> trở lên.\n-"
            + shuyentinhco + "/100K <color=yellow>Huyền Tinh</color> + "
            + stoclenhco + "/" + yeucautoclenh[currentIndex] + " <color=yellow>cờ Tộc Lệnh</color>";
        transform.GetChild(0).transform.Find("btnDoiRong").transform.GetChild(0).GetComponent<Text>().text = YeuCau[currentIndex];
    }    
    public GameObject contentRong;
    private void GetDraInventory()
    {
        for (int i = 1; i < contentRong.transform.childCount; i++)
        {
            Destroy(contentRong.transform.GetChild(i).gameObject);
        }
        if (Inventory.ins.TuiRong.transform.childCount > 1)
        {
            for (int i = 0; i < Inventory.ins.TuiRong.transform.childCount - 1; i++)
            {
                if (Inventory.ins.TuiRong.transform.GetChild(i).transform.childCount > 0)
                {
                    ItemDragon itemdra = Inventory.ins.TuiRong.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemDragon>();
                    string nameimg = itemdra.transform.GetChild(0).GetComponent<Image>().sprite.name;
                    debug.Log("name img " + nameimg + " namesprite " + nameSpriteRongChon[currentIndex]);
                    if (nameimg == nameSpriteRongChon[currentIndex])
                    {
                        if (int.Parse(itemdra.txtSao.text) > 15)
                        {
                            SetRong();
                        }

                        void SetRong()
                        {
                            GameObject rong = Instantiate(contentRong.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
                            rong.transform.SetParent(contentRong.transform, false);
                            // ite
                            rong.name = itemdra.name;
                            Image imgRong = rong.transform.GetChild(0).GetComponent<Image>();
                            imgRong.sprite = Inventory.LoadSpriteRong(nameSpriteRongChon[currentIndex]); imgRong.SetNativeSize();
                            rong.transform.GetChild(1).GetComponent<Text>().text = itemdra.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
                            rong.transform.GetChild(2).GetComponent<Text>().text = itemdra.txtSao.text;
                            // AddSlotRong(item.name, item.nameObjectDragon, ""); //item.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text
                            rong.SetActive(true);
                        }
                    }
                }
            }
        }
    }    
}
