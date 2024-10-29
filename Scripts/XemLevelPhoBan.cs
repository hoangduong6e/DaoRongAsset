using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class XemLevelPhoBan : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler
{
  //  public InfoLevelPhoBan infolevel;public XemPhoBan xemphoban;
    float X, Y;
    void Start()
    {
        X = transform.localScale.x;
        Y = transform.localScale.y;
    }
    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData data)
    {
        float x = transform.localScale.x + 0.001f;
        float y = transform.localScale.y + 0.001f;
      //  transform.localScale = new Vector2(x, y);
        transform.LeanScale(new Vector2(x,y), 0.15f);
    }
    public void OnPointerUp(PointerEventData data)
    {
        transform.LeanScale(new Vector2(X, Y), 0.15f);
    }
    public void OnPointerClick(PointerEventData data)
    {
        InfoLevelPhoBan infolevel = transform.parent.transform.parent.GetComponent<XemPhoBan>().GdMapVienChinh.GetComponent<InfoLevelPhoBan>();
        VienChinh vienchinh = GameObject.FindGameObjectWithTag("vienchinh").GetComponent<VienChinh>();
        vienchinh.nameMapvao = gameObject.name;
        infolevel.StartCoroutine(infolevel.XemLevel(vienchinh.nameMapvao));
    }
}
