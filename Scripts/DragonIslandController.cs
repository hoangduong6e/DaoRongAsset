﻿using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public abstract class DragonIslandController : MonoBehaviour
{
    [SerializeField]
    protected MoveIslandStatus moveIslandStatus;
    protected float time = 0,
        maxtime = 2,
        speed,
        vitribongY;

    protected bool drag = false,
        boi = false,
        dichuyen = true,
        roi = false;

    private bool xem = false, openinfo = false;

    protected Animator anim;
    protected Transform chayvedao;
    protected GameObject hieuungboi;
    public Transform bongrong;
    protected Rigidbody2D rigid;

    public Text txtNameRong { get; set; }

    public bool doi { get; set; }
  //  public Text setTxtNameRong { set { txtNameRong = value; } }
    public string SetNameRong { 
        set 
        { 
            if(value != "")
            {
                txtNameRong.text = value;
                txtNameRong.gameObject.SetActive(true);
            }
        }
    }

   // public DataDragonIsland data { get; set; }
    
    protected void Startt()
    {
        vitribongY = gameObject.transform.position.y - bongrong.transform.position.y;
        rigid = GetComponent<Rigidbody2D>();
        speed = Random.Range(0.3f, 0.75f);
        //txtNameRong.transform.parent.transform.Find("BieuCamRong").transform.SetParent(transform.);
        //RanRun();
        //GameObject CanvasDraIsland = Instantiate(Inventory.ins.GetObj("CanvasDraIsland"), transform.position, Quaternion.identity);
    }
    protected void Awakee()
    {
        anim = GetComponent<Animator>();
        bongrong = transform.GetChild(0).Find("bong").transform;
    }    
    protected abstract void RanRun();
    protected abstract void UpdateAnimator();
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
                    hieuungboi = Instantiate(Inventory.ins.GetEffect("HieuUngBoi"), new Vector3(transform.position.x, transform.position.y - 0.1f), Quaternion.identity) as GameObject;
                    hieuungboi.transform.SetParent(gameObject.transform);
                    //   SpBong.enabled = false;
                    if (collision.CompareTag("duoinuoctrai"))
                    {
                        chayvedao = transform.parent.transform.GetChild(0).transform;
                        scale(-1);
                    }
                    if (collision.CompareTag("duoinuocduoi"))
                    {
                        chayvedao = transform.parent.transform.GetChild(1).transform;
                    }
                    if (collision.CompareTag("duoinuocphai"))
                    {
                        chayvedao = transform.parent.transform.GetChild(2).transform;
                        scale(1);
                    }
                    boi = true;
                    Vector3 newvec = bongrong.transform.GetChild(0).transform.position;
                    newvec.y += 0.3f;
                    hieuungboi.transform.position = newvec;
                    hieuungboi.SetActive(true);
                    UpdateAnimator();
                    return;
                }
            }
        }
        //if (collision.name == "bong" + gameObject.name)
        //{
        //    if (boi == false && roi)
        //    {
        //        AudioManager.PlaySound("roixuongdat");
        //        GameObject Buibay = Instantiate(crgame.Bui, new Vector3(gameObject.transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
        //        Buibay.SetActive(true);
        //        Destroy(Buibay, 0.3f);
        //    }
        //    rigid.bodyType = RigidbodyType2D.Static;
        //    rigid.bodyType = RigidbodyType2D.Kinematic;
        //    bongrong.transform.position = new Vector3(bongrong.transform.position.x, gameObject.transform.position.y - vitribong.y);
        //    bongrong.transform.SetParent(gameObject.transform);
        //    if (roi)
        //    {
        //        roi = false;
        //    }
        //}
        if (collision.name == "vitritrai" || collision.name == "vitriphai")
        {
            chayvedao = transform.parent.transform.GetChild(1).transform;
        }
        if (collision.name == "vitriduoi" || collision.name == "hangraoduoiphai")
        {
            //     SpBong.enabled = true;
            // Destroy(hieuung);
            //  boi = false; dichuyen = true;
            moveIslandStatus = MoveIslandStatus.Up;
        }
        if (collision.name == "hangraotrenphai")
        {
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Left;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.Left;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.UpLeft;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.name == "hangraotren")
        {
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Down;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.DownLeft;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.DownRight;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.name == "hangraotrentrai")
        {
            int ran = Random.Range(1, 4);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Down;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.DownLeft;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.DownRight;
                    break;
                case 4:
                    moveIslandStatus = MoveIslandStatus.Left;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.name == "hangraoduoitrai")
        {
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Right;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.DownRight;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.UpRight;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.name == "hangraoduoiphai")
        {
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Up;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.UpLeft;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.Left;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.name == "hangraoduoi")
        {
            int ran = Random.Range(1, 3);
            switch (ran)
            {
                case 1:
                    moveIslandStatus = MoveIslandStatus.Up;
                    break;
                case 2:
                    moveIslandStatus = MoveIslandStatus.UpLeft;
                    break;
                case 3:
                    moveIslandStatus = MoveIslandStatus.UpRight;
                    break;
            }
            UpdateAnimator();
        }
        if (collision.transform.parent.name == "thucan")
        {
            if (doi)
            {
                AudioManager.PlaySound("rongan");
                //InfoThucAn JSON = new InfoThucAn(gameObject.name, collision.name, crgame.DangODao);
                // string data = JsonUtility.ToJson(JSON);
                // net.socket.Emit("rongan", new JSONObject(data));
                DragonIslandManager.RongAn(gameObject.name, collision.name,gameObject.transform);
                Destroy(collision.transform.parent.gameObject);
                dichuyen = true;
                //Randichuyen();
                txtNameRong.transform.parent.transform.Find("BieuCamRong").gameObject.SetActive(false);
                doi = false;
            }
        }
    }
    protected virtual void scale(float x)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * x;
        transform.localScale = scale;

        Vector3 scaleCanvasIsland = txtNameRong.transform.localScale;
        scaleCanvasIsland.x = Mathf.Abs(scaleCanvasIsland.x) * x;
        txtNameRong.transform.localScale = scaleCanvasIsland;

    }
    protected void UpdateTransform()
    {
        if (time <= maxtime)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            maxtime = Random.Range(2, 4);
            speed = Random.Range(0.3f, 0.75f);
            RanRun();
        }
        if (dichuyen)
        {
            switch (moveIslandStatus)
            {
                case MoveIslandStatus.Left:
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    scale(1);
                    break;
                case MoveIslandStatus.Right:
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    scale(-1);
                    break;
                case MoveIslandStatus.Down:
                    transform.position += Vector3.down * speed * Time.deltaTime;
                    break;
                case MoveIslandStatus.Up:
                    transform.position += Vector3.up * speed * Time.deltaTime;
                    break;
                case MoveIslandStatus.DownLeft:
                    transform.position += Vector3.down * speed * Time.deltaTime;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    scale(1);
                    break;
                case MoveIslandStatus.DownRight:
                    transform.position += Vector3.down * speed * Time.deltaTime;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    scale(-1);
                    break;
                case MoveIslandStatus.UpLeft:
                    transform.position += Vector3.up * speed * Time.deltaTime;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                    scale(1);
                    break;
                case MoveIslandStatus.UpRight:
                    transform.position += Vector3.up * speed * Time.deltaTime;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                    scale(-1);
                    break;
            }
          //  UpdateAnimator();
        }
        if (boi)
        {
            dichuyen = false;
            if (transform.position == transform.parent.transform.GetChild(1).transform.position)
            {
                //SpBong.enabled = true;
                Destroy(hieuungboi);
                boi = false; dichuyen = true;
                moveIslandStatus = MoveIslandStatus.Up;
            }
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chayvedao.transform.position.z), chayvedao.transform.position, 2 * Time.deltaTime);
        }
        else
        {
            //if (Friend.ins.QuaNha)
            //{


            //    return;
            //}    
            GioiHanDiChuyen();
         
            //float limitPosX = Mathf.Clamp(transform.position.x, GamIns.ins.minX, GamIns.ins.MaxX);
            //float limitPosY = Mathf.Clamp(transform.position.y, GamIns.ins.minY, GamIns.ins.MaxY);
            //transform.position = new Vector3(limitPosX, limitPosY, transform.position.z);
        }
        ScanFood();
    }
    protected abstract void ScanFood();
    protected abstract void GioiHanDiChuyen();
    protected void RanDiChuyen()
    {
        byte randichuyen = (byte)Random.Range(1, 16);
        switch (randichuyen)
        {
            case 1: moveIslandStatus = MoveIslandStatus.Left; break;
            case 2: moveIslandStatus = MoveIslandStatus.Right; break;
            case 3:
                moveIslandStatus = MoveIslandStatus.Up;
                break;
            case 4:
                moveIslandStatus = MoveIslandStatus.Down;
                break;
            case 5:
                moveIslandStatus = MoveIslandStatus.DownLeft;
                break;
            case 6:
                moveIslandStatus = MoveIslandStatus.DownRight;
                break;
            case 7:
                moveIslandStatus = MoveIslandStatus.UpLeft;
                break;
            case 8:
                moveIslandStatus = MoveIslandStatus.UpRight;
                break;
            case 9:
                moveIslandStatus = MoveIslandStatus.Left;
                break;
            case 10:
                moveIslandStatus = MoveIslandStatus.Right;
                break;
            case 11:
                moveIslandStatus = MoveIslandStatus.Up;
                break;
            case 12:
                moveIslandStatus = MoveIslandStatus.Down;
                break;
            case 15:
                moveIslandStatus = MoveIslandStatus.Idle;
                break;
            case 16:
                moveIslandStatus = MoveIslandStatus.Idle;
                break;
        }
        UpdateAnimator();
    }
    public void StartDrag(PointerEventData data)
    {
        if (boi == false)
        {
            drag = true;
            dichuyen = false;
            bongrong.transform.SetParent(transform.parent.transform.parent.transform);//GameObject.FindGameObjectWithTag("canvasGame").transform);
            SetLayerBong("Rong");
            GetComponent<BoxCollider2D>().enabled = false;
            SetLayerDra("RongBay");
            anim.Play("Pick");
        }
    }
    public void StopDrag(PointerEventData data)
    {
        if (boi == false)
        {
            drag = false;
            if (transform.position.y - bongrong.transform.position.y > vitribongY + 0.1f)
            {
                roi = true;
                rigid.bodyType = RigidbodyType2D.Dynamic;
            }
            bongrong.transform.position = new Vector3(gameObject.transform.position.x, bongrong.transform.position.y);
            if (bongrong.transform.position.y > transform.position.y)
            {
                bongrong.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - vitribongY);
            }

            //  anim.Play("Idlle");
            UpdateAnimator();
            dichuyen = true;
        }
    }
    protected void UpdateDrag()
    {
        if(drag)
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
                bongrong.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - vitribongY);
            }
            //float limitPosX = Mathf.Clamp(transform.position.x, GamIns.ins.minX, GamIns.ins.MaxX);
            //float limitPosY = Mathf.Clamp(transform.position.y, GamIns.ins.minY, GamIns.ins.MaxY);
            //bongrong.transform.position = new Vector3(limitPosX, limitPosY, bongrong.transform.position.z);
            float vitriduoiY = transform.parent.GetChild(1).transform.position.y;
            if (bongrong.transform.position.y < vitriduoiY)
            {
                bongrong.transform.position = new Vector3(bongrong.transform.position.x, vitriduoiY, bongrong.transform.position.z);
            }
            if(bongrong.transform.position.y >= transform.position.y)
            {
                EndRoi();
            }    
        }
        if (boi == false && roi)
        {
            bongrong.transform.position = new Vector3(transform.position.x,bongrong.transform.position.y,bongrong.transform.position.z);
            if (transform.position.y <= bongrong.transform.position.y)
            {
                AudioManager.PlaySound("roixuongdat");
                GameObject Buibay = Instantiate(Inventory.ins.GetEffect("Bui"), new Vector3(gameObject.transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
                Buibay.transform.position = new Vector3(bongrong.transform.GetChild(0).transform.position.x, bongrong.transform.GetChild(0).transform.position.y + 0.3f);
                Buibay.SetActive(true);
                Destroy(Buibay, 0.3f);
                EndRoi();
            }
        }
    }
    protected void EndRoi()
    {
        rigid.bodyType = RigidbodyType2D.Static;
        rigid.bodyType = RigidbodyType2D.Kinematic;
        bongrong.transform.position = new Vector3(bongrong.transform.position.x, gameObject.transform.position.y - vitribongY);
        bongrong.transform.SetParent(gameObject.transform.GetChild(0));

        SetLayerBong("Default");
        if (roi)
        {
            roi = false;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        UpdateAnimator();
        SetLayerDra("Rong");
    }
    protected void SetLayerDra(string layername)
    {
        SortingGroup group = GetComponent<SortingGroup>();
        group.sortingLayerName = layername;
    }
    protected void SetLayerBong(string namelayer)
    {
        Renderer renderer = bongrong.transform.GetChild(0).GetComponent<Renderer>();
        renderer.sortingLayerName = namelayer;
    }

    public void ButtonDown(PointerEventData d)
    {
        Click(true);
    }
    public void ButtonUp(PointerEventData d)
    {
        Click(false);
    }
    private void Click(bool a)
    {
        xem = a;
        if (a)
        {
            Invoke("invoke", 0.3f);
         //   debug.Log("btn down");
        }
        else
        {
          //  debug.Log("btn up");
            if (drag == false && xem == false && openinfo && Friend.ins.QuaNha == false)
            {
                CrGame.ins.TfrongInfo = gameObject.transform;
                GameObject MenuVongtronXemRong = AllMenu.ins.GetCreateMenu("MenuVongtronXemRong");
                GameObject infoRong = MenuVongtronXemRong.transform.GetChild(0).gameObject;
                infoRong.transform.position = gameObject.transform.position;
            }
            AllMenu.ins.DestroyMenu("PanelInfoRong");
        }
        openinfo = a;
    }
    
    void invoke()
    {
        if (drag == false && xem)
        {
            JSONClass datasend = new JSONClass();
            datasend["class"] = "DragonIsland";
            datasend["method"] = "XemTimeTieuHoa";
            datasend["data"]["idrong"] = gameObject.name;
            datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
            //datasend["data"]["dao"] = CrGame.ins.DangODao.ToString();
            NetworkManager.ins.SendServer(datasend, Ok);
            void Ok(JSONNode json)
            {
                if (json["status"].AsString == "0")
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
                    txtCapRong.text = "Cấp: " + json["level"].AsString;
                    txtnamerong.text = DragonIslandManager.ParseName(json["namerong"].AsString, json["hoangkim"].AsBool, json["sao"].AsString);
                    txtExpRong.text = json["exp"].AsString + "/" + json["maxexp"].AsString;
                    int sec = json["timeconlai"].AsInt, min = 0, h = 0;
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
                    CrGame.ins.LoadExpRong(decimal.Parse(json["exp"].AsString), decimal.Parse(json["maxexp"].AsString));
                    panelInfo.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f);
                    //txtnamerong.text = "<color=#ff00ffff>" + tenrong + "</color>" + "<color=#ffff00ff>" + "(" + sao + " " + "Sao)" + " </color>";
                    if (json["level"].AsString == "0") txtChienBinh.text = "-Baby-";
                    else if (json["level"].AsString == "1" || json["tienhoa"].AsString == "2") txtChienBinh.text = "-Chiến Binh-";
                    else if (json["level"].AsString == "2") txtChienBinh.text = "-Siêu Chiến Binh-";
                   // crgame.InfoRong.SetActive(true);
                }
            }
        }
    }
}
