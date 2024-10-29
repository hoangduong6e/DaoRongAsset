using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuocTrangTri : MonoBehaviour
{
    Vector3 mousePosition; Vector3 Scale;CrGame crGame;int indexobjectcanxoa = -1;
    NetworkManager net;bool Enable = true;
    Collider2D col;
    // Start is called before the first frame update
    private void Awake()
    {
       // debug.Log(transform.GetSiblingIndex());
        mousePosition = transform.parent.transform.position;
        crGame = GameObject.Find("Main Camera").GetComponent<CrGame>();
        net = GameObject.Find("Main Camera").GetComponent<NetworkManager>();
    }
    void Start()
    {
        transform.position = new Vector3();
        Scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mousePosition.x -= 1.5f;
        mousePosition.z = 0;
        transform.Translate(mousePosition);
    }
    private void OnEnable()
    {
        Enable = true;
        Scale.x = 2.5f; Scale.y = 2.5f;
        transform.localScale = Scale;
    }
    private void OnDisable()
    {
        Enable = false;
        if (indexobjectcanxoa != -1)
        {
            //Destroy(GameObject.Find("ObjectTranngTri-" + crGame.DangODao).transform.GetChild(indexobjectcanxoa).gameObject);
            int index = indexobjectcanxoa;
            net.socket.Emit("DeleteTrangtri",JSONObject.CreateStringObject(index.ToString()));
        }
        indexobjectcanxoa = -1;
        transform.position = transform.parent.transform.position;
        Scale.x = 1.5f; Scale.y = 1.5f;
        transform.localScale = Scale;
        if(col != null)
        {
            Color color = new Color(1, 1, 1, 1);
            if (col.GetComponent<SpriteRenderer>())
            {
                col.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                color = col.GetComponent<Image>().color = color;
            }
        }
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.transform.parent.gameObject.name == "ObjectTrangTri")
        {
            col = collision;
            Color color = new Color(0.4811321f, 0.4788626f, 0.4788626f, 1);
            if (collision.GetComponent<SpriteRenderer>())
            {
                collision.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                if(collision.GetComponent<Image>()) color = collision.GetComponent<Image>().color = color;
            }
            indexobjectcanxoa = collision.transform.parent.gameObject.transform.GetSiblingIndex();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.transform.parent.gameObject.name == "ObjectTrangTri")
        {
            if (Enable)
            {
                Color color = new Color(1, 1, 1, 1);
                if (collision.GetComponent<SpriteRenderer>())
                {
                    collision.GetComponent<SpriteRenderer>().color = color;
                }
                else
                {
                    color = collision.GetComponent<Image>().color = color;
                }
                indexobjectcanxoa = -1;
            }
        }
    }
}
