using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class FLowersToggle : MonoBehaviour
{
    private MenuEventTraoHongDoatLong ev;
    public List<GameObject> Flowers = new List<GameObject>();
    Animator anim;

    Vector2 Vec;
    private float Xmin,Xmax,speed = 0.005f;
    public float Min = 1,Max = 1;
    public bool left = true, thuhoach = false, walk = true;
    private void Start()
    {
        Flowers.Add(transform.Find("GameObject/Flowers_1").gameObject);
        Flowers.Add(transform.Find("GameObject/Flowers_2").gameObject);
        ev = EventManager.ins.GetComponent<MenuEventTraoHongDoatLong>();
        anim = GetComponent<Animator>();
        Vec = transform.position;
        Xmin = Vec.x - Min;
        Xmax = Vec.x + Max;
        speed += Random.Range(0.001f,0.004f);
    }

    public void ToggleOnFlowers(int index)
    {
        Flowers[index].SetActive(true);   
    }

    public void ToggleOffFlowers()
    {
        foreach (GameObject flower in Flowers)
        {
            flower.SetActive(false);
        }
    }
    private void Scale(int x = 1)
    {
        Vector3 Scale = transform.localScale;
        Scale.x = Mathf.Abs(Scale.x) * x;
        transform.localScale = Scale;
    }
    private void Update()
    {
        if(ev.socNongDan && !thuhoach && walk)
        {
            anim.Play("Walk");
            if(left)
            {
                Scale();
                transform.position += Vector3.left * speed;
                if(transform.position.x <= Xmin)
                {
                    left = false;
                }
            }
            else
            {
                Scale(-1);
                transform.position += Vector3.right * speed;
                if(transform.position.x >= Xmax)
                {
                    left = true;
                }
            }
        }
        else if (thuhoach)
        {
            anim.Play("Run");
            if (left)
            {
                Scale();
                transform.position += Vector3.left * speed;
                if (transform.position.x <= Xmin)
                {
                    left = false;
                    thuhoach = false;
                    anim.Play("HaiHoa");
                    // ThuHoachOk();
                }
            }
            else
            {
                Scale(-1);
                transform.position += Vector3.right * speed;
                if (transform.position.x >= Xmax)
                {
                    left = true;
                    thuhoach = false;
                    anim.Play("HaiHoa");
                    //    ThuHoachOk();
                }
            }
        }
    }

    public void ThuHoach()
    {
        if (thuhoach || !walk) return;
        thuhoach = true;
        walk = false;
        speed += 0.01f;
    }
    private void ThuHoachOk()
    {
       //thuhoach = false;
        speed -= 0.01f;
        //  anim.Play("HaiHoa");
        walk = true;
        //EventManager.StartDelay2(() => {
        //    walk = true;
        //}, 1f);
    }
}
