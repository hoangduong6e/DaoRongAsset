using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dorodichuyen : MonoBehaviour
{
    public float speed = 0.3f;
    bool left, right, up, down; public bool dichuyen = true;
    bool boi = false, roi = false;
    public float timeconlai = 0, maxtimeconlai; Rigidbody2D rigid;
    GameObject hieuung, bongrong; Vector3 vitribong; SpriteRenderer SpBong;
    public float time, maxtime;CrGame crgame; Animator anim; Transform chayvedao; float scaledra;
    Vector3 Scale;
    // Start is called before the first frame update
    void Start()
    {
        Scale = transform.localScale;
        scaledra = transform.localScale.x;
        if (GetComponent<Animator>()) anim = GetComponent<Animator>();
        Randichuyen(); rigid = gameObject.GetComponent<Rigidbody2D>();
        bongrong = transform.GetChild(0).gameObject;
        SpBong = bongrong.GetComponent<SpriteRenderer>();
        vitribong.y = gameObject.transform.position.y - bongrong.transform.position.y;
        transform.position = crgame.transform.position; //new Vector3();
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
            Randichuyen(); ;
        }
        if (dichuyen)
        {
            if(anim != null) anim.SetBool("dichuyen", true);
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
            if (anim != null) anim.SetBool("dichuyen", false);
        }
        //debug.Log(dichuyen);
    }
    // Update is called once per frame
    void Update()
    {
        if (timeconlai > 0)
        {
            timeconlai -= Time.deltaTime;
        }
        DragonMove();
        if (boi)
        {
            dichuyen = false;
            //   var step = 5 * Time.deltaTime;
            if (transform.position == transform.parent.transform.GetChild(1).transform.position)
            {
                SpBong.enabled = true;
                Destroy(hieuung);
                boi = false; dichuyen = true;
                up = true;
            }
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, chayvedao.transform.position.z), chayvedao.transform.position, 2 * Time.deltaTime);
        }
    }
    void Randichuyen()
    {
        if (boi == false)
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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        maxtime = Random.Range(3, 5);
        time = 0;
        if (collision.CompareTag("duoinuoctrai") || collision.CompareTag("duoinuocduoi") || collision.CompareTag("duoinuocphai"))
        {
            if (boi == false)
            {
                AudioManager.PlaySound("roixuongnuoc");
                hieuung = Instantiate(Inventory.ins.GetEffect("HieuUngBoi"), new Vector3(transform.position.x, transform.position.y - 0.1f), Quaternion.identity) as GameObject;
                hieuung.transform.SetParent(gameObject.transform);
                SpBong.enabled = false;
                if (collision.CompareTag("duoinuoctrai"))
                {
                    chayvedao = crgame.AllDao.transform.GetChild(crgame.DangODao).Find("RongDao").transform.parent.transform.GetChild(0).transform;
                    scale(-scaledra);
                }
                if (collision.CompareTag("duoinuocduoi"))
                {
                    chayvedao = crgame.AllDao.transform.GetChild(crgame.DangODao).Find("RongDao").transform.parent.transform.GetChild(1).transform;
                }
                if (collision.CompareTag("duoinuocphai"))
                {
                    chayvedao = crgame.AllDao.transform.GetChild(crgame.DangODao).Find("RongDao").transform.parent.transform.GetChild(2).transform;
                    scale(scaledra);
                }
                boi = true;
                return;
            }
        }
        if (collision.name == "bong" + gameObject.name)
        {
            if (boi == false && roi)
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
    }
}
