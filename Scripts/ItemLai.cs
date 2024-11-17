using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemLai : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txtSao;
    public string HeRong,Gen;
    public string idronglai,NameObjectRong;
    public int tienhoa;
    public void ClickDeLai()
    {
        LaiRong lai = AllMenu.ins.menu["MenuLaiRong"].GetComponent<LaiRong>();
      
        for (int i = 0; i < lai.Chonlai.Length; i++)
        {
            GameObject olai = lai.transform.GetChild(0).transform.GetChild(3).transform.GetChild(2).transform.GetChild(i).gameObject;
            if (lai.Chonlai[i] == false)
            {
               // lai.spritelai[i].sprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
                lai.Chonlai[i] = true;
                lai.txtDuarong[i].SetActive(false);
                lai.Rongchon[i] = gameObject;
                lai.HeRongLai[i] = HeRong;
                lai.GenRong[i] = Gen;
                lai.idronglai[i] = idronglai;
                lai.SaoRongBoMe[i] = txtSao.text;
                lai.Nameobjectronglai[i] = NameObjectRong;
                //Animator anim = lai.spritelai[i].GetComponent<Animator>();

                //anim.runtimeAnimatorController = Inventory.LoadAnimator(NameObjectRong);
                Transform vitrirong = olai.transform.GetChild(1).transform.GetChild(0).transform;
                float scaleX = 1;
                if (i == 0) scaleX = -1;
                AllMenu.ins.LoadRongGiaoDien(NameObjectRong + tienhoa, vitrirong, scaleX);
                // anim.SetInteger("TienHoa", tienhoa);
                //foreach (Transform child in ChuaRong.transform)
                //{
                //    if (child.name == NameObjectRong)
                //    {
                //        anim.runtimeAnimatorController = child.gameObject.GetComponent<Animator>().runtimeAnimatorController;
                //        anim.SetInteger("TienHoa", tienhoa);
                //        break;
                //    }
                //}

                gameObject.SetActive(false);
                if (lai.Chonlai[0] == true && lai.Chonlai[1] == true)
                {
                    //net.socket.Emit("Xemronglai",JSONObject.CreateStringObject(lai.HeRongLai[0] + "-" + lai.GenRong[0] + "+" + lai.HeRongLai[1] + "-" + lai.GenRong[1]));
                    lai.XemRonglai(lai.HeRongLai[0] + "-" + lai.GenRong[0] + "+" + lai.HeRongLai[1] + "-" + lai.GenRong[1] + "+" + lai.SaoRongBoMe[0] + "+" + lai.SaoRongBoMe[1] + "+" + lai.Nameobjectronglai[0] + "+" + lai.Nameobjectronglai[1]);
                    //lai.btnlai.interactable = true;
                }
               // lai.spritelai[i].enabled = true;
                break;
            }
        }
    }
}

