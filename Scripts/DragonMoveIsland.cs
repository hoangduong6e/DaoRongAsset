using UnityEngine;

public class DragonMoveIsland : DragonIslandController
{
    //  public MoveIslandStatus moveIslandSt;
    // Start is called before the first frame update
    void Awake()
    {
        //moveIslandStatus = MoveIslandStatus.Right;
        //IDragonMoveIsland i = GetComponent<IDragonMoveIsland>() ;
        //debug.Log("Status = " + i.moveIslandStatus);

        // this.AddComponent<IDragonFly>();
        //IDragonMoveIsland abc = transform.parent.GetComponent<IDragonMoveIsland>();
        //if(abc != null)
        //{
        //    debug.Log("Có idragonMoveIsland");
        //}
        //else
        //{
        //    debug.Log("Không có IdragobMoveisland");
        //}
        Awakee();
    }
    //private void OnMouseDrag()
    //{
    //    StartDrag();
    //}
    //private void OnMouseUp()
    //{
    //    StopDrag();
    //}
    private void Start()
    {
        Startt();
        //;
    }
    private void OnEnable()
    {
        // UpdateAnimator();
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
        float maxX = transform.parent.transform.position.x + 8;
        float minX = transform.parent.transform.position.x - 8;
        float minY = transform.parent.transform.position.y - 4;
        float maxY = transform.parent.transform.position.y + 5;
        if (transform.position.x >= maxX) transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        if (transform.position.x <= minX) transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        if (transform.position.y >= maxY) transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        if (transform.position.y <= minY) transform.position = new Vector3(transform.position.x, minY, transform.position.z);
    }
    protected override void ScanFood()
    {
        if (doi && !dabong)
        {
            if (DragonIslandManager.DungThucAn.transform.childCount > 0)
            {
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
                        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.transform.position.z), new Vector3(child.transform.position.x, child.transform.position.y,transform.position.z), 1.2f * Time.deltaTime);
                    }
                }
            }
        }
    }
}
public enum MoveIslandStatus
{
    Idle,
    Left,
    Right,
    Down,
    Up,
    DownLeft,
    DownRight,
    UpLeft,
    UpRight,
}

