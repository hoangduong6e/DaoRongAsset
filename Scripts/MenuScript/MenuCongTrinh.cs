using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCongTrinh : MonoBehaviour
{
    // Start is called before the first frame update
    CrGame crGame;public GameObject contentShopct;
    void Start()
    {
        crGame = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CrGame>();
    }

    public void chonMua(string name)
    {
        crGame.ChonCongTrinhNang(name);
    }
}
