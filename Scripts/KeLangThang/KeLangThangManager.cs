using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeLangThangManager : MonoBehaviour
{
    public static KeLangThangManager ins;
    [SerializeField] GameObject btnKeLangThang,btnThongBao;
    [SerializeField] Text txtTime;
    GameObject xuathien;

    private Queue<byte> daoXuatHien = new();
    private Queue<string> name_KLT = new();
    public List<string> id_KLT = new();

    public float timeInSeconds = 120f; // Thời gian ban đầu
    private float timeAccumulator = 0f; // Biến để tích lũy thời gian
    private bool displaytime = false;

    public bool setBtnKLT { 
        set 
        {
            btnKeLangThang.SetActive(value);
            btnThongBao.SetActive(!value);
        } 
    }
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
            foreach (string KTL in data["KeLangThang"][nameKLT].keys)
            {
                KeLangThangFactory keLangThangFactory = factories[nameKLT];
                JSONObject dataCreate = data["KeLangThang"][nameKLT][KTL];

                dataCreate.AddField("nameobject", nameKLT);

                byte dao = byte.Parse(dataCreate["dao"].ToString());
                byte daoadd = (byte)(dao + 1);
               
                if (!ins.id_KLT.Contains(dataCreate["id"].str))
                {
                  
                    ins.daoXuatHien.Enqueue(daoadd);
                    ins.name_KLT.Enqueue(nameKLT);
                    ins.id_KLT.Add(dataCreate["id"].str);
                }
                keLangThangFactory.Create(dataCreate);

            }
        }
        ins.LoadXuatHien();
    }
    private void CheckDaoHienTai()
    {
        if (daoXuatHien.Count == 0) return;
        if (daoXuatHien.Peek() - 1 == CrGame.ins.DangODao)
        {
            daoXuatHien.Dequeue();
            name_KLT.Dequeue();
        }
    }
    public void LoadXuatHien()
    {
        CheckDaoHienTai();
        setBtnKLT = true;

        if (daoXuatHien.Count > 0)
        {
            Image imgicon = ins.btnKeLangThang.transform.GetChild(0).GetComponent<Image>();
            imgicon.sprite = LoadIcon(ins.name_KLT.Peek());
            imgicon.SetNativeSize();

            xuathien.SetActive(true);
            Text txtdao = xuathien.transform.GetChild(0).GetComponent<Text>();
            txtdao.text = daoXuatHien.Peek().ToString();
           // id_KLT.RemoveAt(0);
        }
        else xuathien.SetActive(false);

    }
    // GameData/KeLangThang/Object/ConLan
    public void OnClickKLT()
    {
        CheckDaoHienTai();
        xuathien.SetActive(false);
        if (daoXuatHien.Count > 0)
        {
            // CrGame.ins.DangODao = 0;
            int daoxh = daoXuatHien.Peek() - 1;
            int dao = daoxh - CrGame.ins.DangODao;
            CrGame.ins.QuaDao(dao);
            //id_KLT.RemoveAt(0);
            name_KLT.Dequeue();
            daoXuatHien.Dequeue();
            LoadXuatHien();

            //đảo mình = 4;
            //đảo xuất hiện = 2;


        }
    }
    public void SetTimeKLT(float time)
    {
        timeInSeconds = time;
        displaytime = true;
        setBtnKLT = true;
    }

    void Update()
    {
        if (displaytime)
        {
            if (timeInSeconds > 0)
            {
                // Tích lũy thời gian trôi qua
                timeAccumulator += Time.deltaTime;

                // Nếu tích lũy >= 1 giây, giảm thời gian 1 lần
                if (timeAccumulator >= 1f)
                {
                    timeInSeconds -= 1f; // Giảm 1 giây
                    timeAccumulator -= 1f; // Reset lại bộ đếm tích lũy

                    UpdateTimerDisplay();
                }
            }
            else
            {
                txtTime.text = "";
                displaytime = false;
            }
        }
       
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        txtTime.text = $"{minutes:00}:{seconds:00}";
    }
    public GameObject LoadObject(string s)
    {
        return Resources.Load("GameData/KeLangThang/Object/" + s) as GameObject;
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
