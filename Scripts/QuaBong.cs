using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class QuaBong : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform tfQuaBong;

    public bool isLan = false;
    private float speed = 0.05f;
    enum Status
    {
        Up, Down, Left, Right,
        UpLeft, DownLeft,
        UpRight, DownRight
    }
    Status status;

    Status[] allStatus = (Status[])Enum.GetValues(typeof(Status));
    void Start()
    {
        
    }
    private void RotationBong(int i = 1)
    {
        tfQuaBong.transform.Rotate(new Vector3(0,0,500*i) * Time.deltaTime);
    }
    // Update is called once per frame

    int index = 3;

    void RongDanhLanLuot()
    {
        Transform RongDao = transform.parent.transform.parent.transform.Find("RongDao");
        if (index <= RongDao.transform.childCount)
        {
            RongDao.transform.GetChild(index).GetComponent<DragonIslandController>().StartDaBong();
            index++;
        }
        else index = 3;
    }
    void Update()
    {
        //    debug.Log(transform.position);
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    RongDanhLanLuot();
        //}
        if (isLan)
        {
            if (status == Status.Up)
            {
                transform.position += Vector3.up * speed;
                RotationBong(-1);
            }
            else if (status == Status.Down)
            {
                transform.position += Vector3.down * speed;
                RotationBong(-1);
            }
            else if (status == Status.Left)
            {
                transform.position += Vector3.left * speed;
                RotationBong();
            }
            else if (status == Status.Right)
            {
                transform.position += Vector3.right * speed;
                RotationBong(-1);
            }
            else if (status == Status.UpLeft)
            {
                transform.position += Vector3.left * speed;
                transform.position += Vector3.up * speed;
                RotationBong();
            }
            else if (status == Status.UpRight)
            {
                transform.position += Vector3.right * speed;
                transform.position += Vector3.up * speed;
                RotationBong(-1);
            }
            else if (status == Status.DownLeft)
            {
                transform.position += Vector3.left * speed;
                transform.position += Vector3.down * speed;
                RotationBong();
            }
            else if (status == Status.DownRight)
            {
                transform.position += Vector3.right * speed;
                transform.position += Vector3.down * speed;
                RotationBong(-1);
            }
        }    
    }

    public void BongLan()
    {
        Status[] arrRandom = new Status[] { };
        if (transform.position.y <= -1.3f && transform.position.x <= 0)// nếu đang ở dưới góc bên trái
        {
            debug.Log("góc dưới trái");
            arrRandom = new Status[] {Status.Up,Status.UpRight};
        }
        else if (transform.position.y <= -1.3f && transform.position.x > 0)// nếu đang ở dưới góc bên phải
        {
            debug.Log("góc dưới phải");
            arrRandom = new Status[] { Status.Up, Status.UpLeft};
        }
        else if (transform.position.y > 1.3f && transform.position.x <= 0)// nếu đang ở trên góc bên trái
        {
            debug.Log("góc trên trái");
            arrRandom = new Status[] { Status.Down, Status.DownRight };
        }
        else if (transform.position.y > 1.3f && transform.position.x > 0)// nếu đang ở trên góc bên phải
        {
            debug.Log("góc trên phải");
            arrRandom = new Status[] { Status.Down, Status.DownLeft };
        }
        else if(transform.position.x < -1) // nếu đang ở bên trái
        {
            arrRandom = new Status[] { Status.Right };
        }
        else if (transform.position.x > 1)// nếu đang ở bên phải
        {
            arrRandom = new Status[] { Status.Left };
        }
        else if (transform.position.y < -1) // nếu đang ở bên dưới
        {
            arrRandom = new Status[] { Status.Up };
        }
        else if (transform.position.y > 1)// nếu đang ở bên trên
        {
            arrRandom = new Status[] { Status.Down };
        }
        else
        {
            debug.Log("gocs");
            arrRandom = allStatus;
        }    
      // debug.Log("length: " + arrRandom.Length);
        status = arrRandom[Random.Range(0, arrRandom.Length)];
        //  debug.Log("Bóng lăn: " + status.ToString());
        speed = Random.Range(0.04f,0.07f);
          isLan = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.name.Contains("hangraodao"))
        {
            isLan = false;
            RongDanhLanLuot();
        }    
    }
}
