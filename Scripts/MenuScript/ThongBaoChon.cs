using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThongBaoChon : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnChon;
    public Text txtThongBao;
    public Toggle toggle;

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
    public void deleteclick()
    {
        AllMenu.ins.DestroyMenu("MenuXacNhan");
        toggle.gameObject.SetActive(false);
      //  btnChon.onClick.RemoveAllListeners();
      // transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
      // gameObject.SetActive(false);
    }
}
public static class ExThongBaoChon
{
    public static ThongBaoChon OpenMenu(Transform parent = default, int index = 0)
    {
        if (parent == default)
        {
            parent = CrGame.ins.trencung;
        }
        ThongBaoChon tbc = AllMenu.ins.GetCreateMenu("MenuXacNhan", parent.gameObject, true, index).GetComponent<ThongBaoChon>();
        return tbc;
    }


    public static ThongBaoChon SetTxt(this ThongBaoChon tbc, string txt)
    {
        tbc.txtThongBao.text = txt;
        return tbc;
    }

    public static ThongBaoChon OnToggle(this ThongBaoChon tbc,string txt, Action<bool> actionIsOn = null)
    {
        tbc.toggle.gameObject.SetActive(true);
        tbc.toggle.isOn = false;
        tbc.toggle.onValueChanged.RemoveAllListeners();
        if (actionIsOn != null) tbc.toggle.onValueChanged.AddListener(actionIsOn.Invoke);
        tbc.toggle.transform.Find("txt").GetComponent<Text>().text = txt;
        return tbc;
    }
}