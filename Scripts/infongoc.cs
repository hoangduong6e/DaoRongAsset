using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class infongoc : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    short id;

    private bool isHolding = false;
    private float holdTimer = 0f;
    private bool isDragging = false; // Cờ để xác định xem có đang kéo hay không


    void Update()
    {
        // Nếu nút đang được giữ và không đang kéo, tăng dần bộ đếm thời gian
        if (isHolding && !isDragging)
        {
            holdTimer += Time.deltaTime;

            // Nếu giữ nút đủ 0.2 giây, kích hoạt sự kiện OnPointerDown
            if (holdTimer >= 0.2f)
            {
                isHolding = false; // Dừng việc đếm thời gian
                OnHoldComplete();  // Gọi hàm xử lý logic khi giữ nút đủ thời gian
            }
        }
    }


    public void OnPointerDown(PointerEventData data)
    {
        // Kiểm tra nếu không đang kéo thì mới bắt đầu đếm giữ nút
        if (!isDragging)
        {
            isHolding = true;
            holdTimer = 0f; // Reset bộ đếm
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        // Reset các biến khi thả nút
        isHolding = false;
        holdTimer = 0f;
        isDragging = false; // Reset trạng thái kéo

        CrGame.ins.OffThongBaoNhanh(id);
        if (Friend.ins.QuaNha == false)
        {

        }
        else
        {

            GameObject menutangqua = AllMenu.ins.GetCreateMenu("MenuTangQua", null, false);
            Image imgItemTang = menutangqua.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            imgItemTang.sprite = GetComponent<Image>().sprite;
            Friend.ins.MaxSoluong = int.Parse(transform.GetChild(0).GetComponent<Text>().text);
            Friend.ins.XemTangQua("item*" + gameObject.transform.parent.name);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nếu có thao tác kéo, cờ isDragging được bật
        isDragging = true;
        isHolding = false; // Ngừng việc đếm giữ nút khi bắt đầu kéo
    }



    private void OnHoldComplete()
    {
        string nameitem = gameObject.transform.parent.name;

        JSONClass datasend = new JSONClass();
        datasend["class"] = "Main";
        datasend["method"] = "XemShop";
        datasend["data"]["nameitem"] = nameitem;
        NetworkManager.ins.SendServer(datasend.ToString(), Ok, true);
        void Ok(JSONNode json)
        {
            if (json["status"].AsString == "0")
            {
                AllMenu.ins.OpenCreateMenu("infoitem", GameObject.FindGameObjectWithTag("trencung"));
                CrGame.ins.OnThongBaoNhanh(json["thongtin"].Value, 2, false);
                id = (short)json["thongtin"].AsString.Length;
            }
            else CrGame.ins.OnThongBaoNhanh(json["message"].AsString);
        }
    }
}
