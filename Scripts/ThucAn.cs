using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThucAn : MonoBehaviour
{
    public GameObject DauV;ThuyenThucAn thuyen;
    public Sprite sPrite;public GameObject ObjThucAn;
    public string NameFood;
    public int levelThucAn;
    // Start is called before the first frame update
    void Start()
    {
        DauV = gameObject.transform.GetChild(2).gameObject;
        thuyen = NetworkManager.ins.GetComponent<ThuyenThucAn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Use()
    {
        if (thuyen.ThaThucAn)
        {
            thuyen.ThucAnTha = gameObject;
            Drop();
        }
    }
    public void XemThucAn()
    {

    }    
    public void XemNangCapThucAn()
    {
        //  crgame.StartCoroutine(crgame.XemThucAn(NameFood, levelThucAn, gameObject));
        thuyen.NameThucAnChon = NameFood;
        NetworkManager.ins.socket.Emit("XemNangCapThucAn",JSONObject.CreateStringObject("xem-"+NameFood));
    }
    public void Drop()
    {
        thuyen.thucAnobj = ObjThucAn;
        //   NetworkManager.ins.socket.Emit("thathucan", JSONObject.CreateStringObject(NameFood));
        DragonIslandManager.DropThucAn(NameFood);
        DauV.SetActive(true);
    }
    public void DropBinhTieuHoa()
    {
        if (thuyen.ThaThucAn)
        {
            debug.Log("tha binh tieu hoa");
            thuyen.ThucAnTha = gameObject;
            thuyen.thucAnobj = ObjThucAn;
          //  NetworkManager.ins.socket.Emit("thathucan", JSONObject.CreateStringObject(NameFood));
            DauV.SetActive(true);
            DragonIslandManager.DropThucAn(NameFood);
        }    
    }
}
