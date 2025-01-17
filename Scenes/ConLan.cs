using UnityEngine;
using UnityEngine.EventSystems;

public class ConLan : DragonIslandController
{
    byte solandap = 0;
    float nhan = 1;
    protected override float setRandomSpeed
    {
        get{return speed; }
        set
        {
            if(nhan != 1)
            {
                speed = nhan;
                return;
            }
            speed = value;
        }
    }
    void Awake()
    { 
        Awakee();
       
    }
    private void Start()
    {
        Startt();
    }
    private void OnEnable()
    {
        RanRun();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateDrag();
        UpdateTransform();
    }
   
    protected override void RanRun()
    {
        if (!boi)
        {
            //debug.Log("ran runnnnnnn");
            RanDiChuyen();
            UpdateAnimator();
        }
    }
    protected override void UpdateAnimator()
    {
        if(nhan != 1)
        {
            anim.Play("Run");
            return;
        }
        if (moveIslandStatus == 0)
        {
            anim.Play("Idlle");
        }
        else
        {
            anim.SetFloat("speedRun", 1 + (speed - 0.4f) / 3f);
            anim.Play("Walking");
        }
    }
    protected override void GioiHanDiChuyen()
    {
        float maxX = transform.parent.transform.position.x + 7;
        float minX = transform.parent.transform.position.x - 7;
        float minY = transform.parent.transform.position.y - 3;
        float maxY = transform.parent.transform.position.y + 4;
        if (transform.position.x >= maxX)
        {
             transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
             RanDiChuyen();
        }
        if (transform.position.x <= minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
              RanDiChuyen();

        }
        if (transform.position.y >= maxY)
        {
         
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
                RanDiChuyen();
        }
        if (transform.position.y <= minY)
        {
         
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
              RanDiChuyen();
        }
    }
    protected override void ScanFood()
    {
     
    }
    protected override void scale(float x)
    {
            Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * x;
        transform.localScale = scale;
    }
    protected override void RanDiChuyen()
    {
        byte randichuyen = (byte)Random.Range(1, 16);
        if(nhan != 1)randichuyen = (byte)Random.Range(1, 12);
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
    public void Click()
    {
        nhan = 4f + solandap / 2;
        speed = nhan;
       
        anim.Play("Run");
        solandap += 1;
         Debug.Log("Đập " + solandap);
    }
}