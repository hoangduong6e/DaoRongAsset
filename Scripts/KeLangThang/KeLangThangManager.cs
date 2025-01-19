using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeLangThangManager:MonoBehaviour
{
    public static KeLangThangManager ins;
    [SerializeField] GameObject btnKeLangThang;
    [SerializeField] Text txtTime;
     GameObject xuathien;

    private Queue<byte> daoXuatHien = new();
    private Queue<string> name_KLT = new();
    private void Awake()
    {
        ins = this;
        xuathien = ins.btnKeLangThang.transform.GetChild(1).gameObject;
    }
    private static readonly Dictionary<string, KeLangThangFactory> factories = new Dictionary<string, KeLangThangFactory>
    {
        { "ConLan", new ConLanFactory() },
       // { "KeLangThang", new KeLangThangFactory() }
    };

    public static void CreateAllKeLangThang(JSONObject data)
    {
        foreach (string nameKLT in data["KeLangThang"].keys)
        {
            ins.name_KLT.Enqueue(nameKLT);// lấy tên trong phần tử cuối cùng
            
            foreach (string KTL in data["KeLangThang"][nameKLT].keys)
            {
               KeLangThangFactory keLangThangFactory = factories[nameKLT];
                JSONObject dataCreate = data["KeLangThang"][nameKLT][KTL];
                dataCreate.AddField("nameobject",nameKLT);

                  byte daoadd = (byte)(byte.Parse(dataCreate["dao"].ToString()) + 1);
                ins.daoXuatHien.Enqueue(daoadd);
                  keLangThangFactory.Create(dataCreate);
              
            }
        }
        ins.LoadXuatHien();
    }
    public void LoadXuatHien()
    {
        //if(daoXuatHien.Peek() - 1 == CrGame.ins.DangODao)
        //{
        //    daoXuatHien.Dequeue(); 
        //    name_KLT.Dequeue(); 
        //}
        ins.btnKeLangThang.SetActive(true);
        
        if(daoXuatHien.Count > 0)
        {
         Image imgicon = ins.btnKeLangThang.transform.GetChild(0).GetComponent<Image>();
        imgicon.sprite = LoadIcon(ins.name_KLT.Dequeue());
        imgicon.SetNativeSize();

        
       
        xuathien.SetActive(true);
        Text txtdao = xuathien.transform.GetChild(0).GetComponent<Text>();
        txtdao.text = daoXuatHien.Dequeue().ToString();
        }
        else xuathien.SetActive(false);

    }
   // GameData/KeLangThang/Object/ConLan
    public void OnClickKLT()
    {
        xuathien.SetActive(false);
        if(daoXuatHien.Count > 0)
        {
            // CrGame.ins.DangODao = 0;
            int daoxh = daoXuatHien.Peek() - 1;
            int dao = daoxh - CrGame.ins.DangODao;
            CrGame.ins.QuaDao(dao);
            LoadXuatHien();

            //đảo mình = 4;
            //đảo xuất hiện = 2;


        }
    }

    public GameObject LoadObject(string s)
    {
          return Resources.Load("GameData/KeLangThang/Object/"+s) as GameObject;
    }

     public static Sprite LoadIcon(string s)
    {
        // Tải sprite từ Resources
        Sprite sprite = Resources.Load<Sprite>("GameData/KeLangThang/icon/" + s);

        // Nếu sprite không tồn tại, trả về sprite mặc định
        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>("GameData/Sprite/Default");
        }

        return sprite;
    }
}//
