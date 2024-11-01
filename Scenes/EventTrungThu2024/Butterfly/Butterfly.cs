using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum _mauBuom
    {
        Blue,
        Pink,
        Red,
        Yellow,
        Purple
    }
    ;
public class Butterfly : MonoBehaviour
{
  

    Material _material;
    public GameObject butterfly;
    
    void Start()
    {
       
        //target = CrGame.ins.transform;
        // Đặt vị trí mục tiêu ban đầu
        float randomx = Random.Range(7,8);
        float randomy = Random.Range(6,7);
        if(Random.Range(1,100) > 50)
        {
            randomx = Random.Range(-7,-8);
            randomy = Random.Range(-6,-7);
        }
        transform.position = new Vector3(randomx,randomy);
        SetNewTargetPosition();
    }
    public _mauBuom setMauBuom { set
        {
           SpriteRenderer _butterfly = butterfly.GetComponent<SpriteRenderer>();
        _material = _butterfly.material;
         switch(value)
         {
            case _mauBuom.Blue:
                _material.SetFloat("_Hue",0f);
                break;
            case _mauBuom.Purple:
                _material.SetFloat("_Hue", 74f);
                break;
            case _mauBuom.Pink:
                _material.SetFloat("_Hue",115f);
                break;
            case _mauBuom.Red:
                _material.SetFloat("_Hue",156f);
                break;
            case _mauBuom.Yellow:
                _material.SetFloat("_Hue", 213f);
                break;
         }
        }
    }


    public float speed = 2f;  // Tốc độ di chuyển của bướm

    private Vector3 targetPosition; // Vị trí mục tiêu tiếp theo

    private bool sleep = false;
    float timesleep,maxtimesleep;

    void Update()
    {
        if(!sleep)
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed*Time.deltaTime);
            if(transform.position == targetPosition)
            {
               SetNewTargetPosition();
               if(Random.Range(0,100)>90)
               {
                  sleep = true;
                  timesleep = 0;
                  maxtimesleep = Random.Range(1,3);
               }
            }
        }
        else
        {
            timesleep += Time.deltaTime;
            if(timesleep >= maxtimesleep)
            {
                sleep = false;
            }
        }
        
    }

    void SetNewTargetPosition()
    {
        // Chọn vị trí ngẫu nhiên với giới hạn x và y từ -3.5 đến 3.5
        float randomX = Random.Range(MenuEventTrungThu2024.inss.traicay.transform.position.x, MenuEventTrungThu2024.inss.phaicay.transform.position.x);
        float randomY = Random.Range(MenuEventTrungThu2024.inss.duoicay.transform.position.y, MenuEventTrungThu2024.inss.trencay.transform.position.y);
        speed += Random.Range(-0.5f,0.5f);
        if(speed < 1.3f) speed = 1.3f;
        else if(speed > 1.6f) speed = 1.6f;
        // Đặt vị trí mục tiêu mới cho bướm với trục z luôn bằng 0
        targetPosition = new Vector3(randomX, randomY, 0);
    }
}
