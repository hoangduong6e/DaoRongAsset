using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThongBaoChon : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnChon;
    public Text txtThongBao;

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }
    public void deleteclick()
    {
        AllMenu.ins.DestroyMenu("MenuXacNhan");
      //  btnChon.onClick.RemoveAllListeners();
       // transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
       // gameObject.SetActive(false);
    }
}
