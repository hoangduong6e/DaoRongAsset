using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class infoitem3 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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

        // Tắt thông báo nhanh khi thả nút
        CrGame.ins.OffThongBaoNhanh(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nếu có thao tác kéo, cờ isDragging được bật
        isDragging = true;
        isHolding = false; // Ngừng việc đếm giữ nút khi bắt đầu kéo
    }

    // Hàm được gọi khi giữ nút đủ thời gian
    public string thongtin;
    private void OnHoldComplete()
    {
        //string nameitem = gameObject.name.Substring(4);
        //StartCoroutine(Load());

        //IEnumerator Load()
        //{
        //    UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemShop/taikhoan/" + LoginFacebook.ins.id + "/nameitem/" + nameitem);
        //    www.downloadHandler = new DownloadHandlerBuffer();
        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        debug.Log(www.error);
        //    }
        //    else
        //    {
        //        // Xử lý kết quả từ server
        //        JSONNode json = JSON.Parse(www.downloadHandler.text);
        //        id = (short)json["thongtin"].AsString.Length;
        //        CrGame.ins.OnThongBaoNhanh(json["thongtin"].AsString, 2, false);
        //    }
        //}
        id = (short)thongtin.Length;
        CrGame.ins.OnThongBaoNhanh(thongtin, 2, false);
    }
}
