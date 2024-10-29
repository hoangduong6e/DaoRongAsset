using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.3f;
    public float time, maxtime;Friend friend;
    bool left, right, up, down;public bool dichuyen = true;
    public float timeconlai = 0,maxtimeconlai;
    public byte Levelrong = 0;
    public byte sao, tienhoa;
    public string tenrong,timedoi;
    public bool doi = false;
    public GameObject thongbaodoi; ThuyenThucAn thuyen; CrGame crgame; NetworkManager net;
    public Text txtnamerong;
    private Vector3 _target; Animator anim;
    Camera Camera;
    bool drag = false;
     Vector3 Scale,Scalename; GameObject dungthucan;
    // public string expRong = "0/100";
    public int Exp, maxExp = 50; Rigidbody2D rigid;bool boi = false,roi = false;
    float scaledra,scaleName;Transform chayvedao; GameObject hieuung,bongrong;Vector3 vitribong;SpriteRenderer SpBong;
    void Start()
    {
       // debug.Log("tét");
        anim = GetComponent<Animator>();
        _target = new Vector3(transform.position.x, transform.position.y,0);
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        crgame = Camera.GetComponent<CrGame>();
        friend = Camera.GetComponent<Friend>();
        maxtime = Random.Range(3, 6);
        bongrong = transform.GetChild(1).gameObject;
        bongrong.name = "bong" + gameObject.name;
        thuyen = Camera.GetComponent<ThuyenThucAn>();
        SpBong = bongrong.GetComponent<SpriteRenderer>();
        net = Camera.GetComponent<NetworkManager>();
        Scale = transform.localScale;
        Scalename = txtnamerong.transform.localScale;
        scaleName = Scalename.x;
        scaledra = Scale.x;
        dungthucan = GameObject.Find("DungThucAn");
        anim.SetInteger("TienHoa", tienhoa);
        Randichuyen(); rigid = gameObject.GetComponent<Rigidbody2D>();
        vitribong.y = gameObject.transform.position.y - bongrong.transform.position.y;
    }
    private void OnEnable()
    {
        if(anim != null)
        anim.SetInteger("TienHoa", tienhoa);
    }
    // Update is called once per frame
    void Update()
    {
     //   transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (timeconlai > 0)
        {
            timeconlai -= Time.deltaTime;
        }
        ScanFood();
        DragonMove();
        if (drag)
        {
            bongrong.transform.position = new Vector3(transform.position.x, bongrong.transform.position.y);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mousePosition.y -= 1;
            transform.Translate(mousePosition);
            if (transform.position.y >= bongrong.transform.position.y + 4.5f)
            {
                bongrong.transform.position = new Vector3(gameObject.transform.position.x, bongrong.transform.position.y + 2);
            }
            if (bongrong.transform.position.y > transform.position.y)
            {
                bongrong.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - vitribong.y);
            }
        }
        if (boi)
        {
            dichuyen = false;
            //   var step = 5 * Time.deltaTime;
            if(transform.position == transform.parent.transform.GetChild(1).transform.position)
            {
                SpBong.enabled = true;
                Destroy(hieuung);
                boi = false; dichuyen = true;
                up = true;
            }
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chayvedao.transform.position.z), chayvedao.transform.position, 2 * Time.deltaTime);
        }
    }
    void DragonMove()
    {
        if (time < maxtime)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            maxtime = Random.Range(2, 5);
            speed = Random.Range(0.3f, 0.6f);
            Randichuyen();;
        }
        if (dichuyen)
        {
            anim.SetBool("dichuyen", true);
            if (left)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                scale(scaledra);
            }
            if (right)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                scale(-scaledra);
            }
            if (up)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
            }
            if (down)
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
            }
        }

        if (dichuyen == false)
        {
            anim.SetBool("dichuyen", false);
        }
        //debug.Log(dichuyen);
    }
    void Randichuyen()
    {
        if(boi == false)
        {
            int randichuyen = Random.Range(1, 16);
            left = false; right = false; up = false; down = false; dichuyen = true;
            switch (randichuyen)
            {
                case 1:
                    left = true;
                    break;
                case 2:
                    right = true;
                    break;
                case 3:
                    up = true;
                    break;
                case 4:
                    down = true;
                    break;
                case 5:
                    down = true;
                    left = true;
                    break;
                case 6:
                    down = true;
                    right = true;
                    break;
                case 7:
                    up = true;
                    left = true;
                    break;
                case 8:
                    up = true;
                    right = true;
                    break;
                case 9:
                    left = true;
                    break;
                case 10:
                    right = true;
                    break;
                case 11:
                    up = true;
                    break;
                case 12:
                    down = true;
                    break;
                case 13:
                    down = true;
                    left = true;
                    break;
                case 14:
                    down = true;
                    right = true;
                    break;
                case 15:
                    left = false; right = false; up = false; down = false; dichuyen = false;
                    break;
                case 16:
                    left = false; right = false; up = false; down = false; dichuyen = false;
                    break;
            }
        }
    }
    void scale(float x)
    {
        Scale.x = x;
        transform.localScale = Scale;
        if(x > 0)
        {
            Scalename.x = scaleName;
            Scalename.y = scaleName;
        }
        else
        {
            Scalename.x = -scaleName;
            Scalename.y = scaleName;
        }
        txtnamerong.transform.localScale = Scalename;
    }
    void ScanFood()
    {
        if (doi)
        {
            if (dungthucan.transform.childCount > 2)
            {
                foreach (Transform child in dungthucan.transform)
                {
                    if (Mathf.Abs(transform.position.x - child.transform.position.x) <= 4.5f &&
                        Mathf.Abs(transform.position.y - child.transform.position.y) <= 4.5f)
                    {
                        if (transform.position.x < child.transform.position.x)
                        {
                            scale(-scaledra);
                        }
                        else
                        {
                            scale(scaledra);
                        }
                        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, child.transform.position.z), child.transform.position, 1.2f * Time.deltaTime);
                    }
                }
            }
        }
    }
    bool xem = false,openinfo = false;
    public void ButtonDown(bool a)
    {
        xem = a;
        if(a)
        {
            Invoke("invoke",0.3f);
        }
        else
        {
            if (drag == false && xem == false && openinfo && friend.QuaNha == false)
            {
                crgame.TfrongInfo = gameObject.transform;
                GameObject MenuVongtronXemRong = AllMenu.ins.GetCreateMenu("MenuVongtronXemRong");
                GameObject infoRong = MenuVongtronXemRong.transform.GetChild(0).gameObject;
                //for (int i = 0; i < infoRong.transform.childCount; i++)
                //{
                //    Button btn = infoRong.transform.GetChild(i).GetComponent<Button>();
                //    btn.onClick.RemoveAllListeners();
                //    switch(i)
                //    {
                //        case 0:
                //            btn.onClick.AddListener(Inventory.ins.PhongChienTuong);
                //            break;
                //        case 1:
                //            btn.onClick.AddListener(Inventory.ins.XemInfoRong);
                //            break;
                //        case 2:
                //         //   btn.onClick.AddListener(Inventory.ins.);
                //            break;
                //        case 3:
                //            btn.onClick.AddListener(Inventory.ins.CatRong);
                //            break;
                //        case 4:
                //            btn.onClick.AddListener(Inventory.ins.XemTenRong);
                //            break;
                //    }    
                   
                //}
                infoRong.transform.position = gameObject.transform.position;
            }
            AllMenu.ins.DestroyMenu("PanelInfoRong");
          //  crgame.InfoRong.SetActive(false);
        }
        openinfo = a;
    }
    void invoke()
    {
        if(drag == false && xem)
        {
            GameObject panelInfo = AllMenu.ins.GetCreateMenu("PanelInfoRong");
            openinfo = false;
          //  crgame.maxTimeDoi = maxtimeconlai;
           // crgame.timeDoi = timeconlai;
           Text txtCapRong = panelInfo.transform.GetChild(4).GetComponent<Text>();    
           Text txtTimeTieuHoa = panelInfo.transform.GetChild(2).GetComponent<Text>();
           Text txtnamerong = panelInfo.transform.GetChild(0).GetComponent<Text>();
           Text txtChienBinh = panelInfo.transform.GetChild(3).GetComponent<Text>();
            Text txtExpRong = panelInfo.transform.GetChild(5).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
            txtCapRong.text = "Cấp: " + Levelrong;

            txtExpRong.text = Exp + "/" + maxExp;
            int sec = (int)timeconlai, min = 0, h = 0;
            while (sec > 60)
            {
                sec -= 60;
                min += 1;
            }
            while (min > 60)
            {
                min -= 60;
                h += 1;
            }
            txtTimeTieuHoa.text = h + ":" + min + ":" + sec;
            crgame.LoadExpRong(Exp, maxExp);
            panelInfo.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f);
            txtnamerong.text = "<color=#ff00ffff>" + tenrong + "</color>" + "<color=#ffff00ff>" + "(" + sao + " " + "Sao)" + " </color>";
            if (Levelrong == 0 ) txtChienBinh.text = "-Baby-";
            else if(Levelrong == 1 || tienhoa == 2) txtChienBinh.text = "-Chiến Binh-";
            else if (Levelrong == 2) txtChienBinh.text = "-Siêu Chiến Binh-";
           // crgame.InfoRong.SetActive(true);
        }
    }
    public void Drag()
    {
        if (boi == false)
        {
            drag = true;
            bongrong.transform.SetParent(GameObject.Find("CanvasGame").transform);
        }                
    }
    public void EndDrag()
    {
        if(boi == false)
        {
            drag = false;
            if (transform.position.y - bongrong.transform.position.y > vitribong.y + 0.1f)
            {
                roi = true;
                rigid.bodyType = RigidbodyType2D.Dynamic;
            }
            bongrong.transform.position = new Vector3(gameObject.transform.position.x, bongrong.transform.position.y);
            if (bongrong.transform.position.y > transform.position.y)
            {
                bongrong.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - vitribong.y);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        maxtime = Random.Range(3, 5);
        time = 0;
        if (collision.CompareTag("duoinuoctrai") || collision.CompareTag("duoinuocduoi") || collision.CompareTag("duoinuocphai"))
        {
            if (drag == false)
            {
                //debug.Log("Rotxuongnuoc");
                if (boi == false)
                {
                    AudioManager.PlaySound("roixuongnuoc");
                    hieuung = Instantiate(Inventory.ins.GetEffect("HieuUngBoi"), new Vector3(transform.position.x, transform.position.y - 0.1f), Quaternion.identity) as GameObject;
                    hieuung.transform.SetParent(gameObject.transform);
                    SpBong.enabled = false;
                    if (collision.CompareTag("duoinuoctrai"))
                    {
                        chayvedao = transform.parent.transform.GetChild(0).transform;
                        scale(-scaledra);
                    }
                    if (collision.CompareTag("duoinuocduoi"))
                    {
                        chayvedao = transform.parent.transform.GetChild(1).transform;
                    }
                    if (collision.CompareTag("duoinuocphai"))
                    {
                        chayvedao = transform.parent.transform.GetChild(2).transform;
                        scale(scaledra);
                    }
                    boi = true;
                    return;
                }
            }
        }
        if (collision.name == "bong" + gameObject.name)
        {
            if(boi == false && roi)
            {
                AudioManager.PlaySound("roixuongdat");
                GameObject Buibay = Instantiate(Inventory.ins.GetEffect("Bui"), new Vector3(gameObject.transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
                Buibay.SetActive(true);
                Destroy(Buibay, 0.3f);
            }
            rigid.bodyType = RigidbodyType2D.Static;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            bongrong.transform.position = new Vector3(bongrong.transform.position.x, gameObject.transform.position.y - vitribong.y);
            bongrong.transform.SetParent(gameObject.transform);
            if (roi)
            {
                roi = false;
            }
        }
        if (collision.name == "vitritrai" || collision.name == "vitriphai")
        {
            chayvedao = transform.parent.transform.GetChild(1).transform;
        }
        if (collision.name == "vitriduoi" || collision.name == "hangraoduoiphai")
        {
            SpBong.enabled = true;
            Destroy(hieuung);
            boi = false; dichuyen = true;
            up = true;
        }
        if (collision.name == "hangraotrenphai")
        {
            left = false; right = false; up = false; down = false;
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    left = true;
                    break;
                case 2:
                    left = true;
                    down = true;
                    break;
                case 3:
                    left = true;
                    up = true;
                    break;
            }
        }
        if (collision.name == "hangraotren")
        {
            left = false; right = false; up = false; down = false;
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    down = true;
                    break;
                case 2:
                    left = true;
                    down = true;
                    break;
                case 3:
                    right = true;
                    down = true;
                    break;
            }
        }
        if (collision.name == "hangraotrentrai")
        {
            left = false; right = false; up = false; down = false;
            int ran = Random.Range(1, 4);
            switch (ran)
            {
                case 1:
                    down = true;
                    break;
                case 2:
                    left = true;
                    down = true;
                    break;
                case 3:
                    right = true;
                    down = true;
                    break;
                case 4:
                    left = true;
                    break;
            }
        }
        if (collision.name == "hangraoduoitrai")
        {
            left = false; right = false; up = false; down = false;
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    right = true;
                    break;
                case 2:
                    right = true;
                    down = true;
                    break;
                case 3:
                    right = true;
                    up = true;
                    break;
            }
        }
        if (collision.name == "hangraoduoiphai")
        {
            left = false; right = false; up = false; down = false;
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    up = true;
                    break;
                case 2:
                    up = true;
                    left = true;
                    break;
                case 3:
                    left = true;
                    break;
            }
        }
        if (collision.name == "hangraoduoi")
        {
            left = false; right = false; up = false; down = false; dichuyen = true;
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    up = true;
                    break;
                case 2:
                    up = true;
                    left = true;
                    break;
                case 3:
                    up = true;
                    right = true;
                    break;
            }
        }
        if (collision.transform.parent.name == "thucan")
        {
            if (doi)
            {
                AudioManager.PlaySound("rongan");
                //InfoThucAn JSON = new InfoThucAn(gameObject.name, collision.name, crgame.DangODao);
                // string data = JsonUtility.ToJson(JSON);
                // net.socket.Emit("rongan", new JSONObject(data));
                DragonIslandManager.RongAn(gameObject.name, collision.name,transform);
                Destroy(collision.transform.parent.gameObject);
                dichuyen = true;
                Randichuyen();
                thongbaodoi.SetActive(false);
                doi = false;
            }
        }
    }
    public void ThuongAn(int exp,int exprong)
    {
        GameObject prefabthuong = Instantiate(thuyen.ThuongChoRongAnPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity) as GameObject;
        //  prefabthuong.transform.SetParent(GameObject.Find("Canvas").transform);
        Text txtTangExp = prefabthuong.transform.GetChild(0).GetComponent<Text>();
        txtTangExp.text = exp + "";
        Text txtTangExpRong = prefabthuong.transform.GetChild(1).GetComponent<Text>();
        txtTangExpRong.text = exprong + "";
        prefabthuong.SetActive(true);
        Destroy(prefabthuong, 3);
    }
    public void LoadExpRong(int exp,int max, int level, byte tien_hoa = 0)
    {
        if(level > Levelrong)
        {
            GameObject hieuung = Instantiate(Inventory.ins.GetObj("HieuUng1"), new Vector3(transform.position.x, transform.position.y + 0.5f), Quaternion.identity) as GameObject;
            hieuung.transform.SetParent(GameObject.Find("CanvasGame").transform, true);
            hieuung.SetActive(true);
            Destroy(hieuung, 1.6f);
            tienhoa += tien_hoa; 
            anim.SetInteger("TienHoa", tienhoa);
        }
        Levelrong = (byte)level;
        maxExp = max;
        Exp = exp;
    }
}
//[SerializeField]
//public class InfoThucAn
//{
//    public string namerong, namethucan;
//    public int dangodao;
//    public InfoThucAn(string NameRong,string NameThucAn,int dao)
//    {
//        namerong = NameRong;namethucan = NameThucAn;
//        dangodao = dao;
//    }
//}

