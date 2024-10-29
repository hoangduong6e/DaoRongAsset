using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KimQuay : MonoBehaviour
{
    // Start is called before the first frame update
    public bool quay = false;
    public Transform tf;
    private float time = 0, speed = 200;
    public string nameQua;
    public static KimQuay ins;
    public Sprite mayTrang, MayVang;
    void Start()
    {
        ins = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (quay)
        {
            time += Time.deltaTime;
            tf.transform.eulerAngles -= Vector3.forward * Time.deltaTime * speed;
            if (time <= 1.5f) speed += 2;
            else if (speed >= 100) speed -= 1f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.name == "AllMay" && quay)
        {
            Image img = collision.gameObject.GetComponent<Image>();
            img.sprite = MayVang;img.SetNativeSize();
            if (collision.name == "item" + nameQua && time >= 1.5f)
            {
                quay = false;
                speed = 200;
                time = 0;
                StartCoroutine(Event7VienNgocRong2023.inss.DelayHienQua());
            }
          //  debug.Log(collision.name);
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.name == "AllMay")
        {
            Image img = collision.gameObject.GetComponent<Image>();
            img.sprite = mayTrang; img.SetNativeSize();
        }
    }
}
