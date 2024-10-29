using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BanRong : MonoBehaviour
{
    public Image imgRongBan;
    public Text txtGiaBan,TxtTenRong;Inventory inventory;
    public string idrongban;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>();
    }
    public void Ban()
    {
        inventory.BanRong(idrongban);
    }
}
