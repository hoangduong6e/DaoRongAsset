using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuaBong : MonoBehaviour
{
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        debug.Log(transform.position);
    }

    public void BongLan()
    {
        Status[] arrRandom = new Status[] { };
        if (transform.position.y <= -2 && transform.position.x <= 0)// nếu đang ở dưới góc bên trái
        {
            arrRandom = new Status[] {Status.Up,Status.UpRight};
        }
        else if (transform.position.y <= 2 && transform.position.x > 0)// nếu đang ở dưới góc bên phải
        {
            arrRandom = new Status[] { Status.Up, Status.UpLeft};
        }
        else if (transform.position.y <= 2 && transform.position.x > 0)// nếu đang ở dưới góc bên phải
        {
            arrRandom = new Status[] { Status.Up, Status.UpLeft };
        }
        status = arrRandom[Random.Range(0, arrRandom.Length - 1)];
    }    
}
