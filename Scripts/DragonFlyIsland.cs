
using UnityEngine;
public class DragonFlyIsland : DragonIslandController
{
    private bool Fly = false, moiFly = false;
    private float TimeFly = 0, MaxTimeFly = 6;
    // Start is called before the first frame update
    void Awake()
    {
        Awakee();
    }
    private void Start()
    {
    
        Startt();
       // RanRun();
    }
    private void OnEnable()
    {

      //  debug.Log(gameObject.name + " di chuyen " + dichuyen.ToString());
      //  moveIslandStatus = MoveIslandStatus.Idle;
     //   UpdateAnimator();
        RanRun();
     //   Fly = true;
        MaxTimeFly = Random.Range(3, 5);
    }
    //private void OnMouseDrag()
    //{
    //    StartDrag();
    //}
    //private void OnMouseUp()
    //{
    //    StopDrag();
    //}
    // Update is called once per frame
    void Update()
    {
        if (Fly)
        {
           // SetMaxY = 3;
            bongrong.transform.position = new Vector3(transform.position.x, bongrong.transform.position.y, bongrong.transform.position.z);
            TimeFly += Time.deltaTime;
            if(TimeFly >= MaxTimeFly)
            {
                dichuyen = false;
                transform.position = Vector3.MoveTowards(transform.position, bongrong.transform.position, 1.1f * Time.deltaTime);

                if (transform.position == bongrong.transform.position || boi)
                {
                    DoneFLy();
                }
            }
       
        }
        UpdateDragFly();
        UpdateTransform();
    }

    void DoneFLy()
    {
        Fly = false;
        dichuyen = true;
        bongrong.transform.SetParent(transform.GetChild(0));
        GetComponent<BoxCollider2D>().enabled = true;
        RanRun();
        TimeFly = 0;
        SetLayerDra("Rong");
        UpdateAnimator();
        SetLayerBong("Default");
        moiFly = true;
    }
    private void UpdateDragFly()
    {
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
            if (bongrong.transform.position.y >= transform.position.y)
            {
                EndRoi();
            }
        }
        if (boi == false && roi)
        {
            Fly = false;
          
            bongrong.transform.position = new Vector3(transform.position.x, bongrong.transform.position.y, bongrong.transform.position.z);
            if (transform.position.y <= bongrong.transform.position.y)
            {
                AudioManager.PlaySound("roixuongdat");
                GameObject Buibay = Instantiate(Inventory.ins.GetEffect("Bui"), new Vector3(gameObject.transform.position.x, transform.position.y), Quaternion.identity);
                Buibay.transform.position = new Vector3(bongrong.transform.GetChild(0).transform.position.x, bongrong.transform.GetChild(0).transform.position.y + 0.3f);
                Buibay.SetActive(true);
                Destroy(Buibay, 0.3f);
                RanDiChuyen();
                dichuyen = true;
                EndRoi();
            }
        }
    }

    protected override void GioiHanDiChuyen()
    {
        Transform parent = transform.parent;
        float maxX = parent.transform.position.x + 8;
        float minX = parent.transform.position.x - 8;
        float minY = parent.transform.position.y - 4;
        float maxY = parent.transform.position.y + 5;
        if (Fly) maxY -= 2;
        if (transform.position.x >= maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        if (transform.position.x <= minX) transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        if (transform.position.y >= maxY) transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        if (transform.position.y <= minY) transform.position = new Vector3(transform.position.x, minY, transform.position.z);
    }    
    protected override void RanRun()
    {
        if (!boi)
        {
            byte random = (byte)Random.Range(0, 100);
            if(random >= 50 && !moiFly)
            {
               // debug.Log("Rồng bay trên đảo");
                bongrong.transform.SetParent(transform.parent.transform.parent.transform);
                GetComponent<BoxCollider2D>().enabled = false;
                MaxTimeFly = Random.Range(3,6);
                TimeFly = 0;
                Fly = true;
                RanRun();
                SetLayerDra("RongBay");
                SetLayerBong("Rong");
                //  moveIslandStatus = MoveIslandStatus.Fly;
            }    
            else
            {
                if(!Fly)
                {
                    maxtime = Random.Range(2, 4);
                    RanDiChuyen();
                    moiFly = false;
                }
                else
                {
                    maxtime = Random.Range(0.5f, 1.5f);
                    //debug.Log(transform.position.y);
                    float maxY = transform.parent.transform.position.y + 2.8f;
                    if (transform.position.y < maxY)
                    {
                        byte randichuyen = (byte)Random.Range(1, 5);
                        switch (randichuyen)
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
                            case 4:
                                moveIslandStatus = MoveIslandStatus.Right;
                                break;
                            case 5:
                                moveIslandStatus = MoveIslandStatus.Left;
                                RandomDaBong();
                                break;
                        }
                        if (Random.Range(0, 10) > 6) TimeFly = MaxTimeFly;
                    }
                    else
                    {
                        byte randichuyen = (byte)Random.Range(1, 5);
                        switch (randichuyen)
                        {
                            case 1: moveIslandStatus = MoveIslandStatus.Left; break;
                            case 2: moveIslandStatus = MoveIslandStatus.Right; break;
                            case 3: moveIslandStatus = MoveIslandStatus.DownLeft; break;
                            case 4: moveIslandStatus = MoveIslandStatus.DownRight; break;
                            case 5:
                                moveIslandStatus = MoveIslandStatus.Down;
                                RandomDaBong();
                                break;
                        }
                    }

                   
                }
            
            }
            UpdateAnimator();

        }
    }
    protected override void UpdateAnimator()
    {
        if (Fly)
        {
            anim.Play("Flying");
        }
        else if (moveIslandStatus == 0)
        {
            anim.Play("Idlle");
        }
        else
        {
            anim.SetFloat("speedRun", 1 + (speed - 0.4f) / 3f);
            anim.Play("Walking");
        }
    }
    protected override void ScanFood()
    {
        if (doi && !dabong)
        {
            if (DragonIslandManager.DungThucAn.transform.childCount > 0)
            {
                TimeFly = MaxTimeFly;
                if (!dichuyen) return;
                foreach (Transform child in DragonIslandManager.DungThucAn.transform)
                {
                    if (Mathf.Abs(transform.position.x - child.transform.position.x) <= 6f &&
                        Mathf.Abs(transform.position.y - child.transform.position.y) <= 6f)
                    {
                        if (transform.position.x < child.transform.position.x)
                        {
                            scale(-1);
                        }
                        else
                        {
                            scale(1);
                        }
                        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.transform.position.z), new Vector3(child.transform.position.x, child.transform.position.y, transform.position.z), 1.2f * Time.deltaTime);
                    }
                }
            }
        }
    }
}