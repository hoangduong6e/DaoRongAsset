using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNhatKyVuonHoa : MonoBehaviour
{
    public GameObject content, Object;
    public Sprite spriteMatTrang;
    public void ParseData(JSONNode json)
    {
       
        for (int i = json["nhatki"].Count - 1; i >= 0; i--)//ngay
        {
            int ngay = i + 1;
           
            for (int j = 0; j < json["nhatki"]["Ngay" + ngay].Count; j++)//dot
            {
                for (int l = 0; l < json["nhatki"]["Ngay" + ngay][j].Count; l++)//vuonhoa
                {
                    //debug.Log("Ngày " + i + "");
                    int dot = j + 1;
                    GameObject instan = Instantiate(Object, transform.position, Quaternion.identity);
                    instan.transform.SetParent(content.transform,false);
                    string quanhanduoc = "Số lượng quà nhận được";
                    if (l == 1)
                    {
                        instan.transform.GetChild(1).GetComponent<Image>().sprite = spriteMatTrang;
                        instan.transform.GetChild(2).GetComponent<Text>().text = "";
                        quanhanduoc = "Số lần thắp đèn";
                    } 
                    else instan.transform.GetChild(2).GetComponent<Text>().text = "<color=orange>Ngày " + ngay + "</color>- Đợt " + dot;

                    instan.transform.GetChild(3).GetComponent<Text>().text = 
                        "Số hoa đã trồng: <color=lime>" + json["nhatki"]["Ngay" + ngay][j][l]["SoHoaDaTrong"].AsString + "</color>\n" +
                        "Số lượng phân bón đã sử dụng: <color=lime>" + json["nhatki"]["Ngay" + ngay][j][0]["SoPhanBonDaDung"].AsString + "</color>\n" +
                        quanhanduoc + ": <color=magenta>" + json["nhatki"]["Ngay" + ngay][j][l]["SoQuaNhanDuoc"].AsString + "</color>\n" +
                        "Tổng EXP đã thu hoạch: <color=cyan>" + json["nhatki"]["Ngay" + ngay][j][l]["TongExpDaThuHoach"].AsString + "</color>\n";
                    instan.SetActive(true);
                    instan.name = "Ngay" + ngay + "-Dot" + dot + "-VuonHoa" + l;
                }
            }
        }
        gameObject.transform.GetChild(0).transform.GetChild(4).GetComponent<Text>().text = "Tổng EXP đã thu hoạch trong mùa: <color=cyan>" + json["nhatki"]["tongexpdathuhoach"].AsString + "</color>";
        gameObject.SetActive(true);
    }    
    public void Exit()
    {
        EventManager.ins.DestroyMenu("MenuNhatKi");
    }
}
