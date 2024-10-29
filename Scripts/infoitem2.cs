using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class infoitem2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    short id;
    private RectTransform buttonRectTransform; // Tham chiếu đến RectTransform của button
    private bool isHolding = false;
    private float holdTimer = 0f;
    private bool isDragging = false; // Cờ để xác định xem có đang kéo hay không

    private void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
    }

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
        // Kiểm tra nếu người dùng thực sự dí vào button và không đang kéo
        if (!isDragging && RectTransformUtility.RectangleContainsScreenPoint(buttonRectTransform, data.position, data.pressEventCamera))
        {
            // Bắt đầu quá trình đếm thời gian giữ nút
            isHolding = true;
            holdTimer = 0f; // Reset bộ đếm
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        // Reset lại trạng thái khi thả nút
        isHolding = false;
        holdTimer = 0f;
        isDragging = false; // Reset trạng thái kéo

        // Tắt thông báo nhanh khi thả nút
        CrGame.ins.OffThongBaoNhanh(id);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Đặt cờ là đang kéo khi người dùng kéo button
        isDragging = true;
        isHolding = false; // Ngừng việc đếm giữ nút khi kéo bắt đầu
    }

    // Hàm được gọi khi giữ nút đủ thời gian
    private void OnHoldComplete()
    {
        string nameitem = gameObject.name;
        StartCoroutine(Load());

        IEnumerator Load()
        {
            UnityWebRequest www = new UnityWebRequest(CrGame.ins.ServerName + "XemShop/taikhoan/" + LoginFacebook.ins.id + "/nameitem/" + nameitem);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                // Xử lý kết quả từ server
                JSONNode json = JSON.Parse(www.downloadHandler.text);
                id = (short)json["thongtin"].AsString.Length;
                CrGame.ins.OnThongBaoNhanh(json["thongtin"].AsString, 2, false);
            }
        }
    }
}
