using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "imgThanhGo")
        {
            if(gameObject.name == "BomDen")
            {
                MiniGameTrungThu.ins.SetDiemThanhGo = 0;
                GameObject Khoi = Instantiate(MiniGameTrungThu.LoadObjectResource("Khoi"), transform.position, Quaternion.identity);
                Khoi.transform.position = transform.position;
                Khoi.SetActive(true);
            }

            else if (gameObject.name == "BomDo")
            {
                MiniGameTrungThu.ins.Hp = MiniGameTrungThu.ins.Hp - 3;
                GameObject Khoi = Instantiate(MiniGameTrungThu.LoadObjectResource("Khoi"), transform.position, Quaternion.identity);
                Khoi.transform.position = transform.position;
                Khoi.SetActive(true);
            }

            else if(gameObject.name == "itemRoi")
            {
                Vector3 newvec = transform.position;
                MiniGameTrungThu.ins.OnBuiChamGo(newvec);
                if (MiniGameTrungThu.ins.GSNgayDem == "Ngay")
                {
                    string nameitem = gameObject.GetComponent<SpriteRenderer>().sprite.name;
                    if (nameitem == "Vang")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(5000,20000),transform);
                    }
                    else if(nameitem == "Exp")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(5000, 10000), transform);
                    }
                    else if (nameitem == "HuyenTinh")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(50, 100), transform);
                    }
                    else if (nameitem == "LongDenKeoQuan")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, 1, transform);

                        MiniGameTrungThu.ins.SetLongDen();
                    }
                    //    debug.Log("Nhat item " + gameObject.GetComponent<Sprite>().name);
                }    
                else
                {
                    string nameitem = gameObject.GetComponent<SpriteRenderer>().sprite.name;
                    if (nameitem == "Vang")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(200000,500000), transform);
                    }
                    else if (nameitem == "Exp")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(15000, 20000), transform);
                    }
                    else if (nameitem == "HuyenTinh")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, Random.Range(50, 100), transform);
                    }
                    else if (nameitem == "LongDenKeoQuan")
                    {
                        MiniGameTrungThu.ins.AddItemRoi(nameitem, 1, transform);
                        MiniGameTrungThu.ins.SetLongDen();
                    }
                  
                }
            
              
            }
         
            Destroy(gameObject);
        }
        else if(collision.name == "roi")
        {
            Destroy(gameObject,2);
        }
    }
}
