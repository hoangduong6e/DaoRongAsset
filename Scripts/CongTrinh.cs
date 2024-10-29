using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CongTrinh : MonoBehaviour
{
    // Start is called before the first frame update
  
    public string nameCongtrinh = "DatTrong";
    public byte levelCongtrinh = 0; public byte idCongtrinh;public bool thuhoach = false;
    public GameObject dcthuhoach;
    private void Start()
    {
        LoadImg();
    }
    private void OnEnable()
    {
        if (GetComponent<Animator>().runtimeAnimatorController)
        {
            GetComponent<Animator>().Play("level" + CrGame.ins.GetAnimationCongTrinh(levelCongtrinh));
        }
    }
    public bool Xemthuhoach
    {
        get { return thuhoach; }
    }
    public void LoadImg()
    {
        SpriteRenderer sprender = GetComponent<SpriteRenderer>();
        Image imgbtn = gameObject.transform.GetChild(0).GetComponent<Image>();
        if (levelCongtrinh > 0)
        {
            if (nameCongtrinh != "NuiThanBi")
            {
                GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator("CongTrinh/" + nameCongtrinh + "/" + "level" + CrGame.ins.GetAnimationCongTrinh(levelCongtrinh));// GameObject.Find("SpriteCongTrinh" + nameCongtrinh).GetComponent<Animator>().runtimeAnimatorController;
                                                                                                                                                                                             // anim.Play("level" + crGame.GetAnimationCongTrinh(levelCongtrinh));

                sprender.enabled = true;
                imgbtn.color = new Color(0, 0, 0, 0);
                scale(imgbtn.gameObject, 0.02f, 0.02f);

                if (gameObject.transform.childCount == 1)
                {
                    GameObject buombuom = Instantiate(Inventory.ins.GetEffect("BuomBuomXanhBay"), transform.position, Quaternion.identity) as GameObject;
                    buombuom.transform.SetParent(gameObject.transform);
                    buombuom.SetActive(true);
                }
            }    
            else
            {
                GameObject NuiThanBi = Instantiate(Inventory.LoadObjectResource("GameData/Animator/CongTrinh/" + nameCongtrinh),transform.position,Quaternion.identity);

                NuiThanBi.transform.SetParent(transform,false);

                NuiThanBi.transform.position = new Vector3(transform.position.x , transform.position.y + 0.3f);
            }
         
        }
        else
        {
            sprender.enabled = false;
            imgbtn.color = new Color(1, 1, 1, 1);
            scale(imgbtn.gameObject, 0.015f, 0.015f);
            if (gameObject.transform.childCount == 2)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
        }
    }
    void scale(GameObject g, float x,float y)
    {
        Vector3 Scale;
        Scale = g.transform.localScale;
        Scale.x = x;Scale.y = y; 
        g.transform.localScale = Scale;
    }
    public void ResetCongTrinh()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        Image imgbtn = gameObject.transform.GetChild(0).GetComponent<Image>();
        imgbtn.color = new Color(1, 1, 1, 1);
        scale(imgbtn.gameObject, 0.015f, 0.015f);
        if (gameObject.transform.childCount == 2)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        nameCongtrinh = "DatTrong";levelCongtrinh = 0;
    }
    public void XemCongTrinh()
    {
        CrGame.ins.VungCongTrinh = gameObject;
        if(thuhoach == false)
        {
            AudioManager.SoundClick();
            if (levelCongtrinh == 0)
            {
                //crGame.menuCongtrinh.SetActive(true);
                AllMenu.ins.OpenMenu("menuShopCongTrinh");
            }
            else
            {
                if (nameCongtrinh == "NuiThanBi")
                {
                    CrGame.ins.OpenGiaoDienHapThuNgoc();
                }
                else
                {
                    AllMenu.ins.OpenMenu("VongTronCongtrinh");
                    AllMenu.ins.menu["VongTronCongtrinh"].transform.GetChild(0).gameObject.transform.position = gameObject.transform.position;
                    AllMenu.ins.menu["VongTronCongtrinh"].SetActive(true);
                }
 

              //if(nameCongtrinh == "NuiThanBi")
              //  {
              //      AllMenu.ins.menu["VongTronCongtrinh"].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
              //      AllMenu.ins.menu["VongTronCongtrinh"].transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
              //  }
              //else
              //  {
              //      AllMenu.ins.menu["VongTronCongtrinh"].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
              //      AllMenu.ins.menu["VongTronCongtrinh"].transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
              //  }
               // if (levelCongtrinh < 30)
               // {
                    //crGame.StartCoroutine(crGame.Xemct(nameCongtrinh, levelCongtrinh, idCongtrinh, true));
                    //crGame.menuMuaCongtrinh.SetActive(true);
                    // crGame.StartCoroutine(crGame.XemNangCapCongTrinh(nameCongtrinh, levelCongtrinh));#
                 
               // }
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NetworkManager>().socket.Emit("ThuHoachCT",JSONObject.CreateStringObject(idCongtrinh.ToString()));
         //   debug.Log("Thu hoach");
            //Destroy(dcthuhoach);
            //thuhoach = false;
        }
    }
    short idd;
    public void Xem(bool b)
    {
        if (nameCongtrinh != "DatTrong" && b == true)
        {
            if (nameCongtrinh == "NuiThanBi")
            {
                string str = "Núi Thần Bí\n-Hấp thụ Ngọc để gia tăng chỉ số cho rồng ở tất cả các đảo\n-Chỉ số gia tăng tủy theo thuộc tính và cấp của Ngọc\n<color=yellow>Nhấn để chọn Ngọc hấp thụ</color>\n<color=magenta>Lưu ý: Ngọc sẽ mất sau khi sử dụng</color>";
                CrGame.ins.OnThongBaoNhanh(str, 2, false);
                idd = (short)str.Length;
            }
            else
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NetworkManager>().socket.Emit("xeminfo", JSONObject.CreateStringObject(idCongtrinh.ToString()));
                Vector3 tf = transform.position;
                tf.x -= 3;
                AllMenu.ins.GetCreateMenu("infoct", null, b).transform.position = tf;
            }
          
        }
        else
        {
            if(AllMenu.ins.menu.ContainsKey("infoct")) AllMenu.ins.menu["infoct"].SetActive(false);
            CrGame.ins.OffThongBaoNhanh(idd);

        }    
    }
}